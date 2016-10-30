CREATE TABLE [dbo].[SlackChannel]
(
	[Id] INT NOT NULL IDENTITY, 
	[SlackChannelName] NVARCHAR(50) NOT NULL UNIQUE, 
    [PivotalProjectId] INT NULL, 
    PRIMARY KEY ([Id])
)
