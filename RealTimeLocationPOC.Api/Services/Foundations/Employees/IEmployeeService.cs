using RealTimeLocationPOC.Api.Models;

namespace RealTimeLocationPOC.Api.Services.Foundations.Employees
{
    public interface IEmployeeService
    {
        ValueTask<Employee> AddEmployeeAsync(Employee employee);
        ValueTask<Employee> RetrieveEmployeeByIdAsync(Guid employeeId);
        ValueTask<Employee> ModifyEmployeeAsync(Employee employee);
        IQueryable<Employee> RetrieveAllEmployees();
    }
}
