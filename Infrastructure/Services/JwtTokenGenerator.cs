using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Core.Entities;
using Core.DTOs;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthResponse GenerateToken(ApplicationUser user)
        {
            // الحصول على الإعدادات مباشرة من التكوين
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = jwtSettings["Key"] ?? throw new ArgumentNullException("JWT Key is missing");

            // إصلاح الخطأ: تحويل المفتاح النصي إلى بايتات مباشرة
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
           new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),// بدلاً من JwtRegisteredClaimNames.Sub
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("fullName", user.FullName ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: credentials
            );

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = user.Id.ToString(),
                Email = user.Email,
                FullName = user.FullName
            };
        }
    }
}
