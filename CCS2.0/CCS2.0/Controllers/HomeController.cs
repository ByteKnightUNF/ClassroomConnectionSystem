
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CCS2._0.Models;
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
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using GalleryModel = CCS2._0.Models.GalleryModel;

namespace CCS2._0.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }



        public IActionResult Index(string sBase64String, ImageModel model)
        {
            List<ImageModel> Match = new List<ImageModel>();

            var match = LoadPhoto();

            foreach (var row in match)
            {


                Match.Add(new ImageModel
                {
                    ImageId = row.ImageId,
                    Name = row.Name,
                    Email = row.Email,
                    SchoolYearBegin  = row.SchoolYearBegin,
                    SchoolYearEnd = row.SchoolYearEnd,
                    Grade = row.Grade,
                    TeacherName = row.TeacherName,
                    src = this.ViewImage(row.ImageFile)
                });
            }

            return View(Match);
 

        }



    public IActionResult ViewPost(int ID, int page = 1)
        {
            List<ImageModel> Match = new List<ImageModel>();
            List<ImageUpload.Models.CommentModel> Com = new List<ImageUpload.Models.CommentModel>();
            List<ImageUpload.Models.AddingTagModel> Tag = new List<ImageUpload.Models.AddingTagModel>();
            List<Models.GalleryModel> Gallery = new List<Models.GalleryModel>();

            var match = GetPhotoId(ID);

            var com = GetCommentId(ID, page);

            var tag = getTagId(ID);

            var Pages = (int)Math.Ceiling((decimal)GetPages(ID)/5);

            var gallery = new List<Models.GalleryModel>();

            foreach (var entry in com)
            {
                Com.Add(new ImageUpload.Models.CommentModel
                {
                    CommentId = entry.CommentId,
                    Comment = entry.Comment,
                    Name = entry.Names,
                    Flag = entry.Flag,
                    ImageId = entry.ImageId
                });
            }

            foreach (var tags in tag)
            {
                Tag.Add(new ImageUpload.Models.AddingTagModel
                {

                    ImageId = tags.ImageId,
                    Tag = tags.Tag,
                    Name = tags.Name

                });
            }
           


            foreach (var row in match)
            {

                foreach (var entry in GetGallery(row.ImageId))
                {
                    gallery.Add(new GalleryModel
                    {
                        Id = entry.Id, 
                        GallerySrc = this.ViewImage(entry.ImageFile)
                    });
                }

                if (row.NumberOfPeople > 0)
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
                        NumberOfPeople = row.NumberOfPeople,
                        TaggedSrc = this.ViewImage(row.TaggedPhoto),
                        AddingTagModel = Tag,
                        CommentModel = Com,
                        Comments = new ImageUpload.Models.CommentModel(),
                        Pages = Pages,
                        CurrentPage = page,
                        GalleryModel = gallery
                    });
                }
                else
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
                        NumberOfPeople = row.NumberOfPeople,
                        CommentModel = Com,
                        Comments = new ImageUpload.Models.CommentModel(),
                        Pages = Pages,
                        CurrentPage = page,
                        GalleryModel = gallery
                    });
                }
                gallery = new List<Models.GalleryModel>();
            }

            return View(Match[0]);
        }

        [HttpPost]
        public IActionResult ViewPost(ImageUpload.Models.ImageModel model, int Id)
        {
          
            _ = CreateComment(model.Comments.Comment, model.Comments.Name, model.Comments.Flag, model.Comments.ImageId);

            return RedirectToAction("ViewPost", new { ID = Id });
        }


        public IActionResult NewTag(int Id, int Tag, string Name)
        {
                CreateTag(Id, Tag, Name);
            

            return RedirectToAction("ViewPost", new { ID = Id });
        }



        private string ViewImage(byte[] arrayImage)

        {
            try
            {
                string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);

                return "data:image/png;base64," + base64String;
            }
            catch
            {
                return null;
            }

           

        }

        public IActionResult FlagComment(int Id)
        {
            List<ImageUpload.Models.CommentModel> Com = new List<ImageUpload.Models.CommentModel>();
            var com = GetComment(Id);

            foreach (var row in com)
            {
                Com.Add(new ImageUpload.Models.CommentModel
                {
                    CommentId = row.CommentId,
                    Comment = row.Comment,
                    Name = row.Names,
                    Flag = row.Flag,
                    ImageId = row.ImageId
                });
            }

            return View(Com[0]);
        }
        [HttpPost]
        public IActionResult FlagComment(ImageUpload.Models.CommentModel model)
        {
            CreateFlag(model.FlagModel.CommentId, model.FlagModel.Reason);
            Flag(model.FlagModel.CommentId, true);
            return RedirectToAction("ViewPost", new { Id = model.ImageId });
        }




        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Email()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
