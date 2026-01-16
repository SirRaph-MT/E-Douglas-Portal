using Core.Enums;
using static Core.Enums.E_DouglasEnums;

namespace Core.Models
{
    public class Enrollment: BaseEntity<long>
    {
        public long StudentProfileId { get; set; }
        public StudentProfile Student { get; set; } = default!;

        public long CourseId { get; set; }
        public Course Course { get; set; } = default!;

        public EnrollmentStatus EnrollmentStatus { get; set; } = EnrollmentStatus.Pending;

        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<ProgressRecord> ProgressRecords { get; set; } = new List<ProgressRecord>();
    }
}
