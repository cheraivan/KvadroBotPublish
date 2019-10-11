using KopterBot.Commons;
using KopterBot.Interfaces;
using KopterBot.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KopterBot.DTO
{
    class UserDTO
    {
        private bool disposed = false;

        #region Properties
        [Key]
        public long ChatId { get; set; }
        public string Login { get; set; }
        public string FIO { get; set; }
        public string Phone { get; set; }
        public int IsRegister { get; set; }
        public int PilotPrivilag { get; set; } 
        public int BuisnesPrivilag { get; set; } 
        public StepDTO step { get; set; }
        public ICollection<ProposalDTO> proposals { get; set; }

        public int IdForShow { get; set; }
        public UserDTO()
        {
            proposals = new List<ProposalDTO>();
        }
        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #region private and protected methods

        protected virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if(disposing)
                {
                    Dispose();
                }
                disposed = true;
            }
        }

        ~UserDTO() =>
            Dispose(false);

        #endregion
    }
}
