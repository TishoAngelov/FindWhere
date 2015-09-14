namespace FindWhere.Web.Controllers
{
    using Common;
    using FindWhere.Data;
    using FindWhere.Model;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    public class LocationsController : BaseController
    {
        private FindWhereDbContext db = new FindWhereDbContext();

        // GET: Locations/Page/3
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

        // GET: Locations/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            ViewBag.NeighbourhoodId = new SelectList(db.Neighbourhoods, "Id", "Name");
            ViewBag.Id = new SelectList(db.ShoppingCenters, "Id", "WorkTime");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Latitude,Longitude,FullAddress,PlaceId,IsApproved,Rating,AddedOn,ModifiedOn,UserId,NeighbourhoodId")] Location location)
        {
            if (ModelState.IsValid)
            {
                location.Id = Guid.NewGuid();
                db.Locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.NeighbourhoodId = new SelectList(db.Neighbourhoods, "Id", "Name", location.NeighbourhoodId);
            ViewBag.Id = new SelectList(db.ShoppingCenters, "Id", "WorkTime", location.Id);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", location.UserId);
            return View(location);
        }

        // GET: Locations/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            ViewBag.NeighbourhoodId = new SelectList(db.Neighbourhoods, "Id", "Name", location.NeighbourhoodId);
            ViewBag.Id = new SelectList(db.ShoppingCenters, "Id", "WorkTime", location.Id);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", location.UserId);
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Latitude,Longitude,FullAddress,PlaceId,IsApproved,Rating,AddedOn,ModifiedOn,UserId,NeighbourhoodId")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NeighbourhoodId = new SelectList(db.Neighbourhoods, "Id", "Name", location.NeighbourhoodId);
            ViewBag.Id = new SelectList(db.ShoppingCenters, "Id", "WorkTime", location.Id);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", location.UserId);
            return View(location);
        }

        // GET: Locations/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
