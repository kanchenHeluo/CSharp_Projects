CREATE TRIGGER [CRMAzure].[trAppDeltaUpdate]
ON [CRMAzure].[MetaApp]
AFTER Update
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @AppId INT
	DECLARE @Active BIT

	SELECT @AppId = inserted.Id, @Active = inserted.ActiveStatus 
	FROM inserted
	INNER JOIN deleted 
	ON deleted.Id = inserted.Id

	IF @Active = 1
	BEGIN
		EXEC [CRMAzure].[spInsertTrigger] @AppId
	END
END

