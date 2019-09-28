using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KopterBot.DTO
{
    class ProposalDTO
    {
        [Key]
        public int Id { get; set; }

        public long ChatId { get; set; }


    }
}
