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
    public class InstructorReleasesController : Controller
    {
        private ScheduleCreaterEntities db = new ScheduleCreaterEntities();

        // GET: InstructorReleases
        public ActionResult Index()
        {
            var instructorReleases = db.InstructorReleases.Include(i => i.Instructor).Include(i => i.Semester);
            return View(instructorReleases.ToList());
        }

        // GET: InstructorReleases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructorRelease instructorRelease = db.InstructorReleases.Find(id);
            if (instructorRelease == null)
            {
                return HttpNotFound();
            }
            return View(instructorRelease);
        }

        // GET: InstructorReleases/Create
        public ActionResult Create()
        {
            ViewBag.instructor_id = new SelectList(db.Instructors, "instructor_id", "instructorWNumber");
            ViewBag.semester_id = new SelectList(db.Semesters, "semester_id", "semesterType");
            return View();
        }

        // POST: InstructorReleases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "instructorRelease_id,instructor_id,semester_id,instructorWNumber,semesterType,semesterYear,releaseDescription,totalReleaseHours")] InstructorRelease instructorRelease)
        {
            if (ModelState.IsValid)
            {
                db.InstructorReleases.Add(instructorRelease);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.instructor_id = new SelectList(db.Instructors, "instructor_id", "instructorWNumber", instructorRelease.instructor_id);
            ViewBag.semester_id = new SelectList(db.Semesters, "semester_id", "semesterType", instructorRelease.semester_id);
            return View(instructorRelease);
        }

        // GET: InstructorReleases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructorRelease instructorRelease = db.InstructorReleases.Find(id);
            if (instructorRelease == null)
            {
                return HttpNotFound();
            }
            ViewBag.instructor_id = new SelectList(db.Instructors, "instructor_id", "instructorWNumber", instructorRelease.instructor_id);
            ViewBag.semester_id = new SelectList(db.Semesters, "semester_id", "semesterType", instructorRelease.semester_id);
            return View(instructorRelease);
        }

        // POST: InstructorReleases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "instructorRelease_id,instructor_id,semester_id,instructorWNumber,semesterType,semesterYear,releaseDescription,totalReleaseHours")] InstructorRelease instructorRelease)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructorRelease).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.instructor_id = new SelectList(db.Instructors, "instructor_id", "instructorWNumber", instructorRelease.instructor_id);
            ViewBag.semester_id = new SelectList(db.Semesters, "semester_id", "semesterType", instructorRelease.semester_id);
            return View(instructorRelease);
        }

        // GET: InstructorReleases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructorRelease instructorRelease = db.InstructorReleases.Find(id);
            if (instructorRelease == null)
            {
                return HttpNotFound();
            }
            return View(instructorRelease);
        }

        // POST: InstructorReleases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InstructorRelease instructorRelease = db.InstructorReleases.Find(id);
            db.InstructorReleases.Remove(instructorRelease);
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
