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

namespace Expedition.Win
{
	public partial class FormMain : Form
	{
		public FormMain()
		{
			InitializeComponent();
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			txtPath.KeyPress += TxtPath_KeyPress;
			treeFolders.AfterSelect += TreeFolders_AfterSelect;

			var root = new TreeNode();
			root.Text = "Root";
			root.ImageKey = "Root";

			var d1 = new TreeNode();
			d1.Text = "Folder 1";
			d1.ImageKey = d1.SelectedImageKey = "Folder";
			root.Nodes.Add(d1);

			treeFolders.Nodes.Add(root);


			var f1 = new ListViewItem();
			f1.Text = "File 1";
			f1.ImageKey = "File";
			listFiles.Items.Add(f1);

			var f2 = new ListViewItem();
			f2.Text = "File 2";
			f2.ImageKey = "FileError";
			listFiles.Items.Add(f2);


			UpdateToolbarCommands();
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
			var node = treeFolders.SelectedNode;
			if (node == null || node.Parent != null)
			{
				cmdCreate.Enabled = false;
				cmdVerify.Enabled = false;
				cmdClose.Enabled = false;
				cmdSave.Enabled = false;
			}
			else
			{
				cmdCreate.Enabled = true;
				cmdVerify.Enabled = true;
				cmdClose.Enabled = true;
				cmdSave.Enabled = true;
			}
		}

		private void OpenFile(string uri)
		{
			var service = new VerifyChecksums();
			var result = service.Execute(new VerifyChecksumsRequest()
			{
				FileUri = uri,
				Preview = true,
			});

			listFiles.BeginUpdate();
			listFiles.Items.Clear();
			foreach (var file in result.Files)
			{
				var item = new ListViewItem();
				item.Text = file.Name;
				item.Tag = file.FullName;
				item.ImageKey = "File";
				item.SubItems.Add(file.Length.ToString());
				item.SubItems.Add("");
				listFiles.Items.Add(item);
			}
			listFiles.EndUpdate();
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

				listFiles.BeginUpdate();
				listFiles.Items.Clear();
				listFiles.EndUpdate();
			}
		}
	}
}
