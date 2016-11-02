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
    public class InstructorDepartmentsController : Controller
    {
        private ScheduleCreaterEntities db = new ScheduleCreaterEntities();

        // GET: InstructorDepartments
        public ActionResult Index()
        {
            var instructorDepartments = db.InstructorDepartments.Include(i => i.Department).Include(i => i.Instructor);
            return View(instructorDepartments.ToList());
        }

        // GET: InstructorDepartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructorDepartment instructorDepartment = db.InstructorDepartments.Find(id);
            if (instructorDepartment == null)
            {
                return HttpNotFound();
            }
            return View(instructorDepartment);
        }

        // GET: InstructorDepartments/Create
        public ActionResult Create()
        {
            ViewBag.department_id = new SelectList(db.Departments, "department_id", "departmentPrefix");
            ViewBag.instructor_id = new SelectList(db.Instructors, "instructor_id", "instructorWNumber");
            return View();
        }

        // POST: InstructorDepartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "instructorDepartment_id,department_id,instructor_id,departmentPrefix,instructorWNumber")] InstructorDepartment instructorDepartment)
        {
            if (ModelState.IsValid)
            {
                db.InstructorDepartments.Add(instructorDepartment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.department_id = new SelectList(db.Departments, "department_id", "departmentPrefix", instructorDepartment.department_id);
            ViewBag.instructor_id = new SelectList(db.Instructors, "instructor_id", "instructorWNumber", instructorDepartment.instructor_id);
            return View(instructorDepartment);
        }

        // GET: InstructorDepartments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructorDepartment instructorDepartment = db.InstructorDepartments.Find(id);
            if (instructorDepartment == null)
            {
                return HttpNotFound();
            }
            ViewBag.department_id = new SelectList(db.Departments, "department_id", "departmentPrefix", instructorDepartment.department_id);
            ViewBag.instructor_id = new SelectList(db.Instructors, "instructor_id", "instructorWNumber", instructorDepartment.instructor_id);
            return View(instructorDepartment);
        }

        // POST: InstructorDepartments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "instructorDepartment_id,department_id,instructor_id,departmentPrefix,instructorWNumber")] InstructorDepartment instructorDepartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructorDepartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.department_id = new SelectList(db.Departments, "department_id", "departmentPrefix", instructorDepartment.department_id);
            ViewBag.instructor_id = new SelectList(db.Instructors, "instructor_id", "instructorWNumber", instructorDepartment.instructor_id);
            return View(instructorDepartment);
        }

        // GET: InstructorDepartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InstructorDepartment instructorDepartment = db.InstructorDepartments.Find(id);
            if (instructorDepartment == null)
            {
                return HttpNotFound();
            }
            return View(instructorDepartment);
        }

        // POST: InstructorDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InstructorDepartment instructorDepartment = db.InstructorDepartments.Find(id);
            db.InstructorDepartments.Remove(instructorDepartment);
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
