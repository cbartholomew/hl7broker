-- =============================================
-- Author:		Christopher Bartholomew
-- Create date: 02/18/2014
-- Description:	Insert/Update sys segment type
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_InsertIntoUpdateSysSegmentType] 
	-- Add the parameters for the stored procedure here
	@NAME VARCHAR(200) = NULL, 
	@ID bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @ID = 0
	BEGIN
		INSERT INTO [dbo].[SYS_SEGMENT_TYPE]
				   ([NAME])
			 VALUES
				   (@NAME)

		SELECT @ID=SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		UPDATE [dbo].[SYS_SEGMENT_TYPE]
		   SET [NAME] = @NAME
		 WHERE [ID] = @ID
	END	

	SELECT @ID AS ID;
END

GO


