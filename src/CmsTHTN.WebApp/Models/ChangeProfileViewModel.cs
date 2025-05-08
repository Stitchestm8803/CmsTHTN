using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CmsTHTN.WebApp.Models
{
    public class ChangeProfileViewModel
    {
        [Required(ErrorMessage = "Phải nhập HỌ của bạn")]
        [DisplayName("Họ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Phải nhập TÊN của bạn")]
        [DisplayName("Tên")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phải nhập ngày sinh của bạn")]
        [DisplayName("Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? Dob { get; set; }

        [Required(ErrorMessage = "Phải nhập SỐ ĐIỆN THOẠI của bạn")]
        [DisplayName("Số điện thoại")]
        public string PhoneNumber { get; set; }

    }
}
