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
        [DisplayName("Veuillez introduire vos coordonnées s.v.p.")]
        public string Login { get; set; }
        [DisplayName("Nom d'utilisateur")]
        public string Name { get; set; }
        [DisplayName("Mot de passe")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}