using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEMPO.Model;

namespace TEMPO.BusinessLayer
{
    public class AccountManager : BaseManager
    {
        public Employee Login(string userName, string password)
        {
            return DataContext.Employees.FirstOrDefault(i => i.employeename == userName && i.password == password);
        }

        public List<Employee> GetEmployees()
        {
            return DataContext.Employees.ToList();            
        }

        public Employee GetEmployee(int employeeId)
        {
            return DataContext.Employees.FirstOrDefault(i => i.empid == employeeId);
        }

        public void UpdateEmployee(int employeeId, string employeeName, string password, decimal rate, bool active)
        {
            Employee employee = GetEmployee(employeeId);
            if(employee != null)
            {
                employee.employeename = employeeName;
                employee.password = password;
                employee.rate = rate;
                employee.active = active;
                DataContext.SaveChanges();
            }
        }

        public void CreateEmployee(string employeeName, string password, decimal rate)
        {
            Employee employee = new Employee
            {
                employeename = employeeName,
                password = password,
                rate = rate,
                active = true
            };
            DataContext.Employees.Add(employee);
            DataContext.SaveChanges();
        }
    }
}
