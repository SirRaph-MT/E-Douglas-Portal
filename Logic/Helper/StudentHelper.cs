using Core.DB;
using Core.ViewModels;
using Logic.IHelper;
using Microsoft.EntityFrameworkCore;   
using X.PagedList;
using X.PagedList.Extensions;

namespace Logic.Helper
{
    public class StudentHelper : IStudentHelper
    {
        private readonly AppDBContext _context;
        public StudentHelper(AppDBContext context)
        {
            _context = context;
        }

        public IQueryable<ApplicationUserViewModel> GetAllStudents()
        {
            var query = _context.applicationUsers
                .Where(x => !x.Deleted && x.StudentProfile != null)
                .Select(x => new ApplicationUserViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    FullName = x.FirstName + " " + x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    DateRegistered = x.DateCreated,
                    DateOfBirth = x.DateOfBirth,
                    ProfilePictureUrl = x.ProfilePictureUrl
                });

            return query;
        }

        public IPagedList<ApplicationUserViewModel> Students(IPageListModel<ApplicationUserViewModel> model, int page)
        {
            try
            {
                var query = GetAllStudents();

                if (!string.IsNullOrEmpty(model.Keyword))
                {
                    var key = model.Keyword.ToLower();
                    query = query.Where(x =>
                        (x.FirstName ?? "").ToLower().Contains(key) ||
                        (x.LastName ?? "").ToLower().Contains(key) ||
                        (x.Email ?? "").ToLower().Contains(key) ||
                        (x.PhoneNumber ?? "").ToLower().Contains(key) ||
                        (x.FullName ?? "").ToLower().Contains(key));
                }

                if (model.StartDate.HasValue)
                    query = query.Where(x => x.DateRegistered >= model.StartDate);

                if (model.EndDate.HasValue)
                    query = query.Where(x => x.DateRegistered <= model.EndDate);

                return query
                    .OrderByDescending(x => x.DateRegistered)
                    .ToPagedList(page, 25);
            }
            catch
            {
                throw;
            }
        }

        public StudentDetailsViewModel? GetStudentDetails(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return null;

            var user = _context.applicationUsers
                .Include(u => u.StudentProfile)
                .FirstOrDefault(u => u.Id == userId && !u.Deleted);

            if (user == null) return null;

            var profile = user.StudentProfile;

            var enrolledCourses = profile == null
                ? new List<EnrolledCourseViewModel>()
                : _context.Enrollments
                    .Include(e => e.Course)
                    .Include(e => e.ProgressRecords)
                    .Where(e => !e.Deleted && e.StudentProfileId == profile.Id)
                    .ToList()
                    .Select(e => new EnrolledCourseViewModel
                    {
                        EnrollmentId = e.Id,
                        CourseId = e.CourseId ?? 0,
                        CourseTitle = e.Course?.Title ?? "Unknown Course",
                        EnrollmentStatus = e.EnrollmentStatus.ToString(),
                        ProgressPercentage = e.ProgressRecords.Any()
                            ? Math.Round(e.ProgressRecords.Average(p => p.ProgressPercentage ?? 0), 1)
                            : 0
                    })
                    .ToList();

            return new StudentDetailsViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = profile?.DateOfBirth ?? user.DateOfBirth,
                StudentType = profile?.StudentType.ToString(),
                SponsorName = profile?.SponsorName,
                SponsorPhone = profile?.SponsorPhone,
                SponsorAddress = profile?.SponsorAddres,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Deleted = user.Deleted,
                EnrolledCourses = enrolledCourses
            };
        }
    }
}