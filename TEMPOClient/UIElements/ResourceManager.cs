using System;
using System.Xml;

namespace TEMPO.Client.UIElements
{
	/// <summary>
	/// Singleton pattern implemented to access resource files
	/// </summary>
	public class ResourceManager
	{
		private const string _resourceFileLocation = "Resources/TextResources.xml";
		private static XmlDocument _resourcefile;
		private static ResourceManager _instance;

		private ResourceManager() {
			_resourcefile = new XmlDocument();
			_resourcefile.Load(_resourceFileLocation);
		}

		/// <summary>
		/// Get a reference to the Resource Manager
		/// </summary>
		public ResourceManager Instance {
			get { 
				if (_instance == null) _instance = new ResourceManager();
				return _instance;
			}
		}

		/// <summary>
		/// Get the localized Resource Value based on the Resource Set and Id
		/// </summary>
		/// <param name="ResourceSet">The name of the resource set to access</param>
		/// <param name="ResourceID">the specific id to access</param>
		/// <returns></returns>
		public static string GetResourceValue(string ResourceSet, string ResourceID) {
			// build access to the document
			XmlNode rootNode = _resourcefile.DocumentElement;			
			// build the xpath query based on this objects local params
			string xpathq = "//Panel[@name='" + ResourceSet + "']/labelvalues/text[@id='" + ResourceID + "']";			
			// return the value
			return (rootNode.SelectSingleNode(xpathq).InnerText);
		}
		
	}
}
