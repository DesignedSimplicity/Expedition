using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Win.UtiliTree
{
	public class IniProcessor
	{
		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

		public string Uri { get; private set; }

		public IniProcessor(string iniUri)
		{
			Uri = iniUri;
		}

		public void Write(string section, string key, string value)
		{
			WritePrivateProfileString(section, key, value, this.Uri);
		}

		public string Read(string section, string key)
		{
			StringBuilder temp = new StringBuilder(255);
			int i = GetPrivateProfileString(section, key, "", temp, 255, this.Uri);
			return temp.ToString();
		}
	}
}
