using System.ComponentModel.DataAnnotations;

namespace CmsTHTN.WebApp.Models
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập {0}")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không đúng đúng dạng")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập {0}")]
        [StringLength(100, ErrorMessage = "{0} phải chứa ít nhất {2} ký tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không đúng")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
