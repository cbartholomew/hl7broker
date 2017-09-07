-- =============================================
-- Author:		Bartholomew, Christopher
-- Create date: 04/09
-- Description:	Inserts Into Message Log Table
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_InsertIntoAppMessageLog]
	-- Add the parameters for the stored procedure here
	@MESSAGE_ID BIGINT = 0, 
	@MESSAGE_LOG_TYPE_ID INT = 0,
	@CREATED_DTTM NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[APP_MESSAGE_LOG]
			   ([MESSAGE_ID]
			   ,[MESSAGE_LOG_TYPE_ID]
			   ,[CREATED_DTTM])
		 VALUES
			   (@MESSAGE_ID
			   ,@MESSAGE_LOG_TYPE_ID
			   ,@CREATED_DTTM)

	SELECT * FROM [dbo].[APP_MESSAGE_LOG] (NOLOCK) WHERE ID = SCOPE_IDENTITY();
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_InsertIntoAppMessageLog] TO [ReadingRadPooledUser]
    AS [dbo];

