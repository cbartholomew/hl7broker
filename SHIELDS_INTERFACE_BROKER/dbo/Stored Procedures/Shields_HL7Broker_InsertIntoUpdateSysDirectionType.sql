-- =============================================
-- Author:		Christopher Bartholomew
-- Create date: 02-18-2014
-- Description:	Insert/Update sys direction type
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_InsertIntoUpdateSysDirectionType] 
	-- Add the parameters for the stored procedure here
	@NAME NVARCHAR(50) = NULL, 
	@ID bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @ID = 0
	BEGIN
		INSERT INTO [dbo].[SYS_DIRECTION_TYPE]
				   ([NAME])
			 VALUES
				   (@NAME)

		 SELECT @ID=SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		UPDATE [dbo].[SYS_DIRECTION_TYPE]
		SET [NAME] = @NAME
		WHERE [ID] = @ID
	END

	SELECT @ID AS ID;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_InsertIntoUpdateSysDirectionType] TO [ReadingRadPooledUser]
    AS [dbo];

