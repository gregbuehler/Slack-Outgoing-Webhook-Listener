using FluentAssertions;
using MarioWebService.Enums;
using MarioWebService.Exceptions;
using MarioWebService.Mappers;
using MarioWebService.Models;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Tests
{
    [TestFixture]
    public class SlackRequestMapperTests
    {
        private static readonly SlackRequestMapper Mapper = new SlackRequestMapper();

        [Test]
        [TestCase("add tasks 123", "AAA", "thedoors", CommandType.AddTasks, "123")]
        [TestCase("add tasks #123", "AAA", "thedoors", CommandType.AddTasks, "123")]
        [TestCase("add tAsks     #123  ", "AAA", "thedoors", CommandType.AddTasks, "123")]
        [TestCase("help", "AAA", "thedoors", CommandType.Help, "")]
        [TestCase(" ", "AAA", "thedoors", CommandType.Help, "")]
        [TestCase(null, "AAA", "thedoors", CommandType.Help, "")]
        [TestCase("google vision http://25.media.tumblr.com/tumblr_lkp44teMYi1qijuhzo1_400.jpg", "AAA", "thedoors", CommandType.GoogleVision, "http://25.media.tumblr.com/tumblr_lkp44teMYi1qijuhzo1_400.jpg")]
        public void SlashCommandTests(string text, string token, string channel_name, CommandType commandType, string commandText)
        {
            var sc = new SlashCommandRequest
            {
                text = text,
                token = token,
                channel_name = channel_name
            };
            var sr = Mapper.Map(sc);
            sr.CommandType.ShouldBeEquivalentTo(commandType);
            sr.CommandText.ShouldBeEquivalentTo(commandText);
            sr.ChannelName.ShouldBeEquivalentTo(channel_name);
            sr.AuthorizationToken.ShouldBeEquivalentTo(token);
        }

        [Test]
        [TestCase("add tasks 123", "AAA", "thedoors", "add tasks", CommandType.AddTasks, "123")]
        [TestCase("add tasks #123", "AAA", "thedoors", "add tasks", CommandType.AddTasks, "123")]
        [TestCase("ADD TASkS     #123  ", "AAA", "thedoors", "add tasks", CommandType.AddTasks, "123")]
        [TestCase("help", "AAA", "thedoors", "help", CommandType.Help, "")]
        public void OutgoingWebhookTests(string text, string token, string channel_name, string trigger_word, CommandType commandType, string commandText)
        {
            var sc = new OutgoingWebhookRequest
            {
                text = text,
                token = token,
                channel_name = channel_name,
                trigger_word = trigger_word
            };
            var sr = Mapper.Map(sc);
            sr.CommandType.ShouldBeEquivalentTo(commandType);
            sr.CommandText.ShouldBeEquivalentTo(commandText);
            sr.ChannelName.ShouldBeEquivalentTo(channel_name);
            sr.AuthorizationToken.ShouldBeEquivalentTo(token);
        }

        [Test]
        [TestCase("adefhbnjhfbe bhehjfber")]
        [TestCase("shelp")]
        [TestCase("helps")]
        public void RejectedSlashCommandTests(string text)
        {
            var sc = new SlashCommandRequest
            {
                text = text
            };
            Assert.Throws<SlackRequestMapException>(() => Mapper.Map(sc));
        }

        [Test]
        [TestCase("adefhbnjhfbe bhehjfber", "adefhbnjhfbe bhehjfber hjubjwehjbf")]
        [TestCase("shelp", "shelp me")]
        [TestCase("helps", "helps me")]
        public void RejectedOutgoingWebhookTests(string triggerWord, string text)
        {
            var sc = new OutgoingWebhookRequest
            {
                text = text,
                trigger_word = triggerWord
            };
            Assert.Throws<SlackRequestMapException>(() => Mapper.Map(sc));
        }

    }
}
