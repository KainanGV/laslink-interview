using System.Collections.Generic;
using System.Reflection.Emit;
using LastLink.Anticipation.Domain.Entities;
using LastLink.Anticipation.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LastLink.Anticipation.Infra.Data
{
    public class AnticipationDbContext : DbContext
    {
        public AnticipationDbContext(DbContextOptions<AnticipationDbContext> options) : base(options) { }

        public DbSet<AnticipationRequest> AnticipationRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnticipationRequest>().ToTable("AnticipationRequests");
            modelBuilder.Entity<AnticipationRequest>().HasKey(ar => ar.Id);
            modelBuilder.Entity<AnticipationRequest>().Property(ar => ar.Status).HasConversion<string>();
        }
    }
}
