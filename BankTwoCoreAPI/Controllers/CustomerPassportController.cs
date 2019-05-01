using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankTwoCoreAPI.Controllers
{
    [Route("api/aurora/[controller]")]
    [ApiController]
    public class CustomerPassportController : ControllerBase
    {
        private ILogger _logger { get; }

        //Constructor
        public CustomerPassportController(ILogger<CustomerPassportController> logger)
        {
            _logger = logger;
        }

        public ActionResult UploadImage(IList<IFormFile> files)
        {
            IFormFile uploadedImage = files.FirstOrDefault();
            if (uploadedImage == null || uploadedImage.ContentType.ToLower().StartsWith("image/"))
            {
                MemoryStream memoryStream = new MemoryStream();
                uploadedImage.OpenReadStream().CopyTo(memoryStream);

               //Image image = System.Drawing.Image.FromStream(memoryStream);

                
            }

            return Ok();
        }
    }
}