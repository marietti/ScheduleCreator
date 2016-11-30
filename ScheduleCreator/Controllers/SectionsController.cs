using System;
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
                 from c in db.Classrooms
                 select new { c.classroom_id, c.buildingPrefix, c.roomNumber, fullName = c.buildingPrefix + " " + c.roomNumber },
                 "classroom_id", "fullName");
            ViewBag.course_id = new SelectList(
                 from co in db.Courses
                 select new { co.course_id, co.coursePrefix, co.courseNumber, fullName = co.coursePrefix + " " + co.courseNumber },
                 "course_id", "fullName");
            ViewBag.instructor_id = new SelectList(
                 from i in db.Instructors
                 select new { i.instructor_id, i.instructorFirstName, i.instructorLastName, fullName = i.instructorFirstName + " " + i.instructorLastName },
                 "instructor_id", "fullName");
            ViewBag.semester_id = new SelectList(
                from s in db.Semesters
                select new { s.semester_id, s.semesterType, s.semesterYear, fullName = s.semesterType + " " + s.semesterYear },
                 "semester_id", "fullName");
            return View();
        }

        // POST: Sections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "section_id,course_id,classroom_id,instructor_id,semester_id,coursePrefix,courseNumber,buildingPrefix,roomNumber,instructorWNumber,semesterType,semesterYear,crn,daysTaught,courseStartTime,courseEndTime,block,courseType,pay,sectionCapacity,creditLoad,creditOverload,comments")] Section section)
        {
            if (ModelState.IsValid)
            {
                db.Sections.Add(section);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.classroom_id = new SelectList(
                 from c in db.Classrooms
                 select new { c.classroom_id, c.buildingPrefix, c.roomNumber, fullName = c.buildingPrefix + " " + c.roomNumber },
                 "classroom_id", "fullName", section.classroom_id);
            ViewBag.course_id = new SelectList(
                 from co in db.Courses
                 select new { co.course_id, co.coursePrefix, co.courseNumber, fullName = co.coursePrefix + " " + co.courseNumber },
                 "course_id", "fullName", section.course_id);
            ViewBag.instructor_id = new SelectList(
                 from i in db.Instructors
                 select new { i.instructor_id, i.instructorFirstName, i.instructorLastName, fullName = i.instructorFirstName + " " + i.instructorLastName },
                 "instructor_id", "fullName", section.instructor_id);
            ViewBag.semester_id = new SelectList(
                from s in db.Semesters
                select new { s.semester_id, s.semesterType, s.semesterYear, fullName = s.semesterType + " " + s.semesterYear },
                 "semester_id", "fullName", section.semester_id);
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
                 from c in db.Classrooms
                 select new { c.classroom_id, c.buildingPrefix, c.roomNumber, fullName = c.buildingPrefix + " " + c.roomNumber },
                 "classroom_id", "fullName", section.classroom_id);
            ViewBag.course_id = new SelectList(
                 from co in db.Courses
                 select new { co.course_id, co.coursePrefix, co.courseNumber, fullName = co.coursePrefix + " " + co.courseNumber },
                 "course_id", "fullName", section.course_id);
            ViewBag.instructor_id = new SelectList(
                 from i in db.Instructors
                 select new { i.instructor_id, i.instructorFirstName, i.instructorLastName, fullName = i.instructorFirstName + " " + i.instructorLastName },
                 "instructor_id", "fullName", section.instructor_id);
            ViewBag.semester_id = new SelectList(
                from s in db.Semesters
                select new { s.semester_id, s.semesterType, s.semesterYear, fullName = s.semesterType + " " + s.semesterYear },
                 "semester_id", "fullName", section.semester_id);
            return View(section);
        }

        // POST: Sections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "section_id,course_id,classroom_id,instructor_id,semester_id,coursePrefix,courseNumber,buildingPrefix,roomNumber,instructorWNumber,semesterType,semesterYear,crn,daysTaught,courseStartTime,courseEndTime,block,courseType,pay,sectionCapacity,creditLoad,creditOverload,comments")] Section section)
        {
            if (ModelState.IsValid)
            {
                db.Entry(section).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.classroom_id = new SelectList(
                 from c in db.Classrooms
                 select new { c.classroom_id, c.buildingPrefix, c.roomNumber, fullName = c.buildingPrefix + " " + c.roomNumber },
                 "classroom_id", "fullName", section.classroom_id);
            ViewBag.course_id = new SelectList(
                 from co in db.Courses
                 select new { co.course_id, co.coursePrefix, co.courseNumber, fullName = co.coursePrefix + " " + co.courseNumber },
                 "course_id", "fullName", section.course_id);
            ViewBag.instructor_id = new SelectList(
                 from i in db.Instructors
                 select new { i.instructor_id, i.instructorFirstName, i.instructorLastName, fullName = i.instructorFirstName + " " + i.instructorLastName },
                 "instructor_id", "fullName", section.instructor_id);
            ViewBag.semester_id = new SelectList(
                from s in db.Semesters
                select new { s.semester_id, s.semesterType, s.semesterYear, fullName = s.semesterType + " " + s.semesterYear },
                 "semester_id", "fullName", section.semester_id);
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
