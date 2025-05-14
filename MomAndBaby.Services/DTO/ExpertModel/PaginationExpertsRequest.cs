using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Services.DTO.ExpertModel
{
    public class PaginationExpertsRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? SearchString { get; set; }
        public double? Stars { get; set; }
    }
}
