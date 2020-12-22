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
    public static class CommentProccesor
    {
        public static int CreateComment(int Id, string Comment, string Name, Boolean Flag, int PhotoId)
        {


            CommentModel data = new CommentModel
            {
                Id = Id,
                Comment = Comment,
                Name = Name,
                Flag = Flag,
                ImageId = PhotoId
            };

            string sql = @"insert into dbo.Comment (Id, Comment, Name, Flag, ImageId)
                          values (@Id, @Comment, @Name, @Flag, @ImageId );";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<CommentModel> GetCommentId(int Id)
        {

            string sql = @"select *
                        from dbo.Comment
                        Where Id = @Id;";

            return SqlDataAccess.LoadData<CommentModel>(sql);
        }

        public static List<CommentModel> LoadComment()
        {

            string sql = @"select Id, Comment, Name, Flag, ImageId
                        from dbo.Comment;";

            return SqlDataAccess.LoadData<CommentModel>(sql);
        }

        public static int DeleteComment(int Id)
        {
            CommentModel data = new CommentModel
            {
                Id = Id

            };

            string sql = @"DELETE FROM dbo.Comment WHERE Id= @Id;";

            return SqlDataAccess.SaveData(sql, data);

        }
    }
}
