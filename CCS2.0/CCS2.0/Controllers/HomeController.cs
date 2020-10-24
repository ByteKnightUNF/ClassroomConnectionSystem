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



       
    public IActionResult Index()
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
                    School_Year = row.School_Year,
                    Grade = row.Grade,
                    Teacher_Name = row.Teacher_Name,
                    src = this.ViewImage(row.ImageFile)
                });
            }

            return View(Match);

           
        }
        private string ViewImage(byte[] arrayImage)

        {

            string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);

            return "data:image/png;base64," + base64String;

        }

        public IActionResult Privacy()
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
