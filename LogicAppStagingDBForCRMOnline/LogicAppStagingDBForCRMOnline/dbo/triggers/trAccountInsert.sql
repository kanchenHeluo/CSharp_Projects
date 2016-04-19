CREATE TRIGGER [dbo].[trAccountInsert]
ON [dbo].[GL04Account]
AFTER INSERT
AS
BEGIN
	INSERT INTO [CRMAzure].[DeltaAccount]
	(
		AppId, FSTableId, RecordId, ActionCode, Status, ProcessDate
	)
	SELECT [CRMAzure].[MetaApp].id, 2, inserted.AccountCode, 0, 0, getutcdate()
	FROM inserted 		
	INNER JOIN [CRMAzure].[MetaApp] 
	ON activestatus = 1 and accounttbenabled = 1


	CREATE TABLE #ResponseTable
	(
		SID VARCHAR(MAX), 
		PID INT
	)

	INSERT INTO #ResponseTable
	(
		PID, SID
	)
	SELECT inserted.AccountCode, inserted.AccountLongDesc
	FROM inserted

	MERGE [CRMAzure].[IdGuidAccount] AS T
	USING #ResponseTable AS R
	ON T.PId = R.PID
	WHEN NOT MATCHED THEN
	INSERT (PId, SID) VALUES(R.PID, R.SID);	

	DROP TABLE　#ResponseTable

END