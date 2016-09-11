CREATE PROCEDURE [dbo].[Delete_DefaultTaskDescription]
	@SlackChannelId int
AS
	DELETE FROM 
		[dbo].DefaultTaskDescription
	WHERE
		[dbo].[DefaultTaskDescription].[Id] = @SlackChannelId