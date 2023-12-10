using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;
using PlayTen.BLL.Services.Jwt;

namespace PlayTen.BLL.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IUserService _userService;

        public JwtService (IOptions<JwtOptions> jwtOptions, IUserService userService)
        {
            _jwtOptions = jwtOptions.Value;
            _userService = userService;
        }

        ///<inheritdoc/>
        public async Task<string> GenerateJwtTokenAsync(UserDTO userDto)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDto.Email),
                new Claim(JwtRegisteredClaimNames.NameId, userDto.Id),
                new Claim(JwtRegisteredClaimNames.FamilyName, userDto.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var roles = await _userService.GetRolesAsync(userDto);
            claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtOptions.Time),
                signingCredentials: creds);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
