-- =============================================
-- Author:		Christopher Bartholomew
-- Create date: 05/28/2014
-- Description:	Updates Interface Status Only
-- =============================================
CREATE PROCEDURE [DBO].[Shields_HL7Broker_UpdateAppBrokerInterfaceStatus]
	-- Add the parameters for the stored procedure here
	@INTERFACE_STATUS_ID int = 0, 
	@ID int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE APP_BROKER SET 
	INTERFACE_STATUS_ID = @INTERFACE_STATUS_ID 
	WHERE [ID] = @ID;

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_UpdateAppBrokerInterfaceStatus] TO [ReadingRadPooledUser]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_UpdateAppBrokerInterfaceStatus] TO [InterfaceBrokerUser]
    AS [dbo];

