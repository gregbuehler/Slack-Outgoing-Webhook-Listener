using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;
using SuperMarioPivotalEdition.Clients;

namespace SuperMarioPivotalEdition.Listeners
{
    class SlackListener
    {
        readonly HttpListener _httpListener;
        private readonly RavenDatabaseClient _ravenDatabaseClient;
        private readonly PivotalClient _pivotalClient;
        private readonly FractalClient _fractalClient;
        private readonly BitlyClient _bitlyClient;
        private readonly CatApiClient _catApiClient;
        private readonly ImgurClient _imgurClient;
        private readonly YouTubeClient _youTubeClient;
        private readonly GoogleVisionClient _googleVisionClient;
        private readonly GoogleBooksClient _googleBooksClient;
        private readonly TextBeltClient _textBeltClient;
        private readonly string _slackOutgoingWebhookToken;

        public SlackListener(RavenDatabaseClient ravenDatabaseClient, string serverAddress, string slackOutgoingWebhookToken, string pivotalApiKey, string bitlyApiKey, string catApiKey, string imgurApiKey, string googleApiKey)
        {
            _ravenDatabaseClient = ravenDatabaseClient;
            _pivotalClient = new PivotalClient(pivotalApiKey);
            _fractalClient = new FractalClient();
            _bitlyClient = new BitlyClient(bitlyApiKey);
            _catApiClient = new CatApiClient(catApiKey);
            _imgurClient = new ImgurClient(imgurApiKey);
            _youTubeClient = new YouTubeClient(googleApiKey);
            _googleVisionClient = new GoogleVisionClient(googleApiKey);
            _googleBooksClient = new GoogleBooksClient(googleApiKey);
            _textBeltClient = new TextBeltClient();
            _slackOutgoingWebhookToken = slackOutgoingWebhookToken;
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(serverAddress);
            _httpListener.Start();
        }

        public async void ListenForSlackOutgoingWebhooks()
        {
            while (true)
            {
                var context = await _httpListener.GetContextAsync();
                NameValueCollection form;
                using (var reader = new StreamReader(context.Request.InputStream))
                {
                    var queryString = reader.ReadToEnd();
                    Console.WriteLine($"Slack request received. Contents:\n\n{queryString}\n\n");
                    form = HttpUtility.ParseQueryString(queryString);
                }
                string responseBody;
                try
                {
                    responseBody = ProcessSlackCommand(form);
                }
                catch (Exception ex)
                {
                    responseBody = "SCREAMS OF DEATH";
                    Console.WriteLine(ex.ToString());
                }
                using (var writer = new StreamWriter(context.Response.OutputStream))
                {
                    var json = new JObject(new JProperty("text", responseBody)).ToString();
                    writer.Write(json);
                }
            }
        }

        private string ProcessSlackCommand(NameValueCollection form)
        {
            var triggerWord = form["trigger_word"].ToLower();
            var formTextContent = form["text"].Substring(triggerWord.Length).Trim(' ', '#', ':', '<', '>');
            Console.WriteLine($"formTextContent:{formTextContent}");
            var channel = form["channel_name"];
            var channelInfo = _ravenDatabaseClient.GetSlackChannelInfo(channel);
            var token = form["token"];
            var response = "";
            if (token != _slackOutgoingWebhookToken) return response;
            switch (triggerWord)
            {
                case "help":
                    response = @"_All commands are case-insensitive_:
**Pivotal commands**:
*help* - Displays command help.
*info* - Displays this channel's associated Pivotal info.
*set project id 123* - Sets this channel's associated Pivotal Project ID to 123.
*add tasks 12345* - Adds default tasks to story ID 12345.
*add story Giant Beetle* - Creates a new Pivotal issue with name ""Giant Beetle"" with default tasks.
*add default task Check exhaust ports* - Adds a new task to your team's default tasks.
*clear default tasks* - Clears default task list.
*set default tasks from json [""task1"", ""task2""]* - Parses a JSON array and sets it as the default tasks. Useful for setting tasks all at once.

**Random commands**
*random fractal* - Posts a random root-finder fractal.
*add cats 2* - Posts 2 cat pictures. Currently Slack only unfurls at most 3 images per post.
*youtube cats and dogs* - Searches YouTube for ""cats and dogs"" and returns a random video from the top 10 results.
*imgur catnip* - Searches Imgur for ""catnip"" and returns a random image from the top 50-ish results.
*google books blastoise*: Searches Google Books for a random book excerpt containing ""Blastoise"".
*google vision [URL of some image]* - Displays a barchart of Google Cloud Vision's interpretation of the most likely features it thinks are in the image.
*send text 5033071525 I'd like a cheeseburger* - Sends a text message to the phone number.";
                    break;
                case "info":
                    response = $"```{channelInfo}```";
                    break;
                case "add story":
                    response = _pivotalClient.PostStory(channelInfo, formTextContent).ShortResponseMessage;
                    break;
                case "add tasks":
                    var allSucceeded = _pivotalClient.PostDefaultTasks(channelInfo, formTextContent).Aggregate(true, (b, resp) => b && resp.IsSuccessful);
                    response = allSucceeded ? "Default tasks added." : "Error adding tasks.";
                    break;
                case "add default task":
                    channelInfo.DefaultTaskDescriptions.Add(formTextContent);
                    _ravenDatabaseClient.UpdateSlackChannelInfo(channelInfo);
                    response = "New default task added.";
                    break;
                case "clear default tasks":
                    channelInfo.DefaultTaskDescriptions = new List<string>();
                    _ravenDatabaseClient.UpdateSlackChannelInfo(channelInfo);
                    response = "Default task list cleared.";
                    break;
                case "set project id":
                    channelInfo.PivotalProjectId = formTextContent;
                    _ravenDatabaseClient.UpdateSlackChannelInfo(channelInfo);
                    response = $"Pivotal project ID set to {formTextContent}.";
                    break;
                case "set default tasks from json":
                    var jarray = JArray.Parse(formTextContent);
                    var taskList = jarray.ToObject<List<string>>();
                    channelInfo.DefaultTaskDescriptions = taskList;
                    _ravenDatabaseClient.UpdateSlackChannelInfo(channelInfo);
                    response = $"Default tasks set to:```{jarray}```";
                    break;
                case "random fractal":
                    response = _bitlyClient.ShortenUrl(_fractalClient.RandomFractal());
                    break;
                case "add cats":
                    var numCats = int.Parse(formTextContent);
                    var catRes = _catApiClient.GetCats(numCats);
                    response = catRes.data.images.Aggregate("", (s, image) => s + _bitlyClient.ShortenUrl(image.url) + "\n").Trim();
                    break;
                case "youtube":
                    response = _youTubeClient.SearchForRandom(formTextContent);
                    break;
                case "imgur":
                    response = _bitlyClient.ShortenUrl(_imgurClient.SearchForRandom(formTextContent));
                    break;
                case "google books":
                    response = _googleBooksClient.SearchForAndReturnRandomTextSnippet(formTextContent);
                    break;
                case "google vision":
                    var barchart = _googleVisionClient.AnnotateAndReturnUrlOfBarchart(formTextContent);
                    response = _bitlyClient.ShortenUrl(barchart);
                    break;
                case "send text":
                    var temp = formTextContent?.Split(new [] {' '}, 2);
                    var phoneNumber = temp[0].Trim();
                    var messageText = temp[1].Trim();
                    response = _textBeltClient.SendMessage(phoneNumber, messageText);
                    break;
            }
            return response;
        }
    }
}