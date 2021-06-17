using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityLayer.Concrete;

namespace MVCProjeKampi.Models.ViewModels
{
    public class AboutHomepageViewModel
    {
        public List<About> Abouts { get; set; }
        public bool AboutAlertStatus { get; set; }
    }
}