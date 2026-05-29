
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using Talabat.Api.Extension;
using Talabat.APIS.Extension;
using Talabat.Core.Models.Identity;
using Talabat.Core.Service;
using Talabat.Service;
using Talabt.Reporisitory.Identity;

namespace Talabat.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddApplicationServices();
            builder.Services.AddDatabaseServices(builder.Configuration);
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins(builder.Configuration["ProjectFrontend"])
                        ;
                });
            });
            #region AddServices
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                  .AddEntityFrameworkStores<AppIdentityDbContext>();
            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var configuration = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(configuration);
            });
            builder.Services.AddApplicationServices();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JWT:ValidAudience"],

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])
                        )
                    };
                });
            #endregion

            var app = builder.Build();
            await app.ApplyMigrationsAndSeedAsync();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
