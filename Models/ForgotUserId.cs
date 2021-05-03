using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ForgotUserId
    {
        [Display(Name = "Contact Number")]
        [Required(ErrorMessage = "Contact Number Required")]
        [MinLength(10, ErrorMessage = "Minimum 10 digits")]
        [DataType(DataType.PhoneNumber)]
        public long ContactNumber { get; set; }

        [Display(Name = "Favourite Song")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Answer Required")]
        public string Ques1 { get; set; }

        [Display(Name = "Favourite Color")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Answer Required")]
        public string Ques2 { get; set; }

        [Display(Name = "Favourite Pet")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Answer Required")]
        public string Ques3 { get; set; }
    }
}