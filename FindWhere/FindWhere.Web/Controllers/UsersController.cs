namespace FindWhere.Web.Controllers
{
    using Common;
    using System.Linq;
    using System.Web.Mvc;

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
    }
}
