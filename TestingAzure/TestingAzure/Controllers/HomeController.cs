using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestingAzure.Models;

using DataLibrary;
using static DataLibrary.BusinessLogic.TestingProccesor;
using DataLibrary.Models;
using System.Data.SqlClient;

namespace TestingAzure.Controllers
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult CreateTesting()
        {
            return View();
        }
        public IActionResult ViewTest()
        {
            var data = LoadTest();
            List<Models.TestModel> oppurtunities = new List<Models.TestModel>();
            foreach (var row in data)
            {
                oppurtunities.Add(new Models.TestModel
                {
                    ID = row.Id,
                    Name = row.Name,
                    Date = row.Date,
                  

                }); ;

            }

         
            return View(oppurtunities);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
