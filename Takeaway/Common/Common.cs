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
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Takeaway.Controllers
{
    public static  class Common
    {
        static TakeawayEntities db = new TakeawayEntities();
      public  enum OfferType
        {
            Restaurant = 1 ,
            ItemFood = 2
        };

        public enum OfferFeeType
        {
            Fixed = 1,
            percent = 2
        };
        public enum Distance
        {
            Distance1 = 1,//"0 => 5"
            Distance2 = 2,//"5 => 10"
            Distance3 = 3,//"10 => 15"
            Distance4 = 4,//"15 => 20"
            Distance5 = 5//"20 => 25"
        };


        public enum UserCategory
        {
            website=1,
          Android = 2, 
           IOS = 3
        }
        public static string GetEnumOfferTypeName(int i)
        {
            switch (i)
            {
                case 1:
                    return "مطعم";
                case 2:
                    return "وجبة";
                default:
                    return "";
            }
        }

        public static string GetEnumOfferFeeTypeName(int i)
        {
            switch (i)
            {
                case 1:
                    return "ثابت";
                case 2:
                    return "نسبة";
                default:
                    return "";
            }
        }

        public static string GetEnumUserCategoryName(int i)
        {
            switch (i)
            {
                case 1:
                    return "موقع";
                case 2:
                    return "اندرويد";
                      case 3:
                    return "ايفون";
                default:
                    return "";
            }
        }

        public static string GetEnumDistanceName(int i)
        {
            switch (i)
            {
                case 1:
                    return "0 => 5";
                case 2:
                    return "5 => 10";
                case 3:
                    return "10 => 15";
                case 4:
                    return "15 => 20";
                case 5:
                    return "20 => 25";
                default:
                    return "";
            }
        }

        public  static  bool OfferIntersectionCheck(Offer  model, DateTime DateFrom, DateTime DateTo)
        {
           
            Offer  offerCheck = db.Offers.Where(x => x.SubjectID   == model.SubjectID & x.OfferType == model.OfferType & x.StartDate  <= DateTo & x.EndDate  >= DateFrom & x.IsDelete  == false & x.IsActive == true).FirstOrDefault();

            if (offerCheck != null)
            {
                return true;
            }
            return false;
        }



        public enum WeekDays
        {
            Saturday = 1,
            Sunday = 2,
            Monday = 3,
            Tuesday = 4,
            Wednesday = 5,
            Thursday=6,
            Friday=7
        };

        public static string GetEnumDayName(int i)
        {
            switch (i)
            {
                case 1:
                    return "السبت";
                case 2:
                    return "الاحد";
                case 3:
                    return "الاتثين";
                case 4:
                    return "الثلاثاء";
                case 5:
                    return "الاربعاء";
                case 6:
                    return "الخميس";
                case 7:
                    return "الجمعه";
                default:
                    return "";
            }
        }
        public static int GetEnumDayNameID(int i)
        {
            switch (i)
            {
                case 6:
                    return 1;
                case 0:
                    return 2;
                case 1:
                    return 3;
                case 2:
                    return 4;
                case 3:
                    return 5;
                case 4:
                    return 6;
                case 5:
                    return 7;
                default:
                    return 0;
            }
        }

        public static bool GetRestaurantStatus(int restaurantID, string Date)
        {
            bool IsOpen = false;

            DateTime DateFrom = DateTime.ParseExact(Date, "MM/dd/yyyy", null);
           
            DayOfWeek day = DateFrom.DayOfWeek;
            int dayNo = GetEnumDayNameID((int)day);
            var data = db.RestaurantAppointments.Where(x => x.RestaurantID == restaurantID && x.Day == dayNo && x.OpeningTime <DateFrom.TimeOfDay && x.CloseTime > DateFrom.TimeOfDay && x.IsActive == true && x.IsDelete == false);
            if (data.Count() > 0)
            {
                IsOpen = true;
            }
            return IsOpen;
        }

        public static bool GetRestaurantStatus(int restaurantID, DateTime Date)
        {
            bool IsOpen = false;
          
            DateTime DateFrom = Date;
            DayOfWeek day = DateFrom.DayOfWeek;
            int dayNo = GetEnumDayNameID((int)day);
            var data = db.RestaurantAppointments.Where(x => x.RestaurantID == restaurantID && x.Day == dayNo && x.OpeningTime < DateFrom.TimeOfDay && x.CloseTime > DateFrom.TimeOfDay && x.IsActive == true && x.IsDelete == false);
            if (data.Count() > 0)
            {
                IsOpen = true;
            }
            return IsOpen;
        }


        public async static Task<string> SendGCMNotification(string apiKey, string postData, string postDataContentType = "application/json")
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateServerCertificate);

            //  
            //  MESSAGE CONTENT  
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //  
            //  CREATE REQUEST  
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
            Request.Method = "POST";
            //  Request.KeepAlive = false;  

            Request.ContentType = postDataContentType;
            Request.Headers.Add(string.Format("Authorization: key={0}", apiKey));
            Request.ContentLength = byteArray.Length;

            Stream dataStream = Request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //  
            //  SEND MESSAGE  
            try
            {
                WebResponse Response = Request.GetResponse();

                HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
                {
                    var text = "Unauthorized - need new token";
                }
                else if (!ResponseCode.Equals(HttpStatusCode.OK))
                {
                    var text = "Response from web service isn't OK";
                }

                StreamReader Reader = new StreamReader(Response.GetResponseStream());
                string responseLine = Reader.ReadToEnd();
                Reader.Close();

                return responseLine;
            }
            catch (Exception e)
            {
            }
            return "error";
        }


        public static bool ValidateServerCertificate(
                                                     object sender,
                                                     X509Certificate certificate,
                                                     X509Chain chain,
                                                     SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }



    }
   



}