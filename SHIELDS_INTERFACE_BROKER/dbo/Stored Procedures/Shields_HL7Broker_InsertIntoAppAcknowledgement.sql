-- =============================================
-- Author:		Bartholomew, Christopher
-- Create date: 04/09
-- Description:	Inserts Into ACK Table
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_InsertIntoAppAcknowledgement] 
	-- Add the parameters for the stored procedure here
	@MESSAGE_ID BIGINT = 0, 
	@ACKNOWLEDGEMENT_TYPE_ID INT = 0,
	@RAW TEXT,
	@CREATED_DTTM NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[APP_ACKNOWLEDGEMENT]
			   ([MESSAGE_ID]
			   ,[ACKNOWLEDGEMENT_TYPE_ID]
			   ,[RAW]
			   ,[CREATED_DTTM])
		 VALUES
			   (@MESSAGE_ID
			   ,@ACKNOWLEDGEMENT_TYPE_ID
			   ,@RAW
			   ,@CREATED_DTTM)
	
	SELECT * FROM [dbo].[APP_ACKNOWLEDGEMENT] (NOLOCK) WHERE ID = SCOPE_IDENTITY();
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_InsertIntoAppAcknowledgement] TO [ReadingRadPooledUser]
    AS [dbo];

