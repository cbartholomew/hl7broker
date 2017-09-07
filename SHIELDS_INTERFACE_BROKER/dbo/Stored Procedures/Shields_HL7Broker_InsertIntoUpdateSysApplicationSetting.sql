-- =============================================
-- Author:		Christopher Bartholomew
-- Create date: 02/16/2012
-- Description:	Insert/Update new system application setting
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_InsertIntoUpdateSysApplicationSetting] 
	-- Add the parameters for the stored procedure here
	@APPLICATION_ID INT = 0, 
	@COMMUNICATION_ID INT = 0, 
	@DISABLED BIT = 0,
	@AUTOSTART BIT = 0,
	@ID bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @ID = 0
	BEGIN
		-- Insert statements for procedure here
		INSERT INTO [dbo].[SYS_APPLICATION_SETTING]
			   ([APPLICATION_ID]
			   ,[COMMUNICATION_ID]
			   ,[DISABLED]
			   ,[AUTOSTART])
		 VALUES
			   (@APPLICATION_ID
			   ,@COMMUNICATION_ID
			   ,@DISABLED
			   ,@AUTOSTART)
		
		SELECT @ID=SCOPE_IDENTITY();
	END
	ELSE
	BEGIN		
		UPDATE [dbo].[SYS_APPLICATION_SETTING]
		   SET [APPLICATION_ID] = @APPLICATION_ID
			  ,[COMMUNICATION_ID] = @COMMUNICATION_ID
			  ,[DISABLED] = @DISABLED
			  ,[AUTOSTART] = @AUTOSTART
		 WHERE 
			[ID] = @ID;		
	END

	SELECT @ID AS ID;
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_InsertIntoUpdateSysApplicationSetting] TO [ReadingRadPooledUser]
    AS [dbo];

