using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Repositories.ConfigContext.EntityConfig
{
    public class TransactionConfig : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.CreatedTime)
                .IsRequired();

            builder.Property(a => a.UpdatedTime)
                .IsRequired();

            builder.Property(a => a.Status)
                .IsRequired();

            builder.Property(t => t.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(t => t.Type)
                .IsRequired();

            builder.Property(t => t.Message);

            builder.Property(t => t.TransferAccountName)
                .IsRequired();

            builder.Property(t => t.TransferAccountNumber)
                .IsRequired();

            builder.HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
