﻿using Microsoft.AspNetCore.Hosting;
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
        }

        /// <summary>
        /// Provides a physical and local file path to an image
        /// </summary>
        /// <param name="PlaylistDTO">The DTO of the playlist the image will be assigned to</param>
        /// <returns>Returns the physical and local path to an image</returns>
        public (string, string) GetFilePathForImage(PlaylistDTO PlaylistDTO)
        {
            var playlistImagesUploadFolder = Path.Combine(_env.WebRootPath, "assets\\img\\playlist");

            this.fileCheck.CreateFolder(playlistImagesUploadFolder);

            var newFileName = $"{Guid.NewGuid()}_{PlaylistDTO.Title.Trim().Replace(" ", "_")}";

            string playlistDBImageLocationPath = $"/assets/img/playlist/{newFileName}.jpg";

            return (playlistDBImageLocationPath, $"{playlistImagesUploadFolder}\\{newFileName}");

        }

        /// <summary>
        /// Provides an image from an external API to be attached to a playlist
        /// </summary>
        /// <param name="PlaylistDTO">The DTO of the playlist the image will be assigned to</param>
        /// <returns>Returns the local file path the image was saved to</returns>
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

        /// <summary>
        /// Determens whether the specific file exists
        /// </summary>
        /// <param name="filePath">The file path to the file</param>
        /// <returns>Returns true if the file exists else returns false</returns>
        public bool FileExists(string filePath)
        {
            return System.IO.File.Exists(filePath.Trim());
        }

        /// <summary>
        /// Checks if a folder exists and creates one if it does not
        /// </summary>
        /// <param name="filePath">The file path to the folder to be chacked</param>
        /// <returns>Returns true if the file path exists else tries to create a new folder or throws exeption on failed folder creation</returns>
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