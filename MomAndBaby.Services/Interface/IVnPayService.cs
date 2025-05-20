using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MomAndBaby.Repositories.Interface;
using MomAndBaby.Services.DTO.TransactionModel;
using MomAndBaby.Services.DTO.VnPayModel;

namespace MomAndBaby.Services.Interface
{
    public interface IVnPayService
    {
        string CreateVNPayUrl(HttpContext context, VnPayRequestModel model);

        CreateTransactionDTO ProcessVNPayResponse(IQueryCollection collections);
    }
}
