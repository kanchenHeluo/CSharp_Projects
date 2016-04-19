CREATE TRIGGER [dbo].[trIOUpdate]
ON [dbo].[GL04InternalOrder]
AFTER UPDATE
AS
BEGIN
	INSERT INTO  [CRMAzure].[DeltaIO]
	(
		AppId, FSTableId, RecordId, ActionCode, Status, ProcessDate
	)
	SELECT [CRMAzure].MetaApp.id, 3, inserted.InternalOrderNbr, 1, 0, getutcdate()
	FROM inserted 
	INNER JOIN deleted 
	ON deleted.InternalOrderNbr = inserted.InternalOrderNbr
	INNER JOIN [CRMAzure].MetaApp 
	ON activestatus = 1 and orderstbenabled = 1
END
