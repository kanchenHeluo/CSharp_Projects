CREATE PROCEDURE [CRMAzure].[spMergeAccountGap]
	@sourceTb [CRMAzure].[tpAccountDeltaTableType] readonly
AS

BEGIN
	SET NOCOUNT ON;

	DECLARE @SummaryOfChanges TABLE(Change VARCHAR(20));

	MERGE dbo.GL04Account AS T
	USING @sourceTb AS S
	ON T.AccountCode = S.AccountCode
	WHEN NOT MATCHED THEN
		INSERT (
		[AccountGuid],
		[AccountCode],
		[AccountShortDesc], 
		[AccountLongDesc])	
		VALUES (
		S.[AccountGuid]      
		,S.[AccountCode]     
		,S.[AccountShortDesc]
		,S.[AccountLongDesc] 
		)
	WHEN MATCHED THEN
		update set 
		[AccountGuid]        =S.[AccountGuid]     
		,[AccountShortDesc]  =S.[AccountShortDesc]
		,[AccountLongDesc]   =S.[AccountLongDesc]
	output $action into @SummaryOfChanges;

	select count(1) from @SummaryOfChanges;
END
GO