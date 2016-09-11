CREATE PROCEDURE [dbo].[Get_SlackChannel]
	@SlackChannelName VARCHAR(50)
AS
	SELECT TOP 1
		[sc].*
	FROM
		[dbo].[SlackChannel] sc
	WHERE
		[sc].SlackChannelName = @SlackChannelName