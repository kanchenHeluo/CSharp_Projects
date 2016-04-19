DROP EXTERNAL TABLE dbo.synctable
DROP EXTERNAL TABLE dbo.synctable2
DROP EXTERNAL DATA SOURCE  MyElasticDBQueryDataSrc
DROP DATABASE SCOPED CREDENTIAL ElasticDBQueryCred;  
DROP MASTER KEY; 

CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'C!0udcrm@((0unt';

CREATE DATABASE SCOPED CREDENTIAL ElasticDBQueryCred
WITH IDENTITY = 'sqlsa',
SECRET = 'C!0udcrm@((0unt';

CREATE EXTERNAL DATA SOURCE MyElasticDBQueryDataSrc WITH
  (TYPE = RDBMS,
  LOCATION = 'azcrmintucnsql1.database.windows.net',
  DATABASE_NAME = 'UtilityCRM_FeedstoreStaging_2016-02-05T04-02Z',
  CREDENTIAL = ElasticDBQueryCred
);

CREATE EXTERNAL TABLE [dbo].[SyncTable]
( 
	[Id] BIGINT , 
	[ActionCode] INT ,
    [FSTableId] INT, 
    [AppId] INT, 
    [RecordId] INT, 
	[RecordGuid] UNIQUEIDENTIFIER,
    [MessageText] NVARCHAR(MAX), 
    [LogStatusCode] INT, 
    [LogCreateOn] DATETIME,
)
WITH ( 
DATA_SOURCE = MyElasticDBQueryDataSrc,
SCHEMA_NAME = N'crmazure',
OBJECT_NAME = N'logmsg'  
) 

MERGE crmazure.logmsg  AS T
USING 
(SELECT *
FROM dbo.[SyncTable]
)AS R
ON T.Id = R.Id
/*
WHEN NOT MATCHED THEN
insert (  
	[ActionCode],
    [FSTableId], 
    [AppId], 
    [RecordId], 
	[RecordGuid],
    [MessageText], 
    [LogStatusCode], 
    [LogCreatedOn]        
)VALUES
(  
	R.[ActionCode],
    R.[FSTableId], 
    R.[AppId], 
    R.[RecordId], 
	R.[RecordGuid],
    R.[MessageText], 
    R.[LogStatusCode], 
    R.[LogCreateOn]
)*/
WHEN MATCHED THEN
	UPDATE SET
    [LogCreatedOn] = R.[LogCreateOn];   
		
CREATE EXTERNAL TABLE [dbo].[SyncTable2]
( 
	[Id] BIGINT , 
	[ActionCode] INT ,
    [FSTableId] INT, 
    [AppId] INT, 
    [RecordId] INT, 
	[RecordGuid] UNIQUEIDENTIFIER,
    [MessageText] NVARCHAR(MAX),      
    [LogCreateOn] DATETIME,
    [Retry] INT
)
WITH ( 
DATA_SOURCE = MyElasticDBQueryDataSrc,
SCHEMA_NAME = N'crmazure',
OBJECT_NAME = N'logerror'  
) 

MERGE crmazure.logerror  AS T
USING 
(SELECT *
FROM dbo.[SyncTable2]
)AS R
ON T.Id = R.Id
/*
WHEN NOT MATCHED THEN
insert (   
	[ActionCode],
    [FSTableId], 
    [AppId], 
    [RecordId], 
	[RecordGuid],
    [MessageText],      
    [LogCreatedOn],
    [Retry]       
)VALUES
(  
	R.[ActionCode],
    R.[FSTableId], 
    R.[AppId], 
    R.[RecordId], 
	R.[RecordGuid],
    R.[MessageText],      
    R.[LogCreatedOn],
    R.[Retry]
)*/
WHEN MATCHED THEN
	UPDATE SET
    [LogCreatedOn] = R.[LogCreateOn];  
	

