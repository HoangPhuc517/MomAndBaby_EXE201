using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MomAndBaby.Services.DTO.JournalModel
{
    public class JournalDto
    {
        public string Head { get; set; }
        public string? Content { get; set; }
        public IFormFile? Image { get; set; }
    }
}
