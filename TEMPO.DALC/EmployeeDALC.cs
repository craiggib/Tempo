using System;
using System.Data.SqlClient;
using System.Data;
using TEMPO.BusinessEntity;
using Microsoft.ApplicationBlocks.Data;

namespace TEMPO.DALC
{
	/// <summary>
	/// Data Access Logic Component for the Employee Business Entity
	/// </summary>
	public class EmployeeDALC : BaseDALC {
		public EmployeeDALC() { }

		/// <summary>
		/// Get a specific Employee
		/// </summary>
		/// <param name="employeeid">the id of the employee to get</param>
		/// <returns>a strongly typed employee data set</returns>
		public EmployeeDS GetAllEmployees() {
			string sql;
			sql = "select * from Employee;";

			EmployeeDS empds = new EmployeeDS();
			string[] tables = new string[] {"Employee"};

			SqlParameter[] sqlparams = new SqlParameter[5];

			SqlParameter temp = new SqlParameter("@EmpID", SqlDbType.Int, 4, "EmpID");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[0] = temp;

			temp = new SqlParameter("@EmployeeName", SqlDbType.VarChar, 4, "EmployeeName");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[1] = temp;

			temp = new SqlParameter("@Password", SqlDbType.VarChar, 4, "Password");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[2] = temp;

			temp = new SqlParameter("@Rate", SqlDbType.Decimal, 4, "Rate");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[3] = temp;

			temp = new SqlParameter("@Active", SqlDbType.Bit, 4, "Active");
			temp.Direction = System.Data.ParameterDirection.Output;
			sqlparams[4] = temp;

			SqlHelper.FillDataset(this.DBConnectionString,System.Data.CommandType.Text, sql, empds, tables, sqlparams);
			return empds;
		}

		public void Update (EmployeeDS ds) {
			string sql;
			SqlConnection conn = new SqlConnection(this.DBConnectionString);

			sql = "insert into Employee (EmployeeName, Password, Rate, Active) values (@EmployeeName, @Password, @Rate, @Active); SELECT @@IDENTITY as EmpID";
			SqlCommand insertCommand = new SqlCommand(sql, conn);
			insertCommand.Parameters.Add("@EmployeeName", System.Data.SqlDbType.VarChar, 255, "EmployeeName");
			insertCommand.Parameters.Add("@Password", System.Data.SqlDbType.VarChar, 15, "Password");
			insertCommand.Parameters.Add("@Rate", System.Data.SqlDbType.Decimal, 9, "Rate");
			insertCommand.Parameters.Add("@Active", System.Data.SqlDbType.Bit, 1, "Active");
			insertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;

			sql = "update Employee set EmployeeName = @EmployeeName, Password = @Password, Rate = @Rate, Active = @Active Where Empid = @EmpID";
			SqlCommand updateCommand = new SqlCommand(sql,conn);
			updateCommand.Parameters.Add("@EmployeeName", System.Data.SqlDbType.VarChar, 255, "EmployeeName");
			updateCommand.Parameters.Add("@Password", System.Data.SqlDbType.VarChar, 15, "Password");
			updateCommand.Parameters.Add("@Rate", System.Data.SqlDbType.Decimal, 9, "Rate");
			updateCommand.Parameters.Add("@Active", System.Data.SqlDbType.Bit, 1, "Active");
			updateCommand.Parameters.Add("@EmpID", System.Data.SqlDbType.Int, 4, "EmpID");

			sql = "delete from Employee Where Empid = @EmpID";
			SqlCommand deleteCommand = new SqlCommand(sql, conn);
			deleteCommand.Parameters.Add("@EmpID", System.Data.SqlDbType.Int, 4, "EmpID");

			try {
				SqlHelper.UpdateDataset(insertCommand,deleteCommand,updateCommand,ds,"Employee");
			}
			catch {
				throw new Exception();
			}
		}

	
	}
}
