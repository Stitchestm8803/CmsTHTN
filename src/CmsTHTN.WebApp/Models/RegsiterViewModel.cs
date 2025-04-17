using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CmsTHTN.WebApp.Models
{
    public class RegsiterViewModel
    {
        [Required(ErrorMessage = "Cần nhập HỌ của bạn")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Cần nhập TÊN của bạn")]
        [DisplayName("Last Name")]

        public string LastName { get; set; }

        [Required(ErrorMessage = "Cần nhập EMAIL của bạn")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Cần nhập MẬT KHẨU của bạn")]
        [DisplayName("Password")]
        public string Password { get; set; }
    }
}
