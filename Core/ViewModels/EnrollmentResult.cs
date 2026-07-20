namespace Core.ViewModels
{
    public class EnrollmentResult
    {
        public bool IsError { get; set; }
        public string? Message { get; set; }
        public long EnrollmentId { get; set; }
    }

    public class CheckoutViewModel
    {
        public long EnrollmentId { get; set; }
        public string CourseTitle { get; set; } = default!;
        public decimal Amount { get; set; }
        public string StudentFullName { get; set; } = default!;
        public string StudentEmail { get; set; } = default!;
        public string PaymentStatus { get; set; } = default!;
        public string? PaymentReference { get; set; }
    }

    public class EnrollmentListViewModel
    {
        public long Id { get; set; }
        public string StudentName { get; set; } = default!;
        public string CourseName { get; set; } = default!;
        public DateTime DateCreated { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; } = default!;
        public string? PaymentReference { get; set; }
    }
}