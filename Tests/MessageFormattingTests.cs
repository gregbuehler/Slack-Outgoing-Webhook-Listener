using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarioWebService.Formatters;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    class MessageFormattingTests
    {
        [Test]
        [TestCase("http://29.media.tumblr.com/tumblr_lon992p2oO1qkkx5qo1_500.jpg", "http://media.tumblr.com/tumblr_lon992p2oO1qkkx5qo1_500.jpg")]
        public void TumblrUrlNormalizationTest(string url, string expected)
        {
            Assert.AreEqual(TumblrFormatter.NormalizeLoadbalancedUrl(url), expected);
        }
    }
}
