using RidePal.Data.Context;
using RidePal.Data.Models.PixaBayAPIModels;
using RidePal.Service.DTO;
using RidePal.Service.Providers.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RidePal.Service
{
    public class PixabayImageService
    {
        const string key = "19219313-933fd243a2660803c93258784";
        private readonly RidePalDbContext context;
        private readonly IDateTimeProvider dateTimeProvider;
        private HttpClient client = new HttpClient();
        private FileServiceProvider fileServiceProvider;

        public PixabayImageService(RidePalDbContext context, 
                                IDateTimeProvider date, 
                                FileServiceProvider fileService)
        {
            this.context = context;
            this.dateTimeProvider = date;
            this.fileServiceProvider = fileService;
        }

        public async Task GetFilePathPixabayImage(GeneratePlaylistDTO genPlaylistDTO)
        {
            //POST: GeneratePlaylist(GeneratePlaylistViewModel model) https://pastebin.com/QmduCCAm
            //we've received model as a parameter
            var playlistImagesUploadFolder = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\assets\\img\\playlist");

            this.fileServiceProvider.CreateFolder(playlistImagesUploadFolder);

            var newFileName = $"{Guid.NewGuid()}_{genPlaylistDTO.PlaylistName}";

            string fullFilePath = Path.Combine(playlistImagesUploadFolder, newFileName);

            string playlistDBImageLocationPath = $"/assets/img/playlist/{newFileName}";

            var playlistDTO = new PlaylistDTO()
            {
                FilePath = playlistDBImageLocationPath
            };

            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                using (var memoryStream = new MemoryStream())
                {
                    //await playlistDTO.File.CopyToAsync(memoryStream);

                    var x = memoryStream.ToArray();

                    stream.Write(x, 0, x.Length);
                }
            }
        }

        public async Task DownloadRandomImage(string queryString, string key)
        {
            var queryToUrl = queryString.TrimStart().TrimEnd().Replace(" ", "+").ToString();

            string startUrl = $"https://pixabay.com/api/?key={key}&q={queryToUrl}&image_type=photo&pretty=true";

            var response = await client.GetAsync(startUrl);

            var jsonString = await response.Content.ReadAsStringAsync();

            var pixaBayImageCollection = JsonSerializer.Deserialize<PixaBayImageCollection>(jsonString);

            var random = new Random(pixaBayImageCollection.PixaBayImages.Count).ToString();

            var imgUrl = pixaBayImageCollection.PixaBayImages[int.Parse(random)];

            using (HttpClient c = new HttpClient())
            {
                using (Stream s = await c.GetStreamAsync(imgUrl.ToString()))
                {
                    // do any logic with the image stream, save it,...
                }
            }
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