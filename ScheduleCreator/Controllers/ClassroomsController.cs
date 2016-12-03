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
    public class ClassroomsController : Controller
    {
        private ScheduleCreatorEntities db = new ScheduleCreatorEntities();

        // GET: Classrooms
        public ActionResult Index()
        {
            var classrooms = db.Classrooms.Include(c => c.Building);
            return View(classrooms.ToList());
        }
        public ActionResult SectionByClassroom()
        {
            var classrooms = db.Classrooms.Include(c => c.Building);
            return View(classrooms.ToList());
        }

        // GET: Classrooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            return View(classroom);
        }

        // GET: Classrooms/Create
        public ActionResult Create()
        {
            ViewBag.building_id = new SelectList(
                 from b in db.Buildings
                 select new { b.building_id, b.buildingPrefix, b.buildingName, fullName = b.buildingPrefix + " - " + b.buildingName },
                 "building_id", "fullName");
            return PartialView();
        }

        // POST: Classrooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "classroom_id,building_id,buildingPrefix,roomNumber,classroomCapacity,computers,availableFromTime,availableToTime,active")] Classroom classroom)
        {
            if (ModelState.IsValid)
            {
                db.Classrooms.Add(classroom);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.building_id = new SelectList(
                 from b in db.Buildings
                 select new { b.building_id, b.buildingPrefix, b.buildingName, fullName = b.buildingPrefix + " - " + b.buildingName },
                 "building_id", "fullName", classroom.building_id);
            return PartialView(classroom);
        }

        // GET: Classrooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            ViewBag.building_id = new SelectList(
                 from b in db.Buildings
                 select new { b.building_id, b.buildingPrefix, b.buildingName, fullName = b.buildingPrefix + " - " + b.buildingName },
                 "building_id", "fullName", classroom.building_id);
            return View(classroom);
        }

        // POST: Classrooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "classroom_id,building_id,buildingPrefix,roomNumber,classroomCapacity,computers,availableFromTime,availableToTime,active")] Classroom classroom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classroom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.classroom_id = new SelectList(
                 from b in db.Buildings
                 select new { b.building_id, b.buildingPrefix, b.buildingName, fullName = b.buildingPrefix + " " + b.buildingName },
                 "building_id", "fullName", classroom.building_id);
            return View(classroom);
        }

        // GET: Classrooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classroom classroom = db.Classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            return View(classroom);
        }

        // POST: Classrooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Classroom classroom = db.Classrooms.Find(id);
            db.Classrooms.Remove(classroom);
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
