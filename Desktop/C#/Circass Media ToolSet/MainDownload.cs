using System;
using System.Management;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Ashlen.Persistence.Form;
using System.Web.Services.Protocols;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace CircassDownloader
{
    public partial class MainDownload : Form
    {
        #region deðiþkenler
        string documenturl = "";
        public NotifyIcon m_notifyicon = new NotifyIcon();
        public bool isFinished = false;
        ContextMenu m_menu = null;
        Stream stream = null;
        FileStream fstream = null;
        string destFileName;
        double filesize;
        private HttpWebRequest webRequest;
        private HttpWebResponse webResponse;
        private static int PercentProgress;
        private delegate void UpdateProgessCallback(double BytesRead, double TotalBytes);
        bool ishidden = false;
        Thread downloadth;

        #endregion


        public MainDownload(string DownloadUri)
        {
            InitializeComponent();
            lstFiles.Items.Add(DownloadUri);
            documenturl = DownloadUri;
        }

        public MainDownload()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addlist(string file)
        {
            lstFiles.Items.Add(file);
            documenturl = file;
        }

        private void Download()
        {
            try
            {
                if (SaveToLocation.Text.Equals(string.Empty))
                {
                    MessageBox.Show("Choose save Path of file");
                    this.Close();
                }

                if (textBox1.Text.Equals(string.Empty))
                {
                    MessageBox.Show("Enter Filename.");
                    this.Close();
                }
                destFileName = SaveToLocation.Text + "\\" + textBox1.Text;
                destFileName = destFileName.Replace("/", " ").Replace("%20", " ").Replace("\\\\", "\\");
                if (File.Exists(destFileName + ".flv") == false)
                {                    
                    webRequest = (HttpWebRequest)WebRequest.Create(lstFiles.Items[0].Text);
                    webRequest.Credentials = CredentialCache.DefaultCredentials;
                    webResponse = (HttpWebResponse)webRequest.GetResponse();
                    filesize = webResponse.ContentLength;
                    stream = webResponse.GetResponseStream();
                    fstream = new FileStream(destFileName.ToString() + ".flv", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                    int bytesSize = 0;
                    byte[] downBuffer = new byte[2048];
                    while ((bytesSize = stream.Read(downBuffer, 0, downBuffer.Length)) > 0)
                    {
                        fstream.Write(downBuffer, 0, bytesSize);
                        this.Invoke(new UpdateProgessCallback(this.UpdateProgress), new object[] { fstream.Length, filesize });
                    }
                    MessageBox.Show("Download Successful");
                    isFinished = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("A file with "+textBox1.Text+" is already exist. Please Enter  a different name");
                }
            }
            catch
            {
            }
        }

        private void UpdateProgress(double BytesRead, double TotalBytes)
        {
            try
            {
                PercentProgress = Convert.ToInt32((BytesRead * 100) / TotalBytes);

                progressBar1.Value = PercentProgress;

                string filerealsize = Convert.ToString((TotalBytes / 1024) / 1024);
                string boyut = Convert.ToString(BytesRead / 1024 / 1024);
                if (filerealsize.Length >= 4)
                {
                    if (boyut.Length >= 4)
                        label3.Text = "Until Now :: " + boyut.Substring(0, 4) + "/" + filerealsize.Substring(0, 4) + " MB" + " (" + PercentProgress + "%)";
                    else
                        label3.Text = "Until Now :: " + boyut + "/" + filerealsize.Substring(0, 4) + " MB" + " (" + PercentProgress + "%)";
                }
                else
                {
                    if (boyut.Length >= 4)
                        label3.Text = "Until Now :: " + boyut.Substring(0, 4) + "/" + filerealsize + " MB" + " (" + PercentProgress + "%)";
                    else
                        label3.Text = "Until Now :: " + boyut + "/" + filerealsize + " MB" + " (" + PercentProgress + "%)";
                }
            }
            catch
            {
                //MessageBox.Show(es.ToString());
            }
        }

        private void BrowseFolder_Click(object sender, EventArgs e)
        {
            if (FolderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                SaveToLocation.Text = FolderBrowserDialog.SelectedPath;
            }
        }
 
        private void BeginButton_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            SaveToLocation.Enabled = false;
            btnBegin.Enabled = false;
            BrowseFolder.Enabled = false;
            label3.Text = "Download is in progress";
            downloadth = new Thread(Download);
            Thread.Sleep(100);
            downloadth.Start();
            button1.Enabled = true;
        }

        private void MainDownload_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            try
            {
                string[] args = new string[2];
                args = Environment.GetCommandLineArgs();
                if (args.Length > 1)
                    addlist(args[1].ToString());
            }
            catch 
            {
            }
            label3.Text = "Waiting";
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_menu = new ContextMenu();
            m_menu.MenuItems.Add(0,
                new MenuItem("Exit", new System.EventHandler(Exit_Click)));
            m_menu.MenuItems.Add(0,
                new MenuItem("Show", new System.EventHandler(Show_Click)));
            m_notifyicon.Icon = new Icon(GetType(), "icon.ico");
            m_notifyicon.Visible = true;
            m_notifyicon.ContextMenu = m_menu;
            m_notifyicon.MouseMove += new MouseEventHandler(m_notifyicon_MouseMove);
            m_notifyicon.MouseClick += new MouseEventHandler(m_notifyicon_MouseClick);
            ToolTipIcon tipicon = new ToolTipIcon();
            m_notifyicon.ShowBalloonTip(1, "Download Statu", "You can see download status here\n" + label3.Text, tipicon);
            ishidden = true;
            this.Hide();
        }

        protected void m_notifyicon_MouseMove(object sender, MouseEventArgs e)
        {
                ToolTipIcon tipicon = new ToolTipIcon();
                m_notifyicon.ShowBalloonTip(1, "Download Statu", "You can see download status here\n" + label3.Text, tipicon);
                m_notifyicon.Text = label3.Text;
        }

        protected void m_notifyicon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Show();
                m_notifyicon.Dispose();
                ishidden = false;
            }
        }

        protected void Show_Click(object sender, System.EventArgs e)
        {
            this.Show();
            m_notifyicon.Dispose();
            ishidden = false;
        }

        protected void Exit_Click(object sender, System.EventArgs e)
        {
            MessageBoxIcon iconmes = new MessageBoxIcon();
            if (DialogResult.OK == MessageBox.Show("Are you sure ?", "Download will be aborted and download file will be deleted", MessageBoxButtons.OKCancel, iconmes))
            {
                this.Close();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            downloadth.Abort();
            stream.Dispose();
            fstream.Dispose();
            File.Delete(destFileName + ".flv");
            textBox1.Enabled = true; ;
            SaveToLocation.Enabled = true;
            btnBegin.Enabled = true;
            BrowseFolder.Enabled = true;
            button1.Enabled = false;
            progressBar1.Value = 0;
            isFinished = false;
            label3.Text = "Waiting";
        }

        private void MainDownload_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isFinished)
            {
                if (stream != null && fstream != null)
                {
                    stream.Dispose();
                    fstream.Dispose();
                    File.Delete(destFileName + ".flv");
                }
            }
        }

        private void menu_Click(object sender, EventArgs e)
        {
            infoForm info = new infoForm();
            info.Show();
        }
    }
}