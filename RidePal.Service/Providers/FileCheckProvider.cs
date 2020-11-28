using RidePal.Service.Providers.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Service.Providers
{
    public class FileCheckProvider : IFileCheckProvider
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
