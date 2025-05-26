using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Core.Store;

namespace MomAndBaby.Services.DTO.AppointmentModel
{
    public class CreateAppointmentDTO
    {
        public string Content { get; set; }
        public AppointmentTypeEnum Type { get; set; }
        public DateTimeOffset AppointmentDate { get; set; }
        public string Place { get; set; }
        public string? LinkMeet { get; set; }
        public Guid TimeSlotId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ExpertId { get; set; }
    }

    public class CreateTimeSlotDTO
    {
        /// <summary>
        /// HH:mm
        /// </summary>
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
