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
    public class GalleryModel
    {

        public int Id { get; set; }

        public int ImageId { get; set; }

        [Required(ErrorMessage = "Please upload a file")]
        public IFormFile ImageFile { get; set; }

        [NotMapped]
        public string GallerySrc { get; set; }
    }
}
