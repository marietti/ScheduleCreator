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
    public class InstructorProgramsController : Controller
    {
        private ScheduleCreatorEntities db = new ScheduleCreatorEntities();

        // GET: InstructorPrograms
        public ActionResult Index()
        {
            var instructorPrograms = db.InstructorPrograms.Include(i => i.Program).Include(i => i.Instructor);
            return View(instructorPrograms.ToList());
        }

        // GET: InstructorPrograms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructorProgram instructorProgram = db.InstructorPrograms.Find(id);
            if (instructorProgram == null)
            {
                return HttpNotFound();
            }
            return View(instructorProgram);
        }

        // GET: InstructorPrograms/Create
        public ActionResult Create()
        {
            ViewBag.program_id = new SelectList(
                 from p in db.Programs
                 orderby p.programPrefix
                 select new { p.program_id, p.programPrefix, p.programName, fullName = p.programPrefix + " - " + p.programName },
                 "program_id", "fullName");
            ViewBag.instructor_id = new SelectList(
                 from i in db.Instructors
                 orderby i.instructorLastName, i.instructorFirstName
                 select new { i.instructor_id, i.instructorFirstName, i.instructorLastName, fullName = i.instructorLastName + ", " + i.instructorFirstName },
                 "instructor_id", "fullName");
            return View();
        }

        // POST: InstructorPrograms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "instructorProgram_id,program_id,instructor_id")] InstructorProgram instructorProgram)
        {
            // programPrefix
            instructorProgram.programPrefix = (from p in db.Programs where p.program_id == instructorProgram.program_id select p.programPrefix).ToList().FirstOrDefault();

            // instructorWNumber
            instructorProgram.instructorWNumber = (from i in db.Instructors where i.instructor_id == instructorProgram.instructor_id select i.instructorWNumber).ToList().FirstOrDefault();

            try
            {
                if (ModelState.IsValid)
                {
                    db.InstructorPrograms.Add(instructorProgram);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                if (e.InnerException.InnerException.Message.Contains("UNIQUE KEY constraint"))
                {
                    ModelState.AddModelError("instructor_id", "The instructor has already been assigned to that program");
                }
                else
                {
                    ModelState.AddModelError("instructor_id", e.InnerException.InnerException.Message);
                }
            }

            ViewBag.program_id = new SelectList(
                 from p in db.Programs
                 orderby p.programPrefix
                 select new { p.program_id, p.programPrefix, p.programName, fullName = p.programPrefix + " - " + p.programName },
                 "program_id", "fullName", instructorProgram.program_id);
            ViewBag.instructor_id = new SelectList(
                 from i in db.Instructors
                 orderby i.instructorLastName, i.instructorFirstName
                 select new { i.instructor_id, i.instructorFirstName, i.instructorLastName, fullName = i.instructorLastName + ", " + i.instructorFirstName },
                 "instructor_id", "fullName", instructorProgram.instructor_id);

            return View(instructorProgram);
        }

        // GET: InstructorPrograms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructorProgram instructorProgram = db.InstructorPrograms.Find(id);
            if (instructorProgram == null)
            {
                return HttpNotFound();
            }
            ViewBag.program_id = new SelectList(
                 from p in db.Programs
                 orderby p.programPrefix
                 select new { p.program_id, p.programPrefix, p.programName, fullName = p.programPrefix + " - " + p.programName },
                 "program_id", "fullName", instructorProgram.program_id);
            ViewBag.instructor_id = new SelectList(
                 from i in db.Instructors
                 orderby i.instructorLastName, i.instructorFirstName
                 select new { i.instructor_id, i.instructorFirstName, i.instructorLastName, fullName = i.instructorLastName + ", " + i.instructorFirstName },
                 "instructor_id", "fullName", instructorProgram.instructor_id);
            return View(instructorProgram);
        }

        // POST: InstructorPrograms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "instructorProgram_id,program_id,instructor_id")] InstructorProgram instructorProgram)
        {
            // programPrefix
            instructorProgram.programPrefix = (from p in db.Programs where p.program_id == instructorProgram.program_id select p.programPrefix).ToList().FirstOrDefault();

            // instructorWNumber
            instructorProgram.instructorWNumber = (from i in db.Instructors where i.instructor_id == instructorProgram.instructor_id select i.instructorWNumber).ToList().FirstOrDefault();

            if (ModelState.IsValid)
            {
                db.Entry(instructorProgram).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.program_id = new SelectList(
                 from p in db.Programs
                 orderby p.programPrefix
                 select new { p.program_id, p.programPrefix, p.programName, fullName = p.programPrefix + " - " + p.programName },
                 "program_id", "fullName", instructorProgram.program_id);
            ViewBag.instructor_id = new SelectList(
                 from i in db.Instructors
                 orderby i.instructorLastName, i.instructorFirstName
                 select new { i.instructor_id, i.instructorFirstName, i.instructorLastName, fullName = i.instructorLastName + ", " + i.instructorFirstName },
                 "instructor_id", "fullName", instructorProgram.instructor_id);

            return View(instructorProgram);
        }

        // GET: InstructorPrograms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructorProgram instructorProgram = db.InstructorPrograms.Find(id);
            if (instructorProgram == null)
            {
                return HttpNotFound();
            }
            return View(instructorProgram);
        }

        // POST: InstructorPrograms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InstructorProgram instructorProgram = db.InstructorPrograms.Find(id);
            db.InstructorPrograms.Remove(instructorProgram);
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
