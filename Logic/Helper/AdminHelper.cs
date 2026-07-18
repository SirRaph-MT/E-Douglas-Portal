using Core.DB;
using Logic.IHelper;

namespace Logic.Helper
{
    public class AdminHelper : IAdminHelper
    {
        private readonly AppDBContext _context;
        public AdminHelper(AppDBContext context)
        {
            _context = context;
        }


    }
}
