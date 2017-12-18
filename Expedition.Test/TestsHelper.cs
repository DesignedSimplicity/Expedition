using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Test
{
	public class TestsHelper
	{
		public static string GetAssemblyDirectory()
		{
			UriBuilder uri = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
			string path = Uri.UnescapeDataString(uri.Path);
			return Path.GetDirectoryName(path);
		}

		public static string GetTestRootDirectory()
		{
			string path = GetAssemblyDirectory();
			var length = path.Length;
			if (path.EndsWith(@"\bin\debug", StringComparison.InvariantCultureIgnoreCase))
				return path.Substring(0, length - 10);
			else if (path.EndsWith(@"\bin\release", StringComparison.InvariantCultureIgnoreCase))
				return path.Substring(0, length - 11);
			else
				return Path.Combine(path, @"..\..\");
		}

		public static string GetTestDataDirectory()
		{
			return Path.Combine(GetTestRootDirectory(), "Data");
		}
	}
}
