using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ImageUpload.Models;
using DataLibrary;
using static DataLibrary.BussinessLogic.PhotoProccesor;
using DataLibrary.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;
using ImageModel = ImageUpload.Models.ImageModel;
using CommentModel = ImageUpload.Models.CommentModel;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Configuration;
using CCS2._0.Models;
using TagModel = ImageUpload.Models.TagModel;

namespace CCS2._0.Controllers
{
    public class ImageController : Controller
    {
        private readonly ILogger<ImageController> _logger;

        public ImageController(ILogger<ImageController> logger)
        {
            _logger = logger;
        }
    

        public IActionResult Photo_Upload()
        {
            ViewBag.image = null;
            return View();
        }


        [HttpPost]
        public IActionResult Photo_Upload(ImageModel model)
        {
            Byte[] bytes = null;

            if (model.File != null)
            {

                using (MemoryStream ms = new MemoryStream())
                {
                    {
                        model.File.OpenReadStream().CopyTo(ms);

                        bytes = ms.ToArray();
                    }


                    int recordCreated = CreatePhoto(model.Name,model.Email,model.School_Year_Begin, model.School_Year_End, model.Grade,model.Teacher_Name, bytes);

                    ViewBag.image = ViewImage(bytes);

                }
                return RedirectToAction("ViewImage");


            }

            return View();
        }

        [NonAction]

        private string ViewImage(byte[] arrayImage)

        {

            string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);

            return "data:image/png;base64," + base64String;

        }



        public IActionResult ViewImage(string sBase64String, ImageModel model, string searchString)
        {
            ViewBag.CurrentSearch = searchString;

            List<ImageModel> Match = new List<ImageModel>();

            var match = LoadPhoto();


            if (!string.IsNullOrEmpty(searchString))
            {
                match = FindImg(searchString);

            }

            foreach (var row in match)
            {


                Match.Add(new ImageModel
                {
                    Id = row.Id,
                    Name = row.Name,
                    Email =row.Email,
                    School_Year_Begin = row.School_Year_Begin,
                    School_Year_End = row.School_Year_End,
                    Grade =row.Grade,
                    Teacher_Name = row.Teacher_Name,
                    src = this.ViewImage(row.ImageFile)
                });
            }

            return View(Match);
        }

        public IActionResult DeleteImage(int ID, ImageModel model)
        {
            try
            {
                int recordCreated = Deleteimage(ID);
                return RedirectToAction("ViewImage");
            }
            catch
            {
                return View();
            }

        }


        public IActionResult ViewComment()
        {
            List<CommentModel> Com = new List<CommentModel>();
            var com = LoadComment();

            foreach(var row in com)
            {
                Com.Add(new CommentModel
                {
                    CommentId = row.Comment_Id,
                    Comment = row.Comment,
                    Name = row.Names,
                    Flag = row.Flag,
                    ImageId = row.ImageId
                });
            }

            return View(Com);
        }

        public IActionResult Delete_Comment(int ID)
        {
            try
            {
                int recordCreated = DeleteComment(ID);
                return RedirectToAction("ViewComment");
            }
            catch
            {
                return View();
            }

        }

        public IActionResult EditComment(int Id)
        {
            CommentModel Com = new CommentModel();
            var com = GetComment(Id);

            foreach (var row in com)
            {
                Com = (new CommentModel
                {
                    CommentId = row.Comment_Id,
                    Comment = row.Comment,
                    Name = row.Names,
                    Flag = row.Flag,
                    ImageId = row.ImageId
                });
            }

            return View(Com);
        }

        [HttpPost]
        public IActionResult EditComment(ImageUpload.Models.CommentModel model)
        {
            int recordCreated = Edit_Comment(model.CommentId, model.Comment, model.Name, model.Flag, model.ImageId);

            return RedirectToAction("ViewComment");
        }


        public IActionResult Tag()
        {
            ViewBag.image = null;
            return View();
        }


        [HttpPost]
        public IActionResult Tag(int ID, TagModel model)
        {
            Byte[] bytes = null;

            if (model.TagFile != null)
            {

                using (MemoryStream ms = new MemoryStream())
                {
                    {
                        model.TagFile.OpenReadStream().CopyTo(ms);

                        bytes = ms.ToArray();
                    }


                    int recordCreated = recordTag(ID, bytes, model.Tag);

                    ViewBag.image = ViewImage(bytes);

                }
                return RedirectToAction("ViewImage");


            }

            return View();
        }



    }
}
