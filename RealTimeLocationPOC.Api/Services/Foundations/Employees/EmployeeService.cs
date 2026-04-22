using RealTimeLocationPOC.Api.Brokers.Storages;
using RealTimeLocationPOC.Api.Models;

namespace RealTimeLocationPOC.Api.Services.Foundations.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IStorageBroker storageBroker;

        public EmployeeService(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }

        public async ValueTask<Employee> AddEmployeeAsync(Employee employee) =>
            await this.storageBroker.InsertEmployeeAsync(employee);

        public async ValueTask<Employee> RetrieveEmployeeByIdAsync(Guid employeeId) =>
            await this.storageBroker.SelectEmployeeByIdAsync(employeeId);

        public async ValueTask<Employee> ModifyEmployeeAsync(Employee employee) =>
            await this.storageBroker.UpdateEmployeeAsync(employee);

        public IQueryable<Employee> RetrieveAllEmployees() =>
            this.storageBroker.SelectAllEmployees();
    }
}
