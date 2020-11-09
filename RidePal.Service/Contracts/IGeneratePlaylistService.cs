using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Service.Contracts
{
    public interface IGeneratePlaylistService
    {
        Task<int> GetTravelDuration(string startLocationName, string destinationName);
    }
}
