using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScheduleCreator.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

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
                    /*var InstructorNewList = from s in db.Sections select s;
                    InstructorNewList = InstructorNewList.Where(s => s.instructor_id == instructor.instructor_id && s.semester_id == semesterId);
                    instructor.Sections = InstructorNewList.ToList();*/
                    if (instructor.Sections != null)
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
                    }
                }
            }
            ViewBag.semester_id = db.Semesters.ToList();
            return View(instructorList);
        }

        public ActionResult InstructorCalendar()
        {
            Dictionary<int, string> instructorEvents = new Dictionary<int, string>();
            foreach (Instructor instructor in db.Instructors)
            {
                foreach (Section section in instructor.Sections)
                {
                    if (section.daysTaught != null)
                    {
                        List<Calendar.Event> events = new List<Calendar.Event>();
                        foreach (char day in section.daysTaught)
                        {
                            string daysToAdd = "0";
                            daysToAdd = char.ToLower(day) == 'm' ? "1" : daysToAdd;
                            daysToAdd = char.ToLower(day) == 't' ? "2" : daysToAdd;
                            daysToAdd = char.ToLower(day) == 'w' ? "3" : daysToAdd;
                            daysToAdd = char.ToLower(day) == 'r' ? "4" : daysToAdd;
                            daysToAdd = char.ToLower(day) == 'f' ? "5" : daysToAdd;
                            daysToAdd = char.ToLower(day) == 's' ? "6" : daysToAdd;

                            string date = "0" + daysToAdd + "-01-1900";
                            string startTime = section.courseStartTime.ToString();
                            string endTime = section.courseEndTime.ToString();
                            startTime = date + " " + startTime;
                            endTime = date + " " + endTime;

                            string id = section.section_id.ToString();
                            string title = section.coursePrefix + section.courseNumber + "<br/>" + section.buildingPrefix + section.roomNumber;
                            string start = startTime;
                            string end = endTime;
                            Calendar.Event temp = new Calendar.Event(id, title, start, end);
                            events.Add(temp);
                        }
                        string jsonEvents = JsonConvert.SerializeObject(events);
                        instructorEvents.Add(section.section_id, jsonEvents);
                    }
                }
            }
            ViewBag.instructorEvents = instructorEvents;
            return View(db.Instructors.ToList());
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
