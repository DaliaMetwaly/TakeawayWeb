using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Takeaway.Models;
using Microsoft.Reporting.WebForms;
namespace Takeaway.Controllers
{
    public class RestaurantReportController : Controller
    {
        TakeawayEntities db = new TakeawayEntities();
        // GET: TodayOrder

        [Authorize(Roles = "Admin,SuperAdmin,Restaurant")]
        public ActionResult RestaurantReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RestaurantReport(RestaurantReportVM _model)
        {
            var restaurantList = db.Restaurants.ToList();
            _model.RestaurantList = new List<RestaurantReportRow>();
            int number = 0;
            foreach (var item in restaurantList)
            {
                RestaurantReportRow _newrow = new RestaurantReportRow();
                _newrow.RestaurantName = item.RestaurantName;
                _newrow.RestaurantPhone = item.RestaurantPhone;
                _newrow.RestaurantStatus = item.RestaurantStatus;
                _newrow.RestaurantStatusName = item.RestaurantStatu.RestaurantStatusAr;
                _model.RestaurantList.Add(_newrow);
                number++;
            }
            _model.RestaurantCounts = number;
            return View(_model);
        }

        public ActionResult OrderDetailsReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OrderDetailsReport(OrderDetailsReportVM _model)
        {
            var restaurantList = db.OrderDetails
                                   .Where(x=>x.IsActive==true && x.IsDetete==false)
                                   .ToList()
                                   .Select(a=> new
                                                 {
                                       UserID=a.Order.DeliveryUserID,
                                       UserName=a.Order.User.AspNetUser.UserName,
                                       OrderID=a.ID,
                                       OrderDate=a.Order.OrderDate,
                                       ItemFoodID = a.ItemFoodID,
                                       FoodName=a.ItemFood.FoodName,
                                       ItemPrice=a.ItemPrice,
                                       ItemCount=a.ItemCount,
                                       TotalPrice=a.ItemCount * a.ItemPrice,
                                       RestaurantID=a.Order.RestaurantData.RestaurantID,
                                       RestaurantName=a.Order.RestaurantData.Restaurant.RestaurantName

                                   })
                                   ;
            _model.UserOrderList = new List<OrderDetailsReportRow> ();
          
            foreach (var item in restaurantList)
            {
                OrderDetailsReportRow _newrow = new OrderDetailsReportRow();
                _newrow.RestaurantName = item.RestaurantName;
                _newrow.RestaurantID = item.RestaurantID;
                _newrow.TotalPrice = item.TotalPrice;
                _newrow.ItemCount = item.ItemCount;
                _newrow.OrderDate = item.OrderDate;
                _newrow.ItemPrice = item.ItemPrice;
                _newrow.FoodName = item.FoodName;
                _newrow.ItemFoodID = item.ItemFoodID;
                _newrow.OrderID = item.OrderID;
                _newrow.UserName = item.UserName;
                _newrow.UserID = item.UserID;
               
                _model.UserOrderList.Add(_newrow);
               
            }
           
            return View(_model);
        }
    }
}