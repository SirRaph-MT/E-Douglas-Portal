namespace Core.Models
{
    public class ProgressRecord
    {
        public int Id { get; set; }
        public long? EnrollmentId { get; set; }
        public Enrollment? Enrollment { get; set; } = default!;

        public long? SubTopicId { get; set; }
        public CourseSubTopic? SubTopic { get; set; } = default!;
        public long? TopicId { get; set; }
        public CourseTopic? Topic { get; set; } = default!;

        public DateTime? StartedAt { get; set; }
        public DateTime? LastAccessedAt { get; set; }

        public bool? StartedByUser { get; set; }
        public decimal? ProgressPercentage { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
