using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModels
{
    public class MembersViewModel
    {
        public int Id{ get; set; }
        public string Name { get; set; } = null!;
        public string BookingDate { get; set; } = null!;
        public bool IsAttend { get; set; }
    }
}
