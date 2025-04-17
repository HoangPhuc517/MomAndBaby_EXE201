using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Repositories.Entities
{
    public class UserPackage
    {
        public DateTimeOffset ExpiryDate { get; set; }
        public int ValidMonths { get; set; }
        public Guid UserId { get; set; }
        public Guid ServicePackageId { get; set; }
        public virtual User User { get; set; }
        public virtual ServicePackage ServicePackage { get; set; }
    }
}
