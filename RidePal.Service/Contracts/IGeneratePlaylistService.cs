using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Service.Contracts
{
    public interface IGeneratePlaylistService
    {
        Task<double> GetTravelDuration(string startLocationName, string destinationName);
    }
}
