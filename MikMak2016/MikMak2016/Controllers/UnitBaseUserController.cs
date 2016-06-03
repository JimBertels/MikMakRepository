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
    public class UnitBaseUserController : Controller
    {
        private MikMak2016Entities db = new MikMak2016Entities();

        // GET: UnitBaseUser
        public ActionResult Index(string searchBy, string search, string sortBy)
        {
            ViewBag.SortNameParameter = string.IsNullOrEmpty(sortBy) ? "Name desc" : "";
            ViewBag.SortCodeParameter = string.IsNullOrEmpty(sortBy) ? "Code desc" : "";

            var unitbaseusers = db.UnitBase.AsQueryable();

            if (searchBy == "Code")
            {
                unitbaseusers = unitbaseusers.Where(x => x.Code.StartsWith(search) || search == null);
            }
            else
            {
                unitbaseusers = unitbaseusers.Where(x => x.Name.StartsWith(search) || search == null);
            }

            switch (sortBy)
            {
                case "Code desc":
                    unitbaseusers = unitbaseusers.OrderByDescending(x => x.Code);
                    break;
                case "Name desc":
                    unitbaseusers = unitbaseusers.OrderByDescending(x => x.Name);
                    break;
                default:
                    unitbaseusers = unitbaseusers.OrderBy(x => x.Code);
                    break;
            }
            return View(unitbaseusers.ToList());
        }

        // GET: UnitBaseUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitBase unitBase = db.UnitBase.Find(id);
            if (unitBase == null)
            {
                return HttpNotFound();
            }
            return View(unitBase);
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
