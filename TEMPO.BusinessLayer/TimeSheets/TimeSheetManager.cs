using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEMPO.Data;

namespace TEMPO.BusinessLayer.TimeSheets
{
    public class TimeSheetManager
    {
        public Data.TimeSheet GetTimeSheet(int timeSheetId)
        {
            TempoDbContext dataConext = new TempoDbContext();
            return dataConext.TimeSheets.FirstOrDefault(i => i.tid == timeSheetId);
        }

        public List<Data.TimeSheet> GetTimeSheets(int employeeId)
        {
            TempoDbContext dataConext = new TempoDbContext();
            return dataConext.TimeSheets.Where(i => i.empid == employeeId).ToList();
        }
    }
}
