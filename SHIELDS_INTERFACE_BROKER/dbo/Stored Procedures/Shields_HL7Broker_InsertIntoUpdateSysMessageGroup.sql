-- =============================================
-- Author:		Christopher Bartholomew
-- Create date: 02/18/2014
-- Description:	insert/update SYS_MESSAGE_GROUP
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_InsertIntoUpdateSysMessageGroup] 
	-- Add the parameters for the stored procedure here
	@MESSAGE_GROUP_INSTANCE_ID bigint = 0,
	@MESSAGE_PART_ID int = 0, 
	@POSITION NVARCHAR(100) = NULL,
	@ID bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @MESSAGE_GROUP_INSTANCE_ID = 0
		SET @MESSAGE_GROUP_INSTANCE_ID = NULL;
	IF @MESSAGE_PART_ID = 0
		SET @MESSAGE_PART_ID = NULL;

    -- Insert statements for procedure here
	IF @ID = 0
	BEGIN
		INSERT INTO [dbo].[SYS_MESSAGE_GROUP]
				([MESSAGE_GROUP_INSTANCE_ID]
				,[MESSAGE_PART_ID]
				,[POSITION])
			VALUES
				(@MESSAGE_GROUP_INSTANCE_ID
				,@MESSAGE_PART_ID
				,@POSITION)
		
		SELECT @ID=SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		UPDATE [dbo].[SYS_MESSAGE_GROUP]
		   SET [MESSAGE_GROUP_INSTANCE_ID] = @MESSAGE_GROUP_INSTANCE_ID
			  ,[MESSAGE_PART_ID] = @MESSAGE_PART_ID
			  ,[POSITION] = @POSITION
		 WHERE [ID] = @ID		
	END
	SELECT @ID AS ID;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_InsertIntoUpdateSysMessageGroup] TO [ReadingRadPooledUser]
    AS [dbo];

