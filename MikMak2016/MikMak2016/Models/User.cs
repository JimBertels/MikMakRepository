using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MikMak2016.Models
{
    public class User
    {
        
        //[Display(Name = "Nom")]
        [DisplayName("Connexion")]
        public string Login { get; set; }
        [DisplayName("Nom")]
        public string Name { get; set; }
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}