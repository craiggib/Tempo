using System;
using System.Windows.Forms;
using System.Drawing;
using TEMPO.Client.UIElements;
using TEMPO.RequestBroker;
using TEMPO.BusinessEntity;

namespace TEMPO.Client {
	/// <summary>
	/// The Project CRUD Screen - used for updating, deleting and creating projects.
	/// </summary>
	public class ProjectCRUDEntryPanel : SubPanel	{
	
		# region Member Declarations

		private Label temp;
		private TextBox projectjobnum, referencenumber, description;
		private Button action, remove;
		private ProjectDS.ProjectRow _projectrow;
		private ComboBox _jobyear, _clients, _projectype;


		# endregion

		# region Public Initialization

		public ProjectCRUDEntryPanel(EntryType type) : base(new Point(378,47), new Size(408,303), "ProjectCRUDEntry", false){
			// UI Init
			this.BackColor = System.Drawing.Color.FromArgb(238,234,231);

			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(238,234,231);
			this.addLabel(new Point(10, 20), temp, "75", false, false, SubPanel.std_fontsize);

			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(238,234,231);
			this.addLabel(new Point(10, 50), temp, "76", false, false, SubPanel.std_fontsize);

			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(238,234,231);
			this.addLabel(new Point(10, 80), temp, "77", false, false, SubPanel.std_fontsize);

			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(238,234,231);
			this.addLabel(new Point(10, 110), temp, "78", false, false, SubPanel.std_fontsize);

			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(238,234,231);
			this.addLabel(new Point(10, 140), temp, "93", false, false, SubPanel.std_fontsize);

			// Description Label
			temp = new Label();
			temp.BackColor = System.Drawing.Color.FromArgb(238,234,231);
			this.addLabel(new Point(10, 170), temp, "121", false, false, SubPanel.std_fontsize);

			// build and populate the client drop down			
			_clients = new ComboBox();
			_clients.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			_clients.Size = new Size(200,21);
			_clients.DataSource = RequestBroker.TEMPOServerProxy.Instance.GetAllClients().Client;
			_clients.DisplayMember = "ClientName";
			_clients.ValueMember = "ClientID";			
			this.addControl(new Point(200, 20), _clients, false,false,SubPanel.std_fontsize);
		
			// build and populate the project type combo box
			_projectype = new ComboBox();
			_projectype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			_projectype.Size = new Size(200,21);
			_projectype.DataSource = RequestBroker.TEMPOServerProxy.Instance.GetProjectTypes().ProjectType;
			_projectype.DisplayMember = "ProjectTypeDesc";
			_projectype.ValueMember = "ProjectTypeID";
			this.addControl(new Point(200, 140), _projectype, false,false,SubPanel.std_fontsize);
			
			// build the job year combo box
			_jobyear = new ComboBox();
			_jobyear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			_jobyear.Size = new Size(200,21);
			_jobyear.DataSource = RequestBroker.TEMPOServerProxy.Instance.GetJobYears().JobYear;
			_jobyear.DisplayMember = "JobYear";
			_jobyear.ValueMember = "JobYearID";
			this.addControl(new Point(200, 50), _jobyear, false,false,SubPanel.std_fontsize);

			projectjobnum = new TextBox();
			projectjobnum.Size = new Size(200,21);
			this.addControl(new Point(200, 80), projectjobnum, false,false,SubPanel.std_fontsize);

			referencenumber = new TextBox();
			referencenumber.Size = new Size(200,21);
			this.addControl(new Point(200, 110), referencenumber, false,false,SubPanel.std_fontsize);

			description = new TextBox();
			description.Size = new Size(200,21);
			this.addControl(new Point(200, 170), description, false,false,SubPanel.std_fontsize);
			
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

		#region Enumerations for Entry Type
		public enum EntryType {
			Save = 0,
			Create = 1
		}

		#endregion

		# region CRUD Operations 

		/// <summary>
		/// Update a project row and send it to the server
		/// </summary>
		/// <param name="o"></param>
		/// <param name="e"></param>
		private void saveRecord(object o, EventArgs e) {
			if (validateRecord()) {
				// populate the entity
				_projectrow.JobNum = projectjobnum.Text;
				_projectrow.JobNumYear = (int)_jobyear.SelectedValue;
				_projectrow.ClientID = (int)_clients.SelectedValue;
				_projectrow.RefJobNum = referencenumber.Text;
				_projectrow.ProjectTypeID = (int)_projectype.SelectedValue;
				_projectrow.Description = description.Text;
				
				try {
					RequestBroker.TEMPOServerProxy.Instance.UpdateProjects((ProjectDS)_projectrow.Table.DataSet);
				}
				catch {
					MessageBox.Show(this.getStringResource("80"));
					return;
				}				
				// success message
				MessageBox.Show(this.getStringResource("81"));	
				// notify the world
				onRecordMaintained();
			}		
		}

		/// <summary>
		/// Create a project row and send it to the server
		/// </summary>
		/// <param name="o"></param>
		/// <param name="e"></param>
		private void createRecord(object o, EventArgs e) {
			if (validateRecord()) {
				// populate the business entity
				ProjectDS ds = new ProjectDS();
				ProjectDS.ProjectRow row = ds.Project.NewProjectRow();
				row.JobNum = projectjobnum.Text;
				row.JobNumYear = (int)_jobyear.SelectedValue;
				row.ClientID = (int)_clients.SelectedValue;
				row.RefJobNum = referencenumber.Text;
				row.ProjectTypeID = (int)_projectype.SelectedValue;
				row.Description = description.Text;
				// add it to the dataset
				ds.Project.AddProjectRow(row);
				
				// perform save action	
				try {
					RequestBroker.TEMPOServerProxy.Instance.UpdateProjects(ds);
				}
				catch {
					MessageBox.Show(this.getStringResource("82"));
					return;
				}

				// success message
				MessageBox.Show(this.getStringResource("83"));	
				// notify the world
				onRecordMaintained();
				// then reset the panel
				resetPanel();
			}		
		}

		private void removeRecord(object o, EventArgs e) {
			// verify deletion
			System.Windows.Forms.DialogResult dr = MessageBox.Show(this.getStringResource("84"), this.getStringResource("85"), System.Windows.Forms.MessageBoxButtons.YesNo);
			if (dr == System.Windows.Forms.DialogResult.Yes) {				
				// delete the row
				_projectrow.Delete();

				try {
					RequestBroker.TEMPOServerProxy.Instance.UpdateProjects((ProjectDS)_projectrow.Table.DataSet);
				}
				catch {
					// an error occured
					MessageBox.Show(this.getStringResource("86"));
					return;
				}
				// succesful removal
				MessageBox.Show(this.getStringResource("87"));
				// notify the world
				onRecordMaintained();
				// now hide this panel
				this.Visible = false;
			}
			else{ 
				// employee can't be deleted
				MessageBox.Show(this.getStringResource("88"));
				return;
			}
			
		}

		#endregion

		# region Helper Functions

		/// <summary>
		/// Reset the panel to its default state
		/// </summary>
		private void resetPanel() {
			_projectrow = null;
			projectjobnum.Text = "";
			_jobyear.SelectedIndex = -1;
			_clients.SelectedIndex = -1;
			_projectype.SelectedIndex = -1;
			referencenumber.Text = "";
		}

		/// <summary>
		/// Validate the record
		/// </summary>
		private bool validateRecord() {
			
			if (projectjobnum.Text == "") {
				MessageBox.Show(this.getStringResource("79"));
				return false;
			}			

			if (referencenumber.Text == "") {
				MessageBox.Show(this.getStringResource("89"));
				return false;
			}	

			if (_clients.SelectedIndex == -1) {
				MessageBox.Show(this.getStringResource("90"));
				return false;
			}	

			if (_jobyear.SelectedIndex == -1) {
				MessageBox.Show(this.getStringResource("91"));
				return false;
			}	

			if (_projectype.SelectedIndex == -1) {
				MessageBox.Show(this.getStringResource("92"));
				return false;
			}	

			if (description.Text == "") {
				MessageBox.Show(this.getStringResource("122"));
				return false;
			}	
			return true;
		}

		/// <summary>
		/// Populate the Screen
		/// </summary>
		/// <param name="projectrow"></param>
		public void Populate(ProjectDS.ProjectRow projectrow) {
			// assign the local member variable
			_projectrow = projectrow;

			// populate the form
			projectjobnum.Text = _projectrow.JobNum;
			referencenumber.Text = _projectrow.RefJobNum;
			description.Text = _projectrow.Description;

			// find the project in the list
			_clients.SelectedValue = _projectrow.ClientID;

			// find the project type in the list
			_projectype.SelectedValue = _projectrow.ProjectTypeID;

			// find the year in the list
			_jobyear.SelectedValue = _projectrow.JobNumYear;

		}


		# endregion

		# region Delegate Declarations 
		
		// create the delegates and events to monitor this objects existance
		public delegate void RecordMaintainDelegate();
		public event RecordMaintainDelegate onRecordMaintained;
		
		# endregion


	}
}
