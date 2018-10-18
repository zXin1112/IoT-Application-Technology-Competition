using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService
{
    public class BorrowBook
    {
        private string bookNo;
        private string studentNo;
        private DateTime addTime;

        public string BookNo { get => bookNo; set => bookNo = value; }
        public string StudentNo { get => studentNo; set => studentNo = value; }
        public DateTime AddTime { get => addTime; set => addTime = value; }
    }
}