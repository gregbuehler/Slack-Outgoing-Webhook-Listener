using System.Configuration;
using MarioWebService.Models;

namespace MarioWebService.Validators
{
    public class RequestValidator
    {
        private static readonly string SlackToken = ConfigurationManager.AppSettings["SlackOutgoingWebhookToken"];

        public bool IsAuthorized(SlackRequest slackRequest)
        {
            var token = slackRequest.AuthorizationToken;
            return !string.IsNullOrWhiteSpace(token) && token == SlackToken;
        }
    }
}