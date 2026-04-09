using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class CourseViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public decimal? Price { get; set; }
        public string? Duration { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime DateCreated { get; set; }
    }
}
