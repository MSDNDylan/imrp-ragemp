using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace IMRP.Discord
{
    public class Bot
    {
        public static string IMRPLogWebHookURL = @"https://discordapp.com/api/webhooks/683754745583894603/JJpNb_nRPxq33R2gHNUeMpjp0PXFeQSvfBPLE_Oi0eo0oHP7iF_FKC9YDbp1xMbJsACU";
        private static readonly HttpClient client = new HttpClient();
        public static async Task SendIMRPLogAsync(string message)
        {
            var values = new Dictionary<string, string>
            {
                { "content", message }
            };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(IMRPLogWebHookURL, content);
            var responseString = await response.Content.ReadAsStringAsync();
        }
    }
}
