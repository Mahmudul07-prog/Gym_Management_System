using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModels
{
    public class CreatedMemberShipViewModel
    {
        [Required(ErrorMessage ="You Must Chose Member")]
        public int MemberId { get; set; }
        [Required(ErrorMessage ="You Must Chose Plan")]
        public int PlanId { get; set; }
    }
}
