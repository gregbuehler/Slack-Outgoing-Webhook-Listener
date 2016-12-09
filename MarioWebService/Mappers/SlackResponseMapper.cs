using System;
using System.Linq;
using MarioWebService.Models;

namespace MarioWebService.Mappers
{
    public interface ISlackResponseMapper
    {
        SlashCommandResponse MapToSlashCommandResponse(SlackResponse slackResponse);
        OutgoingWebhookResponse MapToOutgoingWebhookResponse(SlackResponse slackResponse);
    }

    public class SlackResponseMapper : ISlackResponseMapper
    {
        public SlashCommandResponse MapToSlashCommandResponse(SlackResponse slackResponse)
        {
            return new SlashCommandResponse
            {
                Text = slackResponse.SuppressMessageTextOnSlashCommandResponse ? "" : slackResponse.Text,
                ResponseType = slackResponse.ResponseType,
                Attachments = slackResponse.Attachments
            };
        }

        public OutgoingWebhookResponse MapToOutgoingWebhookResponse(SlackResponse slackResponse)
        {
            return new OutgoingWebhookResponse
            {
                Text = slackResponse.Text
            };
        }
    }
}