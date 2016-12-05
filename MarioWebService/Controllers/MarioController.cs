using System.Web.Http;
using MarioWebService.Action;
using MarioWebService.Mappers;
using MarioWebService.Models;
using MarioWebService.Validators;

namespace MarioWebService.Controllers
{
    public class MarioController : ApiController
    {
        private static readonly SlackRequestProcessor Processor = new SlackRequestProcessor();
        private static readonly SlackRequestMapper Mapper = new SlackRequestMapper();
        private static readonly RequestValidator Validator = new RequestValidator();
        private const string ExceptionResponse = "SCREAMS OF DEATH";
        private const string UnauthorizedResponse = "UNAUTHORIZED LOSER DETECTED";

        [HttpPost]
        public string SlashCommand(SlashCommandRequest slashCommandRequest)
        {
            try
            {
                var slackRequest = Mapper.Map(slashCommandRequest);
                var bAuthorized = Validator.IsAuthorized(slackRequest);
                return bAuthorized ? Processor.Process(slackRequest) : UnauthorizedResponse;
                // TODO: Use real authorization that actually returns a 403. And put the auth logic somewhere sensible.
            }
            catch
            {
                return ExceptionResponse;
            }
        }

        [HttpPost]
        public string OutgoingWebhook(OutgoingWebhookRequest outgoingWebhookRequest)
        {
            try
            {
                var slackRequest = Mapper.Map(outgoingWebhookRequest);
                var bAuthorized = Validator.IsAuthorized(slackRequest);
                return bAuthorized ? Processor.Process(slackRequest) : UnauthorizedResponse;
                // TODO: Use real authorization that actually returns a 403. And put the auth logic somewhere sensible.
            }
            catch
            {
                return ExceptionResponse;
            }
        }
    }
}