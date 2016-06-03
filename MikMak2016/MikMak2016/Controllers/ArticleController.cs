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
    public class ArticleController : Controller
    {
        private MikMak2016Entities db = new MikMak2016Entities();
        private static int aantalPages = 10;
  
        // GET: /Article/
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
                case "Number desc" :
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

        // GET: /Article/Create
        [HttpGet]
        public ActionResult Create()
        {
            var art = db.Article.OrderByDescending(a => a.Id).FirstOrDefault();
            var index = art.Id;
            index++;
            ViewBag.Id = index;
            ViewBag.IdArticle = new SelectList(db.Article, "Id", "Code");
            ViewBag.IdArticleType = new SelectList(db.ArticleType, "Id", "Code");
            ViewBag.IdSupplier = new SelectList(db.Supplier, "Id", "Code");
            ViewBag.IdUnitBase = new SelectList(db.UnitBase, "Id", "Code");

            return View();
        }

        // POST: /Article/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Number,StandardCost,Name,Breadth,GrossWeight,IdArticleType,RestockingTerm,IdUnitBase,UnitPrice,IdSupplier,Image,Id,InsertedBy,InsertedOn,UpdatedBy,UpdatedOn")] Article article)
        {
            string img = article.Image;
            if (!String.IsNullOrEmpty(img))
            {
                String[] imgName = img.Split('\\');
                article.Image = imgName[imgName.Count() - 1];
            }
            if (ModelState.IsValid)
            {
                db.Article.Add(article);
                db.SaveChanges();
                TempData["message"] = "The article " + article.Name + " is created.";
                if (article.IdArticleType == 8)
                {
                    ViewBag.IdArticleType = new SelectList(db.ArticleType, "Id", "Code", article.IdArticleType);
                    ViewBag.IdSupplier = new SelectList(db.Supplier, "Id", "Code", article.IdSupplier);
                    ViewBag.IdUnitBase = new SelectList(db.UnitBase, "Id", "Code", article.IdUnitBase);
                    return RedirectToAction("Edit", "ProductArticle", new { id = article.Id });
                }
                else return RedirectToAction("Index");
            }

            ViewBag.IdArticleType = new SelectList(db.ArticleType, "Id", "Code", article.IdArticleType);
            ViewBag.IdSupplier = new SelectList(db.Supplier, "Id", "Code", article.IdSupplier);
            ViewBag.IdUnitBase = new SelectList(db.UnitBase, "Id", "Code", article.IdUnitBase);
            return View(article);
        }

        // GET: /Article/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Article.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdArticleType = new SelectList(db.ArticleType, "Id", "Code", article.IdArticleType);
            ViewBag.IdSupplier = new SelectList(db.Supplier, "Id", "Code", article.IdSupplier);
            ViewBag.IdUnitBase = new SelectList(db.UnitBase, "Id", "Code", article.IdUnitBase);
            return View(article);
        }

        // POST: /Article/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Number,StandardCost,Name,Breadth,GrossWeight,IdArticleType,RestockingTerm,IdUnitBase,UnitPrice,IdSupplier,Image,Id,InsertedBy,InsertedOn,UpdatedBy,UpdatedOn")] Article article)
        {
            string img = article.Image;
            String[] imgName = img.Split('\\');
            article.Image = imgName[imgName.Count() - 1];

            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "The article " + article.Name + " is modified.";
                return RedirectToAction("Index");
            }

            ViewBag.IdArticleType = new SelectList(db.ArticleType, "Id", "Code", article.IdArticleType);
            ViewBag.IdSupplier = new SelectList(db.Supplier, "Id", "Code", article.IdSupplier);
            ViewBag.IdUnitBase = new SelectList(db.UnitBase, "Id", "Code", article.IdUnitBase);
            return View(article);
        }

        // GET: /Article/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Article.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: /Article/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Article.Find(id);
            db.Article.Remove(article);

            var art = db.ProductArticle.Where(pa => pa.IdArticle == id);
            foreach (var item in art)
            {
                db.ProductArticle.Remove(item);
            }

            db.SaveChanges();
            TempData["message"] = "The article " + article.Name + " is removed.";
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