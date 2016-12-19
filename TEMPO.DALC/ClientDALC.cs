using System;
using System.Data.SqlClient;
using System.Data;
using TEMPO.BusinessEntity;
using Microsoft.ApplicationBlocks.Data;

namespace TEMPO.DALC {
	/// <summary>
	/// Client Data Access Component
	/// </summary>
	public class ClientDALC : BaseDALC {
		
		public ClientDALC() { }

		/// <summary>
		/// Get all the Clients
		/// </summary>
		/// <returns>a strongly typed data set</returns>
		public ClientDS GetAllClients() {
			string sql;
			sql = "select * from Client;";

			ClientDS ds = new ClientDS();
			string[] tables = new string[] {"Client"};

			SqlParameter[] sqlparams = new SqlParameter[2];

			SqlParameter temp = new SqlParameter("@ClientID", SqlDbType.Int, 4, "ClientID");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[0] = temp;

			temp = new SqlParameter("@ClientName", SqlDbType.VarChar, 4, "ClientName");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[1] = temp;

			SqlHelper.FillDataset(this.DBConnectionString,System.Data.CommandType.Text, sql, ds, tables, sqlparams);
			return ds;
		}

		/// <summary>
		/// Update a Client
		/// </summary>
		/// <param name="ds"></param>
		public void Update (ClientDS ds) {
			string sql;
			SqlConnection conn = new SqlConnection(this.DBConnectionString);

			sql = "insert into Client (ClientName) values (@ClientName)";
			SqlCommand insertCommand = new SqlCommand(sql, conn);
			insertCommand.Parameters.Add("@ClientName", System.Data.SqlDbType.VarChar, 255, "ClientName");
			insertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;

			sql = "update Client set ClientName = @ClientName Where ClientID = @ClientID";
			SqlCommand updateCommand = new SqlCommand(sql,conn);
			updateCommand.Parameters.Add("@ClientName", System.Data.SqlDbType.VarChar, 255, "ClientName");
			updateCommand.Parameters.Add("@ClientID", System.Data.SqlDbType.Int, 4, "ClientID");

			sql = "delete from Client Where ClientID = @ClientID";
			SqlCommand deleteCommand = new SqlCommand(sql, conn);
			deleteCommand.Parameters.Add("@ClientID", System.Data.SqlDbType.Int, 4, "ClientID");

			try {
				SqlHelper.UpdateDataset(insertCommand,deleteCommand,updateCommand,ds,"Client");
			}
			catch {
				throw new Exception();
			}
		}


	}
}
