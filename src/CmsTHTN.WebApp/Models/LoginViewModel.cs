using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CmsTHTN.WebApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Cần nhập EMAIL của bạn")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Cần nhập MẬT KHẨU của bạn")]
        [DisplayName("Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
