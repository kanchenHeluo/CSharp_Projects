/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF NOT EXISTS(SELECT * FROM [CRMAzure].[MetaActionType])
BEGIN
	INSERT INTO [CRMAzure].[MetaActionType] (Id, Name) values (0, 'insert');
	INSERT INTO [CRMAzure].[MetaActionType] (Id, Name) values (1, 'update');
	INSERT INTO [CRMAzure].[MetaActionType] (Id, Name) values (2, 'inactive');
	INSERT INTO [CRMAzure].[MetaActionType] (Id, Name) values (3, 'active');
END
GO

IF NOT EXISTS(SELECT * FROM [CRMAzure].[MetaLogStatus])
BEGIN
INSERT INTO [CRMAzure].[MetaLogStatus] (Id, Name) values (0, 'CRM Exec Failed');
INSERT INTO [CRMAzure].[MetaLogStatus] (Id, Name) values (1, 'CRM Exec Succeed');
INSERT INTO [CRMAzure].[MetaLogStatus] (Id, Name) values (2, 'CRM Exec Exception');
END
GO

/*
IF NOT EXISTS(SELECT * FROM [CRMAzure].[MetaApp] WHERE Id = 1)
BEGIN
INSERT INTO [CRMAzure].[MetaApp] (Id, AppName, AppGUIDColID, ActiveStatus, ContactTBEnabled, AccountTBEnabled, OrdersTBEnabled)
VALUES (1, 'ESIT', 'App1Guid', 0, 1, 1, 1)
END
GO

IF NOT EXISTS(SELECT * FROM [CRMAzure].[MetaApp] WHERE Id = 2)
BEGIN
INSERT INTO [CRMAzure].[MetaApp] 
(Id, AppName, AppGUIDColID, ActiveStatus, ContactTBEnabled, AccountTBEnabled, OrdersTBEnabled)
VALUES (2,'MPC','App2Guid', 1,1,0,0)
END
GO
*/

IF NOT EXISTS(SELECT * FROM [CRMAzure].[MetaFeedStore])
BEGIN
INSERT INTO [CRMAzure].[MetaFeedStore] (FSTableId, FSTableName, FSTableKey, FSTableSecKey) values (1, 'Contact', 'PersonnelNumber', 'emailaddress1');
INSERT INTO [CRMAzure].[MetaFeedStore] (FSTableId, FSTableName, FSTableKey, FSTableSecKey) values (2, 'Account', 'AccountCode','AccountLongDesc');
INSERT INTO [CRMAzure].[MetaFeedStore] (FSTableId, FSTableName, FSTableKey, FSTableSecKey) values (3, 'IO', 'InternalOrderNbr','InternalOrderDesc');
END
GO

IF NOT EXISTS (SELECT TOP 1 * FROM [CRMAzure].IdGuidContact)
BEGIN
	INSERT INTO [CRMAzure].IdGuidContact (PID, SID) 
	SELECT personnelnumber, emailaddress1
	FROM dbo.UtilityContact
END

IF NOT EXISTS (SELECT TOP 1 * FROM [CRMAzure].IdGuidAccount)
BEGIN
	INSERT INTO [CRMAzure].IdGuidAccount (PID, SID) 
	SELECT AccountCode, AccountLongDesc
	FROM dbo.GL04Account
END


IF NOT EXISTS (SELECT TOP 1 * FROM [CRMAzure].IdGuidIO)
BEGIN
	INSERT INTO [CRMAzure].IdGuidIO (PID, SID) 
	SELECT InternalOrderNbr, InternalOrderDesc
	FROM dbo.GL04InternalOrder
END




