CREATE PROCEDURE [CRMAzure].[spResetStatus]
	@TableName VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @sqlStr NVARCHAR(500);
	SET @sqlStr = 'update [CRMAzure].Delta' + @TableName + ' set Status = 0 
					where Status<>0 and datediff(mi,ProcessDate,getutcdate())>10;'
	EXEC sp_executesql @sqlStr;

END
GO