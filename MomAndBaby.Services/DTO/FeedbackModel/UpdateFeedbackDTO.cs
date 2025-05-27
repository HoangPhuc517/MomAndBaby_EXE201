using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Services.DTO.FeedbackModel
{
    public class UpdateFeedbackDTO
    {
        [MaxLength(200)]
        public string? Content { get; set; }
        public Guid FeedbackId { get; set; }
    }
}
