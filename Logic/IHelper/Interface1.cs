using Core.ViewModels;
using X.PagedList;

namespace Logic.IHelper
{
    public interface IEnrollmentHelper
    {
        EnrollmentResult CreateEnrollment(string userId, long courseId);
        CheckoutViewModel? GetCheckoutDetails(long enrollmentId, string userId);
        IPagedList<EnrollmentListViewModel> Enrollments(IPageListModel<EnrollmentListViewModel> model, int page);
    }
}