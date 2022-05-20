using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace RestaurantAPI.Controllers
{
    [Route("file")]
    [Authorize]
    public class FileController : ControllerBase
    {
        [HttpGet]
        [ResponseCache(Duration = 1200, VaryByQueryKeys = new[] { "fileName" })]
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

        [HttpPost]
        public ActionResult Upload([FromForm] IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var rootPath = Directory.GetCurrentDirectory();
                var filePath = $"{rootPath}/PrivateFiles/{file.FileName}";

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Ok();
            }

            return BadRequest();
        }

    }
}
