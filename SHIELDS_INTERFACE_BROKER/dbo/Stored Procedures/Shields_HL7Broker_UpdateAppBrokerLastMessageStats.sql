-- =============================================
-- Author:		Christopher Bartholomew
-- Create date: 05/24/2014
-- Description:	Updates the message and queue count for the main worklist
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_UpdateAppBrokerLastMessageStats] 
	-- Add the parameters for the stored procedure here
	@LAST_MESSAGE_ID int = 0, 
	@LAST_MESSAGE_DTTM NVARCHAR(50) = NULL,
	@QUEUE_COUNT int = -1,
	@ID int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT @QUEUE_COUNT=COUNT(MHI_ID)
	FROM [dbo].[vShieldsHL7Broker_GetMessageHeaderInstances] (nolock)
	WHERE MHI_PROCESSED = 0
	
    -- Insert statements for procedure here
	UPDATE APP_BROKER SET 
	LAST_MESSAGE_ID = @LAST_MESSAGE_ID, 
	LAST_MESSAGE_DTTM = Convert(datetime,@LAST_MESSAGE_DTTM,121), 
	QUEUE_COUNT = @QUEUE_COUNT 
	WHERE [ID] = @ID

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_UpdateAppBrokerLastMessageStats] TO [ReadingRadPooledUser]
    AS [dbo];

