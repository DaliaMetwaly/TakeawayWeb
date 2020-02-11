using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Takeaway.Models;
using Microsoft.Reporting.WebForms;
namespace Takeaway.Controllers
{
    public class ReportController : Controller
    {
        TakeawayEntities db = new TakeawayEntities();
        ApplicationDbContext dbMemberShip = new ApplicationDbContext();

        // GET: Report
        [Authorize(Roles = "Admin,SuperAdmin,Restaurant")]
        public ActionResult ActiveCustomersReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ActiveCustomersReport(ActiveCustomersReportVM _model)
        {
            var role = (from r in dbMemberShip.Roles where r.Name.Contains("User") select r).FirstOrDefault();
            var users = dbMemberShip.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).Select(i=>new { UserID=i.Id,UserName=i.UserName}).ToList();
            var Orders = db.Orders.ToList();


            var CustomerList = from o in Orders
                               join u in users
                               on o.DeliveryUserID equals u.UserID
                               select new {u.UserID,u.UserName,o.ID} into x
                               group x by new { x.UserID ,x.UserName} into g
                               orderby g.Count() descending
                               select new {
                                           Name =g.Key.UserName,
                                           NumofOrders=g.Count()
                                          }
                               ;


               _model.customerList = new List<ActiveCustomersReportVMtRow>();

               foreach (var item in CustomerList)
               {
                   ActiveCustomersReportVMtRow _newrow = new ActiveCustomersReportVMtRow();
                   _newrow.CustomerName = item.Name;
                   _newrow.NumofOrders = item.NumofOrders;

                   _model.customerList.Add(_newrow);

               }

            return View(_model);
           
        }


        public ActionResult RestaurantRatesReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RestaurantRatesReport(RestaurantRatesReportVM _model)
        {
           
            var restaurantList = db.RestaurantRates.ToList()
                                .GroupBy(s => new
                                {
                                    s.RestaurantID,
                                    s.Restaurant.RestaurantName,

                                }).Select(i => new
                                {

                                    RestaurantName = i.Key.RestaurantName,
                                    Rate = (i.Sum(y => y.UserRate) / i.Count()) 


                                }).ToList();
            ;


            _model.restaurantList = new List<RestaurantRatesReportVMtRow>();

            foreach (var item in restaurantList)
            {
                RestaurantRatesReportVMtRow _newrow = new RestaurantRatesReportVMtRow();
                _newrow.RestaurantName = item.RestaurantName;
                _newrow.Rate = item.Rate;
                _model.restaurantList.Add(_newrow);

            }

            return View(_model);

        }
    }
}