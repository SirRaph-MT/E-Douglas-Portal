namespace Core.Models
{
    public class CourseTopic : BaseEntity<long>
    {
        public long CourseId { get; set; }
        public Course Course { get; set; } = default!;

        public string Title { get; set; } = default!;
        public int OrderIndex { get; set; }
        public int DurationInHours { get; set; }

        // Navigation
        public ICollection<CourseSubTopic> SubTopics { get; set; } = new List<CourseSubTopic>(); 
    }
}
