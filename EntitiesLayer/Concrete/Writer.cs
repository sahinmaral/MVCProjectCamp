﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntityLayer.Abstract;

namespace EntityLayer.Concrete
{
    public class Writer : IEntity
    {
        [Key, ForeignKey("User")]
        public int WriterId { get; set; }
        public User User { get; set; }

        [StringLength(50)]
        public string WriterName { get; set; }

        [StringLength(50)]
        public string WriterSurname { get; set; }

        [StringLength(500)]
        public string WriterImage { get; set; }

        [StringLength(200)]
        public string WriterMail { get; set; }

        [StringLength(100)]
        public string WriterAbout { get; set; }

        [StringLength(50)]
        public string WriterTitle { get; set; }

        [DefaultValue(true)]
        public bool WriterStatus { get; set; }

        public ICollection<Heading> Headings { get; set; }
        public ICollection<Content> Contents { get; set; }

        


    }
}
