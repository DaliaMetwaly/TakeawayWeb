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
    public class NotificationController : Controller
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
                var Entity = db.NotificationMsgs.Where(x => x.IsDelete == false ).OrderByDescending(x => x.CreatedDate).ToList();
                var userData = dbadmin.Users.ToList();
                Data = Entity.Select(i => new
                {
                    ID = i.ID,
                    Name = i.message,
                    UserCategory = Common.GetEnumUserCategoryName(i.UserCategory),
                    CreatedDate = "<span  id = 'CreatedDate' class=''>" + Convert.ToDateTime(i.CreatedDate).ToString("dd/MM/yyyy hh:mm") + "</span>",
                             IsActive = i.IsActive == true ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                           
                            Deleted = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' onclick='Delete(" + i.ID + ")'  > حذف</a>",
                           //  Details = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' data-toggle='modal' data-target='#myModal' onclick='GetDetailedUserList(" + i.ID + ")'  > التفاصيل</a>"

                }).ToList();

               
            }
            catch(Exception ex)
            {

            }
            return Json(new { data = Data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDetailedUserList(int id)
        {
            var list = db.NotificationUsers.Where(a =>  a.NotificationID == id).ToList().Select(i => new
            {
                UserName = i.User.AspNetUser.UserName,
                isSend = i.isSend == 1 ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;'class='glyphicon glyphicon-remove'></ span >",

            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

       
        public JsonResult ChangeStatus(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return null;
            }
            NotificationMsg notificationMsg = db.NotificationMsgs.Find(id);

            notificationMsg.IsActive = notificationMsg.IsActive == true ? false : true;
            try
            {
                db.Entry(notificationMsg).State = EntityState.Modified;
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
            ViewBag.UserCategory = GetUserCategory();
            return View();
        }
        SelectList GetUserCategory()
        {
            var enums = new List<SelectListItem>();
            foreach (int value in Enum.GetValues(typeof(UserCategory)))
            {
                var item = new SelectListItem();
                item.Value = value.ToString();
                item.Text = Common.GetEnumUserCategoryName(value);// Enum.GetName(typeof(OfferType), value);

                enums.Add(item);
            }
           
            return new SelectList(enums, "Value", "Text");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NotificationMsg notificationMsg)
        {
            if (ModelState.IsValid)
            {
                notificationMsg.CreatedDate = DateTime.Now;           
                

                var userlist = db.Users.Where(x=> x.UserCategory == notificationMsg.UserCategory && x.IsDetete == false && x.IsActive == true);
                foreach (User item in userlist)
                {
                    NotificationUser itm = new NotificationUser();
                    itm.UserID = item.ID;
                    itm.isSend = 0;
                    notificationMsg.NotificationUsers.Add(itm);
                }
                notificationMsg.IsActive = true;
                db.NotificationMsgs.Add(notificationMsg);
                db.SaveChanges();
                ViewBag.Msg = 3;
                return RedirectToAction("Index", new { Msg = 3 });
            }

            ViewBag.UserCategory = GetUserCategory();
            return View(notificationMsg);
        }
        
        public ActionResult Delete(int ID)
        {
            try
            {
                var model = db.NotificationMsgs.Find(ID);
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