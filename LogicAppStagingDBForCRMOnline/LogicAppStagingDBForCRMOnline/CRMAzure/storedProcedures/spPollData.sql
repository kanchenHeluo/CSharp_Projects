CREATE PROCEDURE [CRMAzure].[spPollData]
@AppName VARCHAR(MAX),
@TableName VARCHAR(MAX),
@TableKey VARCHAR(20),
@AppGuidCol VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @sqlStr NVARCHAR(500);

	DECLARE @TableID INT
	SELECT @TableID = FSTableId
	FROM [CRMAzure].MetaFeedStore
	WHERE FSTableName = @TableName	

	DECLARE @AppID INT
	SELECT @AppID = Id
	FROM [CRMAzure].MetaApp
	WHERE AppName = @AppName	
	
	DECLARE @random INT 
	SET @random = CAST(CAST(NEWID() AS VARBINARY) AS INT)
	SET @sqlStr = '	
	DECLARE @cid INT	
	SELECT TOP 1 @cid = RecordId FROM [CRMAzure].Delta'+@TableName+'
	WHERE FSTableId = @TableID AND AppId = @AppId AND (Status = 0 OR Status = 3)
	Order By ProcessDate ASC

	UPDATE [CRMAzure].Delta'+@TableName+'
	SET Status = @random, Processdate = getutcdate()
	WHERE RecordId = @cid AND AppId = @AppId'
	EXEC sp_executesql @sqlStr, N'@TableID INT,@AppID INT,@random INT', @TableID, @AppID,@random;

	SET @sqlStr = '
	SELECT Top 1 E.' + @AppGuidCol + ' as CRMPrimaryKey, V.*  
	FROM [CRMAzure].vDelta'+@TableName+' AS V
	LEFT JOIN [CRMAzure].IdGuid'+@TableName+' AS E
	ON V.'+@TableKey+' = E.PId
	WHERE Status = @random and V.AppId = @AppID'
	EXEC sp_executesql @sqlStr, N'@random INT,@AppID INT', @random, @AppID
END
GO