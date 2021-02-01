using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CCS2._0.Models
{
    public class TagModel
    {
        [Required(ErrorMessage = "Please upload a file")]
        public IFormFile TagFile { get; set; }

        public int Tag { get; set; }


        [NotMapped]
        public string src { get; set; }

    }
}
