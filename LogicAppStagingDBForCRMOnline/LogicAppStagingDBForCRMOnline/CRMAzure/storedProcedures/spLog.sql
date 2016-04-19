
CREATE PROCEDURE [CRMAzure].[spLog]
	@RecordGuid UNIQUEIDENTIFIER, 
    @RecordId INT,
    @LogStatusCode INT,
	@MessageText NVARCHAR(MAX),
	@AppName VARCHAR(MAX),
	@ActionCode INT,
	@FSTableName VARCHAR(MAX)
AS
BEGIN
-- get table name
	DECLARE @FSTableId INT;
	SELECT @FSTableId = FSTableId
	FROM [CRMAzure].MetaFeedStore
	WHERE FSTableName = @FSTableName	
	-- get app name
	DECLARE @AppId INT;
	DECLARE @AppGuidCol VARCHAR(10);
	SELECT @AppId = Id, @AppGuidCol = AppGUIDColID
	FROM [CRMAzure].MetaApp
	WHERE AppName = @AppName	

	INSERT INTO [CRMAzure].LogMsg
	(
		ActionCode, FSTableId, AppId, RecordId, RecordGuid, MessageText, 
		LogStatusCode, LogCreatedOn
	)
	VALUES (@ActionCode, @FSTableId, @AppId, @RecordId, @RecordGuid, @MessageText, @LogStatusCode, getutcdate())
	
	

	DECLARE @sqlStr NVARCHAR(500);
	
	IF @LogStatusCode = 1
	BEGIN	
		SET @sqlStr = 'DELETE FROM [CRMAzure].Delta' + @FSTableName+'
				   WHERE RecordId = @RecordId AND AppId = @AppId AND FSTableId = @FSTableId';

		exec sp_executesql @sqlStr, N'@RecordId INT,@AppId INT,@FSTableId int', @RecordId, @AppId, @FSTableId;

		/*
		CREATE TABLE #ResponseTable2
		(
		   [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		   [RecordGuid] UNIQUEIDENTIFIER, 
		   [RecordId] INT
		)
		INSERT INTO #ResponseTable2
		(
			RecordGuid, RecordId
		)
		VALUES(@RecordGuid, @RecordId)	
		SET @sqlStr = '
		MERGE [CRMAzure].IdGuid' + @AppName +@FSTableName+' AS T
		USING #ResponseTable2 AS R
		ON T.Id = R.RecordId
		WHEN NOT MATCHED THEN
			INSERT (Id, [Guid]) VALUES(R.RecordId, R.RecordGuid);';
		EXEC sp_executesql @sqlStr
		DROP TABLE　#ResponseTable2
		*/
		
		SET @sqlStr = '
		UPDATE CRMAzure.IdGuid' + @FSTableName + ' 
		SET '+ @AppGuidCol + '= @RecordGuid
		WHERE PID =  @RecordId;';
		EXEC sp_executesql @sqlStr, N'@RecordId INT, @RecordGuid UNIQUEIDENTIFIER', @RecordId, @RecordGuid;			
	END
	ELSE
	BEGIN
		SET @sqlStr = '
		UPDATE [CRMAzure].Delta'+@FSTableName+' 
		SET Status = 3, processdate = getutcdate()
		WHERE RecordId = @RecordId AND AppId = @AppId AND FSTableId = @FSTableId';
		EXEC sp_executesql @sqlStr, N'@RecordId INT,@AppId INT,@FSTableId int', @RecordId, @AppId, @FSTableId;

		INSERT INTO [CRMAzure].LOGError (
		ActionCode, FSTableId, AppId, RecordId, RecordGuid, MessageText, 
		Retry, LogCreatedOn)
		VALUES(@ActionCode, @FSTableId, @AppId, @RecordId, @RecordGuid, @MessageText, 3, getutcdate())
	END
	
END
GO