using System;
using System.Collections.Generic;
using ConseilApp.Builders.Interfaces;

namespace ConseilApp.Builders
{
    public class MenuBuilder : IMenuBuilder
    {
        public string[] GetControllerAction(string pageName)
        {
            string[] result = new string[2];
            result[0] = "Home"; result[1] = "Index";

            if (pageName.Equals("Login")) { result[0] = "Home"; result[1] = "Index"; }
            if (pageName.Equals("Register")) { result[0] = "Home"; result[1] = "Index"; }
            if (pageName.Equals("About")) { result[0] = "Home"; result[1] = "About"; }
            if (pageName.Equals("Contact")) { result[0] = "Home"; result[1] = "Contact"; }
            if (pageName.Equals("Proposition")) { result[0] = "Recherche"; result[1] = "Index"; }
            if (pageName.Equals("Demande")) { result[0] = "Recherche"; result[1] = "Index"; }

            return result;
        }
    }
}