using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Repositories.Entities
{
    public class TimeSlot : BaseEntity
    {
        public string Time { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
