using Expedition.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Expedition.Win.UtiliTree
{
	public partial class UtiliTreeForm : Form
	{
		public UtiliTreeForm()
		{
			InitializeComponent();
		}

		private void UtiliTreeForm_Load(object sender, EventArgs e)
		{
			// init previews
			icon16.Width = 16;
			icon16.Height = 16;
			icon32.Width = 32;
			icon32.Height = 32;
			icon64.Width = 64;
			icon64.Height = 64;
			icon64.SizeMode = PictureBoxSizeMode.StretchImage;
			icon128.Width = 128;
			icon128.Height = 128;
			icon128.SizeMode = PictureBoxSizeMode.StretchImage;


			// start default render
			treeFolders.BeginUpdate();
			listIcons.BeginUpdate();

			var root = new TreeNode();
			root.Name = "ICONS";
			root.Text = "ICONS";
			root.ImageKey = "ICONS";
			root.Expand();
			treeFolders.Nodes.Add(root);

			var dlls = new string[]
			{
				@"%SystemRoot%\system32\shell32.dll",
				@"%SystemRoot%\system32\imageres.dll",
				@"%SystemRoot%\system32\DDORes.dll",
				@"%SystemRoot%\system32\setupapi.dll",
				@"%SystemRoot%\system32\networkexplorer.dll",
				@"%SystemRoot%\system32\wmploc.dll",
				@"%SystemRoot%\system32\ieframe.dll",
				//@"%SystemRoot%\system32\gameux.dll",
			};

			foreach (var uri in dlls)
			{
				var icons = IconExtractor.ExtractIconsFromDll(uri);

				var source = icons.First().Source;
				var group = new ListViewGroup(source);
				listIcons.Groups.Add(group);
				
				root.Nodes.Add(source, source);

				foreach (var icon in icons)
				{
					var key = icon.Key;
					imageList16.Images.Add(key, icon.Icon16);
					imageList32.Images.Add(key, icon.Icon32);

					var iconItem = new ListViewItem();
					iconItem.Group = group;
					iconItem.Name = key;
					iconItem.Text = key;
					iconItem.ImageKey = key;
					listIcons.Items.Add(iconItem);
				}
			}


			treeFolders.EndUpdate();
			listIcons.EndUpdate();

			UpdateToolbarCommands();


			txtPath.KeyPress += TxtPath_KeyPress;
			treeFolders.AfterSelect += TreeFolders_AfterSelect;

			listIcons.CheckBoxes = true;
			listIcons.View = View.LargeIcon;

			listIcons.ShowGroups = false;
			listIcons.ItemChecked += ListIcons_ItemChecked;
			listIcons.SelectedIndexChanged += ListIcons_SelectedIndexChanged;

			listSelected.DoubleClick += ListSelected_DoubleClick;
		}

		private void ListSelected_DoubleClick(object sender, EventArgs e)
		{
			if (listSelected.SelectedItems.Count == 1)
			{
				var item = listSelected.SelectedItems[0];
				var key = item.ImageKey;
				SetIcon(key);
			}
		}

		private void ListIcons_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listIcons.SelectedItems.Count > 0)
			{
				var item = listIcons.SelectedItems[0];
				var key = item.ImageKey;

				icon16.Image = imageList16.Images[key];
				icon32.Image = icon64.Image = icon128.Image = imageList32.Images[key];
			}
		}

		private void ListIcons_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			listIcons.BeginUpdate();
			treeFolders.BeginUpdate();
			var key = e.Item.Name;
			if (e.Item.Checked)
			{
				listSelected.Items.Add(key, key, key);

				var node = new TreeNode();
				node.Text = key;
				node.ImageKey = node.SelectedImageKey = key;

				var parentKey = key.Substring(4);
				var parent = treeFolders.Nodes[0].Nodes[parentKey];
				parent.Parent.Expand();
				parent.Nodes.Add(node);
				parent.Expand();
			}
			else
			{
				var item = listSelected.Items[key];
				if (item != null) listSelected.Items[key].Remove();

				var parentKey = key.Substring(4);
				var parent = treeFolders.Nodes[0].Nodes[parentKey];
				if (parent != null)
				{
					foreach(TreeNode node in parent.Nodes)
					{
						if (node != null && node.Text == key) node.Remove();
						//if (node != null) node.Remove();
					}
				}
			}
			listIcons.EndUpdate();
			treeFolders.EndUpdate();
		}

		private void TxtPath_KeyPress(object sender, KeyPressEventArgs e)
		{
			if ((int)e.KeyChar == 13)
			{
				var uri = txtPath.Text;
				if (!String.IsNullOrWhiteSpace(uri))
				{
					if (Directory.Exists(uri))
						OpenDirectory(uri);
					else if (File.Exists(uri))
						OpenFile(uri);
					else
						MessageBox.Show($"{uri} does not exist or is not accessible");
				}
			}
		}

		private void TreeFolders_AfterSelect(object sender, TreeViewEventArgs e)
		{
			//throw new NotImplementedException();
			UpdateToolbarCommands();
		}

		private void UpdateToolbarCommands()
		{
			cmdCreate.Enabled = true;
			cmdVerify.Enabled = true;
			cmdClose.Enabled = true;
			cmdSave.Enabled = true;
		}

		private void OpenFile(string uri)
		{
			listIcons.BeginUpdate();
			listIcons.Items.Clear();
			

			listIcons.EndUpdate();
		}

		private void OpenDirectory(string uri)
		{
			var root = new TreeNode();
			root.Text = uri;
			root.Tag = uri;
			root.ImageKey = "Root";

			var r = new DirectoryInfo(uri);
			foreach (var dir in r.GetDirectories())
			{
				var d = new TreeNode();
				d.Text = dir.Name;
				d.Tag = dir.FullName;
				d.ImageKey = d.SelectedImageKey = "Folder";
				root.Nodes.Add(d);
			}

			treeFolders.BeginUpdate();
			treeFolders.Nodes.Add(root);
			treeFolders.EndUpdate();
		}

		private void cmdLoad_Click(object sender, EventArgs e)
		{
			var pick = new OpenFileDialog();
			if (pick.ShowDialog() == DialogResult.OK)
			{
				OpenFile(pick.FileName);
			}
		}


		private void cmdOpen_Click(object sender, EventArgs e)
		{
			var pick = new FolderBrowserDialog();
			if (pick.ShowDialog() == DialogResult.OK)
			{
				OpenDirectory(pick.SelectedPath);
			}
		}

		private void cmdReset_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to close all open paths?", "Reset", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				treeFolders.BeginUpdate();
				treeFolders.Nodes.Clear();
				treeFolders.EndUpdate();

				listIcons.BeginUpdate();
				listIcons.Items.Clear();
				listIcons.EndUpdate();
			}
		}

		private void cmdClose_Click(object sender, EventArgs e)
		{

		}

		private void SetIcon(string key)
		{
			var dot = key.IndexOf(".");
			var id = Convert.ToInt32(key.Substring(0, dot));
			var name = key.Substring(dot + 1);

			var uri = @"C:\Temp\UtiliTree\desktop.ini";
			if (File.Exists(uri)) File.Delete(uri);

			var ini = new IniProcessor(uri);

			var res = @"C:\Windows\System32\" + name + ".dll," + id.ToString();
			ini.Write(".ShellClassInfo", "IconResource", res);

			File.SetAttributes(uri, FileAttributes.System | FileAttributes.Hidden);
		}

		private void cmdSave_Click(object sender, EventArgs e)
		{
			var uri = @"C:\Temp\UtiliTree.txt";
			var ini = new IniProcessor(uri);

			ini.Write("UtiliTree", "A.Dll", "1,2,3,4");
			ini.Write("UtiliTree", "B.Dll", "1,6,255");
		}

		private void cmdCreate_Click(object sender, EventArgs e)
		{
			//[.ShellClassInfo]
			//IconResource = C:\Windows\System32\shell32.dll,21

			var uri = @"C:\Temp\UtiliTree\desktop.ini";

			if (File.Exists(uri)) File.Delete(uri);

			var ini = new IniProcessor(uri);
			ini.Write(".ShellClassInfo", "IconResource", @"C:\Windows\System32\shell32.dll,76");

			/*
			using (TextWriter file = File.CreateText(uri))
			{
				file.WriteLine("[.ShellClassInfo]");
				file.WriteLine(@"C:\Windows\System32\shell32.dll,26");
			}
			*/
			File.SetAttributes(uri, FileAttributes.System | FileAttributes.Hidden);
		}
	}
}
