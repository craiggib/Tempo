using System;
using System.Windows.Forms;
using System.Drawing;

namespace TEMPO.Client.UIElements
{
	/// <summary>
	/// Summary description for GraphicLabel.
	/// </summary>
	public class GraphicLabel : Label
	{
		public GraphicLabel(Point location, Size size) {
			this.Location = location;
			this.Size = size;
		}

		public void setGraphic (Bitmap graphicfile) {
			this.BackgroundImage = graphicfile;
		}
	}
}
