-- =============================================
-- Author:		Christopher Bartholomew
-- Create date: 02/16/2012
-- Description:	Insert/Update new system application
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_InsertIntoUpdateSysApplication] 
	-- Add the parameters for the stored procedure here
	@NAME varchar(200) = NULL, 
	@DESCRIPTION varchar(200) = NULL,
	@ID bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @ID = 0
	BEGIN
		-- Insert statements for procedure here
		INSERT INTO [dbo].[SYS_APPLICATION]
			   ([NAME]
			   ,[DESCRIPTION])
		 VALUES
			   (@NAME
			   ,@DESCRIPTION)
		
		SELECT @ID=SCOPE_IDENTITY();
	END
	ELSE
	BEGIN		
		UPDATE [dbo].[SYS_APPLICATION]
		   SET [NAME] = @NAME
			  ,[DESCRIPTION] = @DESCRIPTION
		 WHERE 
			[ID] = @ID;		
	END

	SELECT @ID AS ID;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_InsertIntoUpdateSysApplication] TO [ReadingRadPooledUser]
    AS [dbo];


GO


