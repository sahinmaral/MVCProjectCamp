using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityLayer.Concrete;

namespace MVCProjeKampi.Models.ViewModels
{
    public class ContentByWriterViewModel
    {
        public List<Content> Contents { get; set; }
        public Writer Writer { get; set; }
    }
}