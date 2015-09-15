﻿namespace FindWhere.Web.Controllers
{
    using Common;
    using FindWhere.Data;
    using FindWhere.Model;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    public class LocationsController : BaseController
    {
        // GET: Locations/Index/3
        public ActionResult Index(int id = 1)
        {
            var locations = this.Context.Locations
                .Where(l => l.IsApproved)
                .Where(l => l.ShoppingCenter.IsClosed == false)
                .OrderBy(l => l.AddedOn)
                .Skip((id - 1) * GlobalConstants.LocationsPerPage)
                .Take(GlobalConstants.LocationsPerPage)
                .ToList();

            var locationsCount = this.Context.Locations
                .Where(l => l.IsApproved)
                .Where(l => l.ShoppingCenter.IsClosed == false)
                .Count();

            ViewBag.Pages = locationsCount / GlobalConstants.LocationsPerPage + 1;
            ViewBag.CurrentPage = id;

            if (locationsCount % GlobalConstants.LocationsPerPage == 0) ViewBag.Pages--;

            if (id <= 0)
            {
                ViewBag.CurrentPage = 1;
            }

            if (id >= ViewBag.Pages)
            {
                ViewBag.CurrentPage = ViewBag.Pages;
            }

            //var locations = db.Locations.Include(l => l.Neighbourhood).Include(l => l.ShoppingCenter).Include(l => l.User);
            return View(locations);
        }

        // GET: Locations/Details/GuidId
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Location location = this.Context.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }

            // Prevent the normal user to see not approved locations.
            if (location.IsApproved == false && User.IsInRole(UserRoles.User))
            {
                return Redirect("Index");
            }

            return View(location);
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult SimilarByNeighbourhood(int id)
        {
            var locationsByNeighbourhood = this.Context.Locations
                .AsQueryable()
                .Where(l => l.IsApproved)
                .Where(l => l.ShoppingCenter.IsClosed == false)
                .Where(l => l.Neighbourhood.Id == id)
                .OrderBy(r => Guid.NewGuid())       // Random records
                .Take(4)
                .ToList();

            return PartialView("_SimilarLocationsPartial", locationsByNeighbourhood);
        }

        // GET: Categories/NotApproved
        [HttpGet]
        [Authorize(Roles = UserRoles.Moderator)]
        public ActionResult NotApproved()
        {
            var notApprovedLocation = this.Context.Locations
                .AsQueryable()
                .Where(l => l.IsApproved == false)
                .OrderByDescending(l => l.AddedOn)
                .ToList();

            return View(notApprovedLocation);
        }

        // Get: Categories/Approve/GuidId
        [HttpGet]
        [Authorize(Roles = UserRoles.Moderator)]
        public ActionResult Approve(Guid? id)
        {
            if (id == null)
            {
                this.RedirectWithError("Locations", "NotApproved", "Please select a location to be approved first!");
            }

            var location = this.Context.Locations.Find(id);
            if (location == null)
            {
                this.RedirectWithError("Locations", "NotApproved", "Something went wrong! The location does not exists.");
            }

            location.IsApproved = true;
            location.User.ApprovedLocationsCount++;

            var msg = "Location successfuly approved!";

            // Make the user moderator if he reaches the needed number of approved locations for that role.
            if (User.IsInRole(UserRoles.User) && location.User.ApprovedLocationsCount >= GlobalConstants.NeededLocationsToBecomeModerator)
            {
                var userManager = new UserManager<User>(new UserStore<User>(this.Context));
                userManager.AddToRole(location.User.Id, UserRoles.Moderator);
                msg += "\r\nCongratulations! You have reached the needed count of approved locations and you have become a Moderator!";
            }

            this.Context.SaveChanges();

            return RedirectWithSuccess("Locations", "NotApproved", msg);
        }

        // GET: Locations/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(this.Context.Categories, "Id", "Name");

            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        // [Bind(Include = "Id,Latitude,Longitude,FullAddress,PlaceId,IsApproved,Rating,AddedOn,ModifiedOn,UserId,NeighbourhoodId")] 
        public ActionResult Create(Location location)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = new SelectList(this.Context.Categories, "Id", "Name", location.ShoppingCenter.CategoryId);

                return View(location);
            }

            // TODO
            //location.Id = Guid.NewGuid();
            //this.Context.Locations.Add(location);

            //this.Context.SaveChanges();

            return this.RedirectWithSuccess("Locations", "Index", "New location successfuly added!");
        }

        // GET: Locations/Edit/GuidId
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Location location = this.Context.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }

            ViewBag.NeighbourhoodId = new SelectList(this.Context.Neighbourhoods, "Id", "Name", location.NeighbourhoodId);
            ViewBag.Id = new SelectList(this.Context.ShoppingCenters, "Id", "WorkTime", location.Id);
            ViewBag.UserId = new SelectList(this.Context.Users, "Id", "Email", location.UserId);

            return View(location);
        }

        // POST: Locations/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Latitude,Longitude,FullAddress,PlaceId,IsApproved,Rating,AddedOn,ModifiedOn,UserId,NeighbourhoodId")] Location location)
        {
            if (ModelState.IsValid)
            {
                this.Context.Entry(location).State = EntityState.Modified;
                this.Context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.NeighbourhoodId = new SelectList(this.Context.Neighbourhoods, "Id", "Name", location.NeighbourhoodId);
            ViewBag.Id = new SelectList(this.Context.ShoppingCenters, "Id", "WorkTime", location.Id);
            ViewBag.UserId = new SelectList(this.Context.Users, "Id", "Email", location.UserId);

            return View(location);
        }

        // GET: Locations/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Location location = this.Context.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Location location = this.Context.Locations.Find(id);

            this.Context.Locations.Remove(location);
            this.Context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}