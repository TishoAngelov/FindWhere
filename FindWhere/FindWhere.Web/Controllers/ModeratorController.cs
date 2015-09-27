namespace FindWhere.Web.Controllers
{
    using Common;
    using FindWhere.Web.Infrastructure.Filters;
    using System.Web.Mvc;

    [Log]
    [Authorize(Roles = UserRoles.Moderator)]
    public abstract class ModeratorController : BaseController
    {

    }
}