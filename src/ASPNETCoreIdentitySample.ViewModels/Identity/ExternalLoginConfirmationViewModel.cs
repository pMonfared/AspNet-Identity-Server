﻿using System.ComponentModel.DataAnnotations;

namespace ASPNETCoreIdentitySample.ViewModels.Identity
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "(*)")]
        [EmailAddress(ErrorMessage = "لطفا آدرس ایمیل معتبری را وارد نمائید.")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
    }
}
