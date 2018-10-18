using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService
{
    public class BookInfo
    {
        private string bookName;
        private string bookNo;
        private string status;

        public string BookName { get => bookName; set => bookName = value; }
        public string BookNo { get => bookNo; set => bookNo = value; }
        public string Status { get => status; set => status = value; }
    }
}