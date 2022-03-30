
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOSS.COMMON.Config
{
    public static class ConfigurationReader
    {
        private static IConfiguration root;

        static ConfigurationReader()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            root = configurationBuilder.Build();
        }

        public static string GetConfigValueByKey(string key)
        {
            return root.GetSection(key).Value;
        }

        public static string GetConfigValueByKeys(params string[] keys)
        {
            IConfigurationSection section = root.GetSection(keys[0]);

            var len = keys.Length;
            for (var i = 1; i < len; i++)
            {
                section = section.GetSection(keys[i]);
            }

            return section.Value;
        }
    }
}
