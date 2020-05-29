using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Configuration;
using System.Web.Mvc;
using Gallery.BLL;
using System.Threading.Tasks;
using Gallery.PL.Filters;
using Gallery.PL.Manager;

namespace Gallery.Controllers
{
    public class HomeController : Controller
    {
        private IimageService _imageService;
        private IHashService _hashService;
        public HomeController(IimageService imageService, IHashService hashService)
        {
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
        }

        
        //Picture picture = new Picture();
        //[HttpGet]

        string PathFromConfig = ConfigurationManager.AppSettings["PathForSave"];
        [HttpGet]
        public ActionResult Delete(string pathForDelete = "")
        {
            try
            {
                if (pathForDelete.Replace(PathFromConfig, "").Replace(Path.GetFileName(pathForDelete), "").Replace("/", "") == HashService.ComputeSha256Hash(User.Identity.Name))
                {
                    if (pathForDelete != "" && Directory.Exists(Server.MapPath(pathForDelete.Replace(Path.GetFileName(pathForDelete), ""))))
                        System.IO.File.Delete(Server.MapPath(pathForDelete));
                    else
                    {
                        ViewBag.Error = "File not found!";
                        return View("Error");
                    }
                }
                else
                {
                    ViewBag.Error = "Authorisation Error!";
                    return View("Error");
                }
            }
            catch (Exception err)
            {
                ViewBag.Error = "Unexpected error: " + err.Message;
                return View("Error");
            }
            return RedirectToAction("Index");
        }


        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }

        [Authorize]
        [LogFilter]
        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase files)
        {
            byte[] data;

            using (Stream inputStream = files.InputStream) 
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();

                memoryStream.Close();
            }

            var defaultPath = GalleryConfig.GetPathForSave();
            var directoryPath = Server.MapPath(defaultPath) + _hashService.ComputeSha256Hash(User.Identity.Name);
            var mediaPath = Path.Combine(directoryPath, _hashService.ComputeSha256Hash(data));

            var userId = Convert.ToInt32(User.Identity.Name);
            var isUploadSuccess = await _imageService.UploadImageAsync(data, mediaPath, userId);
            if (!isUploadSuccess)
            {
                ViewBag.Error = "Something happened wrong... Try it again.";
                return View("Error");
            }

            return RedirectToAction("Index");

        }

        public ActionResult Upload()
        {
            return View();
        }

    }
}