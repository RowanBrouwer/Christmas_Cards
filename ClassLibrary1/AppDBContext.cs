using Christmas_Cards.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Christmas_Cards.DAL
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Images> Images{ get; set; }
        public DbSet<CardModel> Cards { get; set; }
        public DbSet<EmailModel> Emails { get; set; }
    }
}
