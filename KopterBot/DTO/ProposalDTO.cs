﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KopterBot.DTO
{
    class ProposalDTO
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("UserDTO")]
        public long ChatId { get; set; }

        public string TypeOfInsurance { get; set; }

        public string Adress { get; set; }
        public string RealAdress { get; set; }
        public string Region { get; set; }

        public float?  longtitude{ get; set; }
        public float? latitude { get; set; }

        public string BortNumber { get; set; }

        public UserDTO User { get; set; }
    }
}
