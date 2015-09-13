namespace FindWhere.Web.Controllers
{
    using Data;
    using FindWhere.Model;
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    public abstract class BaseController : Controller
    {
        protected FindWhereDbContext Context { get; set; }

        protected User CurrentUser { get; set; }

        public BaseController() : base()
        {
            this.Context = FindWhereDbContext.Create();
        }

        [ChildActionOnly]
        public RedirectToRouteResult RedirectWithSuccess(string controller, string action, string message)
        {
            TempData["success"] = message;

            return RedirectToAction(action, controller);
        }

        [ChildActionOnly]
        public RedirectToRouteResult RedirectWithError(string controller, string action, string message)
        {
            TempData["error"] = message;

            return RedirectToAction(action, controller);
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            this.CurrentUser = this.Context.Users.Where(u => u.UserName == requestContext.HttpContext.User.Identity.Name).FirstOrDefault();

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}