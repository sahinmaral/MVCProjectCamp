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
        public int EnabledAbouts { get; set; }
        public int DisabledAbouts { get; set; }
    }
}