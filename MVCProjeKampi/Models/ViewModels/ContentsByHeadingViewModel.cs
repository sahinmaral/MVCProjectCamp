using EntityLayer.Concrete;

using System.Collections.Generic;

namespace MVCProjeKampi.Models.ViewModels
{
    public class ContentsByHeadingViewModel
    {
        public List<Content> ContentList { get; set; }
        public string HeadingName { get; set; }
        public Content GoingToAddContent { get; set; }
    }
}