CREATE PROCEDURE [dbo].[Get_DefaultTaskDescription]
	@SlackChannelId int
AS
	SELECT 
		dtd.[Description]
	FROM 
		[dbo].[DefaultTaskDescription] dtd
	WHERE
		dtd.[SlackChannelId] = @SlackChannelId
	ORDER BY
		dtd.[Id]