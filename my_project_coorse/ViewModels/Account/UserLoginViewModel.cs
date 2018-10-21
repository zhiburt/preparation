using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace preparation.ViewModels.Account
{
    public class UserLoginViewModel
    {
            [Required]
            [Display(Name = "Email")]
            public string Email { get; set; }
            
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Must we remember you?")]
            public bool RememberMe { get; set; }

            public string ReturnUrl { get; set; }
    }
}
