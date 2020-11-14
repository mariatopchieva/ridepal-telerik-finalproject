using RidePal.Data.Models;
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
    }
}
