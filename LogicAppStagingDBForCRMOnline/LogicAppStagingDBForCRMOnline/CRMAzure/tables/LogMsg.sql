CREATE TABLE [CRMAzure].[LogMsg]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
	[ActionCode] INT NOT NULL,
    [FSTableId] INT NOT NULL, 
    [AppId] INT NOT NULL, 
    [RecordId] INT NOT NULL, 
	[RecordGuid] UNIQUEIDENTIFIER,
    [MessageText] NVARCHAR(MAX), 
    [LogStatusCode] INT NOT NULL DEFAULT 1, 
    [LogCreatedOn] DATETIME,
    FOREIGN KEY (FSTableId) REFERENCES [CRMAzure].MetaFeedStore(FSTableId),
	FOREIGN KEY (AppId) REFERENCES [CRMAzure].MetaApp(Id),
	FOREIGN KEY (LogStatusCode) REFERENCES [CRMAzure].MetaLogStatus(Id),
	FOREIGN KEY (ActionCode) REFERENCES [CRMAzure].MetaActionType(Id),
)

GO

CREATE NONCLUSTERED INDEX [IX_LogMsg_3Cols] ON [CRMAzure].[LogMsg] ([FSTableId],[AppId],[LogCreatedOn])

GO

CREATE NONCLUSTERED INDEX [IX_LogMsg_4Cols] ON [CRMAzure].[LogMsg] ([FSTableId],[AppId],[LogStatusCode],[LogCreatedOn])

GO

CREATE NONCLUSTERED INDEX [IX_LogMsg_5Cols] ON [CRMAzure].[LogMsg] ([ActionCode],[FSTableId],[AppId],[LogStatusCode],[LogCreatedOn])
