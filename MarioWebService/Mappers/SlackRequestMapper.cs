using System;
using log4net;
using MarioWebService.Enums;
using MarioWebService.Exceptions;
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
                if (i == 0 && (slashCommandRequest.text.Length == description.Length || slashCommandRequest.text[description.Length] == ' '))
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
            if (slashCommandRequest.text.Trim() == "")
            {
                return new SlackRequest
                {
                    CommandType = CommandType.Help,
                    AuthorizationToken = slashCommandRequest.token,
                    CommandText = "",
                    ChannelName = slashCommandRequest.channel_name
                };
            }
            var error = $"Unable to find a command in slash command text\n{slashCommandRequest.text}.";
            _logger.Error(error);
            throw new SlackRequestMapException(error);
        }

        public SlackRequest Map(OutgoingWebhookRequest outgoingWebhookRequest)
        {
            CommandType commandType;
            var bParse = Enum.TryParse(outgoingWebhookRequest.trigger_word.Replace(" ", ""), true, out commandType);
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
            var error = $"Unable to parse trigger word\n{outgoingWebhookRequest.trigger_word}\ninto a command type.";
            _logger.Error(error);
            throw new SlackRequestMapException(error);
        }
    }
}