using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class AdminDashboardViewModel
    {
        public string UserName { get; set; }
        public int TotalStudents { get; set; }
        public int ActiveCourses { get; set; }
        public int CertificatesIssued { get; set; }
        public decimal TotalEarnings { get; set; }
    }
}
