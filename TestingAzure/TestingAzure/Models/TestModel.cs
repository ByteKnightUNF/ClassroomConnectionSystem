using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestingAzure.Models
{
    public class TestModel
    {
        public int ID { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter the name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the date")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public string Date { get; set; }

    }
}
