
-- =============================================
-- Author:		Christopher Bartholomew
-- Create date: 02/17/2014
-- Description:	Insert/Update into database table relation
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_InsertIntoUpdateSysDatabaseTableRelation] 
	-- Add the parameters for the stored procedure here
	@SOURCE_DATABASE_TABLE_ID bigint = 0, 
	@TARGET_DATABASE_TABLE_ID bigint = 0, 
	@REQUIRES_IDENTITY bit = 0,
	@ID bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @SOURCE_DATABASE_TABLE_ID = 0
		SET @SOURCE_DATABASE_TABLE_ID = NULL;

	IF @TARGET_DATABASE_TABLE_ID = 0
		SET @TARGET_DATABASE_TABLE_ID = NULL;

	-- Insert statements for procedure here
	IF @ID = 0
	BEGIN
		INSERT INTO [dbo].[SYS_DATABASE_TABLE_RELATION]
				   ([SOURCE_DATABASE_TABLE_ID],
				   [TARGET_DATABASE_TABLE_ID],
				    [REQUIRES_IDENTITY])
			 VALUES
				   (@SOURCE_DATABASE_TABLE_ID,
					@TARGET_DATABASE_TABLE_ID,
					@REQUIRES_IDENTITY)

		SELECT @ID=SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		UPDATE [dbo].[SYS_DATABASE_TABLE_RELATION]
		   SET	[SOURCE_DATABASE_TABLE_ID] = @SOURCE_DATABASE_TABLE_ID,
				[TARGET_DATABASE_TABLE_ID] = @TARGET_DATABASE_TABLE_ID,
				[REQUIRES_IDENTITY] = @REQUIRES_IDENTITY
		 WHERE ID = @ID
	END

	SELECT @ID AS ID;
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_InsertIntoUpdateSysDatabaseTableRelation] TO [ReadingRadPooledUser]
    AS [dbo];

