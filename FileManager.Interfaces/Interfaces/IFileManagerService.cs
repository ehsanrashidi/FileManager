using FileManager.Core.Entities;

namespace FileManager.Core.Interfaces
{
    public interface IFileManagerService
    {
        Task<List<FileItem>> GetFileList(string rootPath, string directory);
    }
}
