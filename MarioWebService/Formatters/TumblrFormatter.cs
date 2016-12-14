using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MarioWebService.Formatters
{
    public static class TumblrFormatter
    {
        public static string NormalizeLoadbalancedUrl(string url)
        {
            var tumblerRegex = new Regex(@"(\d.*\.)media.tumblr", RegexOptions.IgnoreCase);
            return tumblerRegex.Replace(url, "media.tumblr");
        }
    }
}