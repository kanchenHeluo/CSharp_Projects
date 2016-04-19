CREATE TABLE [CRMAzure].[MetaApp]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[AppName] VARCHAR(50) NOT NULL,
	[AppGUIDColID] VARCHAR(10) NOT NULL,
	[ActiveStatus] BIT DEFAULT 1,
	[ContactTBEnabled] BIT,
	[AccountTBEnabled] BIT,
	[OrdersTBEnabled] BIT
)
