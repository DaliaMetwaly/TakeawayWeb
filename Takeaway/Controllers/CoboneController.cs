using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Takeaway.Models;

namespace Takeaway.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CoboneController : Controller
    {
        private TakeawayEntities db = new TakeawayEntities();

        // GET: Regions
        public ActionResult Index(int? Msg)
        {
            if (Msg != null)
            {
                ViewBag.Msg = Msg;
            }
            return View();
        }
        public ActionResult LoadData()
        {

            var Data = db.Cobones.Where(a => a.IsDelete == false ).ToList().OrderBy(c => c.CoboneNameAr).Select(i => new
            {
                i.CoboneNameAr,
                i.CoboneNameEn,
                i.CoboneType.CoboneTypeNameAr,
                i.CoboneTypeValue,
                i.Duration,

                IsActive = i.IsActive == true ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                Edit = "<a  id='btn_Edit' class='btn default btn-xs green' href='/Cobone/Edit/" + i.ID + "'>تعديل</a>",
                Deleted = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' onclick='Delete(" + i.ID + ")'  > حذف</a>"




            }).ToList();
            return Json(new { data = Data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeStatus(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return null;
            }
            Cobone cobone = db.Cobones.Find(id);

            cobone.IsActive = cobone.IsActive == true ? false : true;
            try
            {
                db.Entry(cobone).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            } //return RedirectToAction("Index");
            return Json("1", JsonRequestBehavior.AllowGet);
        }

     
        public ActionResult Create()
        {
           ViewBag.CoboneTypeID =  new SelectList(db.CoboneTypes.Where(c => c.IsActive == true
             && c.IsDelete == false), "ID", "CoboneTypeNameAr");
            return View();
        }

        // POST: CookingCat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cobone cobone)
        {
            if (ModelState.IsValid)
            {
                cobone.IsActive = true;
                db.Cobones.Add(cobone);
                db.SaveChanges();
                return RedirectToAction("Index", new { Msg = 3 });
            }
            ViewBag.CoboneTypeID = new SelectList(db.CoboneTypes.Where(c => c.IsActive == true
            && c.IsDelete == false), "ID", "CoboneTypeNameAr");
            return View(cobone);
        }

        // GET: package/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cobone cobone = db.Cobones.Find(id);
            if (cobone == null)
            {
                return HttpNotFound();
            }
            ViewBag.CoboneTypeID = new SelectList(db.CoboneTypes.Where(c => c.IsActive == true
            && c.IsDelete == false), "ID", "CoboneTypeNameAr",cobone.CoboneTypeID);
            return View(cobone);
        }


        // POST: package/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cobone cobone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cobone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { Msg = 4 });
            }
            ViewBag.CoboneTypeID = new SelectList(db.CoboneTypes.Where(c => c.IsActive == true
           && c.IsDelete == false), "ID", "CoboneTypeNameAr", cobone.CoboneTypeID);
            return View(cobone);
        }

        // GET: package/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cobone cobone = db.Cobones.Find(id);
            if (cobone == null)
            {
                return HttpNotFound();
            }
            return View(cobone);
        }

        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                Cobone cobone = db.Cobones.Find(id);
                cobone.IsDelete = true;
                db.Entry(cobone).State = EntityState.Modified;
                db.SaveChanges();
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return Json("2", JsonRequestBehavior.AllowGet);
                throw;
            }
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
