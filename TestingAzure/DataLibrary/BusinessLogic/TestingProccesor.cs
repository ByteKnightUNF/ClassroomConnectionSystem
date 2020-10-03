using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
   public class TestingProccesor
    {
        public static List<TestModel> LoadTest()
        {
            string sql = @"select Id, Name, Date
                            from dbo.TestingTable;";

            return SqlDataAccess.LoadData<TestModel>(sql);
        }
    }
}
