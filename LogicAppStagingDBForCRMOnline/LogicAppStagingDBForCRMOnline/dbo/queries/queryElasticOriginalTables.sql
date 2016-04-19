DROP EXTERNAL TABLE dbo.synctable
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
  DATABASE_NAME = 'UtilityCRM_FeedstoreStaging4onprem',
  CREDENTIAL = ElasticDBQueryCred
);

CREATE EXTERNAL TABLE [dbo].[SyncTable]
( 
[ContactId]              INT  NOT NULL,
[ContactGuid]            UNIQUEIDENTIFIER NOT NULL,
[personnelnumber]        INT              NOT NULL,
[firstname]              VARCHAR (40)     NULL,
[lastname]               VARCHAR (40)     NULL,
[fullname]               VARCHAR (93)     NULL,
[middlename]             VARCHAR (10)     NULL,
[emailaddress1]          NVARCHAR (450)   NULL,
[telephone1]             VARCHAR (30)     NOT NULL,
[mobilephone]            VARCHAR (30)     NOT NULL,
[jobtitle]               VARCHAR (30)     NOT NULL,
[address1_line1]         VARCHAR (100)    NOT NULL,
[address1_line2]         VARCHAR (50)     NOT NULL,
[address1_line3]         VARCHAR (50)     NOT NULL,
[address1_city]          VARCHAR (30)     NOT NULL,
[address1_county]        VARCHAR (3)      NOT NULL,
[address1_postalcode]    VARCHAR (25)     NOT NULL,
[address1_country]       VARCHAR (75)     NOT NULL,
[costcenter]             VARCHAR (150)    NOT NULL,
[costcenternumber]       INT              NOT NULL,
[costcentercode]         VARCHAR (20)     NOT NULL,
[ms_alias]               VARCHAR (50)     NOT NULL,
[domain]                 VARCHAR (50)     NOT NULL,
[domain_alias]           VARCHAR (100)    NOT NULL,
[StatusCode]             INT              NOT NULL,
[StateCode]              INT              NOT NULL,
[Created]                DATETIME         NULL,
[LastModified]           DATETIME         NULL,
[StatusLastModified]     DATETIME         NULL,
[StateLastModified]      DATETIME         NULL,
[PositionNbr]            VARCHAR (100)    NULL,
[ReportsToPositionNbr]   VARCHAR (100)    NULL,
[secondaryEmailAddress]  NVARCHAR (450)   NULL,
[workingPositionCountry] VARCHAR (75)     NULL,
[RoomNumber]             VARCHAR (6)      NULL,
[BuildingName]           VARCHAR (80)     NULL,
[CompanyCode]            CHAR (4)         NULL,
[CompanyName]            VARCHAR (25)     NULL,
[ManagerFullName]        VARCHAR (93)     NULL,
[OfficeLocation]         NCHAR (450)      NULL,
[FirstRegularHireDate]   DATETIME         NULL,
[FirstMsjvHireDate]      DATETIME         NULL,
[ManagerPersonnelNumber] INT              NULL,
[Manager]                UNIQUEIDENTIFIER NULL,

)
WITH ( 
DATA_SOURCE = MyElasticDBQueryDataSrc,
SCHEMA_NAME = N'dbo',
OBJECT_NAME = N'utilitycontact'  
) 


MERGE UtilityContact AS T
USING 
(SELECT *
FROM dbo.[SyncTable]
WHERE (lastmodified > '2016-2-3')
		or (StateLastModified > '2016-2-3')
)AS R
ON T.personnelnumber = R.personnelnumber
WHEN NOT MATCHED THEN
insert (
 [ContactGuid]           
,[personnelnumber]       
,[firstname]             
,[lastname]              
,[fullname]              
,[middlename]            
,[emailaddress1]         
,[telephone1]            
,[mobilephone]           
,[jobtitle]              
,[address1_line1]        
,[address1_line2]        
,[address1_line3]        
,[address1_city]         
,[address1_county]       
,[address1_postalcode]   
,[address1_country]      
,[costcenter]            
,[costcenternumber]      
,[costcentercode]        
,[ms_alias]              
,[domain]                
,[domain_alias]          
,[StatusCode]            
,[StateCode]             
,[Created]               
,[LastModified]          
,[StatusLastModified]    
,[StateLastModified]     
,[PositionNbr]           
,[ReportsToPositionNbr]  
,[secondaryEmailAddress] 
,[workingPositionCountry]
,[RoomNumber]            
,[BuildingName]          
,[CompanyCode]           
,[CompanyName]           
,[ManagerFullName]       
,[OfficeLocation]        
,[FirstRegularHireDate]  
,[FirstMsjvHireDate]     
,[ManagerPersonnelNumber]
,[Manager]               
)	VALUES
(           
R.[ContactGuid]           
,R.[personnelnumber]       
,R.[firstname]             
,R.[lastname]              
,R.[fullname]              
,R.[middlename]            
,R.[emailaddress1]         
,R.[telephone1]            
,R.[mobilephone]           
,R.[jobtitle]              
,R.[address1_line1]        
,R.[address1_line2]        
,R.[address1_line3]        
,R.[address1_city]         
,R.[address1_county]       
,R.[address1_postalcode]   
,R.[address1_country]      
,R.[costcenter]            
,R.[costcenternumber]      
,R.[costcentercode]        
,R.[ms_alias]              
,R.[domain]                
,R.[domain_alias]          
,R.[StatusCode]            
,R.[StateCode]             
,R.[Created]               
,R.[LastModified]          
,R.[StatusLastModified]    
,R.[StateLastModified]     
,R.[PositionNbr]           
,R.[ReportsToPositionNbr]  
,R.[secondaryEmailAddress] 
,R.[workingPositionCountry]
,R.[RoomNumber]            
,R.[BuildingName]          
,R.[CompanyCode]           
,R.[CompanyName]           
,R.[ManagerFullName]       
,R.[OfficeLocation]        
,R.[FirstRegularHireDate]  
,R.[FirstMsjvHireDate]     
,R.[ManagerPersonnelNumber]
,R.[Manager]
)
WHEN MATCHED THEN
	UPDATE SET             
[ContactGuid]           =R.[ContactGuid]           
,[personnelnumber]       =R.[personnelnumber]       
,[firstname]             =R.[firstname]             
,[lastname]              =R.[lastname]              
,[fullname]              =R.[fullname]              
,[middlename]            =R.[middlename]            
,[emailaddress1]         =R.[emailaddress1]         
,[telephone1]            =R.[telephone1]            
,[mobilephone]           =R.[mobilephone]           
,[jobtitle]              =R.[jobtitle]              
,[address1_line1]        =R.[address1_line1]        
,[address1_line2]        =R.[address1_line2]        
,[address1_line3]        =R.[address1_line3]        
,[address1_city]         =R.[address1_city]         
,[address1_county]       =R.[address1_county]       
,[address1_postalcode]   =R.[address1_postalcode]   
,[address1_country]      =R.[address1_country]      
,[costcenter]            =R.[costcenter]            
,[costcenternumber]      =R.[costcenternumber]      
,[costcentercode]        =R.[costcentercode]        
,[ms_alias]              =R.[ms_alias]              
,[domain]                =R.[domain]                
,[domain_alias]          =R.[domain_alias]          
,[StatusCode]            =R.[StatusCode]            
,[StateCode]             =R.[StateCode]             
,[Created]               =R.[Created]               
,[LastModified]          =R.[LastModified]          
,[StatusLastModified]    =R.[StatusLastModified]    
,[StateLastModified]     =R.[StateLastModified]     
,[PositionNbr]           =R.[PositionNbr]           
,[ReportsToPositionNbr]  =R.[ReportsToPositionNbr]  
,[secondaryEmailAddress] =R.[secondaryEmailAddress] 
,[workingPositionCountry]=R.[workingPositionCountry]
,[RoomNumber]            =R.[RoomNumber]            
,[BuildingName]          =R.[BuildingName]          
,[CompanyCode]           =R.[CompanyCode]           
,[CompanyName]           =R.[CompanyName]           
,[ManagerFullName]       =R.[ManagerFullName]       
,[OfficeLocation]        =R.[OfficeLocation]        
,[FirstRegularHireDate]  =R.[FirstRegularHireDate]  
,[FirstMsjvHireDate]     =R.[FirstMsjvHireDate]     
,[ManagerPersonnelNumber]=R.[ManagerPersonnelNumber]
,[Manager]               =R.[Manager];       

