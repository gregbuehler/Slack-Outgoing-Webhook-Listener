using FluentAssertions;
using MarioWebService.Enums;
using MarioWebService.Mappers;
using MarioWebService.Models;
using NUnit.Framework;
namespace Tests
{
    [TestFixture]
    public class SlackRequestMapperTests
    {
        [Test]
        [TestCase("add tasks 123", "AAA", "thedoors", CommandType.AddTasks, "123")]
        [TestCase("add tasks #123", "AAA", "thedoors", CommandType.AddTasks, "123")]
        [TestCase("add tasks     #123  ", "AAA", "thedoors", CommandType.AddTasks, "123")]
        [TestCase("help", "AAA", "thedoors", CommandType.Help, "")]
        public void SlashCommandTests(string text, string token, string channel_name, CommandType commandType, string commandText)
        {
            var mapper = new SlackRequestMapper();
            var sc = new SlashCommandRequest
            {
                text = text,
                token = token,
                channel_name = channel_name
            };
            var sr = mapper.Map(sc);
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
            var mapper = new SlackRequestMapper();
            var sc = new OutgoingWebhookRequest
            {
                text = text,
                token = token,
                channel_name = channel_name,
                trigger_word = trigger_word
            };
            var sr = mapper.Map(sc);
            sr.CommandType.ShouldBeEquivalentTo(commandType);
            sr.CommandText.ShouldBeEquivalentTo(commandText);
            sr.ChannelName.ShouldBeEquivalentTo(channel_name);
            sr.AuthorizationToken.ShouldBeEquivalentTo(token);
        }
    }
}
