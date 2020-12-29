using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;

namespace DataLibrary.BussinessLogic
{
    public static class PhotoProccesor
    {
        public static int CreatePhoto(string Name, string Email, int School_Year_Begin, int School_Year_End , string Grade, string Teacher_Name, Byte[] ImageFile)
        {




            ImageModel data = new ImageModel
            {

                Name = Name,
                Email = Email,
                School_Year_Begin = School_Year_Begin,
                School_Year_End = School_Year_End,
                Grade = Grade,
                Teacher_Name = Teacher_Name,
                ImageFile = ImageFile

            };

            string sql = @"insert into dbo.Image (Name, Email, School_Year_Begin, School_Year_End, Grade, Teacher_Name, ImageFile)
                          values (@Name, @Email, @School_Year_Begin, @School_Year_End, @Grade, @Teacher_Name, @ImageFile );";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<ImageModel> GetPhotoId(int Id)
        {
    
            string sql = @"select *
                        from dbo.Image
                        Where Id = @Id;";

            return SqlDataAccess.LoadData<ImageModel>(sql);
        }

        public static List<ImageModel> LoadPhoto()
        {

            string sql = @"select id, Name, Email, School_Year_Begin, School_Year_End, Grade, Teacher_Name, ImageFile
                        from dbo.Image;";

            return SqlDataAccess.LoadData<ImageModel>(sql);
        }

        public static int Deleteimage(int Id)
        {
            ImageModel data = new ImageModel
            {
                Id = Id

            };

            string sql = @"DELETE FROM dbo.Image WHERE Id= @Id;";

            return SqlDataAccess.SaveData(sql, data);

        }
    }
}
