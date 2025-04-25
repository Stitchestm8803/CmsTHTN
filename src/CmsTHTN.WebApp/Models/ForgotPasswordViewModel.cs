using System.ComponentModel.DataAnnotations;

namespace CmsTHTN.WebApp.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Bạn cần nhập Email")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email bạn nhập không đúng")]
        public string Email { get; set; }
    }
}
