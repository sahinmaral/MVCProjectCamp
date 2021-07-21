﻿using EntityLayer.Abstract;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Heading : IEntity
    {
        public Heading()
        {
            HeadingStatus = true;
        }
        [Key]
        public int HeadingId { get; set; }

        [StringLength(50)]
        public string HeadingName { get; set; }

        public DateTime HeadingDate { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public ICollection<Content> Contents { get; set; }

        public int WriterId { get; set; }
        public virtual Writer Writer { get; set; }

        [DefaultValue(true)]
        public bool HeadingStatus { get; set; }
    }
}
