﻿
-- =============================================
-- Author:		Christopher Bartholomew
-- Create date: 02/17/2014
-- Description:	Insert/Update into Webservice Object
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_InsertIntoUpdateSysWebserviceObject] 
	-- Add the parameters for the stored procedure here
	@WEBSERVICE_INSTANCE_ID bigint = 0, 
	@NAME VARCHAR(250) = NULL,
	@ID bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @WEBSERVICE_INSTANCE_ID = 0
		SET @WEBSERVICE_INSTANCE_ID = NULL;

	-- Insert statements for procedure here
	IF @ID = 0
	BEGIN
		INSERT INTO [dbo].[SYS_WEBSERVICE_OBJECT]
				   ([WEBSERVICE_INSTANCE_ID],
				   [NAME])
			 VALUES
				   (@WEBSERVICE_INSTANCE_ID,
					@NAME)

		SELECT @ID=SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		UPDATE [dbo].[SYS_WEBSERVICE_OBJECT]
		   SET	WEBSERVICE_INSTANCE_ID = @WEBSERVICE_INSTANCE_ID,
				NAME = @NAME
		 WHERE ID = @ID
	END

	SELECT @ID AS ID;
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_InsertIntoUpdateSysWebserviceObject] TO [InterfaceBrokerUser]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_InsertIntoUpdateSysWebserviceObject] TO [ReadingRadPooledUser]
    AS [dbo];

