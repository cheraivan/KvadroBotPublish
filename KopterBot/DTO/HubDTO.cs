using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KopterBot.DTO
{
    class HubDTO
    {
        [Key]
        public int Id { get; set; }

        public long ChatIdCreater { get; set; }
        public long ChatIdReceiver { get; set; }

        public HubDTO(long ChatIdCreater,long ChatIdReceiver)
        {
            this.ChatIdCreater = ChatIdCreater;
            this.ChatIdReceiver = ChatIdReceiver;
        }
    }
}
