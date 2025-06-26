using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Services.DTO.FeedbackModel
{
    public class FeedbackViewModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
        public string? Content { get; set; }
        public int Stars { get; set; }
        public Guid? ExpertId { get; set; }
        public Guid? UserId { get; set; }
        public Guid AppointmentId { get; set; }
    }
}
