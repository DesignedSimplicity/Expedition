using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Core.Services
{
	public class QueryHashSystemResponse
	{
		public int BlankLines { get; set; } = 0;

		public List<string> Comments { get; protected set; } = new List<string>();

		public Dictionary<int, string> Invalid { get; protected set; } = new Dictionary<int, string>();

		public Dictionary<string, string> Entries { get; protected set; } = new Dictionary<string, string>();

		public bool HasInvalid { get { return Invalid.Count > 0; } }

		public bool HasEntries { get { return Entries.Count > 0; } }
	}

	public class QueryHashSystem
	{
		public QueryHashSystemResponse Execute(string fileUri)
		{
			var response = new QueryHashSystemResponse();

			// read and process all lines at once
			var count = 0;
			var lines = File.ReadAllLines(fileUri);
			var root = Path.GetDirectoryName(fileUri);
			foreach (var line in lines)
			{
				count++;
				if (String.IsNullOrWhiteSpace(line)) // empty lines
					response.BlankLines++;
				else if (line.StartsWith("#")) // comment lines
					response.Comments.Add(line);
				else // this should parse
				{
					var fix = line.Replace('\t', ' ').Trim();
					var index = fix.IndexOf(" ");
					if (index > 0)
					{
						var hash = fix.Substring(0, index);
						var file = fix.Substring(index + 1).Trim();
						if (!String.IsNullOrWhiteSpace(file)) // add valid entry
						{
							var uri = file;
							if (uri.StartsWith("*"))
								uri = Path.Combine(root, uri.Substring(1));
							else if (!uri.StartsWith(@"\\") && uri[1] != ':')
								uri = Path.Combine(root, uri);
							response.Entries.Add(uri, hash);
						}
						else // add invalid entry
							response.Invalid.Add(count, line);
					}
				}
			}

			return response;
		}
	}
}
