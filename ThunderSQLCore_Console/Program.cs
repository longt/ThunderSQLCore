using System;
using ThunderSQLCore;
using ThunderSQLCore.Runtime;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Dynamic;

namespace ThunderSQLCore_Console
{
    public class DateItem
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {

            #region 1、连接SqlServer数据库
            /*
            using (DataContext cmd = new DataContext())
            {
                string sql = string.Format("select top 1 * from photo_data");
                string n = cmd.GetValue<string>(sql);
            }
            */
            #endregion

            #region 2、连接MySql数据库
            /*
            using (DataContext cmd = new DataContext("MySQLTest", Transaction.No))
            {
                string sql = string.Format("SELECT	* FROM USER LIMIT 1");
                string n = cmd.GetValue<string>(sql);
                //Console.WriteLine(n);
            }
            */
            #endregion

            #region 3、写入测试
            /*
             using (DataContext cmd = new DataContext())
            {
                //int n = cmd.Execute("INSERT INTO Dates VALUES (@0, @1, @2)", 2005, 3, 31);  
                var year = new { Year = 2015, Month = 12, Day = 30 };
                int n = cmd.Execute("insert into Dates values (@Year,@Month,@Day)", year);
                Console.WriteLine(n);
            }
            */
            #endregion

            #region 4、查询
            using (DataContext cmd = new DataContext())
            {
                //（1）直接赋值
                DateItem date1 = cmd.First<DateItem>("select top 1 * from dates");
                Console.WriteLine(date1.Year);

                //（2）动态赋值
                //DateItem date2 = cmd.First<DateItem>("select Year,Month from dates");
                //Console.WriteLine(date2.Year);

                //（3）动态赋值返回最后一条数据，有点神奇
                //dynamic date3 = cmd.First<ExpandoObject>("select Year,Month from dates");
                //Console.WriteLine(date3.Year);

                //（4）返回多个数据
                //var list = cmd.All<DateItem>("select * from dates");
                //foreach (var item in list)
                //{
                //    Console.WriteLine(item.Year);
                //}

            }
            #endregion


        }
    }
}
