CREATE TABLE [dbo].[DefaultTaskDescription]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SlackChannelId] INT NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_DefaultTaskDescription_ToSlackChannel] FOREIGN KEY ([SlackChannelId]) REFERENCES [SlackChannel]([Id])
)
