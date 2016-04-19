CREATE PROCEDURE [CRMAzure].[spClearLog]
AS
	DELETE FROM [CRMAzure].[LogMsg] 
	WHERE [LogCreatedOn] < DATEADD(month, -1, GETUTCDATE()) AND [LogStatusCode] = 1
RETURN @@ROWCOUNT