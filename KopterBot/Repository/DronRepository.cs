﻿using KopterBot.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Repository
{
    class DronRepository
    {
        private ApplicationContext db;
        GenericRepository<DronDTO> drons;
        public DronRepository(ApplicationContext db)
        {
            this.db = db;
            drons = new GenericRepository<DronDTO>(db);
        }

        public async Task CreateDron(DronDTO dto)
        {
            DronDTO dron = await db.Drons.Where(dr => dto.Mark == dr.Mark)
                .FirstOrDefaultAsync();
            if (dron == null)
            { 
                await drons.Create(dto);
            }
            return;
        }
    }
}