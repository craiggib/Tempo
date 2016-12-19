using System;
using System.Windows.Forms;
using System.Drawing;
using TEMPO.Client.UIElements;
using System.Data;
using TEMPO.RequestBroker;
using TEMPO.BusinessEntity;

namespace TEMPO.Client
{
	/// <summary>
	/// Summary description for ProjectCRUDPanel.
	/// </summary>
	public class ProjectCRUDPanel : SubPanel
	{
		private ProjectCRUDEntryPanel entrypanel, createpanel;
		private GraphicButton create;
		private CRUDTreeViewDS ctv;

		public ProjectCRUDPanel() : base(new Point(20,140), new Size(820,430), "ProjectCRUD", false) {
			// background
			this.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.CRUDpanel.png"));
			
			// build the tree view
			// obtain the data from the server
			TEMPOServerProxy tsp = TEMPO.RequestBroker.TEMPOServerProxy.Instance;
			ProjectDS ds = tsp.GetProjectsList();
			ctv = new CRUDTreeViewDS("Projects",ds, "ProjectName");
			ctv.onCRUDItemClicked += new CRUDTreeViewDS.CRUDItemClickedDelegate(openEntity);

			ctv.Location = new Point(32,71);
			ctv.Size = new Size(306,222);

			this.Controls.Add(ctv);

			entrypanel = new ProjectCRUDEntryPanel(ProjectCRUDEntryPanel.EntryType.Save);
			entrypanel.Visible = false;
			entrypanel.onRecordMaintained += new ProjectCRUDEntryPanel.RecordMaintainDelegate(refreshList);
			this.Controls.Add(entrypanel);

			createpanel = new ProjectCRUDEntryPanel(ProjectCRUDEntryPanel.EntryType.Create);
			createpanel.Visible = false;
			createpanel.onRecordMaintained += new ProjectCRUDEntryPanel.RecordMaintainDelegate(refreshList);
			this.Controls.Add(createpanel);

			create = new GraphicButton(new Point(180,315), new Size(155,28));
			create.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.addnewdocument.png"));
			create.Click += new System.EventHandler(newRecord);
			this.Controls.Add(create);
		}

		private void refreshList() {
			TEMPOServerProxy tsp = TEMPO.RequestBroker.TEMPOServerProxy.Instance;
			ProjectDS ds = tsp.GetProjectsList();
			ctv.refreshTree(ds);
		}


		private void newRecord(object sender,EventArgs e) {
			createpanel.Visible = true;
			entrypanel.Visible = false;
		}

		private void openEntity(DataRow dr) {
			// in this case, the business entity is an employee
			ProjectDS.ProjectRow projectrow = (ProjectDS.ProjectRow) dr;
			createpanel.Visible = false;
			entrypanel.Visible = true;
			entrypanel.Populate(projectrow);
		}
	}
}
