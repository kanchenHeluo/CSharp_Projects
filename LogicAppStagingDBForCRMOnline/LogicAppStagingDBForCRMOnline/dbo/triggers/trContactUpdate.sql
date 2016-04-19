CREATE TRIGGER [dbo].[trContactUpdate]
ON [dbo].[UtilityContact]
AFTER UPDATE
AS
BEGIN
	INSERT INTO [CRMAzure].[DeltaContact]
	(
		AppId, FSTableId, RecordId, ActionCode, Status, ProcessDate
	)
	SELECT [CRMAzure].MetaApp.id, 1, inserted.personnelnumber, 1, 0, getutcdate()
	FROM inserted 
	INNER JOIN deleted ON deleted.personnelnumber = inserted.personnelnumber
	INNER JOIN [CRMAzure].MetaApp 
	ON activestatus = 1 and contacttbenabled = 1

END
