using DataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Trainer : GymUser
    {
        public Specialites Specialites { get; set; }

        public ICollection<Session> TrainerSessions { get; set; } = new HashSet<Session>();
    }
}
