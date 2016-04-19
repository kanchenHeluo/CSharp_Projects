CREATE TABLE [dbo].[TriggerHistory]
(
	[Id] BIGINT NOT NULL  IDENTITY(1,1),
    [CorrelationId] UNIQUEIDENTIFIER NOT NULL, 
    [TrackingId] UNIQUEIDENTIFIER, 
    [StartTime] DATETIME, 
    [EndTime] DATETIME, 
    [Input] NVARCHAR(MAX), 
    [Output] NVARCHAR(MAX), 
    [Status] NCHAR(20), 
    [SourceName] NCHAR(50), 
	[Outter] INT DEFAULT 1,
    [CreatedOn] DATETIME NOT NULL default getutcdate(), 
    CONSTRAINT [PK_TriggerHistory] PRIMARY KEY ([Id])
)

GO

CREATE NONCLUSTERED INDEX [IX_TiggerHistory_StartTime4Cols]
ON [dbo].[TriggerHistory] ([StartTime])
