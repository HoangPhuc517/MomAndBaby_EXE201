using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Repositories.Entities;

namespace MomAndBaby.Services.DTO.ServicePackageModel
{
    public class PackageModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int? MonthlyUsageLimit { get; set; }
        public virtual ICollection<PackageDealModel> Deals { get; set; } = new List<PackageDealModel>();
    }

    public class PackageDealModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public double DiscountRate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public int OfferConditions { get; set; }
    }


    public class PackageViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public int? MonthlyUsageLimit { get; set; }
        public virtual ICollection<PackageDealViewModel> Deals { get; set; } = new List<PackageDealViewModel>();
    }

    public class PackageDealViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public double DiscountRate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public int OfferConditions { get; set; }
        public string Status { get; set; }
    }

    public class UpdatePackageModel
    {
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int? MonthlyUsageLimit { get; set; }
    }
}
