
CREATE VIEW [CRMAzure].[vDeltaIO]
AS 
	SELECT C.*, E.ActionCode, E.Status, E.FSTableId, E.AppId
	FROM [CRMAzure].[DeltaIO] as E with(NOLOCK)
	INNER JOIN [dbo].[GL04InternalOrder] C with(NOLOCK) 
	ON E.RecordId = C.InternalOrderNbr
GO