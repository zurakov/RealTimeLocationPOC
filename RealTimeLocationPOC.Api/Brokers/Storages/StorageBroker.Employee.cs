using Microsoft.EntityFrameworkCore;
using RealTimeLocationPOC.Api.Models;

namespace RealTimeLocationPOC.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Employee> Employees { get; set; }

        public async ValueTask<Employee> InsertEmployeeAsync(Employee employee)
        {
            var entityEntry = await this.Employees.AddAsync(employee);
            await this.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async ValueTask<Employee> SelectEmployeeByIdAsync(Guid employeeId)
        {
            return await this.Employees.FindAsync(employeeId);
        }

        public async ValueTask<Employee> UpdateEmployeeAsync(Employee employee)
        {
            var entityEntry = this.Employees.Update(employee);
            await this.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public IQueryable<Employee> SelectAllEmployees()
        {
            return this.Employees;
        }
    }
}
