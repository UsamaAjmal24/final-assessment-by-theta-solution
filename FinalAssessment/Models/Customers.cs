using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalAssessment.Models
{
    public partial class Customers
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please fill this field.")]
        [MaxLength(10)]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [RegularExpression(@"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$", ErrorMessage = "Invalid Email")]

        public string Email { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Please fill this field")]
        public string Phone { get; set; }
        public int? Status { get; set; }
        [Required(ErrorMessage = "Please enter the date when created")]
        public DateTime? CreatedDate { get; set; }
        [Required(ErrorMessage = "Please enter the name who created")]
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    }
}
