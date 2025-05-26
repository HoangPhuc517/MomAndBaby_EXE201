using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Repositories.Entities;

namespace MomAndBaby.Services.Interface
{
    public interface IEmailService
    {
        public Task SendMailRegister(string email);
        public Task SendMailForgotPassword(string email, string token);
        Task SendMailAppointmentNotification(Appointment model, string email);
    }
}
