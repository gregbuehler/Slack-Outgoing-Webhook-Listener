using System;

namespace MarioWebService.Exceptions
{
    public class SlackRequestMapException : Exception
    {
        private const string DefaultError =
            "Generic error occurred while converting either a slash command or an outgoing webook request into a SlackRequest.";

        public SlackRequestMapException() : base(DefaultError)
        {
        }

        public SlackRequestMapException(string message) : base(message)
        {
        }
    }
}