using System;
using System.Windows.Forms;
using System.Drawing;
using TEMPO.Client.UIElements;
using TEMPO.RequestBroker;
using TEMPO.BusinessEntity;
using System.Data;

namespace TEMPO.Client 
{
	/// <summary>
	/// Summary description for EmployeeCRUD.
	/// </summary>
	public class EmployeeCRUDPanel : SubPanel
	{

		EmployeeCRUDEntryPanel entrypanel, createpanel;
		GraphicButton create;
		CRUDTreeViewDS ctv;

		public EmployeeCRUDPanel() : base(new Point(20,140), new Size(820,430), "EmployeeCRUD", false) {
			// background
			this.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.CRUDpanel.png"));
			
			// obtain the data from the server
			TEMPOServerProxy tsp = TEMPO.RequestBroker.TEMPOServerProxy.Instance;
			EmployeeDS eds = tsp.GetAllEmployees();
			// populate the control
			ctv = new CRUDTreeViewDS("Employees",eds, "EmployeeName");
			// assign the event handler
			ctv.onCRUDItemClicked += new CRUDTreeViewDS.CRUDItemClickedDelegate(openEntity);
			ctv.Location = new Point(32,71);
			ctv.Size = new Size(306,222);

			this.Controls.Add(ctv);

			entrypanel = new EmployeeCRUDEntryPanel(EmployeeCRUDEntryPanel.EntryType.Save);
			entrypanel.Visible = false;
			entrypanel.onRecordMaintained += new EmployeeCRUDEntryPanel.RecordMaintainDelegate(refreshList);
			this.Controls.Add(entrypanel);

			createpanel = new EmployeeCRUDEntryPanel(EmployeeCRUDEntryPanel.EntryType.Create);
			createpanel.Visible = false;
			createpanel.onRecordMaintained += new EmployeeCRUDEntryPanel.RecordMaintainDelegate(refreshList);
			this.Controls.Add(createpanel);

			create = new GraphicButton(new Point(180,315), new Size(155,28));
			create.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.addnewdocument.png"));
			create.Click += new System.EventHandler(newRecord);
			this.Controls.Add(create);
		}

		private void refreshList() {
			TEMPOServerProxy tsp = TEMPO.RequestBroker.TEMPOServerProxy.Instance;
			EmployeeDS eds = tsp.GetAllEmployees();
			ctv.refreshTree(eds);
		}


		private void newRecord(object sender,EventArgs e) {
			createpanel.Visible = true;
			entrypanel.Visible = false;
		}

		private void openEntity(DataRow dr) {
			// in this case, the business entity is an employee
			EmployeeDS.EmployeeRow emprow = (EmployeeDS.EmployeeRow) dr;
			createpanel.Visible = false;
			entrypanel.Visible = true;
			entrypanel.populate(emprow);
		}


	}
}
