using Microsoft.EntityFrameworkCore;
using RealTimeLocationPOC.Api.Models;

namespace RealTimeLocationPOC.Api.Brokers.Storages
{
    public partial class StorageBroker : DbContext, IStorageBroker
    {
        private readonly IConfiguration configuration;

        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("RealTimeLocationDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            Guid businessId = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890");

            modelBuilder.Entity<Business>().HasData(
                new Business
                {
                    Id = businessId,
                    Name = "Contoso Ltd"
                }
            );

            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    BusinessId = businessId,
                    FullName = "Ahmad Al-Farsi",
                    IsOnline = false,
                    LastSeenAt = null
                },
                new Employee
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    BusinessId = businessId,
                    FullName = "Sara Johnson",
                    IsOnline = false,
                    LastSeenAt = null
                },
                new Employee
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    BusinessId = businessId,
                    FullName = "Dmitry Volkov",
                    IsOnline = false,
                    LastSeenAt = null
                }
            );
        }
    }
}
