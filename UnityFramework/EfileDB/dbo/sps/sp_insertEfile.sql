CREATE PROCEDURE [dbo].[sp_insertEfile]
	@id [BIGINT] Output,
	@name VARCHAR(20),
	@transmissionId VARCHAR(20),
	@efileStatus INT,
	@content VARCHAR(MAX)
AS
	SET NOCOUNT ON

	INSERT INTO dbo.Efile
                    ( [Name] ,
                      [EfileStatus],
                      [TransmissionId] ,
                      [Content] ,
                      [CreatedTime] ,
                      [ModifiedTime]
                      
                    )
            VALUES  ( 
						@name,
						@efileStatus,
						@transmissionId,
						@content,
						getutcdate(),
						getutcdate()                      
                    )
    SELECT @id = SCOPE_IDENTITY()

	SET NOCOUNT OFF
