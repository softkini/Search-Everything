using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Search_Everything
{
    public partial class Main : Form
    {
        public string FileSearch = "";
        public static ListViewItem item;

        private DriveInfo[] Drives = DriveInfo.GetDrives();
        private string[] strDrive = new string[5];
        private int count = 0;
        public int curent = 0;

        public Main()
        {
            InitializeComponent();
            foreach (DriveInfo d in Drives)
            {
                if (d.IsReady)
                {
                    strDrive[count] = d.Name; count++;
                    cbDrive.Items.Add(d.VolumeLabel + "( " + d.Name + " )");
                }
            }
            cbDrive.Text = (string)cbDrive.Items[0];
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        public void GetAllFiles(DirectoryInfo di)
        {
            try
            {
                Application.DoEvents();
                foreach (FileInfo file in di.GetFiles(FileSearch))
                {
                    if ((file.Attributes & FileAttributes.Hidden) == 0)
                    {
                        item = lstFiles.Items.Add(file.Name);
                        item.SubItems.Add(file.FullName);
                    }
                }
                foreach (DirectoryInfo director in di.GetDirectories())
                {
                    if ((director.Attributes & FileAttributes.Hidden) == 0) GetAllFiles(director);
                }

            }
            catch 
            {
                //
            }
        }

        public void Execute(string cmd)
        {
            if (cmd != string.Empty)
            {
                Process proc = new Process();
                proc.EnableRaisingEvents = false;
                proc.StartInfo.FileName = cmd;
                proc.Start(); proc.Close();
                proc.Dispose();
            }
        }

        private void bSearch_Click(object sender, EventArgs e)
        {
            lstFiles.Items.Clear();
            this.Text = "Searching ...";
            FileSearch = txtSearch.Text + "*";
            GetAllFiles(new DirectoryInfo(strDrive[curent]));
            this.Text = string.Format("Found = {0} items", lstFiles.Items.Count);
        }

        private void cbDrive_SelectedIndexChanged(object sender, EventArgs e)
        {
            curent = cbDrive.SelectedIndex;
        }

        private void lstFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Execute(lstFiles.SelectedItems[0].SubItems[1].Text);
        }
    }
}
