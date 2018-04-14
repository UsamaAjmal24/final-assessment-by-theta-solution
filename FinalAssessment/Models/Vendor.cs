using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalAssessment.Models
{
    public partial class Vendor
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please fill this field.")]
        [MaxLength(10)]
        [Display(Name = "Vendor Name")]
        public string VendorName { get; set; }
        [RegularExpression(@"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessage = "Please describe what you supply")]
        [Display(Name = "Discription")]
        public string Description { get; set; }
    }
}
