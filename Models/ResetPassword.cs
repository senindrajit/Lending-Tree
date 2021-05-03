using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ResetPassword
    {
        [Display(Name = "Enter New Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "No Password Entered")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum Length shoule be 6 characters")]
        public string NewPassword { get; set; }
    }
}