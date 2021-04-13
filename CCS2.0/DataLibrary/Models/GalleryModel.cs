using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class GalleryModel
    {
        public int Id { get; set; }

        public int ImageId { get; set; }

        public byte[] ImageFile { get; set; }

       
    }
}
