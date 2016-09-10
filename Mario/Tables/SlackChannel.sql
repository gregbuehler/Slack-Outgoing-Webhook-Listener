CREATE TABLE [dbo].[SlackChannel]
(
	[Id] INT NOT NULL IDENTITY, 
	[SlackChannelName] NVARCHAR(50) NOT NULL UNIQUE, 
    [PivotalProjectId] NVARCHAR(50) NULL, 
    PRIMARY KEY ([Id])
)
