using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MarioWebService.Enums;
using MarioWebService.Mappers;
using MarioWebService.Models;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Tests
{
    [TestFixture]

    class JsonSerializationTests
    {

        [Test]
        [TestCase("add tasks 123", "AAA", "thedoors", CommandType.AddTasks, "123")]
        [TestCase("add tasks #123", "AAA", "thedoors", CommandType.AddTasks, "123")]
        [TestCase("add tasks     #123  ", "AAA", "thedoors", CommandType.AddTasks, "123")]
        [TestCase("help", "AAA", "thedoors", CommandType.Help, "")]
        public void SlashCommandTests(string text, string token, string channel_name, CommandType commandType, string commandText)
        {
            var r = ResponseType.InChannel;
            var s = JsonConvert
        }
    }
}