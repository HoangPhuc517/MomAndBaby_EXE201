﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Services.DTO.VnPayModel
{
    public class VnPayRequestModel
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
