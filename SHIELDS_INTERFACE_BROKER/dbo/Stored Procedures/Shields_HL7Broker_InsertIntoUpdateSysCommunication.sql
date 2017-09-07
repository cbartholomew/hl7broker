-- =============================================
-- Author:		Christopher Bartholomew
-- Create date: 02/18/2014
-- Description:	insert/update SYS_COMMUNICATION
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_InsertIntoUpdateSysCommunication] 
	-- Add the parameters for the stored procedure here
	@DIRECTION_TYPE_ID bigint = 0,
	@COMMUNICATION_TYPE_ID bigint = 0, 
	@APPLICATION_ID bigint = 0,
	@ID bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @DIRECTION_TYPE_ID = 0
		SET @DIRECTION_TYPE_ID = NULL;
	IF @COMMUNICATION_TYPE_ID = 0
		SET @COMMUNICATION_TYPE_ID = NULL;
	IF @APPLICATION_ID = 0
		SET @APPLICATION_ID = NULL;

    -- Insert statements for procedure here
	IF @ID = 0
	BEGIN
		INSERT INTO [dbo].[SYS_COMMUNICATION]
				([DIRECTION_TYPE_ID]
				,[COMMUNICATION_TYPE_ID]
				,[APPLICATION_ID])
			VALUES
				(@DIRECTION_TYPE_ID
				,@COMMUNICATION_TYPE_ID
				,@APPLICATION_ID)
		
		SELECT @ID=SCOPE_IDENTITY();
	END
	ELSE
	BEGIN

		IF @DIRECTION_TYPE_ID	  = 0
			SET @DIRECTION_TYPE_ID = NULL;
		IF @COMMUNICATION_TYPE_ID = 0 
			SET @COMMUNICATION_TYPE_ID = NULL;
		IF @APPLICATION_ID		  = 0
			SET @APPLICATION_ID = NULL;

		UPDATE [dbo].[SYS_COMMUNICATION]
		   SET [DIRECTION_TYPE_ID] = @DIRECTION_TYPE_ID
			  ,[COMMUNICATION_TYPE_ID] = @COMMUNICATION_TYPE_ID
			  ,[APPLICATION_ID] = @APPLICATION_ID
		 WHERE [ID] = @ID		
	END
	SELECT @ID AS ID;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_InsertIntoUpdateSysCommunication] TO [ReadingRadPooledUser]
    AS [dbo];

