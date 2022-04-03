using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace RestaurantAPI.Controllers
{
    [Route("file")]
    [Authorize]
    public class FileController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetFile([FromQuery] string fileName)
        {
            var rootPath = Directory.GetCurrentDirectory();

            var filePath = $"{rootPath}/PrivateFiles/{fileName}";

            bool fileExist = System.IO.File.Exists(filePath);
            if (!fileExist)
                return NotFound();

            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(filePath, out string fileContentTypeType);

            var fileContent = System.IO.File.ReadAllBytes(filePath);

            return File(fileContent, fileContentTypeType, fileName);
        }
    }
}
