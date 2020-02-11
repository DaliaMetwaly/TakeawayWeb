using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Takeaway.Models;

namespace Takeaway.Controllers
{
    // ,Restaurant
    [Authorize(Roles = "Admin,SuperAdmin,Restaurant")]
    public class OrdersController : Controller
    {
        private TakeawayEntities db = new TakeawayEntities();
        ApplicationDbContext dbMembership = new ApplicationDbContext();
        // GET: Orders
        public ActionResult Index(int? Msg)
        {
            if (Msg != null)
            {
                ViewBag.Msg = Msg;
            }

            string usrID = User.Identity.GetUserId();
            string roleName = dbMembership.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;

            ViewBag.roleName = (roleName == "Restaurant") ? 0 : 1;
            return View();
        }
        public ActionResult LoadData()
        {
            if (User.IsInRole("Restaurant"))
            {

            }
           
            string usrID = User.Identity.GetUserId();
            string roleName = dbMembership.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;

            var OrdersData = (roleName == "Restaurant")
                ? db.Orders.Where(a => a.IsDetete == false && a.RestaurantData.Restaurant.UserID == usrID && a.OrderStatu.IsDone == true && a.OrderDetails.Any(c => c.OrderID != 0)).ToList().Select(i => new
                {
                    Id = i.ID,
                    ContactName = i.User.ContactName,
                    RestaurantName = i.RestaurantData.Restaurant.RestaurantName + " - " + i.RestaurantData.Region.RegionAr,
                    OrderStatus_ar = i.OrderStatu.OrderStatus_ar,
                    //TotalPrice = i.TotalPrice,
                    //PayType = db.PayTypes.Where(a => a.IsDetete == false && i.PayType == a.ID).Select(a => a.PayNameAr),
                    OrderDate = i.OrderDate.ToString("yyyy-MM-dd"),

                    IsActive = i.IsActive == true ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                    Edit = "<a  id='btn_Edit' class='btn default btn-xs green' href='/Orders/Edit/" + i.ID + "'>تعديل</a>",
                    Deleted = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' onclick='Delete(" + i.ID + ")'  > حذف</a>",
                    Details = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' data-toggle='modal' data-target='#myModal' onclick='GetOrderDetails(" + i.ID + ")'  > التفاصيل</a>"

                }).ToList()
                : db.Orders.Where(a => a.IsDetete == false && a.OrderStatu.IsDone == true && a.OrderDetails.Any(c => c.OrderID != 0)).ToList().Select(i => new
                {
                    Id = i.ID,
                    ContactName = i.User.ContactName,
                    RestaurantName = i.RestaurantData.Restaurant.RestaurantName + " - " + i.RestaurantData.Region.RegionAr,
                    OrderStatus_ar = i.OrderStatu.OrderStatus_ar,
                    //TotalPrice = i.TotalPrice,
                    //PayType = db.PayTypes.Where(a => a.IsDetete == false && i.PayType == a.ID).Select(a => a.PayNameAr),
                    OrderDate = i.OrderDate.ToString("yyyy-MM-dd"),

                    IsActive = i.IsActive == true ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                    Edit = "<a  id='btn_Edit' class='btn default btn-xs green' href='/Orders/Edit/" + i.ID + "'>تعديل</a>",
                    Deleted = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' onclick='Delete(" + i.ID + ")'  > حذف</a>",
                    Details = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' data-toggle='modal' data-target='#myModal' onclick='GetOrderDetails(" + i.ID + ")'  > التفاصيل</a>"

                }).ToList();
            //var OrdersData = db.Orders.Where(a => a.IsDetete == false && a.OrderStatu.IsDone == true).ToList().Select(i => new
            //{
            //    Id = i.ID,
            //    ContactName = i.User.ContactName,
            //    RestaurantName = i.RestaurantData.Restaurant.RestaurantName + " - " + i.RestaurantData.Region.RegionAr,
            //    OrderStatus_ar = i.OrderStatu.OrderStatus_ar,
            //    //TotalPrice = i.TotalPrice,
            //    //PayType = db.PayTypes.Where(a => a.IsDetete == false && i.PayType == a.ID).Select(a => a.PayNameAr),
            //    OrderDate = i.OrderDate.ToString("yyyy-MM-dd"),

            //    IsActive = i.IsActive == true ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
            //    Edit = "<a  id='btn_Edit' class='btn default btn-xs green' href='/Orders/Edit/" + i.ID + "'>تعديل</a>",
            //    Deleted = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' onclick='Delete(" + i.ID + ")'  > حذف</a>",
            //    Details = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' data-toggle='modal' data-target='#myModal' onclick='GetOrderDetails(" + i.ID + ")'  > التفاصيل</a>"

            //}).ToList();

            return Json(new { data = OrdersData }, JsonRequestBehavior.AllowGet);
        }
      
       public PartialViewResult DisplayOrderDetails(int ID)
        {
            Order model = new Order();

           
            IQueryable<OrderDetail> orderDetailsLst = db.Orders.Where(x => x.IsDetete == false).Where(x => x.ID == ID)
                                     .Join(db.OrderDetails,
                                      pi => pi.ID,
                                      v => v.OrderID,
                                      (pi, v) => new { Order = pi, OrderDetail = v })
                                      .Select(x=> x.OrderDetail);


             var orderLst = db.Orders.Where(x=>x.IsDetete==false && x.ID==ID)
                           .Select(i=> new
                           {
                               i.ID,
                               i.RestaurantData ,
                               i.User,
                              // i.PayType1,
                               //i.User.ContactName,
                               //i.User.ContactPhone,
                               //i.User.UserDatas.FirstOrDefault().Region.RegionAr,
                               //i.User.UserDatas.FirstOrDefault().Address
                           }).ToList();




            model.OrderDetails = orderDetailsLst.ToList();
            model.ID = orderLst.FirstOrDefault().ID;
            model.RestaurantData= orderLst.FirstOrDefault().RestaurantData;
            model.User = orderLst.FirstOrDefault().User;
           
            //model.PayType1 = orderLst.FirstOrDefault().PayType1;
            //model.User.ContactName = orderLst.FirstOrDefault().ContactName;
            //model.User.ContactPhone = orderLst.FirstOrDefault().ContactPhone;
            //model.User.UserDatas.FirstOrDefault().Region.RegionAr = orderLst.FirstOrDefault().RegionAr;
            //model.User.UserDatas.FirstOrDefault().Address = orderLst.FirstOrDefault().Address;



            return PartialView("DisplayOrderDetails", model);
        }

        public JsonResult GetnewEditDetails(int AddditionId, int DrinkId)
        {

            var listAddsDetails = db.ItemFoods.Where(a => a.IsDetete == false && a.ID == AddditionId || a.ID == DrinkId).ToList().Select(i => new
            {
                FoodName = i.FoodName,
                CategoryTypeName = i.CategoryType.CategoryTypeName,

            }).ToList();

            return Json(listAddsDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEditDetails(int id,int orderID)
        {
            var listOrderDetails = db.OrderDetails.Where(a => a.IsDetete == false && a.ItemFoodParentID == id && a.ItemFoodParentID != 0 && a.OrderID == orderID).ToList().Select(i => new
            {
                FoodName = i.ItemFood.FoodName,
                CategoryTypeName = i.ItemFood.CategoryType.CategoryTypeName,

            }).ToList();
            return Json(listOrderDetails, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteEditDetails(int itemfoodid, int orderId)
        {
            var deletedOrderDetails = db.OrderDetails.Where(a => a.IsDetete == false&& a.ItemFoodID != itemfoodid && a.ItemFoodParentID != itemfoodid && a.OrderID == orderId)
                .ToList().Select(c => new FoodEditTempList//&&c.ItemFoodParentID==0
                {
                    Id = c.ItemFoodID,
                    FoodName = c.ItemFood.FoodName,
                    FoodPrice = c.ItemPrice,
                    Foodcount = c.ItemCount,
                    ItemFoodParentID = c.ItemFoodParentID,
                    ItemfoodId = c.ItemFoodID,
                    OrderId = c.OrderID,


                    FoodCatType = db.ItemFoods.Where(a => a.ID == c.ItemFoodID).Select(b => b.CategoryType.CategoryTypeName).FirstOrDefault(),
                }).ToList();

            List<FoodEditTempList> listfoods = new List<FoodEditTempList>();
            foreach (var item in deletedOrderDetails)
            {

                FoodEditTempList NewItemfooddetails = new FoodEditTempList();



                NewItemfooddetails.Id = item.Id;
                NewItemfooddetails.FoodName = item.FoodName;
                NewItemfooddetails.FoodPrice = item.FoodPrice;
                NewItemfooddetails.Foodcount = item.Foodcount;
                NewItemfooddetails.FoodCatType = item.FoodCatType;
                NewItemfooddetails.ItemFoodParentID = item.ItemFoodParentID;
                NewItemfooddetails.ItemfoodId = item.ItemfoodId;
                NewItemfooddetails.OrderId = item.OrderId;


                listfoods.Add(NewItemfooddetails);
            }
            return Json(listfoods, JsonRequestBehavior.AllowGet);
        }
    

        public JsonResult ChangeStatus(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return null;
            }
            Order order = db.Orders.Find(id);

            order.IsActive = order.IsActive == true ? false : true;
            var _changedetails = db.OrderDetails.Where(i => i.OrderID == order.ID).Select(i => i.ID).ToList();
            foreach (var item in _changedetails)
            {
                OrderDetail orderdetails = db.OrderDetails.Where(i => i.ID == item).FirstOrDefault();
                orderdetails.IsActive = order.IsActive;
                db.Entry(orderdetails).State = EntityState.Modified;
                db.SaveChanges();
            }
            try
            {
                db.Entry(order).State = EntityState.Modified;
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


        public JsonResult FillRadiobutton(int ItemFoodId)
        {
            var foodquery = db.ItemFoodDetails.Where(i => i.IsDetete == false && i.ItemFoodID == ItemFoodId).ToList();
            ItemFood itmfood = db.ItemFoods.Find(ItemFoodId);
            List<ItemFood> foodlist = new List<ItemFood>();
            foreach (var q in foodquery)
            {
                ItemFood fooditem = new ItemFood();


                var conrole = db.ItemFoods.Where(w => w.ID == q.AddItemFoodID);
                if (conrole.Count() > 0)
                {

                    fooditem.FoodName = conrole.FirstOrDefault().FoodName;
                    fooditem.ID = conrole.FirstOrDefault().ID;
                    fooditem.CategoryTypeID = conrole.FirstOrDefault().CategoryTypeID;
                    foodlist.Add(fooditem);
                }


            }

            return Json(new { foodlist = foodlist.ToList(), IsCooking = itmfood.IsCooking }, JsonRequestBehavior.AllowGet);

        }

        // GET: Countries/Create
        public ActionResult Create()
        {     

            ApplicationDbContext dbMembership = new ApplicationDbContext();
            string usrID = User.Identity.GetUserId();
            string roleName = dbMembership.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;


            if ( roleName == "Admin" || roleName == "SuperAdmin")
            {
                ViewBag.RestaurantID = new SelectList(db.Restaurants, "ID", "RestaurantName");
            }
            else if (roleName == "Restaurant")
            {
                ViewBag.RestaurantID = new SelectList(db.Restaurants.Where(x => x.UserID == usrID), "ID", "RestaurantName");
            }

           
            //List<RestaurantData> test = db.RestaurantDatas.Where(x => x.IsActive == true && x.IsDetete == false).ToList();

            //List<SelectListItem> RestaurantDataItem = new List<SelectListItem>();

            //foreach(RestaurantData x in test)
            //{
            //    RestaurantDataItem.Add(new SelectListItem
            //    {
            //        Text = x.Address,
            //        Value = x.ID.ToString()
            //    });
            //}
           
            //ViewBag.RestaurantDataID = RestaurantDataItem;

            ViewBag.RestaurantDataID= new SelectList(db.RestaurantDatas, "ID", "Address");
            ViewBag.DeliveryUserID = new SelectList(db.Users, "ID", "ContactName");
            ViewBag.OrderStatusID = new SelectList(db.OrderStatus, "ID", "OrderStatus_ar");
            //ViewBag.PayType = new SelectList(db.PayTypes, "ID", "PayNameAr");
            var food = db.ItemFoods.Where(x=> x.IsActive == true && x.IsDetete == false).Select(x => new
            {
                x.ID,
                FoodName = x.FoodName + " - " + x.Size.SizeAR
            });
            ViewBag.ItemFoodID = new SelectList(food, "ID", "FoodName");

            ViewBag.CookingCatID = new SelectList(db.CookingCategories.Where(x => x.IsActive == true && x.IsDelete == false), "ID", "CookingCatNameAR");

            ViewBag.DrinkID = new SelectList(db.ItemFoods.Where(a => a.CategoryTypeID == 2), "ID", "FoodName");
            ViewBag.AdditionID = new SelectList(db.ItemFoods.Where(a => a.CategoryTypeID == 3), "ID", "FoodName");
           
            return View();
        }

        [HttpPost]
        public ActionResult Create(OrderViewModel OrderViewModel)
        {
            if (ModelState.IsValid)
            {
                List<OrderDetail> TempList = new List<OrderDetail>();
                //List<OrderDetail> Temp_Remobe = db.OrderDetails.Where(a => a.OrderID == orderviewmodel.ID).ToList();
                //db.OrderDetails.RemoveRange(Temp_Remobe);
                //db.SaveChanges();

                Order newOrder = new Order();


                newOrder.DeliveryUserID = OrderViewModel.DeliveryUserID;
                //newOrder.RestaurantID = OrderViewModel.RestaurantID;
                newOrder.RestaurantDataID = OrderViewModel.RestaurantDataID;
                newOrder.OrderStatusID = OrderViewModel.OrderStatusID;
                
                //newOrder.PayType = OrderViewModel.PayType;
                newOrder.Description = string.IsNullOrEmpty(OrderViewModel.Description) ? string.Empty : OrderViewModel.Description;
                newOrder.TotalPrice = OrderViewModel.TotalPrice;
                newOrder.OrderDate = OrderViewModel.OrderDate;
                newOrder.IsActive = true;
                newOrder.IsDetete = OrderViewModel.IsDetete;
                db.Orders.Add(newOrder);
                db.SaveChanges();




                foreach (var item in OrderViewModel.FoodtempList)
                {
                    OrderDetail _sl = new OrderDetail();
                    _sl.OrderID = newOrder.ID;
                    _sl.ItemFoodID = item.Id;
                    _sl.ItemFoodParentID = 0;
                    _sl.CookingCatID = item.CookingCatID;
                    _sl.ItemPrice = item.FoodPrice;
                    _sl.ItemCount = item.Foodcount;
                    _sl.IsActive = newOrder.IsActive;

                    TempList.Add(_sl);
                    if (item.ItemfoodAddId != 0)
                    {
                        OrderDetail _s2 = new OrderDetail();
                        _s2.OrderID = newOrder.ID;
                        _s2.ItemFoodID = item.ItemfoodAddId;
                        _s2.ItemFoodParentID = item.Id;
                        _s2.CookingCatID = item.CookingCatID;
                        _s2.ItemPrice = 0;
                        _s2.ItemCount = item.Foodcount;
                        _s2.IsActive = newOrder.IsActive;
                        TempList.Add(_s2);
                    }
                    if (item.ItemfoodDrinkId != 0)
                    {
                        OrderDetail _s3 = new OrderDetail();
                        _s3.OrderID = newOrder.ID;
                        _s3.ItemFoodID = item.ItemfoodDrinkId;
                        _s3.ItemFoodParentID = item.Id;
                        _s3.CookingCatID = item.CookingCatID;
                        _s3.ItemPrice = 0;
                        _s3.ItemCount = item.Foodcount;
                        _s3.IsActive = newOrder.IsActive;
                        TempList.Add(_s3);
                    }
                }
                if (TempList.Count() > 0)
                {
                    db.OrderDetails.AddRange(TempList);
                    db.SaveChanges();
                }

                return Json(new { status = 2, ID = 1 });//RedirectToAction("Index", new { Msg = 3 });
            }
            ApplicationDbContext dbMembership = new ApplicationDbContext();
            string usrID = User.Identity.GetUserId();
            string roleName = dbMembership.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;


            if (roleName == "Admin" || roleName == "SuperAdmin")
            {
                ViewBag.RestaurantID = new SelectList(db.Restaurants, "ID", "RestaurantName");
            }
            else if (roleName == "Restaurant")
            {
                ViewBag.RestaurantID = new SelectList(db.Restaurants.Where(x => x.UserID == usrID), "ID", "RestaurantName");
            }


            ViewBag.RestaurantDataID = new SelectList(db.RestaurantDatas, "ID", "Address");
            ViewBag.DeliveryUserID = new SelectList(db.Users, "ID", "ContactName");
            ViewBag.OrderStatusID = new SelectList(db.OrderStatus, "ID", "OrderStatus_ar");
            //ViewBag.PayType = new SelectList(db.PayTypes, "ID", "PayNameAr");
            var food = db.ItemFoods.Where(x => x.IsActive == true && x.IsDetete == false).Select(x => new
            {
                x.ID,
                FoodName = x.FoodName + " - " + x.Size.SizeAR
            });
            ViewBag.ItemFoodID = new SelectList(food, "ID", "FoodName");

            ViewBag.CookingCatID = new SelectList(db.CookingCategories.Where(x => x.IsActive == true && x.IsDelete == false), "ID", "CookingCatNameAR");

            ViewBag.DrinkID = new SelectList(db.ItemFoods.Where(a => a.CategoryTypeID == 2), "ID", "FoodName");
            ViewBag.AdditionID = new SelectList(db.ItemFoods.Where(a => a.CategoryTypeID == 3), "ID", "FoodName");

            return View(OrderViewModel);
        }
        public JsonResult GetRestaurantDataList(int? RestId)
        {
            var Data = (Object)null;
            try
            {
                Data = db.RestaurantDatas.Where(x => x.IsActive == true && x.IsDetete == false && x.RestaurantID == RestId).Select(i => new { ID = i.ID, Name = i.Address }).ToList();

            }
            catch (Exception ex)
            {

            }
            return Json(new { data = Data }, JsonRequestBehavior.AllowGet);
        }
        // POST: Countries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Order ordermodel)
        //{
        //    ViewBag.DeliveryUserID = new SelectList(db.Users, "ID", "ContactName", ordermodel.DeliveryUserID);
        //    ViewBag.RestaurantID = new SelectList(db.Restaurants, "ID", "RestaurantName", ordermodel.RestaurantID);
        //    ViewBag.OrderStatusID = new SelectList(db.OrderStatus, "ID", "OrderStatus_ar", ordermodel.OrderStatusID);
        //    ViewBag.PayType = new SelectList(db.PayTypes, "ID", "PayNameAr", ordermodel.PayType);


        //    ViewBag.ItemFoodID = new SelectList(db.ItemFoods, "ID", "FoodName");
        //    ViewBag.DrinkID = new SelectList(db.ItemFoods.Where(a => a.CategoryTypeID == 2), "ID", "FoodName");
        //    ViewBag.AdditionID = new SelectList(db.ItemFoods.Where(a => a.CategoryTypeID == 3), "ID", "FoodName");




        //    if (ModelState.IsValid)
        //    {

        //        Order newOrder = new Order();


        //        newOrder.DeliveryUserID = ordermodel.DeliveryUserID;
        //        newOrder.RestaurantID = ordermodel.RestaurantID;
        //        newOrder.OrderStatusID = ordermodel.OrderStatusID;
        //        newOrder.PayType = ordermodel.PayType;
        //        newOrder.Description = string.IsNullOrEmpty(ordermodel.Description) ? string.Empty : ordermodel.Description;
        //        newOrder.TotalPrice = ordermodel.TotalPrice;
        //        newOrder.OrderDate = ordermodel.OrderDate;
        //        newOrder.IsActive = ordermodel.IsActive;
        //        newOrder.IsDetete = ordermodel.IsDetete;
        //        db.Orders.Add(newOrder);
        //        db.SaveChanges();





        //        var _id = ordermodel.ID;

        //            if (TempData.Count() > 0)
        //            {
        //                List<ItemFoodDetail> listdrinks = new List<ItemFoodDetail>();
        //                foreach (var item in TempData)
        //                {
        //                    ItemFoodDetail NewItemfooddetails = new ItemFoodDetail();
        //                    NewItemfooddetails.ItemFoodID = _id;
        //                    long itemid = Convert.ToInt64(item.Key);
        //                    var list = db.ItemFoods.Where(a => a.ID == itemid).FirstOrDefault();
        //                    NewItemfooddetails.AddItemFoodID = list.ID;//((DrinkList)item.Value).Id;
        //                    NewItemfooddetails.CategoryTypeID = list.CategoryTypeID;//((DrinkList)item.Value).CategoryTypeID;

        //                    listdrinks.Add(NewItemfooddetails);
        //                }

        //                db.ItemFoodDetails.AddRange(listdrinks);
        //                db.SaveChanges();
        //                TempData.Clear();
        //            }

        //        return RedirectToAction("Index");

        //    }

        //    return View(ordermodel);
        //}



        public JsonResult addtempDrinks(int Id)
        {

            var FoodItms = db.ItemFoods.Where(c => c.ID == Id).Select(c => new FoodTempList
            {
                Id = c.ID,
                isCooking=c.IsCooking,
                CookingCatID = 1,
               
                FoodName = c.FoodName,
                FoodPrice = c.Price ,
                FoodCatType = c.CategoryType.CategoryTypeName
            }).FirstOrDefault();

            //List<FoodTempList> listfoods = new List<FoodTempList>();
            //foreach (var item in FoodItms)
            //{

            //    FoodTempList NewItemfooddetails = new FoodTempList();

            //    NewItemfooddetails.isCooking = item.isCooking;
            //    NewItemfooddetails.CookingCatID = 1;
            //    NewItemfooddetails.Id = item.Id;
            //    NewItemfooddetails.FoodName = item.FoodName;
            //    NewItemfooddetails.FoodPrice = item.FoodPrice;
            //    NewItemfooddetails.FoodCatType = item.FoodCatType;


            //    listfoods.Add(NewItemfooddetails);
            //}
            return Json(FoodItms, JsonRequestBehavior.AllowGet);
        }

        public JsonResult edittempDrinks(int Id)
        {

            var FoodItms = db.OrderDetails.Where(c => c.OrderID == Id).Select(c => new FoodEditTempList//&&c.ItemFoodParentID==0
            {
                Id = c.ItemFoodID,
                FoodName = c.ItemFood.FoodName,
                FoodPrice = c.ItemPrice,
                Foodcount = c.ItemCount,
                ItemFoodParentID = c.ItemFoodParentID,
                ItemfoodId = c.ItemFoodID,
                OrderId = c.OrderID,
                CookingCatID = c.CookingCatID,

                FoodCatType = db.ItemFoods.Where(a => a.ID == c.ItemFoodID).Select(b => b.CategoryType.CategoryTypeName).FirstOrDefault(),
            }).ToList();

            List<FoodEditTempList> listfoods = new List<FoodEditTempList>();
            foreach (var item in FoodItms)
            {

                FoodEditTempList NewItemfooddetails = new FoodEditTempList();



                NewItemfooddetails.Id = item.Id;
                NewItemfooddetails.FoodName = item.FoodName;
                NewItemfooddetails.FoodPrice = item.FoodPrice;
                NewItemfooddetails.Foodcount = item.Foodcount;
                NewItemfooddetails.FoodCatType = item.FoodCatType;
                NewItemfooddetails.ItemFoodParentID = item.ItemFoodParentID;
                NewItemfooddetails.ItemfoodId = item.ItemfoodId;
                NewItemfooddetails.OrderId = item.OrderId;
                NewItemfooddetails.CookingCatID = item.CookingCatID;

                listfoods.Add(NewItemfooddetails);
            }
            return Json(listfoods, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public string addtempDrinks(List<DrinkList> DrinksList)
        {
            if (DrinksList == null)
            {

                return "false";
            }
            if (TempData.Count > 0)
            {
                TempData.Clear();

            }
            foreach (var item in DrinksList)
            {
                var list = db.ItemFoods.Where(a => a.ID == item.Id).Select(c => new
                {
                    id = c.ID,
                    CategoryTypeID = c.CategoryTypeID,
                }).ToList();

                TempData[item.Id.ToString()] = list;


            }
            return "true";

        }


        // GET: Countries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            ViewBag.DeliveryUserID = new SelectList(db.Users, "ID", "ContactName", order.DeliveryUserID);


            ApplicationDbContext dbMembership = new ApplicationDbContext();
            string usrID = User.Identity.GetUserId();
            string roleName = dbMembership.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;


            if (roleName == "Admin" || roleName == "SuperAdmin")
            {
                ViewBag.RestaurantDataID = new SelectList(db.RestaurantDatas.Select(x=> new { ID = x.ID, RestaurantName=x.Restaurant.RestaurantName+" "+x.Region.RegionAr }), "ID", "RestaurantName", order.RestaurantDataID);
            }
            else if (roleName == "Restaurant")
            {
                ViewBag.RestaurantDataID = new SelectList(db.Restaurants.Where(x => x.UserID == usrID), "ID", "RestaurantName", order.RestaurantData.RestaurantID);
            }

            ViewBag.OrderStatusID = new SelectList(db.OrderStatus, "ID", "OrderStatus_ar", order.OrderStatusID);
            //ViewBag.PayType = new SelectList(db.PayTypes, "ID", "PayNameAr", order.PayType);
            var food = db.ItemFoods.Where(x => x.IsActive == true && x.IsDetete == false).Select(x => new
            {
                x.ID,
                FoodName = x.FoodName + " - " + x.Size.SizeAR
            });
            ViewBag.ItemFoodID = new SelectList(food, "ID", "FoodName");

            ViewBag.CookingCatID = new SelectList(db.CookingCategories.Where(x => x.IsActive == true && x.IsDelete == false), "ID", "CookingCatNameAR");


            if (order == null)
            {
                return HttpNotFound();
            }




            OrderViewModel orderviewmodel = new OrderViewModel();
            orderviewmodel.ID = int.Parse(id.ToString());
            orderviewmodel.DeliveryUserID = order.DeliveryUserID;
           //// orderviewmodel.RestaurantID = order.RestaurantID;
            //orderviewmodel.PayType = order.PayType;
            orderviewmodel.OrderStatusID = order.OrderStatusID;
            orderviewmodel.Description = string.IsNullOrEmpty(order.Description) ? string.Empty : order.Description;
            orderviewmodel.TotalPrice = order.TotalPrice;
            orderviewmodel.OrderDate = order.OrderDate;
            orderviewmodel.IsActive = order.IsActive;
            orderviewmodel.IsDetete = order.IsDetete;
            var orderItms = db.OrderDetails.Where(c => c.OrderID == id).Select(c => new FoodEditTempList//&&c.ItemFoodParentID==0
            {
                Id = c.ItemFoodID,
                FoodName = c.ItemFood.FoodName,
                FoodPrice = c.ItemPrice,
                Foodcount = c.ItemCount,
                CookingCatID=c.CookingCatID,
                ItemFoodParentID = c.ItemFoodParentID,
                ItemfoodId = c.ItemFoodID,
                OrderId = c.OrderID,


                FoodCatType = db.ItemFoods.Where(a => a.ID == c.ItemFoodID).Select(b => b.CategoryType.CategoryTypeName).FirstOrDefault(),
            }).ToList();

            List<FoodEditTempList> listfoods = new List<FoodEditTempList>();
            foreach (var item in orderItms)
            {

                FoodEditTempList Neworderdetails = new FoodEditTempList();



                Neworderdetails.Id = item.Id;
                Neworderdetails.FoodName = item.FoodName;
                Neworderdetails.FoodPrice = item.FoodPrice;
                Neworderdetails.Foodcount = item.Foodcount;
                Neworderdetails.FoodCatType = item.FoodCatType;
                Neworderdetails.ItemFoodParentID = item.ItemFoodParentID;
                Neworderdetails.ItemfoodId = item.ItemfoodId;
                Neworderdetails.OrderId = item.OrderId;
                Neworderdetails.CookingCatID = item.CookingCatID;

                listfoods.Add(Neworderdetails);
            }

            orderviewmodel.FoodEdittempList = listfoods;
            return View(orderviewmodel);
        }
        [HttpPost]
        public ActionResult Edit(OrderViewModel OrderViewModel)
        {
            List<OrderDetail> TempList = new List<OrderDetail>();


            var newOrder = db.Orders.FirstOrDefault(c => c.ID == OrderViewModel.ID);
           // newOrder.DeliveryUserID = OrderViewModel.DeliveryUserID;
           //// newOrder.RestaurantID = OrderViewModel.RestaurantID;
            newOrder.OrderStatusID = OrderViewModel.OrderStatusID;
            //newOrder.PayType = OrderViewModel.PayType;
            //newOrder.Description = string.IsNullOrEmpty(OrderViewModel.Description) ? string.Empty : OrderViewModel.Description;
            //newOrder.TotalPrice = OrderViewModel.TotalPrice;
            //newOrder.OrderDate = OrderViewModel.OrderDate;
            //newOrder.IsActive = OrderViewModel.IsActive;
            //newOrder.IsDetete = OrderViewModel.IsDetete;
            db.Entry(newOrder).State = EntityState.Modified;
            db.SaveChanges();




            //List<OrderDetail> Temp_Remobe = db.OrderDetails.Where(a => a.OrderID == OrderViewModel.ID).ToList();
            //db.OrderDetails.RemoveRange(Temp_Remobe);
            //db.SaveChanges();

            //foreach (var item in OrderViewModel.FoodEdittempList)
            //{
            //    OrderDetail _s0 = new OrderDetail();
            //    _s0.OrderID = newOrder.ID;
            //    _s0.ItemFoodID = item.Id;
            //    _s0.ItemFoodParentID = item.ItemFoodParentID;
            //    _s0.CookingCatID = item.CookingCatID;
            //    _s0.ItemPrice = item.FoodPrice;
            //    _s0.ItemCount = item.Foodcount;
            //    _s0.IsActive = newOrder.IsActive;

            //    TempList.Add(_s0);
            //}
            //foreach (var item in OrderViewModel.FoodtempList)
            //{
            //    OrderDetail _sl = new OrderDetail();
            //    _sl.OrderID = newOrder.ID;
            //    _sl.ItemFoodID = item.Id;
            //    _sl.ItemFoodParentID = 0;
            //    _sl.CookingCatID = item.CookingCatID;
            //    _sl.ItemPrice = item.FoodPrice;
            //    _sl.ItemCount = item.Foodcount;
            //    _sl.IsActive = newOrder.IsActive;

            //    TempList.Add(_sl);
            //    if (item.ItemfoodAddId != 0)
            //    {
            //        OrderDetail _s2 = new OrderDetail();
            //        _s2.OrderID = newOrder.ID;
            //        _s2.ItemFoodID = item.ItemfoodAddId;
            //        _s2.ItemFoodParentID = item.Id;
            //        _s2.CookingCatID = item.CookingCatID;
            //        _s2.ItemPrice = 0;
            //        _s2.ItemCount = item.Foodcount;
            //        _s2.IsActive = newOrder.IsActive;
            //        TempList.Add(_s2);
            //    }
            //    if (item.ItemfoodDrinkId != 0)
            //    {
            //        OrderDetail _s3 = new OrderDetail();
            //        _s3.OrderID = newOrder.ID;
            //        _s3.ItemFoodID = item.ItemfoodDrinkId;
            //        _s3.ItemFoodParentID = item.Id;
            //        _s3.CookingCatID = item.CookingCatID;
            //        _s3.ItemPrice = 0;
            //        _s3.ItemCount = item.Foodcount;
            //        _s3.IsActive = newOrder.IsActive;
            //        TempList.Add(_s3);
            //    }
            //}
            //if (TempList.Count() > 0)
            //{
            //    db.OrderDetails.AddRange(TempList);
            //    db.SaveChanges();
            //}

            return RedirectToAction("Index", new { Msg = 1 }); //Json(new { status = 2, ID = 1 });
        }
        // POST: Countries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(ItemfoodViewModel itemfoodviewmodel)
        //{
        //    var itemfood = db.ItemFoods.FirstOrDefault(c => c.ID == itemfoodviewmodel.ID);

        //    ViewBag.RestaurantID = new SelectList(db.Restaurants, "ID", "RestaurantName", itemfood.Category.RestaurantID);
        //    ViewBag.CategoryTypeID = new SelectList(db.CategoryTypes, "ID", "CategoryTypeName", itemfood.CategoryTypeID);
        //    ViewBag.DrinkID = new SelectList(db.ItemFoods.Where(a => a.CategoryTypeID == 2), "ID", "FoodName");
        //    ViewBag.AdditionID = new SelectList(db.ItemFoods.Where(a => a.CategoryTypeID == 3), "ID", "FoodName");

        //    if (ModelState.IsValid)
        //    {


        //        itemfood.FoodName = itemfoodviewmodel.FoodName;
        //        itemfood.FoodNameEn = itemfoodviewmodel.FoodNameEn;
        //        itemfood.Description = string.IsNullOrEmpty(itemfoodviewmodel.Description) ? string.Empty : itemfoodviewmodel.Description;
        //        itemfood.DescriptionEn = string.IsNullOrEmpty(itemfoodviewmodel.DescriptionEn) ? string.Empty : itemfoodviewmodel.DescriptionEn;
        //        itemfood.Price = itemfoodviewmodel.Price;
        //        itemfood.Image = string.IsNullOrEmpty(itemfoodviewmodel.Image) ? string.Empty : itemfoodviewmodel.Image;
        //        itemfood.CategoryID = itemfoodviewmodel.CategoryID;
        //        itemfood.CategoryTypeID = itemfoodviewmodel.CategoryTypeID;
        //        itemfood.NotforSale = itemfoodviewmodel.NotforSale;
        //        itemfood.IsActive = itemfoodviewmodel.IsActive;
        //        itemfood.IsDetete = itemfoodviewmodel.IsDetete;
        //        db.Entry(itemfood).State = EntityState.Modified;
        //        db.SaveChanges();




        //        var _id = itemfood.ID;
        //        if (itemfood.CategoryTypeID != 1)
        //        {

        //            TempData.Clear();
        //            var _deletemodel = db.ItemFoodDetails.Where(i => i.ItemFoodID == itemfoodviewmodel.ID && i.CategoryTypeID == 2).ToList();



        //            db.ItemFoodDetails.RemoveRange(_deletemodel);
        //            db.SaveChanges();
        //            Session.Clear();
        //            var _deletemodels = db.ItemFoodDetails.Where(i => i.ItemFoodID == itemfoodviewmodel.ID && i.CategoryTypeID == 3).ToList();
        //            db.ItemFoodDetails.RemoveRange(_deletemodels);
        //            db.SaveChanges();

        //        }
        //        if (TempData.Count() > 0)
        //        {
        //            List<ItemFoodDetail> listic = new List<ItemFoodDetail>();
        //            foreach (var item in TempData)
        //            {
        //                ItemFoodDetail NewItemfooddetails = new ItemFoodDetail();
        //                NewItemfooddetails.ItemFoodID = _id;
        //                long itemdrinkid = Convert.ToInt64(item.Key);
        //                var list = db.ItemFoods.Where(a => a.ID == itemdrinkid).FirstOrDefault();
        //                NewItemfooddetails.AddItemFoodID = list.ID;
        //                NewItemfooddetails.CategoryTypeID = list.CategoryTypeID;

        //                listic.Add(NewItemfooddetails);
        //            }
        //            var _deletemodels = db.ItemFoodDetails.Where(i => i.ItemFoodID == itemfoodviewmodel.ID && i.CategoryTypeID == 2).ToList();
        //            db.ItemFoodDetails.RemoveRange(_deletemodels);
        //            db.SaveChanges();
        //            db.ItemFoodDetails.AddRange(listic);
        //            db.SaveChanges();
        //            TempData.Clear();
        //        }

        //        if (Session.Count > 0)
        //        {
        //            List<ItemFoodDetail> listic = new List<ItemFoodDetail>();

        //            foreach (var item in Session)
        //            {
        //                ItemFoodDetail NewItemfooddetails = new ItemFoodDetail();
        //                NewItemfooddetails.ItemFoodID = _id;
        //                long itemadditionid = Convert.ToInt64(item);
        //                var list = db.ItemFoods.Where(a => a.ID == itemadditionid).FirstOrDefault();
        //                NewItemfooddetails.AddItemFoodID = list.ID;
        //                NewItemfooddetails.CategoryTypeID = list.CategoryTypeID;

        //                listic.Add(NewItemfooddetails);
        //            }





        //            var _deletemodels = db.ItemFoodDetails.Where(i => i.ItemFoodID == itemfoodviewmodel.ID && i.CategoryTypeID == 3).ToList();
        //            db.ItemFoodDetails.RemoveRange(_deletemodels);
        //            db.SaveChanges();
        //            db.ItemFoodDetails.AddRange(listic);
        //            db.SaveChanges();
        //            Session.Clear();
        //        }

        //        return RedirectToAction("Index");
        //    }
        //    return View(itemfoodviewmodel);
        //}


        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                Order order = db.Orders.Find(id);
                order.IsDetete = true;
                var _deleteold = db.OrderDetails.Where(i => i.OrderID == order.ID).Select(i => i.ID).ToList();
                foreach (var item in _deleteold)
                {
                    OrderDetail orderdetails = db.OrderDetails.Where(i => _deleteold.Contains(i.ID)).FirstOrDefault();
                    orderdetails.IsDetete = true;
                    db.Entry(orderdetails).State = EntityState.Modified;
                    db.SaveChanges();
                }


                db.Entry(order).State = EntityState.Modified;
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

        public ActionResult OrdersbyRestaurant(int ID)
        {
            if (ID != 0)
            {
                ViewBag.ID = ID;
            }
            return View();
        }
     
        public ActionResult LoadOrdersbyRestaurant(int ID)
        {
            
            var firstDay = DateTime.Today.AddDays(-30);
            var OrdersData = 
                 db.Orders.Where(a => a.IsDetete == false 
                               && a.IsActive == true 
                               && a.RestaurantData.RestaurantID == ID
                               && a.OrderDate >= firstDay).ToList().Select(i => new
                {
                    Id = i.ID,
                    ContactName = i.User.ContactName,
                    RestaurantName = i.RestaurantData.Restaurant.RestaurantName,
                    OrderStatus_ar = i.OrderStatu.OrderStatus_ar,
                    TotalPrice = i.TotalPrice,
                    //PayType = db.PayTypes.Where(a => a.IsDetete == false && i.PayType == a.ID).Select(a => a.PayNameAr),
                    OrderDate = i.OrderDate.ToString("yyyy-MM-dd")

                    

                }).ToList();


            return Json(new { data = OrdersData }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadTotalSalesbyRestaurant(int ID)
        {
            decimal totalSales;
            var firstDay = DateTime.Today.AddDays(-30);
            var OrdersData = 
                db.Orders.Where(a => a.IsDetete == false
                               && a.IsActive == true
                               && a.RestaurantData.RestaurantID == ID
                               && a.OrderDate >= firstDay).ToList()
                               .GroupBy(s => new
                               {
                                   s.RestaurantData.RestaurantID

                               }).Select(i => new
                               {
                                   Id = i.Key.RestaurantID,
                                   TotalSales = i.Sum(y => y.TotalPrice)
                                   

                               }).ToList();
            if (OrdersData.Count>0)
            {
                totalSales = OrdersData.FirstOrDefault().TotalSales;
            }
            else
            {
                totalSales = 0;
            }

            
            return Json(new { data = totalSales }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadChart(int ID)
        {
           
            var firstDay = DateTime.Today.AddDays(-30);

            var ChartData =
                db.OrderDetails
                          .Where(x => x.Order.RestaurantData.RestaurantID == ID
                                   && x.IsActive == true
                                   && x.IsDetete == false
                                   && x.Order.IsActive == true
                                   && x.Order.IsDetete == false
                                   && x.Order.OrderDate >= firstDay).ToList()
                          .GroupBy(s => new
                          {
                              s.ItemFood.ID,
                              s.ItemFood.FoodName
                          })
                          .Select(a => new
                          {
                              FoodName = a.Key.FoodName,
                              FoodID = a.Key.ID,
                              TotalSales = a.Sum(y => y.ItemPrice * y.ItemCount)
                          }).ToList();
               
                         

            //Random rnd = new Random();
            
            //int month = rnd.Next(50000, 1000000);
            //int month1 = rnd.Next(50000, 1000000);
            //int month2 = rnd.Next(50000, 1000000);
            //int month3 = rnd.Next(50000, 1000000);

            List<clsProviderRevenue> List_ProviderName = new List<clsProviderRevenue>();

            foreach(var item in ChartData)
            {
                clsProviderRevenue Rev = new clsProviderRevenue();
                Rev.ProviderName = item.FoodName;
                Rev.Revenue = item.TotalSales;
                List_ProviderName.Add(Rev);
            }
            //clsProviderRevenue dd = new clsProviderRevenue();
            //dd.ProviderName = "يخت ازيموت 46 قدم";
            //dd.Revenue = month;
            //List_ProviderName.Add(dd);

            //clsProviderRevenue dd1 = new clsProviderRevenue();
            //dd1.ProviderName = "يخت امباسدور 55 قدم";
            //dd1.Revenue = month1;
            //List_ProviderName.Add(dd1);

            //clsProviderRevenue dd2 = new clsProviderRevenue();
            //dd2.ProviderName = "طراد حالول 28 قدم";
            //dd2.Revenue = month2;
            //List_ProviderName.Add(dd2);

            //clsProviderRevenue dd3 = new clsProviderRevenue();
            //dd3.ProviderName = "جت بوت ياماها limited 242s";
            //dd3.Revenue = month3;
            //List_ProviderName.Add(dd3);
            
          
            return Json(new { data = List_ProviderName }, JsonRequestBehavior.AllowGet);
        }

        #region GetOrderCount
        public ActionResult GetOrderCount( )
        {
            var daily = db.Orders.Where(GetList(1)).ToList();
           
            var monthly = db.Orders.Where(GetList(2)).ToList();
            var yearly = db.Orders.Where(GetList(3)).ToList();
            var dailyOrder = db.Orders.Where(GetList(1));
            var UnderDelivery = dailyOrder.Where(x=> x.OrderStatusID == 1).Count();
            var Canceled = dailyOrder.Where(x => x.OrderStatusID == 2).Count();
            var Delivered = dailyOrder.Where(x => x.OrderStatusID == 3).Count();
            decimal dailysum = 0;
            foreach (var item in daily)
            {
                dailysum = dailysum + item.FeeValue;
                //}
                //if (item.Restaurant.FeeTypeID == 1)
                //{
                //    dailysum = dailysum + item.Restaurant.FeeType.FeeValue;
                //}
                //else
                //{
                //    dailysum = dailysum + ((item.Restaurant.FeeType.FeeValue * item.TotalPrice) / 100);
                //}
            }
            //string  dailyRes = "<span class='rating' data-score='" + (dailysum.ToString()) + "'></span>";
            decimal monthlysum = 0;
            foreach (var item in monthly)
            {
                monthlysum = monthlysum + item.FeeValue;
                //if (item.Restaurant.FeeTypeID == 1)
                //{
                //    monthlysum = monthlysum + item.Restaurant.FeeType.FeeValue;
                //}
                //else
                //{
                //    monthlysum = monthlysum + ((item.Restaurant.FeeType.FeeValue * item.TotalPrice) / 100);
                //}
            }

            decimal yearlysum = 0;
            foreach (var item in yearly)
            {
                yearlysum = yearlysum + item.FeeValue;
                //if (item.Restaurant.FeeTypeID == 1)
                //{
                //    yearlysum = yearlysum + item.Restaurant.FeeType.FeeValue;
                //}
                //else
                //{
                //    yearlysum = yearlysum + ((item.Restaurant.FeeType.FeeValue * item.TotalPrice) / 100);
                //}
            }

            return Json(new { yearlysum = yearlysum, monthlysum  = monthlysum , dailysum= dailysum, Delivered = Delivered, UnderDelivery = UnderDelivery ,Canceled = Canceled }, JsonRequestBehavior.AllowGet);
        }

        private Func<Order, bool> GetList(int? periodID)
        {

            Func<Order, bool> expr = p => p.IsDetete == false && p.IsActive == true;
            DateTime DateFrom = new DateTime();
            DateTime Dateto = new DateTime();
            if (periodID == 1)//daily
            {
                DateFrom = DateTime.Now;//DateTime.ParseExact(DateTime.Now, "MM/dd/yyyy", null);
                TimeSpan ts1 = new TimeSpan(00, 00, 0);
                DateFrom = DateFrom.Date + ts1;
                Dateto = DateTime.Now;// DateTime.ParseExact(to, "MM/dd/yyyy", null);
                TimeSpan ts = new TimeSpan(23, 59, 0);
                Dateto = Dateto.Date + ts;
            }
            else if (periodID == 2)//monthly
            {
                DateFrom = new DateTime (DateTime.Now.Year, DateTime.Now.Month,1);
                TimeSpan ts1 = new TimeSpan(00, 00, 0);
                DateFrom = DateFrom.Date + ts1;
                Dateto = DateTime.Now;
                
            }
            else if (periodID == 3)//yearly
            {
                DateFrom = new DateTime(DateTime.Now.Year,  1,1);
                TimeSpan ts1 = new TimeSpan(00, 00, 0);
                DateFrom = DateFrom.Date + ts1;
                Dateto = DateTime.Now;

            }

            expr = p => p.IsDetete == false && p.IsActive == true && (p.OrderDate >= DateFrom && p.OrderDate <= Dateto);
               
            return expr;
        }
        #endregion
    }


    public class clsProviderRevenue
    {
        public string ProviderName { get; set; }
        public decimal Revenue { get; set; }


    }
}