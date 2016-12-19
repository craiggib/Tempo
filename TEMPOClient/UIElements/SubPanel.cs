using System;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using System.Resources;

namespace TEMPO.Client.UIElements
{
	/// <summary>
	/// Summary description for SubPanel.
	/// </summary>
	public class SubPanel : Panel {

		private string m_resourceName;
		private string m_resourceLocation = "Resources/TextResources.xml";
		public static float std_fontsize = 8;
		public static string std_fontname = "Trebuchet MS";
		protected Font m_panelFont = new Font(std_fontname, std_fontsize);
		
		private XmlDocument xml;
	
		public SubPanel(Point plocation, Size psize, string ResourceName, bool autoscroll) {
			this.Location = plocation;
			this.Size = psize;
			this.BackColor = System.Drawing.Color.Transparent;
			m_resourceName = ResourceName;
			this.AutoScroll = autoscroll;
		
		}

		/// <summary>
		/// Set the current font to bold
		/// </summary>
		private bool SetFontBold {
			get { return m_panelFont.Bold; }
			set {
				if (value)
                    m_panelFont = new Font(m_panelFont,m_panelFont.Style | FontStyle.Bold);
				else
					m_panelFont = new Font(m_panelFont,m_panelFont.Style & (~FontStyle.Bold));
			}
		}
		
		/// <summary>
		/// Set the current font to italic
		/// </summary>
		private bool SetFontItalic {
			get { return m_panelFont.Bold; }
			set {
				if (value)
					m_panelFont = new Font(m_panelFont,m_panelFont.Style | FontStyle.Italic);
				else
					m_panelFont = new Font(m_panelFont,m_panelFont.Style & (~FontStyle.Italic));
			}
		}
		
		/// <summary>
		/// Gets or Sets the current font size
		/// </summary>
		private float SetFontSize {
			get { return m_panelFont.Size; }
			set {m_panelFont = new Font(std_fontname,value,m_panelFont.Style);}
		}
		public void showPanel() {
			this.Show();
		}
		
		public void hidePanel() {
			this.Hide();
		}

		public void setGraphic (Bitmap graphicfile) {
			this.BackgroundImage = graphicfile;
		}

		/// <summary>
		/// Adds a label to the control and parses the resource xml document 
		/// to obtain the name the text value for the label
		/// </summary>
		public void addLabel(Point location, Label label, string ResourceID, bool bold, bool italic, float textsize) {			
			// load the text resources
			xml = new XmlDocument();
			xml.Load(m_resourceLocation);
			XmlNode rootNode = xml.DocumentElement;			
			// build the xpath query based on this objects local params
			string xpathq = "//Panel[@name='" + m_resourceName + "']/labelvalues/text[@id='" + ResourceID + "']";
			
			// build the label details
			SetFontBold = bold;
			SetFontItalic = italic;
			SetFontSize = textsize;
			label.Text = rootNode.SelectSingleNode(xpathq).InnerText;
			label.Font = m_panelFont;
			label.Location = location;
			label.Size = new System.Drawing.Size(determineLabelWidth(label.Text),determineLabelHeight());
			// add the event handler to resize if text is changed
			label.TextChanged += new System.EventHandler(onTextChange);

			// then add the label to the panel
			this.Controls.Add(label);

		}

		/// <summary>
		/// Adds a label to the control with no default text
		/// </summary>
		public void addLabel(Point location, Label label,  bool bold, bool italic, float textsize) {			
			// build the label details
			SetFontBold = bold;
			SetFontItalic = italic;
			SetFontSize = textsize;
			label.Font = m_panelFont;
			label.Location = location;
			label.Size = new System.Drawing.Size(determineLabelWidth(label.Text),determineLabelHeight());
			// add the event handler to resize if text is changed
			label.TextChanged += new System.EventHandler(onTextChange);

			// then add the label to the panel
			this.Controls.Add(label);
		}

		/// <summary>
		/// Event Handler for text updates
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void onTextChange(object sender, EventArgs e) {
			Label l = (Label) sender;
			l.Size = new System.Drawing.Size(determineLabelWidth(l.Text),determineLabelHeight());
		}

		/// <summary>
		/// Utilized for non label components
		/// </summary>
		public void addControl(Point location, Control control,  bool bold, bool italic, float textsize) {						
			// build the label details
			SetFontBold = bold;
			SetFontItalic = italic;
			SetFontSize = textsize;
			control.Font = m_panelFont;
			control.Location = location;
			// then add the label to the panel
			this.Controls.Add(control);
		}


		/// <summary>
		/// Adds a textbox to the panel with the specified font
		/// </summary>
		public void addTextBox(Point location, TextBox textbox) {
			
			textbox.Font = m_panelFont;
			textbox.Location = location;
			// then add the label to the panel
			this.Controls.Add(textbox);
		

		}

		// build the length based on the nubmer o
		private int determineLabelHeight() {  
			return (int) Math.Ceiling((double)m_panelFont.GetHeight()); 
		}
		private int determineLabelWidth(string TextToMeasure) { 
			SizeF size = this.CreateGraphics().MeasureString(TextToMeasure,m_panelFont);
			return (int) Math.Ceiling((double)size.Width);
		}

		/// <summary>
		/// Load the specified String Resource (used for localization)
		/// </summary>
		/// <param name="resourceID"></param>
		/// <returns></returns>
		protected string getStringResource(string resourceID) {
			// load the text resources
			xml = new XmlDocument();
			xml.Load(m_resourceLocation);
			XmlNode rootNode = xml.DocumentElement;			
			// build the xpath query based on this objects local params
			string xpathq = "//Panel[@name='" + m_resourceName + "']/stringvalues/text[@id='" + resourceID + "']";
			return (rootNode.SelectSingleNode(xpathq).InnerText);
			
		}

//		
//		protected bool validateAsFloat(string input) {
//			//validate each text box before trying to calculate 
//			for (int j = 0; j < 18; j++) {
//				t = m_putts[j];
//				for (int i = 0; i < t.Text.Length; i++) {
//					if (!
//						(	(t.Text[i] == '1') || (t.Text[i] == '2') || (t.Text[i] == '3') || (t.Text[i] == '4') || 
//						(t.Text[i] == '5') || (t.Text[i] == '6') || (t.Text[i] == '7') || (t.Text[i] == '8') ||
//						(t.Text[i] == '9') || (t.Text[i] == '0')
//						)
//						)
//						valid = false;
//				}
//			}
//		}
	}
}
