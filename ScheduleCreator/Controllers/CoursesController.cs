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
    public class CoursesController : Controller
    {
        private ScheduleCreatorEntities db = new ScheduleCreatorEntities();

        // GET: Courses
        public ActionResult Index()
        {
            var courses = db.Courses.Include(c => c.Program);
            return View(courses.ToList());
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            ViewBag.program_id = new SelectList(
                 from p in db.Programs
                 orderby p.programPrefix
                 select new { p.program_id, p.programPrefix, p.programName, fullName = p.programPrefix + " - " + p.programName },
                 "program_id", "fullName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "course_id,program_id,coursePrefix,courseNumber,courseName,defaultCredits")] Course course, bool active)
        {
            // programPrefix
            course.programPrefix = (from p in db.Programs where p.program_id == course.program_id select p.programPrefix).ToList().FirstOrDefault();

            course.active = active ? "Y" : "N";
            try
            {
                if (ModelState.IsValid)
                {
                    db.Courses.Add(course);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                if (e.InnerException.InnerException.Message.Contains("UNIQUE KEY constraint"))
                {
                    ModelState.AddModelError("course_id", "The course has already been created");
                }
                else
                {
                    ModelState.AddModelError("course_id", e.InnerException.InnerException.Message);
                }
            }

            ViewBag.program_id = new SelectList(
                 from p in db.Programs
                 orderby p.programPrefix
                 select new { p.program_id, p.programPrefix, p.programName, fullName = p.programPrefix + " - " + p.programName },
                 "program_id", "fullName", course.program_id);

            ViewBag.active = active;
            return View(course);
        }

        
        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            ViewBag.program_id = new SelectList(
                 from p in db.Programs
                 orderby p.programPrefix
                 select new { p.program_id, p.programPrefix, p.programName, fullName = p.programPrefix + " - " + p.programName },
                 "program_id", "fullName", course.program_id);

            ViewBag.active = course.active == "Y" ? true : false;
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "course_id,program_id,coursePrefix,courseNumber,courseName,defaultCredits")] Course course, bool active)
        {
            // programPrefix
            course.programPrefix = (from p in db.Programs where p.program_id == course.program_id select p.programPrefix).ToList().FirstOrDefault();

            course.active = active ? "Y" : "N";
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.program_id = new SelectList(
                from p in db.Programs
                orderby p.programPrefix
                select new { p.program_id, p.programPrefix, p.programName, fullName = p.programPrefix + " - " + p.programName },
                "program_id", "fullName", course.program_id);

            ViewBag.active = active;
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
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
        public JsonResult IsCourseTaken([Bind(Prefix = "coursePrefix")] string coursePrefix, [Bind(Prefix = "program_id")] int program_id, [Bind(Prefix = "courseNumber")] string courseNumber)
        {
            if (!string.IsNullOrEmpty(coursePrefix) && !string.IsNullOrEmpty(courseNumber))
            {
                foreach (Course c in db.Courses.ToList())
                {
                    if ((c.program_id == program_id) && (c.coursePrefix == coursePrefix) && (c.courseNumber == courseNumber))
                        return Json("The program, course prefix and course number already exists", JsonRequestBehavior.AllowGet);
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
