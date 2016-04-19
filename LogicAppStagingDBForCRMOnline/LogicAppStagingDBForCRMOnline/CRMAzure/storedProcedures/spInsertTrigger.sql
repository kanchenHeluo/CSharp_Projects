CREATE PROCEDURE [CRMAzure].[spInsertTrigger]
@AppId INT
AS
BEGIN
	SET NOCOUNT ON	
	
	DECLARE @CurTime DateTime
	SET @CurTime = getutcdate()	

	DECLARE @sqlStr NVARCHAR(1024);
	
	SET @sqlStr = '
	IF EXISTS(SELECT * FROM [CRMAzure].MetaApp WHERE ActiveStatus = 1 and ContactTBEnabled = 1 and Id = @AppId)
	BEGIN
		INSERT INTO [CRMAzure].DeltaContact (AppId, FSTableId, RecordId, ActionCode, Status, ProcessDate)
		SELECT @AppId, 1, U.personnelnumber, 0, 0, @CurTime
		FROM [dbo].[UtilityContact] AS U	

		MERGE [CRMAzure].[IdGuidContact] AS T
		USING [dbo].[UtilityContact] AS R
		ON T.PId = R.personnelnumber
		WHEN NOT MATCHED THEN
		INSERT (PId, SID) VALUES(R.personnelnumber, R.emailaddress1);	
	END';
	EXEC sp_executesql @sqlStr, N'@AppId INT, @CurTime DateTime', @AppId, @CurTime;
	
	SET @sqlStr = '
	IF EXISTS(SELECT * FROM [CRMAzure].MetaApp WHERE ActiveStatus = 1 and AccountTBEnabled = 1 and Id = @AppId)
	BEGIN
		INSERT INTO [CRMAzure].DeltaAccount (AppId, FSTableId, RecordId, ActionCode, Status, ProcessDate)
		SELECT @AppId, 2, U.AccountCode, 0, 0, @CurTime
		FROM [dbo].[GL04Account] AS U	

		MERGE [CRMAzure].[IdGuidAccount] AS T
		USING [dbo].[GL04Account] AS R
		ON T.PId = R.AccountCode
		WHEN NOT MATCHED THEN
		INSERT (PId, SID) VALUES(R.AccountCode, R.AccountLongDesc);	
	END';
	EXEC sp_executesql @sqlStr, N'@AppId INT, @CurTime DateTime', @AppId, @CurTime;
	
	SET @sqlStr = '
	IF EXISTS(SELECT * FROM [CRMAzure].MetaApp WHERE ActiveStatus = 1 and OrdersTBEnabled = 1 and Id = @AppId)
	BEGIN
		INSERT INTO [CRMAzure].DeltaIO (AppId, FSTableId, RecordId, ActionCode, Status, ProcessDate)
		SELECT @AppId, 3, U.InternalOrderNbr, 0, 0, @CurTime
		FROM [dbo].[GL04InternalOrder] AS U	

		MERGE [CRMAzure].[IdGuidIO] AS T
		USING [dbo].[GL04InternalOrder] AS R
		ON T.PId = R.InternalOrderNbr
		WHEN NOT MATCHED THEN
		INSERT (PId, SID) VALUES(R.InternalOrderNbr, R.InternalOrderDesc);	
	END';
	EXEC sp_executesql @sqlStr, N'@AppId INT, @CurTime DateTime', @AppId, @CurTime;
END


GO


