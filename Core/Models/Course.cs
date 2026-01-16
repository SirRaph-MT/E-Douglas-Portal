namespace Core.Models
{
    public class Course : BaseEntity<long>
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<CourseTopic> Topics { get; set; } = new List<CourseTopic>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
