using Microsoft.AspNetCore.Hosting;
using RidePal.Data.Context;
using RidePal.Data.Models.PixaBayAPIModels;
using RidePal.Service.Contracts;
using RidePal.Service.DTO;
using RidePal.Service.Providers;
using RidePal.Service.Providers.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RidePal.Service
{
    public class PixabayImageService : IPixaBayImageService
    {
        const string key = "19219313-933fd243a2660803c93258784";
        string startUrl = $"https://pixabay.com/api/?key={key}&q=music&image_type=photo&pretty=true&per_page=200";

        private HttpClient client = new HttpClient();
        private IWebHostEnvironment _env;
        private readonly IFileCheckProvider fileCheck;

        //private IFileCheckProvider _fileCheck;

        public PixabayImageService(IWebHostEnvironment env, IFileCheckProvider fileCheck)
        {
            this._env = env;
            this.fileCheck = fileCheck;
            //this
        }

        public (string, string) GetFilePathForImage(PlaylistDTO PlaylistDTO)
        {
            //POST: GeneratePlaylist(GeneratePlaylistViewModel model) https://pastebin.com/QmduCCAm
            //we've received model as a parameter
            var playlistImagesUploadFolder = Path.Combine(_env.WebRootPath, "assets\\img\\playlist");

            this.fileCheck.CreateFolder(playlistImagesUploadFolder);

            var newFileName = $"{Guid.NewGuid()}_{PlaylistDTO.Title.Trim().Replace(" ", "_")}";

            //string fullFilePath = Path.Combine(playlistImagesUploadFolder, newFileName)

            string playlistDBImageLocationPath = $"/assets/img/playlist/{newFileName}.jpg";

            return (playlistDBImageLocationPath, $"{playlistImagesUploadFolder}\\{newFileName}");

        }

        public async Task<string> AssignImage(PlaylistDTO PlaylistDTO)
        {
            var jsonString = await client.GetAsync(startUrl).Result.Content.ReadAsStringAsync();
            var pixaBayImageCollection = JsonSerializer.Deserialize<PixaBayImageCollection>(jsonString);
            int imgIndex = new Random().Next(0, pixaBayImageCollection.PixaBayImages.Count);
            string imgUrl = pixaBayImageCollection.PixaBayImages[imgIndex].WebFormatURL;
            (string, string) paths = GetFilePathForImage(PlaylistDTO);
            string filePath = paths.Item1;
            string physicalPath = paths.Item2;

            using (WebClient c = new WebClient())
            {
                byte[] imageData = c.DownloadData(imgUrl.ToString()) 
                    ?? throw new ArgumentNullException("Image not found.");

                using MemoryStream mem = new MemoryStream(imageData);
                using (var yourImage = Image.FromStream(mem))
                {
                    yourImage.Save($"{physicalPath}.jpg", ImageFormat.Jpeg);
                }
            }

            return filePath;
        }

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