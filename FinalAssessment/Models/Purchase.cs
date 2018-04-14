using System;
using System.Collections.Generic;

namespace FinalAssessment.Models
{
    public partial class Purchase
    {
        public int Id { get; set; }
        public int? ItemId { get; set; }
        public int? VendorId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public double? Quantity { get; set; }
        public double? PurchasePrice { get; set; }
        public string Remarks { get; set; }
    }
}
