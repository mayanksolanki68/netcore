using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpManagement.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext context;
        private readonly ILogger<SQLEmployeeRepository> _Logger;
        public SQLEmployeeRepository(AppDbContext context,
            ILogger<SQLEmployeeRepository> logger)
        {
            this.context = context;
            this._Logger = logger;
        }

        

        public Employee Add(Employee employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public Employee Delete(int Id)
        {
            Employee employee = context.Employees.Find(Id);
            if(employee!=null)
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return context.Employees;
        }

        public Employee GetEmployee(int Id)
        {
            _Logger.LogTrace("Trace Log");
            _Logger.LogDebug("Debug Log");
            _Logger.LogInformation("Information Log");
            _Logger.LogWarning("Warning Log");
            _Logger.LogCritical("Critical Log");

            return context.Employees.Find(Id);
        }

        public Employee Update(Employee employeeChanges)
        {
            var employee= context.Employees.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return employeeChanges;
        }
    }
}
