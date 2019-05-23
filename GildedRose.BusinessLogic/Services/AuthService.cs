using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using GildedRose.BusinessLogic.DTOs;
using GildedRose.BusinessLogic.Exceptions;
using GildedRose.BusinessLogic.Interfaces.Services;
using GildedRose.DataAccess.Interfaces.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace GildedRose.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string GetUserToken(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Username or password is incorrect");
            }

            var entity = _userRepository.SignIn(username, password);
            if (entity == null)
            {
                throw new ItemNotFoundException("User didn't found");
            }

            var claims = new List<Claim>
            {
                new Claim("name", entity.Name),
                new Claim("id",  $"{entity.Id}"),
                new Claim(ClaimTypes.Role, entity.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            var jwt = new JwtSecurityToken(TokenConfig.ISSUER, TokenConfig.AUDIENCE, notBefore: DateTime.UtcNow,
                claims: claimsIdentity.Claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(TokenConfig.DAYS_LIFETIME)),
                signingCredentials: new SigningCredentials(TokenConfig.SymmetricSecurityKey,
                    SecurityAlgorithms.HmacSha256));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            return accessToken;
        }

        public UserDto GetUserClaims(ClaimsIdentity identityClaims)
        {
            if (identityClaims == null) return null;

            int.TryParse(identityClaims.FindFirst("id")?.Value, out var id);

            var user = new UserDto
            {
                Id = id,
                Name = identityClaims.FindFirst("name")?.Value,
                Role = identityClaims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
            };

            return user;
        }
    }
}
