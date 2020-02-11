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
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Takeaway.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin,Restaurant")]
    public class ItemFoodsController : Controller
    {
        // GET: ItemFoods
        private TakeawayEntities db = new TakeawayEntities();
       // CustomTempData customtemp = new CustomTempData();
        List<AdditionList> addlist;

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
            ApplicationDbContext dbMembership = new ApplicationDbContext();
            string usrID = User.Identity.GetUserId();
            string roleName = dbMembership.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;

            var ItemFoodsData= (roleName == "Restaurant")
                ? db.ItemFoods.Where(a => a.IsDetete == false  && a.Category.Restaurant.UserID == usrID).ToList().Select(i => new
                {
                    FoodName = i.FoodName,
                    FoodNameEn = i.FoodNameEn,
                    RestaurantName=i.Category.Restaurant.RestaurantName,
                    DescriptionAr = i.Description,
                    DescriptionEn = i.DescriptionEn,
                    CategoryName = i.Category.CategoryName,
                    CategoryNameEn = i.Category.CategoryNameEn,
                    CategoryTypeName = i.CategoryType.CategoryTypeName,
                    CategoryTypeNameEn = i.CategoryType.CategoryTypeNameEn,
                    Price = i.Price,
                    Size = i.Size.SizeAR,
                    IsCooking =i.IsCooking==true ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='ChangeCooking(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='ChangeCooking(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                    IsActive = i.IsActive == true ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                    Edit = "<a  id='btn_Edit' class='btn default btn-xs green' href='/ItemFoods/Edit/" + i.ID + "'>تعديل</a>",
                    Deleted = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' onclick='Delete(" + i.ID + ")'  > حذف</a>"

                }).ToList()
                : db.ItemFoods.Where(a => a.IsDetete == false ).ToList().Select(i => new
                {
                    FoodName = i.FoodName,
                    FoodNameEn = i.FoodNameEn,
                    RestaurantName = i.Category.Restaurant.RestaurantName,
                    DescriptionAr = i.Description,
                    DescriptionEn = i.DescriptionEn,
                    CategoryName = i.Category.CategoryName,
                    CategoryNameEn = i.Category.CategoryNameEn,
                    CategoryTypeName = i.CategoryType.CategoryTypeName,
                    CategoryTypeNameEn = i.CategoryType.CategoryTypeNameEn,
                    Price = i.Price,
                    Size=i.Size.SizeAR,
                    IsCooking = i.IsCooking == true ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='ChangeCooking(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='ChangeCooking(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                    IsActive = i.IsActive == true ? "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-ok'></ span >" : "<span id='btn_Delete'  style='cursor:pointer;cursor:hand;' onclick='Change(" + i.ID + ")' class='glyphicon glyphicon-remove'></ span >",
                    Edit = "<a  id='btn_Edit' class='btn default btn-xs green' href='/ItemFoods/Edit/" + i.ID + "'>تعديل</a>",
                    Deleted = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' onclick='Delete(" + i.ID + ")'  > حذف</a>"

                }).ToList();

          
          
            return Json(new { data = ItemFoodsData }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDrinks(int DrinkId) {

            var ItemDrinksData = db.ItemFoods.Where(a => a.IsDetete == false&&a.ID== DrinkId).ToList().Select(i => new
            {
                DrinkName = i.FoodName,
                DrinkNameEn = i.FoodNameEn,
                Deleted = "<a  id='btn_Delete' align='center' class='delete btn default btn-xs green' onclick='Delete(" + i.ID + ")'  > حذف</a>"

            }).ToList();
            return Json(new { data = ItemDrinksData }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult ChangeStatus(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return null;
            }
            ItemFood itemfood = db.ItemFoods.Find(id);

            itemfood.IsActive = itemfood.IsActive == true ? false : true;
            try
            {
                db.Entry(itemfood).State = EntityState.Modified;
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

        public JsonResult ChangeCooking(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return null;
            }
            ItemFood itemfood = db.ItemFoods.Find(id);

            itemfood.IsCooking = itemfood.IsCooking == true ? false : true;
            try
            {
                db.Entry(itemfood).State = EntityState.Modified;
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

        public JsonResult Get_Category(int id)
        {

            ViewBag.CategoryID = new SelectList(db.Categories.Where(a => a.RestaurantID ==id ), "ID", "CategoryName");
            List<SelectListItem> _Category = new List<SelectListItem>();
            _Category.Add(new SelectListItem { Text = "اختر الصنف ", Value = null });
            var _branchquery = db.Categories.ToList().Where(a => a.RestaurantID == id);

            foreach (var q in _branchquery)
            {
                _Category.Add(new SelectListItem { Text = q.CategoryName, Value = q.ID.ToString() });
            }

            //ViewBag.CityID = new SelectList(db.Cities.Where(a=>a.ID==id), "ID", "CityEn");

            return Json(new SelectList(_Category, "Value", "Text"));

        }

        // GET: Countries/Create
        public ActionResult Create()
        {
            addlist = new List<AdditionList>();
            ApplicationDbContext dbMembership = new ApplicationDbContext();
            string usrID = User.Identity.GetUserId();
            string roleName = dbMembership.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;
            

            if (roleName == "Admin" || roleName == "SuperAdmin")
            {
                ViewBag.RestaurantID = new SelectList(db.Restaurants, "ID", "RestaurantName");
            }
            else if (roleName == "Restaurant")
            {
                ViewBag.RestaurantID = new SelectList(db.Restaurants.Where(x=>x.UserID== usrID), "ID", "RestaurantName");
            }
           
            
            ViewBag.CategoryTypeID = new SelectList(db.CategoryTypes, "ID", "CategoryTypeName");
            ViewBag.DrinkID = new SelectList(db.ItemFoods.Where(a=>a.CategoryTypeID==2), "ID", "FoodName");
            ViewBag.AdditionID = new SelectList(db.ItemFoods.Where(a => a.CategoryTypeID == 3), "ID", "FoodName");
            ViewBag.SizeID = new SelectList(db.Sizes.ToList(),"ID", "SizeAR");
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemfoodViewModel itemfoodviewmodel)
        {
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
            
            ViewBag.CategoryTypeID = new SelectList(db.CategoryTypes, "ID", "CategoryTypeName", itemfoodviewmodel.CategoryTypeID);
            ViewBag.DrinkID = new SelectList(db.ItemFoods.Where(a => a.CategoryTypeID == 2), "ID", "FoodName");
            ViewBag.AdditionID = new SelectList(db.ItemFoods.Where(a => a.CategoryTypeID == 3), "ID", "FoodName");
            ViewBag.SizeID = new SelectList(db.Sizes.ToList(), "ID", "SizeAR");
            var validImageTypes = new string[]
           {
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png",
                    "image/tiff",
                    "image/eps",
                    "image/*"

           };

            if (itemfoodviewmodel.imgpath != null)
            {



                if (!validImageTypes.Contains(itemfoodviewmodel.imgpath.ContentType))
                {
                    ModelState.AddModelError(string.Empty, " رجاء تاكد من نوع الصورة");

                   return View();
                }
                string pic = System.IO.Path.GetFileName(itemfoodviewmodel.imgpath.FileName);

                var parsepic = pic.Split('.');
                Random rand = new Random();
                pic = rand.Next() + "." + parsepic[1];
                string path = System.IO.Path.Combine(Server.MapPath("~/Images/ItemFood"), pic);
                itemfoodviewmodel.Image = pic;
                itemfoodviewmodel.imgpath.SaveAs(path);

            }

            if (ModelState.IsValid)
            {
                List<SizeList> sizeList = (List<SizeList>)TempData["Size"];
                foreach (SizeList sizeListitem in sizeList)
                {
                    ItemFood itemfood = new ItemFood();

                    itemfood.FoodName = itemfoodviewmodel.FoodName;
                    itemfood.FoodNameEn = itemfoodviewmodel.FoodNameEn;
                    itemfood.Description = string.IsNullOrEmpty(itemfoodviewmodel.Description) ? string.Empty : itemfoodviewmodel.Description;
                    itemfood.DescriptionEn = string.IsNullOrEmpty(itemfoodviewmodel.DescriptionEn) ? string.Empty : itemfoodviewmodel.DescriptionEn;
                    itemfood.Price = sizeListitem.Price;//itemfoodviewmodel.Price;
                    itemfood.Image = string.IsNullOrEmpty(itemfoodviewmodel.Image) ? string.Empty : itemfoodviewmodel.Image;
                    itemfood.CategoryID = itemfoodviewmodel.CategoryID;
                    itemfood.CategoryTypeID = itemfoodviewmodel.CategoryTypeID;
                    itemfood.NotforSale = itemfoodviewmodel.NotforSale;
                    itemfood.IsCooking = itemfoodviewmodel.IsCooking;
                    itemfood.SizeID = sizeListitem.SizeId; //itemfoodviewmodel.SizeID;
                    itemfood.IsActive = true;
                    itemfood.IsDetete = itemfoodviewmodel.IsDetete;
                    db.ItemFoods.Add(itemfood);
                    db.SaveChanges();


                    #region comment
                    //List<ItemFoodDetail> listadds = new List<ItemFoodDetail>();
                    //foreach (var item in itemfoodviewmodel.additionList)
                    //{
                    //    var list = db.ItemFoods.Where(a => a.ID == item.Id).FirstOrDefault();
                    //    ItemFoodDetail _sl = new ItemFoodDetail();
                    //    _sl.ItemFoodID = item.Id;
                    //    _sl.AddItemFoodID = list.ID;
                    //    _sl.CategoryTypeID = list.CategoryTypeID;
                    //    listadds.Add(_sl);
                    //}
                    //if (listadds.Count() > 0)
                    //{
                    //    db.ItemFoodDetails.AddRange(listadds);
                    //    db.SaveChanges();
                    //} 
                    #endregion



                    var _id = itemfood.ID;
                    if (itemfood.CategoryTypeID == 1)
                    {
                        // if (TempData.Count() > 0)
                        if (TempData["Drink"] != null)
                        {
                            List<ItemFoodDetail> listdrinks = new List<ItemFoodDetail>();
                            List<ItemFood> itmFood = (List<ItemFood>)TempData["Drink"];
                            foreach (var item in itmFood)
                            {
                                //if (item.Key != "Size")
                                //{
                                ItemFoodDetail NewItemfooddetails = new ItemFoodDetail();
                                NewItemfooddetails.ItemFoodID = _id;
                                long itemid = Convert.ToInt64(item.ID);
                                var list = db.ItemFoods.Where(a => a.ID == itemid).FirstOrDefault();
                                NewItemfooddetails.AddItemFoodID = list.ID;//((DrinkList)item.Value).Id;
                                NewItemfooddetails.CategoryTypeID = list.CategoryTypeID;//((DrinkList)item.Value).CategoryTypeID;

                                listdrinks.Add(NewItemfooddetails);
                                // }
                            }
                            foreach (var item in Session)
                            {
                                ItemFoodDetail NewItemfooddetails = new ItemFoodDetail();
                                NewItemfooddetails.ItemFoodID = _id;
                                long itemadditionid = Convert.ToInt64(item);
                                var list = db.ItemFoods.Where(a => a.ID == itemadditionid).FirstOrDefault();
                                NewItemfooddetails.AddItemFoodID = list.ID;
                                NewItemfooddetails.CategoryTypeID = list.CategoryTypeID;

                                listdrinks.Add(NewItemfooddetails);
                            }
                            db.ItemFoodDetails.AddRange(listdrinks);
                            db.SaveChanges();

                        }
                    }

                }
                TempData.Clear();
                Session.Clear();
                ViewBag.Msg = 3;
                return RedirectToAction("Index", new { Msg = 3 });
                    
                
            }

            return View(itemfoodviewmodel);
        }

      
      
        public JsonResult addtempDrinks(int Id)
        {

            var IMGCats = db.ItemFoodDetails.Where(c => c.ItemFoodID == Id).Select(c => c.ID);
            var CategoriesList = db.ItemFoodDetails.Where(c => IMGCats.Contains(c.ID)&&c.CategoryTypeID==2)
                .Select(c => new DrinkList { Id = c.AddItemFoodID, DrinkName = c.ItemFood.FoodName }).ToList();

                       List<DrinkList> listdrinks = new List<DrinkList>();
            foreach (var item in CategoriesList)
            {
                 
                DrinkList NewItemfooddetails = new DrinkList();
            
               
                var list = db.ItemFoods.Where(a => a.ID == item.Id).FirstOrDefault();
                NewItemfooddetails.Id = list.ID;
                NewItemfooddetails.DrinkName = list.FoodName;
               

                listdrinks.Add(NewItemfooddetails);
            }
            return Json(listdrinks, JsonRequestBehavior.AllowGet);
        }

        public JsonResult addtempAdditiion(int Id)
        {

            var IMGCats = db.ItemFoodDetails.Where(c => c.ItemFoodID == Id).Select(c => c.ID);
            var CategoriesList = db.ItemFoodDetails.Where(c => IMGCats.Contains(c.ID) && c.CategoryTypeID == 3)
               .Select(c => new AdditionList { Id = c.AddItemFoodID, AdditonName = c.ItemFood.FoodName }).ToList();

            List<AdditionList> listadditions = new List<AdditionList>();
            foreach (var item in CategoriesList)
            {

                AdditionList NewItemfooddetails = new AdditionList();


                var list = db.ItemFoods.Where(a => a.ID == item.Id).FirstOrDefault();
                NewItemfooddetails.Id = list.ID;
                NewItemfooddetails.AdditonName = list.FoodName;


                listadditions.Add(NewItemfooddetails);
            }
            return Json(listadditions, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public string addtempSize(List<SizeList> SizeList)
        {
            if (SizeList == null)
            {

                return "false";
            }
            TempData["Size"] = SizeList;
            //if (TempData.Count > 0)
            //{
            //    TempData.Clear();

            //}
            //foreach (var item in SizeList)
            //{
            //    var list = db.ItemFoods.Where(a => a.ID == item.Id).Select(c => new
            //    {
            //        id = c.ID,
            //        CategoryTypeID = c.CategoryTypeID,
            //    }).ToList();

            //    TempData[item.Id.ToString()] = list;


            //}
            return "true";

        }

        [HttpPost]
        public string addtempDrinks(List<DrinkList> DrinksList)
        {
            if (DrinksList == null)
            {

                return "false";
            }
            //if (TempData.Count > 0)
            //{
            //    TempData.Clear();

            //}

          
            List<ItemFood> foodlist = new List<ItemFood>();
            foreach (var item in DrinksList)
            {
                ItemFood list = db.ItemFoods.Find(item.Id);
                    //Where(a=> a.ID == item.Id)
                //    .Select(c => new
                //{
                //    id = c.ID,
                //    CategoryTypeID = c.CategoryTypeID,
                //}).ToList<ItemFood>();
                foodlist.Add(list);
               // TempData[item.Id.ToString()] = list;


            }
            TempData["Drink"] = foodlist;
            return "true";

        }
        [HttpPost]
        public string addtempAdditions(List<AdditionList> additionlist)
        {
            if (additionlist == null)
            {

                return "false";
            }
            if (Session.Count > 0)
            {
                Session.Clear();

            }
            foreach (var item in additionlist)
            {
                var list = db.ItemFoods.Where(a => a.ID == item.Id).Select(c => new
                {
                    Id = c.ID,
                    AdditonName = c.FoodName,
                }).ToList();
                Session[item.Id.ToString()] = list;
            }
            return "true";

        }
        //[HttpPost]
        //public string addtempAdditions(List<AdditionList> additionlist)
        //{
        //    if (additionlist == null)
        //    {

        //        return "false";
        //    }
        //    if (_itemfoodViewModel.additionList != null)
        //    {
        //        _itemfoodViewModel.additionList.Clear();

        //    }
        //    foreach (var item in additionlist)
        //    {
        //        var list = db.ItemFoods.Where(a => a.ID == item.Id).Select(c => new 
        //        {
        //            Id = c.ID,
        //            AdditonName = c.FoodName,
        //        }).ToList();
        //        AdditionList itemlist = new AdditionList();
        //        itemlist.Id = list.FirstOrDefault().Id;
        //        itemlist.AdditonName = list.FirstOrDefault().AdditonName;
        //        addlist.Add(itemlist);

        //        _itemfoodViewModel.additionList.Add(itemlist);

        //        //  = new List<AdditionList>(list);
        //        // List <AdditionList> _additionlist= list;
        //    }
        //    return "true";

        //}

      

        // GET: Countries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemFood itemfood = db.ItemFoods.Find(id);
           // ViewBag.RestaurantID = new SelectList(db.Restaurants, "ID", "RestaurantName",itemfood.Category.RestaurantID);

            ApplicationDbContext dbMembership = new ApplicationDbContext();
            string usrID = User.Identity.GetUserId();
            string roleName = dbMembership.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;

            if (roleName == "Admin" || roleName == "SuperAdmin")
            {
                ViewBag.RestaurantID = new SelectList(db.Restaurants, "ID", "RestaurantName",itemfood.Category.RestaurantID);
            }
            else if (roleName == "Restaurant")
            {
                ViewBag.RestaurantID = new SelectList(db.Restaurants.Where(x => x.UserID == usrID), "ID", "RestaurantName", itemfood.Category.RestaurantID);
            }



            ViewBag.CategoryID = new SelectList(db.Categories.Where(a=>a.RestaurantID==itemfood.Category.RestaurantID), "ID", "CategoryName", itemfood.CategoryID);
            ViewBag.CategoryTypeID = new SelectList(db.CategoryTypes, "ID", "CategoryTypeName", itemfood.CategoryTypeID);
            ViewBag.DrinkID = new SelectList(db.ItemFoods.Where(a => a.CategoryTypeID == 2), "ID", "FoodName");
            ViewBag.AdditionID = new SelectList(db.ItemFoods.Where(a => a.CategoryTypeID == 3), "ID", "FoodName");
            ViewBag.SizeID = new SelectList(db.Sizes.ToList(), "ID", "SizeAR",itemfood.SizeID);
            if (itemfood == null)
            {
                return HttpNotFound();
            }
            ItemfoodViewModel itemfoodviewmodel = new ItemfoodViewModel();
            itemfoodviewmodel.ID = id.Value;
            itemfoodviewmodel.FoodName = itemfood.FoodName;
            itemfoodviewmodel.FoodNameEn = itemfood.FoodNameEn;
            itemfoodviewmodel.Description = string.IsNullOrEmpty(itemfood.Description) ? string.Empty : itemfood.Description;
            itemfoodviewmodel.DescriptionEn = string.IsNullOrEmpty(itemfood.DescriptionEn) ? string.Empty : itemfood.DescriptionEn;
            itemfoodviewmodel.Price = itemfood.Price;
            itemfoodviewmodel.Image = string.IsNullOrEmpty(itemfood.Image) ? string.Empty : itemfood.Image;

            itemfoodviewmodel.CategoryID = itemfood.CategoryID;
            itemfoodviewmodel.CategoryTypeID = itemfood.CategoryTypeID;
            itemfoodviewmodel.NotforSale = itemfood.NotforSale;
            //itemfoodviewmodel.IsActive = itemfood.IsActive;
            itemfoodviewmodel.IsDetete = itemfood.IsDetete;
            itemfoodviewmodel.IsCooking = itemfood.IsCooking;
            ViewBag.tempimgpath = "/Images/ItemFood/" + itemfoodviewmodel.Image;
            return View(itemfoodviewmodel);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ItemfoodViewModel itemfoodviewmodel)
        {
            var itemfood = db.ItemFoods.FirstOrDefault(c => c.ID == itemfoodviewmodel.ID);
            itemfoodviewmodel.Image = itemfood.Image;

            ApplicationDbContext dbMembership = new ApplicationDbContext();
            string usrID = User.Identity.GetUserId();
            string roleName = dbMembership.Roles.Where(r => r.Users.Where(u => u.UserId == usrID).FirstOrDefault().UserId == usrID).FirstOrDefault().Name  /* Get role name for UserID*/ ;

            if (roleName == "Admin" || roleName == "SuperAdmin")
            {
                ViewBag.RestaurantID = new SelectList(db.Restaurants, "ID", "RestaurantName", itemfood.Category.RestaurantID);
            }
            else if (roleName == "Restaurant")
            {
                ViewBag.RestaurantID = new SelectList(db.Restaurants.Where(x => x.UserID == usrID), "ID", "RestaurantName", itemfood.Category.RestaurantID);
            }

           
            ViewBag.CategoryTypeID = new SelectList(db.CategoryTypes, "ID", "CategoryTypeName", itemfood.CategoryTypeID);
            ViewBag.DrinkID = new SelectList(db.ItemFoods.Where(a => a.CategoryTypeID == 2), "ID", "FoodName");
            ViewBag.AdditionID = new SelectList(db.ItemFoods.Where(a => a.CategoryTypeID == 3), "ID", "FoodName");
            ViewBag.SizeID = new SelectList(db.Sizes.ToList(), "ID", "SizeAR", itemfood.SizeID);
            var validImageTypes = new string[]
       {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
       };
            
                if (ModelState.IsValid)
                {


                // string OldImage = _CarData.LicenseImage;
                // var _CarDatarow = db.CarDatas.FirstOrDefault(c => c.ID == _CarData.ID);
                if (itemfoodviewmodel.imgpath != null)
                {
                    string pic = System.IO.Path.GetFileName(itemfoodviewmodel.imgpath.FileName);
                    var parsepic = pic.Split('.');
                    Random rand = new Random();
                    pic = rand.Next() + "." + parsepic[1];
                    string path = System.IO.Path.Combine(Server.MapPath("~/Images/ItemFood"), pic);
                    // file is uploaded
                    //var _path = "IMG/" + oldcompanyinfo.FileName;
                    var _oldpath = System.IO.Path.Combine(Server.MapPath("~/Images/ItemFood"), itemfood.Image);
                    // System.IO.File.Delete(_oldpath);
                    FileInfo fi = new FileInfo(_oldpath);
                    if (fi.Exists)
                        fi.Delete();
                    // file is uploaded
                    itemfoodviewmodel.imgpath.SaveAs(path);
                    itemfoodviewmodel.Image = pic;


                }
                else
                {
                    // _CarData.LicenseImage = OldImage;
                }



                itemfood.FoodName = itemfoodviewmodel.FoodName;
                    itemfood.FoodNameEn = itemfoodviewmodel.FoodNameEn;
                    itemfood.Description = string.IsNullOrEmpty(itemfoodviewmodel.Description) ? string.Empty : itemfoodviewmodel.Description;
                    itemfood.DescriptionEn = string.IsNullOrEmpty(itemfoodviewmodel.DescriptionEn) ? string.Empty : itemfoodviewmodel.DescriptionEn;
                    itemfood.Price = itemfoodviewmodel.Price;
                    itemfood.Image = string.IsNullOrEmpty(itemfoodviewmodel.Image) ? string.Empty : itemfoodviewmodel.Image;
                    itemfood.CategoryID = itemfoodviewmodel.CategoryID;
                    itemfood.CategoryTypeID = itemfoodviewmodel.CategoryTypeID;
                    itemfood.NotforSale = itemfoodviewmodel.NotforSale;
                    itemfood.SizeID = itemfoodviewmodel.SizeID;
                    itemfood.IsCooking = itemfoodviewmodel.IsCooking;
                    //itemfood.IsActive = itemfoodviewmodel.IsActive;
                    itemfood.IsDetete = itemfoodviewmodel.IsDetete;
                    db.Entry(itemfood).State = EntityState.Modified;
                     db.SaveChanges();


                

                var _id = itemfood.ID;
                if (itemfood.CategoryTypeID != 1) {

                    TempData.Clear();
                    var _deletemodel = db.ItemFoodDetails.Where(i => i.ItemFoodID == itemfoodviewmodel.ID && i.CategoryTypeID == 2).ToList();



                    db.ItemFoodDetails.RemoveRange(_deletemodel);
                    db.SaveChanges();
                    Session.Clear();
                    var _deletemodels = db.ItemFoodDetails.Where(i => i.ItemFoodID == itemfoodviewmodel.ID && i.CategoryTypeID == 3).ToList();
                    db.ItemFoodDetails.RemoveRange(_deletemodels);
                    db.SaveChanges();

                }
                    if (TempData.Count() > 0)
                    {
                        List<ItemFoodDetail> listic = new List<ItemFoodDetail>();
                    List<ItemFood> itmFood = (List<ItemFood>)TempData["Drink"];
                    foreach (var item in itmFood)
                    {
                        ItemFoodDetail NewItemfooddetails = new ItemFoodDetail();
                        NewItemfooddetails.ItemFoodID = _id;
                        long itemdrinkid = Convert.ToInt64(item.ID);
                        var list = db.ItemFoods.Where(a => a.ID == itemdrinkid).FirstOrDefault();
                        NewItemfooddetails.AddItemFoodID = list.ID;
                        NewItemfooddetails.CategoryTypeID = list.CategoryTypeID;

                        listic.Add(NewItemfooddetails);
                        }
                    var _deletemodels = db.ItemFoodDetails.Where(i => i.ItemFoodID == itemfoodviewmodel.ID && i.CategoryTypeID == 2).ToList();
                    db.ItemFoodDetails.RemoveRange(_deletemodels);
                    db.SaveChanges();
                    db.ItemFoodDetails.AddRange(listic);
                    db.SaveChanges();
                    
                }
                TempData.Clear();

                if (Session.Count > 0)
                    {
                    List<ItemFoodDetail> listic = new List<ItemFoodDetail>();

                    foreach (var item in Session)
                        {
                            ItemFoodDetail NewItemfooddetails = new ItemFoodDetail();
                            NewItemfooddetails.ItemFoodID = _id;
                            long itemadditionid = Convert.ToInt64(item);
                        var list = db.ItemFoods.Where(a => a.ID == itemadditionid).FirstOrDefault();
                            NewItemfooddetails.AddItemFoodID = list.ID;
                            NewItemfooddetails.CategoryTypeID = list.CategoryTypeID;

                            listic.Add(NewItemfooddetails);
                        }

                    
                       


                    var _deletemodels = db.ItemFoodDetails.Where(i =>i.ItemFoodID== itemfoodviewmodel.ID&&i.CategoryTypeID==3).ToList();
                        db.ItemFoodDetails.RemoveRange(_deletemodels);
                        db.SaveChanges();
                        db.ItemFoodDetails.AddRange(listic);
                        db.SaveChanges();
                      Session.Clear();
                    }

                ViewBag.Msg = 3;
                return RedirectToAction("Index", new { Msg = 3 });
            }
            return View(itemfoodviewmodel);
        }


        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                ItemFood itemfood = db.ItemFoods.Find(id);
                itemfood.IsDetete = true;
                var _deleteold = db.ItemFoodDetails.Where(i => i.ID == itemfood.ID).Select(i => i.ID).ToList();
                foreach (var item in _deleteold)
                {
                    ItemFoodDetail itemfooddetails = db.ItemFoodDetails.Where(i => _deleteold.Contains(i.ID)).FirstOrDefault();
                    itemfooddetails.IsDetete = true;
                    db.Entry(itemfooddetails).State = EntityState.Modified;
                    db.SaveChanges();
                }
              
               
                db.Entry(itemfood).State = EntityState.Modified;
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