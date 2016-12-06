using FluentAssertions;
using MarioWebService.Mappers;
using MarioWebService.Models;
using NUnit.Framework;
namespace Tests
{
    [TestFixture]
    public class SlackRequestMapperTests
    {
        [Test]
        [TestCase("add tasks 123", "AAA", "thedoors", CommandType.AddTasks)]
        [TestCase("add tasks 123", "AAA", "thedoors", CommandType.AddCats)]
        public void SlashCommandTests(string text, string token, string channel_name, CommandType commandType)
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
        }
    }
}
