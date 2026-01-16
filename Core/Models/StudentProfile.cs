using static Core.Enums.E_DouglasEnums;

namespace Core.Models
{
    public class StudentProfile : BaseEntity<long>
    {

            public string UserId { get; set; } = default!;
            public ApplicationUser User { get; set; } = default!;
            public StudentType StudentType { get; set; }

            public DateTime? DateOfBirth { get; set; }
            public string? SponsorName { get; set; }
            public string? SponsorAddres { get; set; }
            public string? SponsorPhone { get; set; }
            public string? PassportUrl { get; set; }

            public ProfileCompletionStage ProfileCompletionStage { get; set; }
                = ProfileCompletionStage.Basic;

            // Navigation
            public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
            public ICollection<ProgressRecord> ProgressRecords { get; set; } = new List<ProgressRecord>();

    }

}
