DROP EXTERNAL TABLE dbo.SyncIdGuidTable
DROP EXTERNAL TABLE dbo.SyncLogTable
DROP EXTERNAL TABLE dbo.SyncErrorTable
DROP EXTERNAL DATA SOURCE  ElasticDBSrc
DROP DATABASE SCOPED CREDENTIAL ElasticDBCred;  
DROP MASTER KEY; 

CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'C!0udcrm@((0unt';

CREATE DATABASE SCOPED CREDENTIAL ElasticDBCred
WITH IDENTITY = 'sqlsa',
SECRET = 'C!0udcrm@((0unt';

-- src db servername
CREATE EXTERNAL DATA SOURCE ElasticDBSrc WITH
  (TYPE = RDBMS,
  LOCATION = 'azcrmintdcnsql2.database.windows.net',
  DATABASE_NAME = 'UtilityCRM_FeedstoreStaging',
  CREDENTIAL = ElasticDBCred
);

/*********  tables   *************/
CREATE EXTERNAL TABLE [dbo].[SyncIdGuidTable]
( 
	[Id] INT NOT NULL,
	[PID] INT NOT NULL, 
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
WITH ( 
DATA_SOURCE = ElasticDBSrc,
SCHEMA_NAME = N'CRMAzure',
OBJECT_NAME = N'IdGuidContact'  
) 

CREATE EXTERNAL TABLE [dbo].[SyncLogTable]
( 
	[Id] BIGINT NOT NULL, 
	[ActionCode] INT NOT NULL,
    [FSTableId] INT NOT NULL, 
    [AppId] INT NOT NULL, 
    [RecordId] INT NOT NULL, 
	[RecordGuid] UNIQUEIDENTIFIER,
    [MessageText] NVARCHAR(max), 
    [LogStatusCode] INT NOT NULL, 
    [LogCreatedOn] DATETIME,

)
WITH ( 
DATA_SOURCE = ElasticDBSrc,
SCHEMA_NAME = N'CRMAzure',
OBJECT_NAME = N'LogMsg'  
) 

CREATE EXTERNAL TABLE [dbo].[SyncErrorTable]
( 
	[Id] BIGINT NOT NULL, 
    [FSTableId] INT NOT NULL, 
    [AppId] INT NOT NULL, 
    [RecordId] INT NOT NULL,
	[RecordGuid] UNIQUEIDENTIFIER, 
	[ActionCode] INT NOT NULL,
    [MessageText] NVARCHAR(max), 
    [Retry] INT NOT NULL, 
    [LogCreatedOn] DATETIME,

)
WITH ( 
DATA_SOURCE = ElasticDBSrc,
SCHEMA_NAME = N'CRMAzure',
OBJECT_NAME = N'LogError'  
)


/************ merge data  *******************/
-- app?Guid 
update O  SET             
O.[app1guid]  = S.[app1guid]  
	from CRMAzure.IdGuidContact O
	inner join [dbo].[SyncIdGuidTable] as S on S.[PID]=O.PID
;   
    
-- appid
declare @appid int
set @appid = 1
insert into [CRMAzure].[LogMsg](
 
	[ActionCode] ,
    [FSTableId] , 
    [AppId] , 
    [RecordId] , 
	[RecordGuid] ,
    [MessageText] , 
    [LogStatusCode] , 
    [LogCreatedOn]                     
)
select	     
[ActionCode] ,
    [FSTableId] , 
    [appid],  
    [RecordId] , 
	[RecordGuid] ,
    [MessageText], 
    [LogStatusCode] , 
    [LogCreatedOn]            
from [dbo].[SyncLogTable]
where appid=@appid;  

insert into [CRMAzure].[LogError](
 
	 [FSTableId] , 
    [AppId] , 
    [RecordId] ,
	[RecordGuid] , 
	[ActionCode] ,
    [MessageText] , 
    [Retry] , 
    [LogCreatedOn]                   
)
select	     
	 [FSTableId] , 
    [appid] , 
    [RecordId] ,
	[RecordGuid] , 
	[ActionCode] ,
    [MessageText] , 
    [Retry] , 
    [LogCreatedOn]       
from [dbo].[SyncErrorTable]
where appid=@appid; 

/*****************  check result ***************************/

