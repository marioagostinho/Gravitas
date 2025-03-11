using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SharedLibrary.Utils
{
    public enum ETokenType
    {
        Access = 0,
        Refresh = 1
    }

    public static class JwtConfig
    {
        public const string ISSUER = "Jwt:Issuer";
        public const string AUDIENCE = "Jwt:Audience";
        public const string ACCESS_TOKEN_SECRET = "Jwt:AccessTokenSecret";
        public const string REFRESH_TOKEN_SECRET = "Jwt:RefreshTokenSecret";
        public const double ACCESS_TOKEN_EXPIRY_DATE = 30;
        public const double REFRESH_TOKEN_EXPIRY_DATE = 7;

        public static string GetKeyPath(ETokenType tokenType)
        {
            return (tokenType == ETokenType.Access) ? ACCESS_TOKEN_SECRET : REFRESH_TOKEN_SECRET;
        }

        public static DateTime GetExpirationDate(ETokenType tokenType)
        {
            return (tokenType == ETokenType.Access) 
                ? DateTime.UtcNow.AddMinutes(ACCESS_TOKEN_EXPIRY_DATE) 
                : DateTime.UtcNow.AddDays(REFRESH_TOKEN_EXPIRY_DATE);
        }
    }

    public class JwtHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtHelper(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public Result<(string Token, DateTime ExpiresAt)> GenerateToken(Guid id, ETokenType tokenType = ETokenType.Access, 
            DateTime? expiresAt = null)
        {
            if (id == Guid.Empty)
            {
                return Result<(string, DateTime)>.Failure("Invalid claim identifier.");
            }
            var expiration = expiresAt ?? JwtConfig.GetExpirationDate(tokenType);

            var issuer = _configuration[JwtConfig.ISSUER];
            var audience = _configuration[JwtConfig.AUDIENCE];
            var tokenSecret = _configuration[JwtConfig.GetKeyPath(tokenType)];

            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(tokenSecret))
            {
                return Result<(string, DateTime)>.Failure("JWT configuration is missing required values.");
            }

            var key = Encoding.UTF8.GetBytes(tokenSecret);
            var signingKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString())
            };

            var tokenDescriptor = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: expiration,
                signingCredentials: credentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return Result<(string Token, DateTime ExpiresAt)>.Success((token, expiration));
        }

        public Result<ClaimsPrincipal> ValidateToken(string token, ETokenType tokenType = ETokenType.Access)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Result<ClaimsPrincipal>.Failure("Token is missing or empty.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();


            if (!(tokenHandler.ReadToken(token) is JwtSecurityToken jwtHelper))
            {
                return Result<ClaimsPrincipal>.Failure("Invalid token format.");
            }

            var issuer = _configuration[JwtConfig.ISSUER];
            var audience = _configuration[JwtConfig.AUDIENCE];
            var tokenSecret = _configuration[JwtConfig.GetKeyPath(tokenType)];

            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(tokenSecret))
            {
                return Result<ClaimsPrincipal>.Failure("JWT configuration is missing required values.");
            }

            var key = Encoding.UTF8.GetBytes(tokenSecret);

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                return Result<ClaimsPrincipal>.Success(tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken));
            }
            catch (SecurityTokenExpiredException)
            {
                return Result<ClaimsPrincipal>.Failure("Token has expired.");
            }
            catch (SecurityTokenValidationException)
            {
                return Result<ClaimsPrincipal>.Failure("Token validation failed.");
            }
            catch
            {
                return Result<ClaimsPrincipal>.Failure("An error occurred while validating the token.");
            }
        }

        public Result<Guid> ExtractIdentifier()
        {
            if (_httpContextAccessor == null)
            {
                return Result<Guid>.Failure("Error trying to access to the http context.");
            }

            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return Result<Guid>.Failure("Token is missing or improperly formatted.");
            }

            var token = authorizationHeader["Bearer ".Length..];
            var tokenValidation = ValidateToken(token);

            if (!tokenValidation.IsSuccess)
            {
                return Result<Guid>.Failure(tokenValidation.Errors);
            }

            var claimId = tokenValidation.Value.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(claimId, out var userId))
            {
                return Result<Guid>.Failure("Invalid claim identifier.");
            }

            return Result<Guid>.Success(userId);
        }
    }
}