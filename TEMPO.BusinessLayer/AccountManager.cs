using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEMPO.Data;

namespace TEMPO.BusinessLayer
{
    public class AccountManager : BaseManager
    {
        public Employee Login(string userName, string password)
        {
            return DataConext.Employees.FirstOrDefault(i => i.employeename == userName && i.password == password);
        }
    }
}
