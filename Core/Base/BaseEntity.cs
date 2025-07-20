using System;
using System.Collections.Generic;
using System.Linq;
namespace MomAndBaby.Core.Base
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedTime = DateTimeOffset.UtcNow;
            UpdatedTime = CreatedTime;
            Status = BaseEnum.Active.ToString();
        }

        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
    }
}
