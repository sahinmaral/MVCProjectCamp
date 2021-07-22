using EntityLayer.Concrete;

using System.Collections.Generic;
using PagedList;

namespace MVCProjeKampi.Models.ViewModels
{
    public class ContentByWriterViewModel
    {
        public IPagedList<Content> Contents { get; set; }
        public Writer Writer { get; set; }
    }
}