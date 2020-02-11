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
    [Authorize(Roles = "Admin,SuperAdmin,Restaurant")]
    public class OrderReportController : Controller
    {
        TakeawayEntities db = new TakeawayEntities();
        // GET: TodayOrder
        private ApplicationDbContext dbMembership = new ApplicationDbContext();

        public ActionResult OrderReport()
        {
            int RestaurantID = 0;
            ViewBag.RestaurantID = new SelectList(db.Restaurants.Where(c => c.RestaurantStatus == 1), "ID", "RestaurantName");
            ViewBag.RegionID = new SelectList( db.RestaurantDatas.Where(x => x.IsActive == true && x.IsDetete == false ).Select(i => new { ID = i.ID, RegionAr = i.Region.RegionAr }), "ID", "RegionAr");
            ViewBag.OrderStatusID = new SelectList(db.OrderStatus.Where(x=> x.IsDetete==false && x.IsActive == true), "ID", "OrderStatus_ar");
            var UsrInRoles = (from s in dbMembership.Users
                              where dbMembership.Roles.Where(x => x.Id == s.Roles.FirstOrDefault().RoleId).FirstOrDefault().Name == "User"
                              select new { s.Id, s.UserName }).ToList();
            var UsersData = from u in db.Users.ToList()
                            join r in UsrInRoles
                            on u.ID equals r.Id
                            where u.IsDetete == false
                            select new
                            {
                                ID = u.ID,
                                UserName = r.UserName,
                                };
            ViewBag.UserID = new SelectList(UsersData, "ID", "UserName");
            
            if (User.IsInRole("Restaurant"))
            {
                try
                {
                    RestaurantID = db.Restaurants.Where(x => x.UserID == User.Identity.GetUserId()).FirstOrDefault().ID;
                 
                }
                catch (Exception)
                {
                    
                }
                ViewBag.RestaurantID = new SelectList(db.Restaurants.Where(c => c.RestaurantStatus == 1 && c.ID == RestaurantID), "ID", "RestaurantName");
            }
         
            return View();
        }

        [HttpPost]
        public ActionResult OrderReport(OrderReportVM _model,int? RestaurantID, int? RegionID, string UserID, int? OrderStatusID, string From,string To)
        {
            var List = db.Orders.Where(GetList(RestaurantID, From, To));//.
            // List = db.Orders.Where(x=> x.RestaurantID== RestaurantID && x.IsDetete == false && x.IsActive == true).ToList();
            if (List != null)
            {
                if (OrderStatusID != null)
                {
                    List = List.Where(x => x.OrderStatusID == OrderStatusID);
                }
                if (UserID != "")
                {
                    List = List.Where(x => x.DeliveryUserID == UserID);
                }
                if (RegionID != null)
                {
                    List = List.Where(x => x.RestaurantDataID == RegionID);
                }
            }
            List = List.ToList();
            _model.OrderList = new List<OrderReportRow>();
            int number = 0;
            foreach (var item in List)
            {
                OrderReportRow _newrow = new OrderReportRow();
                _newrow.DeliveryUserID = item.DeliveryUserID;
                _newrow.DeliveryUserName = item.User.AspNetUser.UserName;
                _newrow.RestaurantName = item.RestaurantData.Restaurant.RestaurantName +" - "+item.RestaurantData.Region.RegionAr;
                _newrow.RestaurantID = item.RestaurantData.RestaurantID;
                _newrow.OrderStatusName = item.OrderStatu.OrderStatus_ar;
                _newrow.TotalPrice = item.TotalPrice;
                _newrow.orderFee = 10;
                _newrow.OrderDate = item.OrderDate;
                _model.OrderList.Add(_newrow);
                number++;
            }
            _model.Counts = number;
            ViewBag.RestaurantID = new SelectList(db.Restaurants.Where(c => c.RestaurantStatus == 1), "ID", "RestaurantName");
            ViewBag.RegionID = new SelectList(db.RestaurantDatas.Where(x => x.IsActive == true && x.IsDetete == false).Select(i => new { ID = i.ID, RegionAr = i.Region.RegionAr }), "ID", "RegionAr");
            ViewBag.OrderStatusID = new SelectList(db.OrderStatus.Where(x => x.IsDetete == false && x.IsActive == true), "ID", "OrderStatus_ar");
            var UsrInRoles = (from s in dbMembership.Users
                              where dbMembership.Roles.Where(x => x.Id == s.Roles.FirstOrDefault().RoleId).FirstOrDefault().Name == "User"
                              select new { s.Id, s.UserName }).ToList();
            var UsersData = from u in db.Users.ToList()
                            join r in UsrInRoles
                            on u.ID equals r.Id
                            where u.IsDetete == false
                            select new
                            {
                                ID = u.ID,
                                UserName = r.UserName,
                            };
            ViewBag.UserID = new SelectList(UsersData, "ID", "UserName");
            return View(_model);
        }

        private Func<Order, bool> GetList(int? RestaurantID, string from, string to)
        {
            
            Func<Order, bool> expr = p => p.IsDetete == false && p.IsActive == true;
            DateTime DateFrom = new DateTime () ;
            DateTime Dateto = new DateTime ();
            if (from != "" && to != "")
            {
                 DateFrom = DateTime.ParseExact(from, "MM/dd/yyyy", null);
                 Dateto = DateTime.ParseExact(to, "MM/dd/yyyy", null);
                TimeSpan ts = new TimeSpan(23, 59, 0);
                Dateto = Dateto.Date + ts;
            }
            if (RestaurantID>0)
            {
                if (from != "" && to != "")
                {
                    expr = p => p.IsDetete == false && p.IsActive == true && p.RestaurantData.RestaurantID == RestaurantID && (p.OrderDate >= DateFrom && p.OrderDate <= Dateto) ;
                }
                else
                {
                    expr = p => p.IsDetete == false && p.IsActive == true && p.RestaurantData.RestaurantID == RestaurantID  ;
                }
            }
            else
            {
                if (from != "" && to != "")
                {
                    expr = p => p.IsDetete == false && p.IsActive == true && (p.OrderDate >= DateFrom && p.OrderDate <= Dateto) ;
                }
                else
                {
                    expr = p => p.IsDetete == false && p.IsActive == true ;
                }
            }
           
           
            
            return expr;
        }

        public ActionResult GetRegion(int? id)
        {
            var Data = (Object)null;
            try
            {
                Data = db.RestaurantDatas.Where(x => x.IsActive == true && x.IsDetete == false && x.RestaurantID == id).Select(i => new { ID = i.ID, Name = i.Region.RegionAr }).ToList();

            }
            catch (Exception ex)
            {

            }
            return Json(new { data = Data }, JsonRequestBehavior.AllowGet);
        }
    }
}