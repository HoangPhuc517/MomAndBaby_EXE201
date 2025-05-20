namespace MomAndBaby.Repositories.Entities
{
    public class Deal : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public double DiscountRate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public int OfferConditions { get; set; }
        public Guid ServicePackageId { get; set; }
        public virtual ServicePackage ServicePackage { get; set; }
    }
}
