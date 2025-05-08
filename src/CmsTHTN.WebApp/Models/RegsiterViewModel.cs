using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CmsTHTN.WebApp.Models
{
    public class RegsiterViewModel
    {
        [Required(ErrorMessage = "Cần nhập HỌ của bạn")]
        [DisplayName("Họ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Cần nhập TÊN của bạn")]
        [DisplayName("Tên")]

        public string LastName { get; set; }

        [Required(ErrorMessage = "Cần nhập EMAIL của bạn")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email phải thuộc Gmail (@gmail.com)")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Cần nhập MẬT KHẨU của bạn")]
        [DisplayName("Mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Cần nhập XÁC NHẬN MẬT KHẨU của bạn")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp với mật khẩu đã nhập")]
        [DisplayName("Xác nhận mật khẩu")]
        public string ConfirmPassword { get; set; }
    }
}
