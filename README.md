# Slack-Outgoing-Webhook-Listener

This is a fairly generic [Slack Outgoing Webhook](https://api.slack.com/outgoing-webhooks) integration, and provides rudimentary integration with various APIs, including Pivotal Tracker, YouTube, Imgur, Google Books/Vision, BitLy, Textbelt, the Cat API, and some crappy fractals.

The primary focus is Pivotal. Pivotal Tracker has no concept of a "story template", and in particular does not have a way of defining "default tasks" for a project (ie, tasks which are always added to a newly-created story, such as development, testing, code reviewing, branch mergers, deploy scheduling, etc.), and this integration allows you to define default tasks and apply them to stories in your queue via a Slack command.

# How it works

You put the bot on a publically-accessible server. Somebody types trigger words into Slack, Slack connects to the bot, and the bot connects to one of the aforementioned APIs.

For the Pivotal functionality, each Slack channel can be assigned default tasks and a Pivotal project ID. That way, different teams can define their own default task list. The Pivotal project ID and default tasks are persisted to a database (either Raven or SQL).

# Prerequisites

You need these:

- A publically-accessible Windows server
- Slack, a Slack Outgoing Webhook (the "bot"), and a Slack Outgoing Webhook token (to make sure the traffic you're getting is really from your Slack team)

The following are optional, but obviously you can't use them if you don't have them:

- [Pivotal API key](http://www.pivotaltracker.com/help/articles/api_token/)
- [BitLy API key](https://bitly.com/a/oauth_apps)
- [Cat API key](http://thecatapi.com/api-key-registration.html)
- [Imgur API key](https://api.imgur.com/oauth2/addclient), specifically the `Client-ID` (the `access_token` isn't needed)
- [Google API key](https://console.developers.google.com)
- Your choice of either a [Raven](https://ravendb.net/) or [SQL database](https://www.microsoft.com/en-us/download/details.aspx?id=52679) (needed for persistence of Pivotal info)

# How to use

### Slack-side setup:

1. Add the following trigger words to your Outgoing Webhook: 

	```
help, add story, add tasks, add default task, set default tasks from json, clear default tasks, set project id, info, add cats, random fractal, youtube, imgur, google books, google vision, send text
	```

2. Put your server name and port in.
3. Grab the Slack Outgoing Webhook Token.

It looks like this:

![](http://i.imgur.com/CTvNpn9.png)

###Server-side setup:

1. Build `SuperMarioPivotalEdition.sln` in Visual Studio.
1. If you have a SQL database on your server, publish the "Mario" database. If you're using Raven, get the name of the database you want to store info in.
2. Copy the `Debug` folder (`...\Slack-Outgoing-Webhook-Listener\SuperMarioPivotalEdition\bin\Debug`) to your server.
3. In the `Debug` folder, open `SuperMarioPivotalEdition.exe.config`, and put your API keys and connection info in the `appSettings` section. Obviously don't commit this to source control, unless you enjoy being hacked like a massive loser.
4. In the `Debug` folder, run `SuperMarioPivotalEdition.exe`. It's now listening for incoming calls.

Type "help" into any public Slack channel, and the bot will post a reply containing instructions on how to use each trigger word.
