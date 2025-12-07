using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModels
{
    public class CreateMemberViewModel
    {
        [Required(ErrorMessage ="Photo Is Required")]
        [Display(Name = "Profile Photo")]
        public IFormFile Photo { get; set; } = null!;

        [Required(ErrorMessage ="Name Is Required !")]
        [StringLength(50, MinimumLength = 2, ErrorMessage ="Name Must Be Between 2 and 50 Chars !")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage ="Name Can Contain Only Letters !")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email Is Required !")]
        [EmailAddress(ErrorMessage ="Invalid Email Format !")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, MinimumLength =6, ErrorMessage ="Email Must Be Between 5 and 100 Chars !")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Is Required !")]
        [Phone(ErrorMessage = "Invalid Phone Format !")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^01[3-9]\d{8}$", ErrorMessage = "You Must Enter Bangladeshi Number Format !")]
        public string Phone { get; set; }= null!;

        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage ="Gender Is Required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Bulding Number Is Required")]
        [Range(1, 9000, ErrorMessage ="Bulding Number Must Be Greater Than 0")]
        public int BuldingNumber { get; set; }

        [Required(ErrorMessage = "Street Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street Must Be Bettwen 2 and 30")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City Is Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City Can Contain Only Letters !")]
        public string City { get; set; } = null!;


        // Health ViewModel
        public HealthViewModel HealthViewModel { get; set; } = null!;
    }
}
