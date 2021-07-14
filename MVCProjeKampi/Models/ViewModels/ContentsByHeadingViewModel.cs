using EntityLayer.Concrete;

using System.Collections.Generic;
using PagedList;

namespace MVCProjeKampi.Models.ViewModels
{
    public class ContentsByHeadingViewModel
    {
        public IPagedList<Content> ContentList { get; set; }
        public Heading Heading { get; set; }
        public Content GoingToAddContent { get; set; }
    }
}