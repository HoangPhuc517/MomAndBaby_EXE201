using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Core.Store;

namespace MomAndBaby.Services.DTO.ExpertModel
{
    public class ExpertProfileViewModel
    {
        public Guid Id { get; set; }
        public string Degree { get; set; }
        public string? Workplace { get; set; }
        public string? Description { get; set; }
        public string Specialty { get; set; }
        public double? Stars { get; set; }
        public string Status { get; set; }
        public Guid UserId { get; set; }
        public virtual ExUserViewModel User { get; set; }
    }

    public class ExUserViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string? Avatar { get; set; }
        public DateTime DateOfBirth { get; set; }
        public UserSexEnum Sex { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
    }


}
