using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GamesPurchases.Models
{
    public class Purchase
    {
        public int PurchaseID { get; set; }             // Primary Key
        public string PurchaseUser { get; set; }        // Foreign Key, Email of the User (Based from ASP.NET AccountUsers)
        public int PurchaseGame { get; set; }           // Foreign Key, ID of the Game
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }
        [DataType(DataType.Currency)]
        public double PurchaseCost { get; set; }
        public bool Refunded { get; set; }
    }
}
