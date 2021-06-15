using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace BusinessLayer.Concrete
{
    public class LoginManager:ILoginService
    {
        IAdminDal _adminDal;

        public LoginManager(IAdminDal adminDal)
        {
            _adminDal = adminDal;
        }

        public bool LoginByAdmin(Admin admin)
        {
            var value = _adminDal.List().FirstOrDefault(x => x.AdminPassword == admin.AdminPassword &&
                                                 x.AdminUsername == admin.AdminUsername);

            if (value != null) return true;
            return false;


        }
    }
}
