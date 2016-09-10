CREATE PROCEDURE [dbo].[Update_SlackChannel]
	@SlackChannelName nvarchar(50),
	@PivotalProjectId nvarchar(50)
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