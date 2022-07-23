using System;
using SalesWebMvc.Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;


namespace SalesWebMvc.Models {
    public class SalesRecord {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double Amount { get; set; }
        public SaleStatus Status { get; set; }
        public Seller Seller { get; set; }
        public int SellerId { get; set; }
        public SalesRecord() { }

        public SalesRecord(int id, DateTime date, double amount, SaleStatus status, Seller seller, int sellerId) { 
            this.Id = id;
            this.Date = date;
            this.Amount = amount;
            this.Status = status;
            this.Seller = seller;
            this.SellerId = sellerId;
        }
    }
}
