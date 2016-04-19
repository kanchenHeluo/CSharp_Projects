﻿CREATE TABLE [CRMAzure].[IdGuidContact]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[PID] INT NOT NULL UNIQUE, 
    [SID] NVARCHAR(450) NULL,
	[App1Guid] UNIQUEIDENTIFIER NULL,
	[App2Guid] UNIQUEIDENTIFIER NULL,
	[App3Guid] UNIQUEIDENTIFIER NULL,
	[App4Guid] UNIQUEIDENTIFIER NULL,
	[App5Guid] UNIQUEIDENTIFIER NULL,
	[App6Guid] UNIQUEIDENTIFIER NULL,
	[App7Guid] UNIQUEIDENTIFIER NULL,
	[App8Guid] UNIQUEIDENTIFIER NULL,
	[App9Guid] UNIQUEIDENTIFIER NULL,
	[App10Guid] UNIQUEIDENTIFIER NULL,
	[App11Guid] UNIQUEIDENTIFIER NULL,
	[App12Guid] UNIQUEIDENTIFIER NULL,
	[App13Guid] UNIQUEIDENTIFIER NULL,
	[App14Guid] UNIQUEIDENTIFIER NULL,
	[App15Guid] UNIQUEIDENTIFIER NULL,
	[App16Guid] UNIQUEIDENTIFIER NULL,
	[App17Guid] UNIQUEIDENTIFIER NULL,
	[App18Guid] UNIQUEIDENTIFIER NULL,
	[App19Guid] UNIQUEIDENTIFIER NULL,
	[App20Guid] UNIQUEIDENTIFIER NULL,
	[App21Guid] UNIQUEIDENTIFIER NULL,
	[App22Guid] UNIQUEIDENTIFIER NULL,
	[App23Guid] UNIQUEIDENTIFIER NULL,
)