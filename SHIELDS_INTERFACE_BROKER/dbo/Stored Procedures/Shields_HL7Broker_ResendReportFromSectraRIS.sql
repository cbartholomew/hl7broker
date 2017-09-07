-- =============================================
-- Author:		Bartholomew, Christopher
-- Create date: 04/28/2014
-- Description:	Update
-- =============================================
CREATE PROCEDURE [dbo].[Shields_HL7Broker_ResendReportFromSectraRIS]
	-- Add the parameters for the stored procedure here
	@ACCESSION_NO NVARCHAR(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @ACCESSION_NO IS NULL
	BEGIN
		-- Insert statements for procedure here
		DECLARE @EID NVARCHAR(50)
		DECLARE REFILE_CURSOR CURSOR FORWARD_ONLY FOR

		SELECT e.accession_no as accession
		from [172.31.100.176].[ris_db_test].[dbo].reports as r 
		right outer join [172.31.100.176].[ris_db_test].[dbo].exams as e
		on e.EXAM_ID = r.EXAM_ID
		right outer  join [172.31.100.176].[ris_db_test].[dbo].patients as p
		on p.PATIENT_ID = e.PATIENT_ID
		where e.exam_status_id = 4
		and report_id is null

		OPEN REFILE_CURSOR

		FETCH NEXT FROM REFILE_CURSOR
		INTO @EID

		WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @MCID AS NVARCHAR(50);

			select @MCID=messageid from [shcsctdb].[rlogic].[rlogic].hl7event with (nolock)
			where 
			examid = (select examid from [shcsctdb].[rlogic].[rlogic].exams where accession_no = @EID)
			and eventtype = 'OUF'
			and [partner] = 'General'

			IF @MCID != ''
			BEGIN
				-- This will re-send missing message 
				update [shcsctdb].[rlogic].[rlogic].messagequeue 
				set [status] = 'A' 
				where messageId = @MCID
			END
		FETCH NEXT FROM REFILE_CURSOR
		INTO @EID

		END

		CLOSE REFILE_CURSOR
		DEALLOCATE REFILE_CURSOR
	END
	ELSE
	BEGIN
			DECLARE @MC AS NVARCHAR(50);
			select @MC=messageid from [shcsctdb].[rlogic].[rlogic].hl7event with (nolock)
			where 
			examid = (select examid from [shcsctdb].[rlogic].[rlogic].exams where accession_no = @ACCESSION_NO)
			and eventtype = 'OUF'
			and [partner] = 'General'

			IF @MCID != ''
			BEGIN
				-- This will re-send missing message 
				update [shcsctdb].[rlogic].[rlogic].messagequeue 
				set [status] = 'A' 
				where messageId = @MC
			END
	END 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Shields_HL7Broker_ResendReportFromSectraRIS] TO [ReadingRadPooledUser]
    AS [dbo];

