CREATE PROCEDURE [CRMAzure].[spMergeInternalOrderGap]
	@sourceTb [CRMAzure].[tpInternalOrderDeltaTableType] readonly
AS

BEGIN
	SET NOCOUNT ON;

	DECLARE @SummaryOfChanges TABLE(Change VARCHAR(20));

	MERGE dbo.GL04InternalOrder AS T
	USING @sourceTb AS S
	ON T.InternalOrderNbr = S.InternalOrderNbr
	WHEN NOT MATCHED THEN
		INSERT (
		[InternalOrderGuid],
		[InternalOrderNbr],
		[InternalOrderStatusCode],
		[GLCompanyCode],
		[InternalOrderDesc],
		[ProfitCenterCode],
		[ProfitCenterEndDate],
		[PhysicalOrderCompleteInd],
		[PhysicalOrderClosedInd],
		[MarkedDeleteInd],
		[InternalOrderReleaseDate],
		[InternalOrderCompletionDate],
		[InternalOrderWorkBeginDate],
		[InternalOrderWorkEndDate],
		[InternalOrderManagerName],
		[CreatedDate],
		[TaxJurisdictionCode])	
		VALUES (
		S.[InternalOrderGuid]      
		,S.[InternalOrderNbr]     
		,S.[InternalOrderStatusCode]
		,S.[GLCompanyCode] 
		,S.[InternalOrderDesc]
		,S.[ProfitCenterCode]
		,S.[ProfitCenterEndDate]
		,S.[PhysicalOrderCompleteInd]
		,S.[PhysicalOrderClosedInd]
		,S.[MarkedDeleteInd]
		,S.[InternalOrderReleaseDate]
		,S.[InternalOrderCompletionDate]
		,S.[InternalOrderWorkBeginDate]
		,S.[InternalOrderWorkEndDate]
		,S.[InternalOrderManagerName]
		,S.[CreatedDate]
		,S.[TaxJurisdictionCode]
		)
	WHEN MATCHED THEN
		update set 
		[InternalOrderGuid]				=S.[InternalOrderGuid]     
		,[InternalOrderStatusCode]		=S.[InternalOrderStatusCode]
		,[GLCompanyCode]				=S.[GLCompanyCode]
		,[InternalOrderDesc]			=S.[InternalOrderDesc]
		,[ProfitCenterCode]				=S.[ProfitCenterCode]
		,[ProfitCenterEndDate]			=S.[ProfitCenterEndDate]
		,[PhysicalOrderCompleteInd]		=S.[PhysicalOrderCompleteInd]
		,[PhysicalOrderClosedInd]		=S.[PhysicalOrderClosedInd]
		,[MarkedDeleteInd]				=S.[MarkedDeleteInd]
		,[InternalOrderReleaseDate]		=S.[InternalOrderReleaseDate]
		,[InternalOrderCompletionDate]	=S.[InternalOrderCompletionDate]
		,[InternalOrderWorkBeginDate]	=S.[InternalOrderWorkBeginDate]
		,[InternalOrderWorkEndDate]		=S.[InternalOrderWorkEndDate]
		,[InternalOrderManagerName]		=S.[InternalOrderManagerName]
		,[CreatedDate]                  =S.[CreatedDate]
		,[TaxJurisdictionCode]          =S.[TaxJurisdictionCode]
	output $action into @SummaryOfChanges;

	select count(1) from @SummaryOfChanges;
END
GO