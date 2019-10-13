using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KopterBot.DTO
{
    class BuisnessTaskDTO
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserDTO")]
        public long ChatId { get; set; } // кто создал,хз там

        public double Sum { get; set; }

        public string Description { get; set; }

        public string Region { get; set; }

        public long? ChatIdPerformer { get; set; }
    }
}
