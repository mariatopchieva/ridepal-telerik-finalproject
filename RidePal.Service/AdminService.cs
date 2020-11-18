using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RidePal.Data.Context;
using RidePal.Data.Models;
using RidePal.Service.Contracts;
using RidePal.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Service
{
    public class AdminService : IAdminService
    {
        private readonly RidePalDbContext context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AdminService(RidePalDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.context = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public async Task<IList<User>> GetAllRegularUsers()
        {
            var users = await this.context.Users.ToListAsync();

            if (users == null)
            {
                throw new ArgumentNullException("Something went wrong, no users wore found.");
            }

            return users;
        }

        public async Task<UserDTO> GetUserById(int userId)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(user => user.Id == userId);

            var userDto = new UserDTO(user);

            return userDto;
        }

        public async Task<UserDTO> EditUser(UserDTO userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException("No user info provided.");
            }

            var userDbEntry = this.context.Users.FirstOrDefaultAsync(user => user.Id == userDto.Id).Result;

            if (userDbEntry == null)
            {
                throw new ArgumentNullException("User not found in the database.");
            }

            var hasher = new PasswordHasher<User>();

            userDbEntry.FirstName = userDto.FirstName;
            userDbEntry.LastName = userDto.LastName;
            userDbEntry.Email = userDto.Email;
            userDbEntry.UserName = userDto.UserName;
            userDbEntry.PasswordHash = hasher.HashPassword(userDbEntry, userDto.Password);

            await this.context.SaveChangesAsync();

            return userDto;
        }

        public async Task<bool> BanUserById(int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return false;
            }
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTime.Now.AddYears(1);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnbanUserById(int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return false;
            }

            user.LockoutEnabled = false;
            user.LockoutEnd = DateTime.Now;

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return false;
            }

            user.IsDeleted = true;

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RevertDeleteUser(int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return false;
            }

            user.IsDeleted = false;

            await context.SaveChangesAsync();

            return true;
        }
    }
}
