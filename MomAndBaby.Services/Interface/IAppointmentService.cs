using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Services.DTO.AppointmentModel;

namespace MomAndBaby.Services.Interface
{
    public interface IAppointmentService
    {
        Task<string> CreateAppointment(CreateAppointmentDTO appointment);
    }
}
