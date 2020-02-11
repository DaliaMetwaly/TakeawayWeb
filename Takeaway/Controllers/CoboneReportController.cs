using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Takeaway.Models;
using Microsoft.Reporting.WebForms;
using Microsoft.AspNet.Identity;

namespace Takeaway.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CoboneReportController : Controller
    {
        TakeawayEntities db = new TakeawayEntities();
        // GET: TodayOrder


        public ActionResult CoboneReport()
        { 
            ViewBag.CoboneID = new SelectList(db.Cobones.Where(c => c.IsDelete == false && c.IsActive == true), "ID", "CoboneNameAr");
            ViewBag.isUsed = getUsedList();
            return View();
        }

        private SelectList getUsedList()
        {
            SelectListItem itm = new SelectListItem();
            List<SelectListItem> itmList = new  List<SelectListItem> ();
            itm.Text = "مستخدم";
            itm.Value = "1";
            SelectListItem itm2 = new SelectListItem();
            itm2.Text = "غير مستخدم";
            itm2.Value = "0";
            itmList.Add(itm);
            itmList.Add(itm2);
            SelectList list = new SelectList(itmList,"Value","Text");
            return list;
        }

        [HttpPost]
        public ActionResult CoboneReport(CoboneReportVM _model,int? CoboneID, string From,string To,string isUsed)
        {
             
           var List = db.CoboneUsers.Where(GetList(CoboneID, From, To)).ToList();
            if (isUsed != "")
            {
               
                if (isUsed=="0")
                {
                    List = List.Where(x => x.isUsed == 0).ToList();
                }
                else
                {
                    List = List.Where(x => x.isUsed == 1).ToList();
                }
                
            }
             
            _model.CoboneList = new List<CoboneReportRow>();
            int number = 0;
            foreach (var item in List)
            {
                CoboneReportRow _newrow = new CoboneReportRow();
                _newrow.CoboneName = item.Cobone.CoboneNameAr;
                _newrow.CoboneSerial = item.CoboneSerial;

                try
                {
                    _newrow.CoboneUserName = item.User.AspNetUser.UserName;
                    _newrow.StartDate = item.StartDate.ToString("dd/MM/yyyy");
                    _newrow.EndDate = item.EndDate.ToString("dd/MM/yyyy");
                    if (item.isUsed == 0)
                    {
                        _newrow.CoboneUserName = "غير مستخدم";
                        _newrow.StartDate = "غير مستخدم";
                        _newrow.EndDate = "غير مستخدم";
                    }
                }
                catch (Exception)
                {
                    _newrow.CoboneUserName = "غير مستخدم";
                    _newrow.StartDate = "غير مستخدم";
                    _newrow.EndDate = "غير مستخدم";
                }
               
                _model.CoboneList.Add(_newrow);
                number++;
            }
            _model.Counts = number;
            ViewBag.CoboneID = new SelectList(db.Cobones.Where(c => c.IsDelete == false && c.IsActive == true), "ID", "CoboneNameAr");
            ViewBag.isUsed = getUsedList();
            return View(_model);
        }

        private Func<CoboneUser, bool> GetList(int? CoboneID, string from, string to)
        {
            
            Func<CoboneUser, bool> expr = p => p.IsDelete == false && p.IsActive == true;
            DateTime DateFrom = new DateTime () ;
            DateTime Dateto = new DateTime ();
            if (from != "" && to != "")
            {
                 DateFrom = DateTime.ParseExact(from, "MM/dd/yyyy", null);
                 Dateto = DateTime.ParseExact(to, "MM/dd/yyyy", null);
                TimeSpan ts = new TimeSpan(23, 59, 0);
                Dateto = Dateto.Date + ts;
            }
            if (CoboneID > 0)
            {
                if (from != "" && to != "")
                {
                    expr = p => p.IsDelete == false && p.IsActive == true && p.CoboneID == CoboneID && (p.CreateDate >= DateFrom && p.CreateDate <= Dateto) ;
                }
                else
                {
                    expr = p => p.IsDelete == false && p.IsActive == true && p.CoboneID == CoboneID;
                }
            }
            else
            {
                if (from != "" && to != "")
                {
                    expr = p => p.IsDelete == false && p.IsActive == true && (p.CreateDate >= DateFrom && p.CreateDate <= Dateto) ;
                }
                else
                {
                    expr = p => p.IsDelete == false && p.IsActive == true ;
                }
            }
           
           
            
            return expr;
        }
    }
}