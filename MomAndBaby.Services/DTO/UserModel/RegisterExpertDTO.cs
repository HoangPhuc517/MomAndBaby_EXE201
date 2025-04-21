using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Core.Store;

namespace MomAndBaby.Services.DTO.UserModel
{
    public class RegisterExpertDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(4)]
        public string UserName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public UserSexEnum Sex { get; set; }
        public ExpertDTO Expert { get; set; }
    }

    public class ExpertDTO
    {
        public string Degree { get; set; }
        public string? Workplace { get; set; }
        public string? Description { get; set; }
        public string Specialty { get; set; }
    }
}
