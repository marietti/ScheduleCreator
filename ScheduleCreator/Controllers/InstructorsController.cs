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
    public class InstructorsController : Controller
    {
        private ScheduleCreatorEntities db = new ScheduleCreatorEntities();

        // GET: Instructors
        public ActionResult Index()
        {
            return View(db.Instructors.ToList());
        }

        public ActionResult SectionByInstructor(int? id)
        {
            int semesterId;
            if (id == null)
            {
                semesterId = ((db.Semesters.ToList()).Last()).semester_id;
            }
            else
            {
                semesterId = (int)id;
            }
            List<Instructor> instructorList = new List<Instructor>(db.Instructors.ToList());
            List<Section> tempSectionList = new List<Section>();

            if (db.Instructors != null)
            {
                //instructorList = from i in db.Instructors where 
                foreach (Instructor instructor in instructorList)
                {
                    var InstructorNewList = from s in db.Sections select s;
                    InstructorNewList = InstructorNewList.Where(s => s.instructor_id == instructor.instructor_id && s.semester_id == semesterId);
                    instructor.Sections = InstructorNewList.ToList();
                    /*if (instructor.Sections != null)
                    {
                        foreach (Section section in instructor.Sections)
                        {
                            if (section.semester_id != semesterId)
                            {
                                tempSectionList.Add(section);
                            }
                        }
                        foreach (Section section in tempSectionList)
                        {
                            instructor.Sections.Remove(section);
                        }
                        tempSectionList.Clear();
                    }*/
                }
            }
            ViewBag.semester_id = new SelectList(
               from s in db.Semesters
               orderby s.startDate descending
               select new { s.semester_id, s.semesterType, s.semesterYear, fullName = s.semesterType + " " + s.semesterYear },
               "semester_id", "fullName");
            return View(instructorList);
        }

        public ActionResult InstructorCalendar(int? id)
        {
            int semesterId;
            if (id == null)
            {
                semesterId = ((db.Semesters.ToList()).Last()).semester_id;
            }
            else
            {
                semesterId = (int)id;
            }
            List<Instructor> instructorList = new List<Instructor>(db.Instructors.ToList());
            List<Section> tempSectionList = new List<Section>();

            if (db.Instructors != null)
            {
                //instructorList = from i in db.Instructors where 
                foreach (Instructor instructor in instructorList)
                {
                    var InstructorNewList = from s in db.Sections select s;
                    InstructorNewList = InstructorNewList.Where(s => s.instructor_id == instructor.instructor_id && s.semester_id == semesterId);
                    instructor.Sections = InstructorNewList.ToList();
                }
            }
            ViewBag.semester_id = new SelectList(
               from s in db.Semesters
               orderby s.startDate descending
               select new { s.semester_id, s.semesterType, s.semesterYear, fullName = s.semesterType + " " + s.semesterYear },
               "semester_id", "fullName");
            return View(instructorList);
        }

        // GET: Instructors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // GET: Instructors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "instructor_id,instructorWNumber,instructorFirstName,instructorLastName,hoursRequired")] Instructor instructor, bool active)
        {
            instructor.active = active ? "Y" : "N";
            if (ModelState.IsValid)
            {
                db.Instructors.Add(instructor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.active = active;
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }

            ViewBag.active = instructor.active == "Y" ? true : false;
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "instructor_id,instructorWNumber,instructorFirstName,instructorLastName,hoursRequired")] Instructor instructor, bool active)
        {

            instructor.active = active ? "Y" : "N";
            if (ModelState.IsValid)
            {
                db.Entry(instructor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.active = active;
            return View(instructor);
        }

        // GET: Instructors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instructor instructor = db.Instructors.Find(id);
            db.Instructors.Remove(instructor);
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
        public JsonResult IsWnumberTaken([Bind(Prefix = "instructorWNumber")] string instructorWNumber)
        {
            if (!string.IsNullOrEmpty(instructorWNumber))
            {
                foreach (Instructor i in db.Instructors.ToList())
                {
                    if (i.instructorWNumber == instructorWNumber)
                        return Json("The instructor wnumber aleady exists", JsonRequestBehavior.AllowGet);
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
