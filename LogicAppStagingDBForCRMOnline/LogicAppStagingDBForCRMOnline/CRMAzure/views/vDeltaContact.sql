CREATE VIEW [CRMAzure].[vDeltaContact]
AS 
	SELECT C.*, E.ActionCode, E.Status, E.FSTableId, E.AppId
	FROM [CRMAzure].[DeltaContact] as E with(NOLOCK)
	INNER JOIN [dbo].[UtilityContact] C with(NOLOCK) 
	ON E.RecordId = C.personnelnumber
GO

