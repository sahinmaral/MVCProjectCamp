using DataAccessLayer.Abstract;

using EntityLayer.Concrete;

using System.Linq;
using System.Web;
using System.Web.Security;


namespace BusinessLayer.Concrete
{
    public class LoginManager:ILoginService
    {
        IAdminDal _adminDal; 
        IUserDal _userDal;

        public LoginManager(IAdminDal adminDal,IUserDal userDal)
        {
            _adminDal = adminDal;
            _userDal = userDal;
        }

        public bool LoginByAdmin(Admin admin)
        {
            var user = _userDal.List().FirstOrDefault(x => x.UserUsername == 
                                                           admin.User.UserUsername &&
                                                           x.UserPassword == admin.User.UserPassword);

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(admin.User.UserUsername, false);
                HttpContext.Current.Session["UserUserName"] = admin.User.UserUsername;
                return true;
            }
            return false;


        }
    }
}
