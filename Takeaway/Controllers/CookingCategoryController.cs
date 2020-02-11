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
    public class CookingCategoryController : Controller
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

            var Data = db.CookingCategories.Where(a => a.IsDelete == false ).ToList().OrderBy(c => c.CookingCatNameAR).Select(i => new
            {
                i.CookingCatNameAR,
                i.CookingCatNameEN,

                IsActive = i.IsActive == true ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                Edit = "<a  id='btn_Edit' class='btn default btn-xs green' href='/CookingCategory/Edit/" + i.ID + "'>تعديل</a>",
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
            CookingCategory cookingCat = db.CookingCategories.Find(id);

            cookingCat.IsActive = cookingCat.IsActive == true ? false : true;
            try
            {
                db.Entry(cookingCat).State = EntityState.Modified;
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
           
            return View();
        }

        // POST: CookingCat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CookingCategory cookingCat)
        {
            if (ModelState.IsValid)
            {
                cookingCat.IsActive = true;
                db.CookingCategories.Add(cookingCat);
                db.SaveChanges();
                return RedirectToAction("Index", new { Msg = 3 });
            }

            return View(cookingCat);
        }

        // GET: package/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CookingCategory cookingCat = db.CookingCategories.Find(id);
            if (cookingCat == null)
            {
                return HttpNotFound();
            }

            return View(cookingCat);
        }


        // POST: package/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CookingCategory cookingCat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cookingCat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { Msg = 4 });
            }
            return View(cookingCat);
        }

        // GET: package/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CookingCategory cookingCat = db.CookingCategories.Find(id);
            if (cookingCat == null)
            {
                return HttpNotFound();
            }
            return View(cookingCat);
        }

        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                CookingCategory cookingCat = db.CookingCategories.Find(id);
                cookingCat.IsDelete = true;
                db.Entry(cookingCat).State = EntityState.Modified;
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
