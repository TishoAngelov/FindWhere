﻿using FindWhere.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FindWhere.Web.Controllers
{
    public class HomeController : BaseController
    {
        private FindWhereDbContext context = FindWhereDbContext.Create();

        [OutputCache(Duration = 1)]
        public ActionResult Index()
        {
            var c = context.Countries.ToList();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Faq()
        {
            ViewBag.Message = "Your Faq page.";

            return View();
        }
    }
}