using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class CommentModel
    {
        public int CommentId { get; set; }

        public string Comment { get; set; }

        public string Names { get; set; }

        public bool Flag { get; set; }

        public int ImageId { get; set; }

        public DateTime CommentDate { get; set; }
    }
}
