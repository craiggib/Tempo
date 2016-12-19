using System;
using System.Windows.Forms;
using System.Drawing;
using TEMPO.Client.UIElements;
using TEMPO.BusinessEntity;
using TEMPO.Authorization;
using TEMPO.RequestBroker;

namespace TEMPO.Client {
	/// <summary>
	/// Entry/Save Panel for the Employee Entity
	/// </summary>
	public class EmployeeCRUDEntryPanel : SubPanel {
		
		# region Member Declarations
		
		private Label temp;
		private TextBox employeename, password, billablerate;
		private Button action, remove;
		private CheckBox active;
		private ListBox _listroles;
		private EmployeeDS.EmployeeRow _employeerow;
		private AuthorizationDS _authds;

		# endregion
 
		# region Panel UI Initialization 

		public EmployeeCRUDEntryPanel(EntryType type): base(new Point(378,47), new Size(408,303), "EmployeeCRUDEntry", false) {
			
			// UI Init
			this.BackColor = System.Drawing.Color.FromArgb(238,234,231);

			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(238,234,231);
			this.addLabel(new Point(10, 20), temp, "33", false, false, SubPanel.std_fontsize);

			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(238,234,231);
			this.addLabel(new Point(10, 50), temp, "34", false, false, SubPanel.std_fontsize);

			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(238,234,231);
			this.addLabel(new Point(10, 80), temp, "35", false, false, SubPanel.std_fontsize);

			employeename = new TextBox();
			employeename.Size = new Size(200,21);
			this.addControl(new Point(200, 20), employeename, false,false,SubPanel.std_fontsize);

			password = new TextBox();
			password.Size = new Size(200,21);
			this.addControl(new Point(200, 50), password, false,false,SubPanel.std_fontsize);

			billablerate = new TextBox();
			billablerate.Size = new Size(200,21);
			this.addControl(new Point(200, 80), billablerate, false,false,SubPanel.std_fontsize);

			// role information			
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(238,234,231);
			this.addLabel(new Point(10, 110), temp, "44", false, false, SubPanel.std_fontsize);

			_listroles = new ListBox(); 
			_listroles.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.addControl( new Point(200, 110), _listroles, false, false, SubPanel.std_fontsize);
			_listroles.Size = new Size(130, 80);

			// bind the data to the authorization information
			_authds = TEMPOServerProxy.Instance.GetAllAuthorizationRoles();
			_listroles.DataSource = _authds.Module;
			_listroles.DisplayMember = "ModuleName";
			_listroles.ValueMember = "ModuleID";

			// active info
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(238,234,231);
			this.addLabel(new Point(10, 200), temp, "47", false, false, SubPanel.std_fontsize);

			active = new CheckBox();
			active.Location = new Point(200, 200);
			active.Checked = false;
			Controls.Add(active);
		
			// set up the type of panel
			action = new Button();
			this.addControl(new Point(200, 250), action, false, false, SubPanel.std_fontsize);
			if (type == EntryType.Save) {
				action.Text = "SAVE";
				action.Click += new System.EventHandler(saveRecord);
				// on save, we also need remove
				remove = new Button();
				remove.Text = "DELETE";
				this.addControl(new Point(280, 250), remove, false, false, SubPanel.std_fontsize);
				remove.Click += new System.EventHandler(removeRecord);

			} else {
				action.Text = "CREATE";
				action.Click += new System.EventHandler(createRecord);			
			}
			
		}

		# endregion

		#region Public Enumaration

		/// <summary>
		/// Enumeration to establish the type of CRUD Panel to create
		/// </summary>
		public enum EntryType {
			Save = 0,
			Create = 1
		}
		#endregion
		
		#region Public UI Helpers

		/// <summary>
		/// Populate a record with a given employee
		/// </summary>
		/// <param name="emprow">The Employee Row that contains the typed data</param>
		public void populate(EmployeeDS.EmployeeRow emprow) {
			// assign the row to the private member for access later
			_employeerow = emprow;

			// update the user controls with the new data
			employeename.Text = _employeerow.EmployeeName;
			password.Text = _employeerow.Password;
			billablerate.Text = String.Format("{0:F}", _employeerow.Rate);
			if (_employeerow.Active == 1) active.Checked = true; else active.Checked = false;

			// get the authorization information
			_authds = TEMPOServerProxy.Instance.GetAuthorization(_employeerow.EmpID);
			_listroles.DataSource = _authds.Module;
			_listroles.DisplayMember = "ModuleName";
			_listroles.ValueMember = "ModuleID";
			
			// now update the value list
			for (int i = 0; i<_listroles.Items.Count; i++) {
				// attempt to find the module in the authorization list
				if ( _authds.Authorization.FindBymoduleid( _authds.Module[i].ModuleID ) != null)
					_listroles.SetSelected(i, true);
				else 
					_listroles.SetSelected(i, false);
			}
		}

		#endregion

		#region Private UI Helpers

		
		/// <summary>
		/// Validate all of the users input
		/// </summary>
		/// <returns>true if the all the validation rules are met, false otherwise</returns>
		private bool validateInput() {
			
			if (InputValidation.Instance.TestStringEmpty(employeename.Text)) {
				MessageBox.Show(this.getStringResource("36"));
				return false;
			}
			
			if (InputValidation.Instance.TestStringEmpty(password.Text)) {
				MessageBox.Show(this.getStringResource("37"));
				return false;
			}
			
			if (InputValidation.Instance.TestStringEmpty(billablerate.Text)) {
				MessageBox.Show(this.getStringResource("38"));
				return false;
			}


			if (!InputValidation.Instance.TestConvertToDecimal(billablerate.Text)) {
				MessageBox.Show(this.getStringResource("39"));
				return false;
			}
			
			return true;
		}

		/// <summary>
		/// This method will destroy the existing authorization table and rebuild it so that they are all new
		/// </summary>
		private void UpdateAuthorization () {
			// clear out the existing table
			_authds.Authorization.Clear();

			// iterate through the value list
			for (int i = 0; i<_listroles.Items.Count; i++) {
				// attempt to find the module in the authorization list
				if ( _authds.Authorization.FindBymoduleid( _authds.Module[i].ModuleID ) != null)
					_listroles.SetSelected(i, true);
				else 
					_listroles.SetSelected(i, false);
			}
		}

		/// <summary>
		/// Reset the Data Input Panel
		/// </summary>
		private void resetPanel() {			
			employeename.Text = "";
			password.Text = "";
			billablerate.Text = "";
		}

		#endregion

		#region Pricate UI Event Handlers
		
		/// <summary>
		/// Perform the Save Action
		/// Validate the input, then send the new entity to the proxy server
		/// </summary>
		private void saveRecord(object o, EventArgs e) {
			// first, validate the input
			if ( validateInput() ) {				
				// determine the active bit
				int isactive;
				if ( active.Checked ) isactive = 1; else isactive = 0;
				_employeerow.Active = isactive;
				_employeerow.EmployeeName = employeename.Text;
				_employeerow.Password = password.Text;
				_employeerow.Rate = Convert.ToDecimal(billablerate.Text);		

				try {
					// now update security
					// build up the authorization list based on the new values
					int[] roles = new int[_listroles.SelectedItems.Count];
					int rcount = 0;
					for (int i=0; i< _listroles.Items.Count; i++)
						if ( _listroles.SelectedItems.Contains(_listroles.Items[i]) )
							roles[rcount++] = _authds.Module[i].ModuleID;
                    
					// and pass it to the service interface
					RequestBroker.TEMPOServerProxy.Instance.UpdateEmployees((EmployeeDS)_employeerow.Table.DataSet, roles, _employeerow.EmpID);	
				
				}
				catch {
					MessageBox.Show(this.getStringResource("40"));
					return;
				}
				
				// success message
				MessageBox.Show(this.getStringResource("41"));	

				// notify the world through our event delegates
				onRecordMaintained();
			}		
		}
		

		/// <summary>
		/// Create the new Employee based on the user input
		/// Validate the input, then send the new entity to the server proxy
		/// </summary>
		private void createRecord(object o, EventArgs e) {
			// validate the input
			if ( validateInput() ) {
				// populate the business entity
				EmployeeDS eds = new EmployeeDS();
				EmployeeDS.EmployeeRow er = eds.Employee.NewEmployeeRow();
				er.EmployeeName = employeename.Text;
				er.Password = password.Text;
				er.Rate = Convert.ToDecimal(billablerate.Text);
				int isactive;
				if ( active.Checked ) isactive = 1; else isactive = 0;
				er.Active = isactive;

				// add it to the dataset
				eds.Employee.AddEmployeeRow(er);

				try {
					
					//  update security
					// build up the authorization list based on the new values
					int[] roles = new int[_listroles.SelectedItems.Count];
					int rcount = 0;
					for (int i=0; i< _listroles.Items.Count; i++)
						if ( _listroles.SelectedItems.Contains(_listroles.Items[i]) )
							roles[rcount++] = _authds.Module[i].ModuleID;
					
					// and pass it to the service interface
					RequestBroker.TEMPOServerProxy.Instance.CreateEmployee(eds, roles);

				}
				catch (Exception ex) {
					MessageBox.Show(this.getStringResource("42"));
					return;
				}


				// success message
				MessageBox.Show(this.getStringResource("43"));	
				// notify the world
				onRecordMaintained();
				// then reset the panel
				resetPanel();
			}		
		}

		private void removeRecord(object o, EventArgs e) {
			// verify deletion using a yes/no dialog
			System.Windows.Forms.DialogResult dr = MessageBox.Show(this.getStringResource("48"), this.getStringResource("49"), System.Windows.Forms.MessageBoxButtons.YesNo);
			
			if (dr == System.Windows.Forms.DialogResult.Yes) {
				// mark the row for deletion
				_employeerow.Delete();
				
				// deleting an employee may throw a sql exception through the layers
				try {
					// pass it to the service interface
					RequestBroker.TEMPOServerProxy.Instance.UpdateEmployees((EmployeeDS)_employeerow.Table.DataSet, new int[1], _employeerow.EmpID);
				} catch {
					// an error occured
					MessageBox.Show(this.getStringResource("50"));
					return;
				}
				
				// succesful removal
				MessageBox.Show(this.getStringResource("52"));
				
				// notify the world
				onRecordMaintained();
				
				// now hide this panel
				this.Visible = false;

			}
		}
		#endregion

		#region Public Delegate Declaration

		// create the delegates and events to monitor this objects existance
		public delegate void RecordMaintainDelegate();
		public event RecordMaintainDelegate onRecordMaintained;

		#endregion

	}
}
