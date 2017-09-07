-- =============================================
-- Author:		Christopher Bartholomew
-- Create date: 02/17/2014
-- Description:	Insert/Update into Credential
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_InsertIntoUpdateSysCredential] 
	-- Add the parameters for the stored procedure here
	@CREDENTIAL_TYPE_ID bigint = 0, 
	@USERNAME VARCHAR(250) = NULL,
	@PASSWORD VARCHAR(250) = NULL,
	@ID bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @CREDENTIAL_TYPE_ID = 0
		SET @CREDENTIAL_TYPE_ID = NULL;

    -- Insert statements for procedure here
	IF @ID = 0
	BEGIN
		INSERT INTO [dbo].[SYS_CREDENTIAL]
				   ([CREDENTIAL_TYPE_ID]
				   ,[USERNAME]
				   ,[PASSWORD])
			 VALUES
				   (@CREDENTIAL_TYPE_ID
				   ,@USERNAME
				   ,@PASSWORD)

			SELECT @ID=SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		UPDATE [dbo].[SYS_CREDENTIAL]
		   SET [CREDENTIAL_TYPE_ID] = @CREDENTIAL_TYPE_ID
			  ,[USERNAME] = @USERNAME
			  ,[PASSWORD] = @PASSWORD
		 WHERE ID = @ID
	 END
	
	 SELECT @ID AS ID;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_InsertIntoUpdateSysCredential] TO [ReadingRadPooledUser]
    AS [dbo];


GO


