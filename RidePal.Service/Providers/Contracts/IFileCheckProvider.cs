using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Service.Providers.Contracts
{
    public interface IFileCheckProvider
    {
        bool FileExists(string filePath);
        (bool result, string message) CreateFolder(string filePath);
    }
}
