using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarioPivotalEdition
{
    class GoogleCalendarClient
    {
        static HttpClient client = new HttpClient { BaseAddress = new Uri("https://www.googlecalendarORSOMETHINGLIKETHAT.com") };
        static string apikey = "INSERT YOUR KEY HERE";

    }
}
