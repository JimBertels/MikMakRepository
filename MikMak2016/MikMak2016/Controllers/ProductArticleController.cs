using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MikMak2016.App_Data.DAL;
using System.IO;


namespace MikMak2016.Controllers
{
    public class ProductArticleController : Controller
    {
        private MikMak2016Entities db = new MikMak2016Entities();
        private const int PF = 8; // Id articletype PF

        // GET: /ProductArticle/

        public ActionResult Index(string searchBy, string search, string sortBy)
        {
            ViewBag.SortNameParameter = string.IsNullOrEmpty(sortBy) ? "Name desc" : "";
            ViewBag.SortNumberParameter = string.IsNullOrEmpty(sortBy) ? "Number desc" : "";

            //var articleTypes = db.ArticleType.AsQueryable();
            var articles = db.Article.Where(a => a.IdArticleType == PF).AsQueryable() ;

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
                case "Code desc":
                    articles = articles.OrderByDescending(x => x.Number);
                    break;
                case "Name desc":
                    articles = articles.OrderByDescending(x => x.Name);
                    break;
                default:
                    articles = articles.OrderBy(x => x.Number);
                    break;
            }

            if (TempData["message"] != null)
                ViewBag.Systeem = TempData["message"].ToString();

            return View(articles.ToList());
        }

        // GET: /ProductArticle/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productarticles = db.ProductArticle.Where(p => p.IdProduct == id).OrderBy(p=>p.Article.Name).OrderBy(p=>p.Article.Name).Include(a => a.Article).Include(a => a.Article.ArticleType);
            ViewBag.IdType = (db.ArticleType.OrderBy(a => a.Code)).ToList();
            var product = db.Article.Find(id);
            ViewBag.NameNrProd = product.Name + "-" + product.Number;

            if (productarticles.Count() < 1)
            {
                TempData["message"] = "System Message: No articles found in database.";
                return RedirectToAction("Index");
            }
            return View(productarticles.ToList());
        }

        //// GET: /ProductArticle/Create
        public ActionResult Create(int? id)
        {
            var pa = db.ProductArticle.OrderByDescending(a => a.Id).FirstOrDefault();
            var index = pa.Id;
            index++;
            ViewBag.Id = index;
            var productarticle = db.Article.Find(id);
            ViewBag.IdProd = id;
            ViewBag.ProdName = productarticle.Number + "-" + productarticle.Name;
            ViewBag.IdArticle = new SelectList(db.Article, "Id", "Name");
            return View();
        }

        // POST: /ProductArticle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductArticle productarticle)
        {
            if (ModelState.IsValid)
            {
                db.ProductArticle.Add(productarticle);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = productarticle.IdProduct });
            }
            // return welke list ?
            ViewBag.IdArticle = new SelectList(db.Article, "Id", "Number");
            return View(productarticle);
        }

        // GET: /ProductArticle/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var productarticles = db.ProductArticle.Where(p => p.IdProduct == id).Include(a => a.Article).OrderBy(p => p.Article.IdArticleType);
            if (productarticles == null)
            {
                return HttpNotFound();
            }
           
            var article = db.Article.Find(id);
            ViewBag.IdProd = id;
            ViewBag.ProdName = article.Number + "-" + article.Name;
            return View(productarticles);
        }

        [HttpGet]
        public ActionResult EditOne(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductArticle productarticle = db.ProductArticle.Find(id);
            var article = db.Article.Find(productarticle.IdProduct);
            ViewBag.IdProd = productarticle.IdProduct;
            ViewBag.ProdName = article.Number + "-" + article.Name;
            if (productarticle == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdArticle = new SelectList(db.Article, "Id", "Number", productarticle.IdArticle);
            return View(productarticle);
        }

        // POST: /ProductArticle/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]

        [HttpPost]
        public ActionResult EditOne([Bind(Include = "IdProduct,IdArticle,Id,Quantity,InsertedBy,InsertedOn,UpdatedBy,UpdatedOn")] ProductArticle productarticle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productarticle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = productarticle.IdProduct });
            }
            ViewBag.IdArticle = new SelectList(db.Article, "Id", "Number", productarticle.IdArticle);
            return View(productarticle);
        }

        // GET: /ProductArticle/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productarticles = db.ProductArticle.Where(p => p.IdProduct == id);
            @ViewBag.Id = id;
            if (productarticles.Count() < 1)
            {
                Article article = db.Article.Find(id);
                db.Article.Remove(article);
                db.SaveChanges();
                TempData["message"] = "System message: The article is removed.";
                return RedirectToAction("Index");
            }
            return View(productarticles.ToList());
        }

        //// POST: /ProductArticle/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {  
            var productarticles = db.ProductArticle.Where(p => p.IdProduct == id);
            Article article = db.Article.Find(id);
            db.Article.Remove(article);  
            foreach (var item in productarticles)
            {
                ProductArticle productarticle = db.ProductArticle.Find(item.Id);
                if (productarticle != null)
                {
                    db.ProductArticle.Remove(productarticle);
                }
            }
            db.SaveChanges();
            ViewBag.message = "The articles are removed.";
            return RedirectToAction("Index");
        }

        public ActionResult DeleteOne(int? id)
        {
            ProductArticle productarticle = db.ProductArticle.Find(id);
            if (productarticle != null)
            {
                var ProdId = productarticle.IdProduct;
                db.ProductArticle.Remove(productarticle);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = ProdId });
            }
            else
            {
                ViewBag.message = "No articles found in database.";
                return RedirectToAction("Edit");
            }
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
