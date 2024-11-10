using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.V1.AccessTokens;

public static class TokenGenerator
{
    public static string GenerateToken(string secretKey)
    {
        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //var claims = new List<Claim>
        //{
        //    new(JwtRegisteredClaimNames.Sub, "user"), // Subject: Can be any value
        //    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //    new("role", "admin") // Custom claim (optional)
        //};

        var tokenDescriptor = new JwtSecurityToken(
            issuer: "authenticationservice",
            audience: "weatherforcastapi",
            //claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenDescriptor);
    }
}
