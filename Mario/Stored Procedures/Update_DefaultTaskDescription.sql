CREATE PROCEDURE [dbo].[Update_DefaultTaskDescription]
	@SlackChannelId int,
	@Description NVARCHAR(max)
AS
	INSERT INTO 
		dbo.DefaultTaskDescription ([dbo].[DefaultTaskDescription].[SlackChannelId], [dbo].[DefaultTaskDescription].[Description])
	VALUES 
		(@SlackChannelId, @Description)