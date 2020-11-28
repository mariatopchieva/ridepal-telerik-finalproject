using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RidePal.Data.Models;
using RidePal.Service.Contracts;
using RidePal.Service.DTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using RidePal.Service.ApiHelpers;

namespace RidePal.Service
{
    public class APIUserService : IAPIUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly AppSettings appSettings;

        public APIUserService(IOptions<AppSettings> appSettings, UserManager<User> userManager, 
                                SignInManager<User>signInManager)
        {
            this.appSettings = appSettings.Value;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public APIUserDTO Authenticate(string username, string password)
        {
            var user = _userManager.FindByNameAsync(username).Result;

            // return null if user not found
            if (user == null)
                return null;

            var signInResult = _signInManager.CheckPasswordSignInAsync(user, password, false).Result;

            if (signInResult != SignInResult.Success)
            {
                return null;
                //Unauthorized
            }

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(14),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return new APIUserDTO(user);
        }

        
    }
}
