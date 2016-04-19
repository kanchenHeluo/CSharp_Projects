CREATE PROCEDURE [CRMAzure].[spGetDailyReport]
	@servername varchar(50),
	@date DATE = null
AS

SET NOCOUNT ON;
IF @date is null SET @date = GetUTCDate();

IF OBJECT_ID('tempdb.#TempReport') IS NOT NULL
    DROP TABLE #TempReport;

CREATE TABLE #TempReport(
	ServerName varchar(50),
	AppName NCHAR(50) NOT NULL,
	Entity varchar(20),
	TotalRecordCnt INT,
	CreateCnt INT,
	UpdateCnt INT,
	StartTime DateTime,
	Duration bigint,
	EndTime DateTime,
	AvergeSpeed Float,
	FailedRecordCnt INT,
	RetriedSucceeded BIT Default 1
);

declare @appid int;
declare @appname varchar(50);
declare @hascontact bit, @hasaccount bit, @hasio bit;


DECLARE fs_cursor CURSOR FOR
SELECT Id,AppName,ContactTBEnabled,AccountTBEnabled,OrdersTBEnabled FROM CRMAzure.MetaApp where ActiveStatus=1;

OPEN fs_cursor;
-- Perform the first fetch.
FETCH NEXT FROM fs_cursor into @appid,@appname,@hascontact,@hasaccount,@hasio;

-- Check @@FETCH_STATUS to see if there are any more rows to fetch.
WHILE @@FETCH_STATUS = 0
BEGIN
	
	DECLARE @startTime DateTime, @endTime DateTime;
	DECLARE @averageSpeed float;
	DECLARE @totalCnt INT, @createdCnt INT, @updatedCnt INT, @failedRecordCnt INT, @retryCnt int;
	DECLARE @duration float;
	declare @retriedSucceeded bit;

	if @hascontact=1
	begin
		select @totalCnt = count(distinct(RecordId)) from CRMAzure.LogMsg
		where AppId=@appid and FSTableId=1 and LogStatusCode=1 and Cast(LogCreatedOn AS DATE)= @date;
		select @createdCnt = count(distinct(RecordId)) from CRMAzure.LogMsg
		where AppId=@appid and FSTableId=1 and LogStatusCode=1 and ActionCode=0 and Cast(LogCreatedOn AS DATE)= @date;
		select @updatedCnt = count(distinct(RecordId)) from CRMAzure.LogMsg
		where AppId=@appid and FSTableId=1 and LogStatusCode=1 and ActionCode=1 and Cast(LogCreatedOn AS DATE)= @date
		select @failedRecordCnt = count(distinct(RecordId)) from CRMAzure.LogError
		where AppId=@appid and FSTableId=1 and Cast(LogCreatedOn AS DATE)= @date and recordid not in
		(
		select recordid
		from crmazure.logmsg
		where AppId=@appid and FSTableId=1 and Cast(LogCreatedOn AS DATE)= @date and logstatuscode = 1
		)

		select @retryCnt = count(distinct(recordid)) from CRMAzure.LogMsg
		where AppId=@appid and FSTableId=1 and LogStatusCode=1 and Cast(LogCreatedOn AS DATE)= @date
			and recordid in(select RecordId from CRMAzure.LogError where AppId=@appid and FSTableId=1 and Cast(LogCreatedOn AS DATE)= @date);

		select @startTime = min(LogCreatedOn), @endTime = max(LogCreatedOn) from CRMAzure.LogMsg
		where AppId=@appid and FSTableId=1 and Cast(LogCreatedOn AS DATE)= @date;

		if @retryCnt=@failedRecordCnt set @retriedSucceeded=1;
		else set @retriedSucceeded=0;

		set @duration = DATEDIFF(ss, @startTime, @endTime);
		set @averageSpeed = case @totalCnt when 0 then 0 else @duration/@totalCnt end;
		
		Insert into #TempReport (ServerName, AppName, Entity, TotalRecordCnt, CreateCnt, UpdateCnt, StartTime, Duration, EndTime, AvergeSpeed, FailedRecordCnt, RetriedSucceeded)
		values (@servername, @appname, 'Contact', @totalCnt, @createdCnt, @updatedCnt, @startTime, @duration, @endTime, @averageSpeed, @failedRecordCnt, @retriedSucceeded);
	end;

	if @hasaccount=1
	begin
		select @totalCnt = count(distinct(RecordId)) from CRMAzure.LogMsg
		where AppId=@appid and FSTableId=2 and LogStatusCode=1 and Cast(LogCreatedOn AS DATE)= @date;
		select @createdCnt = count(distinct(RecordId)) from CRMAzure.LogMsg
		where AppId=@appid and FSTableId=2 and LogStatusCode=1 and ActionCode=0 and Cast(LogCreatedOn AS DATE)= @date;
		select @updatedCnt = count(distinct(RecordId)) from CRMAzure.LogMsg
		where AppId=@appid and FSTableId=2 and LogStatusCode=1 and ActionCode=1 and Cast(LogCreatedOn AS DATE)= @date
		select @failedRecordCnt = count(distinct(RecordId)) from CRMAzure.LogError
		where AppId=@appid and FSTableId=2 and Cast(LogCreatedOn AS DATE)= @date and recordid not in
		(
		select recordid
		from crmazure.logmsg
		where AppId=@appid and FSTableId=2 and Cast(LogCreatedOn AS DATE)= @date and logstatuscode = 1
		)
		select @retryCnt = count(distinct(recordid)) from CRMAzure.LogMsg
		where AppId=@appid and FSTableId=2 and LogStatusCode=1 and Cast(LogCreatedOn AS DATE)= @date
			and recordid in(select RecordId from CRMAzure.LogError where AppId=@appid and FSTableId=2 and Cast(LogCreatedOn AS DATE)= @date);

		select @startTime = min(LogCreatedOn), @endTime = max(LogCreatedOn) from CRMAzure.LogMsg
		where AppId=@appid and FSTableId=2 and Cast(LogCreatedOn AS DATE)= @date;

		if @retryCnt=@failedRecordCnt set @retriedSucceeded=1;
		else set @retriedSucceeded=0;

		set @duration = DATEDIFF(ss, @startTime, @endTime);
		set @averageSpeed = case @totalCnt when 0 then 0 else @duration/@totalCnt end;

		Insert into #TempReport (ServerName, AppName, Entity, TotalRecordCnt, CreateCnt, UpdateCnt, StartTime, Duration, EndTime, AvergeSpeed, FailedRecordCnt, RetriedSucceeded)
		values (@servername, @appname, 'Account', @totalCnt, @createdCnt, @updatedCnt, @startTime, @duration, @endTime, @averageSpeed, @failedRecordCnt, @retriedSucceeded);
	end;

	if @hasio=1
	begin
		select @totalCnt = count(distinct(RecordId)) from CRMAzure.LogMsg
		where AppId=@appid and FSTableId=3 and LogStatusCode=1 and Cast(LogCreatedOn AS DATE)= @date;
		select @createdCnt = count(distinct(RecordId)) from CRMAzure.LogMsg
		where AppId=@appid and FSTableId=3 and LogStatusCode=1 and ActionCode=0 and Cast(LogCreatedOn AS DATE)= @date;
		select @updatedCnt = count(distinct(RecordId)) from CRMAzure.LogMsg
		where AppId=@appid and FSTableId=3 and LogStatusCode=1 and ActionCode=1 and Cast(LogCreatedOn AS DATE)= @date
		select @failedRecordCnt = count(distinct(RecordId)) from CRMAzure.LogError
		where AppId=@appid and FSTableId=3 and Cast(LogCreatedOn AS DATE)= @date and recordid not in
		(
		select recordid
		from crmazure.logmsg
		where AppId=@appid and FSTableId=3 and Cast(LogCreatedOn AS DATE)= @date and logstatuscode = 1
		)

		select @retryCnt = count(distinct(recordid)) from CRMAzure.LogMsg
		where AppId=@appid and FSTableId=3 and LogStatusCode=1 and Cast(LogCreatedOn AS DATE)= @date
			and recordid in(select RecordId from CRMAzure.LogError where AppId=@appid and FSTableId=3 and Cast(LogCreatedOn AS DATE)= @date);

		select @startTime = min(LogCreatedOn), @endTime = max(LogCreatedOn) from CRMAzure.LogMsg
		where AppId=@appid and FSTableId=3 and Cast(LogCreatedOn AS DATE)= @date;

		if @retryCnt=@failedRecordCnt set @retriedSucceeded=1;
		else set @retriedSucceeded=0;

		set @duration = DATEDIFF(ss, @startTime, @endTime);
		set @averageSpeed = case @totalCnt when 0 then 0 else @duration/@totalCnt end;

		Insert into #TempReport (ServerName, AppName, Entity, TotalRecordCnt, CreateCnt, UpdateCnt, StartTime, Duration, EndTime, AvergeSpeed, FailedRecordCnt, RetriedSucceeded)
		values (@servername, @appname, 'InternalOrder', @totalCnt, @createdCnt, @updatedCnt, @startTime, @duration, @endTime, @averageSpeed, @failedRecordCnt, @retriedSucceeded);
	end;

	FETCH NEXT FROM fs_cursor into @appid,@appname,@hascontact,@hasaccount,@hasio;
END

CLOSE fs_cursor;
DEALLOCATE fs_cursor;


SELECT * FROM #TempReport;


GO






