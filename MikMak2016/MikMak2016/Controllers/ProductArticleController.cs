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

        
        public ProductArticleController()
        {
        }
        public ViewResult Index()
        {
            return View(new  Models.ProductArticleModel
                {
                    Pa = GetPa()

                });
        }
            public RedirectToRouteResult AddToPa(int Id)
            {
            Article article = db.Article.SingleOrDefault(p=>p.Id==Id);

                if(article != null)
                {
                GetPa().AddItem(article,1);
                }
                return RedirectToAction("ProductArticle");
            }
        public RedirectToRouteResult RemoveFromPa(int Id)
        {
            GetPa().RemoveItem(Id);
            return RedirectToAction("ProductArticle");
        }
        private Models.ProductArticleModel GetPa()
        {
        Models.ProductArticleModel Pa = (Models.ProductArticleModel)Session["Pa"];
            if(Pa == null)
            {
            Pa = new Models.ProductArticleModel();
                Session["Pa"] = Pa;
            }
            return Pa;
        }

        }

        
        
}
