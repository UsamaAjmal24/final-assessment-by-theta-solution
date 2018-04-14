using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalAssessment.Models
{
    public partial class Items
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please fill this field.")]
        [MaxLength(10)]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }
        [Required(ErrorMessage = "Please give discription")]
        [Display(Name = "Discription")]
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        [Required(ErrorMessage ="Please enter the Quantity.")]
        public double? Quantity { get; set; }
        [Required(ErrorMessage = "Please enter the right price.")]
        public double? Price { get; set; }
    }
}
