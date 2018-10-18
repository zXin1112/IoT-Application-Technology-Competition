using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SchoolService
{
    /// <summary>
    /// WebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        string connectionStr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public List<BookInfo> GetBookInfos(string bookName = "")
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                string sqlCommand = "select * from T_BookInfo where BookName like '%@name%'";

                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    command.Parameters.Add(new SqlParameter("@name", bookName));

                    SqlDataReader reader = command.ExecuteReader();

                    List<BookInfo> infos = new List<BookInfo>();

                    while (reader.Read())
                    {
                        infos.Add(new BookInfo()
                        {
                            BookName = reader.GetString(0).Trim(),
                            BookNo = reader.GetString(1).Trim(),
                            Status = reader.GetString(2).Trim()
                        });
                    }

                    return infos;
                }
            }
        }

        public List<BorrowBook> GetBorrowBooks(string studentNo)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                string sqlCommand = "select * from T_BorrowBook where StudentNo like '%@StuNo%'";

                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    command.Parameters.Add(new SqlParameter("@StuNo", studentNo));

                    SqlDataReader reader = command.ExecuteReader();

                    List<BorrowBook> borrowBooks = new List<BorrowBook>();

                    while (reader.Read())
                    {
                        borrowBooks.Add(new BorrowBook()
                        {
                            BookNo = reader.GetString(0).Trim(),
                            StudentNo = reader.GetString(1).Trim(),
                            AddTime = reader.GetDateTime(2)
                        });
                    }

                    return borrowBooks;
                }
            }
        }

        public bool UpdateStatus(string bookNo, string status)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                string sqlCommand = "update T_BorrowBook set Status=@status where BookNo=@bookNo";

                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    command.Parameters.Add(new SqlParameter("@bookNo", bookNo));
                    command.Parameters.Add(new SqlParameter("@status", status));

                    int result = command.ExecuteNonQuery();

                    if (result == 1)
                        return true;
                    else
                        return false;
                }
            }
        }
    }
}
