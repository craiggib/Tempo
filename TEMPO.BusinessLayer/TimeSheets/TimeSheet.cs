using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEMPO.Data;

namespace TEMPO.BusinessLayer.TimeSheets
{
    public class TimeSheet
    {
        public Data.TimeSheet GetTimeSheet(int timeSheetId)
        {
            TEMPOEntities dataConext = new TEMPOEntities();
            return dataConext.TimeSheets.FirstOrDefault(i => i.tid == timeSheetId);
        }

        public List<Data.TimeSheet> GetTimeSheets(int employeeId)
        {
            TEMPOEntities dataConext = new TEMPOEntities();
            return dataConext.TimeSheets.Where(i => i.empid == employeeId).ToList();
        }
    }
}
