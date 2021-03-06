﻿namespace FindWhere.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    public class HomeController : BaseController
    {
        [OutputCache(Duration = 1)]
        public ActionResult Index()
        {
            var latestLocations = this.Context.Locations
                .Where(l => l.IsApproved)
                .Where(l => l.ShoppingCenter.IsClosed == false)
                .OrderByDescending(l => l.AddedOn)
                .Take(10)
                .ToList();

            return View(latestLocations);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }
    }
}