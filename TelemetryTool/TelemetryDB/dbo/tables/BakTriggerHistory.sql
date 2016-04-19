CREATE TABLE [dbo].[BakTriggerHistory]
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
	CONSTRAINT [PK_BakTriggerHistory] PRIMARY KEY ([Id])
)
