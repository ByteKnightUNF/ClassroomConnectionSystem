
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DataLibrary.Models
{
    public class ImageModel
    {

        public int ImageId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public int SchoolYearBegin { get; set; }

        public int SchoolYearEnd { get; set; }
    

        public string Grade { get; set; }

        public string TeacherName { get; set; }

        public byte[] ImageFile { get; set; }
        public int NumberOfPeople { get; set; }
        public byte[] TaggedPhoto { get; set; }



    }
}
