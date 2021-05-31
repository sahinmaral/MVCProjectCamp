﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using EntitiesLayer.Abstract;
using EntitiesLayer.Concrete;

namespace EntityLayer.Concrete
{
    public class Content : IEntity
    {
        [Key]
        public int ContentId { get; set; }
        [StringLength(1000)]
        public string ContentText { get; set; }
        public DateTime ContentDate { get; set; }

        public int HeadingId { get; set; }
        public virtual Heading Heading { get; set; }

        public int? WriterId { get; set; }
        public virtual Writer Writer { get; set; }

        [DefaultValue(true)]
        public bool ContentStatus { get; set; }

    }
}
