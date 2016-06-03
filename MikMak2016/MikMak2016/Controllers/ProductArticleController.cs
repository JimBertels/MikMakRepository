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
    public class ProductArticleController : Controller
    {
        private MikMak2016Entities db = new MikMak2016Entities();

        // GET: /ProductArticle/
        public ActionResult Index()
        {
            //var productarticle = db.ProductArticle.Include(p => p.Article);
            var productarticle = db.Article.Where(p => p.IdArticleType == 8);
            return View(productarticle.ToList());
        }

        // GET: /ProductArticle/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductArticle productarticle = db.ProductArticle.Find(id);
            if (productarticle == null)
            {
                return HttpNotFound();
            }
            return View(productarticle);
        }

        // GET: /ProductArticle/Create
        public ActionResult Create()
        {
            ViewBag.IdArticle = new SelectList(db.Article, "Id", "Number");
            return View();
        }

        // POST: /ProductArticle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="IdProduct,IdArticle,Id,Quantity,InsertedBy,InsertedOn,UpdatedBy,UpdatedOn")] ProductArticle productarticle)
        {
            if (ModelState.IsValid)
            {
                db.ProductArticle.Add(productarticle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdArticle = new SelectList(db.Article, "Id", "Number", productarticle.IdArticle);
            return View(productarticle);
        }

        // GET: /ProductArticle/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductArticle productarticle = db.ProductArticle.Find(id);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="IdProduct,IdArticle,Id,Quantity,InsertedBy,InsertedOn,UpdatedBy,UpdatedOn")] ProductArticle productarticle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productarticle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
            ProductArticle productarticle = db.ProductArticle.Find(id);
            if (productarticle == null)
            {
                return HttpNotFound();
            }
            return View(productarticle);
        }

        // POST: /ProductArticle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductArticle productarticle = db.ProductArticle.Find(id);
            db.ProductArticle.Remove(productarticle);
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
