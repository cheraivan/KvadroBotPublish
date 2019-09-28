using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KopterBot.DTO
{
    class UsersDronsInclude
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("UserDTO")]
        public long Chatid { get; set; }
        [ForeignKey("DronDTO")]
        public int DronID { get; set; }
    }
}
