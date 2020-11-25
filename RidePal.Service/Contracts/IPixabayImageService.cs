using RidePal.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Service.Contracts
{
    public interface IPixaBayImageService
    {
        (string, string) GetFilePathForImage(PlaylistDTO playlistDTO);
        Task<string> AssignImage(PlaylistDTO playlistDTO);

    }
}
