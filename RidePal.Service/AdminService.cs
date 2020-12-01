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

        public AdminService(RidePalDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Provides a list of all users in the database including the admins
        /// </summary>
        /// <returns>Collection of all users in the database</returns>
        public async Task<IList<UserDTO>> GetAllUsers()
        {
            var users = await this.context.Users
                                        .Select(u => new UserDTO(u))
                                        .ToListAsync();
            return users;
        }

        /// <summary>
        /// Performs search for a user by a given ID in the database 
        /// </summary>
        /// <param name="id">The ID of the user to search for</param>
        /// <returns>Returns the UserDTO entry that matches the provided id</returns>
        public async Task<UserDTO> GetUserById(int userId)
        {
            var user = await this.context.Users
                .FirstOrDefaultAsync(user => user.Id == userId)
                ?? throw new ArgumentNullException("User was not found.");

            var userDto = new UserDTO(user);

            return userDto;
        }

        /// <summary>
        /// Performs search for a user in the database that matches partialy the input string
        /// </summary>
        /// <param name="email">The Email of the user to search for</param>
        /// <returns>Returns a collection of UserDTOs with email that matches the partial string input</returns>
        public async Task<IList<UserDTO>> SearchByEmail(string email)
        {
            if (email == null || email.Length == 0)
            {
                throw new ArgumentNullException("No search parameters submited.");
            }

            var userDTOs = await this.context.Users
                                            .Where(user => user.Email.Contains(email))
                                            .Select(u => new UserDTO(u))
                                            .ToListAsync();

            return userDTOs;
        }

        /// <summary>
        /// Performs an edit operation on a user entry by a given UserDTO 
        /// </summary>
        /// <param name="userDTO">The ID of the playlist to search for</param>
        /// <returns>Returns the edited userDTO</returns>
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

            if (userDto.FirstName != null) { userDbEntry.FirstName = userDto.FirstName; }
            if (userDto.LastName != null) { userDbEntry.LastName = userDto.LastName; }
            if (userDto.Email != null) { userDbEntry.Email = userDto.Email; }
            if (userDto.UserName != null) { userDbEntry.UserName = userDto.UserName; }
            if (userDto.Password != null) { userDbEntry.PasswordHash = hasher.HashPassword(userDbEntry, userDto.Password); }

            await this.context.SaveChangesAsync();

            return userDto;
        }

        /// <summary>
        /// Changes the LockoutEnabled status to true and LockoutEnd to the desired time of a user entry in the database
        /// </summary>
        /// <param name="id">The ID of the user to search for</param>
        /// <returns>If successful, returns true else returns false</returns>
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

        /// <summary>
        /// Changes the LockoutEnabled status to false and removes the ramaining time of the LockoutEnd of a user entry in the database
        /// </summary>
        /// <param name="id">The ID of the user to search for</param>
        /// <returns>If successful, returns true else returns false</returns>
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

        /// <summary>
        /// Changes the IsDeleted status to true of a user entry in the database
        /// </summary>
        /// <param name="id">The ID of the user to search for</param>
        /// <returns>If successful, returns true else returns false</returns>
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

        /// <summary>
        /// Changes the IsDeleted status to false of a user entry in the database
        /// </summary>
        /// <param name="id">The ID of the user to search for</param>
        /// <returns>If successful, returns true else returns false</returns>
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

        /// <summary>
        /// Provides a collection of the playlists with deleted status in playlistDTO form
        /// </summary>
        /// <param></param>
        /// <returns>Collection of playlistDTOs</returns>
        public async Task<IList<PlaylistDTO>> GetDeletedPlaylists()
        {
            var deletedPls = await this.context.Playlists.Where(pl => pl.IsDeleted == true)
                                                        .Select(pl => new PlaylistDTO(pl)).ToListAsync();

            return deletedPls;
        }
    }
}
