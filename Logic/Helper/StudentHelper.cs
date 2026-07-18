using Core.DB;
using Core.ViewModels;
using Logic.IHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .Where(x => x.StudentProfile != null)
                .Select(x => new ApplicationUserViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    ProfilePictureUrl = x.ProfilePictureUrl
                });

            return query;
        }
    }
}
