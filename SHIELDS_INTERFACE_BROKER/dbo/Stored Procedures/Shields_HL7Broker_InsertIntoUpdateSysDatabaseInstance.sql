-- =============================================
-- Author:		Christopher Bartholomew
-- Create date: 02/17/2014
-- Description:	Insert/Update into DATABASE INSTANCE
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_InsertIntoUpdateSysDatabaseInstance] 
	-- Add the parameters for the stored procedure here
	@COMMUNICATION_ID BIGINT = NULL,
	@CREDENTIAL_ID BIGINT = 0, 
	@NAME VARCHAR(250) = NULL,
	@SERVER VARCHAR(250) = NULL,
	@IP_ADDRESS VARCHAR(250) = NULL,
	@ID bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @COMMUNICATION_ID = 0
		SET @COMMUNICATION_ID = NULL;
	IF @CREDENTIAL_ID = 0 
		SET @CREDENTIAL_ID = NULL;

	-- Insert statements for procedure here
	IF @ID = 0
	BEGIN
		INSERT INTO [dbo].[SYS_DATABASE_INSTANCE]
				   ([COMMUNICATION_ID],
				   [CREDENTIAL_ID],
				   [NAME],
				   [SERVER],
				   [IP_ADDRESS])
			 VALUES
				   (@COMMUNICATION_ID,
				    @CREDENTIAL_ID,
					@NAME,
					@SERVER,
					@IP_ADDRESS)

		SELECT @ID=SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		UPDATE [dbo].[SYS_DATABASE_INSTANCE]
		   SET	[COMMUNICATION_ID] = @COMMUNICATION_ID,
				[CREDENTIAL_ID] = @CREDENTIAL_ID,
				[NAME] = @NAME,
				[SERVER] = @SERVER,
				[IP_ADDRESS] = @IP_ADDRESS
		 WHERE ID = @ID
	END

	SELECT @ID AS ID;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_InsertIntoUpdateSysDatabaseInstance] TO [ReadingRadPooledUser]
    AS [dbo];


GO


