using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModels
{
    public class HealthViewModel
    {
        [Required(ErrorMessage = "Height is Required")]
        [Range(1, 300, ErrorMessage ="Height Must Be Greater Than 0")]
        public decimal Height { get; set; }
        [Required(ErrorMessage = "Weight is Required")]
        [Range(1, 700, ErrorMessage = "Weight Must Be Greater Than 0 and Less Than 700")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "BloodType is Required")]
        [StringLength(3, ErrorMessage = "Blood Type Must Be 3 Chars or Less")]
        public string BloodType { get; set; } = null!;
        public string? Note { get; set; }
    }
}
