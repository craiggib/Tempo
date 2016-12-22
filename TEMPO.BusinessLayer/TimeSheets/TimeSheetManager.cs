using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEMPO.Data;

namespace TEMPO.BusinessLayer.TimeSheets
{
    public class TimesheetManager
    {
        public Data.TimeSheet GetTimeSheet(int timeSheetId)
        {
            TempoDbContext dataConext = new TempoDbContext();
            return dataConext.TimeSheets.FirstOrDefault(i => i.tid == timeSheetId);
        }

        public List<Data.TimeSheet> GetTimeSheets(int employeeId, List<TimesheetStatus> status)
        {
            var statusInts = status.Select(i => (int)i);
            TempoDbContext dataConext = new TempoDbContext();
            return dataConext.TimeSheets.Where(i => i.statusid.HasValue && statusInts.Contains(i.statusid.Value) && i.empid == employeeId).ToList();
        }
    }
}
