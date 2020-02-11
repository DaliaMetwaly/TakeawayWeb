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
    public class CoboneUserController : Controller
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

            var Data = db.CoboneUsers.Where(a => a.IsDelete == false ).ToList().OrderBy(c => c.Cobone.CoboneNameAr).Select(i => new
            {
                i.Cobone.CoboneNameAr,
                UserName = i.UserID != null? i.User.AspNetUser.UserName: "غير مستخدم",
                i.CoboneSerial,
                StartDate = i.isUsed == 1 ? Convert.ToDateTime(i.StartDate).ToString("dd/MM/yyyy") : "غير مستخدم",
                EndDate = i.isUsed == 1 ? Convert.ToDateTime(i.EndDate).ToString("dd/MM/yyyy") : "غير مستخدم",
                isUsed = i.isUsed == 1 ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;'  class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' class='glyphicon glyphicon-remove'></ span >",

                IsActive = i.IsActive == true ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                //Edit = "<a  id='btn_Edit' class='btn default btn-xs green' href='/Cobone/Edit/" + i.ID + "'>تعديل</a>",
                Deleted = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' onclick='Delete(" + i.ID + ")'  > حذف</a>"




            }).ToList();
            return Json(new { data = Data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeStatus(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return null;
            }
            CoboneUser coboneUser = db.CoboneUsers.Find(id);

            coboneUser.IsActive = coboneUser.IsActive == true ? false : true;
            try
            {
                db.Entry(coboneUser).State = EntityState.Modified;
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
           ViewBag.CoboneID =  new SelectList(db.Cobones.Where(c => c.IsActive == true
             && c.IsDelete == false), "ID", "CoboneNameAr");

            return View();
        }

        // POST: CookingCat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CoboneUserVM coboneUser)
        {
            if (ModelState.IsValid)
            {
                List<CoboneUser> cobUserList = new List<CoboneUser>();
                for (int i = 0; i < coboneUser.Count; i++)
                {
                    CoboneUser cobUser = new CoboneUser();
                    cobUser.CoboneID = coboneUser.CoboneID;
                    cobUser.isUsed = 0;
                    //cobUser.UserID = "0777420f-8810-4c99-a1f0-59a701ab0218";
                    //cobUser.IsActive = coboneUser.IsActive;
                    cobUser.CreateDate = DateTime.Now;
                    cobUser.StartDate = DateTime.Now;
                    cobUser.EndDate = DateTime.Now;
                    Random random = new Random();
                    string d = DateTime.Now.ToString("ssfff");
                    string num = random.Next().ToString();
                    cobUser.CoboneSerial ="T"+ d + num;
                    db.CoboneUsers.Add(cobUser);
                    cobUserList.Add(cobUser); 
                }
              
                db.SaveChanges();
                CreateExcelDoc excell_app = new CreateExcelDoc();
                //creates the main header
               string CoboneNameAr = db.Cobones.Find(coboneUser.CoboneID).CoboneNameAr;
                excell_app.createHeaders(1, 1, CoboneNameAr, "A1", "A1", 0, "GRAY", true, 14, "");

                for (int i = 0; i < cobUserList.Count; i++)
                {
                    //add Data to to cells
                    excell_app.addData(i + 2, 1, cobUserList[i].CoboneSerial, "A" + (i+2), "A" + (i + 2), "@");

                }
                return RedirectToAction("Index", new { Msg = 3 });
            }
            ViewBag.CoboneID = new SelectList(db.Cobones.Where(c => c.IsActive == true
            && c.IsDelete == false), "ID", "CoboneNameAr");
            return View(coboneUser);
        }
        
        // GET: package/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoboneUser coboneUser = db.CoboneUsers.Find(id);
            if (coboneUser == null)
            {
                return HttpNotFound();
            }
            return View(coboneUser);
        }

        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                CoboneUser coboneUser = db.CoboneUsers.Find(id);
                coboneUser.IsDelete = true;
                db.Entry(coboneUser).State = EntityState.Modified;
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
