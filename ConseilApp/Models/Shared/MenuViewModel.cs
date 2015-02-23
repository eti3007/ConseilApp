using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConseilApp.Models
{
    public class MenuViewModel
    {
        public string Page { get; set; }
        public List<Menu> Menus { get; set; }
    }
    public class Menu
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string PageName { get; set; }
        public string Texte { get; set; }
    }
}