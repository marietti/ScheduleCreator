USE ScheduleCreator

BEGIN TRANSACTION

DELETE FROM dbo.Section;
DELETE FROM dbo.Course;
DELETE FROM dbo.Classroom;
DELETE FROM dbo.Building;
DELETE FROM dbo.InstructorRelease;
DELETE FROM dbo.InstructorProgram;
DELETE FROM dbo.Semester;
DELETE FROM dbo.Instructor;
DELETE FROM dbo.Program;

--Instructor Inserts-----------------------------------------------------------
INSERT INTO dbo.Instructor
    (instructorWNumber, instructorFirstName, instructorLastName, hoursRequired, active)
VALUES ('W0100001', 'Garth', 'Tuck', 9, 'Y');

INSERT INTO dbo.Instructor
    (instructorWNumber, instructorFirstName, instructorLastName, hoursRequired, active)
VALUES ('W0100002', 'Spencer', 'Hilton', 12, 'Y');

INSERT INTO dbo.Instructor
    (instructorWNumber, instructorFirstName, instructorLastName, hoursRequired, active)
VALUES ('W0100003', 'Josh', 'Jensen', 12, 'Y');

INSERT INTO dbo.Instructor
    (instructorWNumber, instructorFirstName, instructorLastName, hoursRequired, active)
VALUES ('W0100004', 'Rich', 'Fry', 12, 'Y');

INSERT INTO dbo.Instructor
    (instructorWNumber, instructorFirstName, instructorLastName, hoursRequired, active)
VALUES ('W0100005', 'Brian', 'Rague', 12, 'Y');

INSERT INTO dbo.Instructor
    (instructorWNumber, instructorFirstName, instructorLastName, hoursRequired, active)
VALUES ('W0100006', 'Hugo', 'Valle', 12, 'Y');


--Program Inserts-----------------------------------------------------------
INSERT INTO dbo.Program
    (programPrefix, programName, maxCreditsAllowed)
VALUES ('CS', 'Computer Science', 12.0);


--Course Inserts---------------------------------------------------------------
INSERT INTO dbo.Course
    (program_id, coursePrefix, courseNumber, courseName, programPrefix,
     defaultCredits, active)
VALUES ((SELECT program_id FROM Program WHERE programPrefix = 'CS'),
         'CS', '1400', 'CS 1400', 'CS', 4.0, 'Y');

INSERT INTO dbo.Course
    (program_id, coursePrefix, courseNumber, courseName, programPrefix,
     defaultCredits, active)
VALUES ((SELECT program_id FROM Program WHERE programPrefix = 'CS'),
        'CS', '1010', 'CS 1010', 'CS', 3.0, 'Y');

INSERT INTO dbo.Course
    (program_id, coursePrefix, courseNumber, courseName, programPrefix,
     defaultCredits, active)
VALUES ((SELECT program_id FROM Program WHERE programPrefix = 'CS'),
        'CS', '3030', 'CS 3030', 'CS', 4.0, 'Y');

INSERT INTO dbo.Course
    (program_id, coursePrefix, courseNumber, courseName, programPrefix,
     defaultCredits, active)
VALUES ((SELECT program_id FROM Program WHERE programPrefix = 'CS'),
        'CS', '4110', 'CS 4110', 'CS', 4.0, 'Y');

INSERT INTO dbo.Course
    (program_id, coursePrefix, courseNumber, courseName, programPrefix,
     defaultCredits, active)
VALUES ((SELECT program_id FROM Program WHERE programPrefix = 'CS'),
        'CS', '4800', 'CS 4800', 'CS', 4.0, 'Y');


--Building Inserts-------------------------------------------------------------
INSERT INTO dbo.Building
    (buildingPrefix, buildingName, campusPrefix)
VALUES ('TE', 'Technical Education', 'WSU');

INSERT INTO dbo.Building
    (buildingPrefix, buildingName, campusPrefix)
VALUES ('D2', 'Davis 2', 'WSD');

INSERT INTO dbo.Building
    (buildingPrefix, buildingName, campusPrefix)
VALUES ('D0', 'Davis 0', 'WSD');


--Classroom Inserts------------------------------------------------------------
INSERT INTO dbo.Classroom
    (building_id, buildingPrefix, roomNumber, classroomCapacity, computers,
     availableFromTime, availableToTime, active)
VALUES ((SELECT building_id FROM Building WHERE buildingPrefix = 'TE'),
        'TE', '104', '50', 0, '9:00AM', '10:00PM', 'Y');

INSERT INTO dbo.Classroom
    (building_id, buildingPrefix, roomNumber, classroomCapacity, computers,
     availableFromTime, availableToTime, active)
VALUES ((SELECT building_id FROM Building WHERE buildingPrefix = 'TE'),    
        'TE', '103C', '30', 6, '9:00AM', '10:00PM', 'Y');

INSERT INTO dbo.Classroom
    (building_id, buildingPrefix, roomNumber, classroomCapacity, computers,
     availableFromTime, availableToTime, active)
VALUES ((SELECT building_id FROM Building WHERE buildingPrefix = 'TE'),
        'TE', '103D', '30', 30, '9:00AM', '10:00PM', 'Y');

INSERT INTO dbo.Classroom
    (building_id, buildingPrefix, roomNumber, classroomCapacity, computers,
     availableFromTime, availableToTime, active)
VALUES ((SELECT building_id FROM Building WHERE buildingPrefix = 'TE'),
        'TE', 'CS conf rm', '10', 0, '9:00AM', '10:00PM', 'Y');

INSERT INTO dbo.Classroom
    (building_id, buildingPrefix, roomNumber, classroomCapacity, computers,
     availableFromTime, availableToTime, active)
VALUES ((SELECT building_id FROM Building WHERE buildingPrefix = 'TE'),
        'D0', '225', '30', 42, '9:00AM', '10:00PM', 'Y');

INSERT INTO dbo.Classroom
    (building_id, buildingPrefix, roomNumber, classroomCapacity, computers,
     availableFromTime, availableToTime, active)
VALUES ((SELECT building_id FROM Building WHERE buildingPrefix = 'TE'),
        'D2', '301', '30', 15, '9:00AM', '10:00PM', 'Y');


--Semester Inserts-------------------------------------------------------------
INSERT INTO dbo.Semester
    (semesterType, semesterYear, startDate, endDate)
VALUES ('Fall', 2017, '2017-01-09', '2017-04-21');


--InstructorRelease Inserts----------------------------------------------------
INSERT INTO dbo.InstructorRelease
    (instructor_id, semester_id, instructorWNumber, semesterType,
     semesterYear, releaseDescription, totalReleaseHours)
VALUES ((SELECT instructor_id FROM Instructor WHERE instructorWNumber = 'W0100004'),
         (SELECT semester_id FROM Semester WHERE semesterType = 'Fall'
          AND semesterYear = 2017),
          'W0100004', 'Fall', 2017, 'Sabatical', 0.0);

INSERT INTO dbo.InstructorRelease
    (instructor_id, semester_id, instructorWNumber, semesterType,
     semesterYear, releaseDescription, totalReleaseHours)
VALUES ((SELECT instructor_id FROM Instructor WHERE instructorWNumber = 'W0100005'),
         (SELECT semester_id FROM Semester WHERE semesterType = 'Fall'
          AND semesterYear = 2017),
          'W0100005', 'Fall', 2017, 'Dept. Chair: 9.0, CS 4110 : 1', 10.0);

INSERT INTO dbo.InstructorRelease
    (instructor_id, semester_id, instructorWNumber, semesterType,
     semesterYear, releaseDescription, totalReleaseHours)
VALUES ((SELECT instructor_id FROM Instructor WHERE instructorWNumber = 'W0100002'),
         (SELECT semester_id FROM Semester WHERE semesterType = 'Fall'
          AND semesterYear = 2017),
          'W0100002', 'Fall', 2017, 'Program Coordinator: 6, CS 1030: 2', 8.0);

--InstructorProgram Inserts-------------------------------------------------
INSERT INTO dbo.InstructorProgram
    (instructor_id, program_id, instructorWNumber, programPrefix)
VALUES ((SELECT instructor_id FROM Instructor WHERE instructorWNumber = 'W0100001'), 
        (SELECT program_id FROM Program WHERE programPrefix = 'CS'), 
        'W0100001', 'CS');
        
INSERT INTO dbo.InstructorProgram
    (instructor_id, program_id, instructorWNumber, programPrefix)
VALUES ((SELECT instructor_id FROM Instructor WHERE instructorWNumber = 'W0100002'), 
        (SELECT program_id FROM Program WHERE programPrefix = 'CS'), 
        'W0100002', 'CS');

INSERT INTO dbo.InstructorProgram
    (instructor_id, program_id, instructorWNumber, programPrefix)
VALUES ((SELECT instructor_id FROM Instructor WHERE instructorWNumber = 'W0100003'), 
        (SELECT program_id FROM Program WHERE programPrefix = 'CS'), 
        'W0100003', 'CS');

INSERT INTO dbo.InstructorProgram
    (instructor_id, program_id, instructorWNumber, programPrefix)
VALUES ((SELECT instructor_id FROM Instructor WHERE instructorWNumber = 'W0100004'), 
        (SELECT program_id FROM Program WHERE programPrefix = 'CS'), 
        'W0100004', 'CS');

INSERT INTO dbo.InstructorProgram
    (instructor_id, program_id, instructorWNumber, programPrefix)
VALUES ((SELECT instructor_id FROM Instructor WHERE instructorWNumber = 'W0100005'), 
        (SELECT program_id FROM Program WHERE programPrefix = 'CS'), 
        'W0100005', 'CS');

INSERT INTO dbo.InstructorProgram
    (instructor_id, program_id, instructorWNumber, programPrefix)
VALUES ((SELECT instructor_id FROM Instructor WHERE instructorWNumber = 'W0100006'), 
        (SELECT program_id FROM Program WHERE programPrefix = 'CS'), 
        'W0100006', 'CS');


--Section Inserts--------------------------------------------------------------
INSERT INTO dbo.Section
   (course_id, classroom_id, instructor_id, semester_id, coursePrefix, 
    courseNumber, buildingPrefix, roomNumber, instructorWNumber, semesterType,
    semesterYear, crn, daysTaught, courseStartTime, courseEndTime, block,
    courseType, pay, sectionCapacity, creditLoad, creditOverload, comments)
VALUES ((SELECT course_id FROM Course WHERE coursePrefix = 'CS'
          AND courseNumber = '4110'),
        (SELECT classroom_id FROM Classroom WHERE buildingPrefix = 'TE'
          AND roomNumber = '103C'),
        (SELECT instructor_id FROM Instructor WHERE instructorWNumber = 'W0100005'),
        (SELECT semester_id FROM Semester WHERE semesterType = 'Fall'
          AND semesterYear = 2017),
        'CS', '4110', 'TE', '103C', ' W0100005', 'Fall', 2017, '', 'MW', '7:30AM', '9:20AM', 'S', 'TRAD', 'Reg/CE', '', 3, 1, 'Comment 1');

INSERT INTO dbo.Section
   (course_id, classroom_id, instructor_id, semester_id, coursePrefix, 
    courseNumber, buildingPrefix, roomNumber, instructorWNumber, semesterType,
    semesterYear, crn, daysTaught, courseStartTime, courseEndTime, block,
    courseType, pay, sectionCapacity, creditLoad, creditOverload, comments)
VALUES ((SELECT course_id FROM Course WHERE coursePrefix = 'CS'
          AND courseNumber = '4800'),
        (SELECT classroom_id FROM Classroom WHERE buildingPrefix = ''
          AND roomNumber = ''),
        (SELECT instructor_id FROM Instructor WHERE instructorWNumber = 'W0100003'),
        (SELECT semester_id FROM Semester WHERE semesterType = 'Fall'
          AND semesterYear = 2017),
        'CS', '4800', '', '', ' W0100003', 'Fall', 2017, '', '', '', '', 'S', 'HYB', 'TI', '30', 0, 1.0, 'Comet Hailey');

INSERT INTO dbo.Section
   (course_id, classroom_id, instructor_id, semester_id, coursePrefix, 
    courseNumber, buildingPrefix, roomNumber, instructorWNumber, semesterType,
    semesterYear, crn, daysTaught, courseStartTime, courseEndTime, block,
    courseType, pay, sectionCapacity, creditLoad, creditOverload, comments)
VALUES ((SELECT course_id FROM Course WHERE coursePrefix = 'CS'
          AND courseNumber = 4110),
        (SELECT classroom_id FROM Classroom WHERE buildingPrefix = 'TE'
          AND roomNumber = '4800'),
        (SELECT instructor_id FROM Instructor WHERE instructorWNumber = 'W0100003'),
        (SELECT semester_id FROM Semester WHERE semesterType = 'Fall'
          AND semesterYear = 2017),
        'CS', '4800', '', '', ' W0100003', 'Fall', 2017, '', '', '', '', 'S', 'TRAD', 'TI', '30', 0, 2.0, 'Concert');

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
        'CS', '1400', 'D0', '225', ' W0100002', 'Fall', 2017, '30654', 'MW', '7:30AM', '9:20AM', 'S', 'TRAD', 'Reg', '30', 4.0, 0, '42');

INSERT INTO dbo.Section
   (course_id, classroom_id, instructor_id, semester_id, coursePrefix, 
    courseNumber, buildingPrefix, roomNumber, instructorWNumber, semesterType,
    semesterYear, crn, daysTaught, courseStartTime, courseEndTime, block,
    courseType, pay, sectionCapacity, creditLoad, creditOverload, comments)
VALUES ((SELECT course_id FROM Course WHERE coursePrefix = 'CS'
          AND courseNumber = '3030'),
        (SELECT classroom_id FROM Classroom WHERE buildingPrefix = 'TE'
          AND roomNumber = '103D'),
        (SELECT instructor_id FROM Instructor WHERE instructorWNumber = 'W0100006'),
        (SELECT semester_id FROM Semester WHERE semesterType = 'Fall'
          AND semesterYear = 2017),
        'CS', '3030', 'TE', '103D', ' W0100006', 'Fall', 2017, '', 'TR', '11:30AM', '1:20PM', 'S', 'TRAD', 'Reg', '', 4.0, 0, '73');

INSERT INTO dbo.Section
   (course_id, classroom_id, instructor_id, semester_id, coursePrefix, 
    courseNumber, buildingPrefix, roomNumber, instructorWNumber, semesterType,
    semesterYear, crn, daysTaught, courseStartTime, courseEndTime, block,
    courseType, pay, sectionCapacity, creditLoad, creditOverload, comments)
VALUES ((SELECT course_id FROM Course WHERE coursePrefix = 'CS'
          AND courseNumber = '1010'),
        (SELECT classroom_id FROM Classroom WHERE buildingPrefix = 'D2'
          AND roomNumber = '301'),
        (SELECT instructor_id FROM Instructor WHERE instructorWNumber = 'W0100001'),
        (SELECT semester_id FROM Semester WHERE semesterType = 'Fall'
          AND semesterYear = 2017),
        'CS', '1010', 'D2', '301', ' W0100001', 'Fall', 2017, '', 'TR', '7:30AM', '8:45AM', 'S', 'TRAD', 'Reg', '', 3.0, 0, '85');



SELECT * FROM dbo.Instructor;
SELECT * FROM dbo.Program;
SELECT * FROM dbo.Course;
SELECT * FROM dbo.Classroom;
SELECT * FROM dbo.Building;
SELECT * FROM dbo.Semester;
SELECT * FROM dbo.InstructorRelease;
SELECT * FROM dbo.InstructorProgram;
SELECT * FROM dbo.Section;

COMMIT;