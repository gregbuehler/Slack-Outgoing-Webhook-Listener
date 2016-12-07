﻿using System;
using System.Web.Http;
using log4net;
using MarioWebService.Action;
using MarioWebService.Enums;
using MarioWebService.Mappers;
using MarioWebService.Models;

namespace MarioWebService.Controllers
{
    public class MarioController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MarioController));
        private static readonly SlackRequestProcessor Processor = new SlackRequestProcessor();
        private static readonly SlackRequestMapper RequestMapper = new SlackRequestMapper();
        private static readonly SlackResponseMapper ResponseMapper = new SlackResponseMapper();
        private const string ExceptionResponse = "SCREAMS OF DEATH";

        [HttpPost]
        public SlashCommandResponse SlashCommand(SlashCommandRequest slashCommandRequest)
        {
            try
            {
                var slackRequest = RequestMapper.Map(slashCommandRequest);
                var slackResponse = Processor.Process(slackRequest);
                return ResponseMapper.MapToSlashCommandResponse(slackResponse);
            }
            catch (Exception e)
            {
                Log.Error("Encountered error while processing slash command.", e);
                return new SlashCommandResponse
                {
                    ResponseType = ResponseType.Ephemeral,
                    Text = ExceptionResponse
                };
            }
        }

        [HttpPost]
        public OutgoingWebhookResponse OutgoingWebhook(OutgoingWebhookRequest outgoingWebhookRequest)
        {
            try
            {
                var slackRequest = RequestMapper.Map(outgoingWebhookRequest);
                var slackResponse = Processor.Process(slackRequest);
                return ResponseMapper.MapToOutgoingWebhookResponse(slackResponse);
            }
            catch (Exception e)
            {
                Log.Error("Encountered error while processing outgoing webhook request.", e);
                return new OutgoingWebhookResponse
                {
                    Text = ExceptionResponse
                };
            }
        }
    }
}