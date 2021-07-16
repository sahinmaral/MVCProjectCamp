using EntityLayer.Concrete;

namespace MVCProjeKampi.Models.ViewModels
{
    public class SelectedRoleViewModel
    {
        public SelectedRoleViewModel()
        {
            IsSelected = false;
        }
        public Role Role { get; set; }
        public bool IsSelected { get; set; }
    }
}