using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol_Programming.Model
{
    public class OperationResult<T>
    {
        public ResultStatus Status;
        public string Message;
        public T AppendData;
        public string ReturnUrl;
        public Object ErrorData;
    }

    public enum ResultStatus
    {
        Success = 1,
        Failure = 2,
        Exception = 4,
        Unknown = 8
    }
}
