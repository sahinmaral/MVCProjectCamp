using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityLayer.Concrete;

namespace MVCProjeKampi.Models.ViewModels
{
    public class HomepageViewModel
    {
        public List<Heading> Headings { get; set; }
        public List<Content> Contents { get; set; }
        public List<User> Users { get; set; }
    }
}