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
    public class ArticleTypeController : Controller
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

        // GET: /ArticleType/Create
        public ActionResult Create()
        {
            var at = db.ArticleType.OrderByDescending(a => a.Id).FirstOrDefault();
            var index = at.Id;
            index++;
            ViewBag.Id = index;
            return View();
        }

        // POST: /ArticleType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,Name,Id,InsertedBy,InsertedOn,UpdatedBy,UpdatedOn")] ArticleType articletype)
        {
            if (ModelState.IsValid)
            {
                db.ArticleType.Add(articletype);
                db.SaveChanges();
                TempData["message"] = "The articletype " + articletype.Name + " is created.";
                return RedirectToAction("Index");
            }

            return View(articletype);
        }

        public ActionResult Collect()
        {

            return View(db.ArticleType.ToList());
        }
        // GET: /ArticleType/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: /ArticleType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,Name,Id,InsertedBy,InsertedOn,UpdatedBy,UpdatedOn")] ArticleType articletype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articletype).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "The articletype " + articletype.Name + " is modified.";
                return RedirectToAction("Index");
            }
            return View(articletype);
        }

        // GET: /ArticleType/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: /ArticleType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArticleType articletype = db.ArticleType.Find(id);
            db.ArticleType.Remove(articletype);
            db.SaveChanges();
            TempData["message"] = "The articletype " + articletype.Name + " is removed.";
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
