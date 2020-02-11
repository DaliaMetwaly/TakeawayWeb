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
    public class RegionsController : Controller
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

            var RegionsData = db.Regions.Where(a => a.IsDetete == false).ToList().OrderBy(c => c.City.CityAr).Select(i => new
            {
                RegionAr = i.RegionAr,
                RegionEn = i.RegionEn,
                CityAr = i.City.CityAr,
                CityEn = i.City.CityEn,
                NotesAr=  i.NotesAr,
                NotesEn=  i.NotesEn,
                PostalCode=i.PostalCode,
                IsActive = i.IsActive == true ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                Edit = "<a  id='btn_Edit' class='btn default btn-xs green' href='/Regions/Edit/" + i.ID + "'>تعديل</a>",
                Deleted = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' onclick='Delete(" + i.ID + ")'  > حذف</a>"




            }).ToList();
            return Json(new { data = RegionsData }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeStatus(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return null;
            }
            Region region = db.Regions.Find(id);

            region.IsActive = region.IsActive == true ? false : true;
            try
            {
                db.Entry(region).State = EntityState.Modified;
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

        public JsonResult Get_city(int id)
        {
            ViewBag.CityID = new SelectList(db.Cities.Where(a=>a.CountryID==id), "ID", "CityEn");

            List<SelectListItem> _city = new List<SelectListItem>();
            _city.Add(new SelectListItem { Text = "اختر المدينة ", Value = null });
            var _branchquery = db.Cities.ToList().Where(a => a.CountryID == id);

            foreach (var q in _branchquery)
            {
                _city.Add(new SelectListItem { Text = q.CityAr, Value = q.ID.ToString() });
            }

            //ViewBag.CityID = new SelectList(db.Cities.Where(a=>a.ID==id), "ID", "CityEn");

             return Json(new SelectList(_city, "Value", "Text"));

        }

        public ActionResult Get_editcity(int? id)
        {
            var Data = (Object)null;
            try
            {
                Data = db.Cities.Where(x => x.IsActive == true && x.IsDelete == false && x.CountryID == id).Select(i => new { ID = i.ID, Name = i.CityAr }).ToList();

            }
            catch (Exception ex)
            {

            }
            return Json(new { data = Data }, JsonRequestBehavior.AllowGet);
        }
        // GET: Regions/Create
        public ActionResult Create()
        {
           
            ViewBag.CountryID = new SelectList(db.Countries.Where(c => c.IsActive == true
                                                                       && c.IsDetete == false), "ID", "CountryAr");
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityEn");

            return View();
        }

        // POST: Regions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RegionEn,RegionAr,CityID,CountryID,NotesAr,NotesEn,PostalCode,IsActive,IsDetete")] Region region)
        {
            if (ModelState.IsValid)
            {
                region.IsActive = true;
                db.Regions.Add(region);
                db.SaveChanges();
                ViewBag.Msg = 3;
                return RedirectToAction("Index", new { Msg = 3 });
            }
            ViewBag.CountryID = new SelectList(db.Countries.Where(c => c.IsActive == true
                                                                      && c.IsDetete == false), "ID", "CountryAr", region.CountryID);
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityEn", region.CityID);
            return View(region);
        }
       
        // GET: Regions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Region region = db.Regions.Find(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryID = new SelectList(db.Countries.Where(c => c.IsActive == true
                                                                    && c.IsDetete == false), "ID", "CountryAr", region.CountryID);
            ViewBag.CityID = new SelectList(db.Cities.Where(a=>a.CountryID==region.CountryID), "ID", "CityEn");
            return View(region);
        }


        // POST: Regions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RegionEn,RegionAr,CityID,CountryID,NotesAr,NotesEn,PostalCode,IsActive,IsDetete")] Region region)
        {
            if (ModelState.IsValid)
            {
                db.Entry(region).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Msg = 3;
                return RedirectToAction("Index", new { Msg = 3 });
            }
            ViewBag.CountryID = new SelectList(db.Countries.Where(c => c.IsActive == true
                                                                    && c.IsDetete == false), "ID", "CountryAr", region.CountryID);
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityEn");
            return View(region);
        }

        // GET: Regions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Region region = db.Regions.Find(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return View(region);
        }

        // POST: Regions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Region region = db.Regions.Find(id);
        //    db.Regions.Remove(region);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                Region region = db.Regions.Find(id);
                region.IsDetete = true;
                db.Entry(region).State = EntityState.Modified;
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
