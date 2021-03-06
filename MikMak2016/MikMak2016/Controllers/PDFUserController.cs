﻿using MikMak2016.App_Data.DAL;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MikMak2016.Controllers
{
    public class PDFUserController : Controller
    {
        private MikMak2016Entities db = new MikMak2016Entities();

        public ActionResult PdfIndex()
        {
            return View();
        }

        public ActionResult GeneratePDF(string path)
        {
            return new Rotativa.ActionAsPdf(path);
        }
        
        public ActionResult UnitBase()
        {
            var unitbase = db.UnitBase.OrderBy(u => u.Code).OrderBy(u => u.Name);
            DateTime date = DateTime.Now;
            ViewBag.date = date;
            return View("PdfUnitBase", unitbase);
        }

        public ActionResult Supplier()
        {
            var supplier = db.Supplier.OrderBy(s => s.Country).OrderBy(s => s.Region).OrderBy(s => s.City).OrderBy(s => s.Code).OrderBy(s => s.Name);
            DateTime date = DateTime.Now;
            ViewBag.date = date;
            return View("PdfSupplier", supplier);
        }

        public ActionResult ArticleType()
        {
            var articletype = db.ArticleType.OrderBy(s => s.Code).OrderBy(s => s.Name);
            DateTime date = DateTime.Now;
            ViewBag.date = date;
            return View("PdfArticleType", articletype);
        }

        public ActionResult Article()
        {
            var article = db.Article.OrderBy(a => a.Number).OrderBy(a => a.Name);
            DateTime date = DateTime.Now;
            ViewBag.date = date;
            return View("PdfArticle", article);
        }

        public ActionResult ProductArticle()
        {
            var productarticle = db.ProductArticle.OrderBy(p => p.Article.Name).OrderBy(p => p.Article.ArticleType.Code).OrderBy(p => p.IdProduct).Include(a => a.Article).Include(a => a.Article.ArticleType);
            ViewBag.IdProd = (db.Article.Where(i => i.IdArticleType == 8).OrderBy(a => a.Name)).ToList();
            ViewBag.IdType = (db.ArticleType.OrderBy(a => a.Code)).ToList();
            DateTime date = DateTime.Now;
            ViewBag.date = date;
            return View("PdfProductArticle", productarticle);
        }
    }
}