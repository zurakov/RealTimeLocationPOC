using Microsoft.EntityFrameworkCore;
using RealTimeLocationPOC.Api.Models;

namespace RealTimeLocationPOC.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<LocationPing> LocationPings { get; set; }

        public async ValueTask<LocationPing> InsertLocationPingAsync(LocationPing locationPing)
        {
            var entityEntry = await this.LocationPings.AddAsync(locationPing);
            await this.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public IQueryable<LocationPing> SelectAllLocationPings()
        {
            return this.LocationPings;
        }
    }
}
