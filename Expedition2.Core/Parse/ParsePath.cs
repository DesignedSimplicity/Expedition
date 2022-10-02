using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition2.Core
{
	public class ParsePath
	{
		private const char _slashChar = '\\';
		private const string _slashString = @"\";
		private const string _slashDouble = @"\\";

		/// <summary>
		/// Checks if the uri matches a network path format (starts with \\)
		/// </summary>
		public static bool IsNetworkPath(string uri)
		{
			if (String.IsNullOrWhiteSpace(uri) || uri.Length < 3) return false;
			return uri.StartsWith(_slashDouble);
		}

		/// <summary>
		/// Returns the machine name for the network path
		/// </summary>
		public static string GetNetworkRoot(string uri)
		{
			if (!IsNetworkPath(uri)) return String.Empty;
			return GetPaths(uri)[0];
		}

		/// <summary>
		/// Checks if the uri matches a local drive path format (starts with X:)
		/// </summary>
		public static bool IsLocalDrive(string uri)
		{
			if (String.IsNullOrWhiteSpace(uri)) return false;
			if (uri.Length < 2) return false;
			if (uri[1] != ':') return false;
			return Char.IsLetter(uri[0]);
		}

		/// <summary>
		/// Returns the drive letter for the local drive
		/// </summary>
		public static string GetLocalDrive(string uri)
		{
			if (!IsLocalDrive(uri)) return String.Empty;
			return uri.Substring(0, 1).ToUpperInvariant();
		}


		/// <summary>
		/// Fixes instances of \\ which occure past the start of the uri and optionally adds or removes the trailing \
		/// </summary>
		public static string FixUri(string uri, bool addTrailingSlash = false)
		{
			// remember if we are a network share
			var isNetworkPath = IsNetworkPath(uri);

			// ensure uri does not end with \
			var uriFixed = uri.TrimEnd(_slashChar).Replace(_slashDouble, _slashString);

			// build final completed uri
			return (isNetworkPath ? _slashString : String.Empty) + uriFixed + (addTrailingSlash ? _slashString : String.Empty);
		}

		/// <summary>
		/// Determines the depth of the current uri from the root
		/// </summary>
		public static int GetDepth(string uri)
		{
			if (String.IsNullOrWhiteSpace(uri)) return -1;
			return GetPaths(FixUri(uri)).Length - 1;
		}

		/// <summary>
		/// Converts a path hierarchy into an array of components
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		public static string[] GetPaths(string uri)
		{
			if (String.IsNullOrWhiteSpace(uri)) return new string[0];
			return FixUri(uri).Split(_slashChar);
		}

		/// <summary>
		/// Determins if descendant is contained within ancestor
		/// </summary>
		public static bool IsAncestor(string uriAncestor, string uriDescendant)
		{
			// check for invalid data or not on same root
			if (!HasSameRoot(uriAncestor, uriDescendant)) return false;

			// prepare 
			var uriBase = FixUri(uriAncestor, true);
			var uriCheck = FixUri(uriDescendant, false);

			// 
			return uriCheck.StartsWith(uriBase, StringComparison.InvariantCultureIgnoreCase);
		}

		public static bool IsSamePath(string uri1, string uri2)
		{
			if (String.IsNullOrWhiteSpace(uri1) || String.IsNullOrWhiteSpace(uri2)) return false;
			return (String.Compare(FixUri(uri1), FixUri(uri2)) == 0);
		}

		public static bool HasSameRoot(string uri1, string uri2)
		{
			if (IsLocalDrive(uri1))
				return (GetLocalDrive(uri1) == GetLocalDrive(uri2));
			else if (IsNetworkPath(uri1))
				return (String.Compare(GetNetworkRoot(uri1), GetNetworkRoot(uri2)) == 0);
			else // unknown type
				return false;
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
