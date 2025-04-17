using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Repositories.Entities
{
    public class Deal
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public double DiscountRate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string OfferConditions { get; set; }
        public Guid ServicePackageId { get; set; }
        public virtual ServicePackage ServicePackage { get; set; }
    }
}
