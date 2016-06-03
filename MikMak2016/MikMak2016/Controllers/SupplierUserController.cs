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
    public class SupplierUserController : Controller
    {
        private MikMak2016Entities db = new MikMak2016Entities();

        // GET: SupplierUser
        public ActionResult Index(string searchBy, string search, string sortBy)
        {
            ViewBag.SortNameParameter = string.IsNullOrEmpty(sortBy) ? "Name desc" : "";
            ViewBag.SortNumberParameter = string.IsNullOrEmpty(sortBy) ? "Code desc" : "";

            var supplierUsers = db.Supplier.AsQueryable();

            if (searchBy == "Code")
            {
                supplierUsers = supplierUsers.Where(x => x.Code.StartsWith(search) || search == null);
            }
            else
            {
                supplierUsers = supplierUsers.Where(x => x.Name.StartsWith(search) || search == null);
            }

            switch (sortBy)
            {
                case "Code desc":
                    supplierUsers = supplierUsers.OrderByDescending(x => x.Code);
                    break;
                case "Name desc":
                    supplierUsers = supplierUsers.OrderByDescending(x => x.Name);
                    break;
                default:
                    supplierUsers = supplierUsers.OrderBy(x => x.Code);
                    break;
            }
            return View(supplierUsers.ToList());
        }

        // GET: SupplierUser/Details/5
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
