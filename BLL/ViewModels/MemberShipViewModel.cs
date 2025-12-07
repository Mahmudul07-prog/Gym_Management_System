using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModels
{
    public class MemberShipViewModel
    {
        public int MemberId { get; set; }
        public int PlanId { get; set; }
        public string Member { get; set; } = null!;
        public string Plan { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
