namespace FindWhere.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    public class NeighborhoodsController : BaseController
    {
        [HttpGet]
        [ChildActionOnly]
        public ActionResult List()
        {
            var neighborhoods = this.Context.Neighbourhoods
                .AsQueryable()
                .Where(n => n.Locations.Count() > 0)
                .OrderBy(c => c.Name)
                .ToList();

            return PartialView("_NeighborhoodsListSidebarPartial", neighborhoods);
        }
    }
}