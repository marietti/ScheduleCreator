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
    public class ClassroomsController : Controller
    {
        private ScheduleCreatorEntities db = new ScheduleCreatorEntities();

        // GET: Classrooms
        public ActionResult Index()
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
                 orderby b.buildingPrefix
                 select new { b.building_id, b.buildingPrefix, b.buildingName, fullName = b.buildingPrefix + " - " + b.buildingName },
                 "building_id", "fullName");
            return View();
        }

        // POST: Classrooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "classroom_id,building_id,roomNumber,classroomCapacity,computers,availableFromTime,availableToTime")] Classroom classroom, bool active)
        {
            // buildingPrefix
            classroom.buildingPrefix = (from b in db.Buildings where b.building_id == classroom.building_id select b.buildingPrefix).ToList().FirstOrDefault();

            classroom.active = active ? "Y" : "N";

            try
            {
                if (ModelState.IsValid)
                {
                    db.Classrooms.Add(classroom);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                if (e.InnerException.InnerException.Message.Contains("UNIQUE KEY constraint"))
                {
                    ModelState.AddModelError("active", "The classroom has already been created");
                }
                else
                {
                    ModelState.AddModelError("active", e.InnerException.InnerException.Message);
                }
            }

            ViewBag.building_id = new SelectList(
                 from b in db.Buildings
                 orderby b.buildingPrefix
                 select new { b.building_id, b.buildingPrefix, b.buildingName, fullName = b.buildingPrefix + " - " + b.buildingName },
                 "building_id", "fullName", classroom.building_id);

            ViewBag.active = active;
            return View(classroom);
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
                 orderby b.buildingPrefix
                 select new { b.building_id, b.buildingPrefix, b.buildingName, fullName = b.buildingPrefix + " - " + b.buildingName },
                 "building_id", "fullName", classroom.building_id);

            ViewBag.active = classroom.active == "Y" ? true : false;
            return View(classroom);
        }

        // POST: Classrooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "classroom_id,building_id,roomNumber,classroomCapacity,computers,availableFromTime,availableToTime")] Classroom classroom, bool active)
        {
            // buildingPrefix
            classroom.buildingPrefix = (from b in db.Buildings where b.building_id == classroom.building_id select b.buildingPrefix).ToList().FirstOrDefault();

            classroom.active = active ? "Y" : "N";
            if (ModelState.IsValid)
            {
                db.Entry(classroom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.building_id = new SelectList(
                 from b in db.Buildings
                 orderby b.buildingPrefix
                 select new { b.building_id, b.buildingPrefix, b.buildingName, fullName = b.buildingPrefix + " - " + b.buildingName },
                 "building_id", "fullName", classroom.building_id);

            ViewBag.active = active;
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
