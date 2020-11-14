using RidePal.Service.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static RidePal.Service.GetPixabayImage;

namespace RidePal.Service
{
    public class GetPixabayImage
    {
        FileServiceProvider fileServiceProvider;
        public void GetFilePathPixabayImage(GeneratePlaylistDTO playlistDTO)
        {
            //POST: GeneratePlaylist(GeneratePlaylistViewModel model) https://pastebin.com/QmduCCAm
            //we've received model as a parameter
            var playlistImagesUploadFolder = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\assets\\img\\playlist");
            this.fileServiceProvider.CreateFolder(playlistImagesUploadFolder);
            //var newFileName = $"{Guid.NewGuid()}_{playlistDTO.File.FileName}";
            //string fullFilePath = Path.Combine(playlistImagesUploadFolder, newFileName);
            //string playlistDBImageLocationPath = $"/assets/img/playlist/{newFileName}";

            //playlistDTO.ImagePath = playlistDBImageLocationPath;

            //using (var stream = new FileStream(fullFilePath, FileMode.Create))
            //{
            //    using (var memoryStream = new MemoryStream())
            //    {
            //        await playlistDTO.File.CopyToAsync(memoryStream);
            //        var x = memoryStream.ToArray();
            //        stream.Write(x, 0, x.Length);
            //    }
            //}
        }


        
    }
    public class FileServiceProvider //: IFileServiceProvider
    {
        public bool FileExists(string filePath)
        {
            return System.IO.File.Exists(filePath.Trim());
        }

        public (bool result, string message) CreateFolder(string filePath)
        {
            if (FileExists(filePath.Trim()))
            {
                return (true, $"Folder already exists: {filePath}");
            }
            try
            {
                System.IO.Directory.CreateDirectory(filePath);
                return (true, "");
            }
            catch (System.IO.IOException e)
            {
                return (false, e.Message);
            }
        }
    }
}