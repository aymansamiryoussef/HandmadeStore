    using HandmadeStore.Application;
    using HandmadeStore.Application.Interfaces;
    using HandmadeStore.Application.Interfaces.Repo;
    using HandmadeStore.Application.Interfaces.Service;
    using HandmadeStore.Application.Services;
    using HandmadeStore.Domain.Entities;
    using HandmadeStore.Infrastructure.Data;
    using HandmadeStore.Infrastructure.Data.Identity;
    using HandmadeStore.Infrastructure.Repositories;
    using HandmadeStore.Infrastructure.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using System.Text;

    var builder = WebApplication.CreateBuilder(args);

    // Controllers
    builder.Services.AddControllers();

    // Database
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddDbContext<DbContextForIdentity>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

    builder.Services
        .AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<DbContextForIdentity>()
        .AddDefaultTokenProviders();

    // ==================== Dependency Injection ====================

    // Unit of Work
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    // Generic Repositories
    builder.Services.AddScoped<IGenericRepo<Product>, GenericRepo<Product>>();
    builder.Services.AddScoped<IGenericRepo<Category>, GenericRepo<Category>>();
    builder.Services.AddScoped<IGenericRepo<Cart>, GenericRepo<Cart>>();
    builder.Services.AddScoped<IGenericRepo<Order>, GenericRepo<Order>>();

    // Specific Repositories
    builder.Services.AddScoped<ICartRepo, CartRepo>();
    builder.Services.AddScoped<IOrderRepo, OrderRepo>();

    // Services
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ICartService, CartService>();
    builder.Services.AddScoped<IOrderService, OrderService>();

    // AutoMapper (Clean Architecture way)
    builder.Services.AddAutoMapper(c => c.AddProfile<ProductProfile>());
    // If you have separate profiles, add them like this:
    // builder.Services.AddAutoMapper(typeof(ProductProfile), typeof(OrderMappingProfile));

    // Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // JWT Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),
            ClockSkew = TimeSpan.Zero
        };
    });

    var app = builder.Build();

    // Middleware
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "HandmadeStore API v1");
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();