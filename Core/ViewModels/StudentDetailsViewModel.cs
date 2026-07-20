using System.Collections.Generic;

namespace Core.ViewModels
{
    public class StudentDetailsViewModel
    {
        public string Id { get; set; } = default!;
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? StudentType { get; set; }
        public string? SponsorName { get; set; }
        public string? SponsorPhone { get; set; }
        public string? SponsorAddress { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public bool Deleted { get; set; }
        public List<EnrolledCourseViewModel> EnrolledCourses { get; set; } = new();
    }

    public class EnrolledCourseViewModel
    {
        public long EnrollmentId { get; set; }
        public long CourseId { get; set; }
        public string CourseTitle { get; set; } = default!;
        public string EnrollmentStatus { get; set; } = default!;
        public decimal ProgressPercentage { get; set; }
    }
}