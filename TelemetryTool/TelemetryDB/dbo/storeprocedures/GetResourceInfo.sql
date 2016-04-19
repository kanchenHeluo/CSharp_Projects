
CREATE PROCEDURE [dbo].[GetResourceInfo]
AS
if OBJECT_ID('tempdb..#tempTB') IS NOT NULL
    DROP TABLE #tempTB

create table #tempTB(
	subscriptionid uniqueidentifier,
	appname nvarchar(50),
	workflowname nvarchar(50),
	starttime datetime
)

insert into #tempTB (subscriptionid, appname, workflowname, starttime)
select distinct subscriptionid, appname, workflowname, max(starttime)
from RunHistory
where WorkflowTypeId = 1
group by subscriptionid, appname, workflowname

insert into #tempTB (subscriptionid, appname, workflowname, starttime)
select distinct subscriptionid, appname, workflowname, max(starttime)
from RunHistory
where WorkflowTypeId = 0
group by subscriptionid, appname, workflowname

-- aggregate
if OBJECT_ID('tempdb..#tempTB3') IS NOT NULL
    DROP TABLE #tempTB3

create table #tempTB3(
	subscriptionid uniqueidentifier,
	appname nvarchar(50),
	workflowname nvarchar(50),
	runname nvarchar(30),
	endtime datetime
)

insert into #tempTB3 (subscriptionid, appname, workflowname, runname, endtime)
select distinct t.subscriptionid, t.appname, t.workflowname, runname, r.endtime
from #tempTB as t
inner join RunHistory as r
on t.subscriptionid = r.subscriptionid and t.appname = r.appname and t.workflowname = r.workflowname and t.starttime = r.starttime

if OBJECT_ID('tempdb..#tempTB31') IS NOT NULL
    DROP TABLE #tempTB31


create table #tempTB31(
	subscriptionid uniqueidentifier,
	appname nvarchar(50),
	workflowname nvarchar(50),
	maxEndTime datetime,
)
insert into #tempTB31 (subscriptionid, appname, workflowname, maxEndTime)
select distinct t.subscriptionid, t.appname, t.workflowname, max(endtime)
from #tempTB3 as t
group by t.subscriptionid, t.appname, t.workflowname


create table #tempTB32(
	subscriptionid uniqueidentifier,
	appname nvarchar(50),
	workflowname nvarchar(50),
	runname nvarchar(30)
)

insert into #tempTB32 (subscriptionid, appname, workflowname, runname)
select distinct t.subscriptionid, t.appname, t.workflowname, max(t.runname)
from #tempTB3 as t
inner join #tempTB31 as t1
on t.subscriptionid = t1.subscriptionid and t.appname = t1.appname and t.workflowname = t1.workflowname and t.endtime = t1.maxEndTime
group by t.subscriptionid, t.appname, t.workflowname


select distinct m.subscriptionid, m.appname, m.workflowname, crmResourceUrl, crmSecondKey, entity, e.dbPrimaryKey,resourcegroupname,workflowtypeid, t.runname
from metaapp as m
left join #tempTB32 as t
on m.subscriptionid = t.subscriptionid and m.appname = t.appname and m.workflowname = t.workflowname 
left join metaentity as e
on m.Entity = e.EntityName
where activestatus = 1

GO
