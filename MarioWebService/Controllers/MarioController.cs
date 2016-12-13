using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using MarioWebService.Action;
using MarioWebService.Enums;
using MarioWebService.Mappers;
using MarioWebService.Models;
using Newtonsoft.Json;

namespace MarioWebService.Controllers
{
    public class MarioController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MarioController));
        private static readonly SlackRequestProcessor Processor = new SlackRequestProcessor();
        private static readonly SlackRequestMapper RequestMapper = new SlackRequestMapper();
        private static readonly SlackResponseMapper ResponseMapper = new SlackResponseMapper();
        private static readonly HttpClient HttpClient = new HttpClient();
        private const string ExceptionResponse = "SCREAMS OF DEATH";

        [HttpPost]
        public SlashCommandResponse SlashCommand(SlashCommandRequest slashCommandRequest)
        {
            Task.Run(() => ProcessSlashCommand(slashCommandRequest));
            return new SlashCommandResponse
            {
                Text = "Response incoming. Only you should see this.",
                ResponseType = ResponseType.Ephemeral
            };
        }

        [HttpGet]
        public SlashCommandResponse SlashCommand()
        {
            return new SlashCommandResponse
            {
                Text = "Slash Command endpoint is up!",
                ResponseType = ResponseType.Ephemeral
            };
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

        [HttpGet]
        public OutgoingWebhookResponse OutgoingWebhook()
        {
            return new OutgoingWebhookResponse
            {
                Text = "Outgoing Webhook endpoint is up!"
            };
        }


        private static async Task ProcessSlashCommand(SlashCommandRequest slashCommandRequest)
        {
            try
            {
                var slackRequest = RequestMapper.Map(slashCommandRequest);
                var slackResponse = Processor.Process(slackRequest);
                var slashCommandResponse = ResponseMapper.MapToSlashCommandResponse(slackResponse);
                await HttpClient.PostAsync(slashCommandRequest.response_url,
                    new StringContent(JsonConvert.SerializeObject(slashCommandResponse))
                    {
                        Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                    });
            }
            catch (Exception e)
            {
                Log.Error("Encountered error while processing slash command.", e);
                var slashCommandResponse = new SlashCommandResponse
                {
                    ResponseType = ResponseType.Ephemeral,
                    Text = ExceptionResponse
                };
                await HttpClient.PostAsync(slashCommandRequest.response_url,
                    new StringContent(JsonConvert.SerializeObject(slashCommandResponse))
                    {
                        Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                    });
            }
        }
    }
}