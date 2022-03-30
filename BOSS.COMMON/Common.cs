using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOSS.COMMON
{
    public static class Common
    {
        public static string GetAppSettingValue(string masterKey, string sectionKey)
        {
            try
            {
                return HttpHelper.Configuration.GetSection(masterKey).GetSection(sectionKey).Value;
            }
            catch (Exception ex)
            {
                return "";

            }


        }
    }
}
