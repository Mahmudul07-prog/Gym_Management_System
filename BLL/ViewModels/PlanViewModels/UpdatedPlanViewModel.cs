using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModels.PlanViewModels
{
    public class UpdatedPlanViewModel
    {
        public string PlanName { get; set; } = null!;
        [Required(ErrorMessage ="Required !")]
        [StringLength(200, MinimumLength = 5, ErrorMessage ="You Must Enter Description in Range 5 : 200 Chars !")]
        public string PlanDescription { get; set; } = null!;
        [Required]
        [Range(1, 365, ErrorMessage = "You Must Be Greter Than 1 And Less Than 365")]
        public int DurationDays { get; set; }
        [Required]
        [Range(1, 100000, ErrorMessage = "You Must Be Greter Than 1")]
        public decimal Price { get; set; }
    }
}
