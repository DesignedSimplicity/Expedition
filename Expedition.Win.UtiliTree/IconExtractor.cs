using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Win.UtiliTree
{
	public class ExtractedIcon
	{
		public string Key => $"{Index.ToString("000")}.{Source}";
		public bool IsValid => Icon16 != null && Icon32 != null;


		public string Source { get; set; }
		public int Index{ get; set; }
		public Icon Icon16 { get; set; }
		public Icon Icon32 { get; set; }
	}

	public class IconExtractor
	{
		[DllImport("Shell32.dll", EntryPoint = "ExtractIconExW", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		private static extern int ExtractIconEx(string sFile, int iIndex, out IntPtr piLargeVersion, out IntPtr piSmallVersion, int amountIcons);
		public static Icon ExtractIcon(string file, int number, bool largeIcon = false)
		{
			IntPtr large;
			IntPtr small;
			ExtractIconEx(file, number, out large, out small, 1);
			try
			{
				return Icon.FromHandle(largeIcon ? large : small);
			}
			catch
			{
				return null;
			}
		}


		public static List<ExtractedIcon> ExtractIconsFromDll(string uri)
		{
			var list = new List<ExtractedIcon>();
			var source = uri.ToLowerInvariant().Replace(@"%systemroot%\system32\", "").Replace(".dll", "");
			var errors = 0;
			for (var i = 0; i < 100; i++)
			{
				try
				{
					var icon = new ExtractedIcon();
					icon.Index = i;
					icon.Source = source;
					icon.Icon16 = IconExtractor.ExtractIcon(uri, i, false);
					icon.Icon32 = IconExtractor.ExtractIcon(uri, i, true);

					if (icon.IsValid) list.Add(icon);
				}
				catch{
					if (errors++ >= 3)
					{
						i = Int32.MaxValue;
					}
				}
			}
			return list;
		}
	}
}
