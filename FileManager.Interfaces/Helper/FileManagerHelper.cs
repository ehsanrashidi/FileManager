using FileManager.Core.Entities;

namespace FileManager.Core.Helper
{
    public class FileManagerHelper
    {
        private readonly string _rootPath;

        public FileManagerHelper(string rootPath)
        {
            _rootPath = rootPath;
        }

        public async Task<List<FileItem>> GetFilesAsync(string directoryPath)
        {
            var files = Directory.GetFiles(Path.Combine(_rootPath, directoryPath));
            var fileItems = new List<FileItem>();

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                fileItems.Add(new FileItem
                {
                    Name = fileInfo.Name,
                    Size = fileInfo.Length,
                    DateModified = fileInfo.LastWriteTimeUtc,
                    IsDirectory = false
                });
            }

            var directories = Directory.GetDirectories(Path.Combine(_rootPath, directoryPath));

            foreach (var directory in directories)
            {
                var directoryInfo = new DirectoryInfo(directory);
                fileItems.Add(new FileItem
                {
                    Name = directoryInfo.Name,
                    Size = 0,
                    DateModified = directoryInfo.LastWriteTimeUtc,
                    IsDirectory = true
                });
            }

            return fileItems;
        }

        public async Task<FileItem> GetFileAsync(string filePath)
        {
            var fileInfo = new FileInfo(Path.Combine(_rootPath, filePath));

            if (!fileInfo.Exists)
            {
                return null;
            }

            return new FileItem
            {
                Name = fileInfo.Name,
                Size = fileInfo.Length,
                DateModified = fileInfo.LastWriteTimeUtc,
                IsDirectory = false
            };
        }

        public async Task<bool> CreateDirectoryAsync(string directoryPath)
        {
            var path = Path.Combine(_rootPath, directoryPath);

            if (Directory.Exists(path))
            {
                return false;
            }

            Directory.CreateDirectory(path);
            return true;
        }

        public async Task<bool> DeleteFileAsync(string filePath)
        {
            var path = Path.Combine(_rootPath, filePath);

            if (!File.Exists(path))
            {
                return false;
            }

            File.Delete(path);
            return true;
        }

        public async Task<bool> DeleteDirectoryAsync(string directoryPath, bool recursive)
        {
            var path = Path.Combine(_rootPath, directoryPath);

            if (!Directory.Exists(path))
            {
                return false;
            }

            Directory.Delete(path, recursive);
            return true;
        }
    }
}
