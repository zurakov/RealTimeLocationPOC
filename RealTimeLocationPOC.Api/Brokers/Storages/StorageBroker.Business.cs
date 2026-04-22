using Microsoft.EntityFrameworkCore;
using RealTimeLocationPOC.Api.Models;

namespace RealTimeLocationPOC.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Business> Businesses { get; set; }

        public async ValueTask<Business> InsertBusinessAsync(Business business)
        {
            var entityEntry = await this.Businesses.AddAsync(business);
            await this.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async ValueTask<Business> SelectBusinessByIdAsync(Guid businessId)
        {
            return await this.Businesses.FindAsync(businessId);
        }
    }
}
