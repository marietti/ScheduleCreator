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
    public class ProgramsController : Controller
    {
        private ScheduleCreatorEntities db = new ScheduleCreatorEntities();

        // GET: Programs
        public ActionResult Index()
        {
            return View(db.Programs.ToList());
        }

        // GET: Programs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Program Program = db.Programs.Find(id);
            if (Program == null)
            {
                return HttpNotFound();
            }
            return View(Program);
        }

        // GET: Programs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Programs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "program_id,programPrefix,ProgramName,maxCreditsAllowed")] Program Program)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Programs.Add(Program);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                if (e.InnerException.InnerException.Message.Contains("UNIQUE KEY constraint"))
                {
                    ModelState.AddModelError("maxCreditsAllowed", "The program has already been created");
                }
                else
                {
                    ModelState.AddModelError("maxCreditsAllowed", e.InnerException.InnerException.Message);
                }
            }

            return View(Program);
        }

        // GET: Programs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Program Program = db.Programs.Find(id);
            if (Program == null)
            {
                return HttpNotFound();
            }
            return View(Program);
        }

        // POST: Programs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "program_id,programPrefix,ProgramName,maxCreditsAllowed")] Program Program)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Program).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Program);
        }

        // GET: Programs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Program Program = db.Programs.Find(id);
            if (Program == null)
            {
                return HttpNotFound();
            }
            return View(Program);
        }

        // POST: Programs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Program Program = db.Programs.Find(id);
            db.Programs.Remove(Program);
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
        public JsonResult IsProgramTaken([Bind(Prefix = "programPrefix")] string programPrefix, [Bind(Prefix = "programName")] string programName)
        {
            if (!string.IsNullOrEmpty(programPrefix) && !string.IsNullOrEmpty(programName))
            {
                foreach (Program p in db.Programs.ToList())
                {
                    if ((p.programPrefix == programPrefix) && (p.programName == programName))
                        return Json("The program and program name aleady exists", JsonRequestBehavior.AllowGet);
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
