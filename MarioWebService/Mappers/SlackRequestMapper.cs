using MarioWebService.Models;

namespace MarioWebService.Mappers
{
    public interface ISlackRequestMapper
    {
        SlackRequest Map(SlashCommandRequest slashCommandRequest);
        SlackRequest Map(OutgoingWebhookRequest outgoingWebhookRequest);
    }

    public class SlackRequestMapper : ISlackRequestMapper
    {
        public SlackRequest Map(SlashCommandRequest slashCommandRequest)
        {
            return new SlackRequest
            {
                Command = slashCommandRequest.command.Trim().TrimStart('/').ToLower(),
                Token = slashCommandRequest.token,
                Text = slashCommandRequest.text.Trim(' ', '#', ':', '<', '>'),
                ChannelName = slashCommandRequest.channel_name
            };
        }

        public SlackRequest Map(OutgoingWebhookRequest outgoingWebhookRequest)
        {
            return new SlackRequest
            {
                Command = outgoingWebhookRequest.trigger_word.Trim().ToLower(),
                Text = outgoingWebhookRequest.text.Substring(outgoingWebhookRequest.trigger_word.Length).Trim(' ', '#', ':', '<', '>'),
                Token = outgoingWebhookRequest.token,
                ChannelName = outgoingWebhookRequest.channel_name
            };
        }
    }
}