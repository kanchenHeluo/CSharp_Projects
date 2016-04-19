
CREATE VIEW [CRMAzure].[vDeltaAccount]
AS 
	SELECT C.*, E.ActionCode, E.Status, E.FSTableId, E.AppId
	FROM [CRMAzure].[DeltaAccount] as E with(NOLOCK)
	INNER JOIN [dbo].[GL04Account] C with(NOLOCK) 
	ON E.RecordId = C.AccountCode
GO
	