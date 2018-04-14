using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalAssessment.Models
{
    public partial class Users
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please fill this field.")]
        [MaxLength(10)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; }
        public string Role { get; set; }
        public int? Status { get; set; }
    }
}
