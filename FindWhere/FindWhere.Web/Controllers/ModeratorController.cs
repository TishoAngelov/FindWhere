namespace FindWhere.Web.Controllers
{
    using Common;
    using FindWhere.Web.Infrastructure.Filters;
    using System.Web.Mvc;

    [Log]
    [Authorize(Roles = UserRoles.Admin)]
    public abstract class ModeratorController : BaseController
    {

    }
}