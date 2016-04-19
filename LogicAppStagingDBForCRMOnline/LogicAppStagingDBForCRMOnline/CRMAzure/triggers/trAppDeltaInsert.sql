CREATE TRIGGER [CRMAzure].[trAppDeltaInsert]
ON [CRMAzure].[MetaApp]
AFTER INSERT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @AppId INT
	DECLARE @Active BIT

	SELECT @AppId = inserted.Id, @Active = inserted.ActiveStatus 
	FROM inserted

	IF @Active = 1
	BEGIN
		EXEC [CRMAzure].[spInsertTrigger] @AppId
	END
END

