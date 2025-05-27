using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MomAndBaby.Core.Base;
using MomAndBaby.Core.Store;
using MomAndBaby.Repositories.Entities;

namespace MomAndBaby.Services.Helpers
{
    public static class AppointmentHelper
    {
        public static void CheckValidUserUpdateAppointment(string role, AppointmentStatusEnum appointmentStatusEnum)
        {
            if (role == UserRoleEnum.CUSTOMER.ToString() 
                && appointmentStatusEnum == AppointmentStatusEnum.Approved)
            {
                throw new BaseException(StatusCodes.Status403Forbidden, "Customer is not authorized to approve appointments");
            }
        }

        public static void CheckIsCancelCompleteAppointment(Appointment appointment)
        {
            if (appointment.Status == AppointmentStatusEnum.Canceled.ToString() 
             || appointment.Status == AppointmentStatusEnum.Completed.ToString())
            {
                throw new BaseException(StatusCodes.Status400BadRequest, "Appointment has already been canceled or Completed!!!");
            }
        }

    }
}
