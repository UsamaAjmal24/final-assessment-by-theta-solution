using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalAssessment.Models
{
    public partial class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please fill this field.")]
        [MaxLength(10)]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
        [Required(ErrorMessage ="Please give discription")]
        [Display(Name ="Discription")]
        public string Description { get; set; }
        public int? Status { get; set; }
        [Required(ErrorMessage = "Please enter the date when created")]
        public DateTime? CreatedDate { get; set; }
        [Required(ErrorMessage = "Please enter the name who created")]
        public string CreatedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    }
}
