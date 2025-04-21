using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Repositories.Entities
{
    public class Report : BaseEntity
    {
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public Guid? BlogId { get; set; }
        public Guid? AppointmentId { get; set; }
        public Guid? CommentId { get; set; }
        public virtual User User { get; set; }
        public virtual Blog? Blog { get; set; }
        public virtual Appointment? Appointment { get; set; }
        public virtual Comment? Comment { get; set; }
    }
}
