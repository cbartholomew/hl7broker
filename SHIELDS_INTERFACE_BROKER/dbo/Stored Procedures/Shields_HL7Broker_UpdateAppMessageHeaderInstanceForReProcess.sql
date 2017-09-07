
-- =============================================
-- Author:		Bartholomew, Christopher
-- Create date: 04/16/2014
-- Description:	Updates the processed flag
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_UpdateAppMessageHeaderInstanceForReProcess] 
	-- Add the parameters for the stored procedure here
	@ID INT = 0,
	@MESSAGE_CONTROL_ID NVARCHAR(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @IS_PROCESSED AS BIT = 'false'
	DECLARE @RETURNTHIS AS NVARCHAR(50);

	IF @ID = 0
		BEGIN
			IF @MESSAGE_CONTROL_ID IS NULL
				BEGIN
					SET @ID = -1
					SET @RETURNTHIS  = 0
				END
			ELSE
				BEGIN
					BEGIN TRY
						--UPDATE MESSAGE HEADER INSTANCE BY REGULAR ID
						UPDATE [dbo].[APP_MESSAGE_HEADER_INSTANCE] 
						SET 
						PROCESSED			   = @IS_PROCESSED,
						PENDING_REPROCESS_DTTM = GETDATE()
						WHERE [MESSAGE_CONTROL_ID]= @MESSAGE_CONTROL_ID

						SET @RETURNTHIS  = @MESSAGE_CONTROL_ID
					END TRY
					BEGIN CATCH
						SET @ID = -1
						SET @RETURNTHIS  = 0
					END CATCH
				END
		END
	ELSE
		BEGIN
			BEGIN TRY
				--UPDATE MESSAGE HEADER INSTANCE BY REGULAR ID
				UPDATE [dbo].[APP_MESSAGE_HEADER_INSTANCE] 
				SET 
				PROCESSED			   = @IS_PROCESSED,
				PENDING_REPROCESS_DTTM = GETDATE()
				WHERE [ID]			   = @ID

				SET @RETURNTHIS  = @ID
		END TRY
			BEGIN CATCH
				SET @ID = -1
				SET @RETURNTHIS  = 0
			END CATCH
		END	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_UpdateAppMessageHeaderInstanceForReProcess] TO [ReadingRadPooledUser]
    AS [dbo];

