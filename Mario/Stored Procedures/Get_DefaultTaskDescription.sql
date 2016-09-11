CREATE PROCEDURE [dbo].[Get_DefaultTaskDescription]
	@SlackChannelName VARCHAR(50)
AS
	DECLARE @SlackChannelId int = (
		SELECT TOP 1
			[sc].[Id] 
		FROM 
			[dbo].[SlackChannel] sc
		WHERE
			[sc].SlackChannelName = @SlackChannelName
		)

	SELECT 
		dtd.[Description]
	FROM 
		[dbo].[DefaultTaskDescription] dtd
	WHERE
		dtd.[SlackChannelId] = @SlackChannelId
	ORDER BY
		dtd.[Id]