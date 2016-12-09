using FluentAssertions;
using MarioWebService.Enums;
using MarioWebService.Mappers;
using MarioWebService.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    class JsonSerializationTests
    {
        private readonly CustomJsonConverter _converter = new CustomJsonConverter();

        [Test]
        [TestCase(ResponseType.InChannel, "\"in_channel\"")]
        [TestCase(ResponseType.Ephemeral, "\"ephemeral\"")]
        public void ResponseTypeTests(ResponseType responseType, string stringThatSlackExpects)
        {
            var s = JsonConvert.SerializeObject(responseType, _converter);
            s.ShouldBeEquivalentTo(stringThatSlackExpects);
        }

        [Test]
        [TestCase("some text", "{\"text\":\"some text\"}")]
        public void OutgoingWebhookResponseTests(string text, string stringThatSlackExpects)
        {
            var owr = new OutgoingWebhookResponse
            {
                Text = text
            };
            var s = JsonConvert.SerializeObject(owr, _converter);
            s.ShouldBeEquivalentTo(stringThatSlackExpects);
        }

        [Test]
        [TestCase("some text", ResponseType.InChannel, "{\"response_type\":\"in_channel\",\"text\":\"some text\",\"attachments\":null}")]
        public void SlashCommandResponseTests(string text, ResponseType responseType, string stringThatSlackExpects)
        {
            var owr = new SlashCommandResponse
            {
                Text = text,
                ResponseType = responseType
            };
            var s = JsonConvert.SerializeObject(owr, _converter);
            s.ShouldBeEquivalentTo(stringThatSlackExpects);
        }

    }
}