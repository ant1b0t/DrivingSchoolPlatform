
using DrivingSchoolPlatform.Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;
using System.Text;

namespace DrivingSchoolPlatform.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<DrivingSchoolDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
                options.Password.RequiredLength = 3;   // минимальна€ длина
                options.Password.RequireNonAlphanumeric = false;   //требуютс€ ли не алфавитно-цифровые символы
                options.Password.RequireLowercase = false; // требуютс€ ли символы в нижнем регистре
                options.Password.RequireUppercase = false; // требуютс€ ли символы в верхнем регистре
                options.Password.RequireDigit = true; // требуютс€ ли цифры
            })
                .AddEntityFrameworkStores<DrivingSchoolDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // укзывает, будет ли валидироватьс€ издатель при валидации токена
                        ValidateIssuer = true,
                        // строка, представл€юща€ издател€
                        ValidIssuer = builder.Configuration["JwtIssuer"],
                        // будет ли валидироватьс€ потребитель токена
                        ValidateAudience = true,
                        // установка потребител€ токена
                        ValidAudience = builder.Configuration["JwtAudience"],
                        // будет ли валидироватьс€ врем€ существовани€
                        ValidateLifetime = true,
                        // установка ключа безопасности
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"])),
                        // валидаци€ ключа безопасности
                        ValidateIssuerSigningKey = true,
                    };
                });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
