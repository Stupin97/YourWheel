namespace YourWheel.Host.Services
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using YourWheel.Host.Helpers;

    public class AuthenticationSettings : IAuthenticationSettings
    {
        public AuthenticationSettings()
        {
            /*Позже перенести в appsettings.json*/
            this.JwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

            this.JwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");

            this.JwtLifeTime = Convert.ToInt32(Environment.GetEnvironmentVariable("JWT_LIFETIME"));

            var key = Environment.GetEnvironmentVariable("JWT_KEY");

            if (key != null) this.JwtSymmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

            this.JwtValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = this.JwtIssuer,
                ValidateAudience = true,
                ValidAudience = this.JwtAudience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = this.JwtSymmetricSecurityKey,
                ClockSkew = TimeSpan.Zero
            };
        }

        public string JwtAudience { get; }

        public string JwtIssuer { get; }

        public int JwtLifeTime { get; }

        public SymmetricSecurityKey JwtSymmetricSecurityKey { get; }

        public TokenValidationParameters JwtValidationParameters { get; }
    }
}
