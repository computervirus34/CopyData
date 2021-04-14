using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyData
{
    public partial class Form1 : Form
    {
        //private readonly object Interaction;
        Font boldFont;
        Font normalFont;
        List<TreeNode> selectedNodes;
        //TreeNode previousNode;
        public Form1()
        {
            selectedNodes = new List<TreeNode>();
            boldFont = new Font("Arial", 10, FontStyle.Bold);
            normalFont = new Font("Arial", 10, FontStyle.Regular);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = Environment.UserName;
            textBox2.Text = Environment.MachineName;
            Button2.Enabled = false;
            Button5.Enabled = false;
            button7.Enabled = false;
            if (treeView1.HasChildren)
            {
                Button3.Enabled = true;
                Button4.Enabled = true;
            }else
            {
                Button3.Enabled = false;
                Button4.Enabled = false;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ListView1.Items.Clear();
            int i = 0;
            const double BytesInGB = 1073741824;
            //DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                string itemText = drive.Name;
                string Type = "Invalid";
                long freeSpace = 0;
                string ltr = drive.Name;
                if (drive.IsReady)
                {
                    freeSpace = drive.TotalFreeSpace;
                }
                if (drive.IsReady && drive.VolumeLabel != "")
                {
                    itemText = drive.VolumeLabel;
                }
                else
                    switch (drive.DriveType)
                    {
                        case System.IO.DriveType.Fixed:
                            {
                                itemText = "Local Disk";
                                break;
                            }

                        case System.IO.DriveType.CDRom:
                            {
                                itemText = "CD-ROM";
                                break;
                            }

                        case System.IO.DriveType.Network:
                            {
                                itemText = "Network Drive";
                                break;
                            }

                        case System.IO.DriveType.Removable:
                            {
                                itemText = "Removable Disk";
                                break;
                            }

                        case System.IO.DriveType.Unknown:
                            {
                                itemText = "Unknown";
                                break;
                            }
                    }
                switch (drive.DriveType)
                {
                    case System.IO.DriveType.Fixed:
                        {
                            Type = "Local Disk";
                            break;
                        }

                    case System.IO.DriveType.CDRom:
                        {
                            Type = "CD-ROM";
                            break;
                        }

                    case System.IO.DriveType.Network:
                        {
                            Type = "Network Drive";
                            break;
                        }

                    case System.IO.DriveType.Removable:
                        {
                            Type = "Removable Disk";
                            break;
                        }

                    case System.IO.DriveType.Unknown:
                        {
                            Type = "Unknown";
                            break;
                        }
                }

                ListView1.Items.Add(itemText);
                ListView1.Items[i].SubItems.Add(ltr);
                ListView1.Items[i].SubItems.Add(Type);
                ListView1.Items[i].SubItems.Add(Math.Round(freeSpace / BytesInGB, 2) + " GB");
                if (i % 2 == 0) ListView1.Items[i].BackColor = Color.LightBlue;
                i += 1;
            }
        }
        private void ListView1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string SelItem = ListView1.SelectedItems[0].SubItems[1].Text;
                foreach (System.IO.DriveInfo drive in System.IO.DriveInfo.GetDrives())
                {
                    try
                    {
                        Process.Start(SelItem);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "ERROR");
                    }
                }
            }
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void ListView1_DoubleClick(object sender, System.EventArgs e)
        {
            string SelItem = ListView1.SelectedItems[0].SubItems[1].Text;
            foreach (System.IO.DriveInfo drive in System.IO.DriveInfo.GetDrives())
            {
                try
                {
                    Process.Start(SelItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,"ERROR");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            
            //fbd.RootFolder = Environment.SpecialFolder.UserProfile;
            fbd.Description = "---Select Source Folder---";
            fbd.ShowNewFolderButton = false;
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = fbd.SelectedPath;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Button1.Enabled = false;
            Button2.Enabled = false;
            Button3.Enabled = false;
            Button4.Enabled = false;
            Button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            Button2.Enabled = false;
            foreach (TreeNode node in treeView1.SelectedNodes)
            {
                try
                {
                    string SourcePath = textBox3.Text + "\\" + node.Text;
                    string DestinationPath = textBox4.Text.ToString()+"\\" + Environment.UserName + "\\" + node.Text;
                    Directory.CreateDirectory(DestinationPath);
                    foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                                        SearchOption.AllDirectories))
                        if(Directory.Exists(dirPath))
                            Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

                    //Copy all the files & Replaces any files with the same name
                    foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                        SearchOption.AllDirectories))
                        File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }
            }
            MessageBox.Show("Copy Completed Successfully...", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Button1.Enabled = true;
            Button2.Enabled = true;
            Button3.Enabled = true;
            Button4.Enabled = true;
            Button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            Button2.Enabled = true;
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            String directory = textBox3.Text;
            BuildTree(directory, null);
            button7.Enabled = true;
        }
        private void BuildTree(String directory, TreeNode node)
        {
            try
            {
                String[] subdirectories = Directory.GetDirectories(directory);
                foreach (String subdirectory in subdirectories)
                {
                    String name = Path.GetFileName(subdirectory);
                    TreeNode subnode = (node == null) ? treeView1.Nodes.Add(name) : node.Nodes.Add(name);
                    subnode.NodeFont = boldFont;
                    subnode.Text = subnode.Text;
                }
                Button3.Enabled = true;
                Button4.Enabled = true;
            }
            catch (Exception)
            {
            }
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            treeView1.SelectedNodes = null;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            List<TreeNode> list = new List<TreeNode>();
            foreach (TreeNode node in treeView1.Nodes)
            {
                list.Add(node);
            }
            treeView1.SelectedNodes = list;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            //fbd.RootFolder = Environment.SpecialFolder.UserProfile;
            fbd.Description = "---Select Destination Folder---";
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = fbd.SelectedPath;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            Button2.Enabled = true;
            Button5.Enabled = true;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Button1.Enabled = false;
            Button2.Enabled = false;
            Button3.Enabled = false;
            Button4.Enabled = false;
            Button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            Button2.Enabled = false;
            foreach (TreeNode node in treeView1.SelectedNodes)
            {
                try
                {
                    string SourcePath = textBox3.Text + "\\" + node.Text;
                    string DestinationPath = textBox4.Text.ToString()+ "\\"+node.Text;
                    Directory.CreateDirectory(DestinationPath);
                    foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                                        SearchOption.AllDirectories))
                        Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

                    //Copy all the files & Replaces any files with the same name
                    foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                        SearchOption.AllDirectories))
                        File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }
            }
            MessageBox.Show("Restore Completed Successfully...", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Button1.Enabled = true;
            Button2.Enabled = true;
            Button3.Enabled = true;
            Button4.Enabled = true;
            Button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            Button2.Enabled = true;
        }
    }
}
