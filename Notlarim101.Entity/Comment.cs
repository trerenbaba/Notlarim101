﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim101.Entity
{
    [Table("tblComments")]
    public class Comment:MyEntityBase
    {
        [Required,StringLength(300)]
        public string Text { get; set; }
        [ForeignKey("Note")]
        public int NoteId { get; set; }
        public virtual Note Note { get; set; }
        public virtual NotlarimUser Owner { get; set; }


    }
}
