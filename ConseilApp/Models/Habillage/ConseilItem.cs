using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConseilApp.Models.Habillage
{
    public class ConseilItem
    {
        public int Id { get; set; }
        public int PersonneId { get; set; }
        public string Pseudo { get; set; }
        public string Date { get; set; }
        public int NbHabillage { get; set; }

        public ConseilItem(int Id, int PersonneId, string Pseudo, string Date, int NbHabillage)
        {
            this.Id = Id;
            this.PersonneId = PersonneId;
            this.Pseudo = Pseudo;
            this.Date = Date;
            this.NbHabillage = NbHabillage;
        }
    }
}