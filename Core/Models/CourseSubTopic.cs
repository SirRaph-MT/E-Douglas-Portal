using static Core.Enums.E_DouglasEnums;
namespace Core.Models
{
    public class CourseSubTopic : BaseEntity<long>
    {

        public long TopicId { get; set; }
        public CourseTopic Topic { get; set; } = default!;

        public string Title { get; set; } = default!;
        public ContentType ContentType { get; set; }
        public string ContentUrl { get; set; } = default!;
        public int DurationInMinutes { get; set; }
        public int OrderIndex { get; set; }

        // Navigation
        public ICollection<ProgressRecord> ProgressRecords { get; set; } = new List<ProgressRecord>();
    }
}
