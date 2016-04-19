CREATE PROCEDURE [dbo].[GenerateAlertInfo]
@alertSTime DATETIME = null,
@alertDuration INT = 300
AS

if @alertSTime is null
begin
set @alertSTime = DateAdd(mi, -30, GetUTCDate())
end

declare @alertD int
set @alertD = @alertDuration*1000
select r.SubscriptionId, r.AppName, ResourceGroupName, m.WorkflowName, m.Entity, r.Status, RecordId, r.CorrelationId, Duration, r.StartTime, r.EndTime, TriggerOutput
from runhistory as r
inner join MetaApp as m
on m.AppName = r.AppName and m.workflowname = r.workflowname
where ((r.status = 'Failed') or (r.status = 'Succeeded' and r.Duration > @alertD)) and r.CreatedOn >= @alertSTime

GO
