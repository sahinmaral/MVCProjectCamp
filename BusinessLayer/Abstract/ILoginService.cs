using EntityLayer.Concrete;

namespace BusinessLayer.Concrete
{
    public interface ILoginService
    {
        bool LoginByAdmin(Admin admin);
    }
}