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
    public class OrderStatusController : Controller
    {
        // GET: OrderStatus
        private TakeawayEntities db = new TakeawayEntities();

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

            var OrderStatusData = db.OrderStatus.Where(a => a.IsDetete == false).ToList().Select(i => new
            {
                OrderStatus_ar = i.OrderStatus_ar,
                OrderStatus_En = i.OrderStatus_En,
                IsDone = i.IsDone == true ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='ChangeDone(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='ChangeDone(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                IsActive = i.IsActive == true ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                Edit = "<a  id='btn_Edit' class='btn default btn-xs green' href='/OrderStatus/Edit/" + i.ID + "'>تعديل</a>",
                Deleted = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' onclick='Delete(" + i.ID + ")'  > حذف</a>"

            }).ToList();
            return Json(new { data = OrderStatusData }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeStatus(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return null;
            }
            OrderStatu orderstatus = db.OrderStatus.Find(id);

            orderstatus.IsActive = orderstatus.IsActive == true ? false : true;
            try
            {
                db.Entry(orderstatus).State = EntityState.Modified;
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

        public JsonResult ChangeDone(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return null;
            }
            OrderStatu orderstatus = db.OrderStatus.Find(id);

            orderstatus.IsDone = orderstatus.IsDone == true ? false : true;
            try
            {
                db.Entry(orderstatus).State = EntityState.Modified;
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

        // GET: Countries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderStatu orderstatus)
        {
            if (ModelState.IsValid)
            {
                orderstatus.IsActive = true;
                db.OrderStatus.Add(orderstatus);
                db.SaveChanges();
                return RedirectToAction("Index", new { Msg = 3 });
            }

            return View(orderstatus);
        }

        // GET: Countries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderStatu orderstatus = db.OrderStatus.Find(id);
            if (orderstatus == null)
            {
                return HttpNotFound();
            }
            return View(orderstatus);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderStatu orderstatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderstatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { Msg = 4 });
            }
            return View(orderstatus);
        }


        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                OrderStatu orderstatus = db.OrderStatus.Find(id);
                orderstatus.IsDetete = true;
                db.Entry(orderstatus).State = EntityState.Modified;
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