namespace FileManager.Core.Entities
{
    public class FileItem
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsDirectory { get; set; }
    }
}