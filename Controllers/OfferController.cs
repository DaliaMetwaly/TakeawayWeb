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
    public class OfferController : Controller
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
                var Entity = db.Offers.Where(x => x.IsDelete == false ).OrderByDescending(x => x.CreationDate).ToList();
                var userData = dbadmin.Users.ToList();
                Data = (from c in userData
                        join i in Entity on c.Id equals i.UserID
                        where i.OfferType == (int)OfferType.ItemFood
                        select new
                        {
                            ID = i.ID,
                            Name = db.ItemFoods.Where(x => x.ID == i.SubjectID).FirstOrDefault().FoodName,
                            UserName = c.UserName,
                            FeeType = Common.GetEnumOfferFeeTypeName(i.FeeType),
                            FeeValue = i.FeeValue,
                            OfferType = i.OfferType,
                            StartDate = "<span  id = 'StartDate' class=''>" + Convert.ToDateTime(i.StartDate).ToString("dd/MM/yyyy hh:mm") + "</span>",
                            EndDate = "<span  id = 'EndDate' class=''>" + Convert.ToDateTime(i.EndDate).ToString("dd/MM/yyyy hh:mm") + "</span>",
                            CreationDate = "<span  id = 'CreationDate' class=''>" + Convert.ToDateTime(i.CreationDate).ToString("dd/MM/yyyy hh:mm") + "</span>",
                            IsActive = i.IsActive == true ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                            Edit = "<a  id='btn_Edit' class='btn default btn-xs green' href='/Offer/Edit/" + i.ID + "'>تعديل</a>",
                            Deleted = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' onclick='Delete(" + i.ID + ")'  > حذف</a>"

                        }).Union

                                                     (from c in userData
                                                      join i in Entity on c.Id equals i.UserID
                                                      where i.OfferType == (int)OfferType.Restaurant
                                                      select new
                                                      {
                                                          ID = i.ID,
                                                          Name = db.Restaurants.Where(x => x.ID == i.SubjectID).FirstOrDefault().RestaurantName,
                                                          UserName = c.UserName,
                                                          FeeType = Common.GetEnumOfferFeeTypeName(i.FeeType),
                                                          FeeValue = i.FeeValue,
                                                          OfferType = i.OfferType,
                                                          StartDate = "<span  id = 'StartDate' class=''>" + Convert.ToDateTime(i.StartDate).ToString("dd/MM/yyyy hh:mm") + "</span>",
                                                          EndDate = "<span  id = 'EndDate' class=''>" + Convert.ToDateTime(i.EndDate).ToString("dd/MM/yyyy hh:mm") + "</span>",
                                                          CreationDate = "<span  id = 'CreationDate' class=''>" + Convert.ToDateTime(i.CreationDate).ToString("dd/MM/yyyy hh:mm") + "</span>",
                                                          IsActive = i.IsActive == true ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                                                          Edit = "<a  id='btn_Edit' class='btn default btn-xs green' href='/Offer/Edit/" + i.ID + "'>تعديل</a>",
                                                          Deleted = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' onclick='Delete(" + i.ID + ")'  > حذف</a>"

                                                      })
                                                      .OrderBy(x=> x.OfferType)
                                                     .ToList();

               
            }
            catch(Exception ex)
            {

            }
            return Json(new { data = Data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubjectList(int? id)
        {
            var Data = (Object)null;
            try
            {
                Data = db.ItemFoods.Where(x => x.IsActive == true && x.IsDetete == false).Select(i => new { ID = i.ID, Name = i.FoodName }).ToList();
                if (id == 1)
                {
                    Data = db.Restaurants.Where(x => x.RestaurantStatus == 1).Select(i => new { ID = i.ID, Name = i.RestaurantName }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { data = Data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetfoodList(int? id)
        {
            var Data = (Object)null;
            try
            {
                Data = db.ItemFoods.Where(x => x.IsActive == true && x.IsDetete == false && x.Category.RestaurantID == id).Select(i => new { ID = i.ID, Name = i.FoodName }).ToList();
              
            }
            catch (Exception ex)
            {

            }
            return Json(new { data = Data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeStatus(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return null;
            }
            Offer offer = db.Offers.Find(id);

            offer.IsActive = offer.IsActive == true ? false : true;
            try
            {
                db.Entry(offer).State = EntityState.Modified;
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
            
            var Feeenums = new List<SelectListItem>();
            foreach (int value in Enum.GetValues(typeof(OfferFeeType)))
            {
                var item = new SelectListItem();
                item.Value = value.ToString();
                item.Text = Common.GetEnumOfferFeeTypeName(value);
                Feeenums.Add(item);
            }
           
            ViewBag.FeeType = new SelectList(Feeenums, "Value", "Text"); 

            var enums = new List<SelectListItem>();
            foreach (int value in Enum.GetValues(typeof(OfferType)))
            {
                var item = new SelectListItem();
                item.Value = value.ToString();
                item.Text = Common.GetEnumOfferTypeName(value);// Enum.GetName(typeof(OfferType), value);

                //if (selected != null)
                //    item.Selected = (int)selected == value;

                enums.Add(item);
            }
            ViewBag.OfferType = new SelectList(enums, "Value", "Text"); 

            ViewBag.SubjectID = new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1).Select(x => new
            {
                ID =x.ID,
                Name = x.RestaurantName
            }), "ID", "Name");

            ViewBag.Restaurant = new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1).Select(x => new
            {
                ID = x.ID,
                Name = x.RestaurantName
            }), "ID", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Offer offer)
        {
            if (ModelState.IsValid)
            {
                offer.CreationDate = DateTime.Now;
                offer.UserID = User.Identity.GetUserId();
                offer.IsActive = true;
                db.Offers.Add(offer);
                db.SaveChanges();
                ViewBag.Msg = 3;
                return RedirectToAction("Index", new { Msg = 3 });
            }

            var Feeenums = new List<SelectListItem>();
            foreach (int value in Enum.GetValues(typeof(FeeType)))
            {
                var item = new SelectListItem();
                item.Value = value.ToString();
                item.Text = Common.GetEnumOfferFeeTypeName(value);
                Feeenums.Add(item);
            }

            ViewBag.FeeType = new SelectList(Feeenums, "Value", "Text");


            var enums = new List<SelectListItem>();
            foreach (int value in Enum.GetValues(typeof(OfferType)))
            {
                var item = new SelectListItem();
                item.Value = value.ToString();
                item.Text = Common.GetEnumOfferTypeName(value);// Enum.GetName(typeof(OfferType), value);

                //if (selected != null)
                //    item.Selected = (int)selected == value;

                enums.Add(item);
            }
            ViewBag.OfferType = new SelectList(enums, "Value", "Text", offer.OfferType); 
            ViewBag.SubjectID = new SelectList(db.ItemFoods.Where(x => x.IsActive == true && x.IsDetete == false).Select(i => new { ID = i.ID, Name = i.FoodName }), "ID", "Name");
           
            if (offer.OfferType == 1)
            {
                ViewBag.SubjectID = new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1).Select(x => new
                {
                    ID = x.ID,
                    Name = x.RestaurantName
                }), "ID", "Name");
            }
            ViewBag.Restaurant = new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1).Select(x => new
            {
                ID = x.ID,
                Name = x.RestaurantName
            }), "ID", "Name");
            return View(offer);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
           
            var Feeenums = new List<SelectListItem>();
            foreach (int value in Enum.GetValues(typeof(OfferFeeType)))
            {
                var item = new SelectListItem();
                item.Value = value.ToString();
                item.Text = Common.GetEnumOfferFeeTypeName(value);
                if (offer.FeeType != 0)
                    item.Selected = (int)offer.FeeType == value;
                Feeenums.Add(item);
            }

            ViewBag.FeeType = new SelectList(Feeenums, "Value", "Text");


            var enums = new List<SelectListItem>();
            foreach (int value in Enum.GetValues(typeof(OfferType)))
            {
                var item = new SelectListItem();
                item.Value = value.ToString();
                item.Text = Common.GetEnumOfferTypeName(value);// Enum.GetName(typeof(OfferType), value);

                if (offer.OfferType != 0)
                    item.Selected = (int)offer.OfferType == value;

                enums.Add(item);
            }
            ViewBag.OfferType = new SelectList(enums, "Value", "Text", offer.OfferType);
            if (offer.OfferType == 2)
            {
                ViewBag.SubjectID = new SelectList(db.ItemFoods.Where(x => x.IsActive == true && x.IsDetete == false).Select(i => new { ID = i.ID, Name = i.FoodName }), "ID", "Name", offer.SubjectID);
                ItemFood itemfood = db.ItemFoods.Find(offer.SubjectID);
                ViewBag.Restaurant = new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1).Select(x => new
                {
                    ID = x.ID,
                    Name = x.RestaurantName
                }), "ID", "Name", itemfood.Category.RestaurantID);
            }
            if (offer.OfferType == 1)
            {
                ViewBag.SubjectID = new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1).Select(x => new
                {
                    ID = x.ID,
                    Name = x.RestaurantName
                }), "ID", "Name", offer.SubjectID);
                ViewBag.Restaurant = new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1).Select(x => new
                {
                    ID = x.ID,
                    Name = x.RestaurantName
                }), "ID", "Name", offer.SubjectID);
            }
            return View(offer);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Offer offer)
        {
            if (ModelState.IsValid)
            {
                Offer oldoffer = db.Offers.Find(offer.ID);
                oldoffer.EndDate = offer.EndDate;
                oldoffer.StartDate = offer.StartDate;
                oldoffer.FeeType = offer.FeeType;
                oldoffer.OfferType = offer.OfferType;
                oldoffer.SubjectID = offer.SubjectID;
                oldoffer.FeeValue = offer.FeeValue;
                oldoffer.IsActive = offer.IsActive;
                //offer.CreationDate = oldoffer.CreationDate;
                db.Entry(oldoffer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { Msg = 4 });
            }
            var Feeenums = new List<SelectListItem>();
            foreach (int value in Enum.GetValues(typeof(FeeType)))
            {
                var item = new SelectListItem();
                item.Value = value.ToString();
                item.Text = Common.GetEnumOfferFeeTypeName(value);
                if (offer.FeeType != 0)
                    item.Selected = (int)offer.FeeType == value;
                Feeenums.Add(item);
            }

            ViewBag.FeeType = new SelectList(Feeenums, "Value", "Text");

            var enums = new List<SelectListItem>();
            foreach (int value in Enum.GetValues(typeof(OfferType)))
            {
                var item = new SelectListItem();
                item.Value = value.ToString();
                item.Text = Common.GetEnumOfferTypeName(value);// Enum.GetName(typeof(OfferType), value);

                if (offer.OfferType != 0)
                    item.Selected = (int)offer.OfferType == value;

                enums.Add(item);
            }

            ViewBag.OfferType = new SelectList(enums, "Value", "Text", offer.OfferType);
            if (offer.OfferType == 2)
            {
                ViewBag.SubjectID = new SelectList(db.ItemFoods.Where(x => x.IsActive == true && x.IsDetete == false).Select(i => new { ID = i.ID, Name = i.FoodName }), "ID", "Name", offer.SubjectID);

            ItemFood itemfood = db.ItemFoods.Find(offer.SubjectID);
            ViewBag.Restaurant = new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1).Select(x => new
            {
                ID = x.ID,
                Name = x.RestaurantName
            }), "ID", "Name", itemfood.Category.RestaurantID);
        }
            if (offer.OfferType == 1)
            {
                ViewBag.SubjectID = new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1).Select(x => new
                {
                    ID = x.ID,
                    Name = x.RestaurantName
                }), "ID", "Name", offer.SubjectID);
                ViewBag.Restaurant = new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1).Select(x => new
                {
                    ID = x.ID,
                    Name = x.RestaurantName
                }), "ID", "Name", offer.SubjectID);
            }
            return View(offer);
        }

        public ActionResult Delete(int ID)
        {
            try
            {
                var model = db.Offers.Find(ID);
                model.IsDelete = true;

                db.SaveChanges();
             
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

       
    }
}