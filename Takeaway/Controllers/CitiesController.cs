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
    public class CitiesController : Controller
    {
        private TakeawayEntities db = new TakeawayEntities();

        // GET: Cities
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

            var CitiesData = db.Cities.Where(a => a.IsDelete == false).ToList().Select(i => new
            {
                CityAr = i.CityAr,
                CityEn = i.CityEn,
                CountryAr = i.Country.CountryAr,
                CountryEn = i.Country.CountryEng,
                IsActive = i.IsActive == true ? "<span id='btn_Active'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Active'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                Edit = "<a  id='btn_Edit' class='btn default btn-xs green' href='/Cities/Edit/" + i.ID + "'>تعديل</a>",
                Deleted = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' onclick='Delete(" + i.ID + ")'  > حذف</a>"




            }).OrderBy(a => a.CountryEn).ToList();
            return Json(new { data = CitiesData }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeStatus(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return null;
            }
            City city = db.Cities.Find(id);

            city.IsActive = city.IsActive == true ? false : true;
            try
            {
                db.Entry(city).State = EntityState.Modified;
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

        // GET: Cities/Create
        public ActionResult Create()
        {
            ViewBag.CountryID = new SelectList(db.Countries.Where(c => c.IsActive == true 
            && c.IsDetete == false), "ID", "CountryAr");
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CityEn,CityAr,CountryID,RegionID,IsActive,IsDetete")] City city)
        {
            if (ModelState.IsValid)
            {
                city.IsActive = true;
                db.Cities.Add(city);
                db.SaveChanges();
                ViewBag.Msg = 3;
                return RedirectToAction("Index", new { Msg = 3 });
            }

            ViewBag.CountryID = new SelectList(db.Countries.Where(c => c.IsActive == true
            && c.IsDetete == false), "ID", "CountryAr", city.CountryID);
            return View(city);
        }

        // GET: Cities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryID = new SelectList(db.Countries.Where(c => c.IsActive == true
           && c.IsDetete == false), "ID", "CountryAr",city.CountryID);
            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CityEn,CityAr,CountryID,RegionID,IsDetete,IsActive")] City city)
        {
            if (ModelState.IsValid)
            {
                db.Entry(city).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Msg = 3;
                return RedirectToAction("Index", new { Msg = 3 });
            }
            ViewBag.CountryID = new SelectList(db.Countries.Where(c => c.IsActive == true
           && c.IsDetete == false), "ID", "CountryAr", city.CountryID);
            return View(city);
        }

        // GET: Cities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }

        // POST: Cities/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
      

        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                City city = db.Cities.Find(id);
                city.IsDelete = true;
                db.Entry(city).State = EntityState.Modified;
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
