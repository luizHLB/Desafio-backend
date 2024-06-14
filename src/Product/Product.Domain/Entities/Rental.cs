using Product.Domain.Entities.Base;

namespace Product.Domain.Entities
{
    public class Rental : BaseEntity
    {
        public long DriverId { get; set; }
        public virtual Driver Driver { get; set; }

        public long VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        public long PlanId { get; set; }
        public virtual Plan Plan { get; set; }

        public DateTime WithdrawDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime EstimatedReturnDate { get; set; }

        public double? TotalRental { get; set; }
        public double? TotalFines { get; set; }
        public double? TotalExtras { get; set; }

    }
}
