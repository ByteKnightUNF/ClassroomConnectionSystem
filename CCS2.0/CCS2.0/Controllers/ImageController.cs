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


                    int recordCreated = CreatePhoto(model.Name,model.Email,model.SchoolYearBegin, model.SchoolYearEnd, model.Grade,model.TeacherName, bytes);

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
                    ImageId = row.ImageId,
                    Name = row.Name,
                    Email = row.Email,
                    SchoolYearBegin = row.SchoolYearBegin,
                    SchoolYearEnd = row.SchoolYearEnd,
                    Grade = row.Grade,
                    TeacherName = row.TeacherName,
                    src = this.ViewImage(row.ImageFile),
                    NumberOfPeople = row.NumberOfPeople

                });
            }

            return View(Match);
        }
        public IActionResult ManageTag(string sBase64String, ImageModel model, string searchString)
        {
            ViewBag.CurrentSearch = searchString;
            List<ImageModel> Match = new List<ImageModel>();
            List<ImageUpload.Models.AddingTagModel> Tag = new List<ImageUpload.Models.AddingTagModel>();

            var match = LoadPhoto();
            var tag = new List<DataLibrary.Models.AddingTagModel>();

            if (!string.IsNullOrEmpty(searchString))
            {
                match = FindTag(searchString); 
            }



            foreach (var row in match)
            {
                if (row.NumberOfPeople > 0)
                {
                    tag = getTagId(row.ImageId);

                    foreach (var tags in tag)
                    {
                        Tag.Add(new ImageUpload.Models.AddingTagModel
                        {

                            ImageId = tags.ImageId,
                            Tag = tags.Tag,
                            Name = tags.Name

                        });
                    }

                    Match.Add(new ImageModel
                    {

                        ImageId = row.ImageId,
                        SchoolYearBegin = row.SchoolYearBegin,
                        SchoolYearEnd = row.SchoolYearEnd,
                        Grade = row.Grade,
                        TeacherName = row.TeacherName,
                        Name = row.Name,
                        NumberOfPeople = row.NumberOfPeople,
                        TaggedSrc = this.ViewImage(row.TaggedPhoto),
                        AddingTagModel = Tag

                    });  ;

                    Tag = new List<ImageUpload.Models.AddingTagModel>();
                }

            }

            return View(Match);
        }



        public IActionResult DeleteImage(int ID, ImageModel model)
        {
            try
            {
                int recordCreated = RemoveImage(ID);
                return RedirectToAction("ViewImage");
            }
            catch
            {
                return View();
            }

        }


        public IActionResult ViewComment(string filter, int page = 1, string Search = "")
        {
            List<CommentModel> Com = new List<CommentModel>();
            var com = LoadComment(page, 15);
            var TotalPages = (int)Math.Ceiling((decimal)GetPages() / 15);
            ViewBag.CurrentSearch = Search;
            if (!string.IsNullOrEmpty(Search))
            {
                com = FindCom(Search, page);
                TotalPages = (int)Math.Ceiling((decimal)GetPagesSearchCom(Search) / 15);
            }
            else
            { 
                switch (filter)
                {
                    case "flag":
                        com = FlaggedComment();
                        TotalPages = (int)Math.Ceiling((decimal)FlagCount() / 15);
                        break;
                    case "ascending":
                        com = Sort("a", page);
                        break;
                    case "descending":
                        com = Sort("d", page);
                        break;
                    case "new":
                        com = Sort("n", page);
                        break;
                    case "old":
                        com = Sort("o", page);
                        break;
                    default:
                        break;
                }
            }
            foreach (var row in com)
            {
                List<ImageModel> Match = new List<ImageModel>();
                var match = GetPhotoId(row.ImageId);
                foreach (var side in match)
                {
                    Match.Add(new ImageModel
                    {
                        ImageId = side.ImageId,
                        SchoolYearEnd = side.SchoolYearEnd,
                        Grade = side.Grade,
                        TeacherName = side.TeacherName
                    });
                }
                Com.Add(new CommentModel
                {
                    CommentId = row.CommentId,
                    Comment = row.Comment,
                    Name = row.Names,
                    Flag = row.Flag,
                    ImageId = row.ImageId,
                    Class = Match[0].Grade+" | "+Match[0].SchoolYearEnd+" | "+Match[0].TeacherName,
                    CurrentPage = page,
                    Pages = TotalPages,
                    Filter = filter,
                    CommentDate = row.CommentDate.ToShortDateString()
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
            List<CommentModel> Com = new List<CommentModel>();
            List<FlagModel> List = new List<FlagModel>();
            var com = GetComment(Id);
            var list = GetReason(Id);
  
            foreach (var item in list)
            {
                List.Add(new FlagModel
                {
                    CommentId = item.CommentId,
                    Reason = item.Reason
                });
            }

            foreach (var row in com)
            {
                if (row.Flag == true)
                {

                    Com.Add(new CommentModel
                    {
                        CommentId = row.CommentId,
                        Comment = row.Comment,
                        Name = row.Names,
                        Flag = row.Flag,
                        ImageId = row.ImageId,
                        FlagModel = List[0],
                        CommentDate = row.CommentDate.ToShortDateString()
                    });
                }
                else
                {
                    Com.Add(new CommentModel
                    {
                        CommentId = row.CommentId,
                        Comment = row.Comment,
                        Name = row.Names,
                        Flag = row.Flag,
                        ImageId = row.ImageId,
                        CommentDate = row.CommentDate.ToShortDateString()
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
        public IActionResult DeleteTag(int ID, ImageModel model)
        {
            try
            {
                int recordCreated = RemoveTag(ID);

                return RedirectToAction("ManageTag");
            }
            catch
            {
                return View();
            }

        }

        public IActionResult EditTag(int Id, int Tag, string Name)
        {

            int recordCreated = Edit_Tag(Id, Tag, Name);

            return RedirectToAction("ManageTag");
        }



        public IActionResult RemoveFlag(int id)
        {
            Flag(id, false);
            DeleteFlag(id);
            return RedirectToAction("EditComment", new { Id = id});
        }

    }
}
