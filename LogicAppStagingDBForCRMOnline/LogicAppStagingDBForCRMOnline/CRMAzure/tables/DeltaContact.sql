CREATE TABLE [CRMAzure].[DeltaContact]
(
	[Id] BIGINT PRIMARY KEY NOT NULL IDENTITY(1,1),
	[AppId] INT NOT NULL,
	[FSTableId] INT NOT NULL,
	[RecordId] INT NOT NULL,
	[ActionCode] INT NOT NULL,
	[Status] INT NOT NULL,
	[ProcessDate]            DATETIME         NULL,
	FOREIGN KEY (FSTableId) REFERENCES [CRMAzure].MetaFeedStore(FSTableId),
	FOREIGN KEY (AppId) REFERENCES [CRMAzure].MetaApp(Id),
	FOREIGN KEY (ActionCode) REFERENCES [CRMAzure].MetaActionType(Id),
)
