using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Takeaway.Models;
using System.Data.Entity;

namespace Takeaway.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin,Restaurant")]
    public class HomeController : Controller
    {
        ApplicationDbContext dbMembership = new ApplicationDbContext();
        TakeawayEntities db = new TakeawayEntities();
        public ActionResult Index()
        {
          //  ViewBag.NewOrderD = db.Orders.Where(o => o.OrderDate <= DateTime.Now).Count();
            //ViewBag.MonthlyIncome = 
            //ViewBag.DailyIncome = 
            return View();
        }

        public ActionResult LoadLatestOffersData()
        {
            string usrID = User.Identity.GetUserId();
            string roleName = dbMembership.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;
            DateTime currentDate = DateTime.Now.Date;
            List<int> RestaurantList = (db.Restaurants.Where(c => c.UserID == usrID).Select(c => c.ID)).ToList();

            var LatestOffers = (roleName == "Restaurant")
                             ?db.Offers
                               .Where(x => x.StartDate >= currentDate                               
                                      && RestaurantList.Contains(int.Parse(x.SubjectID.ToString()))
                                      && x.IsActive == true
                                      && x.IsDelete == false
                                      && x.OfferType==1).ToList()
                               .Select(i => new
                                            {

                                                 RestaurantName=(db.Restaurants.Where(y=>y.ID==i.SubjectID).FirstOrDefault().RestaurantName),
                                                 FeeValues=i.FeeType==2?i.FeeValue+" % ":i.FeeValue+" ريا ل",
                                                 StartDate = "<span  id = 'StartDate' class=''>" + Convert.ToDateTime(i.StartDate).ToString("dd/MM/yyyy hh:mm")+ "</span>",
                                                 EndDate   = "<span  id = 'EndDate' class=''>" + Convert.ToDateTime(i.EndDate).ToString("dd/MM/yyyy hh:mm")+ "</span>"

                               }).ToList()
                               
                             : db.Offers
                               .Where(x => x.StartDate >= currentDate
                                      && x.IsActive == true
                                      && x.IsDelete == false
                                      && x.OfferType == 1).ToList()
                               .Select(i => new
                               {

                                   RestaurantName = (db.Restaurants.Where(y => y.ID == i.SubjectID).FirstOrDefault().RestaurantName),
                                   FeeValues =  i.FeeType == 2 ? i.FeeValue + " % " : i.FeeValue + " ريا ل",
                                   StartDate = "<span  id = 'StartDate' class=''>" + Convert.ToDateTime(i.StartDate).ToString("dd/MM/yyyy hh:mm") + "</span>",
                                   EndDate = "<span  id = 'EndDate' class=''>" + Convert.ToDateTime(i.EndDate).ToString("dd/MM/yyyy hh:mm") + "</span>"

                               }).ToList();

            return Json(new { data = LatestOffers }, JsonRequestBehavior.AllowGet);

        }

      

        public ActionResult LoadRestaurantRatingData()
        {
            string usrID = User.Identity.GetUserId();
            string roleName = dbMembership.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;

         



            var RestaurantRating = (roleName == "Restaurant")
                              ? db.RestaurantRates.ToList().Where(x => x.Restaurant.UserID == usrID).ToList()
                               .GroupBy(s => new
                               {
                                   s.RestaurantID,
                                   s.Restaurant.RestaurantName,

                               }).Select(i => new
                               {

                                   RestaurantName = i.Key.RestaurantName,
                                   Rate = "<span class='rating' data-score='"+(i.Sum(y=>y.UserRate)/i.Count())+"'></span>"

                               }).ToList()
                               : db.RestaurantRates.ToList()
                                .GroupBy(s => new
                                {
                                    s.RestaurantID,
                                    s.Restaurant.RestaurantName,

                                }).Select(i => new
                                {

                                   RestaurantName = i.Key.RestaurantName,
                                   Rate = "<span class='rating' data-score='"+ (i.Sum(y => y.UserRate) / i.Count()) + "'></span>"


                               }).ToList();

            return Json(new { data = RestaurantRating }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}