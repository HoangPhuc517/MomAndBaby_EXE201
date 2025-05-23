﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Repositories.Entities
{
    public class Notification : BaseEntity
    {
        public string Message { get; set; }
        public string? Type { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
