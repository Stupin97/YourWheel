﻿namespace YourWheel.Host.Services
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;

    public class AuthenticationSettings : IAuthenticationSettings
    {
        public AuthenticationSettings()
        {
            this.JwtAudience = ConfigurationHelper.Configuration.GetValue<string>("JWT_AUDIENCE")
                                ?? Environment.GetEnvironmentVariable("JWT_AUDIENCE");

            this.JwtIssuer = ConfigurationHelper.Configuration.GetValue<string>("JWT_ISSUER")
                                ?? Environment.GetEnvironmentVariable("JWT_ISSUER");

            this.JwtLifeTime = Convert.ToInt32(ConfigurationHelper.Configuration.GetValue<string>("JWT_LIFETIME")
                                ?? Environment.GetEnvironmentVariable("JWT_LIFETIME"));

            var key = ConfigurationHelper.Configuration.GetValue<string>("JWT_KEY")
                        ?? Environment.GetEnvironmentVariable("JWT_KEY");

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

            this.IsAuthenticationEnabled = ConfigurationHelper.Configuration.GetValue<bool>("AuthenticationEnabled");
        }

        public string JwtAudience { get; }

        public string JwtIssuer { get; }

        public int JwtLifeTime { get; }

        public SymmetricSecurityKey JwtSymmetricSecurityKey { get; }

        public TokenValidationParameters JwtValidationParameters { get; }

        public bool IsAuthenticationEnabled { get; }
    }
}
