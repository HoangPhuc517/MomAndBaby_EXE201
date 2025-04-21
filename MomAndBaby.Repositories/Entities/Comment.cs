using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Repositories.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public Guid BlogId { get; set; }
        public virtual User User { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
