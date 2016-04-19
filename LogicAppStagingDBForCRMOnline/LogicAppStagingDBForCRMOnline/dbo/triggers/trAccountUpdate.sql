CREATE TRIGGER [dbo].[trAccountUpdate]
ON [dbo].[GL04Account]
AFTER UPDATE
AS
BEGIN
	INSERT INTO [CRMAzure].[DeltaAccount]
	(
		AppId, FSTableId, RecordId, ActionCode, Status, ProcessDate
	)
	SELECT [CRMAzure].MetaApp.id, 2, inserted.AccountCode, 1, 0, getutcdate()
	FROM inserted 
	INNER JOIN deleted ON deleted.AccountCode = inserted.AccountCode
	INNER JOIN [CRMAzure].MetaApp 
	ON activestatus = 1 and accounttbenabled = 1
END
