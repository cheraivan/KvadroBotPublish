﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace KopterBot.DTO
{
    class ApplicationContext:DbContext
    {
        public DbSet<UserDTO> Users { get; set; }
        public DbSet<StepDTO> Steps { get; set; }
        public DbSet<DronDTO> Drons { get; set; }
        public DbSet<HubDTO> Hubs { get; set; } 
        public DbSet<AdminDTO> Admins { get; set; }
        public DbSet<ProposalDTO> proposalsDTO { get; set; }
        public DbSet<CountPropose> CountPurpose { get; set; }
        public DbSet<StorageDTO> Storage { get; set; }
        public DbSet<ManagerDTO> Managers { get; set; }
        public DbSet<BuisnessTaskDTO> buisnessTasks { get; set; }
        public DbSet<OfferDTO> offers { get; set; }
        public DbSet<SosDTO> sosTable { get; set; }
        public DbSet<ShowOrdersDTO> showOrdersDTO { get; set; }
        public DbSet<RegionsDTO> regions { get; set; }

        public DbSet<ShowUsersDTO> showUsersDTO { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;UserId=root;Password=12345;database=kvadrobot;");
            
        }
    }
}
