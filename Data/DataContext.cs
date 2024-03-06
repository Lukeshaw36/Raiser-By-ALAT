using GROUP2.Models;
using Microsoft.EntityFrameworkCore;

namespace GROUP2.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        { }

        public DbSet<User> Users { get; set; }
       // public DbSet<Interest> Interests { get; set; }
        //public DbSet<InvestmentInterest> InvestmentInterests { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>()
            //    .HasOne(u => u.Profile)
            //    .WithOne(p => p.User)
            //    .HasForeignKey<UserProfile>(p => p.UserId);

            base.OnModelCreating(modelBuilder);
        }

        //public DataContext(DbContextOptions<DataContext> options) : base(options) 
        //{ }

        //public DbSet<User> Users { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<Investment> Investments { get; set; }


    }
    
}
