using System;
using System.Configuration;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;
namespace TEMPO.DALC
{
	/// <summary>
	/// Base class for all Data Access Logic Components
	/// </summary>
	public class BaseDALC
	{
		private string _connectionstring = System.Configuration.ConfigurationManager.AppSettings["DataAccess.ConnectionString"];
        protected SqlDatabase tempodb;
		public BaseDALC() {
            tempodb = new SqlDatabase(_connectionstring);
        }

		protected string DBConnectionString {
			get { return _connectionstring; }
		}

	}
}
