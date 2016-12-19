using System;
using System.Windows.Forms;
using TEMPO.BusinessObjects.Entity;

namespace TEMPO.Client.UIElements
{
	/// <summary>
	/// A list view item that also contains an associated object for easy access
	/// </summary>
	public class ListItemID
	{
		private TEMPO.BusinessObjects.Entity.BusinessEntity m_bo;
		private string m_text;

		public ListItemID(string ItemText, TEMPO.BusinessObjects.Entity.BusinessEntity bo){
			m_bo = bo;	
			m_text = ItemText;
		}

		public TEMPO.BusinessObjects.Entity.BusinessEntity BusinessObject {
			get { return m_bo; }
			set { m_bo = value; }
		}

		public override string ToString() {
			return m_text;
		}


	}
}
