using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOSS.COMMON
{
    public class ServiceResult<t>
    {
        public ResultStatus Status { get; set; }
        public string Message { get; set; }
        public t Data { get; set; }
    }

    public enum ResultStatus
    {
        Ok,
        processError,
        dataBaseError,
        ComError,
        unHandeledError,
        InvalidToken
    }
}
