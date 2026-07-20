using Logic.Helper;
using Logic.Helpers;
using Logic.IHelper;
using Microsoft.Extensions.DependencyInjection;

namespace Logic.Services
{
    public static class MiddlewareRegister
    {
        public static IServiceCollection RegisterHelpers(this IServiceCollection services)
        {
            services.AddScoped<IUserHelper, UserHelper>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<ICourseHelper, CourseHelper>();
            services.AddScoped<IStudentHelper, StudentHelper>();
            services.AddScoped<IEnrollmentHelper, EnrollmentHelper>();   
            return services;
        }
    }
}
