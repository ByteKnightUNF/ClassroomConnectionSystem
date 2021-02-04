﻿
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
                    Id = row.Id,
                    Name = row.Name,
                    Email = row.Email,
                    School_Year_Begin = row.School_Year_Begin,
                    School_Year_End = row.School_Year_End,
                    Grade = row.Grade,
                    Teacher_Name = row.Teacher_Name,
                    src = this.ViewImage(row.ImageFile)
                });
            }

            return View(Match);
 

        }



    public IActionResult ViewPost(int ID )
        {
            List<ImageModel> Match = new List<ImageModel>();
            List<ImageUpload.Models.CommentModel> Com = new List<ImageUpload.Models.CommentModel>();
            List<ImageUpload.Models.AddingTagModel> Tag = new List<ImageUpload.Models.AddingTagModel>();

            var match = GetPhotoId(ID);

            var com = GetCommentId(ID);

            var tag = getTagId(ID);

            foreach (var entry in com)
            {
                Com.Add(new ImageUpload.Models.CommentModel
                {
                    CommentId = entry.Comment_Id,
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

                    Photo_id = tags.Photo_id,
                    Tag = tags.Tag,
                    Name = tags.Name

                });
            }
           

            foreach (var row in match)
            {


                Match.Add(new ImageModel
                {
                    Id = row.Id,
                    Name = row.Name,
                    Email = row.Email,
                    School_Year_Begin = row.School_Year_Begin,
                    School_Year_End = row.School_Year_End,
                    Grade = row.Grade,
                    Teacher_Name = row.Teacher_Name,
                    src = this.ViewImage(row.ImageFile),
                    Number_Of_People = row.Number_Of_People,
                    Tagged_src = this.ViewImage(row.Tagged_Photo),
                    CommentModel = Com,
                    AddingTagModel =Tag

                });
            }


            return View(Match);
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

        public IActionResult NewComment(int Id)
        {
            ViewBag.ImageId = Id;
            return View();
        }

        [HttpPost]
        public IActionResult NewComment(ImageUpload.Models.CommentModel model, int Id)
        {
            int recordCreated = CreateComment(model.Comment, model.Name, model.Flag, model.ImageId);

            return RedirectToAction("ViewPost", new { ID = Id });
        }

        public IActionResult NewTag(int Id, int Tag)
        {
            ViewBag.Photo_id = Id;
            ViewBag.Tag = Tag;
            return View();
        }

        [HttpPost]
        public IActionResult NewTag(int Id, int Tag, ImageUpload.Models.AddingTagModel model)
        {
            int recordCreated = CreateTag(Id, Tag, model.Name);

            return RedirectToAction("ViewPost", new { ID = Id });
        }


    }
}
