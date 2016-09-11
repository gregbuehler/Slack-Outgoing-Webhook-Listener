CREATE PROCEDURE [dbo].[Update_SlackChannel]
	@SlackChannelName nvarchar(50),
	@PivotalProjectId nvarchar(50),
	@Descriptions [dbo].[GenericListOfStrings] READONLY
AS
	MERGE
		dbo.SlackChannel as Target
	USING 
		(values(@SlackChannelName, @PivotalProjectId)) AS Source ([SlackChannelName], [PivotalProjectId])
	ON 
		(Target.[SlackChannelName] = Source.[SlackChannelName])
	WHEN MATCHED THEN
		UPDATE SET
			Target.[PivotalProjectId] = Source.[PivotalProjectId]
	WHEN NOT MATCHED BY TARGET THEN
		INSERT ([SlackChannelName], [PivotalProjectId])
		VALUES (Source.[SlackChannelName], Source.[PivotalProjectId]);
	
	DECLARE @SlackChannelId int =
		(SELECT TOP 1 
			[Id] 
		FROM 
			[dbo].[SlackChannel]
		WHERE
			[SlackChannelName] = @SlackChannelName)
	
	DELETE FROM
		[dbo].[DefaultTaskDescription]
	WHERE 
		[SlackChannelId] = @SlackChannelId

	INSERT INTO 
		[dbo].[DefaultTaskDescription] ([SlackChannelId], [Description])
	SELECT 
		@SlackChannelId, d.Name
	FROM 
		@Descriptions as d