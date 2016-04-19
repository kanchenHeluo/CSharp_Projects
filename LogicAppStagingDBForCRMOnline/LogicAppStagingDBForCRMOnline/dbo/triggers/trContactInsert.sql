CREATE TRIGGER [dbo].[trContactInsert]
ON [dbo].[UtilityContact]
AFTER INSERT
AS
BEGIN
	INSERT INTO [CRMAzure].[DeltaContact]
	(
		AppId, FSTableId, RecordId, ActionCode, Status, ProcessDate
	)
	SELECT [CRMAzure].MetaApp.id, 1, inserted.personnelnumber, 0, 0, getutcdate()
	FROM inserted 		
	INNER JOIN [CRMAzure].MetaApp 
	ON activestatus = 1 and contacttbenabled = 1

	--merge 
	CREATE TABLE #ResponseTable
	(
		SID VARCHAR(MAX), 
		PID INT
	)

	INSERT INTO #ResponseTable
	(
		PID, SID
	)
	SELECT inserted.personnelnumber, inserted.emailaddress1
	FROM inserted

	MERGE [CRMAzure].[IdGuidContact] AS T
	USING #ResponseTable AS R
	ON T.PId = R.PID
	WHEN NOT MATCHED THEN
	INSERT (PId, SID) VALUES(R.PID, R.SID);	

	DROP TABLE　#ResponseTable
END