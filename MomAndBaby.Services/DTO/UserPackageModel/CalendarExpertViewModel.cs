using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Services.DTO.UserPackageModel
{
    public class CalendarExpertViewModel
    {
        public DateTimeOffset AppointmentDate { get; set; }
        public string Time { get; set; }
    }
}
