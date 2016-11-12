--USE ScheduleCreator
USE [3750User]

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

    SELECT @instructor_id = @instructor_id
    FROM Instructor
    WHERE @instructorWNumber = @instructorWNumber;
    RETURN @instructor_id
END
GO


---   udf_getDepartmentID   ---------------------------------------------------

IF EXISTS (
    SELECT *
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE SPECIFIC_NAME = 'udf_getDepartmentID'
	)
    DROP FUNCTION udf_getDepartmentID;
GO
CREATE FUNCTION dbo.udf_getDepartmentID
    (@departmentPrefix nvarchar(10))
RETURNS int
AS
BEGIN
    DECLARE @department_id int;

    SELECT @department_id = @department_id
    FROM Department    WHERE departmentPrefix = @departmentPrefix;
    RETURN @department_id
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

    SELECT @course_id = @course_id
    FROM Course    WHERE coursePrefix = @coursePrefix AND
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

    SELECT @classroom_id = @classroom_id
    FROM Classroom    WHERE buildingPrefix = @buildingPrefix AND
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

    SELECT @semester_id = @semester_id
    FROM Semester    WHERE semesterType = @semesterType AND
		  semesterYear = @semesterYear;
    RETURN @semester_id
END
GO

-------------------------------------------------------------------------------
---   Stored Procedures
-------------------------------------------------------------------------------

---   usp_addInstructorDepartment  --------------------------------------------
IF EXISTS (
    SELECT *
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE SPECIFIC_NAME = 'usp_addInstructorDepartment'
    )
    DROP PROCEDURE usp_addInstructorDepartment;
GO
CREATE PROCEDURE usp_addInstructorDepartment
    @instructorWNumber nvarchar(9),
    @departmentPrefix nvarchar(10)
AS
BEGIN
    DECLARE @instructor_id int;
    DECLARE @department_id int;
    SET @instructor_id = dbo.udf_getInstructorID(@instructorWNumber);
    IF (@instructor_id IS NULL) 
        BEGIN
            RAISERROR('Invalid instructorWNumber',0,1);
        END

    SET @department_id = dbo.udf_getDepartmentID(@departmentPrefix);
    IF (@department_id IS NULL)         BEGIN            RAISERROR('Invalid departmentPrefix',0,1);        END
    BEGIN TRY
    INSERT INTO dbo.InstructorDepartment
    (instructor_id, department_id, instructorWNumber, departmentPrefix)
VALUES (@instructor_id, @department_id, @instructorWNumber, @departmentPrefix);
    END TRY
    BEGIN CATCH
        RAISERROR('Error could not insert row',10,1); 
    END CATCH
END
GO