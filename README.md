# Slack-Outgoing-Webhook-Listener

This is a fairly generic Slack integration that supports both [Slash Commands](https://api.slack.com/slash-commands) and [Slack Outgoing Webhook](https://api.slack.com/outgoing-webhooks). It provides rudimentary integration with various APIs, including Pivotal Tracker, YouTube, Imgur, Google Books/Vision, BitLy, Textbelt, the Cat API, and some crappy fractals.

The primary focus is Pivotal. Pivotal Tracker has no concept of a "story template", and in particular does not have a way of defining "default tasks" for a project (ie, tasks which are always added to a newly-created story, such as development, testing, code reviewing, branch mergers, deploy scheduling, etc.), and this integration allows you to define default tasks and apply them to stories in your queue via a Slack command.

# How it works

You deploy the bot (an ASP.NET service) to a publically-accessible server. Somebody types trigger words or Slash commands into Slack, Slack connects to the service, and the service connects to one of the aforementioned APIs.

The service has two endpoints:

- `.../OutgoingWebhook`
- `.../SlashCommand`

For the Pivotal functionality, each Slack channel can be assigned default tasks and a Pivotal project ID. That way, different teams can define their own default task list. The Slack channel name, Pivotal project ID, and default tasks are persisted to a database (either Raven or SQL).

# Prerequisites

You need these:

- A publically-accessible Windows server
- Slack, and either a Slack Outgoing Webhook or Slash command configured

The following are optional, but obviously you can't use them if you don't have them:

- [Pivotal API key](http://www.pivotaltracker.com/help/articles/api_token/)
- [BitLy API key](https://bitly.com/a/oauth_apps)
- [Cat API key](http://thecatapi.com/api-key-registration.html)
- [Imgur API key](https://api.imgur.com/oauth2/addclient), specifically the `Client-ID` (the `access_token` isn't needed)
- [Google API key](https://console.developers.google.com)
- Your choice of either a [Raven](https://ravendb.net/) or [SQL database](https://www.microsoft.com/en-us/download/details.aspx?id=52679) (needed for persistence of Pivotal info)

# How to use

### Slack-side setup

#### If you want an Outgoing Webhook

1. Create an outgoing webhook, and add the following trigger words: 

	```
help, add story, add tasks, add default task, set default tasks from json, clear default tasks, set project id, info, add cats, random fractal, youtube, imgur, google books, google vision, send text, search repos
	```

2. Put your server name in, along with the outgoing webhook endpoint mentioned in "How It Works". Example: http://mywebsite.com/OutgoingWebhook.

3. Grab the Token.

#### If you want a Slash Command

1. Create a Slash command. Use Method POST.

2. Put your server name in, along with the Slash Command endpoint mentioned in "How It Works". Needs SSL (see Slack's docs on Slash Commands for more info). Example: https://mysecurewebsite.com/SlashCommand.

3. Grab the Token.


###Server-side setup:

1. Deploy the project "MarioWebService" to your server.
2. If you have a SQL database on your server, publish the "Mario" database project to your server. If you're instead opting to use Raven, get the name of the database you want to store info in.
3. Put your API keys and database connection info in the web.config on your server. Obviously don't commit this to source control, unless you enjoy being hacked like a massive loser.
4. To check that it's running, use Postman or similar to send a GET request to either the `SlashCommand` or `OutgoingWebhook` endpoints. You should get a healthcheck response. 

If all this works, then you should be good to use the integration.
