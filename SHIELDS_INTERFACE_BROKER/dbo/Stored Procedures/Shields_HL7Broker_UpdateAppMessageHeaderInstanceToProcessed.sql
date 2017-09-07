-- =============================================
-- Author:		Bartholomew, Christopher
-- Create date: 04/16/2014
-- Description:	Updates the processed flag
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_UpdateAppMessageHeaderInstanceToProcessed] 
	-- Add the parameters for the stored procedure here
	@ID INT = 0,
	@MESSAGE_ID INT = 0,
	@ISREPROCESS BIT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- SET UP CONSTANTS	
	DECLARE @REPROCESS		AS INT = 2;
	DECLARE @ERRORED		AS INT = 3;
	DECLARE @UNKNOWN		AS INT = 4;
	DECLARE @APP_PROCESS	AS INT = 5;

	-- SET PROCESSED FLAGS
	DECLARE @IS_PROCESSED AS BIT;
	DECLARE @MESSAGE_LOG_TYPE AS INT;
	DECLARE @CURRENT_DTTM AS DATETIME;
	
	SET @IS_PROCESSED	  = 'true';
	SET @MESSAGE_LOG_TYPE = @UNKNOWN;
	SET @CURRENT_DTTM	  = GETDATE();

	IF @ID = 0
		BEGIN
			SET @ID = -1
		END
	ELSE
		BEGIN
			BEGIN TRY
				--UPDATE MESSAGE HEADER INSTANCE
				UPDATE [dbo].[APP_MESSAGE_HEADER_INSTANCE] 
				SET 
				PROCESSED_COUNT = PROCESSED_COUNT + 1, 
				PROCESSED_DTTM  = @CURRENT_DTTM,
				PROCESSED		= @IS_PROCESSED,
				PENDING_REPROCESS_DTTM = NULL
				WHERE [ID]		= @ID

				IF @ISREPROCESS = 0
					BEGIN
						SET @MESSAGE_LOG_TYPE = @APP_PROCESS;
					END
				ELSE
					BEGIN
						SET @MESSAGE_LOG_TYPE = @REPROCESS;
					END
		END TRY
			BEGIN CATCH
				SET @IS_PROCESSED		= 'false';
				SET @MESSAGE_LOG_TYPE	= @ERRORED;
			END CATCH
		END	

	--CREATE TEMP TABLE TO HOLD RESULTS
	DECLARE @TEMP_VALUE TABLE (ID INT, MESSAGE_ID INT, MESSAGE_LOG_TYPE_ID INT, CREATED_DTTM DATETIME)
	
	--UPDATE MESSAGE LOG TO REFLECT THIS IF THE ID IS NOT ZERO
	INSERT INTO @TEMP_VALUE	
	EXEC [dbo].[Shields_HL7Broker_InsertIntoAppMessageLog]@MESSAGE_ID,@MESSAGE_LOG_TYPE,@CURRENT_DTTM;
	
	--RETURN THE ID TO THROW AN ERROR
	SELECT @ID AS [ID]
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_UpdateAppMessageHeaderInstanceToProcessed] TO [ReadingRadPooledUser]
    AS [dbo];

