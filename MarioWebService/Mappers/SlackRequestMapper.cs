using System;
using log4net;
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
        private readonly ILog _logger = LogManager.GetLogger(typeof(SlackRequestMapper));

        public SlackRequest Map(SlashCommandRequest slashCommandRequest)
        {
            foreach (CommandType commandType in Enum.GetValues(typeof(CommandType)))
            {
                var description = commandType.GetDescription();
                var i = slashCommandRequest.text.IndexOf(description, StringComparison.CurrentCultureIgnoreCase);
                if (i > -1)
                {
                    return new SlackRequest
                    {
                        CommandType = commandType,
                        AuthorizationToken = slashCommandRequest.token,
                        CommandText = slashCommandRequest.text.Substring(i + description.Length).Trim(' ', '#', ':', '<', '>'),
                        ChannelName = slashCommandRequest.channel_name
                    };
                }
            }
            _logger.Error($"Unable to find a command in slash command text\n{slashCommandRequest.text}.");
            throw new Exception();
        }

        public SlackRequest Map(OutgoingWebhookRequest outgoingWebhookRequest)
        {
            CommandType commandType;
            var bParse = Enum.TryParse(outgoingWebhookRequest.trigger_word.Replace(" ", ""), false, out commandType);
            if (bParse)
                return new SlackRequest
                {
                    CommandType = commandType,
                    CommandText =
                        outgoingWebhookRequest.text.Substring(outgoingWebhookRequest.trigger_word.Length)
                            .Trim(' ', '#', ':', '<', '>'),
                    AuthorizationToken = outgoingWebhookRequest.token,
                    ChannelName = outgoingWebhookRequest.channel_name
                };
            _logger.Error($"Unable to parse trigger word\n{outgoingWebhookRequest.trigger_word}\ninto a command type.");
            throw new Exception();
        }
    }
}