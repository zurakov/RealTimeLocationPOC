using RealTimeLocationPOC.Api.Models;

namespace RealTimeLocationPOC.Api.Brokers.Storages
{
    public interface IStorageBroker
    {
        ValueTask<Employee> InsertEmployeeAsync(Employee employee);
        ValueTask<Employee> SelectEmployeeByIdAsync(Guid employeeId);
        ValueTask<Employee> UpdateEmployeeAsync(Employee employee);
        IQueryable<Employee> SelectAllEmployees();

        ValueTask<LocationPing> InsertLocationPingAsync(LocationPing locationPing);
        IQueryable<LocationPing> SelectAllLocationPings();

        ValueTask<Business> InsertBusinessAsync(Business business);
        ValueTask<Business> SelectBusinessByIdAsync(Guid businessId);
    }
}
