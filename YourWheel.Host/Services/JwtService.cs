namespace YourWheel.Host.Services
{
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;

    public class JwtService : IJwtService
    {
        private readonly IAuthenticationSettings _authSettings;

        public JwtService(IAuthenticationSettings authSettings)
        {
            this._authSettings = authSettings;
        }

        public JwtSecurityToken GenerateToken(IEnumerable<Claim> claims)
        {
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                _authSettings.JwtIssuer,
                _authSettings.JwtAudience,
                notBefore: now,
                claims: claims,
                expires: now.Add(TimeSpan.FromHours(_authSettings.JwtLifeTime)),
                signingCredentials: new SigningCredentials(_authSettings.JwtSymmetricSecurityKey,
                    SecurityAlgorithms.HmacSha256));

            return jwt;
        }

        public DateTime GetTokenExpiryDateTime(string tokenString)
        {
            var token = new JwtSecurityToken(tokenString);

            var tokenExpiryDateTime = token.ValidTo;

            return tokenExpiryDateTime;
        }

        public string GetTokenString(JwtSecurityToken token)
        {
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        public Guid GetUserId(string tokenString)
        {
            var token = new JwtSecurityToken(tokenString);

            var userId = Guid.Parse(token.Payload[Constants.UserIdClaimType].ToString());

            return userId;
        }

        public bool IsTokenStringValid(string tokenString)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(tokenString, _authSettings.JwtValidationParameters, out _);
            }
            catch (SecurityTokenExpiredException)
            {
                return true;
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
