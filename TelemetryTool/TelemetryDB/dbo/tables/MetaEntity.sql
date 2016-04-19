CREATE TABLE [dbo].[MetaEntity]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	[EntityName] NCHAR(20) NOT NULL,
	[DBPrimaryKey] NCHAR(20) NOT NULL
)

