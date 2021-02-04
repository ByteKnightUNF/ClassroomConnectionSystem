using CCS2._0.Models;
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

    public class ImageModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the name of the person that submitted the photo")]
        [StringLength(50, ErrorMessage = "The Name cannot exceed 50 characters. ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the Email of the person that submitted the photo")]
        [StringLength(100, ErrorMessage = "The Email cannot exceed 100 characters. ")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter the beginning School Year of the photo")]
        [Display(Name = "School Year Beginning")]
        
        public int School_Year_Begin { get; set; }


        [Required(ErrorMessage = "Please enter the ending School Year of the photo")]
        [Display(Name = "School Year Ending")]
        public int School_Year_End { get; set; }

        [Required(ErrorMessage = "Please enter the Grade of the photo")]
        public string Grade { get; set; }


        [Required(ErrorMessage = "Please enter the Teacher Name in the photo")]
        [StringLength(50, ErrorMessage = "The Email cannot exceed 50 characters. ")]
        [Display(Name = "Teacher Name")]
        public string Teacher_Name { get; set; }


        [Required(ErrorMessage = "Please upload a file")]
        public IFormFile File { get; set; }
        
        public int Number_Of_People { get; set; }
        public byte[] Tagged_Photo { get; set; }

        [NotMapped]
        public string src { get; set; }
        [NotMapped]
        public string Tagged_src { get; set; }

        public ICollection<CommentModel> CommentModel { get; set; }

        public ICollection<AddingTagModel> AddingTagModel { get; set; }

    }


}
