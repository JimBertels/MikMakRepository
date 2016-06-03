using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MikMak2016.App_Data.DAL;

namespace MikMak2016.Controllers
{
    public class SupplierController : Controller
    {
        private MikMak2016Entities db = new MikMak2016Entities();

        // GET: /Supplier/
        public ActionResult Index(string searchBy, string search, string sortBy)
        {
            ViewBag.SortNameParameter = string.IsNullOrEmpty(sortBy) ? "Name desc" : "";
            ViewBag.SortNumberParameter = string.IsNullOrEmpty(sortBy) ? "Code desc" : "";

            var suppliers = db.Supplier.AsQueryable();

            if (searchBy == "Code")
            {
                suppliers = suppliers.Where(x => x.Code.StartsWith(search) || search == null);
            }
            else
            {
                suppliers = suppliers.Where(x => x.Name.StartsWith(search) || search == null);
            }

            switch (sortBy)
            {
                case "Code desc":
                    suppliers = suppliers.OrderByDescending(x => x.Code);
                    break;
                case "Name desc":
                    suppliers = suppliers.OrderByDescending(x => x.Name);
                    break;
                default:
                    suppliers = suppliers.OrderBy(x => x.Code);
                    break;
            } 

            if (TempData["message"] != null)
                ViewBag.Systeem = TempData["message"].ToString();

            return View(suppliers.ToList());
        }

        // GET: /Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Supplier.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: /Supplier/Create
        public ActionResult Create()
        {
            var sup = db.Supplier.OrderByDescending(s => s.Id).FirstOrDefault();
            var index = sup.Id;
            index++;
            ViewBag.Id = index;
            return View();
        }

        // POST: /Supplier/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,Name,Contact,Address,City,Region,PostalCode,Country,Phone,Mobile,Id,InsertedBy,InsertedOn,UpdatedBy,UpdatedOn")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Supplier.Add(supplier);
                db.SaveChanges();
                TempData["message"] = "The supplier " + supplier.Name + " is created.";
                return RedirectToAction("Index");
            }

            return View(supplier);
        }

        // GET: /Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Supplier.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: /Supplier/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,Name,Contact,Address,City,Region,PostalCode,Country,Phone,Mobile,Id,InsertedBy,InsertedOn,UpdatedBy,UpdatedOn")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "The supplier " + supplier.Name + " is modified.";
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        // GET: /Supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Supplier.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: /Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = db.Supplier.Find(id);
            db.Supplier.Remove(supplier);
            db.SaveChanges();
            TempData["message"] = "The supplier " + supplier.Name + " is removed.";
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
