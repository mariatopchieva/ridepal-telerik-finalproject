using RidePal.Data.Models;
using RidePal.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Service.Contracts
{
    public interface IAdminService
    {
        Task<IList<User>> GetAllRegularUsers();
        Task<bool> BanUserById(int id);
        Task<bool> UnbanUserById(int id);
        Task<bool> DeleteUser(int id);
        Task<bool> RevertDeleteUser(int id);
        Task<UserDTO> EditUser(UserDTO userDto);
        Task<UserDTO> GetUserById(int userId);
    }
}
