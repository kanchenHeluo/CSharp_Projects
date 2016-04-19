CREATE PROCEDURE [dbo].[spPurgeHistoryData]
	@days int
AS

BEGIN
	SET NOCOUNT ON;
	declare @daterule date;
	set @daterule = cast(DATEADD(DAY, -@days, GETUTCDATE()) as date)
	
	insert into [dbo].[BakRunHistory]
	(
		[SubscriptionId], [AppName], [Entity], [WorkflowName], [WorkflowTypeId], [Status], [StartTime], [EndTime], [Duration], 
		[CorrelationId], [RunName], [TriggerOutput], [RecordId], [URL], [CreatedOn] 
	)
	select [SubscriptionId], [AppName], [Entity], [WorkflowName], [WorkflowTypeId], [Status], [StartTime], [EndTime], [Duration], 
		[CorrelationId], [RunName], [TriggerOutput], [RecordId], [URL], [CreatedOn]
	from RunHistory H
	where H.StartTime < @daterule;

	drop index [dbo].[RunHistory].Idx_RH_SAWS
	drop index [dbo].[RunHistory].[IX_RunHistory_StartTime]

	DELETE FROM RunHistory where StartTime < @daterule;

	CREATE NONCLUSTERED INDEX Idx_RH_SAWS
	ON [dbo].[RunHistory](WorkflowTypeId)
	INCLUDE(SubscriptionId, [AppName], [WorkflowName],[StartTime])
	
	CREATE NONCLUSTERED INDEX [IX_RunHistory_StartTime]
	ON [dbo].[RunHistory] ([StartTime])
	

	insert into [dbo].[BakTriggerHistory]
	(
		[CorrelationId], [TrackingId], [StartTime], [EndTime], [Input], [Output], [Status], [SourceName], [Outter], [CreatedOn] 
	)
	select [CorrelationId], [TrackingId], [StartTime], [EndTime], [Input], [Output], [Status], [SourceName], [Outter], [CreatedOn]
	from TriggerHistory H
	where H.StartTime < @daterule;


drop index TriggerHistory.[IX_TiggerHistory_StartTime4Cols]
DELETE FROM TriggerHistory where StartTime < @daterule;
CREATE NONCLUSTERED INDEX [IX_TiggerHistory_StartTime4Cols]
ON [dbo].[TriggerHistory] ([StartTime])
	
	insert into [dbo].[BakActionHistory]
	(
		[CorrelationId], [TrackingId], [StartTime], [EndTime], [Input], [Output], [Status], [SourceName], [Outter], [CreatedOn] 
	)
	select [CorrelationId], [TrackingId], [StartTime], [EndTime], [Input], [Output], [Status], [SourceName], [Outter], [CreatedOn]
	from ActionHistory H
	where H.StartTime < @daterule;

drop index ActionHistory.[IX_ActionHistory_StartTime4Cols]
DELETE FROM ActionHistory where StartTime < @daterule;

CREATE NONCLUSTERED INDEX [IX_ActionHistory_StartTime4Cols]
ON [dbo].[ActionHistory] ([StartTime])

END



GO


