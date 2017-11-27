using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Core
{
	public class ParsePath
	{
		private const char _slashChar = '\\';
		private const string _slashString = @"\";
		private const string _slashDouble = @"\\";

		private static string FixUri(string uri, bool addTrailingSlash = false)
		{
			return uri.TrimEnd(_slashChar).Replace(_slashDouble, _slashString) + (addTrailingSlash ? _slashString : "");
		}

		public static int GetDepth(string uri)
		{
			if (String.IsNullOrWhiteSpace(uri)) return -1;
			return GetPaths(FixUri(uri)).Length - 1;
		}

		public static string[] GetPaths(string uri)
		{
			if (String.IsNullOrWhiteSpace(uri)) return new string[0];
			return FixUri(uri).Split(_slashChar);
		}

		public static bool HasSameRoot(string uri1, string uri2)
		{
			if (String.IsNullOrWhiteSpace(uri1) || String.IsNullOrWhiteSpace(uri2)) return false;
			if (uri1.Length < 2 || uri2.Length < 2) return false;
			return (String.Compare(uri1.Substring(0, 2), uri2.Substring(0, 2)) == 0);
		}

		public static string GetCommonPath(string uri1, string uri2)
		{
			if (!HasSameRoot(uri1, uri2)) return "";
			var path1 = FixUri(uri1, true);
			var path2 = FixUri(uri2, true);
			if (path1 == path2)
				return FixUri(path1);
			else if (path1.StartsWith(path2, StringComparison.InvariantCultureIgnoreCase))
				return FixUri(path2);
			else if (path2.StartsWith(path1, StringComparison.InvariantCultureIgnoreCase))
				return FixUri(path1);
			else
			{
				path1 = GetParentPath(uri1);
				path2 = GetParentPath(uri2);
				return GetCommonPath(path1, path2);
			}
		}

		private static string GetParentPath(string uri)
		{
			if (String.IsNullOrWhiteSpace(uri) || uri.Length < 2) return "";
			var path = FixUri(uri);
			if (path.Length == 2) return path;
			int index = path.LastIndexOf(_slashChar);
			return FixUri(path.Substring(0, index));
		}

		private static string GetRootPath(string uri)
		{
			if (String.IsNullOrWhiteSpace(uri) || uri.Length < 2) return "";
			return uri.Substring(0, 2);
		}
	}
}
