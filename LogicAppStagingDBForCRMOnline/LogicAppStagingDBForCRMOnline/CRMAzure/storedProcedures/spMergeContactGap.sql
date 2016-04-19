﻿CREATE PROCEDURE [CRMAzure].[spMergeContactGap]
	@sourceTb [CRMAzure].[tpContactDeltaTableType] readonly
AS

BEGIN
	SET NOCOUNT ON;

	DECLARE @SummaryOfChanges TABLE(Change VARCHAR(20));

	MERGE dbo.UtilityContact AS T
	USING @sourceTb AS S
	ON T.personnelnumber = S.personnelnumber
	WHEN NOT MATCHED THEN
		INSERT (
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
		,[Manager])	
		VALUES (
		S.[ContactGuid]            
		,S.[personnelnumber]       
		,S.[firstname]             
		,S.[lastname]              
		,S.[fullname]              
		,S.[middlename]            
		,S.[emailaddress1]         
		,S.[telephone1]            
		,S.[mobilephone]           
		,S.[jobtitle]              
		,S.[address1_line1]        
		,S.[address1_line2]        
		,S.[address1_line3]        
		,S.[address1_city]         
		,S.[address1_county]       
		,S.[address1_postalcode]   
		,S.[address1_country]      
		,S.[costcenter]            
		,S.[costcenternumber]      
		,S.[costcentercode]        
		,S.[ms_alias]              
		,S.[domain]                
		,S.[domain_alias]          
		,S.[StatusCode]            
		,S.[StateCode]             
		,S.[Created]               
		,S.[LastModified]          
		,S.[StatusLastModified]    
		,S.[StateLastModified]     
		,S.[PositionNbr]           
		,S.[ReportsToPositionNbr]  
		,S.[secondaryEmailAddress] 
		,S.[workingPositionCountry]
		,S.[RoomNumber]            
		,S.[BuildingName]          
		,S.[CompanyCode]           
		,S.[CompanyName]           
		,S.[ManagerFullName]       
		,S.[OfficeLocation]        
		,S.[FirstRegularHireDate]  
		,S.[FirstMsjvHireDate]     
		,S.[ManagerPersonnelNumber]
		,S.[Manager]
		)
	WHEN MATCHED THEN
		update set 
		[ContactGuid]           =S.[ContactGuid]            
		,[firstname]             =S.[firstname]             
		,[lastname]              =S.[lastname]              
		,[fullname]              =S.[fullname]              
		,[middlename]            =S.[middlename]            
		,[emailaddress1]         =S.[emailaddress1]         
		,[telephone1]            =S.[telephone1]            
		,[mobilephone]           =S.[mobilephone]           
		,[jobtitle]              =S.[jobtitle]              
		,[address1_line1]        =S.[address1_line1]        
		,[address1_line2]        =S.[address1_line2]        
		,[address1_line3]        =S.[address1_line3]        
		,[address1_city]         =S.[address1_city]         
		,[address1_county]       =S.[address1_county]       
		,[address1_postalcode]   =S.[address1_postalcode]   
		,[address1_country]      =S.[address1_country]      
		,[costcenter]            =S.[costcenter]            
		,[costcenternumber]      =S.[costcenternumber]      
		,[costcentercode]        =S.[costcentercode]        
		,[ms_alias]              =S.[ms_alias]              
		,[domain]                =S.[domain]                
		,[domain_alias]          =S.[domain_alias]          
		,[StatusCode]            =S.[StatusCode]            
		,[StateCode]             =S.[StateCode]             
		,[Created]               =S.[Created]               
		,[LastModified]          =S.[LastModified]          
		,[StatusLastModified]    =S.[StatusLastModified]    
		,[StateLastModified]     =S.[StateLastModified]     
		,[PositionNbr]           =S.[PositionNbr]           
		,[ReportsToPositionNbr]  =S.[ReportsToPositionNbr]  
		,[secondaryEmailAddress] =S.[secondaryEmailAddress] 
		,[workingPositionCountry]=S.[workingPositionCountry]
		,[RoomNumber]            =S.[RoomNumber]            
		,[BuildingName]          =S.[BuildingName]          
		,[CompanyCode]           =S.[CompanyCode]           
		,[CompanyName]           =S.[CompanyName]           
		,[ManagerFullName]       =S.[ManagerFullName]       
		,[OfficeLocation]        =S.[OfficeLocation]        
		,[FirstRegularHireDate]  =S.[FirstRegularHireDate]  
		,[FirstMsjvHireDate]     =S.[FirstMsjvHireDate]     
		,[ManagerPersonnelNumber]=S.[ManagerPersonnelNumber]
		,[Manager]               =S.[Manager]
	output $action into @SummaryOfChanges;

	select count(1) from @SummaryOfChanges;
END
GO