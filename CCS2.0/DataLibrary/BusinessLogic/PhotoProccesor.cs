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
        
        public static int recordTag(int Photo_id, Byte[] TagFile, int Tag)
        {




            TagModel data = new TagModel
            {

                TagFile = TagFile,
                Tag = Tag
               

            };

            string sql = @"update dbo.Image " +
                          "SET Number_Of_People = @Tag, Tagged_Photo = @TagFile " +
                          "where Id =" + Photo_id + ";";

            return SqlDataAccess.SaveData(sql, data);
        }

        //"insert into dbo.Tag (Photo_Id, Tag)
        // values(" + Photo_id + ", @Tag);" +

        public static int CreateComment(string Comment, string Name, Boolean Flag, int ImageId)
        {




            CommentModel data = new CommentModel
            {
                Comment = Comment,
                Names = Name,
                Flag = Flag,
                ImageId = ImageId

            };

            string sql = @"insert into dbo.Comment (Comment, Names, Flag, ImageId)
                          values (@Comment, @Names, @Flag, @ImageId);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<ImageModel> GetPhotoId(int Id)
        {

            string sql = @"select *
                        from dbo.Image
                        Where Id = "+@Id+ ";";

            return SqlDataAccess.LoadData<ImageModel>(sql);
        }


        public static List<CommentModel> GetCommentId(int Id)
        {

            string sql = @"select *
                        from dbo.Comment
                        Where ImageId = " + @Id + ";";

            return SqlDataAccess.LoadData<CommentModel>(sql);
        }

        public static List<CommentModel> GetComment(int Id)
        {

            string sql = @"select *
                        from dbo.Comment
                        Where Comment_Id = " + @Id + ";";

            return SqlDataAccess.LoadData<CommentModel>(sql);
        }
        public static List<ImageModel> FindImg(string imgId)
        {

            string sql = @"select *
                        from dbo.Image

                        Where Grade Like '%" + @imgId + "%';";

            return SqlDataAccess.LoadData<ImageModel>(sql);

        }

        public static List<ImageModel> LoadPhoto()
        {

            string sql = @"select id, Name, Email, School_Year_Begin, School_Year_End, Grade, Teacher_Name, ImageFile
                        from dbo.Image;";

            return SqlDataAccess.LoadData<ImageModel>(sql);
        }

        public static List<CommentModel> LoadComment()
        {

            string sql = @"select Comment_Id, Comment, Names, Flag, ImageId
                        from dbo.Comment;";

            return SqlDataAccess.LoadData<CommentModel>(sql);
        }

        public static int Deleteimage(int Id)
        {
            ImageModel data = new ImageModel
            {
                Id = Id

            };

            string sql = @"DELETE FROM dbo.Image WHERE Id= @Id; DELETE FROM dbo.Comment WHERE ImageId= @Id;";

            return SqlDataAccess.SaveData(sql, data);

        }

        public static int DeleteComment(int Id)
        {
            CommentModel data = new CommentModel
            {
                Comment_Id = Id

            };

            string sql = @"DELETE FROM dbo.Comment WHERE Comment_Id= @Comment_Id;";

            return SqlDataAccess.SaveData(sql, data);

        }

        public static int Edit_Comment(int Id, string Comment, string Name, bool Flag, int ImageId)
        {
            CommentModel data = new CommentModel
            {
                Comment_Id = Id,
                Comment = Comment,
                Names = Name,
                Flag = Flag,
                ImageId = ImageId
            };

            string sql = @"UPDATE dbo.Comment SET Comment = @Comment, Names = @Names, Flag = @Flag WHERE Comment_Id = @Comment_Id;";

            return SqlDataAccess.SaveData(sql, data);
        }
    }
}
