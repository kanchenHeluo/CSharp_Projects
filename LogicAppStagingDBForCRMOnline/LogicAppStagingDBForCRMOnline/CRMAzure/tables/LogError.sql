CREATE TABLE [CRMAzure].[LogError]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [FSTableId] INT NOT NULL, 
    [AppId] INT NOT NULL, 
    [RecordId] INT NOT NULL,
	[RecordGuid] UNIQUEIDENTIFIER, 
	[ActionCode] INT NOT NULL,
    [MessageText] NVARCHAR(MAX), 
    [Retry] INT NOT NULL DEFAULT 3, 
    [LogCreatedOn] DATETIME,
	FOREIGN KEY (FSTableId) REFERENCES [CRMAzure].MetaFeedStore(FSTableId),
	FOREIGN KEY (AppId) REFERENCES [CRMAzure].MetaApp(Id),
	FOREIGN KEY (ActionCode) REFERENCES [CRMAzure].MetaActionType(Id),
)

GO

CREATE NONCLUSTERED INDEX [IX_LogError_3Cols] ON [CRMAzure].[LogError] ([FSTableId],[AppId],[LogCreatedOn]) INCLUDE ([RecordId])
