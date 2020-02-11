using Microsoft.AspNet.Identity;
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

namespace Takeaway.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin,Restaurant")]
    public class RestaurantController : Controller
    {
        TakeawayEntities db = new TakeawayEntities();
        private ApplicationDbContext dbMembership = new ApplicationDbContext();
        string mediapath = "~\\images\\Restaurant\\";
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
            ApplicationDbContext dbMembership = new ApplicationDbContext();
            string usrID = User.Identity.GetUserId();
            string roleName = dbMembership.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;

            var Data = (roleName == "Restaurant")
                ? db.Restaurants.Where(a => a.UserID == usrID).ToList().Select(i => new
                {
                    ID = i.ID,
                    RestaurantName = i.RestaurantName,
                    RestaurantNameEn = i.RestaurantNameEn,
                    RestaurantStatus = i.RestaurantStatu.RestaurantStatusAr,
                    DeliveryName = i.DeliveryWay.DeliveryNameAr,
                    //feetype = i.FeeType.FeeNameAr,
                    owner = dbMembership.Users.Where(r => r.Id == i.UserID).FirstOrDefault().UserName  ,
                    //Country = i.Country.CountryAr,
                    City = i.City.CityAr,
                    IsActive = i.RestaurantStatus == 1 ? "<span id='btn_Delete'  class='btn default btn-xs green' onclick='Change(" + i.ID + ")' >نشط</ span >" : "<span id='btn_Delete'  class='btn default btn-xs red' onclick='Change(" + i.ID + ")'  >منتهى الصلاحية</ span >",
                    Edit = "<a  id='btn_Edit' class='btn default btn-xs green' href='/Restaurant/Edit/" + i.ID + "'>تعديل</a>",
                    Orders = "<a  id='btn_Edit' class='btn default btn-xs green' href='/Orders/OrdersbyRestaurant/" + i.ID + "'>الطلبات</a>",
                    Addresses = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' data-toggle='modal' data-target='#myModal' onclick='GetRestaurantAddresses(" + i.ID + ")'  > الفروع</a>",
                }).OrderBy(a => a.RestaurantName).ToList()
                : db.Restaurants.ToList().Select(i => new
                {
                    ID = i.ID,
                    RestaurantName = i.RestaurantName,
                    RestaurantNameEn = i.RestaurantNameEn,
                    RestaurantStatus = i.RestaurantStatu.RestaurantStatusAr,
                    DeliveryName = i.DeliveryWay.DeliveryNameAr,
                    //feetype = i.FeeType.FeeNameAr,
                    owner = dbMembership.Users.Where(r => r.Id == i.UserID).FirstOrDefault().UserName  /* Get user name for UserID*/ ,
                    //Country = i.Country.CountryAr,
                    City = i.City.CityAr,
                    // IsActive = i.RestaurantStatus == 1 ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                    IsActive = i.RestaurantStatus == 1 ? "<span id='btn_Delete'  class='btn default btn-xs green' onclick='Change(" + i.ID + ")' >نشط</ span >" : "<span id='btn_Delete'  class='btn default btn-xs red' onclick='Change(" + i.ID + ")'  >منتهى الصلاحية</ span >",
                    Edit = "<a  id='btn_Edit' class='btn default btn-xs green' href='/Restaurant/Edit/" + i.ID + "'>تعديل</a>",
                    Orders = "<a  id='btn_Edit' class='btn default btn-xs green' href='/Orders/OrdersbyRestaurant/" + i.ID + "'>الطلبات</a>",
                    Addresses = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' data-toggle='modal' data-target='#myModal' onclick='GetRestaurantAddresses(" + i.ID + ")'  > الفروع</a>",
                }).OrderBy(a => a.RestaurantName).ToList();


            return Json(new { data = Data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadRestauantData(int restaurantID)
        {
            int Srno = 1;
            var RestauarntDataJson = db.RestaurantDatas.Where(a => a.IsDetete == false && a.IsActive == true && a.RestaurantID == restaurantID).ToList().Select(a => new AddressList
            {
                count = Srno++,
                Address = a.Address,
                Region_id = a.Region_id,
                Region_Name = a.Region.RegionAr,
                MapLatitude = a.MapLatitude,
                MapLongitude = a.MapLongitude,
                Phone= a.Phone,
                Mobile=a.Mobile


            }).ToList();
            TempData["Address"] = RestauarntDataJson ;
            return Json(new { data = RestauarntDataJson }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemFoods(int id)
        {
            var listItemFood = db.ItemFoods.Where(a => a.IsDetete == false && a.Category.RestaurantID == id).ToList().Select(i => new
            {
                FoodName = i.FoodName,
                ItemPrice = i.Price,
                ItemSize = i.Size.SizeAR,
            }).ToList();
            return Json(listItemFood, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ChangeStatus(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return null;
            }
            Restaurant restaurant = db.Restaurants.Find(id);

            restaurant.RestaurantStatus = restaurant.RestaurantStatus == 1 ? 2 : 1;
            try
            {
                db.Entry(restaurant).State = EntityState.Modified;
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



        public ActionResult Create(string ResUserId,string ResUserName , int ?Msg)
        {
            RestaurantVM restaurantVM = new RestaurantVM();
            /// if parameters have values mean we in the wizard
            if (!string.IsNullOrEmpty(ResUserId))
            {
                ViewBag.UserName = ResUserName;
            }
            //ViewBag.FeeTypeID = new SelectList(db.FeeTypes.Where(c => c.IsActive == true
            //&& c.IsDetete == false), "ID", "FeeNameAr");

            ViewBag.RestaurantStatus = new SelectList(db.RestaurantStatus.Where(c => c.IsActive == true
            && c.IsDetete == false), "ID", "RestaurantStatusAr");

            ViewBag.DeliveryWayID = new SelectList(db.DeliveryWays.Where(c => c.IsActive == true
           && c.IsDetete == false), "ID", "DeliveryNameAr");

            string roleid = dbMembership.Roles.Where(x => x.Name == "Restaurant").FirstOrDefault().Id;
            // Here need only users with no restaurants attacked with them
            var resusersid = db.Restaurants.Where(r => r.UserID != string.Empty).Select(r => r.UserID).ToList();

            ViewBag.UserID = new SelectList(dbMembership.Users.Where(u => u.Roles.FirstOrDefault().RoleId == roleid && !resusersid.Contains(u.Id)), "ID", "UserName");

           // ViewBag.RestaurantOpeningID = new SelectList(db.RestaurantOpenings.Where(c => c.IsActive == true
           //&& c.IsDetete == false), "ID", "RestaurantStatusAr");

            ViewBag.CountryID = new SelectList(db.Countries.Where(c => c.IsActive == true
            && c.IsDetete == false), "ID", "CountryAr");

            ViewBag.CityID = new SelectList(db.Cities.Where(c => c.IsActive == true
            && c.IsDelete == false), "ID", "CityAr");


            ViewBag.RegionID = new SelectList(db.Regions.Where(c => c.IsActive == true
            && c.IsDetete == false), "ID", "RegionAr");

            ViewBag.RegionN = new SelectList(db.Regions.Where(c => c.IsActive == true
            && c.IsDetete == false), "ID", "RegionAr");
         
            restaurantVM.wizard = true;
            return View(restaurantVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RestaurantVM restaurantVM, HttpPostedFileBase FileName)
        {
            var resusersid = db.Restaurants.Where(r => r.UserID != string.Empty).Select(r => r.UserID).ToList();
            var RestRoleId = (dbMembership.Roles.Where(x => x.Name == "Restaurant").FirstOrDefault().Id);
            if (TempData["Address"] == null)
            {
                    ModelState.AddModelError("RestaurantAddresses", "يجب اضافه عنوان فرع واحد على الاقل للمطعم");
                    //return View(restaurantVM);
            }

            if (ModelState.IsValid)
            {
               
               
                Restaurant restaurant = new Restaurant();
                restaurant.RestaurantName = restaurantVM.RestaurantName;
                restaurant.RestaurantNameEn = restaurantVM.RestaurantNameEn;
                restaurant.RestaurantDescrption = restaurantVM.RestaurantDescrption;
                restaurant.RestaurantDescrptionEn = restaurantVM.RestaurantDescrptionEn;
                restaurant.RestaurantPhone = restaurantVM.RestaurantPhone;
                restaurant.DeliveryWayID = restaurantVM.DeliveryWayID;
                //restaurant.RestaurantOpeningID = restaurantVM.RestaurantOpeningID;
                //restaurant.FeeTypeID = restaurantVM.FeeTypeID;
                restaurant.UserID = restaurantVM.UserID;
                restaurant.RestaurantStatus = restaurantVM.RestaurantStatus;
                restaurant.Stars = restaurantVM.Stars;
                //restaurant.MapLatitude = restaurantVM.MapLatitude;
                //restaurant.MapLongitude = restaurantVM.MapLongitude;

                //DateTime time = DateTime.Parse(restaurantVM.Openingtime);
                //restaurant.Openingtime = time.TimeOfDay;
                //DateTime ctime = DateTime.Parse(restaurantVM.Closetime);
                //restaurant.Closetime = ctime.TimeOfDay;

                restaurant.Date_Created = DateTime.Now;
                restaurant.Date_modified = DateTime.Now;
                restaurant.Country_Id = restaurantVM.CountryID;
                restaurant.City_Id = restaurantVM.CityID;
                restaurant.Region_Id = restaurantVM.RegionID;
                restaurant.Address = restaurantVM.Address;
                #region media

                if (FileName != null && FileName.ContentLength > 0)
                {

                    restaurant.Logo = savemsgimg(FileName);

                }
                #endregion


               
                // restaurant.MembershipExpired = DateTime.Now;
                try
                {
                    db.Restaurants.Add(restaurant);
                    db.SaveChanges();
                    #region RestaurantAddress
                    if (TempData["Address"] != null)
                    {
                        if ((TempData["Address"] as List<AddressList>).Count > 0)
                        {
                            foreach (AddressList item in TempData["Address"] as List<AddressList>)
                            {
                                RestaurantData restaurantData = new RestaurantData();
                                restaurantData.Address = item.Address;
                                restaurantData.MapLatitude = item.MapLatitude;
                                restaurantData.MapLongitude = item.MapLongitude;
                                restaurantData.Phone = item.Phone;
                                restaurantData.Mobile = item.Mobile;
                                restaurantData.Region_id = item.Region_id;
                                restaurantData.RestaurantID = restaurant.ID;
                                restaurantData.IsActive = true;
                                db.RestaurantDatas.Add(restaurantData);

                            }
                        }
                       
                    }
                    

                    db.SaveChanges();

                    #endregion



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



                    //ViewBag.FeeTypeID = new SelectList(db.FeeTypes.Where(c => c.IsActive == true
                    //&& c.IsDetete == false), "ID", "FeeNameAr");
                    ViewBag.RestaurantStatus = new SelectList(db.RestaurantStatus.Where(c => c.IsActive == true
                  && c.IsDetete == false), "ID", "RestaurantStatusAr");
                    ViewBag.DeliveryWayID = new SelectList(db.DeliveryWays.Where(c => c.IsActive == true
                 && c.IsDetete == false), "ID", "DeliveryNameAr");

                    // Here need only users with no restaurants attacked with them
                    //var resusersid = db.Restaurants.Where(r => r.UserID != string.Empty).Select(r => r.UserID).ToList();

                    //var RestRoleId = (dbMembership.Roles.Where(x => x.Name == "Restaurant").FirstOrDefault().Id);
                    ViewBag.UserID = new SelectList(dbMembership.Users.Where(u => u.Roles.FirstOrDefault().RoleId ==  RestRoleId && !resusersid.Contains(u.Id)), "ID", "UserName");
                    //   ViewBag.RestaurantOpeningID = new SelectList(db.RestaurantOpenings.Where(c => c.IsActive == true
                    //&& c.IsDetete == false), "ID", "RestaurantStatusAr");

                    ViewBag.CountryID = new SelectList(db.Countries.Where(c => c.IsActive == true
                  && c.IsDetete == false), "ID", "CountryAr");

                    ViewBag.CityID = new SelectList(db.Cities.Where(c => c.IsActive == true
                    && c.IsDelete == false), "ID", "CityAr");


                    ViewBag.RegionID = new SelectList(db.Regions.Where(c => c.IsActive == true
                    && c.IsDetete == false), "ID", "RegionAr");
                    ViewBag.RegionN = new SelectList(db.Regions.Where(c => c.IsActive == true
                     && c.IsDetete == false), "ID", "RegionAr");
                    return View(restaurantVM);
                    throw;
                }

            }

         //   ViewBag.FeeTypeID = new SelectList(db.FeeTypes.Where(c => c.IsActive == true
         //&& c.IsDetete == false), "ID", "FeeNameAr");
            ViewBag.RestaurantStatus = new SelectList(db.RestaurantStatus.Where(c => c.IsActive == true
          && c.IsDetete == false), "ID", "RestaurantStatusAr");
            ViewBag.DeliveryWayID = new SelectList(db.DeliveryWays.Where(c => c.IsActive == true
         && c.IsDetete == false), "ID", "DeliveryNameAr");

            // Here need only users with no restaurants attacked with them
            //var resusersid = db.Restaurants.Where(r => r.UserID != string.Empty).Select(r => r.UserID).ToList();
            //var RestRoleId = (dbMembership.Roles.Where(x => x.Name == "Restaurant").FirstOrDefault().Id);

            ViewBag.UserID = new SelectList(dbMembership.Users.Where(u => u.Roles.FirstOrDefault().RoleId == RestRoleId && !resusersid.Contains(u.Id)), "ID", "UserName");
            //   ViewBag.RestaurantOpeningID = new SelectList(db.RestaurantOpenings.Where(c => c.IsActive == true
            //&& c.IsDetete == false), "ID", "RestaurantStatusAr");

            ViewBag.CountryID = new SelectList(db.Countries.Where(c => c.IsActive == true
          && c.IsDetete == false), "ID", "CountryAr");

            ViewBag.CityID = new SelectList(db.Cities.Where(c => c.IsActive == true
            && c.IsDelete == false), "ID", "CityAr");


            ViewBag.RegionID = new SelectList(db.Regions.Where(c => c.IsActive == true
            && c.IsDetete == false), "ID", "RegionAr");

            ViewBag.RegionN = new SelectList(db.Regions.Where(c => c.IsActive == true
                     && c.IsDetete == false), "ID", "RegionAr");

            return View(restaurantVM);
        }

        public ActionResult GetCityList(int? id)
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

        public ActionResult GetRegionList(int? id)
        {
            var Data = (Object)null;
            try
            {
                Data = db.Regions.Where(x => x.IsActive == true && x.IsDetete == false && x.CityID == id).Select(i => new { ID = i.ID, Name = i.RegionAr }).ToList();

            }
            catch (Exception ex)
            {

            }
            return Json(new { data = Data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFeeValue(int? id)
        {
            string Data = "";
            try
            {
                //Data = db.FeeTypes.Where(x => x.IsActive == true && x.IsDetete == false && x.ID == id).FirstOrDefault().FeeValue.ToString();

            }
            catch (Exception ex)
            {

            }
            return Json(Data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurantVM = db.Restaurants.Find(id);
            if (restaurantVM == null)
            {
                return HttpNotFound();
            }
            RestaurantVM restaurant = new RestaurantVM();
            restaurant.ResID = restaurantVM.ID;
            restaurant.RestaurantName = restaurantVM.RestaurantName;
            restaurant.RestaurantNameEn = restaurantVM.RestaurantNameEn;
            restaurant.RestaurantDescrption = restaurantVM.RestaurantDescrption;
            restaurant.RestaurantDescrptionEn = restaurantVM.RestaurantDescrptionEn;
            restaurant.RestaurantPhone = restaurantVM.RestaurantPhone;
            restaurant.DeliveryWayID = restaurantVM.DeliveryWayID;
            //restaurant.RestaurantOpeningID = restaurantVM.RestaurantOpeningID;
            //restaurant.FeeTypeID = restaurantVM.FeeTypeID;
            restaurant.UserID = restaurantVM.UserID;
            restaurant.RestaurantStatus = restaurantVM.RestaurantStatus;
            restaurant.Stars = restaurantVM.Stars;
            restaurant.MapLatitude = restaurantVM.MapLatitude;
            restaurant.MapLongitude = restaurantVM.MapLongitude;


            //restaurant.Openingtime = restaurantVM.Openingtime.ToString();

            //restaurant.Closetime = restaurantVM.Closetime.ToString();

            restaurant.Date_Created = restaurantVM.Date_Created;
            restaurant.Date_modified = restaurantVM.Date_modified;
            restaurant.CountryID = restaurantVM.Country_Id;
            restaurant.CityID = restaurantVM.City_Id;
            restaurant.RegionID = restaurantVM.Region_Id;
            restaurant.Address = restaurantVM.Address;

          //  ViewBag.FeeTypeID = new SelectList(db.FeeTypes.Where(c => c.IsActive == true
          //&& c.IsDetete == false), "ID", "FeeNameAr", restaurant.FeeTypeID);
            ViewBag.RestaurantStatus = new SelectList(db.RestaurantStatus.Where(c => c.IsActive == true
          && c.IsDetete == false), "ID", "RestaurantStatusAr", restaurant.RestaurantStatus);
            ViewBag.DeliveryWayID = new SelectList(db.DeliveryWays.Where(c => c.IsActive == true
         && c.IsDetete == false), "ID", "DeliveryNameAr", restaurant.DeliveryWayID);

            // Here need only users with no restaurants attacked with them

            var resusersid = db.Restaurants.Where(r => r.UserID != string.Empty && r.UserID != restaurant.UserID).Select(r => r.UserID).ToList();
            var RestRoleId = (dbMembership.Roles.Where(x => x.Name == "Restaurant").FirstOrDefault().Id);
            ViewBag.UserID = new SelectList(dbMembership.Users.Where(u => u.Roles.FirstOrDefault().RoleId == RestRoleId && !resusersid.Contains(u.Id)), "ID", "UserName", restaurant.UserID);


            
         //   ViewBag.RestaurantOpeningID = new SelectList(db.RestaurantOpenings.Where(c => c.IsActive == true
         //&& c.IsDetete == false), "ID", "RestaurantStatusAr", restaurant.RestaurantOpeningID);

            ViewBag.CountryID = new SelectList(db.Countries.Where(c => c.IsActive == true && c.IsDetete == false), "ID", "CountryAr", restaurant.CountryID);

            ViewBag.CityID = new SelectList(db.Cities.Where(c => c.IsActive == true && c.IsDelete == false), "ID", "CityAr", restaurant.CityID);


            ViewBag.RegionID = new SelectList(db.Regions.Where(c => c.IsActive == true && c.IsDetete == false), "ID", "RegionAr", restaurant.RegionID);

            ViewBag.RegionN = new SelectList(db.Regions.Where(c => c.IsActive == true && c.IsDetete == false), "ID", "RegionAr");

            return View(restaurant);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RestaurantVM restaurantVM, HttpPostedFileBase FileName)
        {
            if (ModelState.IsValid)
            {
                Restaurant restaurant = db.Restaurants.Find(restaurantVM.ResID);
                restaurant.RestaurantName = restaurantVM.RestaurantName;
                restaurant.RestaurantNameEn = restaurantVM.RestaurantNameEn;
                restaurant.RestaurantDescrption = restaurantVM.RestaurantDescrption;
                restaurant.RestaurantDescrptionEn = restaurantVM.RestaurantDescrptionEn;
                restaurant.RestaurantPhone = restaurantVM.RestaurantPhone;
                restaurant.DeliveryWayID = restaurantVM.DeliveryWayID;
               // restaurant.RestaurantOpeningID = restaurantVM.RestaurantOpeningID;
                //restaurant.FeeTypeID = restaurantVM.FeeTypeID;
                restaurant.UserID = restaurantVM.UserID;
                restaurant.RestaurantStatus = restaurantVM.RestaurantStatus;
                restaurant.Stars = restaurantVM.Stars;
                restaurant.MapLatitude = restaurantVM.MapLatitude;
                restaurant.MapLongitude = restaurantVM.MapLongitude;

                //DateTime time = DateTime.Parse(restaurantVM.Openingtime);
                //restaurant.Openingtime = time.TimeOfDay;
                //DateTime ctime = DateTime.Parse(restaurantVM.Closetime);
                //restaurant.Closetime = ctime.TimeOfDay;
                restaurant.Date_modified = DateTime.Now;

                restaurant.Country_Id = restaurantVM.CountryID;
                restaurant.City_Id = restaurantVM.CityID;
                restaurant.Region_Id = restaurantVM.RegionID;
                restaurant.Address = restaurantVM.Address;

                #region media
                if (FileName != null && FileName.ContentLength > 0)
                {
                    removefile(restaurant.Logo);
                    restaurant.Logo = savemsgimg(FileName);

                }
                #endregion
                db.Entry(restaurant).State = EntityState.Modified;

                #region RestaurantAddress
             if (TempData["Address"] !=null)
             {
                 if ((TempData["Address"] as List<AddressList>).Count>0)
                 {
                        //DM 23-8-2017
                        //Update RestauandBracncehs ISDelete=True
                         db.RestaurantDatas
                        .Where(x => x.RestaurantID == restaurant.ID)
                        .ToList()
                        .ForEach(a => a.IsDetete = true);

                        //Then Add New Resords
                        //db.RestaurantDatas.RemoveRange(db.RestaurantDatas.Where(x => x.RestaurantID == restaurant.ID).ToList());
                        foreach (AddressList item in TempData["Address"] as List<AddressList>)
                    {
                        RestaurantData restaurantData = new RestaurantData();
                        restaurantData.Address = item.Address;
                        restaurantData.MapLatitude = item.MapLatitude;
                        restaurantData.MapLongitude = item.MapLongitude;
                        restaurantData.Phone = item.Phone;
                        restaurantData.Mobile = item.Mobile;
                        restaurantData.Region_id = item.Region_id;
                        restaurantData.RestaurantID = restaurant.ID;
                        restaurantData.IsActive = true;
                        db.RestaurantDatas.Add(restaurantData);

                    }
                    
                        
                        //db.SaveChanges();
                    }
                }
                #endregion

                db.SaveChanges();
                return RedirectToAction("Index", new { Msg = 4 });
            }
       //     ViewBag.FeeTypeID = new SelectList(db.FeeTypes.Where(c => c.IsActive == true
       //&& c.IsDetete == false), "ID", "FeeNameAr", restaurantVM.FeeTypeID);
            ViewBag.RestaurantStatus = new SelectList(db.RestaurantStatus.Where(c => c.IsActive == true
          && c.IsDetete == false), "ID", "RestaurantStatusAr", restaurantVM.RestaurantStatus);
            ViewBag.DeliveryWayID = new SelectList(db.DeliveryWays.Where(c => c.IsActive == true
         && c.IsDetete == false), "ID", "DeliveryNameAr", restaurantVM.DeliveryWayID);

            // Here need only users with no restaurants attacked with them
            var resusersid = db.Restaurants.Where(r => r.UserID != string.Empty && r.UserID != restaurantVM.UserID).Select(r => r.UserID).ToList();

            ViewBag.UserID = new SelectList(dbMembership.Users.Where(u => u.Roles.FirstOrDefault().RoleId == (dbMembership.Roles.Where(x => x.Name == "Restaurant").FirstOrDefault().Id) && !resusersid.Contains(u.Id)), "ID", "UserName", restaurantVM.UserID);
            //     ViewBag.RestaurantOpeningID = new SelectList(db.RestaurantOpenings.Where(c => c.IsActive == true
            //&& c.IsDetete == false), "ID", "RestaurantStatusAr", restaurantVM.RestaurantOpeningID);

            ViewBag.CountryID = new SelectList(db.Countries.Where(c => c.IsActive == true && c.IsDetete == false), "ID", "CountryAr", restaurantVM.CountryID);

            ViewBag.CityID = new SelectList(db.Cities.Where(c => c.IsActive == true && c.IsDelete == false), "ID", "CityAr", restaurantVM.CityID);


            ViewBag.RegionID = new SelectList(db.Regions.Where(c => c.IsActive == true && c.IsDetete == false), "ID", "RegionAr", restaurantVM.RegionID);
            ViewBag.RegionN = new SelectList(db.Regions.Where(c => c.IsActive == true
            && c.IsDetete == false), "ID", "RegionAr");
            return View(restaurantVM);
        }

        #region Media

        private string savemsgimg(HttpPostedFileBase FilePath)
        {
            string pic = System.IO.Path.GetFileName(FilePath.FileName);
            //var parsepic = pic.Split('.');
            string vExtension = Path.GetExtension(FilePath.FileName);
            Random rand = new Random();
            //pic = rand.Next() + "." + parsepic[1];
            pic = rand.Next() +  vExtension;
            string path = string.Empty;
            path = System.IO.Path.Combine(
                                       Server.MapPath(mediapath), pic);


            // file is uploaded
            FilePath.SaveAs(path);
            return pic;
        }

        private void removefile(string filename)
        {
            string fullPath = string.Empty;

            fullPath = Request.MapPath(mediapath + filename);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        #endregion

        #region RestaurantData
        [HttpPost]
        public string addtempAddress(List<AddressList> addressList)
        {
            if (addressList == null)
            {

                return "false";
            }
            TempData["Address"] = addressList;
           
            return "true";

        }

        public bool IsRestaurantBranchHasOrder(int restaurantID)
        {
            bool hasOrders = false;
            return hasOrders;

        }
        #endregion

        public PartialViewResult DisplayRestaurantAddresses(int ID)
        {
            RestaurantVM model = new RestaurantVM();


            IQueryable<RestaurantData> RestaurantDataList = db.Restaurants.Where(x => x.RestaurantStatus == 1).Where(x => x.ID == ID)
                                     .Join(db.RestaurantDatas,
                                      pi => pi.ID,
                                      v => v.RestaurantID,
                                      (pi, v) => new { Restaurant = pi, RestaurantData = v })
                                      .Where(c=>c.RestaurantData.IsActive==true && c.RestaurantData.IsDetete==false )
                                      .Select(x => x.RestaurantData);
            

            model.RestaurantDataList = RestaurantDataList.ToList();
          
            return PartialView("DisplayRestaurantAddresses", model);
        }
    }
}