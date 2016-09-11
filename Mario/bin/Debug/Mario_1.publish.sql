﻿/*
Deployment script for Mario

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "Mario"
:setvar DefaultFilePrefix "Mario"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL13.SQLEXPRESS\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL13.SQLEXPRESS\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Rename refactoring operation with key 2d4e7104-82dd-4312-b919-9c285d5743bb is skipped, element [dbo].[SlackChannels].[Id] (SqlSimpleColumn) will not be renamed to SlackChannelName';


GO
PRINT N'Creating [dbo].[SlackChannel]...';


GO
CREATE TABLE [dbo].[SlackChannel] (
    [SlackChannelName] NVARCHAR (50)  NOT NULL,
    [PivotalProjectId] NVARCHAR (MAX) NULL,
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([SlackChannelName] ASC)
);


GO
PRINT N'Creating [dbo].[FK_DefaultTaskDescription_ToSlackChannel]...';


GO
ALTER TABLE [dbo].[DefaultTaskDescription] WITH NOCHECK
    ADD CONSTRAINT [FK_DefaultTaskDescription_ToSlackChannel] FOREIGN KEY ([SlackChannelId]) REFERENCES [dbo].[SlackChannel] ([Id]);


GO
PRINT N'Creating [dbo].[Update_SlackChannel]...';


GO
CREATE PROCEDURE [dbo].[Update_SlackChannel]
	@SlackChannelName nvarchar(max),
	@PivotalProjectId nvarchar(max)
AS
	MERGE INTO SlackChannel as sc
	USING (values(@SlackChannelName, @PivotalProjectId)) AS X ([SlackChannelName],[PivotalProjectId])
	ON sc.SlackChannelName = X.SlackChannelName
	WHEN NOT MATCHED BY TARGET THEN
	INSERT ([SlackChannelName],[PivotalProjectId])
	VALUES (X.[SlackChannelName],X.[PivotalProjectId]);
GO
-- Refactoring step to update target server with deployed transaction logs

IF OBJECT_ID(N'dbo.__RefactorLog') IS NULL
BEGIN
    CREATE TABLE [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
    EXEC sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
END
GO
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '2d4e7104-82dd-4312-b919-9c285d5743bb')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('2d4e7104-82dd-4312-b919-9c285d5743bb')

GO

GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[DefaultTaskDescription] WITH CHECK CHECK CONSTRAINT [FK_DefaultTaskDescription_ToSlackChannel];


GO
PRINT N'Update complete.';


GO