﻿using EntityLayer.Concrete;

using System.Collections.Generic;

namespace MVCProjeKampi.ViewModels
{
    public class ContentByHeadingViewModel
    {
        public List<Content> ContentList { get; set; }
        public string HeadingName { get; set; }
    }
}