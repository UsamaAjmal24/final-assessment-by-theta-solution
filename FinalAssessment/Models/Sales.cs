using System;
using System.Collections.Generic;

namespace FinalAssessment.Models
{
    public partial class Sales
    {
        public int Id { get; set; }
        public int? ItemId { get; set; }
        public int? CustomersId { get; set; }
        public DateTime? SaleDate { get; set; }
        public double? Quantity { get; set; }
        public double? Price { get; set; }
    }
}
