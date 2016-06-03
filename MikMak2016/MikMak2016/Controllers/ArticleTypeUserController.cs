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

        // GET: ArticleTypeUser
        
        public ActionResult Index(string searchBy, string search, string sortBy)
        {
            ViewBag.SortNameParameter = string.IsNullOrEmpty(sortBy) ? "Name desc" : "";
            ViewBag.SortNumberParameter = string.IsNullOrEmpty(sortBy) ? "Code desc" : "";

            var articleTypeUsers = db.ArticleType.AsQueryable();

            if (searchBy == "Code")
            {
                articleTypeUsers = articleTypeUsers.Where(x => x.Code.StartsWith(search) || search == null);
            }
            else
            {
                articleTypeUsers = articleTypeUsers.Where(x => x.Name.StartsWith(search) || search == null);
            }

            switch (sortBy)
            {
                case "Code desc":
                    articleTypeUsers = articleTypeUsers.OrderByDescending(x => x.Code);
                    break;
                case "Name desc":
                    articleTypeUsers = articleTypeUsers.OrderByDescending(x => x.Name);
                    break;
                default:
                    articleTypeUsers = articleTypeUsers.OrderBy(x => x.Code);
                    break;
            }
            return View(articleTypeUsers.ToList());
        }

        // GET: ArticleTypeUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleType articleType = db.ArticleType.Find(id);
            if (articleType == null)
            {
                return HttpNotFound();
            }
            return View(articleType);
        }

        // GET: ArticleTypeUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ArticleTypeUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,Name,Id,InsertedBy,InsertedOn,UpdatedBy,UpdatedOn")] ArticleType articleType)
        {
            if (ModelState.IsValid)
            {
                db.ArticleType.Add(articleType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(articleType);
        }

        // GET: ArticleTypeUser/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleType articleType = db.ArticleType.Find(id);
            if (articleType == null)
            {
                return HttpNotFound();
            }
            return View(articleType);
        }

        // POST: ArticleTypeUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,Name,Id,InsertedBy,InsertedOn,UpdatedBy,UpdatedOn")] ArticleType articleType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articleType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(articleType);
        }

        // GET: ArticleTypeUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleType articleType = db.ArticleType.Find(id);
            if (articleType == null)
            {
                return HttpNotFound();
            }
            return View(articleType);
        }

        // POST: ArticleTypeUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArticleType articleType = db.ArticleType.Find(id);
            db.ArticleType.Remove(articleType);
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
