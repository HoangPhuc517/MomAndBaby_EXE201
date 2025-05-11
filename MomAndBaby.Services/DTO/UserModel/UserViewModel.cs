using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Core.Store;
using MomAndBaby.Repositories.Entities;

namespace MomAndBaby.Services.DTO.UserModel
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string? Avatar { get; set; }
        public DateTime DateOfBirth { get; set; }
        public UserSexEnum Sex { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
        public virtual ExpertViewModel? Expert { get; set; }
    }

    public class ExpertViewModel
    {
        public Guid Id { get; set; }
        public string Degree { get; set; }
        public string? Workplace { get; set; }
        public string? Description { get; set; }
        public string Specialty { get; set; }
        public double? Stars { get; set; }
    }

    public class UserUpdateDTO
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public UserSexEnum Sex { get; set; }
    }
}
