using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModels
{
    public class CreatedSessionScheduleViewModel
    {
        [Required]
        public int MemberId { get; set; }
        [Required]
        public int SessionId { get; set; }
    }
}
