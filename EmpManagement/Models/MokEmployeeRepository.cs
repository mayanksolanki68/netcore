using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpManagement.Models
{
    public class MokEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;
        public MokEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee(){ Id=1,Name="Mayank",Department=Dept.HR,Email="mayank@gmail.com"},
                new Employee(){ Id=2,Name="Taksh",Department=Dept.IT,Email="taksh@gmail.com"},
                new Employee(){ Id=3,Name="Bhavesh",Department=Dept.Payroll,Email="bhavesh@gmail.com"},
                new Employee(){ Id=4,Name="Ajay",Department=Dept.None,Email="ajay@gmail.com"},
                new Employee(){ Id=5,Name="Arman",Department=Dept.Sales,Email="arman@gmail.com"}
            };
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(x => x.Id == Id);
        }
        public Employee Add(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Update(Employee EmployeeChanges)
        {
            Employee employee = _employeeList.FirstOrDefault(x => x.Id == EmployeeChanges.Id);
            if (employee != null)
            {
                employee.Name = EmployeeChanges.Name;
                employee.Email = EmployeeChanges.Email;
                employee.Department = EmployeeChanges.Department;
                _employeeList.Remove(employee);
            }
            return employee;
        }

        public Employee Delete(int Id)
        {
            Employee employee = _employeeList.FirstOrDefault(x => x.Id == Id);
            if (employee != null)
            {
                _employeeList.Remove(employee);
            }
            return employee;

        }
    }
}
