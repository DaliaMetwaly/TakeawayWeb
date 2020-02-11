using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Takeaway.Models;

[assembly: OwinStartupAttribute(typeof(Takeaway.Startup))]
namespace Takeaway
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        // In this method we will create default User roles and Admin user for login   
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ///// temp for Add User
            ///// 
            //var tempuser = new ApplicationUser();
            //tempuser.UserName = "DeliveryUser";
            //tempuser.Email = "DeliveryUser@gmail.com";

            //string tempuserPWD = "User@#23";

            //var tempchkUser = UserManager.Create(tempuser, tempuserPWD);

            ////Add default User to Role Admin   
            //if (tempchkUser.Succeeded)
            //{
            //    var result1 = UserManager.AddToRole(tempuser.Id, "User");

            //}
            ////
            //// End Temp
            ///////////////////////////////////////////////
            //var usersa = new ApplicationUser();
            //usersa.UserName = "SourceCodeAdmin";
            //usersa.Email = "NeedForSpeed5736315@gmail.com";

            //string userPWDsa = "@AdminSC@";

            //var chkUsersa = UserManager.Create(usersa, userPWDsa);

            //Add default User to Role Admin   
            //if (chkUsersa.Succeeded)
            //{
            //    var result1 = UserManager.AddToRole(usersa.Id, "SuperAdmin");

            //}

            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("SuperAdmin"))
            {

                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "SuperAdmin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "SourceCodeAdmin";
                user.Email = "NeedForSpeed5736315@gmail.com";

                string userPWD = "@AdminSC@";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "SuperAdmin");

                }
            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("Restaurant"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Restaurant";
                roleManager.Create(role);





            }
            // creating Creating Employee role    
            if (!roleManager.RoleExists("User"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "User";
                roleManager.Create(role);

                // need to create defaute user TODO Order

            }
        }


    }
}
