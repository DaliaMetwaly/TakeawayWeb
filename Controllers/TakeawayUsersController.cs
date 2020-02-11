using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Takeaway.Models;
using static Takeaway.Controllers.Common;

namespace Takeaway.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class TakeawayUsersController : Controller
    {
        private TakeawayEntities db = new TakeawayEntities();
        private ApplicationDbContext dbMembership = new ApplicationDbContext();
        /// <summary>
        /// For Restaurants and Admins
        /// </summary>
        /// <param name="Msg"></param>
        /// <returns></returns>
        // GET: TakeawayUsers
        public ActionResult Index(int? Msg)
        {
            if (Msg != null)
            {
                ViewBag.Msg = Msg;
            }

            return View();
        }
        /// <summary>
        /// For Users only
        /// </summary>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public ActionResult Index2(int? Msg)
        {
            if (Msg != null)
            {
                ViewBag.Msg = Msg;
            }

            return View();
        }


        public ActionResult LoadData()
        {
            string[] usrRoles = new string[] { "Restaurant", "Admin" };
            // var UsrInRoles = dbMembership.Users.Where(x=> usrRoles.Contains(x.Roles.FirstOrDefault().RoleId)).Select(a=>a.Id).ToArray();

            var UsrInRoles = (from s in dbMembership.Users
                              where usrRoles.Contains(dbMembership.Roles.Where(x => x.Id == s.Roles.FirstOrDefault().RoleId).FirstOrDefault().Name)
                              select new { s.Id, s.UserName }).ToList();


            var UsersData = from u in db.Users.ToList()
                            join r in UsrInRoles
                            on u.ID equals r.Id
                            where u.IsDetete == false
                            select new
                            {
                                Name = u.ContactName,
                                UserName = r.UserName,
                                Phone = u.ContactPhone,
                                Email = u.ContactEmail,
                                RoleName = dbMembership.Roles.Where(s => s.Users.Where(n => n.UserId == u.ID).FirstOrDefault().UserId == u.ID).FirstOrDefault().Name  /* Get role name for UserID*/ ,
                                UserCategory = Common.GetEnumUserCategoryName(u.UserCategory),
                                ChangePassword = "<button type='button' class='delete btn default btn-xs green' data-toggle='modal' data-target='#myModal'  onclick='ChangePassword(\"" + u.ID + "\")' >  تغير كلمة المرور </button>",
                                ChangeEmail = "<button type='button' class='delete btn default btn-xs green' data-toggle='modal' data-target='#myModalEmail'  onclick='ChangeEmail(\"" + u.ID + "\")' >  تغير البريد الالكترونى </button>",
                                IsActive = u.IsActive == true ? "<span id='btn_ChangeStatus'  style='cursor:pointer;cursor:hand;' onclick='Change(\"" + u.ID + "\")'  class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_ChangeStatus'  style='cursor:pointer;cursor:hand;' onclick='Change(\"" + u.ID + "\")'  class='glyphicon glyphicon-remove'></ span >",
                                Edit = "<a  id='btn_Edit' class='btn default btn-xs green' href='/TakeawayUsers/Edit/" + u.ID + "'>تعديل</a>",
                                Deleted = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' onclick='Delete(\"" + u.ID + "\")'  > حذف</a>"
                            };


            return Json(new { data = UsersData }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadCustomerData()
        {


            var UsrInRoles = (from s in dbMembership.Users
                              where dbMembership.Roles.Where(x => x.Id == s.Roles.FirstOrDefault().RoleId).FirstOrDefault().Name == "User"
                              select new { s.Id, s.UserName }).ToList();


            var UsersData = from u in db.Users.ToList()
                            join r in UsrInRoles
                            on u.ID equals r.Id
                            where u.IsDetete == false
                            select new
                            {
                                Name = u.ContactName,
                                UserName = r.UserName,
                                Phone = u.ContactPhone,
                                Email = u.ContactEmail,
                                //RoleName = dbMembership.Roles.Where(s => s.Users.Where(n => n.UserId == u.ID).FirstOrDefault().UserId == u.ID).FirstOrDefault().Name  /* Get role name for UserID*/ ,
                                CreationDate = "<span  id = 'CreationDate' class=''>" + Convert.ToDateTime(u.RegisterDate).ToString("dd/MM/yyyy hh:mm") + "</span>",
                                UserCategory = Common.GetEnumUserCategoryName(u.UserCategory),
                                ChangePassword = "<button type='button' class='delete btn default btn-xs green' data-toggle='modal' data-target='#myModal'  onclick='ChangePassword(\"" + u.ID + "\")' >  تغير كلمة المرور </button>",
                                ChangeEmail = "<button type='button' class='delete btn default btn-xs green' data-toggle='modal' data-target='#myModalEmail'  onclick='ChangeEmail(\"" + u.ID + "\")' >  تغير البريد الالكترونى </button>",
                                IsActive = u.IsActive == true ? "<span id='btn_ChangeStatus'  style='cursor:pointer;cursor:hand;' onclick='Change(\"" + u.ID + "\")'  class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_ChangeStatus'  style='cursor:pointer;cursor:hand;' onclick='Change(\"" + u.ID + "\")'  class='glyphicon glyphicon-remove'></ span >",
                                Edit = "<a  id='btn_Edit' class='btn default btn-xs green' href='/TakeawayUsers/Edit/" + u.ID + "'>تعديل</a>",
                                Deleted = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' onclick='Delete(\"" + u.ID + "\")'  > حذف</a>"
                            };


            return Json(new { data = UsersData }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadUserData(string usrID)
        {
            int Srno = 1;
            var UsersDataJson = db.UserDatas.Where(a => a.IsDetete == false && a.IsActive == true && a.UserID == usrID).ToList().Select(a => new
            {
                count = Srno++,
                Address = a.Address,
                Region_id = a.Region_id,
                Region_Name = a.Region.RegionAr,
                MapLatitude = a.MapLatitude,
                MapLongitude = a.MapLongitude


            }).ToList();
            return Json(new { data = UsersDataJson }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ChangeStatus(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return null;
            }

            try
            {
                User user = db.Users.Find(id);
                user.IsActive = user.IsActive == true ? false : true;
                db.Entry(user).State = EntityState.Modified;
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

            }


            return Json("1", JsonRequestBehavior.AllowGet);
        }


        // GET: TakeawayUsers/Create
        public ActionResult Create(int? isClient)
        {
            if (isClient != null)
            {
                ViewBag.isClient = isClient;
            }

            ApplicationDbContext dbadmin = new ApplicationDbContext();

            //ViewBag.PayTypeId = new SelectList(db.PayTypes, "ID", "PayNameAr");

            ViewBag.RegionN = new SelectList(db.Regions.ToList(), "ID", "RegionAr");

            ViewBag.RoleId = new SelectList(dbadmin.Roles.Where(x => x.Name != "SuperAdmin").ToList(), "Id", "Name");

            if (isClient != null)
            {
                if (isClient == 0)
                {
                    ViewBag.RoleId = new SelectList(dbadmin.Roles.Where(x => x.Name != "SuperAdmin" && x.Name != "User").ToList(), "Id", "Name");
                }
                else if (isClient == 1)
                {

                    ViewBag.RoleId = new SelectList(dbadmin.Roles.Where(x => x.Name != "SuperAdmin" && x.Name == "User").ToList(), "Id", "Name");
                }

            }
            //else
            //{
            //    ViewBag.RoleId = new SelectList(dbadmin.Roles.Where(x => x.Name != "SuperAdmin").ToList(), "Id", "Name");
            //}


            //ViewBag.UserCategory = GetUserCategory();
            TakeawayUser takeawayUser = new TakeawayUser();
            takeawayUser.isClient = isClient.Value;
            // User Country
            //ViewBag.CountryN = new SelectList(db.Countries.ToList(), "ID", "CountryAr");

            //ViewBag.CityN = new SelectList(db.Cities.ToList(), "ID", "CityAr");
            return View(takeawayUser);
        }
        //SelectList GetUserCategory()
        //{
        //    var enums = new List<SelectListItem>();
        //    foreach (int value in Enum.GetValues(typeof(UserCategory)))
        //    {
        //        var item = new SelectListItem();
        //        item.Value = value.ToString();
        //        item.Text = Common.GetEnumUserCategoryName(value);// Enum.GetName(typeof(OfferType), value);

        //        enums.Add(item);
        //    }

        //    return new SelectList(enums, "Value", "Text");
        //}
        // POST: TakeawayUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TakeawayUser takeawayUser)
        {
            //bool status = false;
            string userid = string.Empty;
            string username = string.Empty;
            try
            {
                if (ModelState.ContainsKey("RoleID"))
                    ModelState["RoleID"].Errors.Clear();

                if (ModelState.IsValid)
                {
                    UserStore<ApplicationUser> _store = new UserStore<ApplicationUser>(dbMembership);
                    ApplicationUserManager UserManager = new ApplicationUserManager(_store);
                    ApplicationUser membershipUser = new ApplicationUser();

                    membershipUser.UserName = takeawayUser.UserName;
                    membershipUser.Email = takeawayUser.ContactEmail;
                    membershipUser.PhoneNumber = takeawayUser.ContactPhone;

                    var result = UserManager.CreateAsync(membershipUser, takeawayUser.Password);
                    if (result.Result.Succeeded)
                    {
                        string tempRole="" ;
                        if (takeawayUser.isClient == 0) // is Restaurant or admin
                        {
                            tempRole = dbMembership.Roles.Find(takeawayUser.RoleID).Name;
                        }
                        if (takeawayUser.isClient == 1)// is user 
                        {
                            tempRole = "User";
                        }
                            result = UserManager.AddToRolesAsync(membershipUser.Id, tempRole);
                        if (result.Result.Succeeded)
                        {
                            User newuser = new Models.User();
                            newuser.ID = membershipUser.Id;
                            newuser.ContactEmail = takeawayUser.ContactEmail;
                            newuser.ContactPhone = takeawayUser.ContactPhone;
                            newuser.ContactName = takeawayUser.ContactName;
                            newuser.IsActive = true;
                            newuser.IsDetete = takeawayUser.IsDetete;
                            //newuser.PayTypeID = takeawayUser.PayTypeID;
                            newuser.RegisterDate = DateTime.Now;
                            newuser.UserCategory = (int)UserCategory.website;
                            db.Users.Add(newuser);
                            userid = newuser.ID;
                            username = newuser.ContactName;

                            if (takeawayUser.Address!=null)
                            {
                                for(int i=0;i< takeawayUser.Address.Count;i++)
                                {
                                    UserData Userdataitem     = new UserData();
                                    Userdataitem.Region_id    = takeawayUser.Region_id[i];
                                    Userdataitem.Address      = takeawayUser.Address[i];
                                    Userdataitem.UserID       = membershipUser.Id;
                                    Userdataitem.MapLatitude  = takeawayUser.MapLatitude[i];
                                    Userdataitem.MapLongitude = takeawayUser.MapLongitude[i];
                                    Userdataitem.IsActive = true;
                                    db.UserDatas.Add(Userdataitem);
                                }
                                //userData = takeawayUser.UserDataList;
                                //foreach (var userDataItem in userData)
                                //{
                                //    UserData singleUserData = new UserData();
                                //    singleUserData.UserID = membershipUser.Id;
                                //    singleUserData.Address = userDataItem.Address;
                                //    singleUserData.Region_id = userDataItem.Region_id;
                                //    singleUserData.MapLatitude = userDataItem.MapLatitude;
                                //    singleUserData.MapLongitude = userDataItem.MapLongitude;
                                //    singleUserData.IsActive = true;
                                //    db.UserDatas.Add(singleUserData);
                                //}

                            }

                            db.SaveChanges();
                            if (takeawayUser.isClient == 0) // is Restaurant or admin
                            {
                                //return JsonResult()
                                return RedirectToAction("Index", "TakeawayUsers", new { Msg = 6 });
                            }
                            if (takeawayUser.isClient == 1) // is user 
                            {
                                return RedirectToAction("Index2", "TakeawayUsers", new { Msg = 6 });
                            }
                        }
                    }
                }


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
                // prepare my create view again
                //ApplicationDbContext dbadmin = new ApplicationDbContext();

                //ViewBag.PayTypeId = new SelectList(db.PayTypes, "ID", "PayNameAr");

                ViewBag.RegionN = new SelectList(db.Regions.ToList(), "ID", "RegionAr");

                ViewBag.RoleId = new SelectList(dbMembership.Roles.Where(x => x.Name != "SuperAdmin").ToList(), "Id", "Name");

                //ViewBag.UserCategory = GetUserCategory();

                return View(takeawayUser);

            }


            ////if (takeawayUser.isClient == "0")//User Role
            ////{
            ////    return RedirectToAction("Index2", new { Msg = 6 });
            ////}
            ////if restaurant or admin  Role
            ////  return RedirectToAction("Index", new { Msg = 6 });
            //// return new JsonResult { Data = new { status = status, isClient= isClientVar } };
            //if (takeawayUser.isClient == "0")
            //{
            //    return RedirectToAction("Create", "Restaurant", new { ResUserId = userid ,ResUserName = username, Msg = 1});
            //}
            return View();
            //return Json(takeawayUser.isClient, JsonRequestBehavior.AllowGet);
        }

        // GET: TakeawayUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var Usersmodel = db.Users.Find(id);
            var UserDataModel = db.UserDatas.Where(x => x.UserID == id).ToList();
            TakeawayUser takeawayUser = new TakeawayUser();
            takeawayUser.ID = Usersmodel.ID;
            takeawayUser.ContactName = Usersmodel.ContactName;
            takeawayUser.UserName = Usersmodel.AspNetUser.UserName;
            //takeawayUser.Password = Usersmodel.AspNetUser.PasswordHash;
            takeawayUser.ContactEmail = Usersmodel.ContactEmail;
            takeawayUser.ContactPhone = Usersmodel.ContactPhone;
            //takeawayUser.PayTypeID = Usersmodel.PayTypeID;
            takeawayUser.RoleID = dbMembership.Roles.Where(r => r.Users.Where(u => u.UserId == Usersmodel.ID).FirstOrDefault().UserId == Usersmodel.ID).FirstOrDefault().Id;  //Usersmodel.RoleID;
            //takeawayUser.IsActive = Usersmodel.IsActive;
            takeawayUser.IsDetete = Usersmodel.IsDetete;
            if (UserDataModel!=null)
            {
                takeawayUser.Address = new List<string>();
                takeawayUser.Region_id = new List<int>();
                takeawayUser.MapLatitude = new List<string>();
                takeawayUser.MapLongitude = new List<string>();
                takeawayUser.RegionName = new List<string>();
                foreach (UserData userDataItem in UserDataModel)
                {
                    takeawayUser.Address.Add(userDataItem.Address);
                    takeawayUser.Region_id.Add(userDataItem.Region_id);
                    takeawayUser.MapLatitude.Add(userDataItem.MapLatitude);
                    takeawayUser.MapLongitude.Add(userDataItem.MapLongitude);
                    takeawayUser.RegionName.Add(db.Regions.Find(userDataItem.Region_id).RegionAr);
                }
            }
           
            
            //takeawayUser.IP_Address = Usersmodel.IP_Address;
            //takeawayUser.LastLogin = Usersmodel.LastLogin;
            //takeawayUser.RegisterDate = Usersmodel.RegisterDate;
            //takeawayUser.UserDataList = Usersmodel.UserDatas.ToList();

            if (takeawayUser == null)
            {
                return HttpNotFound();
            }
            // ViewBag.PayTypeID = new SelectList(db.PayTypes, "ID", "PayNameAr", takeawayUser.PayTypeID);
            ViewBag.RoleId = new SelectList(dbMembership.Roles.Where(x => x.Name != "SuperAdmin").ToList(), "Id", "Name", takeawayUser.RoleID);
            ViewBag.RegionN = new SelectList(db.Regions.ToList(), "ID", "RegionAr");

            return View(takeawayUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TakeawayUser takeawayUser)
        {
            bool status = false;
            try { 
            if (ModelState.ContainsKey("Password"))
                ModelState["Password"].Errors.Clear();


            if (ModelState.IsValid)
            {

                //Update Aspnet Users
                ApplicationUser usr = dbMembership.Users.Find(takeawayUser.ID);
                usr.UserName = takeawayUser.UserName;
                usr.Email = takeawayUser.ContactEmail;
                usr.PhoneNumber = takeawayUser.ContactPhone;
                var result = dbMembership.SaveChanges();


                UserStore<ApplicationUser> _store = new UserStore<ApplicationUser>(dbMembership);
                ApplicationUserManager UserManager = new ApplicationUserManager(_store);

                if (UserManager.IsInRole(usr.Id, dbMembership.Roles.Find(takeawayUser.RoleID).Name) == false)
                {
                    var resultRemove = UserManager.RemoveFromRoles(usr.Id, dbMembership.Roles.Find(usr.Roles.FirstOrDefault().RoleId).Name);
                    if (resultRemove.Succeeded)
                    {
                        var resultAdd = UserManager.AddToRole(takeawayUser.ID, dbMembership.Roles.Find(takeawayUser.RoleID).Name);
                        if (resultAdd.Succeeded == false)
                        {
                            status = false;
                        }

                    }
                }

                //Update Table User  
                User tempUsr = db.Users.Find(takeawayUser.ID);
                tempUsr.ContactEmail = takeawayUser.ContactEmail;
                tempUsr.ContactName = takeawayUser.ContactName;
                tempUsr.ContactPhone = takeawayUser.ContactPhone;
                db.Entry(tempUsr).State = EntityState.Modified;

                //**************************************************************
               

                if (takeawayUser.Address != null)
                {
                    db.UserDatas.RemoveRange(db.UserDatas.Where(x => x.UserID == takeawayUser.ID).ToList());

                    for (int i = 0; i < takeawayUser.Address.Count; i++)
                    {
                        UserData Userdataitem = new UserData();
                        Userdataitem.Region_id = takeawayUser.Region_id[i];
                        Userdataitem.Address = takeawayUser.Address[i];
                        Userdataitem.UserID = takeawayUser.ID;
                        Userdataitem.MapLatitude = takeawayUser.MapLatitude[i];
                        Userdataitem.MapLongitude = takeawayUser.MapLongitude[i];
                        Userdataitem.IsActive = true;
                        db.UserDatas.Add(Userdataitem);
                    }
                   
                }
                
                db.SaveChanges();

                if (takeawayUser.isClient == 0) // is Restaurant or admin
                {
                    //return JsonResult()
                    return RedirectToAction("Index", "TakeawayUsers", new { Msg = 6 });
                }
                if (takeawayUser.isClient == 1) // is user 
                {
                    return RedirectToAction("Index2", "TakeawayUsers", new { Msg = 6 });
                }

             }
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

                ViewBag.RegionN = new SelectList(db.Regions.ToList(), "ID", "RegionAr");
                ViewBag.RoleId = new SelectList(dbMembership.Roles.Where(x => x.Name != "SuperAdmin").ToList(), "Id", "Name", takeawayUser.RoleID);
                return View(takeawayUser);

            }
            ViewBag.RegionN = new SelectList(db.Regions.ToList(), "ID", "RegionAr");
            ViewBag.RoleId = new SelectList(dbMembership.Roles.Where(x => x.Name != "SuperAdmin").ToList(), "Id", "Name", takeawayUser.RoleID);
            return View(takeawayUser);

        }


        [HttpGet]
        public PartialViewResult ChangePassword(string Id)
        {
            UserStore<ApplicationUser> _store = new UserStore<ApplicationUser>(dbMembership);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(_store);
            var User = dbMembership.Users.Single(u => u.Id == Id);
            ChangeUserPassword Model = new ChangeUserPassword();
            Model.UserName = User.UserName;
            Model.UserId = User.Id;
            ModelState.Clear();
            return PartialView("ChangePassword", Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangeUserPassword Model)
        {
            ModelState.Remove("OldEmail");
            ModelState.Remove("NewEmail");

            if (ModelState.IsValid)
            {
                UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());

                userManager.RemovePassword(Model.UserId);
                var result = userManager.AddPassword(Model.UserId, Model.NewPassword);
                if (result.Succeeded)
                {
                    ViewBag.Msg = 3;
                    return View("Index");
                }
                else
                {
                    //AddErrors(result);
                    //Model.ErrorMessage = "Error :" + result.Errors.ToList()[0];
                    //ViewBag.Error = "Error :" + result.Errors.ToList()[0];
                    ViewBag.Msg = 2;
                    //ViewBag.Error = ModelState.
                    return View("Index");
                }
            }
            else
            {
                //AddErrors(ModelState.);
                //Model.ErrorMessage = "Error :" + result.Errors.ToList()[0];
                //ViewBag.Error = "Error :" + result.Errors.ToList()[0];
                ViewBag.Msg = 2;
                //ViewBag.Error = ModelState.
                return View("Index");
            }

        }

        [HttpGet]
        public PartialViewResult ChangeEmail(string Id)
        {
            UserStore<ApplicationUser> _store = new UserStore<ApplicationUser>(dbMembership);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(_store);
            var User = dbMembership.Users.Single(u => u.Id == Id);
            ChangeUserPassword Model = new ChangeUserPassword();
            Model.UserName = User.UserName;
            Model.UserId = User.Id;
            ModelState.Clear();
            return PartialView("ChangeEmail", Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeEmail(ChangeUserPassword Model)
        {
            ModelState.Remove("OldEmail");
            ModelState.Remove("NewPassword");
            ModelState.Remove("RepeatPassword");

            if (ModelState.IsValid)
            {
                UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());

                var result = userManager.SetEmail(Model.UserId, Model.NewEmail);

                if (result.Succeeded)
                {
                    db.Users.Find(Model.UserId).ContactEmail = Model.NewEmail;
                    db.SaveChanges();

                    ViewBag.Msg = 5;
                    return View("Index");
                }
                else
                {

                    ViewBag.Msg = 2;
                    return View("Index");
                }
            }
            else
            {
                ViewBag.Msg = 2;
                return View("Index");
            }

        }

        //[HttpPost]
        //public JsonResult Delete(string usrID)
        //{
        //    bool status = true;
        //    db.Users.Remove(db.Users.Where(x=>x.ID == usrID).FirstOrDefault());
        //    db.UserDatas.Remove(db.UserDatas.Where(x=>x.UserID == usrID).FirstOrDefault());
        //    if(db.SaveChanges()>0)
        //    {
        //        //UserStore<ApplicationUser> _store = new UserStore<ApplicationUser>(dbMembership);
        //        //ApplicationUserManager UserManager = new ApplicationUserManager(_store);
        //        //resultRemove = UserManager.remo(usr.Id, dbMembership.Roles.Find(usr.Roles.FirstOrDefault().RoleId).Name);
        //        dbMembership.Users.Remove(dbMembership.Users.Find(usrID));
        //        dbMembership.SaveChanges();
        //    }
        //    else
        //    {
        //        status = false;
        //    }



        //    return new JsonResult { Data = new { status = status } };

        //}

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // POST: TakeawayUsers/Delete/5
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {

            try
            {
                db.Users.Remove(db.Users.Where(x => x.ID == id).FirstOrDefault());
                db.UserDatas.RemoveRange(db.UserDatas.Where(x => x.UserID == id).ToList());
                if (db.SaveChanges() > 0)
                {
                    dbMembership.Users.Remove(dbMembership.Users.Find(id));
                    dbMembership.SaveChanges();
                }
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("2", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult FillCities(int? CountryID)
        {
            if (CountryID != 0)
            {
                var CitiesList = db.Cities.Where(l => l.IsDelete == false
                && l.IsActive == true && l.CountryID == CountryID).Select(l => new { value = l.ID, text = l.CityAr });
                return Json(CitiesList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                throw new HttpException(500, "Error processing request.");
            }

        }

        public JsonResult FillRegions(int? CityID)
        {
            if (CityID != 0)
            {
                var RegionsList = db.Regions.Where(l => l.IsDetete == false
                && l.IsActive == true && l.CityID == CityID).Select(l => new { value = l.ID, text = l.RegionAr });
                return Json(RegionsList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                throw new HttpException(500, "Error processing request.");
            }

        }



    }
}
