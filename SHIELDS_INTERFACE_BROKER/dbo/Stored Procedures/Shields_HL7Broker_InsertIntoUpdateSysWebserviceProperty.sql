
-- =============================================
-- Author:		CHRISTOPHER BARTHOLOMEW
-- Create date: 02/17/2014
-- Description:	INSERT / UPDATE INTO WEB SERVICE PROPERTY SET
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_InsertIntoUpdateSysWebserviceProperty] 
	-- Add the parameters for the stored procedure here
	@WEBSERVICE_OBJECT_ID BIGINT = 0,
	@NAME VARCHAR(200) = NULL,
	@MESSAGE_GROUP_INSTANCE_ID BIGINT = 0, 
	@COLUMN_DATA_TYPE NVARCHAR(50) = NULL,
	@ID bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- HANDLE EMPTY FK RELATIONSHIPS 
	IF @WEBSERVICE_OBJECT_ID = 0
		SET @WEBSERVICE_OBJECT_ID = NULL;
	
	IF @MESSAGE_GROUP_INSTANCE_ID = 0
		SET @MESSAGE_GROUP_INSTANCE_ID = NULL;

    -- Insert statements for procedure here
	IF @ID = 0
	BEGIN
		INSERT INTO [dbo].[SYS_WEBSERVICE_PROPERTY_SET]
           ([WEBSERVICE_OBJECT_ID]
		   ,[NAME]
		   ,[MESSAGE_GROUP_INSTANCE_ID]
		   ,[COLUMN_DATA_TYPE])
		VALUES
           (@WEBSERVICE_OBJECT_ID
		   ,@NAME
		   ,@MESSAGE_GROUP_INSTANCE_ID
		   ,@COLUMN_DATA_TYPE)

		   SELECT @ID=SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		UPDATE [dbo].[SYS_WEBSERVICE_PROPERTY_SET]
		SET [WEBSERVICE_OBJECT_ID] = @WEBSERVICE_OBJECT_ID
		,[NAME] = @NAME
		,[MESSAGE_GROUP_INSTANCE_ID] = @MESSAGE_GROUP_INSTANCE_ID
		,[COLUMN_DATA_TYPE] = @COLUMN_DATA_TYPE
		WHERE ID = @ID
	END 

	SELECT @ID AS ID;
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_InsertIntoUpdateSysWebserviceProperty] TO [InterfaceBrokerUser]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_InsertIntoUpdateSysWebserviceProperty] TO [ReadingRadPooledUser]
    AS [dbo];

