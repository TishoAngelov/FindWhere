namespace FindWhere.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    public class NeighborhoodsController : BaseController
    {
        [HttpGet]
        [ChildActionOnly]
        public ActionResult List(int? id)
        {
            var neighborhoods = this.Context.Neighbourhoods
                .AsQueryable()
                .Where(n => n.Locations.Count() > 0)
                .OrderBy(c => c.Name)
                .ToList();

            ViewBag.NeighborhoodId = id;

            return PartialView("_NeighborhoodsListSidebarPartial", neighborhoods);
        }
    }
}