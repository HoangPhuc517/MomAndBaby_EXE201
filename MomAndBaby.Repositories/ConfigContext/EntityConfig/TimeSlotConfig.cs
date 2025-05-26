using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Repositories.ConfigContext.EntityConfig
{
    public class TimeSlotConfig : IEntityTypeConfiguration<TimeSlot>
    {
        public void Configure(EntityTypeBuilder<TimeSlot> builder)
        {
            builder.ToTable("TimeSlots");

            builder.HasKey(ts => ts.Id);

            builder.Property(ts => ts.Time)
                .IsRequired();

            builder.HasMany(ts => ts.Appointments)
                .WithOne(a => a.TimeSlot)
                .HasForeignKey(es => es.TimeSlotId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
