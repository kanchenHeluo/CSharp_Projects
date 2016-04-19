CREATE PROCEDURE [CRMAzure].[spCheckAvailable]
@AppName VARCHAR(MAX),
@TableName VARCHAR(MAX)

AS
BEGIN
	SET NOCOUNT ON;	

	DECLARE @TableID INT

	SELECT @TableID = FSTableId
	FROM [CRMAzure].[MetaFeedStore]
	WHERE FSTableName = @TableName	

	DECLARE @AppID INT

	SELECT @AppID = Id
	FROM [CRMAzure].[MetaApp]
	WHERE AppName = @AppName		

	DECLARE @sqlStr NVARCHAR(500);
	SET @sqlStr = '
	SELECT COUNT(1) 
	FROM [CRMAzure].Delta'+@TableName+'
	WHERE FSTableId = @TableID AND AppId = @AppID AND (Status = 0 or Status = 3)';
	EXEC sp_executesql @sqlStr, N'@TableID INT,@AppID INT', @TableID, @AppID;
END
GO


