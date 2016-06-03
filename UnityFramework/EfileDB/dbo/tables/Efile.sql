CREATE TABLE [dbo].[Efile]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] VARCHAR(20) NOT NULL,
	[TransmissionId] VARCHAR(20) NOT NULL,
	[EfileStatus] INT NOT NULL,
	[Content] VARCHAR(MAX),
	[CreatedTime] DateTime,
	[ModifiedTime] DateTime
)
