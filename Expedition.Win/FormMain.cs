using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
			listFIles.Items.Add(f1);

			var f2 = new ListViewItem();
			f2.Text = "File 2";
			f2.ImageKey = "FileError";
			listFIles.Items.Add(f2);
		}
	}
}
