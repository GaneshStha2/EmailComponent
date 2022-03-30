
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOSS.COMMON.Logger
{
    public static class ExceptionLogger
    {
        public static string MapPath(string path)
        {
            var Serverpath = Path.Combine(
                (string)AppDomain.CurrentDomain.GetData("WebRootPath"),
                path);
            return Serverpath;
        }
        public static void Write(string text)
        {

            string txt = string.Format("time={0},message={1}", DateTime.Now, text) + Environment.NewLine;

            File.AppendAllText(ExceptionLogger.MapPath("File\\Log.txt"), txt);
        }
        public static void SytemLog(string text)
        {
            string txt = string.Format(" time={0},message={1}", DateTime.Now, text) + Environment.NewLine;
            File.AppendAllText(ExceptionLogger.MapPath("File\\Log.txt"), txt);
        }

    }
}
