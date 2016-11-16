USE ScheduleCreater

--Instructor(Needed?)
--Department (Needed?)
--InstructorRelease
--Semester (Needed?)
--Course

 IF EXISTS( 
	SELECT * 
	FROM INFORMATION_SCHEMA.ROUTINES
	WHERE SPECIFIC_NAME = 'usp_addCourse'
	)
		DROP PROCEDURE usp_addCourse;
GO

CREATE PROCEDURE usp_addCourse
	@pbUserID nvarchar(10),
	@movieItemID int,	
	@rentalDate smalldatetime
AS
BEGIN --{
	DECLARE @pbUser_id int;
	DECLARE @pbMovieItem_id int;

	SELECT @pbUser_id = dbo.udf_getUserID(@pbUserID);
	SELECT @pbMovieItem_id = dbo.udf_getMovieItemID(@movieItemID);

	IF (@pbUser_id IS NULL) 
		BEGIN
			RAISERROR ('Invalid User ID', 0,1);
			RETURN 1;
		END

	IF (@pbMovieItem_id IS NULL) 
		BEGIN
			RAISERROR ('Invalid movieItemID', 0,3);
			RETURN 1;
		END

	BEGIN TRY
		INSERT PbRental(pbUserID, movieItemID, rentalDate, dueDate, pbUser_id, pbMovieItem_id, lastChangedBy, lastChangedOn)
		VALUES(@pbUserID, @movieItemID, @rentalDate, GETDATE(), @pbUser_id, @pbMovieItem_id, 'kwr', GETDATE());
	END TRY
	BEGIN CATCH
		--PRINT 'Failed to insert';
		RAISERROR('Insert statement failed', 0, 2);
	END CATCH
END --}