﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gabriel.Models
{
    public class SignupViewModel
    {
        [Required(ErrorMessage ="Username is required")]
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; }

    }
}
