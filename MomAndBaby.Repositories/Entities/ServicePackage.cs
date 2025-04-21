using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Repositories.Entities
{
    public class ServicePackage : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<UserPackage> UserPackages { get; set; } = new List<UserPackage>();
        public virtual ICollection<Deal> Deals { get; set; } = new List<Deal>();
    }
}
