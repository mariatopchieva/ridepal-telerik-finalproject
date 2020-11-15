using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RidePal.Data.Context;
using RidePal.Data.Models;
using RidePal.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Service
{
    public class AdminService : IAdminService
    {
        private readonly RidePalDbContext context;
        private readonly UserManager<User> _userManager;

        public AdminService(RidePalDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this._userManager = userManager;
        }

        public async Task<IList<User>> GetAllRegularUsers()
        {
            var regularUsers = new List<User>();
            var users = this.context.Users.ToList();

            foreach (var user in users)
            {
                if (await this._userManager.IsInRoleAsync(user, "User"))
                {
                    regularUsers.Add(user);
                }
            }

            return regularUsers;
        }

        public async Task<bool> BanUserById(int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return false;
            }

            user.LockoutEnabled = true;

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

            await context.SaveChangesAsync();

            return true;
        }
    }
}
