using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Services.DTO.DealModel
{
    public class DealViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public double DiscountRate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public int OfferConditions { get; set; }
        public Guid ServicePackageId { get; set; }
    }
}
