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
    public class CommentModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your Comment.")]
        [StringLength(256, ErrorMessage = "The Comment cannot exceed 256 characters.")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Please enter your name.")]
        [StringLength(50, ErrorMessage = "The Name cannot exceed 50 characters.")]
        public string Name { get; set; }

        public Boolean Flag { get; set; }

        [ForeignKey]
        public int ImageId { get; set; }
    }
}
