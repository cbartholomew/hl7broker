-- =============================================
-- Author:		Christopher Bartholomew
-- Create date: 05/28/2014
-- Description:	
-- =============================================
CREATE PROCEDURE Shields_HL7Broker_UpdateAppBrokerProcessIdentity 
	-- Add the parameters for the stored procedure here
	@PROCESS_ID int = 0, 
	@ID int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE APP_BROKER SET 
	PROCESS_ID = @PROCESS_ID 
	WHERE [ID] = @ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_UpdateAppBrokerProcessIdentity] TO [ReadingRadPooledUser]
    AS [dbo];

