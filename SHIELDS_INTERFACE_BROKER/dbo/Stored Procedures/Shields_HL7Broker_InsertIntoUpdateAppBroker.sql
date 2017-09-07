
-- =============================================
-- Author:		Christopher Bartholomew
-- Create date: 02/16/2012
-- Description:	Insert/Update app broker
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_InsertIntoUpdateAppBroker] 
	-- Add the parameters for the stored procedure here           
	@INTERFACE_STATUS_ID  int		= 0,
	@COMMUNICATION_ID     int		= 0,
	@PROCESS_ID           int		= 0,
	@LAST_MESSAGE_ID      int		= 0,
	@QUEUE_COUNT          int		= 0,
	@LAST_MESSAGE_DTTM    datetime	= NULL,
	@ID bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @ID = 0
	BEGIN
		-- Insert statements for procedure here
		INSERT INTO [dbo].[APP_BROKER]
			   ([INTERFACE_STATUS_ID]  
			   ,[COMMUNICATION_ID]     
			   ,[PROCESS_ID]           
			   ,[LAST_MESSAGE_ID]      
			   ,[QUEUE_COUNT]          
			   ,[LAST_MESSAGE_DTTM]    
				)
		 VALUES
			   (@INTERFACE_STATUS_ID
			   ,@COMMUNICATION_ID   
			   ,@PROCESS_ID         
			   ,@LAST_MESSAGE_ID    
			   ,@QUEUE_COUNT        
			   ,@LAST_MESSAGE_DTTM
			   )
		
		SELECT @ID=SCOPE_IDENTITY();
	END
	ELSE
	BEGIN		
		UPDATE [dbo].[APP_BROKER]
		   SET
			[INTERFACE_STATUS_ID]= @INTERFACE_STATUS_ID
		   ,[COMMUNICATION_ID]   = @COMMUNICATION_ID   
		   ,[PROCESS_ID]         = @PROCESS_ID         
		   ,[LAST_MESSAGE_ID]    = @LAST_MESSAGE_ID    
		   ,[QUEUE_COUNT]        = @QUEUE_COUNT        
		   ,[LAST_MESSAGE_DTTM]  = @LAST_MESSAGE_DTTM
		 WHERE 
			[ID] = @ID;		
	END

	SELECT @ID AS ID;
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_InsertIntoUpdateAppBroker] TO [ReadingRadPooledUser]
    AS [dbo];

