    using HandmadeStore.Application.DTOs.UserDtos;
    using HandmadeStore.Application.Interfaces.Service;
    using HandmadeStore.Infrastructure.Data.Identity;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    namespace HandmadeStore.Infrastructure.Services
    {
        public class AuthService : IAuthService
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly IConfiguration _configuration;

            public AuthService(
                UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager,
                RoleManager<IdentityRole> roleManager,
                IConfiguration configuration)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _roleManager = roleManager;
                _configuration = configuration;
            }

            // ================= REGISTER =================
            public async Task<AppUserDto> RegisterAsync(RegisterDto registeruser)
            {
                var user = new AppUser
                {
                    UserName = registeruser.Email,
                    Email = registeruser.Email,
                    FirstName = registeruser.FirstName,
                    LastName = registeruser.LastName
                };

                var result = await _userManager.CreateAsync(user, registeruser.Password);

                if (!result.Succeeded)
                    throw new Exception(string.Join(",", result.Errors.Select(e => e.Description)));

                string roleName = "Customer";

                if (!await _roleManager.RoleExistsAsync(roleName))
                    await _roleManager.CreateAsync(new IdentityRole(roleName));

                await _userManager.AddToRoleAsync(user, roleName);

                var roles = await _userManager.GetRolesAsync(user);

                return new AppUserDto
                {
                    UserId = user.Id,
                    Email = user.Email!,
                    Roles = roles
                    , Token = await GenerateTokenAsync(
                        new AppUserDto
                        {
                            UserId = user.Id,
                            Email = user.Email!,
                            Roles = roles
                        }
                    )
                };
            }

            // ================= LOGIN =================
            public async Task<AppUserDto?> LoginAsync(LoginDto? loginDto)
            {
                if (loginDto == null)
                    return null;

                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                    return null;

                var result = await _signInManager.CheckPasswordSignInAsync(
                    user,
                    loginDto.Password,
                    false
                );

                if (!result.Succeeded)
                    return null;

                var roles = await _userManager.GetRolesAsync(user);

                return new AppUserDto
                {
                    UserId = user.Id,
                    Email = user.Email!,
                    Roles = roles
                    , Token = await GenerateTokenAsync(
                        new AppUserDto
                        {
                            UserId = user.Id,
                            Email = user.Email!,
                            Roles = roles
                        }
                    )
                };
            }

            // ================= LOGOUT =================
            public async Task LogoutAsync()
            {
                await _signInManager.SignOutAsync();
            }

            // ================= JWT TOKEN =================
            public async Task<string?> GenerateTokenAsync(AppUserDto userDto)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userDto.UserId),
                    new Claim(ClaimTypes.Email, userDto.Email)
                };

                foreach (var role in userDto.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
                );

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(
                        double.Parse(_configuration["Jwt:DurationInMinutes"]!)
                    ),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
