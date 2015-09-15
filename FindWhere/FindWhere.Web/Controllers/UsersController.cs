namespace FindWhere.Web.Controllers
{
    using Common;
    using Model;
    using Microsoft.AspNet.Identity;
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class UsersController : AdminController
    {
        // GET: Users
        public ActionResult Index()
        {
            var users = this.Context.Users.ToList();

            return View(users);
        }
        
        // POST: Users/Search/text
        [HttpPost]
        public ActionResult Search(string query)
        {
            var result = this.Context.Users
                .AsQueryable()
                .Where(u => u.Email.Contains(query))
                .ToList();

            return PartialView("_UsersSearchPartial", result);
        }

        // POST: Users/Ban/Guid
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ban(string id)
        {
            var user = this.Context.Users.AsQueryable().FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return this.RedirectWithError("Users", "Index", "Something went wrong! Please try again later.");
            }

            var adminRole = this.Context.Roles.FirstOrDefault(r => r.Name == UserRoles.Admin);

            if (user.Roles.Any(r => r.RoleId == adminRole.Id))
            {
                return this.RedirectWithError("Users", "Index", "You cannot ban admins.");
            }

            user.IsBanned = true;

            this.Context.SaveChanges();

            return this.RedirectWithSuccess("Users", "Index", "User successfuly banned!");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakeModerator(string id)
        {
            var user = this.Context.Users.AsQueryable().FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return this.RedirectWithError("Users", "Index", "Something went wrong! Please try again later.");
            }

            var moderatorRole = this.Context.Roles.FirstOrDefault(r => r.Name == UserRoles.Moderator);

            if (!user.Roles.Any(r => r.RoleId == moderatorRole.Id) && user.IsBanned == false)
            {
                var userManager = new UserManager<User>(new UserStore<User>(this.Context));
                userManager.AddToRole(user.Id, UserRoles.Moderator);

                this.Context.SaveChanges();

                return this.RedirectWithSuccess("Users", "Index", "User added to role moderator!");
            }

            return this.RedirectWithError("Users", "Index", "The user is already in role moderator or is banned!");
        }
    }
}
