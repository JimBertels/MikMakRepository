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
    public class ArticleTypeUserController : Controller
    {
        private MikMak2016Entities db = new MikMak2016Entities();

        // GET: /ArticleType/
        public ActionResult Index(string searchBy, string search, string sortBy)
        {
            ViewBag.SortNameParameter = string.IsNullOrEmpty(sortBy) ? "Name desc" : "";
            ViewBag.SortNumberParameter = string.IsNullOrEmpty(sortBy) ? "Code desc" : "";

            var articleTypes = db.ArticleType.AsQueryable();

            if (searchBy == "Code")
            {
                articleTypes = articleTypes.Where(x => x.Code.StartsWith(search) || search == null);
            }
            else
            {
                articleTypes = articleTypes.Where(x => x.Name.StartsWith(search) || search == null);
            }

            switch (sortBy)
            {
                case "Code desc":
                    articleTypes = articleTypes.OrderByDescending(x => x.Code);
                    break;
                case "Name desc":
                    articleTypes = articleTypes.OrderByDescending(x => x.Name);
                    break;
                default:
                    articleTypes = articleTypes.OrderBy(x => x.Code);
                    break;
            }

            if (TempData["message"] != null)
                ViewBag.Systeem = TempData["message"].ToString();

            return View(articleTypes.ToList());
        }

        // GET: /ArticleType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleType articletype = db.ArticleType.Find(id);
            if (articletype == null)
            {
                return HttpNotFound();
            }
            return View(articletype);
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
