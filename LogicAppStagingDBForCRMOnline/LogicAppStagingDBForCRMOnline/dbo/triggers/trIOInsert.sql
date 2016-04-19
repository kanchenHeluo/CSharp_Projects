CREATE TRIGGER [dbo].[trIOInsert]
ON [dbo].[GL04InternalOrder]
AFTER INSERT
AS
BEGIN
	INSERT INTO [CRMAzure].[DeltaIO]
	(
		AppId, FSTableId, RecordId, ActionCode, Status, ProcessDate
	)
	SELECT [CRMAzure].MetaApp.id, 3, inserted.InternalOrderNbr, 0, 0, getutcdate()
	FROM inserted 		
	INNER JOIN [CRMAzure].MetaApp 
	ON activestatus = 1 and orderstbenabled = 1

	CREATE TABLE #ResponseTable
	(
		SID VARCHAR(MAX), 
		PID INT
	)

	INSERT INTO #ResponseTable
	(
		PID, SID
	)
	SELECT inserted.InternalOrderNbr, inserted.InternalOrderDesc
	FROM inserted

	MERGE [CRMAzure].[IdGuidIO] AS T
	USING #ResponseTable AS R
	ON T.PId = R.PID
	WHEN NOT MATCHED THEN
	INSERT (PId, SID) VALUES(R.PID, R.SID);	

	DROP TABLE　#ResponseTable	
END