using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using TEMPO.Client.UIElements;
using TEMPO.RequestBroker;
using TEMPO.BusinessEntity;

namespace TEMPO.Client
{
	/// <summary>
	/// Summary description for TypeCRUDpanel.
	/// </summary>
	public class TypeCRUDPanel:SubPanel
	{
		#region Member Declarations

		private TypeCRUDEntryPanel entrypanel, createpanel;
		private GraphicButton create;
		private CRUDTreeViewDS ctv;

		#endregion

		# region Public Initalization

		public TypeCRUDPanel() : base(new Point(20,140), new Size(820,430), "TypeCRUD", false) {
			// background
			this.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.CRUDpanel.png"));

			// build the tree view			
			TEMPOServerProxy tsp = TEMPO.RequestBroker.TEMPOServerProxy.Instance;
			WorkTypeDS ds = tsp.GetAllWorkTypes();
			ctv = new CRUDTreeViewDS("Work Types", ds, "WorkTypeName");
			ctv.onCRUDItemClicked += new CRUDTreeViewDS.CRUDItemClickedDelegate(openEntity);

			ctv.Location = new Point(32,71);
			ctv.Size = new Size(306,222);

			this.Controls.Add(ctv);

			entrypanel = new TypeCRUDEntryPanel(TypeCRUDEntryPanel.EntryType.Save);
			entrypanel.Visible = false;
			entrypanel.onRecordMaintained += new TypeCRUDEntryPanel.RecordMaintainDelegate(refreshList);
			this.Controls.Add(entrypanel);

			createpanel = new TypeCRUDEntryPanel(TypeCRUDEntryPanel.EntryType.Create);
			createpanel.Visible = false;
			createpanel.onRecordMaintained += new TypeCRUDEntryPanel.RecordMaintainDelegate(refreshList);
			this.Controls.Add(createpanel);

			create = new GraphicButton(new Point(180,315), new Size(155,28));
			create.setGraphic(new Bitmap(typeof(TEMPO.Client.MainScreen), "Resources.addnewdocument.png"));
			create.Click += new System.EventHandler(newRecord);
			this.Controls.Add(create);
		}

		#endregion


		private void refreshList() {
			TEMPOServerProxy tsp = TEMPO.RequestBroker.TEMPOServerProxy.Instance;
			WorkTypeDS ds = tsp.GetAllWorkTypes();
			ctv.refreshTree(ds);
		}


		private void newRecord(object sender,EventArgs e) {
			createpanel.Visible = true;
			entrypanel.Visible = false;
		}

		private void openEntity(DataRow dr) {


			WorkTypeDS.WorkTypeRow worktyperow = (WorkTypeDS.WorkTypeRow) dr;
			createpanel.Visible = false;
			entrypanel.Visible = true;
			entrypanel.Populate(worktyperow);
		}

	}
}
