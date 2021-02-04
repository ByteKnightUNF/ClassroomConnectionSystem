using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUpload.Models
{
    public class AddingTagModel
    {

        public int Photo_id { get; set; }

        public int Tag { get; set; }


       
        public string Name { get; set; }

    }
}
