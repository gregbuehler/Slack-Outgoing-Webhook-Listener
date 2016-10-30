using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ApiIntegrations.Clients
{
    public class TextBeltClient
    {
        private readonly HttpClient _client;

        public TextBeltClient()
        {
            _client = new HttpClient {BaseAddress = new Uri("http://textbelt.com")};
        }

        public string SendMessage(string phoneNumber, string messageText)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return "Error sending text.";
            var values = new Dictionary<string, string>
            {
                {"number", phoneNumber},
                {"message", messageText}
            };
            _client.PostAsync("/text", new FormUrlEncodedContent(values));
            return $"Sent text to {phoneNumber}.";
        }
    }
}