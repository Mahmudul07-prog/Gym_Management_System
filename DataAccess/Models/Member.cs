using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Member : GymUser
    {
        public string Photo { get; set; } = null!;


        #region 1 : 1 Between Member HealthRecord
        public HealthRecord HealthRecord { get; set; }
        #endregion

        public ICollection<MemberShip> MemberShips { get; set; } = new HashSet<MemberShip>();
        public ICollection<MemberSession> MemberSessions { get; set; } = new HashSet<MemberSession>();
    }
}
