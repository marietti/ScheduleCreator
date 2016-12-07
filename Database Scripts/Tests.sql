USE [3750User]

-------------------------------------------------------------------------------
---   Tests
-------------------------------------------------------------------------------

DELETE Section WHERE courseNumber = '1410'
DELETE InstructorProgram WHERE instructorWNumber = 'W0100007';
DELETE InstructorRelease WHERE instructorWNumber = 'W0100007';
DELETE Instructor WHERE instructorWNumber = 'W0100007';

DELETE Course WHERE courseNumber = '1410'

DELETE Classroom WHERE active = 'N'

DELETE InstructorRelease WHERE instructorWNumber = 'W0100003'

DELETE Section WHERE courseNumber = '1401'

------usp_addInstructorProgram Test ----------------------------------------
INSERT INTO dbo.Instructor
    (instructorWNumber, instructorFirstName, instructorLastName, hoursRequired, active)
VALUES ('W0100007', 'Rob', 'Hilton', 12, 'Y');
GO

EXEC usp_addInstructorProgram 
    @instructorWNumber='W0100007', @programPrefix='CS'; 

SELECT * FROM Instructor;
SELECT * FROM InstructorProgram;

------usp_addCourse Test ------------------------------------------------------
--Should Work--
EXEC usp_addCourse
	@coursePrefix='CS', @courseNumber='1410', @programPrefix='CS', @courseName='CS1410', @defaultCredits='4', @active='Y'

--Should Fail--
EXEC usp_addCourse
	@coursePrefix='CS', @courseNumber='1410', @programPrefix='ART', @courseName='CS1410', @defaultCredits='4', @active='Y'

--Should fail(?)
--Raises questions about the coursePrefix argument
EXEC usp_addCourse
	@coursePrefix='ART', @courseNumber='1410', @programPrefix='CS', @courseName='CS1410', @defaultCredits='4', @active='Y'

SELECT * FROM Course
------usp_addClassroom Test ---------------------------------------------------
--Should work--
EXEC usp_addClassroom
	@buildingPrefix='TE', @roomNumber='530', @classroomCapacity='20', @computers='10', @availableFromTime='', @availableToTime='', @active='N' 

--Should fail--
EXEC usp_addClassroom
	@buildingPrefix='LL', @roomNumber='530', @classroomCapacity='20', @computers='10', @availableFromTime='', @availableToTime='', @active='N' 

SELECT * FROM Classroom

------usp_addSection Test -----------------------------------------------------
--Should Work--
EXEC usp_addSection
	@coursePrefix='CS', @courseNumber='1410', @buildingPrefix='TE', @roomNumber='530', @instructorWNumber='W0100007', @semesterType='Fall', @semesterYear=2017,
	@crn='', @daysTaught='MWF', @courseStartTime='7:30AM', @courseEndTime='9:20PM', @block='S', @courseType='TRAD', @pay='Reg', @sectionCapacity='30', @creditLoad=4,
	@creditOverload=0, @comments='Comment'

--Should fail, and does
EXEC usp_addSection
	@coursePrefix='C', @courseNumber='1410', @buildingPrefix='TE', @roomNumber='530', @instructorWNumber='W0100007', @semesterType='Fall', @semesterYear=2017,
	@crn='', @daysTaught='MWF', @courseStartTime='7:30AM', @courseEndTime='9:20PM', @block='S', @courseType='TRAD', @pay='Reg', @sectionCapacity='', @creditLoad=4,
	@creditOverload=0, @comments='Comment'

--Should fail, and does
EXEC usp_addSection
	@coursePrefix='CS', @courseNumber='9999', @buildingPrefix='TE', @roomNumber='530', @instructorWNumber='W0100007', @semesterType='Fall', @semesterYear=2017,
	@crn='', @daysTaught='MWF', @courseStartTime='7:30AM', @courseEndTime='9:20PM', @block='S', @courseType='TRAD', @pay='Reg', @sectionCapacity='', @creditLoad=4,
	@creditOverload=0, @comments='Comment'

--Should fail, but doesn't
--Needs to test to make sure the building and classroom are valid
EXEC usp_addSection
	@coursePrefix='CS', @courseNumber='1410', @buildingPrefix='AB', @roomNumber='530', @instructorWNumber='W0100007', @semesterType='Fall', @semesterYear=2017,
	@crn='', @daysTaught='MWF', @courseStartTime='7:30AM', @courseEndTime='9:20PM', @block='S', @courseType='TRAD', @pay='Reg', @sectionCapacity='', @creditLoad=4,
	@creditOverload=0, @comments='Comment'

--Should fail, but doesn't
EXEC usp_addSection
	@coursePrefix='CS', @courseNumber='1410', @buildingPrefix='TE', @roomNumber='7530', @instructorWNumber='W0100007', @semesterType='Fall', @semesterYear=2017,
	@crn='', @daysTaught='MWF', @courseStartTime='7:30AM', @courseEndTime='9:20PM', @block='S', @courseType='TRAD', @pay='Reg', @sectionCapacity='', @creditLoad=4,
	@creditOverload=0, @comments='Comment'

--Should fail, but doesn't
--Why are we allowing a null instructor_id in section?
EXEC usp_addSection
	@coursePrefix='CS', @courseNumber='1410', @buildingPrefix='TE', @roomNumber='530', @instructorWNumber='WXXXXXXX', @semesterType='Fall', @semesterYear=2017,
	@crn='', @daysTaught='MWF', @courseStartTime='7:30AM', @courseEndTime='9:20PM', @block='S', @courseType='TRAD', @pay='Reg', @sectionCapacity='', @creditLoad=4,
	@creditOverload=0, @comments='Comment'

--Should fail, and does
EXEC usp_addSection
	@coursePrefix='CS', @courseNumber='1410', @buildingPrefix='TE', @roomNumber='530', @instructorWNumber='W0100007', @semesterType='Sprjiolk', @semesterYear=2017,
	@crn='', @daysTaught='MWF', @courseStartTime='7:30AM', @courseEndTime='9:20PM', @block='S', @courseType='TRAD', @pay='Reg', @sectionCapacity='', @creditLoad=4,
	@creditOverload=0, @comments='Comment'

--Should fail, but doesn't
EXEC usp_addSection 
	@coursePrefix='CS', @courseNumber='1410', @buildingPrefix='TE', @roomNumber='530', @instructorWNumber='W0100007', @semesterType='Fall', @semesterYear=3000,
	@crn='', @daysTaught='MWF', @courseStartTime='7:30AM', @courseEndTime='9:20PM', @block='S', @courseType='TRAD', @pay='Reg', @sectionCapacity='', @creditLoad=4,
	@creditOverload=0, @comments='Comment'
SELECT * FROM Section


------usp_addInstructorRelease Test -----------------------------------------------------
--Should Work---
EXEC usp_addInstructorRelease 
	@instructorWNumber='w0100007', @semesterType='Fall', @semesterYear='2017', @releaseDescription='Sabatical', @totalReleaseHours='0'

--Should Fail--
EXEC usp_addInstructorRelease 
	@instructorWNumber='wXXXXXXX', @semesterType='Fall', @semesterYear='2017', @releaseDescription='Sabatical', @totalReleaseHours='0'
EXEC usp_addInstructorRelease 
	@instructorWNumber='wXXXXXXX', @semesterType='', @semesterYear='2800', @releaseDescription='Sabatical"', @totalReleaseHours='0'
EXEC usp_addInstructorRelease 
	@instructorWNumber='wXXXXXXX', @semesterType='Fall', @semesterYear='2800', @releaseDescription='Sabatical"', @totalReleaseHours='0'

SELECT * FROM InstructorRelease

------udt_sectionConflicts Test -----------------------------------------------------

INSERT INTO dbo.Section
   (course_id, classroom_id, instructor_id, semester_id, coursePrefix, 
    courseNumber, buildingPrefix, roomNumber, instructorWNumber, semesterType,
    semesterYear, crn, daysTaught, courseStartTime, courseEndTime, block,
    courseType, pay, sectionCapacity, creditLoad, creditOverload, comments)
VALUES ((SELECT course_id FROM Course WHERE coursePrefix = 'CS'
          AND courseNumber = 4110),
        (SELECT classroom_id FROM Classroom WHERE buildingPrefix = 'D0'
          AND roomNumber = '225'),
        (SELECT instructor_id FROM Instructor WHERE instructorWNumber = 'W0100002'),
        (SELECT semester_id FROM Semester WHERE semesterType = 'Fall'
          AND semesterYear = 2017),
        'CS', '1401', 'D0', '225', ' W0100002', 'Fall', 2017, '30654', 'MW', '10:30AM', '11:20AM', 'S', 'TRAD', 'Reg', '30', 4.0, 0, '42');

INSERT INTO dbo.Section
   (course_id, classroom_id, instructor_id, semester_id, coursePrefix, 
    courseNumber, buildingPrefix, roomNumber, instructorWNumber, semesterType,
    semesterYear, crn, daysTaught, courseStartTime, courseEndTime, block,
    courseType, pay, sectionCapacity, creditLoad, creditOverload, comments)
VALUES ((SELECT course_id FROM Course WHERE coursePrefix = 'CS'
          AND courseNumber = 4110),
        (SELECT classroom_id FROM Classroom WHERE buildingPrefix = 'D0'
          AND roomNumber = '225'),
        (SELECT instructor_id FROM Instructor WHERE instructorWNumber = 'W0100001'),
        (SELECT semester_id FROM Semester WHERE semesterType = 'Fall'
          AND semesterYear = 2017),
        'CS', '1401', 'D0', '225', ' W0100001', 'Fall', 2017, '30654', 'MW', '9:30AM', '11:20AM', 'S', 'TRAD', 'Reg', '30', 4.0, 0, '42');
GO

------udt_creditOverloadCheck Test -----------------------------------------------------
INSERT INTO dbo.InstructorRelease
    (instructor_id, semester_id, instructorWNumber, semesterType,
     semesterYear, releaseDescription, totalReleaseHours)
VALUES ((SELECT instructor_id FROM Instructor WHERE instructorWNumber = 'W0100003'),
         (SELECT semester_id FROM Semester WHERE semesterType = 'Fall'
          AND semesterYear = 2017),
          'W0100003', 'Fall', 2017, '{ "Release" : 13.0 }', 13.0);