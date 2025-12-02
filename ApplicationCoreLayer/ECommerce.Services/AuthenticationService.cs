using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.ServicesAbstraction;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<Result<UserDTO>> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Error.NotFound("User.NotFound", $"No User With Email {email} Was Found");

            var token = await CreateTokenAsync(user);

            return new UserDTO(user.Email!, user.DisplayName, token);
        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user is null)
                return Error.InvalidCrendentials("User.InvalidCrendentials");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!isPasswordValid)
                return Error.InvalidCrendentials("User.InvalidCrendentials");

            var token = await CreateTokenAsync(user);

            return new UserDTO(user.Email!, user.DisplayName, token);
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = new ApplicationUser
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.UserName,
            };

            var identityResult = await _userManager.CreateAsync(user, registerDTO.Password);
            if (identityResult.Succeeded)
            {
                var token = await CreateTokenAsync(user);
                return new UserDTO(user.Email!, user.DisplayName, token);
            }

            return identityResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Email, user.Email!),
                new (JwtRegisteredClaimNames.Name, user.UserName!)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new(ClaimTypes.Role, role));

            var secretKey = _configuration["JWTOptions:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWTOptions:Issuer"],
                audience: _configuration["JWTOptions:Audience"],
                expires: DateTime.UtcNow.AddHours(1),
                claims: claims,
                signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
