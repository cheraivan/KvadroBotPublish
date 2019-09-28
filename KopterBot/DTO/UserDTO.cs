using KopterBot.Commons;
using KopterBot.Interfaces;
using KopterBot.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KopterBot.DTO
{
    class UserDTO:BaseRepository,IBaseEntity
    {
        private string m_BortNumber;
        private bool disposed = false;

        #region Properties
        [Key]
        public long ChatId { get; set; }
        public string Login { get; set; }
        public string FIO { get; set; }
        public string TypeOfInsurance { get; set; }
        public string Adress { get; set; }
        public string Mode { get; set; }
        public StepDTO step { get; set; }

        public float longtitude { get; set; }
        public float latitude { get; set; }

        public string BortNumber
        {
            get
            {
                return m_BortNumber;
            }
            set
            {
                if(Common.isDigit(value))
                {
                    m_BortNumber = value;
                }
            }
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
