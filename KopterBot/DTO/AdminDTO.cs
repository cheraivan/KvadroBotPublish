using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KopterBot.DTO
{
    class AdminDTO
    {
        [Key]
        public int Id { get; set; }
       
        public long ChatId { get; set; }

        // желание быть админом,1 если да 
        public int Wish { get; set; }
    }
}
