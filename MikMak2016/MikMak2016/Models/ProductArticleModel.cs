using MikMak2016.App_Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MikMak2016.Models
{
    public class ProductArticleModel
    {
        private List<App_Data.DAL.ProductArticle> items = new List<App_Data.DAL.ProductArticle>();
        public IEnumerable<ProductArticle> Items //collection van items productarticle aanmaken
        {
            get { return items; }
        }

        public void AddItem(Article article, int quantity) // item toevoegen en article en aantal meegeven
        {
            ProductArticle item = items.SingleOrDefault(p => p.Article.Id == article.Id);  // query om te zien of article er al in zit

            if (item == null) //indien nog niet
            {
                items.Add(new App_Data.DAL.ProductArticle
                    {
                        Article = article, // article en hoeveelheid worden toegevoegd
                        Quantity = quantity.ToString()
                    });
            }
            else
            {
                item.Quantity += quantity; //indien wel wordt hoeveelheid verhoogd
            }
        }

            public void RemoveItem (int Id)
            {
            items.RemoveAll(l => l.Article.Id==Id);
            }
        //public decimal GetArticleTotal()
        //{
        // Convert.ToDecimal(Article.StandardCost);
        //    Convert.ToDecimal(Article.Quantity);
        //    return items.Sum(e=>e.Article.StandardCost * e.Quantity);
        //}
        }
    }
