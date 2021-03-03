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
        public static int CreatePhoto(string Name, string Email, int SchoolYearBegin, int SchoolYearEnd , string Grade, string TeacherName, Byte[] ImageFile)
        {




            ImageModel data = new ImageModel
            {

                Name = Name,
                Email = Email,
                SchoolYearBegin = SchoolYearBegin,
                SchoolYearEnd = SchoolYearEnd,
                Grade = Grade,
                TeacherName = TeacherName,
                ImageFile = ImageFile

            };

            string sql = @"insert into dbo.Image (Name, Email, SchoolYearBegin, SchoolYearEnd, Grade, TeacherName, ImageFile)
                          values (@Name, @Email, @SchoolYearBegin, @SchoolYearEnd, @Grade, @TeacherName, @ImageFile );";

            return SqlDataAccess.SaveData(sql, data);
        }
        
        public static int recordTag(int ImageId, Byte[] TagFile, int Tag)
        {




            TagModel data = new TagModel
            {
                ImageId = ImageId,
                TagFile = TagFile,
                Tag = Tag
               

            };

            string sql = @"update dbo.Image " +
                          "SET NumberOfPeople = @Tag, TaggedPhoto = @TagFile " +
                          "where ImageId = @ImageId;";

            return SqlDataAccess.SaveData(sql, data);
        }

      

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
        public static int CreateTag(int ImageId, int Tag, string Name )
        {

            AddingTagModel data = new AddingTagModel
            {
                ImageId = ImageId,
                Tag = Tag,
                Name = Name

            };

            string sql = @"insert into dbo.Tag (ImageId, Tag, Name)
                          values (@ImageId, @Tag, @Name);";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<ImageModel> GetPhotoId(int ImageId)
        {
            var parameters = new { ImageId = ImageId };

            string sql = @"select *
                        from dbo.Image
                        Where ImageId = @ImageId ;";

            return SqlDataAccess.LoadData<ImageModel>(sql, parameters);
        }
        
        public static List<AddingTagModel> getTagId(int ImageId)
        {
            var parameters = new { ImageId = ImageId };

            string sql = @"select *
                        from dbo.Tag
                        Where ImageId =  @ImageId ;";

            return SqlDataAccess.LoadData<AddingTagModel>(sql, parameters);

        }

        public static List<CommentModel> GetCommentId(int ImageId)
        {
            var parameters = new { ImageId = ImageId };
            string sql = @"select *
                        from dbo.Comment
                        Where ImageId = @ImageId ;";

            return SqlDataAccess.LoadData<CommentModel>(sql, parameters);
        }

        public static List<CommentModel> GetComment(int ImageId)
        {
            var parameters = new { ImageId = ImageId };

            string sql = @"select *
                        from dbo.Comment
                        Where Comment_Id = @ImageId ;";

            return SqlDataAccess.LoadData<CommentModel>(sql, parameters);
        }
        public static List<ImageModel> FindImg(string ImageId)
        {
            var parameters = new { ImageId = ImageId };
            string sql = @"select *
                        from dbo.Image


                        Where Name Like '%@ImageId%' OR Email Like '%@ImageId%'" +

                        "OR SchoolYearBegin Like '%@ImageId%' OR SchoolYearEnd Like '%@ImageId%'" +
                        "OR Grade Like '%@ImageId%' OR TeacherName Like '%@ImageId%';";

            return SqlDataAccess.LoadData<ImageModel>(sql, parameters);

        }


        public static List<ImageModel> LoadPhoto()
        {

            string sql = @"select ImageId, Name, Email, SchoolYearBegin, SchoolYearEnd, Grade, TeacherName, ImageFile
                        from dbo.Image;";

            return SqlDataAccess.LoadData<ImageModel>(sql);
        }

        public static List<CommentModel> LoadComment()
        {

            string sql = @"select CommentId, Comment, Names, Flag, ImageId
                        from dbo.Comment;";

            return SqlDataAccess.LoadData<CommentModel>(sql);
        }

        public static int Deleteimage(int ImageId)
        {

            ImageModel data = new ImageModel
            {
                ImageId = ImageId

            };

            string sql = @"DELETE FROM dbo.Image WHERE ImageId= @ImageId; DELETE FROM dbo.Comment WHERE ImageId= @ImageId;";

            return SqlDataAccess.SaveData(sql, data);

        }

        public static int DeleteComment(int Id)
        {
            CommentModel data = new CommentModel
            {
                CommentId = Id

            };

            string sql = @"DELETE FROM dbo.Comment WHERE CommentId= @CommentId;";

            return SqlDataAccess.SaveData(sql, data);

        }

        public static int Edit_Comment(int Id, string Comment, string Name, bool Flag, int ImageId)
        {
            CommentModel data = new CommentModel
            {
                CommentId = Id,
                Comment = Comment,
                Names = Name,
                Flag = Flag,
                ImageId = ImageId
            };

            string sql = @"UPDATE dbo.Comment SET Comment = @Comment, Names = @Names, Flag = @Flag WHERE CommentId= @CommentId;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<CommentModel> FlaggedComment()
        {
            string sql = @"select *
                        from dbo.Comment
                        Where Flag = 1;";
            return SqlDataAccess.LoadData<CommentModel>(sql);

        }

        public static List<CommentModel> SortName(string option)
        {
            string sql;
            if (option == "a") {
                sql = @"select *
                            from dbo.Comment
                            ORDER BY Names ASC;";
            } 
            else
            {
                sql = @"select *
                            from dbo.Comment
                            ORDER BY Names DESC;";
            }
            return SqlDataAccess.LoadData<CommentModel>(sql);
        }
    }
}
