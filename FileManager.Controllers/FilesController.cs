using FileManager.Controllers.DTO;
using FileManager.Core.Extentions;
using FileManager.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FileManager.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class FileManagerController : ControllerBase
    {
        private readonly IFileManagerService fileManagerService;
        public FileManagerController(IFileManagerService fileManagerService)
        {
            this.fileManagerService = fileManagerService;
        }

        [HttpGet]
        [Route("~/files")]
        public async Task<IList<FileItemDTO>> Get(string rootPath, string directory)
        {
            var files = await fileManagerService.GetFileList(rootPath, directory);

            return files.MapTo<FileItemDTO>().ToList();
        }
    }
}
