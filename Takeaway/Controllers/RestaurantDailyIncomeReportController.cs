using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Takeaway.Models;
using Microsoft.Reporting.WebForms;
using System.Data.Entity;

namespace Takeaway.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin,Restaurant")]
    public class RestaurantDailyIncomeReportController : Controller
    {
        TakeawayEntities db = new TakeawayEntities();
        // GET: TodayOrder


        public ActionResult RestaurantDailyIncomeReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RestaurantDailyIncomeReport(RestaurantDailyIncomeReportVM _model)
        {
            DateTime currDate = DateTime.Now.Date;
            DateTime orderDate = DateTime.Now.Date;
            var restaurantDailyIncomeList = db.Orders.Where(x=> DbFunctions.TruncateTime(x.OrderDate)== DbFunctions.TruncateTime(DateTime.Now) && x.IsActive==true && x.IsDetete==false).ToList()
                                                    .GroupBy(s => new
                                                    {
                                                        s.RestaurantData.RestaurantID,
                                                        s.RestaurantData.Restaurant.RestaurantName,
                                                       
                                                    }).Select(i => new 
                                                     {
                                                         restaurantID=i.Key.RestaurantID,
                                                         restaurantName=i.Key.RestaurantName,
                                                         totalPrice=i.Sum(y=>y.TotalPrice),
                                                         Income=i.Count()*10
                                                     });
            _model.RestaurantDailyIncomeList = new List<RestaurantDailyIncomeReportRow>();
            int number = 0;
            foreach (var item in restaurantDailyIncomeList)
            {
                RestaurantDailyIncomeReportRow _newrow = new RestaurantDailyIncomeReportRow();
                _newrow.restaurantID = item.restaurantID;
                _newrow.restaurantName = item.restaurantName;
                _newrow.totalPrice = item.totalPrice;
                _newrow.Income = item.Income;
                _model.RestaurantDailyIncomeList.Add(_newrow);
                number++;
            }
          
            return View(_model);
        }
    }
}