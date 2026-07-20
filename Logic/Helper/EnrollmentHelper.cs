using Core.DB;
using Core.Models;
using Core.ViewModels;
using Logic.IHelper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using X.PagedList;
using X.PagedList.Extensions;
using static Core.Enums.E_DouglasEnums;

namespace Logic.Helper
{
    public class EnrollmentHelper : IEnrollmentHelper
    {
        private readonly AppDBContext _context;
        public EnrollmentHelper(AppDBContext context)
        {
            _context = context;
        }

        public EnrollmentResult CreateEnrollment(string userId, long courseId)
        {
            var profile = _context.StudentProfiles.FirstOrDefault(p => p.UserId == userId && !p.Deleted);
            if (profile == null)
                return new EnrollmentResult { IsError = true, Message = "Please complete your profile before enrolling." };

            var course = _context.Courses.FirstOrDefault(c => c.Id == courseId && !c.Deleted && c.IsActive);
            if (course == null)
                return new EnrollmentResult { IsError = true, Message = "Course not found or no longer available." };

            var existing = _context.Enrollments
                .FirstOrDefault(e => e.StudentProfileId == profile.Id && e.CourseId == courseId && !e.Deleted);
            if (existing != null)
                return new EnrollmentResult { IsError = false, EnrollmentId = existing.Id };

            var enrollment = new Enrollment
            {
                StudentProfileId = profile.Id,
                CourseId = courseId,
                EnrollmentStatus = EnrollmentStatus.Pending
            };
            _context.Enrollments.Add(enrollment);
            _context.SaveChanges();

            _context.Payments.Add(new Payment
            {
                EnrollmentId = enrollment.Id,
                Amount = course.Price,
                PaymentStatus = PaymentStatus.Pending,
                PaymentChannel = PaymentChannel.Manual,
                VerifiedByAdmin = false
            });
            _context.SaveChanges();

            return new EnrollmentResult { IsError = false, EnrollmentId = enrollment.Id };
        }

        public CheckoutViewModel? GetCheckoutDetails(long enrollmentId, string userId)
        {
            var enrollment = _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student).ThenInclude(s => s!.User)
                .Include(e => e.Payments)
                .FirstOrDefault(e => e.Id == enrollmentId && !e.Deleted);

            if (enrollment == null) return null;
            if (enrollment.Student?.UserId != userId) return null; // ownership check — no IDOR

            var payment = enrollment.Payments.OrderByDescending(p => p.DateCreated).FirstOrDefault();

            return new CheckoutViewModel
            {
                EnrollmentId = enrollment.Id,
                CourseTitle = enrollment.Course?.Title ?? "",
                Amount = payment?.Amount ?? enrollment.Course?.Price ?? 0,
                StudentFullName = enrollment.Student?.User?.FullName?.Trim() ?? "",
                StudentEmail = enrollment.Student?.User?.Email ?? "",
                PaymentStatus = payment?.PaymentStatus?.ToString() ?? "Pending",
                PaymentReference = payment?.PaymentReference
            };
        }

        public IPagedList<EnrollmentListViewModel> Enrollments(IPageListModel<EnrollmentListViewModel> model, int page)
        {
            var baseQuery = _context.Enrollments
                .Include(e => e.Student).ThenInclude(s => s!.User)
                .Include(e => e.Course)
                .Where(e => !e.Deleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(model.Keyword))
            {
                var key = model.Keyword.ToLower();
                baseQuery = baseQuery.Where(e =>
                    (e.Student != null && e.Student.User != null &&
                        ((e.Student.User.FirstName ?? "") + " " + (e.Student.User.LastName ?? "")).ToLower().Contains(key)) ||
                    (e.Course != null && e.Course.Title.ToLower().Contains(key)));
            }
            if (model.StartDate.HasValue) baseQuery = baseQuery.Where(e => e.DateCreated >= model.StartDate);
            if (model.EndDate.HasValue) baseQuery = baseQuery.Where(e => e.DateCreated <= model.EndDate);

            var pagedEnrollments = baseQuery.OrderByDescending(e => e.DateCreated).ToPagedList(page, 25);

            var enrollmentIds = pagedEnrollments.Select(e => e.Id).ToList();
            var latestPayments = _context.Payments
                .Where(p => p.EnrollmentId != null && enrollmentIds.Contains(p.EnrollmentId.Value))
                .GroupBy(p => p.EnrollmentId)
                .Select(g => g.OrderByDescending(p => p.DateCreated).First())
                .ToList();

            var mapped = pagedEnrollments.Select(e =>
            {
                var payment = latestPayments.FirstOrDefault(p => p.EnrollmentId == e.Id);
                return new EnrollmentListViewModel
                {
                    Id = e.Id,
                    StudentName = e.Student?.User?.FullName?.Trim() ?? "Unknown",
                    CourseName = e.Course?.Title ?? "Unknown",
                    DateCreated = e.DateCreated,
                    Amount = payment?.Amount ?? e.Course?.Price ?? 0,
                    PaymentStatus = payment?.PaymentStatus?.ToString() ?? "Pending",
                    PaymentReference = payment?.PaymentReference
                };
            }).ToList();

            return new StaticPagedList<EnrollmentListViewModel>(mapped, pagedEnrollments.GetMetaData());
        }
    }
}