using BOSS.COMMON;
using BOSS.COMMON.Config;
using BOSS.COMMON.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOSS.UTILITIES.Filters
{
    public class BossApiAccessFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var accessId = context.HttpContext.Request.Headers["AccessId"];
            var apiKey = context.HttpContext.Request.Headers["ApiKey"];
            var apiSecret = context.HttpContext.Request.Headers["ApiSecret"];
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();

            var isAuthorized = false;

            if (!string.IsNullOrEmpty(accessId)
                && !string.IsNullOrEmpty(apiKey)
                && !string.IsNullOrEmpty(apiSecret))
            {


                var setting = this.GetBossAPiSettingsByAccessId(accessId);
                if (setting != null)
                {
                    if (setting.ApiKey == apiKey
                        && setting.ApiSecret == apiSecret)
                    {
                        if (setting.AllowedIPs == "*")
                            isAuthorized = true;
                        else
                        {
                            var allowedIPs = setting.AllowedIPs.Replace(" ", "").Split(',').ToList();
                            if (allowedIPs.Contains(ipAddress))
                                isAuthorized = true;
                        }
                    }
                }
            }

            if (!isAuthorized)
                context.Result = new UnauthorizedObjectResult($"Access denied. Invalid access id or key or secret or IP address.");

            base.OnActionExecuting(context);
        }


        private ApiSettings GetBossAPiSettingsByAccessId(string accessID)
        {
            var apiAccessSettings = new ApiSettings();
            if (string.IsNullOrEmpty(accessID)) return apiAccessSettings;

            string bossApiAccessId = ConfigurationReader.GetConfigValueByKeys("BossApi", "accessId");

            if (accessID == bossApiAccessId)
            {
                string bossApiKey = ConfigurationReader.GetConfigValueByKeys("BossApi", "key");
                string bossApiSecret = ConfigurationReader.GetConfigValueByKeys("BossApi", "secret");
                apiAccessSettings.AccessID = bossApiAccessId;
                apiAccessSettings.ApiKey = bossApiKey;
                apiAccessSettings.ApiSecret = bossApiSecret;
                apiAccessSettings.AllowedIPs = "*";
            }

            return apiAccessSettings;
        }
    }
}
