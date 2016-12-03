﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScheduleCreator.Models;

namespace ScheduleCreator.Controllers
{
    public class SectionsController : Controller
    {
        private ScheduleCreatorEntities db = new ScheduleCreatorEntities();

        // GET: Sections
        public ActionResult Index()
        {
            var sections = db.Sections.Include(s => s.Classroom).Include(s => s.Course).Include(s => s.Instructor).Include(s => s.Semester);
            return View(sections.ToList());
        }

        // GET: Sections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // GET: Sections/Create
        public ActionResult Create()
        {
            ViewBag.classroom_id = new SelectList(
                 from cl in db.Classrooms
                 orderby cl.buildingPrefix, cl.roomNumber
                 select new { cl.classroom_id, cl.buildingPrefix, cl.roomNumber, fullName = cl.buildingPrefix + " " + cl.roomNumber },
                 "classroom_id", "fullName");
            ViewBag.course_id = new SelectList(
                 from c in db.Courses
                 orderby c.coursePrefix, c.courseNumber
                 select new { c.course_id, c.coursePrefix, c.courseNumber, fullName = c.coursePrefix + " " + c.courseNumber },
                 "course_id", "fullName");
            ViewBag.instructor_id = new SelectList(
                 from i in db.Instructors
                 orderby i.instructorLastName, i.instructorFirstName
                 select new { i.instructor_id, i.instructorFirstName, i.instructorLastName, fullName = i.instructorLastName + ", " + i.instructorFirstName  },
                 "instructor_id", "fullName");
            ViewBag.semester_id = new SelectList(
                from s in db.Semesters
                orderby s.semesterYear descending
                select new { s.semester_id, s.semesterType, s.semesterYear, fullName = s.semesterType + " " + s.semesterYear },
                "semester_id", "fullName");
            
            ViewBag.block = new SelectList(Section.BlockTypes.Keys.ToList());
            ViewBag.courseType = new SelectList(Section.CourseTypes.Keys.ToList());
            return View();
        }

        // POST: Sections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "section_id,course_id,classroom_id,instructor_id,semester_id,crn,daysTaught,courseStartTime,courseEndTime,block,courseType,pay,sectionCapacity,creditLoad,creditOverload,comments")] Section section)
        {
            // Get values for rest of fields based of id
            // coursePrefix,courseNumber
            section.coursePrefix = (from c in db.Courses where c.course_id == section.course_id select c.coursePrefix).ToList()[0];
            section.courseNumber = (from c in db.Courses where c.course_id == section.course_id select c.courseNumber).ToList()[0];

            // buildingPrefix,roomNumber
            section.buildingPrefix = (from cl in db.Classrooms where cl.classroom_id == section.classroom_id select cl.buildingPrefix).ToList()[0];
            section.roomNumber = (from cl in db.Classrooms where cl.classroom_id == section.classroom_id select cl.roomNumber).ToList()[0];

            // instructorWNumber
            section.instructorWNumber = (from i in db.Instructors where i.instructor_id == section.instructor_id select i.instructorWNumber).ToList()[0];

            // semesterType,semesterYear
            section.semesterType = (from s in db.Semesters where s.semester_id == section.semester_id select s.semesterType).ToList()[0];
            section.semesterYear = (from s in db.Semesters where s.semester_id == section.semester_id select s.semesterYear).ToList()[0];

            // Replace selection with shortened form
            section.block = Section.BlockTypes[section.block];
            section.courseType = Section.CourseTypes[section.courseType];
            if (ModelState.IsValid)
            {
                db.Sections.Add(section);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.classroom_id = new SelectList(
                 from cl in db.Classrooms
                 orderby cl.buildingPrefix, cl.roomNumber
                 select new { cl.classroom_id, cl.buildingPrefix, cl.roomNumber, fullName = cl.buildingPrefix + " " + cl.roomNumber },
                 "classroom_id", "fullName", section.classroom_id);
            ViewBag.course_id = new SelectList(
                 from c in db.Courses
                 orderby c.coursePrefix, c.courseNumber
                 select new { c.course_id, c.coursePrefix, c.courseNumber, fullName = c.coursePrefix + " " + c.courseNumber },
                 "course_id", "fullName", section.course_id);
            ViewBag.instructor_id = new SelectList(
                 from i in db.Instructors
                 orderby i.instructorLastName, i.instructorFirstName
                 select new { i.instructor_id, i.instructorFirstName, i.instructorLastName, fullName = i.instructorLastName + ", " + i.instructorFirstName  },
                 "instructor_id", "fullName", section.instructor_id);
            ViewBag.semester_id = new SelectList(
                from s in db.Semesters
                orderby s.semesterYear descending
                select new { s.semester_id, s.semesterType, s.semesterYear, fullName = s.semesterType + " " + s.semesterYear },
                "semester_id", "fullName", section.semester_id);

            // Lookup key based on value. Doesn't work if there are duplicate values.
            string previousBlock = Section.BlockTypes.FirstOrDefault(x => x.Value == section.block).Key;
            string previousCourseType = Section.CourseTypes.FirstOrDefault(x => x.Value == section.courseType).Key;
            ViewBag.block = new SelectList(Section.BlockTypes.Keys.ToList(), previousBlock);
            ViewBag.courseType = new SelectList(Section.CourseTypes.Keys.ToList(), previousCourseType);
            return View(section);
        }

        // GET: Sections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            ViewBag.classroom_id = new SelectList(
                 from cl in db.Classrooms
                 orderby cl.buildingPrefix, cl.roomNumber
                 select new { cl.classroom_id, cl.buildingPrefix, cl.roomNumber, fullName = cl.buildingPrefix + " " + cl.roomNumber },
                 "classroom_id", "fullName", section.classroom_id);
            ViewBag.course_id = new SelectList(
                 from c in db.Courses
                 orderby c.coursePrefix, c.courseNumber
                 select new { c.course_id, c.coursePrefix, c.courseNumber, fullName = c.coursePrefix + " " + c.courseNumber },
                 "course_id", "fullName", section.course_id);
            ViewBag.instructor_id = new SelectList(
                 from i in db.Instructors
                 orderby i.instructorLastName, i.instructorFirstName
                 select new { i.instructor_id, i.instructorFirstName, i.instructorLastName, fullName = i.instructorLastName + ", " + i.instructorFirstName  },
                 "instructor_id", "fullName", section.instructor_id);
            ViewBag.semester_id = new SelectList(
                from s in db.Semesters
                orderby s.semesterYear descending
                select new { s.semester_id, s.semesterType, s.semesterYear, fullName = s.semesterType + " " + s.semesterYear },
                "semester_id", "fullName", section.semester_id);

            // Lookup key based on value. Doesn't work if there are duplicate values.
            string previousBlock = Section.BlockTypes.FirstOrDefault(x => x.Value == section.block).Key;
            string previousCourseType = Section.CourseTypes.FirstOrDefault(x => x.Value == section.courseType).Key;
            ViewBag.block = new SelectList(Section.BlockTypes.Keys.ToList(), previousBlock);
            ViewBag.courseType = new SelectList(Section.CourseTypes.Keys.ToList(), previousCourseType);
            return View(section);
        }

        // POST: Sections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "section_id,course_id,classroom_id,instructor_id,semester_id,crn,daysTaught,courseStartTime,courseEndTime,block,courseType,pay,sectionCapacity,creditLoad,creditOverload,comments")] Section section)
        {
            // Get values for rest of fields based of id
            // coursePrefix,courseNumber
            section.coursePrefix = (from c in db.Courses where c.course_id == section.course_id select c.coursePrefix).ToList()[0];
            section.courseNumber = (from c in db.Courses where c.course_id == section.course_id select c.courseNumber).ToList()[0];

            // buildingPrefix,roomNumber
            section.buildingPrefix = (from cl in db.Classrooms where cl.classroom_id == section.classroom_id select cl.buildingPrefix).ToList()[0];
            section.roomNumber = (from cl in db.Classrooms where cl.classroom_id == section.classroom_id select cl.roomNumber).ToList()[0];

            // instructorWNumber
            section.instructorWNumber = (from i in db.Instructors where i.instructor_id == section.instructor_id select i.instructorWNumber).ToList()[0];

            // semesterType,semesterYear
            section.semesterType = (from s in db.Semesters where s.semester_id == section.semester_id select s.semesterType).ToList()[0];
            section.semesterYear = (from s in db.Semesters where s.semester_id == section.semester_id select s.semesterYear).ToList()[0];

            // Replace selection with shortened form
            section.block = Section.BlockTypes[section.block];
            section.courseType = Section.CourseTypes[section.courseType];

            if (ModelState.IsValid)
            {
                db.Entry(section).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.classroom_id = new SelectList(
                 from cl in db.Classrooms
                 orderby cl.buildingPrefix, cl.roomNumber
                 select new { cl.classroom_id, cl.buildingPrefix, cl.roomNumber, fullName = cl.buildingPrefix + " " + cl.roomNumber },
                 "classroom_id", "fullName", section.classroom_id);
            ViewBag.course_id = new SelectList(
                 from c in db.Courses
                 orderby c.coursePrefix, c.courseNumber
                 select new { c.course_id, c.coursePrefix, c.courseNumber, fullName = c.coursePrefix + " " + c.courseNumber },
                 "course_id", "fullName", section.course_id);
            ViewBag.instructor_id = new SelectList(
                 from i in db.Instructors
                 orderby i.instructorLastName, i.instructorFirstName
                 select new { i.instructor_id, i.instructorFirstName, i.instructorLastName, fullName = i.instructorLastName + ", " + i.instructorFirstName  },
                 "instructor_id", "fullName", section.instructor_id);
            ViewBag.semester_id = new SelectList(
                from s in db.Semesters
                orderby s.semesterYear descending
                select new { s.semester_id, s.semesterType, s.semesterYear, fullName = s.semesterType + " " + s.semesterYear },
                "semester_id", "fullName", section.semester_id);

            // Lookup key based on value. Doesn't work if there are duplicate values.
            string previousBlock = Section.BlockTypes.FirstOrDefault(x => x.Value == section.block).Key;
            string previousCourseType = Section.CourseTypes.FirstOrDefault(x => x.Value == section.courseType).Key;
            ViewBag.block = new SelectList(Section.BlockTypes.Keys.ToList(), previousBlock);
            ViewBag.courseType = new SelectList(Section.CourseTypes.Keys.ToList(), previousCourseType);
            return View(section);
        }

        // GET: Sections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // POST: Sections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Section section = db.Sections.Find(id);
            db.Sections.Remove(section);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
