﻿using System;
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
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Configuration;

namespace CCS2._0.Controllers
{
    public class ImageController : Controller
    {
        private readonly ILogger<ImageController> _logger;

        public ImageController(ILogger<ImageController> logger)
        {
            _logger = logger;
        }
    

        public IActionResult index()
        {
            ViewBag.image = null;
            return View();
        }


        [HttpPost]
        public IActionResult index(ImageModel model)
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


                    int recordCreated = CreatePhoto(model.Name,model.Email,model.School_Year,model.Grade,model.Teacher_Name, bytes);

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



        public IActionResult ViewImage(string sBase64String, ImageModel model)
        {

            List<ImageModel> Match = new List<ImageModel>();

            var match = LoadPhoto();

            foreach (var row in match)
            {


                Match.Add(new ImageModel
                {
                    Id = row.Id,
                    Name = row.Name,
                    Email =row.Email,
                    School_Year =row.School_Year,
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
    }
}