using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Core.Store
{
    public enum AppointmentTypeEnum
    {
        /// <summary>
        /// Khám trực tiếp tại cơ sở y tế
        /// </summary>
        InPerson = 0,

        /// <summary>
        /// Tư vấn trực tuyến qua video hoặc chat
        /// </summary>
        Online = 1,

        /// <summary>
        /// Tái khám sau điều trị hoặc theo dõi
        /// </summary>
        FollowUp = 2,

        /// <summary>
        /// Khám sức khỏe định kỳ
        /// </summary>
        RoutineCheck = 3,

        /// <summary>
        /// Hẹn để làm xét nghiệm (máu, nước tiểu, v.v.)
        /// </summary>
        LabTest = 4,

        /// <summary>
        /// Hẹn tiêm vắc xin
        /// </summary>
        Vaccination = 5,

        /// <summary>
        /// Tư vấn chung về sức khỏe, dinh dưỡng,...
        /// </summary>
        Consultation = 6
    }

    public enum AppointmentStatusEnum
    {
        /// <summary>
        /// Đã hủy
        /// </summary>
        Canceled = 0,

        /// <summary>
        /// Đã hoàn thành
        /// </summary>
        Completed = 1,

        /// <summary>
        /// Đang chờ
        /// </summary>
        Pending = 2,

        ///<summary>
        ///Đã Duyệt
        /// </summary>
        Approved = 3,

    }

}
