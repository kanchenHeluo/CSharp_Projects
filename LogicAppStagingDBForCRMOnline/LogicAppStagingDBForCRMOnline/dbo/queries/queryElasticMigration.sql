DROP EXTERNAL TABLE dbo.SyncOnepayrollTable
DROP EXTERNAL TABLE dbo.SyncLogTable
DROP EXTERNAL DATA SOURCE  ElasticDBSrc
DROP DATABASE SCOPED CREDENTIAL ElasticDBCred;  
DROP MASTER KEY; 


CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'C!0udcrm@((0unt';

CREATE DATABASE SCOPED CREDENTIAL ElasticDBCred
WITH IDENTITY = 'sqlsa',
SECRET = 'C!0udcrm@((0unt';

CREATE EXTERNAL DATA SOURCE ElasticDBSrc WITH
  (TYPE = RDBMS,
  LOCATION = 'azcrmintpcnsq1.database.windows.net',
  DATABASE_NAME = 'UtilityCRM_FeedstoreStaging4onprem',
  CREDENTIAL = ElasticDBCred
);


/*********  tables   *************/
CREATE EXTERNAL TABLE [dbo].[SyncOnepayrollTable]
( 
	[Id]					 INT			  NOT NULL,
	[CRMGuid]				 UNIQUEIDENTIFIER NULL,

	[LogicStatus]            INT              NOT NULL ,
	[Action]				 INT			  NOT NULL ,
	[ProcessDate]            DATETIME         NULL,
	[ResponseDate]           DATETIME         NULL,
	[Retry]					 INT			  NOT NULL ,
	[ResponseText]			 NVARCHAR (400)   NULL,
	[TransactionId]			 INT NULL, 

)
WITH ( 
DATA_SOURCE = ElasticDBSrc,
SCHEMA_NAME = N'dbo',
OBJECT_NAME = N'onepayrollcontact'  
) 

CREATE EXTERNAL TABLE [dbo].[SyncLogTable]
( 
    [MessageId]          INT            NOT NULL,
    [MessageDescription] NVARCHAR (MAX) NOT NULL,
    [AppId]              INT            NULL,
    [RecordType]         NVARCHAR (250) NULL,
	[RecordId]			 INT			NULL,
    [RecordPrimaryKey]   NVARCHAR (50)  NULL,
    [ErrorLevel]         INT            NOT NULL,
    [LogCreatedOn]       DATETIME       NOT NULL,

)
WITH ( 
DATA_SOURCE = ElasticDBSrc,
SCHEMA_NAME = N'dbo',
OBJECT_NAME = N'MessageLog'  
) 



/************ merge data  *******************/
MERGE CRMAzure.IdGuidContact AS T
USING 
(SELECT *
FROM dbo.SyncOnepayrollTable
WHERE crmguid is not null
)AS R
ON T.pid = R.id
WHEN NOT MATCHED THEN
insert (
 pid           
,[app1guid]                     
)	VALUES
(           
R.id           
,R.crmguid       
)
WHEN MATCHED THEN
	UPDATE SET             
[app1guid]           =R.crmguid           
;       



insert into [CRMAzure].[LogMsg](
 
	[ActionCode] ,
    [FSTableId] , 
    [AppId] , 
    [RecordId] , 
	[RecordGuid] ,
    [MessageText] , 
    [LogStatusCode] , 
    [LogCreateOn]                     
)
select	     
case LEFT([MessageDescription], 6) when 'update' then 1 else 0 end,
1,4,
[RecordId], [RecordPrimaryKey],    [MessageDescription] ,
    case [ErrorLevel]  when 8 then 1 else 0 end,
    [LogCreatedOn]            
from [dbo].[SyncLogTable];  




insert into [CRMAzure].[LogError](
 
	 [FSTableId] , 
    [AppId] , 
    [RecordId] ,
	[RecordGuid] , 
	[ActionCode] ,
    [MessageText] , 
    [Retry] , 
    [LogCreateOn]                   
)
select	     
1,4,
[RecordId], [RecordPrimaryKey],
case LEFT([MessageDescription], 6) when 'update' then 1 else 0 end,
    [MessageDescription] ,
    3,
    [LogCreatedOn]            
from [dbo].[SyncLogTable]
where errorlevel<>8
; 

/*****************  check result ***************************/

