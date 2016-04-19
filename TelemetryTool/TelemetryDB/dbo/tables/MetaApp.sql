CREATE TABLE [dbo].[MetaApp]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [AppName] NCHAR(50) NOT NULL, 
	[Entity] NCHAR(20) NOT NULL,
    [SubscriptionId] UNIQUEIDENTIFIER NOT NULL, 
    [ResourceGroupName] NCHAR(50) NOT NULL, 
    [WorkflowName] NCHAR(50) NOT NULL, 
	[WorkflowTypeId] INT NOT NULL,
	[CrmResourceUrl] NVARCHAR(MAX) ,
	[CrmSecondKey] NVARCHAR(20) ,
    [ActiveStatus] BIT NOT NULL DEFAULT 1, 
	FOREIGN KEY (WorkflowTypeId) REFERENCES [dbo].MetaWorkflow(TypeId)
)
