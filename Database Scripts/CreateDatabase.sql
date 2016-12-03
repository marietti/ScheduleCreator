--Kyle Richetti
USE master;

-- Drop the database if it exists
--IF EXISTS(SELECT * FROM sys.sysdatabases WHERE name='ScheduleCreator')--
--    DROP DATABASE ScheduleCreator;

--CREATE DATABASE [ScheduleCreator]
-- ON  PRIMARY
--( NAME = N'ScheduleCreator', FILENAME =
-- N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\ScheduleCreator.mdf',
-- SIZE = 5120KB , FILEGROWTH = 1024KB )
--LOG ON
--( NAME = N'ScheduleCreator_log', FILENAME =
--N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\ScheduleCreator_log.ldf',
--SIZE = 2048KB , FILEGROWTH = 10%);
--GO

--Since we're creating the constraints after the tables are created, we can create the tables in any order
--Any foreign key that references another table can't be created until the table it's referencing has been created
--Good idea to create the prime tables (tables without foreign keys) first
--Need drop table statements at the top for testing purposes
--Prime tables need to be dropped last (Everything that is dependent on them needs to be dropped first)

-- Make sure we are attached to the correct database
--USE ScheduleCreator;

IF EXISTS(SELECT * FROM sys.sysdatabases WHERE name='3750User')
    DROP DATABASE [3750User];

CREATE DATABASE [3750User]
 ON  PRIMARY
( NAME = N'3750User', FILENAME =
 N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\3750User.mdf',
 SIZE = 5120KB , FILEGROWTH = 1024KB )
LOG ON
( NAME = N'3750User_log', FILENAME =
N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\3750User_log.ldf',
SIZE = 2048KB , FILEGROWTH = 10%);
GO

USE [3750User];

-- Drop tables if they exist
-- Tables must be dropped in order
IF EXISTS (
  SELECT * FROM sys.tables WHERE name = N'Section'
) DROP TABLE Section;

IF EXISTS (
  SELECT * FROM sys.tables WHERE name = N'Classroom'
) DROP TABLE Classroom;

IF EXISTS (
  SELECT * FROM sys.tables WHERE name = N'Building'
) DROP TABLE Building;

IF EXISTS (
  SELECT * FROM sys.tables WHERE name = N'InstructorRelease'
) DROP TABLE InstructorRelease;

IF EXISTS (
  SELECT * FROM sys.tables WHERE name = N'Semester'
) DROP TABLE Semester;

IF EXISTS (
  SELECT * FROM sys.tables WHERE name = N'InstructorProgram'
) DROP TABLE InstructorProgram;

IF EXISTS (
  SELECT * FROM sys.tables WHERE name = N'Instructor'
) DROP TABLE Instructor;

IF EXISTS (
  SELECT * FROM sys.tables WHERE name = N'Program'
) DROP TABLE Program;

IF EXISTS (
  SELECT * FROM sys.tables WHERE name = N'Course'
) DROP TABLE Course;



CREATE TABLE [InstructorRelease] (
    [instructorRelease_id] int IDENTITY(1000,1) NOT NULL,
    [instructor_id] int NOT NULL,
    [semester_id] int NOT NULL,
    [instructorWNumber] nvarchar(9) NOT NULL,
    [semesterType] nvarchar(10) NOT NULL,
    [semesterYear] int NOT NULL,
    [releaseDescription] nvarchar(255) NOT NULL,
    [totalReleaseHours] decimal
);

CREATE TABLE [InstructorProgram] (
    [instructorProgram_id] int IDENTITY(1000,1) NOT NULL,
    [program_id] int NOT NULL,
    [instructor_id] int NOT NULL,
    [programPrefix] nvarchar(10) NOT NULL,
    [instructorWNumber] nvarchar(9) NOT NULL,
);

CREATE TABLE [Program] (
    [program_id] int IDENTITY(1000,1) NOT NULL,
    [programPrefix] nvarchar(10) NOT NULL,
    [programName] nvarchar(100) NOT NULL,
    [maxCreditsAllowed] decimal NOT NULL,
);

CREATE TABLE [Course] (
    [course_id] int IDENTITY(1000,1) NOT NULL,
    [program_id] int NOT NULL,
    [coursePrefix] nvarchar(10) NOT NULL,
    [courseNumber] nvarchar(10) NOT NULL,
    [programPrefix] nvarchar(10) NOT NULL,
    [courseName] nvarchar(100) NOT NULL,
    [defaultCredits] decimal,
    [active] nvarchar(5) NOT NULL,
);

CREATE TABLE [Instructor] (
    [instructor_id] int IDENTITY(1000,1) NOT NULL,
    [instructorWNumber] nvarchar(9) NOT NULL,
    [instructorFirstName] nvarchar(50) NOT NULL,
    [instructorLastName] nvarchar(50) NOT NULL,
    [hoursRequired] decimal NOT NULL,
    [active] nvarchar(5) NOT NULL,
);

CREATE TABLE [Semester] (
    [semester_id] int IDENTITY(1000,1) NOT NULL,
    [semesterType] nvarchar(10) NOT NULL,
    [semesterYear] int NOT NULL,
    [startDate] date NOT NULL,
    [endDate] date NOT NULL,
);

CREATE TABLE [Classroom] (
    [classroom_id] int IDENTITY(1000,1) NOT NULL,
    [building_id] int NOT NULL,
    [buildingPrefix] nvarchar(10) NOT NULL,
    [roomNumber] nvarchar(10) NOT NULL,
    [classroomCapacity] int NOT NULL,
    [computers] int NOT NULL,
    [availableFromTime] time NOT NULL,
    [availableToTime] time NOT NULL,
    [active] nvarchar(4) NOT NULL,
);

CREATE TABLE [Building] (
    [building_id] int IDENTITY(1000,1) NOT NULL,
    [buildingPrefix] nvarchar(10) NOT NULL,
    [buildingName] nvarchar(100) NOT NULL,
    [campusPrefix] nvarchar(10) NOT NULL,
);

CREATE TABLE [Section] (
    [section_id] int IDENTITY(1000,1) NOT NULL,
    [course_id] int NOT NULL,
    [classroom_id] int,
    [instructor_id] int,
    [semester_id] int,
    [coursePrefix] nvarchar(10) NOT NULL,
    [courseNumber] nvarchar(10) NOT NULL,
    [buildingPrefix] nvarchar(10),
    [roomNumber] nvarchar(10),
    [instructorWNumber] nvarchar(9),
    [semesterType] nvarchar(10),
    [semesterYear] int,
    [crn] nvarchar(10),
    [daysTaught] nvarchar(10) NOT NULL,
    [courseStartTime] time NOT NULL,
    [courseEndTime] time NOT NULL,
    [block] nvarchar(5) NOT NULL,
    [courseType] nvarchar(10) NOT NULL,
    [pay] nvarchar(50),
    [sectionCapacity] int NOT NULL,
    [creditLoad] decimal,
    [creditOverload] decimal,
    [comments] nvarchar(255),
);
GO

--Add Primary Keys
ALTER TABLE Building
	ADD CONSTRAINT PK_Building
	PRIMARY KEY CLUSTERED (building_id);

ALTER TABLE Classroom
	ADD CONSTRAINT PK_Classroom
	PRIMARY KEY CLUSTERED (classroom_id);

ALTER TABLE Course
	ADD CONSTRAINT PK_Course
	PRIMARY KEY CLUSTERED (course_id);

ALTER TABLE Program
	ADD CONSTRAINT PK_Program
	PRIMARY KEY CLUSTERED (program_id);

ALTER TABLE Instructor
	ADD CONSTRAINT PK_Instructor
	PRIMARY KEY CLUSTERED (instructor_id);

ALTER TABLE InstructorProgram
	ADD CONSTRAINT PK_InstructorProgram
	PRIMARY KEY CLUSTERED (instructorProgram_id);

ALTER TABLE InstructorRelease
	ADD CONSTRAINT PK_InstructorRelease
	PRIMARY KEY CLUSTERED (instructorRelease_id);

ALTER TABLE Section
	ADD CONSTRAINT PK_Section
	PRIMARY KEY CLUSTERED (section_id);

ALTER TABLE Semester
	ADD CONSTRAINT PK_Semester
	PRIMARY KEY CLUSTERED (semester_id);
GO

--Add Alternate Keys
ALTER TABLE InstructorRelease
	ADD CONSTRAINT AK_InstructorRelease UNIQUE(instructorWNumber, semesterType, semesterYear);

ALTER TABLE InstructorProgram
	ADD CONSTRAINT AK_InstructorProgram UNIQUE(programPrefix, instructorWNumber);

ALTER TABLE Program
	ADD CONSTRAINT AK_Program UNIQUE(programPrefix);

ALTER TABLE Course
	ADD CONSTRAINT AK_Course UNIQUE(coursePrefix, courseNumber);

ALTER TABLE Instructor
	ADD CONSTRAINT AK_Instructor UNIQUE(instructorWNumber);

ALTER TABLE Semester
	ADD CONSTRAINT AK_Semester UNIQUE(semesterType, semesterYear);

ALTER TABLE Classroom
	ADD CONSTRAINT AK_Classroom UNIQUE(buildingPrefix, roomNumber);

ALTER TABLE Building
	ADD CONSTRAINT AK_Building UNIQUE(buildingPrefix);
GO

--Add Foreign Keys
ALTER TABLE InstructorRelease
	ADD CONSTRAINT FK_InstructorRelease_Instructor_id
	FOREIGN KEY (instructor_id) REFERENCES Instructor(instructor_id);

ALTER TABLE InstructorRelease
	ADD CONSTRAINT FK_InstructorRelease_Semester_id
	FOREIGN KEY (semester_id) REFERENCES Semester(semester_id);

ALTER TABLE InstructorProgram
	ADD CONSTRAINT FK_InstructorProgram_Instructor_id
	FOREIGN KEY (instructor_id) REFERENCES Instructor(instructor_id);

ALTER TABLE InstructorProgram
	ADD CONSTRAINT FK_InstructorProgram_Program_id
	FOREIGN KEY (program_id) REFERENCES Program(program_id);

ALTER TABLE Course
	ADD CONSTRAINT FK_Course_Program_id
	FOREIGN KEY (program_id) REFERENCES Program(program_id);

ALTER TABLE Classroom
	ADD CONSTRAINT FK_Classroom_Building_id
	FOREIGN KEY (building_id) REFERENCES Building(building_id);

ALTER TABLE Section
	ADD CONSTRAINT FK_Section_Course_id
	FOREIGN KEY (course_id) REFERENCES Course(course_id);

ALTER TABLE Section
	ADD CONSTRAINT FK_Section_Classroom_id
	FOREIGN KEY (classroom_id) REFERENCES Classroom(classroom_id);

ALTER TABLE Section
	ADD CONSTRAINT FK_Section_Instructor_id
	FOREIGN KEY (instructor_id) REFERENCES Instructor(instructor_id);

ALTER TABLE Section
	ADD CONSTRAINT FK_Section_Semester_id
	FOREIGN KEY (semester_id) REFERENCES Semester(semester_id);

GO

--Add Check Constraints
ALTER TABLE Semester
	ADD CONSTRAINT CK_Semester_semesterType
	CHECK (semesterType = 'Spring' or semesterType = 'Summer' or semesterType = 'Fall');

ALTER TABLE Section
	ADD CONSTRAINT CK_Section_semesterType
	CHECK (semesterType = 'Spring' or semesterType = 'Summer' or semesterType = 'Fall');

ALTER TABLE Section
	ADD CONSTRAINT CK_Section_block
	CHECK (block = 'FB' or block = 'SB' or block = 'S');

ALTER TABLE Section
	ADD CONSTRAINT CK_Section_courseType
	CHECK (courseType = 'ONL' or courseType = 'HYB' or courseType = 'TRAD');