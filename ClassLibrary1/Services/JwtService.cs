using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ClassLibrary1.Services
{
    public interface IJWTService
    {
        string GenerateJWT<T>(T entity) where T : class;
        string DecodeToken(string jwtToken, string claimType);
    }

    public class JWTService : IJWTService
    {
        private readonly IConfiguration _config;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public JWTService(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public string DecodeToken(string jwtToken, string nameClaim)
        {
            if (string.IsNullOrEmpty(jwtToken))
                throw new ArgumentNullException(nameof(jwtToken));

            try
            {
                var token = _tokenHandler.ReadJwtToken(jwtToken);
                var claim = token?.Claims.FirstOrDefault(c => c.Type.Equals(nameClaim));
                return claim?.Value ?? "Claim not found";
            }
            catch (Exception ex)
            {
                return $"Error decoding token: {ex.Message}";
            }
        }

        public string GenerateJWT<T>(T entity) where T : class
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:JwtKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();

            // Kiểm tra nếu entity là User (vì User có liên kết với bảng Role)
            if (entity is User user)
            {
                // Lấy Role của User từ bảng Role
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.RoleName));  
                claims.Add(new Claim("UserId", user.UserId.ToString())); 
                claims.Add(new Claim("FullName", user.FullName));  
                claims.Add(new Claim("Email", user.Email));  
            }
            else
            {
                throw new ArgumentException("Unsupported entity type", nameof(entity));
            }

            var token = new JwtSecurityToken(
               issuer: _config["JwtSettings:Issuer"],
               audience: _config["JwtSettings:Audience"],
               claims: claims,
               expires: DateTime.Now.AddMinutes(15),  // Thời gian hết hạn của token (15 phút)
               signingCredentials: credentials
               );

            return _tokenHandler.WriteToken(token);  // Trả về token đã mã hóa dưới dạng chuỗi
        }

    }
}
