using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KopterBot.DTO
{
    class StorageDTO
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
    }
}
