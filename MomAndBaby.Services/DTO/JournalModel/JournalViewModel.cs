using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Services.DTO.JournalModel
{
    public class JournalViewModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
        public string Head { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public Guid UserId { get; set; }
    }
}
