using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEMPO.Model;
using TEMPO.Data;
namespace TEMPO.BusinessLayer
{
    public abstract class BaseManager
    {

        protected TempoContext DataContext { get; private set; }

        public BaseManager()
        {
            DataContext = new TempoContext();
        }
        
    }
}
