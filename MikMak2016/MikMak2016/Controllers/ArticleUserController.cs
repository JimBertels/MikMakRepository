using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MikMak2016.App_Data.DAL;
using PagedList;
using PagedList.Mvc;

namespace MikMak2016.Controllers
{
    public class ArticleUserController : Controller
    {
        private MikMak2016Entities db = new MikMak2016Entities();
        private static int aantalPages = 10;

        // GET: /ArticleUser/
        public ActionResult Index(string searchBy, string search, string sortBy, int? page) //int? = nullable --> bij eerste bezoek pagina nog nul
        {
            ViewBag.SortNameParameter = string.IsNullOrEmpty(sortBy) ? "Name desc" : "";
            ViewBag.SortNumberParameter = string.IsNullOrEmpty(sortBy) ? "Number desc" : "";

            //var article = db.Article.Include(a => a.ArticleType).Include(a => a.Supplier).Include(a => a.UnitBase);
            var articles = db.Article.AsQueryable();

            if (searchBy == "Number")
            {
                articles = articles.Where(x => x.Number.StartsWith(search) || search == null);
            }
            else
            {
                articles = articles.Where(x => x.Name.StartsWith(search) || search == null);
            }

            switch (sortBy)
            {
                case "Number desc":
                    articles = articles.OrderByDescending(x => x.Number);
                    break;
                case "Name desc":
                    articles = articles.OrderByDescending(x => x.Name);
                    break;
                default:
                    articles = articles.OrderBy(x => x.Number);
                    break;
            }

            int pageSize = aantalPages;
            int pageNumber = (page ?? 1);
            if (TempData["message"] != null)
                ViewBag.Systeem = TempData["message"].ToString();

            return View(articles.ToList().ToPagedList(pageNumber, pageSize)); //page ?? 1 = als het 0 is gebruik 1. 2e parameter is aantal items/pagina
        }


        // GET: /Article/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Article.Find(id);
            if (article.IdArticleType == 8)
                ViewBag.Idproductarticle = db.ProductArticle.Where(art => art.IdArticle == id);

            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
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