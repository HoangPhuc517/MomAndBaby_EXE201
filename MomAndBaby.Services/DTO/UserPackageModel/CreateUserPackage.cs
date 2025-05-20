using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Services.DTO.UserPackageModel
{
    public class CreateUserPackage
    {
        [Range(1, 120)]
        public int MonthNumber { get; set; }
        public string PackageId { get; set; }
        public string? DealId { get; set; }
    }
}
