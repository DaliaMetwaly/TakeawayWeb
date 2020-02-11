using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Takeaway.Models;
namespace Takeaway.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin,Restaurant")]
    public class RestaurantAppointmentController : Controller
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
            var Data = (Object)null;
            try
            {
                string usrID = User.Identity.GetUserId();
                string roleName = dbadmin.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;

                var restaurant = (roleName == "Restaurant") ?
                      db.Restaurants.Where(x => x.RestaurantStatus == 1 && x.UserID == usrID).ToList()
                    : db.Restaurants.Where(x => x.RestaurantStatus == 1).ToList();


                var restaurantAppointment = db.RestaurantAppointments.ToList();

                Data = (from r in restaurant
                        from o in restaurantAppointment
                            .Where(o => o.RestaurantID == r.ID && o.IsActive==true && o.IsDelete==false)
                            .DefaultIfEmpty()
                        select new {
                            RestaurantID = r.ID,
                            RestaurantName = r.RestaurantName,
                            IsAppoinment = db.RestaurantAppointments.Where(x => x.IsActive == true).Any(v => v.RestaurantID == r.ID)? "<span id='btn_ChangeStatus'>له</ span >" : "<span id='btn_ChangeStatus'>ليس</ span >",
                            IsActive = db.RestaurantAppointments.Where( x=>x.IsActive==true).Any(v => v.RestaurantID == r.ID)  ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + r.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + r.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                            Edit = db.RestaurantAppointments.Any(v => v.RestaurantID == r.ID)? "<a  id='btn_Edit' class='btn default btn-xs green' href='/RestaurantAppointment/Edit/" + r.ID + "'>تعديل</a>": "<a  id='btn_Edit' class='btn default btn-xs green' href='/RestaurantAppointment/Create/" + r.ID + "'>اضف مواعيد</a>",
                        }).Distinct();





                //from r in restaurant
                //        join o in restaurantAppointment.DefaultIfEmpty()
                //        on r.ID  equals o.RestaurantID
                //        //into cuc
                //        //from x in cuc.DefaultIfEmpty()
                //        select new
                //        {
                //            RestaurantID  = r.ID,
                //            RestaurantName = r.RestaurantName,
                //            //IsAppoinment = x.,
                           
                            
                //            Edit = "<a  id='btn_Edit' class='btn default btn-xs green' href='/RestaurantAppointment/Edit/" + r.ID + "'>تعديل</a>",
                           

                //        }).OrderBy(x => x.RestaurantName).ToList();


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
                return null;
            }
                        db.RestaurantAppointments
                        .Where(x => x.RestaurantID == id)
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

        public JsonResult GetRestaurantAppointments(int id)
        {
            var listRestaurantAppointment = db.RestaurantAppointments.Where(a => a.IsDelete == false &&a.RestaurantID == id).ToList().Select(i => new
            {
               

            Day =Common.GetEnumDayName(i.Day),
                OpeningTime = "<span  id = 'OpeningTime' class=''>" + DateTime.Today.Add(i.OpeningTime).ToString("hh:mm tt") + "</span>",
                CloseTime =   "<span  id = 'CloseTime' class=''>" + DateTime.Today.Add(i.CloseTime).ToString("hh:mm tt") + "</span>",
            }).ToList();
            return Json(listRestaurantAppointment, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(int?id)
        {
            string usrID = User.Identity.GetUserId();
            string roleName = dbadmin.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;

            ViewBag.RestaurantID = (roleName == "Restaurant") 
                                  ? new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1 && x.UserID == usrID), "ID", "RestaurantName",id) 
                                  : new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1), "ID", "RestaurantName",id);



            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RestaurantAppointmentVM restaurantAppointmentvm)
        {
            if (ModelState.IsValid)
            {
                //if (restaurantAppointmentvm.From1 != null && restaurantAppointmentvm.to1 != null)
                //{
                RestaurantAppointment restaurantAppointment1 = new RestaurantAppointment();
                restaurantAppointment1.RestaurantID = restaurantAppointmentvm.RestaurantID;
                restaurantAppointment1.Day = 1;
                if (restaurantAppointmentvm.From1 != null && restaurantAppointmentvm.to1 != null)
                {
                    DateTime time1 = DateTime.Parse(restaurantAppointmentvm.From1);
                    restaurantAppointment1.OpeningTime = time1.TimeOfDay;
                    DateTime ctime1 = DateTime.Parse(restaurantAppointmentvm.to1);
                    restaurantAppointment1.CloseTime = ctime1.TimeOfDay;
                }
                restaurantAppointment1.IsActive = true;
                db.RestaurantAppointments.Add(restaurantAppointment1);
                //}

                //if (restaurantAppointmentvm.From12 != null && restaurantAppointmentvm.to12 != null)
                //{
                RestaurantAppointment restaurantAppointment12 = new RestaurantAppointment();
                restaurantAppointment12.RestaurantID = restaurantAppointmentvm.RestaurantID;
                restaurantAppointment12.Day = 1;
                if (restaurantAppointmentvm.From12 != null && restaurantAppointmentvm.to12 != null)
                {
                    DateTime time12 = DateTime.Parse(restaurantAppointmentvm.From12);
                    restaurantAppointment12.OpeningTime = time12.TimeOfDay;
                    DateTime ctime12 = DateTime.Parse(restaurantAppointmentvm.to12);
                    restaurantAppointment12.CloseTime = ctime12.TimeOfDay;
                }
                restaurantAppointment12.IsActive = true;
                db.RestaurantAppointments.Add(restaurantAppointment12);
                //}




                RestaurantAppointment restaurantAppointment2 = new RestaurantAppointment();
                restaurantAppointment2.RestaurantID = restaurantAppointmentvm.RestaurantID;
                restaurantAppointment2.Day = 2;
                if (restaurantAppointmentvm.From2 != null && restaurantAppointmentvm.to2 != null)
                {
                    DateTime time2 = DateTime.Parse(restaurantAppointmentvm.From2);
                    restaurantAppointment2.OpeningTime = time2.TimeOfDay;
                    DateTime ctime2 = DateTime.Parse(restaurantAppointmentvm.to2);
                    restaurantAppointment2.CloseTime = ctime2.TimeOfDay;
                }
                restaurantAppointment2.IsActive = true;
                db.RestaurantAppointments.Add(restaurantAppointment2);






                RestaurantAppointment restaurantAppointment22 = new RestaurantAppointment();
                restaurantAppointment22.RestaurantID = restaurantAppointmentvm.RestaurantID;
                restaurantAppointment22.Day = 2;
                if (restaurantAppointmentvm.From22 != null && restaurantAppointmentvm.to22 != null)
                {
                    DateTime time22 = DateTime.Parse(restaurantAppointmentvm.From22);
                    restaurantAppointment22.OpeningTime = time22.TimeOfDay;
                    DateTime ctime22 = DateTime.Parse(restaurantAppointmentvm.to22);
                    restaurantAppointment22.CloseTime = ctime22.TimeOfDay;
                }
                restaurantAppointment22.IsActive = true;
                db.RestaurantAppointments.Add(restaurantAppointment22);




                RestaurantAppointment restaurantAppointment3 = new RestaurantAppointment();
                restaurantAppointment3.RestaurantID = restaurantAppointmentvm.RestaurantID;
                restaurantAppointment3.Day = 3;
                if (restaurantAppointmentvm.From3 != null && restaurantAppointmentvm.to3 != null)
                {
                DateTime time3 = DateTime.Parse(restaurantAppointmentvm.From3);
                restaurantAppointment3.OpeningTime = time3.TimeOfDay;
                DateTime ctime3 = DateTime.Parse(restaurantAppointmentvm.to3);
                restaurantAppointment3.CloseTime = ctime3.TimeOfDay;
                }
                restaurantAppointment3.IsActive = true;
                db.RestaurantAppointments.Add(restaurantAppointment3);

                //}

               
                    RestaurantAppointment restaurantAppointment23 = new RestaurantAppointment();
                    restaurantAppointment23.RestaurantID = restaurantAppointmentvm.RestaurantID;
                    restaurantAppointment23.Day = 3;
                     if (restaurantAppointmentvm.From32 != null && restaurantAppointmentvm.to32 != null)
                     {
                         DateTime time23 = DateTime.Parse(restaurantAppointmentvm.From32);
                         restaurantAppointment23.OpeningTime = time23.TimeOfDay;
                         DateTime ctime23 = DateTime.Parse(restaurantAppointmentvm.to32);
                         restaurantAppointment23.CloseTime = ctime23.TimeOfDay;
                     }
                restaurantAppointment23.IsActive = true;
                db.RestaurantAppointments.Add(restaurantAppointment23);





               
                    RestaurantAppointment restaurantAppointment4 = new RestaurantAppointment();
                    restaurantAppointment4.RestaurantID = restaurantAppointmentvm.RestaurantID;
                    restaurantAppointment4.Day = 4;
                   if (restaurantAppointmentvm.From4 != null && restaurantAppointmentvm.to4 != null)
                   {
                       DateTime time4 = DateTime.Parse(restaurantAppointmentvm.From4);
                       restaurantAppointment4.OpeningTime = time4.TimeOfDay;
                       DateTime ctime4 = DateTime.Parse(restaurantAppointmentvm.to4);
                       restaurantAppointment4.CloseTime = ctime4.TimeOfDay;
                   }
                restaurantAppointment4.IsActive = true;
                db.RestaurantAppointments.Add(restaurantAppointment4);


                
                    RestaurantAppointment restaurantAppointment42 = new RestaurantAppointment();
                    restaurantAppointment42.RestaurantID = restaurantAppointmentvm.RestaurantID;
                    restaurantAppointment42.Day = 4;
                    if (restaurantAppointmentvm.From42 != null && restaurantAppointmentvm.to42 != null)
                    {
                        DateTime time42 = DateTime.Parse(restaurantAppointmentvm.From42);
                        restaurantAppointment42.OpeningTime = time42.TimeOfDay;
                        DateTime ctime42 = DateTime.Parse(restaurantAppointmentvm.to42);
                        restaurantAppointment42.CloseTime = ctime42.TimeOfDay;
                    }
                restaurantAppointment42.IsActive = true;
                db.RestaurantAppointments.Add(restaurantAppointment42);



                
                    RestaurantAppointment restaurantAppointment5 = new RestaurantAppointment();
                    restaurantAppointment5.RestaurantID = restaurantAppointmentvm.RestaurantID;
                    restaurantAppointment5.Day = 5;
                    if (restaurantAppointmentvm.From5 != null && restaurantAppointmentvm.to5 != null)
                    {
                        DateTime time5 = DateTime.Parse(restaurantAppointmentvm.From5);
                        restaurantAppointment5.OpeningTime = time5.TimeOfDay;
                        DateTime ctime5 = DateTime.Parse(restaurantAppointmentvm.to5);
                        restaurantAppointment5.CloseTime = ctime5.TimeOfDay;
                    }
                restaurantAppointment5.IsActive = true;
                db.RestaurantAppointments.Add(restaurantAppointment5);
               
               
                    RestaurantAppointment restaurantAppointment52 = new RestaurantAppointment();
                    restaurantAppointment52.RestaurantID = restaurantAppointmentvm.RestaurantID;
                    restaurantAppointment52.Day = 5;
                    if (restaurantAppointmentvm.From52 != null && restaurantAppointmentvm.to52 != null)
                    {
                        DateTime time52 = DateTime.Parse(restaurantAppointmentvm.From52);
                        restaurantAppointment52.OpeningTime = time52.TimeOfDay;
                        DateTime ctime52 = DateTime.Parse(restaurantAppointmentvm.to52);
                        restaurantAppointment52.CloseTime = ctime52.TimeOfDay;
                    }
                restaurantAppointment52.IsActive = true;
                db.RestaurantAppointments.Add(restaurantAppointment52);



               
                    RestaurantAppointment restaurantAppointment6 = new RestaurantAppointment();
                    restaurantAppointment6.RestaurantID = restaurantAppointmentvm.RestaurantID;
                    restaurantAppointment6.Day = 6;
                    if (restaurantAppointmentvm.From6 != null && restaurantAppointmentvm.to6 != null)
                    {
                        DateTime time6 = DateTime.Parse(restaurantAppointmentvm.From6);
                        restaurantAppointment6.OpeningTime = time6.TimeOfDay;
                        DateTime ctime6 = DateTime.Parse(restaurantAppointmentvm.to6);
                        restaurantAppointment6.CloseTime = ctime6.TimeOfDay;
                    }
                restaurantAppointment6.IsActive = true;
                db.RestaurantAppointments.Add(restaurantAppointment6);
               

               
                RestaurantAppointment restaurantAppointment62 = new RestaurantAppointment();
                restaurantAppointment62.RestaurantID = restaurantAppointmentvm.RestaurantID;
                restaurantAppointment62.Day = 6;
                if (restaurantAppointmentvm.From62 != null && restaurantAppointmentvm.to62 != null)
                {
                    DateTime time62 = DateTime.Parse(restaurantAppointmentvm.From62);
                    restaurantAppointment62.OpeningTime = time62.TimeOfDay;
                    DateTime ctime62 = DateTime.Parse(restaurantAppointmentvm.to62);
                    restaurantAppointment62.CloseTime = ctime62.TimeOfDay;
                }
                restaurantAppointment62.IsActive = true;
                db.RestaurantAppointments.Add(restaurantAppointment62);




                
                    RestaurantAppointment restaurantAppointment7 = new RestaurantAppointment();
                    restaurantAppointment7.RestaurantID = restaurantAppointmentvm.RestaurantID;
                    restaurantAppointment7.Day = 7;
                    if (restaurantAppointmentvm.From7 != null && restaurantAppointmentvm.to7 != null)
                    {
                        DateTime time7 = DateTime.Parse(restaurantAppointmentvm.From7);
                        restaurantAppointment7.OpeningTime = time7.TimeOfDay;
                        DateTime ctime7 = DateTime.Parse(restaurantAppointmentvm.to7);
                        restaurantAppointment7.CloseTime = ctime7.TimeOfDay;
                    }
                restaurantAppointment7.IsActive = true;
                db.RestaurantAppointments.Add(restaurantAppointment7);


                
                    RestaurantAppointment restaurantAppointment72 = new RestaurantAppointment();
                    restaurantAppointment72.RestaurantID = restaurantAppointmentvm.RestaurantID;
                    restaurantAppointment72.Day = 7;
                    if (restaurantAppointmentvm.From72 != null && restaurantAppointmentvm.to72 != null)
                    {
                        DateTime time72 = DateTime.Parse(restaurantAppointmentvm.From72);
                        restaurantAppointment72.OpeningTime = time72.TimeOfDay;
                        DateTime ctime72 = DateTime.Parse(restaurantAppointmentvm.to72);
                        restaurantAppointment72.CloseTime = ctime72.TimeOfDay;
                    }
                restaurantAppointment72.IsActive = true;
                db.RestaurantAppointments.Add(restaurantAppointment72);
               




                db.SaveChanges();
                ViewBag.Msg = 3;
                return RedirectToAction("Index", new { Msg = 3 });
            }

            string usrID = User.Identity.GetUserId();
            string roleName = dbadmin.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;

            ViewBag.RestaurantID = (roleName == "Restaurant") ? new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1 && x.UserID == usrID), "ID", "RestaurantName", restaurantAppointmentvm.RestaurantID) : new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1), "ID", "RestaurantName", restaurantAppointmentvm.RestaurantID);

            return View(restaurantAppointmentvm);
        }

        public ActionResult Edit(int? id)
        {
            //id ==> RestaurantID
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<RestaurantAppointment> restaurantappointmentList = db.RestaurantAppointments.Where(x => x.IsDelete == false && x.RestaurantID == id).ToList();

            if (restaurantappointmentList == null)
            {
                return HttpNotFound();
            }
            RestaurantAppointmentVM restaurantAppointmentVM = new RestaurantAppointmentVM();
            restaurantAppointmentVM.RestaurantID = restaurantappointmentList[0].RestaurantID;
            restaurantAppointmentVM.IsDelete = restaurantappointmentList[0].IsDelete;

            // TimeSpan timespan = new TimeSpan(restaurantappointmentList[0].OpeningTime.Hours, int.Parse(restaurantappointmentList[0].OpeningTime.ToString().Substring(3, 4)), 00);
            //DateTime time = DateTime.Today.Add(restaurantappointmentList[0].OpeningTime);
            //string displayTime = time.ToString("hh:mm tt");
            if (restaurantappointmentList.Count > 1)
            {
                restaurantAppointmentVM.ID1 = restaurantappointmentList[1].ID;
                restaurantAppointmentVM.Saturday = restaurantappointmentList[1].Day;
                if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Saturday).ToList().Count > 0)
                {
                    if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Saturday).ToList()[0] != null)
                    {
                        
                        restaurantAppointmentVM.From1 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Saturday).ToList()[0].OpeningTime).ToString("hh:mm tt");
                        restaurantAppointmentVM.to1 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Saturday).ToList()[0].CloseTime).ToString("hh:mm tt");
                        if (restaurantAppointmentVM.From1 == restaurantAppointmentVM.to1)
                        {
                            restaurantAppointmentVM.From1 = restaurantAppointmentVM.to1 = string.Empty;
                        }
                    }
                }

                if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Saturday).ToList().Count > 1)
                {
                    if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Saturday).ToList()[1] != null)
                    {
                        restaurantAppointmentVM.From12 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Saturday).ToList()[1].OpeningTime).ToString("hh:mm tt");
                        restaurantAppointmentVM.to12 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Saturday).ToList()[1].CloseTime).ToString("hh:mm tt");
                        if (restaurantAppointmentVM.From12 == restaurantAppointmentVM.to12)
                        {
                            restaurantAppointmentVM.From12 = restaurantAppointmentVM.to12 = string.Empty;
                        }
                    }
                }
            }



            if (restaurantappointmentList.Count > 2)
            {
                restaurantAppointmentVM.ID2 = restaurantappointmentList[3].ID;
                restaurantAppointmentVM.Sunday = restaurantappointmentList[3].Day;
                if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Sunday).ToList().Count > 0)
                {
                    if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Sunday).ToList()[0] != null)
                    {
                        restaurantAppointmentVM.From2 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Sunday).ToList()[0].OpeningTime).ToString("hh:mm tt");
                        restaurantAppointmentVM.to2 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Sunday).ToList()[0].CloseTime).ToString("hh:mm tt");
                        if (restaurantAppointmentVM.From2 == restaurantAppointmentVM.to2)
                        {
                            restaurantAppointmentVM.From2 = restaurantAppointmentVM.to2 = string.Empty;
                        }
                    }
                }

                if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Sunday).ToList().Count > 1)
                {
                    if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Sunday).ToList()[1] != null)
                    {
                        restaurantAppointmentVM.From22 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Sunday).ToList()[1].OpeningTime).ToString("hh:mm tt");
                        restaurantAppointmentVM.to22 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Sunday).ToList()[1].CloseTime).ToString("hh:mm tt");
                        if (restaurantAppointmentVM.From22 == restaurantAppointmentVM.to22)
                        {
                            restaurantAppointmentVM.From22 = restaurantAppointmentVM.to22 = string.Empty;
                        }
                    }
                }
            }
            //restaurantAppointmentVM.From2 = restaurantappointmentList[1].OpeningTime.ToString();
            //restaurantAppointmentVM.to2 = restaurantappointmentList[1].CloseTime.ToString();
            if (restaurantappointmentList.Count > 3)
            {
                restaurantAppointmentVM.ID3 = restaurantappointmentList[5].ID;
                restaurantAppointmentVM.Monday = restaurantappointmentList[5].Day;

                if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Monday).ToList().Count > 0)
                {
                    if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Monday).ToList()[0] != null)
                    {
                        restaurantAppointmentVM.From3 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Monday).ToList()[0].OpeningTime).ToString("hh:mm tt");
                        restaurantAppointmentVM.to3 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Monday).ToList()[0].CloseTime).ToString("hh:mm tt");
                        if (restaurantAppointmentVM.From3 == restaurantAppointmentVM.to3)
                        {
                            restaurantAppointmentVM.From3 = restaurantAppointmentVM.to3 = string.Empty;
                        }
                    }
                }

                if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Monday).ToList().Count > 1)
                {
                    if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Monday).ToList()[1] != null)
                    {
                        restaurantAppointmentVM.From32 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Monday).ToList()[1].OpeningTime).ToString("hh:mm tt");
                        restaurantAppointmentVM.to32 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Monday).ToList()[1].CloseTime).ToString("hh:mm tt");
                        if (restaurantAppointmentVM.From32 == restaurantAppointmentVM.to32)
                        {
                            restaurantAppointmentVM.From32 = restaurantAppointmentVM.to32 = string.Empty;
                        }
                    }
                }
            }
            //restaurantAppointmentVM.From3 = restaurantappointmentList[2].OpeningTime.ToString();
            //restaurantAppointmentVM.to3 = restaurantappointmentList[2].CloseTime.ToString();
            if (restaurantappointmentList.Count > 4)
            {
                restaurantAppointmentVM.ID4 = restaurantappointmentList[7].ID;
                restaurantAppointmentVM.Tuesday = restaurantappointmentList[7].Day;

                if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Tuesday).ToList().Count > 0)
                {
                    if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Tuesday).ToList()[0] != null)
                    {
                        restaurantAppointmentVM.From4 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Tuesday).ToList()[0].OpeningTime).ToString("hh:mm tt");
                        restaurantAppointmentVM.to4 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Tuesday).ToList()[0].CloseTime).ToString("hh:mm tt");
                        if (restaurantAppointmentVM.From4 == restaurantAppointmentVM.to4)
                        {
                            restaurantAppointmentVM.From4 = restaurantAppointmentVM.to4 = string.Empty;
                        }
                    }
                }


                if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Tuesday).ToList().Count > 1)
                {
                    if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Tuesday).ToList()[1] != null)
                    {
                        restaurantAppointmentVM.From42 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Tuesday).ToList()[1].OpeningTime).ToString("hh:mm tt");
                        restaurantAppointmentVM.to42 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Tuesday).ToList()[1].CloseTime).ToString("hh:mm tt");
                        if (restaurantAppointmentVM.From42 == restaurantAppointmentVM.to42)
                        {
                            restaurantAppointmentVM.From42 = restaurantAppointmentVM.to42 = string.Empty;
                        }
                    }
                }
            }
            //restaurantAppointmentVM.From4 = restaurantappointmentList[3].OpeningTime.ToString();
            //restaurantAppointmentVM.to4 = restaurantappointmentList[3].CloseTime.ToString();
            if(restaurantappointmentList.Count > 5)
            {
                restaurantAppointmentVM.ID5 = restaurantappointmentList[9].ID;
                restaurantAppointmentVM.Wednesday = restaurantappointmentList[9].Day;

                if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Wednesday).ToList().Count > 0)
                {
                    if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Wednesday).ToList()[0] != null)
                    {
                        restaurantAppointmentVM.From5 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Wednesday).ToList()[0].OpeningTime).ToString("hh:mm tt");
                        restaurantAppointmentVM.to5 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Wednesday).ToList()[0].CloseTime).ToString("hh:mm tt");
                        if (restaurantAppointmentVM.From5 == restaurantAppointmentVM.to5)
                        {
                            restaurantAppointmentVM.From5 = restaurantAppointmentVM.to5 = string.Empty;
                        }
                    }
                }

                if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Wednesday).ToList().Count > 1)
                {
                    if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Wednesday).ToList()[1] != null)
                    {
                        restaurantAppointmentVM.From52 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Wednesday).ToList()[1].OpeningTime).ToString("hh:mm tt");
                        restaurantAppointmentVM.to52 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Wednesday).ToList()[1].CloseTime).ToString("hh:mm tt");
                        if (restaurantAppointmentVM.From52 == restaurantAppointmentVM.to52)
                        {
                            restaurantAppointmentVM.From52 = restaurantAppointmentVM.to52 = string.Empty;
                        }
                    }
                }
            }



            //restaurantAppointmentVM.From5= restaurantappointmentList[4].OpeningTime.ToString();
            //restaurantAppointmentVM.to5 = restaurantappointmentList[4].CloseTime.ToString();
            if (restaurantappointmentList.Count > 6)
            {
                restaurantAppointmentVM.ID6 = restaurantappointmentList[11].ID;
                restaurantAppointmentVM.Thursday = restaurantappointmentList[11].Day;

                if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Thursday).ToList().Count > 0)
                {
                    if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Thursday).ToList()[0] != null)
                    {
                        restaurantAppointmentVM.From6 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Thursday).ToList()[0].OpeningTime).ToString("hh:mm tt");
                        restaurantAppointmentVM.to6 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Thursday).ToList()[0].CloseTime).ToString("hh:mm tt");
                        if (restaurantAppointmentVM.From6 == restaurantAppointmentVM.to6)
                        {
                            restaurantAppointmentVM.From6 = restaurantAppointmentVM.to6 = string.Empty;
                        }
                    }
                }

                if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Thursday).ToList().Count > 1)
                {
                    if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Thursday).ToList()[1] != null)
                    {
                        restaurantAppointmentVM.From62 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Thursday).ToList()[1].OpeningTime).ToString("hh:mm tt");
                        restaurantAppointmentVM.to62 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Thursday).ToList()[1].CloseTime).ToString("hh:mm tt");
                        if (restaurantAppointmentVM.From62 == restaurantAppointmentVM.to62)
                        {
                            restaurantAppointmentVM.From62 = restaurantAppointmentVM.to62 = string.Empty;
                        }
                    }
                }

            }
            //restaurantAppointmentVM.From6 = restaurantappointmentList[5].OpeningTime.ToString();
            //restaurantAppointmentVM.to6 = restaurantappointmentList[5].CloseTime.ToString();
            if (restaurantappointmentList.Count > 7)
            {
                restaurantAppointmentVM.ID7 = restaurantappointmentList[13].ID;
                restaurantAppointmentVM.Friday = restaurantappointmentList[13].Day;
                if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Friday).ToList().Count > 0)
                {
                    if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Friday).ToList()[0] != null)
                    {
                        restaurantAppointmentVM.From7 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Friday).ToList()[0].OpeningTime).ToString("hh:mm tt");
                        restaurantAppointmentVM.to7 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Friday).ToList()[0].CloseTime).ToString("hh:mm tt");
                        if (restaurantAppointmentVM.From7 == restaurantAppointmentVM.to7)
                        {
                            restaurantAppointmentVM.From7 = restaurantAppointmentVM.to7 = string.Empty;
                        }
                    }
                }

                if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Friday).ToList().Count > 1)
                {
                    if (restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Friday).ToList()[1] != null)
                    {
                        restaurantAppointmentVM.From72 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Friday).ToList()[1].OpeningTime).ToString("hh:mm tt");
                        restaurantAppointmentVM.to72 = DateTime.Today.Add(restaurantappointmentList.Where(x => x.Day == (int)Common.WeekDays.Friday).ToList()[1].CloseTime).ToString("hh:mm tt");
                        if (restaurantAppointmentVM.From72 == restaurantAppointmentVM.to72)
                        {
                            restaurantAppointmentVM.From72 = restaurantAppointmentVM.to72 = string.Empty;
                        }
                    }
                }
            }
            //restaurantAppointmentVM.From7 = restaurantappointmentList[6].OpeningTime.ToString();
            //restaurantAppointmentVM.to7 = restaurantappointmentList[6].CloseTime.ToString();
            

            string usrID = User.Identity.GetUserId();
            string roleName = dbadmin.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;

            ViewBag.RestaurantID = (roleName == "Restaurant") ? new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1 && x.UserID == usrID), "ID", "RestaurantName",id ) : new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1), "ID", "RestaurantName", id);

            return View(restaurantAppointmentVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RestaurantAppointmentVM restaurantAppointmentVM)
        {
            if (ModelState.IsValid)
            {

                List<RestaurantAppointment> restaurantAppointment1 = db.RestaurantAppointments.Where(x=>x.RestaurantID == restaurantAppointmentVM.RestaurantID &&x.Day == restaurantAppointmentVM.Saturday).ToList();
               
                if (restaurantAppointmentVM.From1 != null && restaurantAppointmentVM.to1 != null && restaurantAppointment1.Count>0)
                {
                    restaurantAppointment1[0].RestaurantID = restaurantAppointmentVM.RestaurantID;
                    restaurantAppointment1[0].Day = restaurantAppointmentVM.Saturday;
                    if (restaurantAppointmentVM.From1 != null && restaurantAppointmentVM.to1 != null)
                    {
                        restaurantAppointment1[0].OpeningTime = DateTime.Parse(restaurantAppointmentVM.From1).TimeOfDay;
                        restaurantAppointment1[0].CloseTime = DateTime.Parse(restaurantAppointmentVM.to1).TimeOfDay;
                    }
                   
                    db.Entry(restaurantAppointment1[0]).State = EntityState.Modified;
                }  
                //else if (restaurantAppointmentVM.From1 != null && restaurantAppointmentVM.to1 != null && restaurantAppointment1.Count==0)
                //{
                //    RestaurantAppointment addRestaurantAppointment = new RestaurantAppointment();
                //    addRestaurantAppointment.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //    addRestaurantAppointment.Day = restaurantAppointmentVM.Saturday;
                //    if (restaurantAppointmentVM.From1 != null && restaurantAppointmentVM.to1 != null)
                //    {
                //        addRestaurantAppointment.OpeningTime = DateTime.Parse(restaurantAppointmentVM.From1).TimeOfDay;
                //        addRestaurantAppointment.CloseTime = DateTime.Parse(restaurantAppointmentVM.to1).TimeOfDay;
                //    }
                    
                //    db.RestaurantAppointments.Add(addRestaurantAppointment);
                //}

                if (restaurantAppointmentVM.From12 != null && restaurantAppointmentVM.to12 != null && restaurantAppointment1.Count > 1)
                {
                    restaurantAppointment1[1].RestaurantID = restaurantAppointmentVM.RestaurantID;
                    restaurantAppointment1[1].Day = restaurantAppointmentVM.Saturday;
                    if (restaurantAppointmentVM.From12 != null && restaurantAppointmentVM.to12 != null)
                    {
                        restaurantAppointment1[1].OpeningTime = DateTime.Parse(restaurantAppointmentVM.From12).TimeOfDay;
                        restaurantAppointment1[1].CloseTime = DateTime.Parse(restaurantAppointmentVM.to12).TimeOfDay;
                    }
                    
                    db.Entry(restaurantAppointment1[1]).State = EntityState.Modified;
                }
                //else if (restaurantAppointmentVM.From12 != null && restaurantAppointmentVM.to12 != null && restaurantAppointment1.Count == 1)
                //{
                //    RestaurantAppointment addRestaurantAppointment = new RestaurantAppointment();
                //    addRestaurantAppointment.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //    addRestaurantAppointment.Day = restaurantAppointmentVM.Saturday;
                //    if (restaurantAppointmentVM.From12 != null && restaurantAppointmentVM.to12 != null)
                //    {
                //        addRestaurantAppointment.OpeningTime = DateTime.Parse(restaurantAppointmentVM.From12).TimeOfDay;
                //        addRestaurantAppointment.CloseTime = DateTime.Parse(restaurantAppointmentVM.to12).TimeOfDay;
                //    }
                       
                //    db.RestaurantAppointments.Add(addRestaurantAppointment);
                //}


                List<RestaurantAppointment> restaurantAppointment2 = db.RestaurantAppointments.Where(x => x.RestaurantID == restaurantAppointmentVM.RestaurantID && x.Day == restaurantAppointmentVM.Sunday).ToList();

                if (restaurantAppointmentVM.From2 != null && restaurantAppointmentVM.to2 != null &&  restaurantAppointment2.Count > 0)
                {
                    restaurantAppointment2[0].RestaurantID = restaurantAppointmentVM.RestaurantID;
                    restaurantAppointment2[0].Day = restaurantAppointmentVM.Sunday;
                    if (restaurantAppointmentVM.From2 != null && restaurantAppointmentVM.to2 != null)
                    {
                        restaurantAppointment2[0].OpeningTime = DateTime.Parse(restaurantAppointmentVM.From2).TimeOfDay;
                        restaurantAppointment2[0].CloseTime = DateTime.Parse(restaurantAppointmentVM.to2).TimeOfDay;
                    }
                        
                    db.Entry(restaurantAppointment2[0]).State = EntityState.Modified;
                }
                //else if (restaurantAppointmentVM.From2 != null && restaurantAppointmentVM.to2 != null && restaurantAppointment2.Count == 0)
                //{
                //    RestaurantAppointment addRestaurantAppointment = new RestaurantAppointment();
                //    addRestaurantAppointment.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //    addRestaurantAppointment.Day = restaurantAppointmentVM.Sunday;
                //    if (restaurantAppointmentVM.From2 != null && restaurantAppointmentVM.to2 != null)
                //    {
                //        addRestaurantAppointment.OpeningTime = DateTime.Parse(restaurantAppointmentVM.From2).TimeOfDay;
                //        addRestaurantAppointment.CloseTime = DateTime.Parse(restaurantAppointmentVM.to2).TimeOfDay;
                //    }
                      
                //    db.RestaurantAppointments.Add(addRestaurantAppointment);
                //}

                if (restaurantAppointmentVM.From22 != null && restaurantAppointmentVM.to22 != null && restaurantAppointment2.Count > 1)
                {
                    restaurantAppointment2[1].RestaurantID = restaurantAppointmentVM.RestaurantID;
                    restaurantAppointment2[1].Day = restaurantAppointmentVM.Sunday;
                    if (restaurantAppointmentVM.From22 != null && restaurantAppointmentVM.to22 != null)
                    {
                        restaurantAppointment2[1].OpeningTime = DateTime.Parse(restaurantAppointmentVM.From22).TimeOfDay;
                        restaurantAppointment2[1].CloseTime = DateTime.Parse(restaurantAppointmentVM.to22).TimeOfDay;
                    }
                        
                    db.Entry(restaurantAppointment2[1]).State = EntityState.Modified;
                }
                //else if (restaurantAppointmentVM.From22 != null && restaurantAppointmentVM.to22 != null && restaurantAppointment2.Count == 1)
                //{
                //    RestaurantAppointment addRestaurantAppointment = new RestaurantAppointment();
                //    addRestaurantAppointment.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //    addRestaurantAppointment.Day = restaurantAppointmentVM.Sunday;
                //    if (restaurantAppointmentVM.From22 != null && restaurantAppointmentVM.to22 != null)
                //    {
                //        addRestaurantAppointment.OpeningTime = DateTime.Parse(restaurantAppointmentVM.From22).TimeOfDay;
                //        addRestaurantAppointment.CloseTime = DateTime.Parse(restaurantAppointmentVM.to22).TimeOfDay;
                //    }
                       
                //    db.RestaurantAppointments.Add(addRestaurantAppointment);
                //}


                List<RestaurantAppointment> restaurantAppointment3 = db.RestaurantAppointments.Where(x => x.RestaurantID == restaurantAppointmentVM.RestaurantID && x.Day == restaurantAppointmentVM.Monday).ToList();

                if (restaurantAppointmentVM.From3 != null && restaurantAppointmentVM.to3 != null && restaurantAppointment3.Count > 0)
                {
                    restaurantAppointment3[0].RestaurantID = restaurantAppointmentVM.RestaurantID;
                    restaurantAppointment3[0].Day = restaurantAppointmentVM.Monday;
                    if (restaurantAppointmentVM.From3 != null && restaurantAppointmentVM.to3 != null)
                    {
                        restaurantAppointment3[0].OpeningTime = DateTime.Parse(restaurantAppointmentVM.From3).TimeOfDay;
                        restaurantAppointment3[0].CloseTime = DateTime.Parse(restaurantAppointmentVM.to3).TimeOfDay;
                    }
                        
                    db.Entry(restaurantAppointment3[0]).State = EntityState.Modified;
                }
                //else if (restaurantAppointmentVM.From3 != null && restaurantAppointmentVM.to3 != null && restaurantAppointment3.Count == 0)
                //{
                //    RestaurantAppointment addRestaurantAppointment = new RestaurantAppointment();
                //    addRestaurantAppointment.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //    addRestaurantAppointment.Day = restaurantAppointmentVM.Monday;
                //    if (restaurantAppointmentVM.From3 != null && restaurantAppointmentVM.to3 != null)
                //    {
                //        addRestaurantAppointment.OpeningTime = DateTime.Parse(restaurantAppointmentVM.From3).TimeOfDay;
                //        addRestaurantAppointment.CloseTime = DateTime.Parse(restaurantAppointmentVM.to3).TimeOfDay;
                //    }
                //    db.RestaurantAppointments.Add(addRestaurantAppointment);
                //}

                if (restaurantAppointmentVM.From32 != null && restaurantAppointmentVM.to32 != null && restaurantAppointment3.Count > 1)
                {
                    restaurantAppointment3[1].RestaurantID = restaurantAppointmentVM.RestaurantID;
                    restaurantAppointment3[1].Day = restaurantAppointmentVM.Monday;
                    if (restaurantAppointmentVM.From32 != null && restaurantAppointmentVM.to32 != null)
                    {
                        restaurantAppointment3[1].OpeningTime = DateTime.Parse(restaurantAppointmentVM.From32).TimeOfDay;
                        restaurantAppointment3[1].CloseTime = DateTime.Parse(restaurantAppointmentVM.to32).TimeOfDay;
                    }
                    db.Entry(restaurantAppointment3[1]).State = EntityState.Modified;
                }
                //else if (restaurantAppointmentVM.From32 != null && restaurantAppointmentVM.to32 != null && restaurantAppointment3.Count == 1)
                //{
                //    RestaurantAppointment addRestaurantAppointment = new RestaurantAppointment();
                //    addRestaurantAppointment.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //    addRestaurantAppointment.Day = restaurantAppointmentVM.Monday;
                //    if (restaurantAppointmentVM.From32 != null && restaurantAppointmentVM.to32 != null)
                //    {
                //        addRestaurantAppointment.OpeningTime = DateTime.Parse(restaurantAppointmentVM.From32).TimeOfDay;
                //        addRestaurantAppointment.CloseTime = DateTime.Parse(restaurantAppointmentVM.to32).TimeOfDay;
                //    }
                       
                //    db.RestaurantAppointments.Add(addRestaurantAppointment);
                //}


                List<RestaurantAppointment> restaurantAppointment4 = db.RestaurantAppointments.Where(x => x.RestaurantID == restaurantAppointmentVM.RestaurantID && x.Day == restaurantAppointmentVM.Tuesday).ToList();

                if (restaurantAppointmentVM.From4 != null && restaurantAppointmentVM.to4 != null && restaurantAppointment4.Count > 0)
                {
                    restaurantAppointment4[0].RestaurantID = restaurantAppointmentVM.RestaurantID;
                    restaurantAppointment4[0].Day = restaurantAppointmentVM.Tuesday;
                    if (restaurantAppointmentVM.From4 != null && restaurantAppointmentVM.to4 != null)
                    {
                        restaurantAppointment4[0].OpeningTime = DateTime.Parse(restaurantAppointmentVM.From4).TimeOfDay;
                        restaurantAppointment4[0].CloseTime = DateTime.Parse(restaurantAppointmentVM.to4).TimeOfDay;
                    }
                       
                    db.Entry(restaurantAppointment4[0]).State = EntityState.Modified;
                }
                //else if (restaurantAppointmentVM.From4 != null && restaurantAppointmentVM.to4 != null && restaurantAppointment4.Count == 0)
                //{
                //    RestaurantAppointment addRestaurantAppointment = new RestaurantAppointment();
                //    addRestaurantAppointment.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //    addRestaurantAppointment.Day = restaurantAppointmentVM.Tuesday;
                //    if (restaurantAppointmentVM.From4 != null && restaurantAppointmentVM.to4 != null)
                //    {
                //        addRestaurantAppointment.OpeningTime = DateTime.Parse(restaurantAppointmentVM.From4).TimeOfDay;
                //        addRestaurantAppointment.CloseTime = DateTime.Parse(restaurantAppointmentVM.to4).TimeOfDay;
                //    }
                        
                //    db.RestaurantAppointments.Add(addRestaurantAppointment);
                //}

                if (restaurantAppointmentVM.From42 != null && restaurantAppointmentVM.to42 != null && restaurantAppointment4.Count > 1)
                {
                    restaurantAppointment4[1].RestaurantID = restaurantAppointmentVM.RestaurantID;
                    restaurantAppointment4[1].Day = restaurantAppointmentVM.Tuesday;
                    if (restaurantAppointmentVM.From42 != null && restaurantAppointmentVM.to42 != null)
                    {
                        restaurantAppointment4[1].OpeningTime = DateTime.Parse(restaurantAppointmentVM.From42).TimeOfDay;
                        restaurantAppointment4[1].CloseTime = DateTime.Parse(restaurantAppointmentVM.to42).TimeOfDay;
                    }
                       
                    db.Entry(restaurantAppointment4[1]).State = EntityState.Modified;
                }
                //else if (restaurantAppointmentVM.From42 != null && restaurantAppointmentVM.to42 != null && restaurantAppointment4.Count == 1)
                //{
                //    RestaurantAppointment addRestaurantAppointment = new RestaurantAppointment();
                //    addRestaurantAppointment.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //    addRestaurantAppointment.Day = restaurantAppointmentVM.Tuesday;
                //    if (restaurantAppointmentVM.From42 != null && restaurantAppointmentVM.to42 != null)
                //    {
                //        addRestaurantAppointment.OpeningTime = DateTime.Parse(restaurantAppointmentVM.From42).TimeOfDay;
                //        addRestaurantAppointment.CloseTime = DateTime.Parse(restaurantAppointmentVM.to42).TimeOfDay;
                //    }
                       
                //    db.RestaurantAppointments.Add(addRestaurantAppointment);
                //}

                List<RestaurantAppointment> restaurantAppointment5 = db.RestaurantAppointments.Where(x => x.RestaurantID == restaurantAppointmentVM.RestaurantID && x.Day == restaurantAppointmentVM.Wednesday).ToList();

                if (restaurantAppointmentVM.From5 != null && restaurantAppointmentVM.to5 != null && restaurantAppointment5.Count > 0)
                {
                    restaurantAppointment5[0].RestaurantID = restaurantAppointmentVM.RestaurantID;
                    restaurantAppointment5[0].Day = restaurantAppointmentVM.Wednesday;
                    if (restaurantAppointmentVM.From5 != null && restaurantAppointmentVM.to5 != null)
                    {
                        restaurantAppointment5[0].OpeningTime = DateTime.Parse(restaurantAppointmentVM.From5).TimeOfDay;
                        restaurantAppointment5[0].CloseTime = DateTime.Parse(restaurantAppointmentVM.to5).TimeOfDay;
                    }
                        
                    db.Entry(restaurantAppointment5[0]).State = EntityState.Modified;
                }
                //else if (restaurantAppointmentVM.From5 != null && restaurantAppointmentVM.to5 != null && restaurantAppointment5.Count == 0)
                //{
                //    RestaurantAppointment addRestaurantAppointment = new RestaurantAppointment();
                //    addRestaurantAppointment.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //    addRestaurantAppointment.Day = restaurantAppointmentVM.Wednesday;
                //    if (restaurantAppointmentVM.From5 != null && restaurantAppointmentVM.to5 != null)
                //    {
                //        addRestaurantAppointment.OpeningTime = DateTime.Parse(restaurantAppointmentVM.From5).TimeOfDay;
                //        addRestaurantAppointment.CloseTime = DateTime.Parse(restaurantAppointmentVM.to5).TimeOfDay;
                //    }
                        
                //    db.RestaurantAppointments.Add(addRestaurantAppointment);
                //}

                if (restaurantAppointmentVM.From52 != null && restaurantAppointmentVM.to52 != null && restaurantAppointment5.Count > 1)
                {
                    restaurantAppointment5[1].RestaurantID = restaurantAppointmentVM.RestaurantID;
                    restaurantAppointment5[1].Day = restaurantAppointmentVM.Wednesday;
                    if (restaurantAppointmentVM.From52 != null && restaurantAppointmentVM.to52 != null)
                    {
                        restaurantAppointment5[1].OpeningTime = DateTime.Parse(restaurantAppointmentVM.From52).TimeOfDay;
                        restaurantAppointment5[1].CloseTime = DateTime.Parse(restaurantAppointmentVM.to52).TimeOfDay;
                    }
                       
                    db.Entry(restaurantAppointment5[1]).State = EntityState.Modified;
                }
                //else if (restaurantAppointmentVM.From52 != null && restaurantAppointmentVM.to52 != null && restaurantAppointment5.Count == 1)
                //{
                //    RestaurantAppointment addRestaurantAppointment = new RestaurantAppointment();
                //    addRestaurantAppointment.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //    addRestaurantAppointment.Day = restaurantAppointmentVM.Wednesday;
                //    if (restaurantAppointmentVM.From52 != null && restaurantAppointmentVM.to52 != null)
                //    {
                //        addRestaurantAppointment.OpeningTime = DateTime.Parse(restaurantAppointmentVM.From52).TimeOfDay;
                //        addRestaurantAppointment.CloseTime = DateTime.Parse(restaurantAppointmentVM.to52).TimeOfDay;
                //    }
                       
                //    db.RestaurantAppointments.Add(addRestaurantAppointment);
                //}
                List<RestaurantAppointment> restaurantAppointment6 = db.RestaurantAppointments.Where(x => x.RestaurantID == restaurantAppointmentVM.RestaurantID && x.Day == restaurantAppointmentVM.Thursday).ToList();

                if (restaurantAppointmentVM.From6 != null && restaurantAppointmentVM.to6 != null && restaurantAppointment6.Count > 0)
                {
                    restaurantAppointment6[0].RestaurantID = restaurantAppointmentVM.RestaurantID;
                    restaurantAppointment6[0].Day = restaurantAppointmentVM.Thursday;
                    if (restaurantAppointmentVM.From6 != null && restaurantAppointmentVM.to6 != null)
                    {
                        restaurantAppointment6[0].OpeningTime = DateTime.Parse(restaurantAppointmentVM.From6).TimeOfDay;
                        restaurantAppointment6[0].CloseTime = DateTime.Parse(restaurantAppointmentVM.to6).TimeOfDay;
                    }
                        
                    db.Entry(restaurantAppointment6[0]).State = EntityState.Modified;
                }
                //else if (restaurantAppointmentVM.From6 != null && restaurantAppointmentVM.to6 != null && restaurantAppointment6.Count == 0)
                //{
                //    RestaurantAppointment addRestaurantAppointment = new RestaurantAppointment();
                //    addRestaurantAppointment.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //    addRestaurantAppointment.Day = restaurantAppointmentVM.Thursday;
                //    if (restaurantAppointmentVM.From6 != null && restaurantAppointmentVM.to6 != null)
                //    {
                //        addRestaurantAppointment.OpeningTime = DateTime.Parse(restaurantAppointmentVM.From6).TimeOfDay;
                //        addRestaurantAppointment.CloseTime = DateTime.Parse(restaurantAppointmentVM.to6).TimeOfDay;
                //    }
                       
                //    db.RestaurantAppointments.Add(addRestaurantAppointment);
                //}

                if (restaurantAppointmentVM.From62 != null && restaurantAppointmentVM.to62 != null && restaurantAppointment6.Count > 1)
                {
                    restaurantAppointment6[1].RestaurantID = restaurantAppointmentVM.RestaurantID;
                    restaurantAppointment6[1].Day = restaurantAppointmentVM.Thursday;
                    if (restaurantAppointmentVM.From62 != null && restaurantAppointmentVM.to62 != null)
                    {
                        restaurantAppointment6[1].OpeningTime = DateTime.Parse(restaurantAppointmentVM.From62).TimeOfDay;
                        restaurantAppointment6[1].CloseTime = DateTime.Parse(restaurantAppointmentVM.to62).TimeOfDay;
                    }
                       
                    db.Entry(restaurantAppointment6[1]).State = EntityState.Modified;
                }
                //else if (restaurantAppointmentVM.From62 != null && restaurantAppointmentVM.to62 != null && restaurantAppointment6.Count == 1)
                //{
                //    RestaurantAppointment addRestaurantAppointment = new RestaurantAppointment();
                //    addRestaurantAppointment.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //    addRestaurantAppointment.Day = restaurantAppointmentVM.Thursday;
                //    if (restaurantAppointmentVM.From62 != null && restaurantAppointmentVM.to62 != null)
                //    {
                //        addRestaurantAppointment.OpeningTime = DateTime.Parse(restaurantAppointmentVM.From62).TimeOfDay;
                //        addRestaurantAppointment.CloseTime = DateTime.Parse(restaurantAppointmentVM.to62).TimeOfDay;
                //    }
                        
                //    db.RestaurantAppointments.Add(addRestaurantAppointment);
                //}

                List<RestaurantAppointment> restaurantAppointment7 = db.RestaurantAppointments.Where(x => x.RestaurantID == restaurantAppointmentVM.RestaurantID && x.Day == restaurantAppointmentVM.Friday).ToList();

                if (restaurantAppointmentVM.From7 != null && restaurantAppointmentVM.to7 != null && restaurantAppointment7.Count > 0)
                {
                    restaurantAppointment7[0].RestaurantID = restaurantAppointmentVM.RestaurantID;
                    restaurantAppointment7[0].Day = restaurantAppointmentVM.Friday;
                    if (restaurantAppointmentVM.From7 != null && restaurantAppointmentVM.to7 != null)
                    {
                        restaurantAppointment7[0].OpeningTime = DateTime.Parse(restaurantAppointmentVM.From7).TimeOfDay;
                        restaurantAppointment7[0].CloseTime = DateTime.Parse(restaurantAppointmentVM.to7).TimeOfDay;
                    }
                        
                    db.Entry(restaurantAppointment7[0]).State = EntityState.Modified;
                }
                //else if (restaurantAppointmentVM.From7 != null && restaurantAppointmentVM.to7 != null && restaurantAppointment6.Count == 0)
                //{
                //    RestaurantAppointment addRestaurantAppointment = new RestaurantAppointment();
                //    addRestaurantAppointment.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //    addRestaurantAppointment.Day = restaurantAppointmentVM.Friday;
                //    if (restaurantAppointmentVM.From7 != null && restaurantAppointmentVM.to7 != null)
                //    {
                //        addRestaurantAppointment.OpeningTime = DateTime.Parse(restaurantAppointmentVM.From7).TimeOfDay;
                //        addRestaurantAppointment.CloseTime = DateTime.Parse(restaurantAppointmentVM.to7).TimeOfDay;
                //    }
                      
                //    db.RestaurantAppointments.Add(addRestaurantAppointment);
                //}

                if (restaurantAppointmentVM.From72 != null && restaurantAppointmentVM.to72 != null && restaurantAppointment6.Count > 1)
                {
                    restaurantAppointment7[1].RestaurantID = restaurantAppointmentVM.RestaurantID;
                    restaurantAppointment7[1].Day = restaurantAppointmentVM.Friday;
                    if (restaurantAppointmentVM.From72 != null && restaurantAppointmentVM.to72 != null)
                    {
                        restaurantAppointment7[1].OpeningTime = DateTime.Parse(restaurantAppointmentVM.From72).TimeOfDay;
                        restaurantAppointment7[1].CloseTime = DateTime.Parse(restaurantAppointmentVM.to72).TimeOfDay;
                    }
                       
                    db.Entry(restaurantAppointment7[1]).State = EntityState.Modified;
                }
                //else if (restaurantAppointmentVM.From72 != null && restaurantAppointmentVM.to72 != null && restaurantAppointment6.Count == 1)
                //{
                //    RestaurantAppointment addRestaurantAppointment = new RestaurantAppointment();
                //    addRestaurantAppointment.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //    addRestaurantAppointment.Day = restaurantAppointmentVM.Friday;
                //    if (restaurantAppointmentVM.From72 != null && restaurantAppointmentVM.to72 != null)
                //    {
                //        addRestaurantAppointment.OpeningTime = DateTime.Parse(restaurantAppointmentVM.From72).TimeOfDay;
                //        addRestaurantAppointment.CloseTime = DateTime.Parse(restaurantAppointmentVM.to72).TimeOfDay;
                //    }
                     
                //    db.RestaurantAppointments.Add(addRestaurantAppointment);
                //}






                //RestaurantAppointment restaurantAppointment2 = db.RestaurantAppointments.Find(restaurantAppointmentVM.ID2);
                //restaurantAppointment2.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //restaurantAppointment2.Day = restaurantAppointmentVM.Sunday;
                //restaurantAppointment2.OpeningTime = TimeSpan.Parse(restaurantAppointmentVM.From2);
                //restaurantAppointment2.CloseTime = TimeSpan.Parse(restaurantAppointmentVM.to2);
                //db.Entry(restaurantAppointment2).State = EntityState.Modified;

                //RestaurantAppointment restaurantAppointment3 = db.RestaurantAppointments.Find(restaurantAppointmentVM.ID3);
                //restaurantAppointment3.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //restaurantAppointment3.Day = restaurantAppointmentVM.Monday;
                //restaurantAppointment3.OpeningTime = TimeSpan.Parse(restaurantAppointmentVM.From3);
                //restaurantAppointment3.CloseTime = TimeSpan.Parse(restaurantAppointmentVM.to3);
                //db.Entry(restaurantAppointment3).State = EntityState.Modified;

                //RestaurantAppointment restaurantAppointment4 = db.RestaurantAppointments.Find(restaurantAppointmentVM.ID4);
                //restaurantAppointment4.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //restaurantAppointment4.Day = restaurantAppointmentVM.Tuesday;
                //restaurantAppointment4.OpeningTime = TimeSpan.Parse(restaurantAppointmentVM.From4);
                //restaurantAppointment4.CloseTime = TimeSpan.Parse(restaurantAppointmentVM.to4);
                //db.Entry(restaurantAppointment4).State = EntityState.Modified;

                //RestaurantAppointment restaurantAppointment5 = db.RestaurantAppointments.Find(restaurantAppointmentVM.ID5);
                //restaurantAppointment5.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //restaurantAppointment5.Day = restaurantAppointmentVM.Wednesday;
                //restaurantAppointment5.OpeningTime = TimeSpan.Parse(restaurantAppointmentVM.From5);
                //restaurantAppointment5.CloseTime = TimeSpan.Parse(restaurantAppointmentVM.to5);
                //db.Entry(restaurantAppointment5).State = EntityState.Modified;

                //RestaurantAppointment restaurantAppointment6 = db.RestaurantAppointments.Find(restaurantAppointmentVM.ID6);
                //restaurantAppointment6.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //restaurantAppointment6.Day = restaurantAppointmentVM.Thursday;
                //restaurantAppointment6.OpeningTime = TimeSpan.Parse(restaurantAppointmentVM.From6);
                //restaurantAppointment6.CloseTime = TimeSpan.Parse(restaurantAppointmentVM.to6);
                //db.Entry(restaurantAppointment6).State = EntityState.Modified;

                //RestaurantAppointment restaurantAppointment7 = db.RestaurantAppointments.Find(restaurantAppointmentVM.ID7);
                //restaurantAppointment7.RestaurantID = restaurantAppointmentVM.RestaurantID;
                //restaurantAppointment7.Day = restaurantAppointmentVM.Friday;
                //restaurantAppointment7.OpeningTime = TimeSpan.Parse(restaurantAppointmentVM.From7);
                //restaurantAppointment7.CloseTime = TimeSpan.Parse(restaurantAppointmentVM.to7);
                //db.Entry(restaurantAppointment7).State = EntityState.Modified;



                db.SaveChanges();
                return RedirectToAction("Index", new { Msg = 4 });
            }
            string usrID = User.Identity.GetUserId();
            string roleName = dbadmin.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;

            ViewBag.RestaurantID = (roleName == "Restaurant") ? new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1 && x.UserID == usrID), "ID", "RestaurantName", restaurantAppointmentVM.RestaurantID) : new SelectList(db.Restaurants.Where(x => x.RestaurantStatus == 1), "ID", "RestaurantName", restaurantAppointmentVM.RestaurantID);

            return View(restaurantAppointmentVM);
        }

    }
}