-- =============================================
-- Author:		Christopher Bartholomew
-- Create date: 02/18/2014
-- Description:	insert/update message group instance
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_InsertIntoUpdateSysMessageGroupInstance] 
	-- Add the parameters for the stored procedure here
	@MESSAGE_TYPE_ID bigint = 0, 
	@SEGMENT_TYPE_ID bigint = 0,
	@DESCRIPTION NVARCHAR(255) = NULL,
	@ID bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @UPDATED_DTTM AS DATETIME;
	SELECT @UPDATED_DTTM=GETDATE();

	IF @MESSAGE_TYPE_ID = 0
		SET @MESSAGE_TYPE_ID = NULL;
	IF @SEGMENT_TYPE_ID = 0
		SET @SEGMENT_TYPE_ID = NULL;


    -- Insert statements for procedure here
	IF @ID = 0
	BEGIN
		-- GET CREATED DATE
		DECLARE @CREATED_DTTM AS DATETIME;		
		SELECT @CREATED_DTTM=GETDATE();

		INSERT INTO [dbo].[SYS_MESSAGE_GROUP_INSTANCE]
				   ([MESSAGE_TYPE_ID]
				   ,[SEGMENT_TYPE_ID]
				   ,[CREATED_DTTM]
				   ,[UPDATED_DTTM]
				   ,[DESCRIPTION])
			 VALUES
				   (@MESSAGE_TYPE_ID
				   ,@SEGMENT_TYPE_ID
				   ,@CREATED_DTTM
				   ,@UPDATED_DTTM
				   ,@DESCRIPTION)
		
		SELECT @ID=SCOPE_IDENTITY();

	END
	ELSE
	BEGIN
		-- DON'T UPDATE CREATED DATE
		UPDATE [dbo].[SYS_MESSAGE_GROUP_INSTANCE]
		   SET [MESSAGE_TYPE_ID] = @MESSAGE_TYPE_ID
			  ,[SEGMENT_TYPE_ID] = @SEGMENT_TYPE_ID
			  ,[UPDATED_DTTM] = @UPDATED_DTTM
			  ,[DESCRIPTION] = @DESCRIPTION
		 WHERE [ID] = @ID
	END

	SELECT @ID AS ID;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_InsertIntoUpdateSysMessageGroupInstance] TO [ReadingRadPooledUser]
    AS [dbo];

