using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Services.Interface
{
    public interface IEmailService
    {
        public Task SendMailRegister(string email);
    }
}
