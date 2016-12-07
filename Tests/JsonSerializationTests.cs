using FluentAssertions;
using MarioWebService.Enums;
using MarioWebService.Mappers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    class JsonSerializationTests
    {

        [Test]
        [TestCase(ResponseType.InChannel, "\"in_channel\"")]
        [TestCase(ResponseType.Ephemeral, "\"ephemeral\"")]
        public void ResponseTypeTests(ResponseType responseType, string nameThatSlackExpects)
        {
            var c = new CustomJsonConverter();
            var s = JsonConvert.SerializeObject(responseType, c);
            s.ShouldBeEquivalentTo(nameThatSlackExpects);
        }

        //[Test]
        //public void OutgoingWebhookResponseTests()
    }
}