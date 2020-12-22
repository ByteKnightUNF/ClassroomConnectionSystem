using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class CommentModel
    {
        public int Id { get; set; }

        public string Comment { get; set; }

        public string Name { get; set; }

        public Boolean Flag { get; set; }

        public int ImageId { get; set; }
    }
}
