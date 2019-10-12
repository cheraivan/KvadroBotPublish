using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KopterBot.DTO
{
    class SosDTO
    {
        [Key]
        public int Id { get; set; }
        public Int16 Type { get; set; }

        [ForeignKey("UserDTO")]
        public long ChatId { get; set; }


        public float longtitude { get; set; }
        public float lautitude { get; set; }

    }
}
