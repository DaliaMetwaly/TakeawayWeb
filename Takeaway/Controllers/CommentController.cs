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

namespace Takeaway.Controllers
{
    public class CommentController : Controller
    {
        //DM5-7-2017
        //Variables
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
            //DM 5-7-2017
            //Load Comments Data
            var commentsData= (Object)null;
            try
            {
                var commentsEntity = db.UserComments.Where(x => x.IsDetete == false).OrderByDescending(x => x.CreationDate).ToList();
                var userData = dbadmin.Users.ToList();
                commentsData = (                     from c in userData
                                                     join i in commentsEntity on c.Id equals i.UserID
                                                     select new
                                                     {
                                                         ID = i.ID,
                                                         RestaurantName = i.Restaurant.RestaurantName,
                                                         UserName = c.UserName ,
                                                         Comment = i.Comment,
                                                         CreationDate = "<span  id = 'CreationDate' class=''>" + Convert.ToDateTime(i.CreationDate).ToString("dd/MM/yyyy hh:mm") + "</span>",
                                                         IsActive = i.IsActive == true ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                                                          Deleted = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' onclick='Delete(" + i.ID + ")'  > حذف</a>"

                                                     }).ToList();

               
            }
            catch(Exception ex)
            {

            }
            return Json(new { data = commentsData }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeStatus(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return null;
            }
            UserComment userComment = db.UserComments.Find(id);

            userComment.IsActive = userComment.IsActive == true ? false : true;
            try
            {
                db.Entry(userComment).State = EntityState.Modified;
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

        public ActionResult DeleteComment(int ID)
        {
            try
            {
                var model = db.UserComments.Find(ID);
                model.IsDetete = true;

                db.SaveChanges();
                //removemedia(id);
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult CreateComment(int restaurantID, string comment)
        {
            UserComment userCommentobj = new UserComment();
            string userID = User.Identity.GetUserId();
            if (userID != null && userID != "")
            {
                try
                {
                    userCommentobj.UserID = userID;
                    userCommentobj.RestaurantID = restaurantID;
                    userCommentobj.IsActive = false;
                    userCommentobj.CreationDate = DateTime.Now;
                    userCommentobj.Comment = comment;

                    db.UserComments.Add(userCommentobj);

                    db.SaveChanges();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }


            }


            return Json(2, JsonRequestBehavior.AllowGet);
        }
    }
}