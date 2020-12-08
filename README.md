# ThunderSQLCore
ThunderSQLCore 是  .NET Core 下的一个快速数据访问库。

不是ORM框架，使用ADO.NET原生处理数据库操作，性能更快。

目前支持：
SQL Server
MySQL


## 一、配置。创建配置文件setting.json ##

    
````
{
  "ConnectionStrings": {

    //默认连接配置：
    "Default": {
      "providerName": "System.Data.SqlClient",
      "connectionString": "server=(local);database=Test;uid=sa;pwd=abc123!@#;"
    },
    //MySQL连接配置：
    "MySQLTest": {
      "providerName": "MySql.Data.MySqlClient",
      "connectionString": "server=localhost;port=3306;database=mysql;user=root;password=123!@#abcABC"
    }
  }
}
````
## 二、连接测试
## （1）SQLServer连接测试，默认为Default配置 ##

````
			 using (DataContext cmd = new DataContext())
			 {
				string sql = string.Format("select top 1 * from photo_data");
				string n = cmd.GetValue<string>(sql);
			 }
````

## （2）MySql连接测试，默认为MySQL配置 ##

````
			using (DataContext cmd = new DataContext("MySQLTest", Transaction.No))
			{
				string sql = string.Format("SELECT	* FROM USER LIMIT 1");
				string n = cmd.GetValue<string>(sql);
				//Console.WriteLine(n);
			}
````

## 三、数据写入 ##

````
			using (DataContext cmd = new DataContext())
			{
				//int n = cmd.Execute("INSERT INTO Dates VALUES (@0, @1, @2)", 2005, 3, 31);  
				var year = new { Year = 2015, Month = 12, Day = 30 };
				int n = cmd.Execute("insert into Dates values (@Year,@Month,@Day)", year);
				Console.WriteLine(n);
			}
````

## 四、数据读取 ##

````
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
                var list = cmd.All<DateItem>("select * from dates");
                foreach (var item in list)
                {
                    Console.WriteLine(item.Year);
                }

            }
````