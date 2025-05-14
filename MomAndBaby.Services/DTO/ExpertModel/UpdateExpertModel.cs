using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Services.DTO.ExpertModel
{
    public class UpdateExpertModel
    {
        public string Degree { get; set; }
        public string? Workplace { get; set; }
        public string? Description { get; set; }
        public string Specialty { get; set; }
    }
}
