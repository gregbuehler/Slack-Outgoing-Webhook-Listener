using System;
using System.Net.Http;

namespace SuperMarioPivotalEdition
{
    class GoogleCalendarClient
    {
        static HttpClient client = new HttpClient { BaseAddress = new Uri("https://www.googlecalendarORSOMETHINGLIKETHAT.com") };
        static string apikey = "INSERT YOUR KEY HERE";

    }
}
