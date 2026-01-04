using HandmadeStore.Application.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Application.Interfaces.Service
{
    public interface IAuthService
    {
        Task<AppUserDto> RegisterAsync(RegisterDto registeruser);
        Task<AppUserDto?> LoginAsync(LoginDto? loginDto);
        Task LogoutAsync();

        Task<string?> GenerateTokenAsync(AppUserDto userDto);

    }
}
