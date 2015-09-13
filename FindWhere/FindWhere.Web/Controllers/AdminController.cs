namespace FindWhere.Web.Controllers
{
    using FindWhere.Web.Infrastructure.Filters;
    using System.Web.Mvc;

    [Log]
    [Authorize(Roles = "Admin")]
    public abstract class AdminController : BaseController
    {

    }
}