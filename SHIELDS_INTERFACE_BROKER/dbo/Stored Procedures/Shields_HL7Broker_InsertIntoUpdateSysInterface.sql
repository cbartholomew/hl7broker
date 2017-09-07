-- =============================================
-- Author:		Christopher Bartholomew
-- Create date: 02/18/2014
-- Description:	Insert/Update into sys interface
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_InsertIntoUpdateSysInterface] 
	-- Add the parameters for the stored procedure here
	@COMMUNICATION_ID bigint = 0,
	@CREDENTIAL_ID bigint = 0, 
	@IP_ADDRESS NVARCHAR(200),
	@PORT NVARCHAR(10) = 0,
	@MAX_CONNECTIONS bigint = 0,
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
		INSERT INTO [dbo].[SYS_INTERFACE]
				([COMMUNICATION_ID]
				,[CREDENTIAL_ID]
				,[IP_ADDRESS]
				,[PORT]
				,[MAX_CONNECTIONS])
			VALUES
				(@COMMUNICATION_ID
				,@CREDENTIAL_ID
				,@IP_ADDRESS
				,@PORT
				,@MAX_CONNECTIONS)

		SELECT @ID=SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		UPDATE [dbo].[SYS_INTERFACE]
		   SET [COMMUNICATION_ID] = @COMMUNICATION_ID 
			  ,[CREDENTIAL_ID] = @CREDENTIAL_ID
			  ,[IP_ADDRESS] = @IP_ADDRESS
			  ,[PORT] = @PORT
			  ,[MAX_CONNECTIONS] = @MAX_CONNECTIONS
		 WHERE ID = @ID		 
	END

	SELECT @ID AS ID;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_InsertIntoUpdateSysInterface] TO [ReadingRadPooledUser]
    AS [dbo];


GO


