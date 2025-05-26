using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using MomAndBaby.Repositories.Entities;

namespace MomAndBaby.Services.DTO.AppointmentModel
{
    public class AppointmentViewModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public DateTimeOffset AppointmentDate { get; set; }
        public string TimeSlot { get; set; }
        public string Place { get; set; }
        public string? LinkMeet { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ExpertId { get; set; }
        public FeedbackAppointmentVM Feedback { get; set; }
        public int ReportCount { get; set; }
    }

    public class FeedbackAppointmentVM
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
        public string? Content { get; set; }
        public int Stars { get; set; }
        public Guid? UserId { get; set; }
    }
}
