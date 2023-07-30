using FileManager.Core.Entities;
using FileManager.Core.Helper;
using FileManager.Core.Interfaces;

namespace FileManager.Host.Services
{
    public class FileManagerService : IFileManagerService
    {
        public async Task<List<FileItem>> GetFileList(string rootPath,string directory)
        {
            return await new FileManagerHelper(rootPath).GetFilesAsync(directory);
        }
    }
}
