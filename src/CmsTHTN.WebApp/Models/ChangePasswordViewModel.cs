using CmsTHTN.WebApp.Extensions;
using System.ComponentModel.DataAnnotations;

namespace CmsTHTN.WebApp.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Cần nhập mật khẩu cũ")]
        public required string OldPassword { get; set; }

        [Required(ErrorMessage = "Cần nhập mật khẩu mới")]
        public required string NewPassword { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu")]
        [PasswordMatch("NewPassword", ErrorMessage = "Xác nhận mật khẩu không trùng khớp")]
        public required string ConfirmPassword { get; set; }
    }
}
