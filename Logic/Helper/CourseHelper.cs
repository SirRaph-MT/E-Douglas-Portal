using Core.DB;
using Core.DbContext;
using Core.ViewModels;
using Logic.IHelper;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;
using static QRCoder.PayloadGenerator.SwissQrCode;

namespace Logic.Helper
{
    public class CourseHelper(AppDBContext context) : ICourseHelper
    {
        private readonly AppDBContext _context = context;
        public IPagedList<CourseViewModel> Courses(IPageListModel<CourseViewModel> model, int page)
        {
            try
            {
                var query = _context.Courses
                    .Include(c => c.Topics)
                    .ThenInclude(t => t.SubTopics)
                    .Where(c => !c.Deleted);

                if (!string.IsNullOrEmpty(model.Keyword))
                {
                    var key = model.Keyword.ToLower();

                    query = query.Where(c =>
                        c.Title.ToLower().Contains(key) ||
                        (c.Description ?? "").ToLower().Contains(key) ||
                        c.Price.ToString().Contains(key) ||
                        (c.Duration ?? "").ToLower().Contains(key)
                    );
                }

                if (model.EndDate.HasValue)
                {
                    query = query.Where(c => c.DateCreated <= model.EndDate.Value);
                }

                var projected = query.Select(c => new CourseViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Price = c.Price ?? 0,
                    Duration = c.Duration,
                    DateCreated = c.DateCreated,
                    IsActive = c.IsActive
                });

                return projected.ToPagedList(page, 25);
            }
            catch
            {
                throw;
            }
        }
    }
}
