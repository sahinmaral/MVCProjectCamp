using EntityLayer.Concrete;

using PagedList;

namespace MVCProjeKampi.Models.ViewModels
{
    public class HeadingsByWriterViewModel
    {
        public IPagedList<Heading> Headings { get; set; }
        public User User { get; set; }
    }
}