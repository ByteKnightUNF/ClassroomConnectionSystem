
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DataLibrary.Models
{
    public class ImageModel
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public int School_Year_Begin { get; set; }

        public int School_Year_End { get; set; }
    

        public string Grade { get; set; }

        public string Teacher_Name { get; set; }

        public byte[] ImageFile { get; set; }
        public int Number_Of_People { get; set; }
        public byte[] Tagged_Photo { get; set; }



    }
}
