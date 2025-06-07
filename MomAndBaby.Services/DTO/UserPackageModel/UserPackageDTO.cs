using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Repositories.Entities;

namespace MomAndBaby.Services.DTO.UserPackageModel
{
    public class UserPackageDTO
    {

    }

    public class UserPackageViewModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }
        public int ValidMonths { get; set; }
        public int? UsageCount { get; set; }
        public decimal Amount { get; set; }
        public long? OrderCode { get; set; }
        public Guid UserId { get; set; }
        public virtual SubPackageViewModel ServicePackage { get; set; }
    }

    public class SubPackageViewModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
    }
}
