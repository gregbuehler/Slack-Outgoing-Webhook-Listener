using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ApiIntegrations.Clients;
using ApiIntegrations.Models.Pivotal;
using MarioWebService.Data;
using MarioWebService.Enums;
using MarioWebService.Models;
using MarioWebService.Validators;
using Newtonsoft.Json.Linq;

namespace MarioWebService.Action
{
    internal class SlackRequestProcessor
    {
        private readonly BitlyClient _bitlyClient;
        private readonly CatApiClient _catApiClient;
        private readonly IDatabaseClient _databaseClient;
        private readonly Dictionary<CommandType, Func<SlackResponse>> _triggerWordMap;
        private readonly FractalClient _fractalClient;
        private readonly GitHubClient _gitHubClient;
        private readonly GoogleBooksClient _googleBooksClient;
        private readonly GoogleVisionClient _googleVisionClient;
        private readonly ImgurClient _imgurClient;
        private readonly PivotalClient _pivotalClient;
        private readonly TextBeltClient _textBeltClient;
        private readonly YouTubeClient _youTubeClient;
        private SlackChannelInfo _channelInfo;
        private string _formTextContent;
        private readonly RequestValidator _validator;
        private const string UnauthorizedResponse = "UNAUTHORIZED LOSER DETECTED";


        public SlackRequestProcessor()
        {
            _triggerWordMap = new Dictionary<CommandType, Func<SlackResponse>>
            {
                {CommandType.Help, Help},
                {CommandType.Info, Info},
                {CommandType.SetProjectId, SetProjectId},
                {CommandType.AddTasks, AddTasks},
                {CommandType.AddStory, AddStory},
                {CommandType.AddDefaultTask, AddDefaultTask},
                {CommandType.ClearDefaultTasks, ClearDefaultTasks},
                {CommandType.SetDefaultTasksFromJson, SetDefaultTasksFromJson},
                {CommandType.RandomFractal, RandomFractal},
                {CommandType.AddCats, AddCats},
                {CommandType.YouTube, YouTube},
                {CommandType.Imgur, Imgur},
                {CommandType.GoogleBooks, GoogleBooks},
                {CommandType.GoogleVision, GoogleVision},
                {CommandType.SendText, SendText},
                {CommandType.SearchRepos, SearchRepos}
            };
            _databaseClient = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["SqlConnectionString"])
                ? (IDatabaseClient) new SqlDatabaseClient()
                : new RavenDatabaseClient();
            _pivotalClient = new PivotalClient();
            _fractalClient = new FractalClient();
            _bitlyClient = new BitlyClient();
            _catApiClient = new CatApiClient();
            _imgurClient = new ImgurClient();
            _youTubeClient = new YouTubeClient();
            _googleVisionClient = new GoogleVisionClient();
            _googleBooksClient = new GoogleBooksClient();
            _textBeltClient = new TextBeltClient();
            _gitHubClient = new GitHubClient();
            _validator = new RequestValidator();
        }

        public SlackResponse Process(SlackRequest slackRequest)
        {
            var bAuthorized = _validator.IsAuthorized(slackRequest);
            if (!bAuthorized)
            {
                return new SlackResponse
                {
                    Text = UnauthorizedResponse,
                    ResponseType = ResponseType.Ephemeral
                };
            }
            _formTextContent = slackRequest.CommandText;
            _channelInfo = _databaseClient.GetSlackChannelInfo(slackRequest.ChannelName);
            return _triggerWordMap[slackRequest.CommandType]();
        }

        private SlackResponse SearchRepos()
        {
            return new SlackResponse
            {
                Text = $"<{_gitHubClient.GetUrlToCodeSearchOrganizationRepos(_formTextContent)}|Search results.>",
                ResponseType = ResponseType.Ephemeral
            };
        }

        private SlackResponse SendText()
        {
            var temp = _formTextContent?.Split(new[] {' '}, 2);
            var phoneNumber = temp[0].Trim();
            var messageText = temp[1].Trim();
            return new SlackResponse
            {
                Text = _textBeltClient.SendMessage(phoneNumber, messageText),
                ResponseType = ResponseType.InChannel
            };
        }

        private SlackResponse GoogleVision()
        {
            var barchart = _googleVisionClient.AnnotateAndReturnUrlOfBarchart(_formTextContent);
            return new SlackResponse
            {
                Text = _bitlyClient.ShortenUrl(barchart),
                ResponseType = ResponseType.InChannel
            };
        }

        private SlackResponse GoogleBooks()
        {
            return new SlackResponse
            {
                Text = _googleBooksClient.SearchForAndReturnRandomTextSnippet(_formTextContent),
                ResponseType = ResponseType.InChannel
            };
        }

        private SlackResponse Imgur()
        {
            return new SlackResponse
            {
                Text = _bitlyClient.ShortenUrl(_imgurClient.SearchForRandom(_formTextContent)),
                ResponseType = ResponseType.InChannel
            };
        }

        private SlackResponse YouTube()
        {
            return new SlackResponse
            {
                Text = _youTubeClient.SearchForRandom(_formTextContent),
                ResponseType = ResponseType.InChannel
            };
        }

        private SlackResponse AddCats()
        {
            var numCats = int.Parse(_formTextContent);
            if (numCats <= 0 || numCats >= 21)
            {
                return new SlackResponse
                {
                    Text = "Too many cats.",
                    ResponseType = ResponseType.Ephemeral
                };
            }
            var catRes = _catApiClient.GetCats(numCats);
            var random = new Random();
            return new SlackResponse
            {
                SuppressMessageTextOnSlashCommandResponse = true,
                Attachments = catRes.data.images.Select(i => new Attachment
                {
                    title = "",
                    image_url = TweakImageLinks(i.url),
                    color = $"#{random.Next(0x1000000):X6}"
                }).ToList(),
                Text =
                    catRes.data.images.Aggregate("", (s, image) => s + image.url + "\n").Trim(),
                ResponseType = ResponseType.InChannel
            };
        }

        private string TweakImageLinks(string rawUrl)
        {
            var tumblerRegex = new Regex(@"(\d.*\.)media.tumblr", RegexOptions.IgnoreCase);
            return tumblerRegex.Replace(rawUrl, "media.tumblr");
        }

        private SlackResponse RandomFractal()
        {
            return new SlackResponse
            {
                Text = _bitlyClient.ShortenUrl(_fractalClient.RandomFractal()),
                ResponseType = ResponseType.InChannel
            };
        }

        private SlackResponse SetDefaultTasksFromJson()
        {
            var jarray = JArray.Parse(_formTextContent);
            var taskList = jarray.ToObject<List<string>>();
            _channelInfo.DefaultTaskDescriptions = taskList;
            _databaseClient.UpdateSlackChannelInfo(_channelInfo);
            return new SlackResponse
            {
                Text = $"Default tasks set to:```{jarray}```",
                ResponseType = ResponseType.Ephemeral
            };
        }

        private SlackResponse SetProjectId()
        {
            _channelInfo.PivotalProjectId = int.Parse(_formTextContent);
            _databaseClient.UpdateSlackChannelInfo(_channelInfo);
            return new SlackResponse
            {
                Text = $"Pivotal project ID set to {_formTextContent}.",
                ResponseType = ResponseType.Ephemeral
            };
        }

        private SlackResponse ClearDefaultTasks()
        {
            _channelInfo.DefaultTaskDescriptions = new List<string>();
            _databaseClient.UpdateSlackChannelInfo(_channelInfo);
            return new SlackResponse
            {
                Text = "Default task list cleared.",
                ResponseType = ResponseType.Ephemeral
            };
        }

        private SlackResponse AddDefaultTask()
        {
            _channelInfo.DefaultTaskDescriptions.Add(_formTextContent);
            _databaseClient.UpdateSlackChannelInfo(_channelInfo);
            return new SlackResponse
            {
                Text = "New default task added.",
                ResponseType = ResponseType.Ephemeral
            };
        }

        private SlackResponse AddTasks()
        {
            var story = _pivotalClient.GetStoryWithProjectIdSafetyCheck(new Story
            {
                id = int.Parse(_formTextContent),
                project_id = _channelInfo.PivotalProjectId
            });
            var tasks = _channelInfo.DefaultTaskDescriptions.Select(d => new Task {description = d}).ToArray();
            var count = 0;
            foreach (var task in tasks)
                try
                {
                    _pivotalClient.PostTask(story, task);
                    count++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            return new SlackResponse
            {
                Text = count == tasks.Length
                    ? $"<{story.url}|Default tasks added.>"
                    : $"Error addings tasks. <{story.url}|{count} of {tasks.Length} tasks added.>",
                ResponseType = ResponseType.Ephemeral
            };
        }

        private SlackResponse AddStory()
        {
            var url = _pivotalClient.PostStory(new Story
            {
                name = _formTextContent,
                project_id = _channelInfo.PivotalProjectId,
                tasks = _channelInfo.DefaultTaskDescriptions.Select(d => new Task {description = d}).ToArray()
            }).url;
            return new SlackResponse
            {
                Text = $"<{url}|New story created.>",
                ResponseType = ResponseType.Ephemeral
            };
        }

        private SlackResponse Info()
        {
            return new SlackResponse
            {
                Text = $"```{_channelInfo}```",
                ResponseType = ResponseType.Ephemeral
            };
        }
        
        public SlackResponse Help()
        {
            const string genericHelp = "_All commands are case-insensitive_:\n*help* - Displays command help.\n";
            const string pivotalHelp = @"**Pivotal commands**:
*info* - Displays this channel's associated Pivotal info.
*set project id 123* - Sets this channel's associated Pivotal Project ID to 123.
*add tasks 12345* - Adds default tasks to story ID 12345.
*add story Giant Beetle* - Creates a new Pivotal issue with name ""Giant Beetle"" with default tasks.
*add default task Check exhaust ports* - Adds a new task to your team's default tasks.
*clear default tasks* - Clears default task list.
*set default tasks from json [""task1"", ""task2""]* - Parses a JSON array and sets it as the default tasks. Useful for setting tasks all at once." + "\n";
            const string fractalHelp = "*random fractal* - Posts a random root-finder fractal.";
            const string catApiHelp = "*add cats 2* - Posts 2 cat pictures. Currently Slack only unfurls at most 3 images per post.";
            const string youTubeHelp = @"*youtube cats and dogs* - Searches YouTube for ""cats and dogs"" and returns a random video from the top 10 results.";
            const string imgurHelp = @"*imgur catnip* - Searches Imgur for ""catnip"" and returns a random image from the top 50-ish results.";
            const string googleBooksHelp = @"*google books blastoise*: Searches Google Books for a random book excerpt containing ""Blastoise"".";
            const string googleVisionHelp = @"*google vision [URL of some image]* - Displays a barchart of Google Cloud Vision's interpretation of the most likely features it thinks are in the image.";
            const string textBeltHelp = @"*send text 5033071525 I'd like a cheeseburger* - Sends a text message to the phone number.";
            const string gitHubHelp = @"*search repos blah* - Returns a GitHub URL that searches all organization repos code for ""blah"". GitHub removed the easy way to do this in 2013 for performance reasons.";
            var dict = new Dictionary<IClient, string>
            {
                {_pivotalClient, pivotalHelp},
                {_fractalClient, fractalHelp},
                {_catApiClient, catApiHelp},
                {_youTubeClient, youTubeHelp},
                {_imgurClient, imgurHelp},
                {_googleBooksClient, googleBooksHelp},
                {_googleVisionClient, googleVisionHelp},
                {_textBeltClient, textBeltHelp},
                {_gitHubClient, gitHubHelp}
            };
            var helpString = new StringBuilder(genericHelp);
            foreach (var kvp in dict)
            {
                if (kvp.Key.HealthCheck())
                {
                    helpString.AppendLine(kvp.Value);
                }
            }
            return new SlackResponse
            {
                Text = helpString.ToString().Trim(),
                ResponseType = ResponseType.Ephemeral
            };
        }
    }
}