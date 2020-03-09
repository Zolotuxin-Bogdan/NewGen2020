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

namespace Gallery.Controllers
{
    public class HomeController : Controller
    {
        public static string title;
        public static string manufacturer;
        public static string modelOfCamera;
        public static string fileSize;
        public static string dateCreation;
        public static string dateUpload;



        


        public static void LoadExifData(string LoadExifPath)
        {
            manufacturer = "Data not found";
            modelOfCamera = "Data not found";
            fileSize = "Data not found";
            dateCreation = "Data not found";
            dateUpload = "Data not found";

            FileInfo fileInfo = new FileInfo(LoadExifPath);
            FileStream ExifFileStream = new FileStream(LoadExifPath, FileMode.Open);
            try
            {

                BitmapSource img = BitmapFrame.Create(ExifFileStream);
                BitmapMetadata md = (BitmapMetadata)img.Metadata;

                //
                //FileSize from FileInfo
                if (fileInfo.Length >= 1024)
                {
                    fileSize = Math.Round((fileInfo.Length / 1024f), 1).ToString() + " KB";
                    if ((fileInfo.Length / 1024f) >= 1024f)
                    {
                        fileSize = Math.Round((fileInfo.Length / 1024f) / 1024f, 2).ToString() + " MB";
                    }
                }
                else
                {
                    fileSize = fileInfo.Length.ToString() + " B";
                }

                //
                //DateUpload from FileInfo
                if (fileInfo.CreationTime == null)
                {
                    dateUpload = "Data not found";
                }
                else
                {
                    dateUpload = fileInfo.CreationTime.ToString("dd.MM.yyyy HH:mm:ss");
                }

                //
                //title from FileInfo
                if (string.IsNullOrEmpty(fileInfo.Name))
                {
                    title = "Data not found";
                }
                else
                {
                    title = fileInfo.Name;
                }


                //
                //manufacturer from EXIF
                if (string.IsNullOrEmpty(md.CameraManufacturer))
                {
                    manufacturer = "Data not found";
                }
                else
                {
                    manufacturer = md.CameraManufacturer;
                }


                //
                //modelOfCamera from EXIF
                if (string.IsNullOrEmpty(md.CameraModel))
                {
                    modelOfCamera = "Data not found";
                }
                else
                {
                    modelOfCamera = md.CameraModel;
                }

                //
                //DateCreation from EXIF
                if (string.IsNullOrEmpty(md.DateTaken))
                {
                    dateCreation = "Data not found";
                }
                else
                {
                    dateCreation = md.DateTaken;
                }
            }
            catch (Exception err)
            {

                // need to static errors
            }
            ExifFileStream.Close();

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


        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase files)
        {
            try
            {
                if (files != null)
                {
                    //if (!string.IsNullOrEmpty(User.Identity.Name))
                    //{
                        string FormatsList = ConfigurationManager.AppSettings["SupportedPhotoFormat"].Replace(",", " ");
                        string UploadFileFormat = files.ContentType.Replace("image/", "");
                        bool IsFormatOk = FormatsList.Contains(UploadFileFormat);
                        if (IsFormatOk)//files.ContentType == "image/jpeg"
                        {
                            FileStream TempFileStream;
                            // Verify that the user selected a file and User is logged in
                            if (files.ContentLength > 0)
                            {
                                bool IsLoad = true;

                                // Encrypted User's directory path
                                string DirPath = Server.MapPath(PathFromConfig) + "/" + HashService.ComputeSha256Hash(User.Identity.Name);

                                // extract only the filename
                                var fileName = Path.GetFileName(files.FileName);

                                // store the file inside ~/Content/Temp folder
                                var TempPath = Path.Combine(Server.MapPath(PathFromConfig), fileName);
                                files.SaveAs(TempPath);

                                TempFileStream = new FileStream(TempPath, FileMode.Open);
                                BitmapSource img = BitmapFrame.Create(TempFileStream);
                                BitmapMetadata md = (BitmapMetadata)img.Metadata;
                                var DateTaken = md.DateTaken;
                                TempFileStream.Close();

                                if (!string.IsNullOrEmpty(DateTaken) || files.ContentType != "image/jpeg")
                                {
                                    if (Convert.ToDateTime(DateTaken) >= DateTime.Now.AddYears(-1) || files.ContentType != "image/jpeg")
                                    {
                                        TempFileStream = new FileStream(TempPath, FileMode.Open);
                                        Bitmap TempBmp = new Bitmap(TempFileStream);
                                        TempBmp = new Bitmap(TempBmp, 64, 64);
                                        TempFileStream.Close();

                                        // List of all Directories names
                                        List<string> dirsname = Directory.GetDirectories(Server.MapPath(PathFromConfig)).ToList<string>();

                                        FileStream CheckFileStream;
                                        Bitmap CheckBmp;

                                        List<string> filesname;

                                        // foreach inside foreach in order to check a new photo for its copies in all folders of all users
                                        foreach (string dir in dirsname)
                                        {
                                            filesname = Directory.GetFiles(dir).ToList<string>();
                                            foreach (string fl in filesname)
                                            {
                                                CheckFileStream = new FileStream(fl, FileMode.Open);
                                                CheckBmp = new Bitmap(CheckFileStream);
                                                CheckBmp = new Bitmap(CheckBmp, 64, 64);

                                                CheckFileStream.Close();

                                                if (ImageService.CompareBitmaps(TempBmp, CheckBmp))
                                                {
                                                    IsLoad = false;
                                                    ViewBag.Error = "Photo already exists!";
                                                    CheckBmp.Dispose();
                                                    break;
                                                }
                                                else
                                                    CheckBmp.Dispose();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.Error = "Photo created more than a year ago!";
                                        IsLoad = false;
                                    }
                                }
                                else
                                {
                                    ViewBag.Error = "Photo creation date not found!";
                                    IsLoad = false;
                                }

                                if (IsLoad)
                                {
                                    // extract only the filename
                                    var OriginalFileName = Path.GetFileName(files.FileName);
                                    // store the file inside User's folder
                                    var OriginalPath = Path.Combine(DirPath, OriginalFileName);
                                    //System.Windows.MessageBox.Show(OriginalPath);
                                    files.SaveAs(OriginalPath);
                                    System.IO.File.Delete(TempPath);
                                }
                                else
                                {
                                    System.IO.File.Delete(TempPath);
                                    return View("Error");
                                }

                            }
                            else
                            {
                                ViewBag.Error = "File too small!";
                                return View("Error");
                            }
                            // redirect back to the index action to show the form once again

                        }
                        else
                        {
                            ViewBag.Error = "Inappropriate format!";
                            return View("Error");
                        }
                    /*}
                    else
                    {
                        ViewBag.Error = "Log in please!";
                        return View("Error");
                    }*/
                }
                else
                {
                    return View();
                }
            }
            catch (Exception err)
            {

                //ViewBag.Error = "Unexpected error: " + err.Message;
                //return View("Error");

            }
            return RedirectToAction("Index");
        }

        public ActionResult Upload()
        {
            return View();
        }

    }
}