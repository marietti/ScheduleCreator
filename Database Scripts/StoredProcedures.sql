--USE ScheduleCreator
USE [3750User]

--- Delete Test Data
DELETE Section WHERE courseNumber = '1410'
DELETE InstructorProgram WHERE instructorWNumber = 'W0100007';
DELETE InstructorRelease WHERE instructorWNumber = 'W0100007';
DELETE Instructor WHERE instructorWNumber = 'W0100007';

DELETE Course WHERE courseNumber = '1410'

DELETE Classroom WHERE active = 'N'
GO

-------------------------------------------------------------------------------
---   Functions
-------------------------------------------------------------------------------

---   udf_getInstructorID   ---------------------------------------------------

IF EXISTS (
    SELECT *
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE SPECIFIC_NAME = 'udf_getInstructorID'
	)
    DROP FUNCTION udf_getInstructorID;
GO

CREATE FUNCTION dbo.udf_getInstructorID
    (@instructorWNumber nvarchar(9))
RETURNS int
AS
BEGIN
    DECLARE @instructor_id int;

    SELECT @instructor_id = instructor_id
    FROM Instructor
    WHERE instructorWNumber = @instructorWNumber;
    RETURN @instructor_id
END
GO


---   udf_getProgramID   ---------------------------------------------------

IF EXISTS (
    SELECT *
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE SPECIFIC_NAME = 'udf_getProgramID'
	)
    DROP FUNCTION udf_getProgramID;
GO

CREATE FUNCTION dbo.udf_getProgramID
    (@programPrefix nvarchar(10))
RETURNS int
AS
BEGIN
    DECLARE @program_id int;

    SELECT @program_id = program_id
    FROM Program
    WHERE programPrefix = @programPrefix;
    RETURN @program_id
END
GO


---   udf_getCourseID   -------------------------------------------------------

IF EXISTS (
    SELECT *
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE SPECIFIC_NAME = 'udf_getCourseID'
	)
    DROP FUNCTION udf_getCourseID;
GO

CREATE FUNCTION dbo.udf_getCourseID
    (@coursePrefix nvarchar(10),
	 @courseNumber nvarchar(10))
RETURNS int
AS
BEGIN
    DECLARE @course_id int;

    SELECT @course_id = course_id
    FROM Course
    WHERE coursePrefix = @coursePrefix AND
		  courseNumber = @courseNumber;
    RETURN @course_id
END
GO


---   udf_getClassroomID   ----------------------------------------------------

IF EXISTS (
    SELECT *
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE SPECIFIC_NAME = 'udf_getClassroomID'
	)
    DROP FUNCTION udf_getClassroomID;
GO

CREATE FUNCTION dbo.udf_getClassroomID
    (@buildingPrefix nvarchar(10),
	 @roomNumber nvarchar(10))
RETURNS int
AS
BEGIN
    DECLARE @classroom_id int;

    SELECT @classroom_id = classroom_id
    FROM Classroom
    WHERE buildingPrefix = @buildingPrefix AND
		  roomNumber = @roomNumber;
    RETURN @classroom_id
END
GO


---   udf_getSemesterID   -----------------------------------------------------

IF EXISTS (
    SELECT *
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE SPECIFIC_NAME = 'udf_getSemesterID'
	)
    DROP FUNCTION udf_getSemesterID;
GO

CREATE FUNCTION dbo.udf_getSemesterID
    (@semesterType nvarchar(10),
	 @semesterYear nvarchar(10))
RETURNS int
AS
BEGIN
    DECLARE @semester_id int;

    SELECT @semester_id = semester_id
    FROM Semester
    WHERE semesterType = @semesterType AND
		  semesterYear = @semesterYear;
    RETURN @semester_id
END
GO


---   udf_getBuildingID   ----------------------------------------------------

IF EXISTS (
    SELECT *
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE SPECIFIC_NAME = 'udf_getBuildingID'
	)
    DROP FUNCTION udf_getBuildingID;
GO

CREATE FUNCTION dbo.udf_getBuildingID
    (@buildingPrefix nvarchar(10))
RETURNS int
AS
BEGIN
    DECLARE @building_id int;

    SELECT @building_id = building_id
    FROM Building
    WHERE buildingPrefix = @buildingPrefix;
    RETURN @building_id
END
GO


-------------------------------------------------------------------------------
---   Stored Procedures
-------------------------------------------------------------------------------

---   usp_addInstructorProgram  --------------------------------------------

IF EXISTS (
    SELECT *
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE SPECIFIC_NAME = 'usp_addInstructorProgram'
    )
    DROP PROCEDURE usp_addInstructorProgram;
GO

CREATE PROCEDURE usp_addInstructorProgram
    @instructorWNumber nvarchar(9),
    @programPrefix nvarchar(10)
AS
BEGIN
    DECLARE @instructor_id int;
    DECLARE @program_id int;

    SET @instructor_id = dbo.udf_getInstructorID(@instructorWNumber);

    IF (@instructor_id IS NULL) 
        BEGIN
            RAISERROR('Invalid instructorWNumber',0,1);
        END


    SET @program_id = dbo.udf_getProgramID(@programPrefix);

    IF (@program_id IS NULL) 
        BEGIN
            RAISERROR('Invalid programPrefix',0,1);
        END

    BEGIN TRY
    INSERT INTO dbo.InstructorProgram
    (instructor_id, program_id, instructorWNumber, programPrefix)
VALUES (@instructor_id, @program_id, @instructorWNumber, @programPrefix);
    END TRY
    BEGIN CATCH
        RAISERROR('Error could not insert row',10,1); 
    END CATCH

END
GO


---   usp_addCourse  --------------------------------------------

IF EXISTS (
    SELECT *
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE SPECIFIC_NAME = 'usp_addCourse'
    )
    DROP PROCEDURE usp_addCourse;
GO

CREATE PROCEDURE usp_addCourse
    @coursePrefix nvarchar(10), --Should we just make this the same as program prefix?
    @courseNumber nvarchar(10),
    @programPrefix nvarchar(10),
    @courseName nvarchar(100),
    @defaultCredits decimal,
    @active nvarchar(5)
AS
BEGIN
	DECLARE @program_id int;

	SET @program_id = dbo.udf_getProgramID(@programPrefix);
    
	IF (@program_id IS NULL) 
        BEGIN
            RAISERROR('Invalid programPrefix',0,1);
        END
    
	BEGIN TRY
    INSERT INTO dbo.Course
    (program_id, coursePrefix, courseNumber, courseName, programPrefix,
     defaultCredits, active)
VALUES (@program_id, @coursePrefix, @courseNumber, @courseName, @programPrefix,
	    @defaultCredits, @active);
    END TRY
    BEGIN CATCH
        RAISERROR('Error could not insert row',10,1); 
    END CATCH

END
GO


---   usp_addClassroom  --------------------------------------------

IF EXISTS (
    SELECT *
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE SPECIFIC_NAME = 'usp_addClassroom'
    )
    DROP PROCEDURE usp_addClassroom;
GO

CREATE PROCEDURE usp_addClassroom
    @buildingPrefix nvarchar(10),
    @roomNumber nvarchar(10),
    @classroomCapacity int,
    @computers int,
    @availableFromTime time,
    @availableToTime time,
    @active nvarchar(4)
AS
BEGIN
	DECLARE @building_id int;

	SET @building_id = dbo.udf_getBuildingID(@buildingPrefix);
    
	IF (@building_id IS NULL) 
        BEGIN
            RAISERROR('Invalid buildingPrefix',0,1);
        END
    
	BEGIN TRY
    INSERT INTO dbo.Classroom
    (building_id, buildingPrefix, roomNumber, classroomCapacity, computers,
     availableFromTime, availableToTime, active)
VALUES (@building_id, @buildingPrefix, @roomNumber, @classroomCapacity, @computers,
		@availableFromTime, @availableToTime, @active);
    END TRY
    BEGIN CATCH
        RAISERROR('Error could not insert row',10,1); 
    END CATCH

END
GO


---   usp_addSection  --------------------------------------------

IF EXISTS (
    SELECT *
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE SPECIFIC_NAME = 'usp_addSection'
    )
    DROP PROCEDURE usp_addSection;
GO

CREATE PROCEDURE usp_addSection
    @coursePrefix nvarchar(10),
    @courseNumber nvarchar(10),
    @buildingPrefix nvarchar(10),
    @roomNumber nvarchar(10),
    @instructorWNumber nvarchar(9),
    @semesterType nvarchar(10),
    @semesterYear int,
    @crn nvarchar(10),
    @daysTaught nvarchar(10),
    @courseStartTime time,
    @courseEndTime time,
    @block nvarchar(5),
    @courseType nvarchar(10),
    @pay nvarchar(50),
    @sectionCapacity int,
    @creditLoad decimal,
    @creditOverload decimal,
    @comments nvarchar(255)
AS
BEGIN
	DECLARE @course_id int;
    DECLARE @classroom_id int;
    DECLARE @instructor_id int;
    DECLARE @semester_id int;

	SET @course_id = dbo.udf_getCourseID(@coursePrefix, @courseNumber);
    
	IF (@coursePrefix IS NOT NULL AND @courseNumber IS NOT NULL AND @course_id IS NULL) 
        BEGIN
            RAISERROR('Invalid coursePrefix and courseNumber',0,1);
        END


	SET @classroom_id = dbo.udf_getClassroomID(@buildingPrefix, @roomNumber);
    
	IF (@buildingPrefix IS NOT NULL AND @roomNumber IS NOT NULL AND @classroom_id IS NULL) 
        BEGIN
            RAISERROR('Invalid buildingPrefix and roomNumber',0,1);
        END


	SET @instructor_id = dbo.udf_getInstructorID(@instructorWNumber);
    
	IF (@instructorWNumber IS NOT NULL AND @instructor_id IS NULL) 
        BEGIN
            RAISERROR('Invalid coursePrefix and instructorWNumber',0,1);
        END


	SET @semester_id = dbo.udf_getSemesterID(@semesterType, @semesterYear);
    
	IF (@semesterType IS NOT NULL AND @semesterType IS NOT NULL AND @semester_id IS NULL) 
        BEGIN
            RAISERROR('Invalid semesterType and semesterYear',0,1);
        END

    
	BEGIN TRY
		INSERT INTO dbo.Section
			(course_id, classroom_id, instructor_id, semester_id, coursePrefix, 
			 courseNumber, buildingPrefix, roomNumber, instructorWNumber, semesterType,
			 semesterYear, crn, daysTaught, courseStartTime, courseEndTime, block,
			 courseType, pay, sectionCapacity, creditLoad, creditOverload, comments)
		VALUES (@course_id, @classroom_id, @instructor_id, @semester_id, @coursePrefix, 
				@courseNumber, @buildingPrefix, @roomNumber, @instructorWNumber, @semesterType,
				@semesterYear, @crn, @daysTaught, @courseStartTime, @courseEndTime, @block,
				@courseType, @pay, @sectionCapacity, @creditLoad, @creditOverload, @comments);
    END TRY
    BEGIN CATCH
        RAISERROR('Error could not insert row',10,1); 
    END CATCH

END
GO


---   usp_addInstructorRelease  --------------------------------------------

IF EXISTS (
    SELECT *
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE SPECIFIC_NAME = 'usp_addInstructorRelease'
    )
    DROP PROCEDURE usp_addInstructorRelease;
GO

CREATE PROCEDURE usp_addInstructorRelease
    @instructorWNumber nvarchar(9),
	@semesterType nvarchar(10),
	@semesterYear int,
	@releaseDescription nvarchar(255),
	@totalReleaseHours decimal
AS
BEGIN
	DECLARE @semester_id int;

	SET @semester_id = dbo.udf_getSemesterID(@semesterType, @semesterYear);
    
	IF (@semester_id IS NULL) 
        BEGIN
            RAISERROR('Invalid semesterType or semesterYear',0,1);
        END

	DECLARE @instructor_id int;

	SET @instructor_id = dbo.udf_getInstructorID(@instructorWNumber);
    
	IF (@instructor_id IS NULL) 
        BEGIN
            RAISERROR('Invalid instructorWNumber',0,1);
        END
    
	BEGIN TRY
    INSERT INTO dbo.InstructorRelease
	(instructor_id, semester_id, instructorWNumber, semesterType, semesterYear, releaseDescription, totalReleaseHours)
VALUES (@instructor_id, @semester_id, @instructorWNumber, @semesterType, @semesterYear,
		@releaseDescription, @totalReleaseHours);
    END TRY
    BEGIN CATCH
        RAISERROR('Error could not insert row',10,1); 
    END CATCH

END
GO

-------------------------------------------------------------------------------
---   Triggers
-------------------------------------------------------------------------------
--Check for time conflicts centered around the schedule table
DROP TRIGGER dbo.udt_sectionConflicts
GO

CREATE TRIGGER dbo.udt_sectionConflicts
on Section
AFTER INSERT
AS
BEGIN

	--One instructor can't teach two sections at the same time
	
		--Check if any sections are being taught at the same time by the same professor
		DECLARE @instructorTimeOverlap int

		SELECT  @instructorTimeOverlap = COUNT(*)
		FROM Section s
		JOIN inserted i ON s.instructor_id = i.instructor_id
		WHERE ((i.courseStartTime >= s.courseStartTime AND i.courseStartTime <= s.courseEndTime)
		OR (i.courseEndTime >= s.courseStartTime AND i.courseEndTime <= s.courseEndTime)
		OR (i.courseStartTime <= s.courseStartTime AND i.courseEndTime >= s.courseEndTime))
		AND (i.section_id != s.section_id)
		AND (i.daysTaught LIKE s.daysTaught)
		AND (i.semester_id = s.semester_id)

		
		IF (@instructorTimeOverlap > 0)
		-- The instructor is already teaching a section that the inserted one would overlap with
			BEGIN
				RAISERROR('The instructor is already teaching a section at this time', 10, 5)
				ROLLBACK TRANSACTION
				RETURN
			END

		--Two sections can't be in the same classroom at the same time
		DECLARE @ClassroomTimeOverlap int

		SELECT  @ClassroomTimeOverlap = COUNT(*)
		FROM Section s
		JOIN inserted i ON s.classroom_id = i.classroom_id
		WHERE ((i.courseStartTime >= s.courseStartTime AND i.courseStartTime <= s.courseEndTime)
		OR (i.courseEndTime >= s.courseStartTime AND i.courseEndTime <= s.courseEndTime)
		OR (i.courseStartTime <= s.courseStartTime AND i.courseEndTime >= s.courseEndTime))
		AND (i.section_id != s.section_id)
		AND (i.daysTaught LIKE s.daysTaught)
		AND (i.semester_id = s.semester_id)

		IF (@ClassroomTimeOverlap > 0)
		-- The Classroom is already being used 
			BEGIN
				RAISERROR('The classroom is already being used at this time', 9, 4)
				ROLLBACK TRANSACTION
				RETURN
			END

	--Insturctor can't go over the absolute max credit load
	--Not the most elegent solution, but it's serviceable
	DECLARE @CreditLoad int
	DECLARE @ReleaseCredits int
	DECLARE @ProgramMax int

	SELECT @CreditLoad = SUM(s.creditLoad) 
	FROM Section s
	JOIN inserted i on s.instructor_id = i.instructor_id
	WHERE s.instructor_id = i.instructor_id GROUP BY s.instructor_id

	SELECT @ReleaseCredits = r.totalReleaseHours
	FROM InstructorRelease r
	JOIN inserted i on r.instructor_id = i.instructor_id
	WHERE r.semester_id = i.semester_id

	SELECT @ProgramMax = p.maxCreditsAllowed
	FROM Program p
	JOIN InstructorProgram ip on ip.program_id = p.program_id
	JOIN inserted i on i.instructor_id = ip.instructor_id

	IF ( @CreditLoad + @ReleaseCredits > @ProgramMax)
	BEGIN
		RAISERROR('Instructor is now overloaded on credits', 8, 3)
	END
END
GO

DROP TRIGGER dbo.udt_creditOverloadCheck
GO

CREATE TRIGGER dbo.udt_creditOverloadCheck
on InstructorRelease
AFTER INSERT
AS
BEGIN
	--Insturctor can't go over the absolute max credit load
	--Not the most elegent solution, but it's serviceable
	DECLARE @CreditLoad int
	DECLARE @ReleaseCredits int
	DECLARE @ProgramMax int

	SELECT @CreditLoad = SUM(s.creditLoad) 
	FROM Section s
	JOIN inserted i on s.instructor_id = i.instructor_id
	WHERE s.instructor_id = i.instructor_id GROUP BY s.instructor_id

	SELECT @ReleaseCredits = r.totalReleaseHours
	FROM InstructorRelease r
	JOIN inserted i on r.instructor_id = i.instructor_id
	WHERE r.semester_id = i.semester_id

	SELECT @ProgramMax = p.maxCreditsAllowed
	FROM Program p
	JOIN InstructorProgram ip on ip.program_id = p.program_id
	JOIN inserted i on i.instructor_id = ip.instructor_id

	IF ( @CreditLoad + @ReleaseCredits > @ProgramMax)
	BEGIN
		RAISERROR('Instructor is now overloaded on credits', 8, 3)
	END
END