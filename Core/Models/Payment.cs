using static Core.Enums.E_DouglasEnums;

namespace Core.Models
{
    public class Payment: BaseEntity<long>
    {
        public long EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = default!;

        public decimal Amount { get; set; }
        public string PaymentReference { get; set; } = default!;
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentChannel PaymentChannel { get; set; }

        public bool VerifiedByAdmin { get; set; }
        public DateTime? PaidAt { get; set; }

    }
}
