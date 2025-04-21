namespace MomAndBaby.Repositories.Entities
{
    public class Transaction : BaseEntity
    {
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string? Message { get; set; }
        public string TransferAccountName { get; set; }
        public string TransferAccountNumber { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
