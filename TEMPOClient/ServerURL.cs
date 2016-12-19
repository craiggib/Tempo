using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;

namespace TEMPO.Client
{
	/// <summary>
	/// Summary description for ServerURL.
	/// </summary>
	public class ServerURL : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox URL;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.Button Save;
		private System.Windows.Forms.Label lbl_serverurl;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;

		System.Configuration.AppSettingsReader configurationAppSettings;

		public ServerURL()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// load the app settings
			configurationAppSettings = new System.Configuration.AppSettingsReader();
			string currentsetting = ((string)(configurationAppSettings.GetValue("TEMPO.BusinessObjects.TEMPOServer.Tempo", typeof(string))));
			URL.Text = currentsetting;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.URL = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.Cancel = new System.Windows.Forms.Button();
			this.Save = new System.Windows.Forms.Button();
			this.lbl_serverurl = new System.Windows.Forms.Label();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.SuspendLayout();
			// 
			// URL
			// 
			this.URL.Location = new System.Drawing.Point(16, 64);
			this.URL.Name = "URL";
			this.URL.Size = new System.Drawing.Size(424, 20);
			this.URL.TabIndex = 0;
			this.URL.Text = "";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(448, 64);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(96, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Test Connection";
			// 
			// Cancel
			// 
			this.Cancel.Location = new System.Drawing.Point(464, 112);
			this.Cancel.Name = "Cancel";
			this.Cancel.TabIndex = 1;
			this.Cancel.Text = "Cancel";
			// 
			// Save
			// 
			this.Save.Location = new System.Drawing.Point(376, 112);
			this.Save.Name = "Save";
			this.Save.TabIndex = 1;
			this.Save.Text = "Save";
			this.Save.Click += new System.EventHandler(this.Save_Click);
			// 
			// lbl_serverurl
			// 
			this.lbl_serverurl.Location = new System.Drawing.Point(16, 40);
			this.lbl_serverurl.Name = "lbl_serverurl";
			this.lbl_serverurl.Size = new System.Drawing.Size(112, 24);
			this.lbl_serverurl.TabIndex = 2;
			this.lbl_serverurl.Text = "TEMPO Server URL";
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateTimePicker1.Location = new System.Drawing.Point(24, 104);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(96, 20);
			this.dateTimePicker1.TabIndex = 3;
			// 
			// ServerURL
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(552, 150);
			this.Controls.Add(this.dateTimePicker1);
			this.Controls.Add(this.lbl_serverurl);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.URL);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.Save);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ServerURL";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "TEMPO Server URL";
			this.ResumeLayout(false);

		}
		#endregion

		private void Save_Click(object sender, System.EventArgs e) {
			XmlDocument xml = new XmlDocument();
			try {
				xml.LoadXml("TEMPO.Client.exe.config");
			}catch (System.Xml.XmlException xmle) {
				MessageBox.Show(xmle.Message);
			}
	
		}
	}
}
