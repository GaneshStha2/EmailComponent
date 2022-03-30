
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOSS.COMMON
{
    public static class HttpHelper
    {
        private static IHttpContextAccessor _accessor;
        private static IConfiguration _configuration;
        public static void Configure(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _accessor = httpContextAccessor;
            _configuration = configuration;
        }
        public static HttpContext HttpContext => _accessor.HttpContext;
        public static IConfiguration Configuration => _configuration;
    }
}
