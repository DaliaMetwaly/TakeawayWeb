using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Takeaway.Models;
using Microsoft.AspNet.Identity;
using static Takeaway.Controllers.Common;

namespace Takeaway.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin,Restaurant")]
    public class MinDistancePriceController : Controller
    {
      
        TakeawayEntities db = new TakeawayEntities();       
        ApplicationDbContext dbadmin = new ApplicationDbContext();


        // GET: Comment
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
            var Data= (Object)null;
            try
            {
                string usrID = User.Identity.GetUserId();
                string roleName = dbadmin.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;

                var Entity = (roleName == "Restaurant") ? db.MinDistancePrices.Where(x => x.IsDelete == false && x.Restaurant.UserID == usrID).OrderByDescending(x => x.CreatedDate).ToList() : db.MinDistancePrices.Where(x => x.IsDelete == false ).OrderByDescending(x => x.CreatedDate).ToList();
                var userData = dbadmin.Users.ToList();
                Data = (from c in userData
                        join i in Entity on c.Id equals i.CreatedUser

                        select new
                        {
                            RestaurantID = i.RestaurantID,
                            RestaurantName = i.Restaurant.RestaurantName,
                            UserName = c.UserName,
                            Address = i.Restaurant.Address,
                            IsActive = db.MinDistancePrices.Where(x => x.IsActive == true).Any(v => v.RestaurantDataID == i.RestaurantDataID) ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.RestaurantDataID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.RestaurantDataID + ")' class='glyphicon glyphicon-remove'></ span >",
                            //deliveryFeeValue = i.deliveryFeeValue,
                            //minPrice = i.minPrice,
                            //Distance = Common.GetEnumDistanceName(i.DistanceID),
                            //IsActive = i.IsActive == true ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                            Delete = "<a  id='btn_Edit' class='btn default btn-xs green' onclick='Delete(" + i.RestaurantID + ")'>حذف</a>",
                           // Deleted = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' onclick='Delete(" + i.ID + ")'  > حذف</a>"

                        }).OrderBy(x => x.RestaurantName).Distinct().ToList();

               
            }
            catch(Exception ex)
            {

            }
            return Json(new { data = Data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeStatus(int? id)
        {
            if (id == null)
            {
                return null;
            }
            db.MinDistancePrices
            .Where(x => x.RestaurantDataID == id)
            .ToList()
            .ForEach(a => a.IsActive = a.IsActive == true ? false : true);

            try
            {
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
            }

            //return RedirectToAction("Index");
            return Json("1", JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetDetails(int id)
        {
            var entity = db.MinDistancePrices.Where(i => i.IsDelete == false && i.RestaurantID == id).ToList();
            var Data = (from i in entity
                        select new
                        {
                            ID = i.ID,
                            deliveryFeeValue = i.deliveryFeeValue,
                            minPrice = i.minPrice,
                            City = i.Region.City.CityAr,
                            Region = i.Region.RegionAr
                           
                        }).OrderBy(x => x.ID).ToList();

            return Json(Data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(int? restaurantID)
        {
            string usrID = User.Identity.GetUserId();
            string roleName = dbadmin.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;

            ViewBag.RestaurantID = (roleName == "Restaurant") ? new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1 && x.UserID == usrID), "ID", "RestaurantName") : new SelectList(db.Restaurants.Where(x =>x.RestaurantStatus == 1 ), "ID", "RestaurantName");

            ViewBag.RestDataID = new SelectList(db.RestaurantDatas.Where(x => x.IsActive == true && x.IsDetete == false), "ID", "Address");

            ViewBag.RegionID =  new SelectList(db.Regions.Where(x => x.IsActive==true && x.IsDetete==false), "ID", "RegionAr") ;
            
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            if (ModelState.IsValid)
            {

                if (TempData.Keys.Where(x => x.ToString() == "Distance").Count() > 0)
                {
                    foreach (DistanceList item in TempData["Distance"] as List<DistanceList>)
                    {
                        MinDistancePrice minDistancePrice = new MinDistancePrice();
                        minDistancePrice.RestaurantID = item.Restaurant_id;
                        minDistancePrice.RestaurantDataID = item.RestaurantData_id;
                        minDistancePrice.RegionID = item.Region_id;
                        minDistancePrice.deliveryFeeValue = item.deliveryFeeValue;
                        minDistancePrice.minPrice = item.minPrice;
                        minDistancePrice.CreatedDate = DateTime.Now;
                        minDistancePrice.CreatedUser = User.Identity.GetUserId();
                        db.MinDistancePrices.Add(minDistancePrice);
                    }
                    try
                    {
                        db.SaveChanges();
                        ViewBag.Msg = 3;
                        return RedirectToAction("Index", new { Msg = 3 });
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the 			following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        return View();
                    }
                    //return View();
                }
            }
            

            string usrID = User.Identity.GetUserId();
            string roleName = dbadmin.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;

            ViewBag.RestaurantID = (roleName == "Restaurant") ? new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1 && x.UserID == usrID), "ID", "RestaurantName") : new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1), "ID", "RestaurantName");
            ViewBag.RestaurantDataID = new SelectList(db.RestaurantDatas.Where(x => x.IsActive == true && x.IsDetete == false), "ID", "Address");
            ViewBag.RegionID = new SelectList(db.Regions.Where(x => x.IsActive == true && x.IsDetete == false), "ID", "RegionAr");
            return View();
        }


        public ActionResult Edit(int? id)
        {
            //id ==> RestaurantID
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MinDistancePrice minDistancePrice = db.MinDistancePrices.Where(x => x.IsDelete == false && x.ID == id).FirstOrDefault();

            if (minDistancePrice == null)
            {
                return HttpNotFound();
            }
            MinDistancePriceVM minDistancePriceVm = new MinDistancePriceVM();
            minDistancePriceVm.ID = minDistancePrice.ID;
            minDistancePriceVm.RestaurantID = minDistancePrice.RestaurantID;
            minDistancePriceVm.RestaurantDataID = minDistancePrice.RestaurantDataID;
            minDistancePriceVm.RegionID = minDistancePrice.RegionID;
            minDistancePriceVm.minPrice = minDistancePrice.minPrice;
            minDistancePriceVm.deliveryFeeValue = minDistancePrice.deliveryFeeValue;


            string usrID = User.Identity.GetUserId();
            string roleName = dbadmin.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;

            ViewBag.RestaurantID = (roleName == "Restaurant") ? new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1 && x.UserID == usrID), "ID", "RestaurantName", minDistancePrice.RestaurantID) : new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1), "ID", "RestaurantName", minDistancePrice.RestaurantID);

            List<int> restaurantDataID = db.RestaurantDatas.Where(x => x.RestaurantID == minDistancePrice.RestaurantID).Select(i => i.ID).ToList();
            ViewBag.RestaurantDataID = new SelectList(db.RestaurantDatas.Where(x => x.IsActive == true && x.IsDetete == false && restaurantDataID.Contains(x.ID)), "ID", "Address",minDistancePrice.RestaurantDataID);

            List<int> regionID = db.Regions.Where(x => x.CityID ==(db.Cities.Where(a=>a.ID==minDistancePrice.RestaurantData.Region.CityID)).FirstOrDefault().ID).Select(i => i.ID).ToList();
            ViewBag.RegionID = new SelectList(db.Regions.Where(x => x.IsActive == true && x.IsDetete == false && regionID.Contains(x.ID)), "ID", "RegionAr", minDistancePrice.RegionID);

            return View(minDistancePriceVm);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MinDistancePriceVM minDistancePriceVM)
        {
            if (ModelState.IsValid)
            {

                MinDistancePrice minDistancePrice = db.MinDistancePrices.Find(minDistancePriceVM.ID);
                minDistancePrice.RestaurantID = minDistancePriceVM.RestaurantID;
                minDistancePrice.RegionID = minDistancePriceVM.RegionID;
                minDistancePrice.deliveryFeeValue = minDistancePriceVM.deliveryFeeValue;
                minDistancePrice.minPrice = minDistancePriceVM.minPrice;
                minDistancePrice.CreatedDate = DateTime.Now;
                minDistancePrice.CreatedUser = User.Identity.GetUserId();

                db.Entry(minDistancePrice).State = EntityState.Modified;

               
                db.SaveChanges();
                return RedirectToAction("Index", new { Msg = 4 });
            }
            string usrID = User.Identity.GetUserId();
            string roleName = dbadmin.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;

            ViewBag.RestaurantID = (roleName == "Restaurant") ? new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1 && x.UserID == usrID), "ID", "RestaurantName", minDistancePriceVM.RestaurantID) : new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1), "ID", "RestaurantName", minDistancePriceVM.RestaurantID);

            List<int> restaurantDataID = db.RestaurantDatas.Where(x => x.RestaurantID == minDistancePriceVM.RestaurantID).Select(i => i.ID).ToList();
            ViewBag.RestaurantDataID = new SelectList(db.RestaurantDatas.Where(x => x.IsActive == true && x.IsDetete == false && restaurantDataID.Contains(x.ID)), "ID", "Address", minDistancePriceVM.RestaurantDataID);

            List<int> regionID = db.Regions.Where(x => x.CityID == (db.Cities.Where(a => a.ID ==db.RestaurantDatas.Where(c=>c.RestaurantID== minDistancePriceVM.RestaurantID).FirstOrDefault().Region.CityID)).FirstOrDefault().ID).Select(i => i.ID).ToList();
            ViewBag.RegionID = new SelectList(db.Regions.Where(x => x.IsActive == true && x.IsDetete == false && regionID.Contains(x.ID)), "ID", "RegionAr", minDistancePriceVM.RegionID);

            return View(minDistancePriceVM);
        }

        public ActionResult GetRegionList(int? id)
        {
            var Data = (Object)null;
            try
            {
                // Edit by Mohamed Elsayed at 5:00 PM 22/08/2017
                //var currentcity = db.Cities.Where(c => c.Regions.Any(r => r.ID == id)).FirstOrDefault();
                var selectedID = db.RestaurantDatas.Where(x => x.ID == id).FirstOrDefault().Region_id;

                var currentcity = db.Cities.Where(c => c.Regions.Any(r => r.ID == selectedID)).FirstOrDefault();

                //var currentcity = db.Cities.Where(c => c.Regions.Where(z=>z.ID == selectedID ).FirstOrDefault();
                if (currentcity != null)
                {
                    Data = db.Regions.Where(x => x.IsActive == true && x.IsDetete == false && x.CityID == currentcity.ID).Select(i => new { ID = i.ID, Name = i.RegionAr }).ToList();
                }
               

            }
            catch (Exception ex)
            {

            }
            return Json(new { data = Data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string addtempDistance(List<DistanceList> distanceList)
        {
           
            TempData.Remove("Distance");

            if (distanceList == null)
            {

                return "false";
            }
            TempData["Distance"] = distanceList;

            return "true";

        }

        public JsonResult DeleteConfirmed(int Id)
        {
            try
            {
                List<MinDistancePrice> minDistanceLst = db.MinDistancePrices.Where(x => x.RestaurantID == Id).ToList() ;
                foreach (MinDistancePrice item in minDistanceLst)
                {
                    item.IsDelete = true;
                    db.Entry(item).State = EntityState.Modified;
                }
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


    }
}