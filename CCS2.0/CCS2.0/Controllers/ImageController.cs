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
using FlagModel = ImageUpload.Models.FlagModel;
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



        public IActionResult ViewImage(string sBase64String, ImageModel model, string searchString, string searchInput)
        {
            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentSearch2 = searchInput;
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


        public IActionResult ViewComment(string filter, string cla)
        {
            List<CommentModel> Com = new List<CommentModel>();
            var com = LoadComment();
            bool ss = false;
            switch (filter)
            {
                case "flag":
                    com = FlaggedComment();
                    break;
                case "ascending":
                    com = SortName("a");
                    break;
                case "descending":
                    com = SortName("d");
                    break;
                case "new":
                    break;
                case "old":
                    break;
                case "class":
                    ss = true;
                    break;
                default:
                    break;
            }
            foreach (var row in com)
            {
                List<ImageModel> Match = new List<ImageModel>();
                var match = GetPhotoId(row.ImageId);
                foreach (var side in match)
                {
                    Match.Add(new ImageModel
                    {
                        School_Year_End = side.School_Year_End,
                        Grade = side.Grade,
                        Teacher_Name = side.Teacher_Name
                    });
                }
                Com.Add(new CommentModel
                {
                    CommentId = row.Comment_Id,
                    Comment = row.Comment,
                    Name = row.Names,
                    Flag = row.Flag,
                    ImageId = row.ImageId,
                    Class = Match[0].Grade+" Grade | "+Match[0].School_Year_End+" | "+Match[0].Teacher_Name
                });
            }
            if (ss)
            {
                List<CommentModel> sea = new List<CommentModel>();
                foreach (var item in Com)
                {
                    if (item.Class == cla)
                    {
                        sea.Add(new CommentModel{
                            CommentId = item.CommentId,
                            Comment = item.Comment,
                            Name = item.Name,
                            Flag = item.Flag,
                            ImageId = item.ImageId,
                            Class = item.Class
                        });
                    }
                }
                return View(sea);
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
            List<CommentModel> Com = new List<CommentModel>();
            List<FlagModel> List = new List<FlagModel>();
            var com = GetComment(Id);
            var list = GetReason(Id);
  
            foreach (var item in list)
            {
                List.Add(new FlagModel
                {
                    CommentId = item.comment_id,
                    Reason = item.reason
                });
            }

            foreach (var row in com)
            {
                if (row.Flag == true)
                {
                    Com.Add(new CommentModel
                    {
                        CommentId = row.Comment_Id,
                        Comment = row.Comment,
                        Name = row.Names,
                        Flag = row.Flag,
                        ImageId = row.ImageId,
                        FlagModel = List[0]
                    });
                }
                else
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
            }
            CommentModel test = new CommentModel();
            test = Com[0];

            return View(test);
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

        public IActionResult RemoveFlag(int id)
        {
            Flag(id, false);
            DeleteFlag(id);
            return RedirectToAction("EditComment", new { Id = id});
        }

    }
}
