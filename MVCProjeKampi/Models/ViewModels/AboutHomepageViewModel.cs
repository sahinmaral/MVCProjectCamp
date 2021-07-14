using EntityLayer.Concrete;

using PagedList;

namespace MVCProjeKampi.Models.ViewModels
{
    public class AboutHomepageViewModel
    {
        public IPagedList<About> Abouts { get; set; }
        public int EnabledAbouts { get; set; }
        public int DisabledAbouts { get; set; }
    }
}