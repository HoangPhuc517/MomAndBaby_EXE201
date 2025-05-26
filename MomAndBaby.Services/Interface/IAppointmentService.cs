using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Core.Store;
using MomAndBaby.Repositories.Entities;
using MomAndBaby.Repositories.Helpers;
using MomAndBaby.Services.DTO.AppointmentModel;

namespace MomAndBaby.Services.Interface
{
    public interface IAppointmentService
    {
        Task<AppointmentViewModel> CreateAppointment(CreateAppointmentDTO appointment);
        Task UpdateStatusAppointment(string id, AppointmentStatusEnum statusEnum);
        Task<AppointmentViewModel> GetAppointmentById(string id);
        Task<Pagination<AppointmentViewModel>> GetAppointmentByPagination(int pageIndex, int pageSize, string? searchString, string? userId, AppointmentStatusEnum? appointmentStarusEnum, AppointmentTypeEnum? appointmentTypeEnum, bool isDescending);
        Task<List<TimeSlot>> GetTimeSlots();
        Task<TimeSlot> CreateTimeSlot(TimeOnly startTime, TimeOnly endTime);
    }
}
