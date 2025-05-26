using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Services.DTO.AppointmentModel
{
    public class UpdateAppointmentDTO
    {
        public string Content { get; set; }
        public DateTimeOffset AppointmentDate { get; set; }
        public string Place { get; set; }
        public string LinkMeet { get; set; }
    }
}
