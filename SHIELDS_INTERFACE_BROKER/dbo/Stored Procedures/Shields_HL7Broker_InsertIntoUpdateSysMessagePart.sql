-- =============================================
-- Author:		Christopher Bartholomew
-- Create date: 02-18-2014
-- Description:	Insert/Update sys message part value
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_InsertIntoUpdateSysMessagePart] 
	-- Add the parameters for the stored procedure here
	@NAME NVARCHAR(100) = NULL, 
	@DELIMITER CHAR(10) = NULL,
	@ID bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @ID = 0
	BEGIN
		INSERT INTO [dbo].[SYS_MESSAGE_PART]
				   ([NAME]
				   ,[DELIMITER])
			 VALUES
				   (@NAME
				   ,@DELIMITER)

		SELECT @ID=SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		UPDATE [dbo].[SYS_MESSAGE_PART]
		   SET [NAME] = @NAME
			  ,[DELIMITER] = @DELIMITER
		 WHERE ID = @ID	
	END

	SELECT @ID AS ID;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_InsertIntoUpdateSysMessagePart] TO [ReadingRadPooledUser]
    AS [dbo];

