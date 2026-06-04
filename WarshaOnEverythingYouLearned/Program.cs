using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WarshaOnEverythingYouLearned.Configrations;
using WarshaOnEverythingYouLearned.Data;

namespace WarshaOnEverythingYouLearned
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load environment variables (Railway uses __ as separator)
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.Configure<DbOptions>(builder.Configuration.GetSection("ConnectionStrings"));

            var ops = builder.Configuration.GetSection("Jwt").Get<JWTOptions>();

            // Add null check
            if (ops == null || string.IsNullOrEmpty(ops.key))
            {
                throw new InvalidOperationException("JWT configuration is missing or invalid");
            }

            builder.Services.AddSingleton(ops);

            builder.Services.AddDbContext<ApplicationDbContext>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ops.key)),
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? ops.issuer,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"] ?? ops.audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddControllers();

            // Add authorization
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseAuthentication(); // Add this BEFORE UseAuthorization
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}