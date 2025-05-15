using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Services.DTO.DealModel
{
    public class CreateDealModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public double DiscountRate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string OfferConditions { get; set; }
        public string ServicePackageId { get; set; }
    }

    public class UpdateDealModel
    {
        public string Description { get; set; }
        public string? Image { get; set; }
        public double DiscountRate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string OfferConditions { get; set; }
    }
}
