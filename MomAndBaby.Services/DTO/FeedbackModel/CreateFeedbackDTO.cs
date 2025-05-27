using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Services.DTO.FeedbackModel
{
    public class CreateFeedbackDTO
    {
        [MaxLength(200)]
        public string? Content { get; set; }
        [Range(0, 5)]
        public int Stars { get; set; }
        public Guid AppointmentId { get; set; }
    }
}
