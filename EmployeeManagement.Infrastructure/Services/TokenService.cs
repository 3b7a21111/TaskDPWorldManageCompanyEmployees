using EmployeeManagement.Core.Models;
using EmployeeManagement.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Infrastructure.Services
{
    //public class TokenService : ITokenService
    //{
    //    private readonly IConfiguration configuration;

    //    public TokenService(IConfiguration configuration)
    //    {
    //        this.configuration = configuration;
    //    }
    //    public string GenerateToken(User user)
    //    {
    //        var claims = new[]
    //       {
    //            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    //            new Claim(ClaimTypes.Name, user.Username),
    //            new Claim(ClaimTypes.Role, user.Role)
    //        };

    //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
    //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //        var token = new JwtSecurityToken(
    //            issuer: configuration["Jwt:Issuer"],
    //            audience: configuration["Jwt:Audience"],
    //            claims: claims,
    //            expires: DateTime.Now.AddDays(7),
    //            signingCredentials: creds
    //        );

    //        return new JwtSecurityTokenHandler().WriteToken(token);

    //    }
    //}
}
