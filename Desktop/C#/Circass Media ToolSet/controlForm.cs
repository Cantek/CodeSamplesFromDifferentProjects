using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;
using System.Resources;
using System.Threading;
using QuartzTypeLib;
using System.Diagnostics;
using System.IO;
using CircassMediaToolSet.Properties;
using divxplanetform;
using Saricaovaliyiz;
using CircassDownloader;
using CircassCropTakeVideoSound;
using System.Web.Services;
using System.Net;

namespace CircassMediaToolSet
{

    public partial class From1 : Form
    {
        #region constructor lar
        public From1()
        {
            InitializeComponent();
            UpdateToolStrip();
        }

        public From1(ListBox itemList, ListBox listoffilenames, bool playercontrolform, int indexoffile, int indexOfItem)
        {
            InitializeComponent();
            UpdateToolStrip();
            this.itemList.Items.Clear();
            this.itemList = itemList;
            this.listoffilenames.Items.Clear();
            this.listoffilenames = listoffilenames;
            fromPlayerControlForm = true;
            indexOfFromControlForm = indexOfItem;
            searchform = new search(itemList, listoffilenames, 0, filename);
        }

        public From1(ListBox itemlistfromsc, ListBox filenames, string TargetFile)
        {
            InitializeComponent();
            this.itemList.Items.Clear();
            itemList = itemlistfromsc;
            this.listoffilenames.Items.Clear();
            listoffilenames = filenames;
            this.Refresh();
            if (fileIndex == -1)
                fileIndex = 0;
            if (!running)
                OpenFileFuncTh(fileIndex);
            searchform = new search(itemList, listoffilenames, 0, filename);
            searchform.Refresh();
            fromweb = true;
            UpdateToolStrip();
        }

        public From1(ListBox itemlist, ListBox filenames, Label filename, int fileindex)
        {
            InitializeComponent();
            UpdateToolStrip();
            this.filename = filename;
            this.itemList.Items.Clear();
            this.itemList.Items.AddRange(itemlist.Items);
            this.listoffilenames.Items.Clear();
            this.listoffilenames.Items.AddRange(filenames.Items);
            itemList.SelectedIndex = fileindex;
            fileIndex = itemList.SelectedIndex;
            if (!running)
                OpenFileFuncTh(0);
            searchform = new search(itemList, listoffilenames, 0, filename);
            searchform.Refresh();
        }


        public From1(ListBox itemlist, ListBox filenames, int width, int height, int x, int y, Label filename)
        {
            InitializeComponent();
            UpdateToolStrip();
            this.itemList.Items.Clear();
            this.itemList.Items.AddRange(itemlist.Items);
            this.listoffilenames.Items.Clear();
            this.listoffilenames.Items.AddRange(filenames.Items);
            this.filename = filename;
            setHeight(height);
            setWidth(width);
            setX(x);
            setY(y);
            fileIndex = getfileindex();
            newPlayerIsCreated = false;
            searchform = new search(itemList, listoffilenames, fileIndex, filename);
            searchform.Refresh();
        }
        public From1(ListBox items, ListBox filenames, int fileindex, bool doubleClick, Label filename)
        {
            InitializeComponent();
            this.itemList = items;
            this.filename = filename;
            this.listoffilenames = filenames;
            itemList.SelectedIndex = fileindex;
            try
            {
                m_obj_BasicAudio.Volume = volumeValue;
                m_CurrentStatus = MediaStatus.Running;
                m_obj_MediaControl.Run();
            }
            catch { }
            OpenFileFuncTh(itemList.SelectedIndex);
            searchform = new search(itemList, listoffilenames, fileindex, filename);
            searchform.Refresh();
        }
        #endregion

        #region deðiþkenler
        public static bool fromPlayerControlForm = false;
        public static int indexOfFromControlForm = -1;
        string TargetUri = "";
        UserActivityHook acthook;
        public string[] fileNames;
        public bool isHidden = false;
        public int fileSituation = 0;
        public bool searchListOpen = false;
        public static int fileIndex = -1;
        public int searchindex;
        public static bool fromEmpty = false;
        public NotifyIcon m_notifyicon = new NotifyIcon();
        ServerConnect newServerCon = null;
        public search searchform = null;
        public static player playerform = null;
        public static int playerformWidth = 0;
        public static int playerformHeight = 0;
        public static int playerformLocationX = 0;
        public static int playerformLocationY = 0;

        public bool controlformactif = false, itemlistActive = false;
        public bool fromweb = false;
        public int filesayisi = 0;
        public int NumOfDirs = 0;
        public string[] filesfromdirectory = new string[10000];
        public bool transferinitialized = false;
        public double time;
        public static bool cursorhidden = false;
        public static bool running = true, videoplaying = false, cancelfullscreen = false;
        public static bool mute = false;

        public static int panelx;
        public static int panely;
        public static int panelWidth;
        public static int panelHeight;
        public static int currentx;
        public static int currenty;
        public bool randomplay = false;
        public static int volumeValue = 0;
        public static bool fullscreen = false, repeatList = false, repeatFile = false, playerformclosed = false;
        int numara = 0;
        public static OpenFileDialog openFileDialog = new OpenFileDialog();
        public static IMediaPosition m_obj_MediaPosition = null;
        public static IMediaControl m_obj_MediaControl = null;
        public static IVideoWindow m_obj_VideoWindow = null;
        public static FilgraphManager m_obj_FilterGraph = null;
        public static IBasicAudio m_obj_BasicAudio = null;
        public static IMediaEvent m_obj_MediaEvent = null;
        public static IMediaEventEx m_obj_MediaEventEx = null;
        public static int WM_APP = 0x8000;
        public static int WM_GRAPHNOTIFY = WM_APP + 1;
        public static int EC_COMPLETE = 0x01;
        public static int WS_CHILD = 0x40000000;
        public static int WS_CLIPCHILDREN = 0x200000;
        public static MediaStatus m_CurrentStatus = MediaStatus.None;
        public enum MediaStatus { None, Stopped, Paused, Running };
        public static ContextMenu m_menu;
        public static ContextMenu m_options_menu;
        bool itemlistempty = false;
        int itemlistlistfile = -1;
        public delegate void OpenFileFuncDel(int fileindex);
        public static bool newPlayerIsCreated = false;

        #endregion

        #region playerform location
        public void OpenFileFuncTh(int fileindex)
        {
            OpenFileFuncDel openFileFunFromDel = new OpenFileFuncDel(OpenFileFunc);
            Thread.Sleep(100);
            openFileFunFromDel.Invoke(fileindex);
        }

        public void setHeight(int height)
        {
            playerformHeight = height;
        }

        public void setWidth(int width)
        {
            playerformWidth = width;
        }

        public void setX(int x)
        {
            playerformLocationX = x;
        }

        public void setY(int y)
        {
            playerformLocationY = y;
        }

        public int getx()
        {
            return playerformLocationX;
        }
        public int gety()
        {
            return playerformLocationY;
        }
        public int getheight()
        {
            return playerformHeight;
        }
        public int getWidth()
        {
            return playerformWidth;
        }

        #endregion

        #region info
        public void label5_Click(object sender, EventArgs e)
        {
            infoForm info = new infoForm();
            info.SetBounds(this.Width, 0, 292, 265);
            info.Show();
        }
        #endregion

        #region dispose
        //Override edilmiþ bitirme iþi...
        protected override void Dispose(bool disposing)
        {
            ReLoad();
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        //Override edilmiþ bitirme iþi sonu...
        #endregion

        #region listeye ekleme olaylarý
        //Buton la gelen dosyalarýn listeye eklenemesi iþi....
        public void addList(OpenFileDialog openFileDialog)
        {
            itemlistlistfile = 0;
            if (itemList.Items.Count == 0)
            {
                itemlistempty = true;
            }
            else
            {
                itemlistlistfile = itemList.Items.Count;
                itemlistempty = false;
            }
            int i = 0, j = 0;
            clearB.Enabled = true;
            int numberOfItem = openFileDialog.FileNames.Length;
            for (i = (numberOfItem - 1); i >= 0; i--)
            {
                numara++;
                String fileName = openFileDialog.FileNames[((numberOfItem - 1) - i)].ToString();
                String newfileName = "";
                int length = fileName.Length;
                for (j = length - 1; j > 0; j--)
                {
                    if (fileName[j] != '\\')
                        newfileName += fileName[j];
                    else
                        break;
                }
                fileName = "";
                for (j = newfileName.Length - 1; j >= 0; j--)
                    fileName += newfileName[j];
                if (fileName.Length > 25)
                {
                    newfileName = "";
                    for (int shortname = 0; shortname < 22; shortname++)
                        newfileName += fileName[shortname];
                    newfileName += "...";
                    fileName = newfileName;
                }
                itemList.Items.Add((object)fileName);
                listoffilenames.Items.Add((object)openFileDialog.FileNames[((numberOfItem - 1) - i)].ToString());
                if (!running && itemlistempty)
                {
                    fileIndex = 0;
                    itemList.SelectedIndex = fileIndex;
                    OpenFileFuncTh(fileIndex);
                }
                else if (!running && !itemlistempty)
                {
                    fileIndex = itemlistlistfile;
                    itemList.SelectedIndex = fileIndex;
                    OpenFileFuncTh(fileIndex);
                }
            }
        }
        //Butonla gelen dosyalarýn listeye eklenemesi iþi sonu...

        //Drag dropla atýlan dosyalarýn listeye ekelenmesi iþi...
        public void addlistFrom(String[] files)
        {
            itemList.Items.Clear();
            listoffilenames.Items.Clear();
            addList(files);
        }
        public void addList(String[] files)
        {
            itemlistlistfile = 0;
            if (itemList.Items.Count == 0)
            {
                itemlistempty = true;
            }
            else
            {
                itemlistlistfile = itemList.Items.Count;
                itemlistempty = false;
            }

            int j = 0;
            clearB.Enabled = true;
            //clearsB.Enabled = true;
            foreach (string file in files)
            {
                numara++;
                String fileName = file.ToString();
                String newfileName = "";
                int length = fileName.Length;
                for (j = length - 1; j > 0; j--)
                {
                    if (fileName[j] != '\\')
                        newfileName += fileName[j];
                    else
                        break;
                }
                fileName = "";
                for (j = newfileName.Length - 1; j >= 0; j--)
                    fileName += newfileName[j];
                if (fileName.Length > 22)
                {
                    newfileName = "";
                    for (int shortname = 0; shortname < 22; shortname++)
                        newfileName += fileName[shortname];
                    newfileName += "...";
                    fileName = newfileName;
                }
                itemList.Items.Add((object)fileName);
                listoffilenames.Items.Add((object)file.ToString());
                if (!running && itemlistempty)
                {
                    fileIndex = 0;
                    itemList.SelectedIndex = fileIndex;
                    OpenFileFuncTh(fileIndex);
                }
                else if (!running && !itemlistempty)
                {
                    fileIndex = itemlistlistfile;
                    itemList.SelectedIndex = fileIndex;
                    OpenFileFuncTh(fileIndex);
                }
            }
        }
        //Drag dropla gelen dosyalarýn listeye eklenemesi iþi sonu...
        #endregion

        #region çalýnacak dosyanýn açýlmasý
        public void OpenFileFunc(int filedex)
        {
            fileIndex = filedex;
            listoffilenames.SelectedIndex = fileIndex;
            object sender = new object();
            EventArgs er = new EventArgs();
            listoffilenames_SelectedIndexChanged(sender, er);
            ReLoad();
            itemList.Refresh();
            string openedfile = this.listoffilenames.Items[fileIndex].ToString();
            m_obj_FilterGraph = new FilgraphManager();
            try
            {
                m_obj_FilterGraph.RenderFile(openedfile);
            }
            catch (Exception)
            {
                MessageBox.Show("Please choose a valid file.\n Check the file path.\n File path could be changed.");
                return;
            }
            try
            {
                m_obj_BasicAudio = m_obj_FilterGraph as IBasicAudio;
                trackBar1.Value = volumeValue;
                m_obj_BasicAudio.Volume = trackBar1.Value;
                if (mute)
                {
                    volumeValue = -10000;
                    trackBar1.Value = volumeValue;
                    m_obj_BasicAudio.Volume = trackBar1.Value;
                }
            }
            catch { }

            try
            {
                if (!newPlayerIsCreated)
                {
                    playerform = new player(itemList, listoffilenames, fileIndex, filename);
                    newPlayerIsCreated = true;
                    playerform.Location = new Point(playerformLocationX, playerformLocationY);
                }
                m_obj_VideoWindow = m_obj_FilterGraph as IVideoWindow;
                m_obj_VideoWindow.Owner = (int)playerform.Handle;
                m_obj_VideoWindow.WindowStyle = WS_CHILD;
                videoplaying = true;
                m_obj_VideoWindow.SetWindowPosition(playerform.ClientRectangle.Left,
                    playerform.ClientRectangle.Top,
                    playerform.ClientRectangle.Right,
                    playerform.ClientRectangle.Bottom);
            }
            catch (Exception)
            {
                m_obj_VideoWindow = null;
                videoplaying = false;
            }

            if (videoplaying)
            {
                trackBar2.Enabled = true;
                trackBar1.Enabled = true;
                if (fullscreen)
                {
                    if (playerformactivatedonce == 0)
                        playerform.Show();
                    fullscreen = false;
                    fullscreenfunc();
                }
                else
                {
                    if (playerformactivatedonce == 0)
                        playerform.Show();
                }
                playerform.Activate();
            }
            else
            {
                trackBar2.Enabled = true;
                trackBar1.Enabled = true;
                playerformHeight = playerform.Height;
                playerformWidth = playerform.Width;
                playerformLocationX = playerform.Location.X;
                playerformLocationY = playerform.Location.Y;
                playerform.Dispose();
                newPlayerIsCreated = false;
            }
            running = true;
            m_obj_MediaEvent = m_obj_FilterGraph as IMediaEvent;
            m_obj_MediaEventEx = m_obj_FilterGraph as IMediaEventEx;
            m_obj_MediaEventEx.SetNotifyWindow((int)this.Handle, WM_GRAPHNOTIFY, 0);
            m_obj_MediaPosition = m_obj_FilterGraph as IMediaPosition;
            m_obj_MediaControl = m_obj_FilterGraph as IMediaControl;
            trackBar2.Maximum = (int)m_obj_MediaPosition.Duration;
            trackBar2.Minimum = 0;
            trackBar2.Value = (int)m_obj_MediaPosition.CurrentPosition;
            m_obj_MediaControl.Run();
            m_CurrentStatus = MediaStatus.Running;
            try
            {
                if (searchform != null)
                    searchform.listBox1.SelectedIndex = itemList.SelectedIndex;
            }
            catch { }
            if (isHidden)
                openModifyWindow();
            string[] newfilenamearray = listoffilenames.Items[fileIndex].ToString().Split('\\');
            string newfilename = newfilenamearray[newfilenamearray.Length - 1].ToString();
            if(newfilename.Length>30)
                filename.Text = newfilename.Substring(0,29) + "  ";
            else
                filename.Text = newfilename+ "  ";
            this.Refresh();
            UpdateToolStrip();
        }

        //Çalýnacak dosyayý açma iþlemi sonu...
        #endregion

        #region reload
        //PLAYER ý yeniden yükleme iþlemi....
        public void ReLoad()
        {
            if (m_obj_MediaControl != null)
            {
                m_obj_MediaControl.Stop();
                m_CurrentStatus = MediaStatus.Stopped;
            }

            if (m_obj_MediaEventEx != null)
                m_obj_MediaEventEx.SetNotifyWindow(0, 0, 0);

            if (m_obj_VideoWindow != null)
            {
                m_obj_VideoWindow.Visible = 0;
                m_obj_VideoWindow.Owner = 0;
            }

            if (m_obj_MediaControl != null) m_obj_MediaControl = null;
            if (m_obj_MediaPosition != null) m_obj_MediaPosition = null;
            if (m_obj_MediaEventEx != null) m_obj_MediaEventEx = null;
            if (m_obj_MediaEvent != null) m_obj_MediaEvent = null;
            if (m_obj_VideoWindow != null) m_obj_VideoWindow = null;
            if (m_obj_BasicAudio != null) m_obj_BasicAudio = null;
            if (m_obj_FilterGraph != null) m_obj_FilterGraph = null;
            if (itemList.Items.Count == 0)
            {
                m_CurrentStatus = MediaStatus.None;
                trackBar2.Enabled = false;
                trackBar1.Enabled = false;
                if (videoplaying)
                    playerform.Dispose();
                videoplaying = false;
                m_CurrentStatus = MediaStatus.None;
            }
            UpdateToolStrip();
        }
        //Playeri yeniden yükleme iþlemi sonu....
        #endregion

        #region çalma iþlemi
        //Medya nýn çalmasý için gerekli kýsým....
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_GRAPHNOTIFY)
            {
                int lEventCode;
                int lParam1, lParam2;
                while (true)
                {
                    try
                    {
                        m_obj_MediaEventEx.GetEvent(out lEventCode,
                            out lParam1,
                            out lParam2,
                            0);
                        m_obj_MediaEventEx.FreeEventParams(lEventCode, lParam1, lParam2);

                        if (lEventCode == EC_COMPLETE)
                        {
                            m_obj_MediaControl.Stop();
                            m_CurrentStatus = MediaStatus.Stopped;
                            if (videoplaying)
                            {
                                playerform.Close();
                                newPlayerIsCreated = false;
                                videoplaying = false;
                                CompleteAndReOpen = true;
                            }
                            UpdateToolStrip();
                            if (randomplay)
                            {
                                Random rand = new Random();
                                int fileturn = rand.Next(0, itemList.Items.Count - 1);
                                OpenFileFuncTh(fileturn);
                            }
                            else
                            {
                                if (fileIndex < itemList.Items.Count - 1 && !repeatFile)
                                {
                                    fileIndex++;
                                    volumeValue = trackBar1.Value;
                                    OpenFileFuncTh(fileIndex);
                                }
                                else if (repeatFile)
                                {
                                    OpenFileFuncTh(fileIndex);
                                }
                                else if (fileIndex == itemList.Items.Count - 1 && repeatList)
                                {
                                    fileIndex = 0;
                                    OpenFileFuncTh(fileIndex);
                                }
                                else
                                {
                                    fileIndex = 0;
                                    trackBar1.Value = 0;
                                    trackBar1.Enabled = false;
                                    trackBar2.Enabled = false;
                                    m_CurrentStatus = MediaStatus.Stopped;
                                    UpdateToolStrip();
                                }
                            }
                        }
                    }
                    catch (Exception explain)
                    {
                        break;
                    }
                }
            }
            base.WndProc(ref m);
        }

        //Medyanýn çalmasý için gerekli kýsmýn sonu...
        #endregion

        #region toolstrip olaylarý
        bool CompleteAndReOpen = false;
        //Toolstrip baþlangýcý....
        public static int clicked = -1;
        public void toolStrip1_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {
            clicked = toolStrip1.Items.IndexOf(e.ClickedItem);
            if (fromPlayerControlForm)
            {
                clicked = indexOfFromControlForm;
                fromPlayerControlForm = false;
            }

            switch (clicked)
            {
            	//Toolstrip üzerindeki butonlara týklandýðýnda yapýlacak iþler buraya yazýlacak.
            }
            UpdateToolStrip();
        }

        public void label4_Click_1(object sender, EventArgs e)
        {
            if (!mute)
            {
                if (running)
                {
                    m_obj_BasicAudio.Volume = -10000;
                    mute = true;
                    label4.Text = "Open";
                }
            }
            else
            {
                m_obj_BasicAudio.Volume = trackBar1.Value;
                mute = false;
                label4.Text = "Mute";
            }
        }

        public void UpdateToolStrip()
        {
            switch (m_CurrentStatus)
            {
            	//Toolstripte yada çalýnan parçada olan statu deðiþikliklerine göre 
            	//toolstripin güncellenemesi için gerekli olan kodlar buraya yazýlacak.
            }
        }
        //Toolstrip sonu...
        #endregion

        #region timertick
        //Zamanla ilgili kýsýmlar...
        public int trackMax = 0;
        public void timer1_Tick(object sender, EventArgs e)
        {
            if (m_CurrentStatus == MediaStatus.Running)
            {
                UpdateToolStrip();
                trackBar2.Maximum = (int)m_obj_MediaPosition.Duration;
                trackBar2.Value = (int)m_obj_MediaPosition.CurrentPosition;
                fileSituation = trackBar2.Value;
                trackMax = trackBar2.Maximum;
                filename.Text =filename.Text.Substring(1) + filename.Text[0].ToString();
            }
        }
        #endregion

        #region trackbars scrool
        public void trackBar2_Scroll(object sender, EventArgs e)
        {
            trackBar2.Maximum = (int)m_obj_MediaPosition.Duration;
            m_obj_MediaPosition.CurrentPosition = trackBar2.Value;
            fileSituation = trackBar2.Value;
        }
        //Zamanla ilgili fonlarýn sonu....


        //Sesle ilgili fonsiyonlar...
        public void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                m_obj_BasicAudio.Volume = 0 + trackBar1.Value;
                volumeValue = m_obj_BasicAudio.Volume;
                label4.Text = "Mute";
                mute = false;
            }
            catch { }
        }
        #endregion

        #region form Klavye kýsayollarý ile ilgili fonsiyonlar...
        //Klavye kýsayollarý ile ilgili fonsiyonlar...
        public void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!itemlistActive)
            {
                if (e.KeyData == Keys.Right && running)
                {
                    durationForward();
                }

                if (e.KeyData == Keys.Left && running)
                {
                    durationBackward();
                }

                if (e.KeyData == Keys.Up && running)
                {
                    if (running)
                    {
                        volumeUp();
                    }
                }
                if (e.KeyData == Keys.Down && running)
                {
                    if (running)
                    {
                        volumeDown();
                    }
                }

                if (e.KeyData == Keys.Escape)
                {
                    if (MessageBox.Show("Do ou really want to close CMP ?", "Program is shutting down", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        Close();
                }
                if (e.KeyData == Keys.M && running)
                {
                    if (mute)
                    {
                        m_obj_BasicAudio.Volume = trackBar1.Value;
                        volumeValue = m_obj_BasicAudio.Volume;
                        label4.Text = "Mute";
                        mute = false;
                    }
                    else
                    {
                        m_obj_BasicAudio.Volume = -10000;
                        volumeValue = m_obj_BasicAudio.Volume;
                        label4.Text = "Open";
                        mute = true;
                    }
                }

                if (e.KeyData == Keys.P || e.KeyData == Keys.Space)
                {
                    if (m_CurrentStatus == MediaStatus.Paused)
                    {
                        m_CurrentStatus = MediaStatus.Running;
                        m_obj_MediaControl.Run();
                        running = true;
                        UpdateToolStrip();
                    }
                    else if (m_CurrentStatus == MediaStatus.Running)
                    {
                        m_CurrentStatus = MediaStatus.Paused;
                        m_obj_MediaControl.Pause();
                        running = false;
                        UpdateToolStrip();
                    }
                }

                if (e.KeyData == Keys.F)
                {
                    fullscreenfunc();
                }
            }

        }
        #endregion

        #region formload
        private void From1_Load(object sender, EventArgs e)
        {
            m_options_menu = new ContextMenu();
            m_options_menu.MenuItems.Add(0,
                new MenuItem("Convert Video to wmv", new System.EventHandler(ConvertVideo)));
            m_options_menu.MenuItems.Add(1,
                new MenuItem("Download Youtube Videos", new System.EventHandler(DownloadVideo)));
            m_options_menu.MenuItems.Add(2,
                new MenuItem("Turkish-English-Adýðabze Dictionary", new System.EventHandler(OpenCircasDic)));
            m_options_menu.MenuItems.Add(3,
                new MenuItem("Crop Videos - Take Sound a part of Video", new System.EventHandler(cropVidTakeSound)));
            m_options_menu.MenuItems.Add(4,
                new MenuItem("Audio Conversion", new System.EventHandler(AudioConvert)));
            m_options_menu.MenuItems.Add(5,
                new MenuItem("Subtitle Editor", new System.EventHandler(subTitleEditor)));
            ContextMenu = m_options_menu;

            string[] args = Environment.GetCommandLineArgs();
            int numOfLoadFiles = 0;
            if (args.Length > 1)
            {
                int count = 1;
                for (numOfLoadFiles = 1; numOfLoadFiles < args.Length; numOfLoadFiles++)
                {
                    string file = args[numOfLoadFiles].ToString();
                    string[] fileName = file.Split('\\');
                    string[] fileType = fileName[fileName.Length - 1].Split('.');
                    string fullPath = args[1].ToString();
                    string typeOffile = "";
                    for (count = 2; count < args.Length; count++)
                    {
                        fullPath += " " + args[count].ToString();
                    }
                    DirectoryInfo director = new DirectoryInfo(file);
                    if (fileType[fileType.Length - 1] == "cmpl")
                    {
                        typeOffile = "cmpl";
                    }
                    else if (fileType[fileType.Length - 1].ToLower() == "wma" || fileType[fileType.Length - 1].ToLower() == "snd" || fileType[fileType.Length - 1].ToLower() == "aif" || fileType[fileType.Length - 1].ToLower() == "aiff" || fileType[fileType.Length - 1].ToLower() == "au" || fileType[fileType.Length - 1].ToLower() == "mp3" || fileType[fileType.Length - 1] == "mp4" || fileType[fileType.Length - 1].ToLower() == "avi" || fileType[fileType.Length - 1].ToLower() == "mpg" || fileType[fileType.Length - 1].ToLower() == "mpeg" || fileType[fileType.Length - 1].ToLower() == "wmv" || fileType[fileType.Length - 1].ToLower() == "mov" || fileType[fileType.Length - 1].ToLower() == "flv" || fileType[fileType.Length - 1].ToLower() == "wav" || fileType[fileType.Length - 1].ToLower() == "wma" || fileType[fileType.Length - 1].ToLower() == "vob" || fileType[fileType.Length - 1].ToLower() == "dat" || fileType[fileType.Length - 1].ToLower() == "avý")
                    {
                        typeOffile = "media";
                    }
                    else if (director.Attributes == director.Attributes)
                    {
                        typeOffile = "director";
                    }

                    else
                        typeOffile = fileType[fileType.Length - 1].ToString();
                    switch (typeOffile)
                    {
                        case "cmpl":
                            int files = 0;
                            StreamReader sr = new StreamReader(file);
                            fileNames = new String[1000];
                            while (!sr.EndOfStream)
                            {
                                fileNames[files] = sr.ReadLine().ToString();
                                files++;
                            }
                            addList(fileNames, files);
                            break;
                        case "media":
                            string mediafile = "";
                            mediafile = args[numOfLoadFiles].ToString();
                            addlist(mediafile);
                            break;
                        case "director":
                            this.filesayisi = 0;
                            this.NumOfDirs = 0;
                            DirectoryInfo basedirectory = null;
                            try
                            {
                                basedirectory = new DirectoryInfo(file);
                                if (basedirectory.GetFiles().Length > 0)
                                    fileadderfromdirectory(basedirectory);
                                MakeDecision(basedirectory.ToString());
                            }
                            catch (Exception exeptin)
                            {
                                MessageBox.Show("This Media type is not supported");
                                break;
                            }
                            break;
                        default:
                            MessageBox.Show(typeOffile + " format is not supported.\n Be sure that this file is a valid media file.");
                            break;
                    }
                }
            }
        }
        #endregion

        #region form üstündeki butonlar

        //Minimize iþlemi...
        public void label4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            if (videoplaying)
            {
                playerform.TopMost = true;
                playerform.TopMost = false;
            }
        }
        //Minimize iþlemi sonu...



        private void button6_Click(object sender, EventArgs e)
        {
            if (running)
            {
                GoTo gotoform = new GoTo();
                gotoform.Show();
            }
        }

        public void goToTime(int second)
        {
            if (running)
                if (second < m_obj_MediaPosition.Duration && second > 0)
                    m_obj_MediaPosition.CurrentPosition = second;
        }

        private void itemList_Enter(object sender, EventArgs e)
        {
            itemlistActive = true;
        }

        private void itemList_Leave(object sender, EventArgs e)
        {
            itemlistActive = false;
        }


        //Listeden silme iþlemleri sonu...


        private void saveB_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveList = new SaveFileDialog();
            saveList.Filter = "Circass List|*.cmpl|All Files|*.*";
            saveList.DefaultExt = ".cmpl";
            if (saveList.ShowDialog() == DialogResult.OK)
            {

                string msg = "{0}";
                int files = 0;
                StreamWriter sw = new StreamWriter(saveList.FileName);
                for (files = 0; files < listoffilenames.Items.Count; files++)
                    sw.WriteLine(String.Format(msg, listoffilenames.Items[files].ToString()));
                sw.Flush();
            }
            saveList.Dispose();

        }

        private void loadB_Click(object sender, EventArgs e)
        {
            OpenFileDialog openList = new OpenFileDialog();
            openList.Filter = "Circass List|*.cmpl|All files|*.*";
            if (openList.ShowDialog() == DialogResult.OK)
            {
                int files = 0;
                StreamReader sr = new StreamReader(openList.FileName);
                fileNames = new String[1000];
                files = 0;
                while (!sr.EndOfStream)
                {
                    fileNames[files] = sr.ReadLine().ToString();
                    files++;
                }
                addList(fileNames, files);
            }
        }


        private void searchB_Click(object sender, EventArgs e)
        {
            searchform = new search(itemList, listoffilenames, fileIndex, filename);
            searchform.Show();
            searchListOpen = true;
        }
        string standart = Application.StartupPath;
        
        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(standart + "\\AltyaziBul.exe");
        }

        #endregion

        #region Control formunu taþýma iþlemi....
        //Control formunu taþýma iþlemi....
        public bool mouseDown = false;

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            panelHeight = this.Height;
            panelWidth = this.Width;
            currentx = this.Location.X;
            currenty = this.Location.Y;
            panelx = Cursor.Position.X;
            panely = Cursor.Position.Y;
            mouseDown = true;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                Location = new Point(currentx + Cursor.Position.X - panelx, currenty + Cursor.Position.Y - panely);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        // Control formunu panel aracýlýðýyla taþýma fonksiyonu sonu...
        #endregion

        #region form sütündeki key yada mouse algýlamalarý
        //Panel üstünden klavye tuþlarýný almak için gerekli...
        public void hookStart()
        {
            acthook = new UserActivityHook();
            acthook.KeyUp += new KeyEventHandler(MyKeyUp);
            acthook.KeyDown += new KeyEventHandler(MyKeyDown);
            acthook.KeyPress += new KeyPressEventHandler(MyKeyPress);
            acthook.OnMouseActivity += new MouseEventHandler(MyMouseMove);
        }

        public void MyMouseMove(object sender, MouseEventArgs e)
        {
            if (!itemlistActive)
            {
                if (e.Delta == -120)
                {
                    volumeWithMouse(false);
                }
                else if (e.Delta == 120)
                {
                    volumeWithMouse(true);
                }
            }
        }
        bool voiceupdown = false;
        public void MyKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Right || e.KeyData == Keys.Left || e.KeyData == Keys.Down || e.KeyData == Keys.Up && !itemlistActive)
            {
                voiceupdown = true;
                openVoice(e);
            }
            if (e.Alt == true && e.KeyCode == Keys.F4)
                this.Close();
        }

        public void openVoice(KeyEventArgs e)
        {
            if (voiceupdown)
            {
                try
                {
                    Thread.Sleep(25);
                    if (e.KeyData == Keys.Left && running)
                    {
                        durationForward();
                    }
                    if (e.KeyData == Keys.Right && running)
                    {
                        durationBackward();
                    }

                    if (e.KeyData == Keys.Up && running)
                    {
                        if (running && !itemlistActive)
                        {
                            volumeUp();
                        }
                    }
                    if (e.KeyData == Keys.Down && running)
                    {
                        if (running && !itemlistActive)
                        {
                            volumeDown();
                        }
                    }
                }
                catch { }
            }
        }

        public void MyKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Right || e.KeyData == Keys.Left || e.KeyData == Keys.Down || e.KeyData == Keys.Up && !itemlistActive)
            {
                voiceupdown = false;
            }
            else
                Form1_KeyUp(sender, e);
        }

        public void MyKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == Keys.Left.ToString())
            {
                int newvalue = trackBar2.Value - 1;
                if (running && newvalue > 0)
                {
                    trackBar2.Value = newvalue;
                    m_obj_MediaPosition.CurrentPosition = trackBar2.Value;
                }
                if (running && newvalue <= 0)
                {
                    trackBar2.Value = 0;
                    m_obj_MediaPosition.CurrentPosition = trackBar2.Value;
                }
            }

            From1_KeyPress(sender, e);
        }

        int playerformactivatedonce = 0;
        private void From1_Activated(object sender, EventArgs e)
        {
            hookStart();
            if (videoplaying)
            {
                if (playerformactivatedonce == 0)
                {
                    playerform.SendToBack();
                    playerformactivatedonce = 1;
                }

                if (playerform.pcfactive)
                {
                    PlayerControlForm pcf = new PlayerControlForm();
                    pcf.Close();
                }
            }
            trackBar1.Value = volumeValue;
            if (mute)
                label4.Text = "Open";
            else
                label4.Text = "Mute";
            TopMost = true;
            TopMost = false;
        }

        private void From1_Deactivate(object sender, EventArgs e)
        {
            controlformactif = false;
            TopMost = false;
            if (playerform != null)
                playerformactivatedonce = 0;
            acthook.Stop();
        }


        public void From1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (running)
            {
                if (e.KeyChar.ToString() == Keys.E.ToString().ToLower())
                {
                    volumeUp();
                }
                if (e.KeyChar.ToString() == Keys.W.ToString().ToLower())
                {
                    volumeDown();
                }

                if (e.KeyChar.ToString() == "+")
                {
                    volumeUp();
                }
                if (e.KeyChar.ToString() == "-")
                {
                    volumeDown();
                }

            }


        }

        public void volumeDown()
        {
            try
            {
                trackBar1.Value = m_obj_BasicAudio.Volume;
                if (m_obj_BasicAudio.Volume <= 0 && m_obj_BasicAudio.Volume > -9900)
                {
                    trackBar1.Value = trackBar1.Value - 100;
                    volumeValue = trackBar1.Value;
                }
                if (trackBar1.Value < 0 && trackBar1.Value > -10000)
                {
                    m_obj_BasicAudio.Volume = trackBar1.Value;
                    volumeValue = trackBar1.Value;
                }
            }
            catch
            { }
            //this.Refresh();

        }

        public void volumeUp()
        {
            try
            {
                trackBar1.Value = m_obj_BasicAudio.Volume;
                if (m_obj_BasicAudio.Volume >= -10000 && m_obj_BasicAudio.Volume < -100)
                {
                    trackBar1.Value = trackBar1.Value + 100;
                    volumeValue = trackBar1.Value;
                }
                if (trackBar1.Value < 0 && trackBar1.Value > -10000)
                {
                    m_obj_BasicAudio.Volume = trackBar1.Value;
                    volumeValue = trackBar1.Value;
                }
                //this.Refresh();
            }
            catch
            { }
        }
        public void durationForward()
        {

            int newvalue = trackBar2.Value - 1;
            if (running && newvalue > 0)
            {
                trackBar2.Value = newvalue;
                m_obj_MediaPosition.CurrentPosition = trackBar2.Value;
            }
            if (running && newvalue <= 0)
            {
                trackBar2.Value = 0;
                m_obj_MediaPosition.CurrentPosition = trackBar2.Value;
            }
            //this.Refresh();
        }

        public void durationBackward()
        {
            int newvalue = trackBar2.Value + 1;
            if (running && newvalue < m_obj_MediaPosition.Duration)
            {
                trackBar2.Value = newvalue;
                m_obj_MediaPosition.CurrentPosition = trackBar2.Value;
            }
            if (running && newvalue >= m_obj_MediaPosition.Duration)
            {
                trackBar2.Value = (int)m_obj_MediaPosition.Duration;
                m_obj_MediaPosition.CurrentPosition = m_obj_MediaPosition.Duration;
            }
            //this.Refresh();
        }
        //Panel üstünden klavye tuþlarýný almak için gerekli fonksiyon sonu...
        #endregion

        #region çalma özellikleri
        //Çalma özellikleri ....(parçayý tekrar çal  / listeyi tekrar çal...)
        public void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (repeatList)
                repeatList = false;
            else
                repeatList = true;
        }

        public void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (repeatFile)
                repeatFile = false;
            else
                repeatFile = true;
        }
        //Çalma özellikleri sonu....
        #endregion

        #region itemlist çift týklama
        //itemlist çift týklama olayý
        public void itemList_DoubleClick(object sender, EventArgs e)
        {
            fullscreen = false;
            if ((itemList.SelectedIndex >= 0) && (itemList.SelectedIndex <= itemList.Items.Count - 1))
            {
                listoffilenames.SelectedIndex = itemList.SelectedIndex;
                fileIndex = itemList.SelectedIndex;
                try
                {
                    volumeValue = trackBar1.Value;
                }
                catch { }
                if (videoplaying)
                {
                    //playerform.Close();
                }
                OpenFileFuncTh(fileIndex);
            }
        }
        //itemlist çift týklama olayý sonu
        #endregion

        #region itemlist dragdrop olayý
        //Ýtemlist üstüne drag drop olayý....
        public void itemList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.All;
            }
        }
        public void itemList_DragDrop(object sender, DragEventArgs e)
        {
            DragDropEvent(sender, e);
        }
        #endregion

        #region itemlist klavye kýsayollarý
        public void itemList_KeyUp(object sender, KeyEventArgs e)
        {
            itemlistActive = true;
            if (e.KeyData == Keys.Enter)
            {
                if ((itemList.SelectedIndex >= 0) && (itemList.SelectedIndex <= itemList.Items.Count - 1))
                {
                    //if (playerform != null)
                    //playerform.Close();
                    OpenFileFuncTh(itemList.SelectedIndex);
                }
            }

            if (e.KeyData == Keys.Delete && itemList.Items.Count != 0)
            {
                if (itemList.Items.Count != 0)
                {
                    int indexofdelete = itemList.SelectedIndex;
                    if (indexofdelete <= itemList.Items.Count - 1 && indexofdelete != -1)
                    {
                        itemList.Items.RemoveAt(indexofdelete);
                        listoffilenames.Items.RemoveAt(indexofdelete);
                        //searchform.deleteitem(indexofdelete);
                        if (fileIndex == indexofdelete && fileIndex < itemList.Items.Count - 1)
                        {
                            ReLoad();
                            itemList.SelectedIndex = indexofdelete;
                            if (!running && !videoplaying)
                            {
                                OpenFileFuncTh(fileIndex);
                            }
                            else if (videoplaying && !running)
                            {
                                playerform.Hide();
                                OpenFileFuncTh(fileIndex);
                            }
                        }
                        else if (fileIndex == indexofdelete && fileIndex == itemList.Items.Count)
                        {
                            ReLoad();
                            UpdateToolStrip();
                            if (!running && !videoplaying)
                            {
                                fileIndex -= 1;
                                itemList.SelectedIndex = fileIndex;
                                listoffilenames.SelectedIndex = fileIndex;
                                if (fileIndex != -1)
                                    OpenFileFuncTh(fileIndex);
                            }
                            if (!running && videoplaying)
                            {
                                fileIndex -= 1;
                                playerform.Hide();
                                if (fileIndex != -1)
                                    OpenFileFuncTh(fileIndex);
                            }
                        }
                        if (indexofdelete < fileIndex)
                            fileIndex -= 1;
                        if (itemList.Items.Count > 0)
                        {
                            if (indexofdelete < itemList.Items.Count)
                                itemList.SelectedIndex = indexofdelete;
                            if (indexofdelete == itemList.Items.Count)
                                itemList.SelectedIndex = indexofdelete - 1;
                        }
                        else
                        {
                            clearB.Enabled = false;
                            trackBar2.Value = 0;
                            filename.Text = "There is no file to play";
                        }
                        try
                        {
                            if (searchform != null)
                                searchform.listBox1.SelectedIndex = itemList.SelectedIndex;
                        }
                        catch { }
                        UpdateToolStrip();
                    }
                }
                if (searchform != null)
                {
                    KeyEventArgs ess = new KeyEventArgs(Keys.A);
                    object senderss = new object();
                    searchform.textBox1_KeyUp(senderss, ess);
                    ess = new KeyEventArgs(Keys.Back);
                    searchform.textBox1_KeyUp(senderss, ess);
                }
            }
        }
        //Klavye kýsayollarý sonu...
        #endregion

        #region itemlist farklý renk
        //itemlistteki farklý renk olayý
        public void itemList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (itemList.Items.Count > 0 && getfileindex() < itemList.Items.Count && getfileindex() != -1)
            {
                e.DrawBackground();
                Brush textBrush = SystemBrushes.ControlText;
                Font drawFont = e.Font;
                if (e.Index == listoffilenames.SelectedIndex)
                {
                    textBrush = Brushes.Goldenrod;
                    if ((e.State & DrawItemState.Selected) > 0)
                        drawFont = new Font(drawFont.FontFamily, drawFont.Size, FontStyle.Bold);
                }
                else if ((e.State & DrawItemState.Selected) > 0)
                {
                    textBrush = SystemBrushes.HighlightText;
                }
                else
                {
                    textBrush = Brushes.White;
                    if ((e.State & DrawItemState.Selected) > 0)
                        drawFont = new Font(drawFont.FontFamily, drawFont.Size, FontStyle.Bold);
                }

                e.Graphics.DrawString(itemList.Items[e.Index].ToString(), drawFont, textBrush, e.Bounds);
            }
        }
        //itemlist farklý renk sonu
        #endregion

        #region klasör ekleme olayý
        public void MakeDecision(string director)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(director.ToString()))
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        string[] fileType = f.ToString().Split('.');
                        if (fileType[fileType.Length - 1].ToLower() == "wma" || fileType[fileType.Length - 1].ToLower() == "snd" || fileType[fileType.Length - 1].ToLower() == "aif" || fileType[fileType.Length - 1].ToLower() == "aiff" || fileType[fileType.Length - 1].ToLower() == "au" || fileType[fileType.Length - 1].ToLower() == "mp3" || fileType[fileType.Length - 1] == "mp4" || fileType[fileType.Length - 1].ToLower() == "avi" || fileType[fileType.Length - 1].ToLower() == "mpg" || fileType[fileType.Length - 1].ToLower() == "mpeg" || fileType[fileType.Length - 1].ToLower() == "wmv" || fileType[fileType.Length - 1].ToLower() == "mov" || fileType[fileType.Length - 1].ToLower() == "flv" || fileType[fileType.Length - 1].ToLower() == "wav" || fileType[fileType.Length - 1].ToLower() == "wma" || fileType[fileType.Length - 1].ToLower() == "vob" || fileType[fileType.Length - 1].ToLower() == "dat")
                            addlist(f.ToString());
                    }
                    MakeDecision(d);
                }
            }
            catch (System.Exception excpt)
            {
                MessageBox.Show("Be sure that this folder includes valid Media files.");
            }
        }

        public void fileadderfromdirectory(DirectoryInfo director1)
        {
            filesfromdirectory = new string[2000];
            int filesayi = 0;
            if (director1.GetFiles().Length > 0)
            {
                FileInfo[] Files = director1.GetFiles();
                if (Files.Length > 0)
                {
                    foreach (FileInfo DirFile in Files)
                    {
                        string[] fileType = DirFile.Name.Split('.');
                        if (fileType[fileType.Length - 1].ToLower() == "wma" || fileType[fileType.Length - 1].ToLower() == "snd" || fileType[fileType.Length - 1].ToLower() == "aif" || fileType[fileType.Length - 1].ToLower() == "aiff" || fileType[fileType.Length - 1].ToLower() == "au" || fileType[fileType.Length - 1].ToLower() == "mp3" || fileType[fileType.Length - 1] == "mp4" || fileType[fileType.Length - 1].ToLower() == "avi" || fileType[fileType.Length - 1].ToLower() == "mpg" || fileType[fileType.Length - 1].ToLower() == "mpeg" || fileType[fileType.Length - 1].ToLower() == "wmv" || fileType[fileType.Length - 1].ToLower() == "mov" || fileType[fileType.Length - 1].ToLower() == "flv" || fileType[fileType.Length - 1].ToLower() == "wav" || fileType[fileType.Length - 1].ToLower() == "wma" || fileType[fileType.Length - 1].ToLower() == "vob" || fileType[fileType.Length - 1].ToLower() == "dat")
                        {
                            filesfromdirectory[filesayi] = DirFile.Directory.ToString() + "\\" + DirFile.Name;
                            filesayi++;
                        }
                    }
                    if (filesayi != 0)
                        addList(filesfromdirectory, filesayi);
                }
            }
        }

        public DirectoryInfo[] getDirectories(String strDir)
        {
            DirectoryInfo dir = new DirectoryInfo(strDir);
            DirectoryInfo[] childDirs = dir.GetDirectories();
            return childDirs;
        }
        #endregion

        #region tam ekran
        //Tam Ekran fonksiyonu....
        public static void fullscreenfunc()
        {
            if (videoplaying)
            {
                if (!fullscreen && videoplaying)
                {
                    try
                    {
                        playerformHeight = playerform.Height;
                        playerformWidth = playerform.Width;
                        playerformLocationX = playerform.Location.X;
                        playerformLocationY = playerform.Location.Y;
                        m_obj_VideoWindow.SetWindowPosition(playerform.ClientRectangle.Left,
                            playerform.ClientRectangle.Top,
                            playerform.ClientRectangle.Right,
                            playerform.ClientRectangle.Bottom);
                    }
                    catch (Exception)
                    {
                        m_obj_VideoWindow = null;
                    }
                    playerform.WindowState = FormWindowState.Maximized;
                    playerform.FormBorderStyle = FormBorderStyle.None;
                    fullscreen = true;
                }
                else
                {
                    playerform.WindowState = FormWindowState.Normal;
                    playerform.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                    try
                    {
                        playerform.Width = playerformWidth;
                        playerform.Height = playerformHeight;
                        playerform.Location = new Point(playerformLocationX, playerformLocationY);
                        m_obj_VideoWindow.SetWindowPosition(playerform.ClientRectangle.Left,
                            playerform.ClientRectangle.Top,
                            playerform.ClientRectangle.Right,
                            playerform.ClientRectangle.Bottom);
                    }
                    catch (Exception)
                    {
                        m_obj_VideoWindow = null;
                    }
                    fullscreen = false;
                    playerform.mousedown = false;
                    playerform.resizing = false;
                }
            }
        }
        //Tam ekran fonksiyonu sonu...
        #endregion

        #region video penceresi olaylarý
        //Player formunun büyüklük deðiþiminde çalýþacak fonksiyon...
        public void resize()
        {
            if (playerform.WindowState == FormWindowState.Maximized)
            {
                playerform.FormBorderStyle = FormBorderStyle.None;

                try
                {
                    m_obj_VideoWindow = m_obj_FilterGraph as IVideoWindow;
                    m_obj_VideoWindow.Owner = (int)playerform.Handle;
                    m_obj_VideoWindow.WindowStyle = WS_CHILD;
                    m_obj_VideoWindow.SetWindowPosition(playerform.ClientRectangle.Left,
                        playerform.ClientRectangle.Top,
                        playerform.ClientRectangle.Right,
                        playerform.ClientRectangle.Bottom);
                    playerform.Show();
                }
                catch (Exception)
                {
                    m_obj_VideoWindow = null;
                }

            }
            else
            {
                playerform.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                try
                {
                    m_obj_VideoWindow = m_obj_FilterGraph as IVideoWindow;
                    m_obj_VideoWindow.Owner = (int)playerform.Handle;
                    m_obj_VideoWindow.WindowStyle = WS_CHILD;
                    m_obj_VideoWindow.SetWindowPosition(playerform.ClientRectangle.Left,
                        playerform.ClientRectangle.Top,
                        playerform.ClientRectangle.Right,
                        playerform.ClientRectangle.Bottom);
                    playerform.Show();
                }
                catch (Exception)
                {
                    m_obj_VideoWindow = null;
                }
            }
        }

        public void resizeWithMouse(bool whell)
        {
            if (whell)
            {
                playerform.SetBounds(playerform.Location.X, playerform.Location.Y, playerform.Width + 10, playerform.Height + 10);
                playerform.mousedown = false;
                playerform.resizing = false;
            }
            if (!whell)
            {
                playerform.SetBounds(playerform.Location.X, playerform.Location.Y, playerform.Width - 10, playerform.Height - 10);
                playerform.mousedown = false;
                playerform.resizing = false;
            }
        }
        //Player boyut deðiþiminde çalýþacak fonksiyon sonu....

        //Mouse Hareketiyle ses deðiþimi
        public void volumeWithMouse(bool volumeWM)
        {
            if (running)
            {
                if (volumeWM)
                {
                    volumeUp();
                }
                else
                {
                    volumeDown();
                }
            }
        }
        //Mouse hareketiyle ses deðiþimi sonu

        #endregion

        #region Notify
        //Saðdaki butonlarýn fonksiyonlarý...



        protected void m_notifyicon_MouseMove(object sender, MouseEventArgs e)
        {
            if (running)
                m_notifyicon.Text = itemList.Items[fileIndex].ToString();
        }

        public void m_notifyicon_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.isHidden)
            {
                if (MouseButtons.Left == e.Button)
                {
                    itemList.SelectedIndex = fileIndex;
                    Show();
                    m_notifyicon.Visible = false;
                    isHidden = false;
                }
            }
        }

        //Sað týkla açýlan menu fonksiyonlarý....
        public void Exit_Click(Object sender, System.EventArgs e)
        {
            m_notifyicon.Dispose();
            Close();
        }

        public void Mute_Click(Object sender, System.EventArgs e)
        {
            mute = !mute;
            m_notifyicon.Visible = true;
            if (running)
            {
                if (mute)
                {
                    m_obj_BasicAudio.Volume = -10000;
                    m_menu.MenuItems[2].Text = "Open";
                }
                else
                {
                    m_obj_BasicAudio.Volume = trackBar1.Value;
                    m_menu.MenuItems[2].Text = "Mute";
                }

            }
        }

        public void Hide_Click(Object sender, System.EventArgs e)
        {
            m_notifyicon.Visible = true;
            Hide();
            isHidden = true;
        }

        public void Next_Click(Object sender, System.EventArgs e)
        {
            m_notifyicon.Visible = true;
            if (fileIndex < itemList.Items.Count - 1)
            {
                fileIndex++;
                OpenFileFuncTh(fileIndex);
            }
            else if (itemList.Items.Count == 0)
            {
                MessageBox.Show("There is no file to play");
            }
            else
            {
                fileIndex = 0;
                OpenFileFuncTh(fileIndex);
            }
        }

        public void Previous_Click(Object sender, System.EventArgs e)
        {
            m_notifyicon.Visible = true;
            if (fileIndex > 0)
            {
                fileIndex--;
                OpenFileFuncTh(fileIndex);
            }
            else if (itemList.Items.Count == 0)
            {
                MessageBox.Show("There is no file to play");
            }
            else
            {
                fileIndex = itemList.Items.Count - 1;
                OpenFileFuncTh(fileIndex);
            }
        }

        public void Show_Click(Object sender, System.EventArgs e)
        {
            m_notifyicon.Visible = false;
            this.ShowInTaskbar = true;
            Show();
            isHidden = false;
        }
        //Sað týkla açýlan menu fonksiyonlarý sonu....

        protected void Playlist_Show(Object sender, System.EventArgs e)
        {
            searchB_Click(sender, e);
        }


        //Notify icon la ilgili fonksiyonlar
        public void openModifyWindow()
        {
            if (running)
            {
                int s = (int)m_obj_MediaPosition.Duration;
                int h = s / 3600;
                int m = (s - (h * 3600)) / 60;
                s = s - (h * 3600 + m * 60);
                string time = String.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);
                string[] fullfilename = listoffilenames.Items[fileIndex].ToString().Split('\\');
                NotifyWindow notify = new NotifyWindow("Circass Media Player", fullfilename[fullfilename.Length - 1].ToString() + "\n Time ::" + time);
                notify.TitleClicked += new System.EventHandler(titleClick);
                notify.TextClicked += new System.EventHandler(textClick);
                notify.SetDimensions(130, 100);
                notify.Notify();
            }
        }

        public void titleClick(object sender, System.EventArgs e)
        {
            if (this.IsAccessible)
            {
            }
            else
            {
                itemList.SelectedIndex = fileIndex;
                if (isHidden)
                {
                    m_notifyicon.Visible = false;
                    Show();
                    isHidden = false;
                }
            }
        }

        public void textClick(object sender, System.EventArgs e)
        {
            if (this.IsAccessible)
            {

            }
            else
            {
                itemList.SelectedIndex = fileIndex;
                if (isHidden)
                {
                    Show();
                    isHidden = false;
                    m_notifyicon.Visible = false;
                }

            }
        }
        //Notify icon la ilgili fonksiyonlar

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            m_menu = new ContextMenu();
            m_menu.MenuItems.Add(0,
                new MenuItem("Show Form", new System.EventHandler(Show_Click)));

            m_menu.MenuItems.Add(1,
                new MenuItem("Close CMP", new System.EventHandler(Exit_Click)));

            m_menu.MenuItems.Add(2,
                new MenuItem("Mute", new System.EventHandler(Mute_Click)));

            m_menu.MenuItems.Add(3,
                 new MenuItem("Next", new System.EventHandler(Next_Click)));

            m_menu.MenuItems.Add(4,
                 new MenuItem("Previous", new System.EventHandler(Previous_Click)));

            m_menu.MenuItems.Add(5,
                 new MenuItem("Play List", new System.EventHandler(Playlist_Show)));

            m_notifyicon.Text = "CircassMediaPlayer(Right Click)";
            m_notifyicon.Icon = new Icon(GetType(), "iPod Video White.ico");
            m_notifyicon.Visible = true;

            m_notifyicon.ContextMenu = m_menu;
            //m_notifyicon.DoubleClick += new System.EventHandler(m_notifyicon_DoubleClick);
            m_notifyicon.MouseMove += new MouseEventHandler(m_notifyicon_MouseMove);
            m_notifyicon.MouseClick += new MouseEventHandler(m_notifyicon_MouseClick);
            Hide();
            isHidden = true;
        }
        //Tray iconda sað týklanýnca çalýþacak fonksitonlarý sonu....

        //Saðdaki butonlarýn sonu...
        #endregion

        #region form elemanlarý events
        public void From1_DoubleClick(object sender, EventArgs e)
        {
            fullscreenfunc();
        }
        private void exitb_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }
        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            fullscreenfunc();
        }

        protected void ConvertVideo(object sender, System.EventArgs e)
        {
            CircassConverter fileConverter = new CircassConverter();
            fileConverter.Show();
        }

        protected void DownloadVideo(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process.Start(standart + "\\YoutubeIndirici.exe");
        }
        protected void subTitleEditor(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process.Start(standart + "\\SubTitleEditor.exe");
        }

        protected void cropVidTakeSound(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process.Start(standart + "\\CircassCropTakeVideoSound.exe");
        }

        protected void AudioConvert(object sender, System.EventArgs e)
        {
            AudioConvert convertaudio = new AudioConvert();
            convertaudio.Show();
        }

        protected void OpenCircasDic(object sender, System.EventArgs e)
        {
            CircassDic sozluk = new CircassDic();
            sozluk.Show();
        }

        protected void GetListFromServer(object sender, System.EventArgs e)
        {
            newServerCon = new ServerConnect(itemList, listoffilenames, TargetUri);
            Thread.Sleep(100);
            newServerCon.Show();
        }

        //protected void CdRiperForm(object sender, System.EventArgs e)
        //{
        //    CdRip = new RipTracks();
        //    Thread.Sleep(100);
        //    CdRip.Show();
        //}

        private void panel1_Click(object sender, EventArgs e)
        {
            exitb.Focus();
        }

        public int getfileindex()
        {
            return fileIndex;
        }

        public void setfileindex(int indexfile)
        {
            fileIndex = indexfile;
        }

        private void listoffilenames_SelectedIndexChanged(object sender, EventArgs e)
        {
            setfileindex(listoffilenames.SelectedIndex);
            itemList.Refresh();
        }
        public void clearB_MouseClick(object sender, MouseEventArgs e)
        {
            listoffilenames.Items.Clear();
            itemList.Items.Clear();
            if (playerform != null)
                playerform.Close();
            //clearsB.Enabled = false;
            clearB.Enabled = false;
            if (running)
                m_obj_FilterGraph.Stop();
            fileIndex = -1;
            running = false;
            trackBar2.Value = 0;
            ReLoad();
            UpdateToolStrip();
            filename.Text = "There is no file to play";
            if (searchform != null)
            {
                KeyEventArgs ess = new KeyEventArgs(Keys.A);
                object senderss = new object();
                searchform.textBox1_KeyUp(senderss, ess);
                ess = new KeyEventArgs(Keys.Back);
                searchform.textBox1_KeyUp(senderss, ess);
            }
            itemlistlistfile = -1;
        }

        private void button3_MouseClick(object sender, MouseEventArgs e)
        {
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Media Files|*.mp4;*.vob;*.flv;*.avi;*.mpg;*.mp3;*.wmv;*.wav|All Formats|*.*";
            if (DialogResult.OK == openFileDialog.ShowDialog())
            {
                addList(openFileDialog);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CircassDic getVideo = new CircassDic();
            getVideo.Show();
        }

        public void panel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.All;
            }
        }

        public void panel1_DragDrop(object sender, DragEventArgs e)
        {
            DragDropEvent(sender, e);
        }

        private void From1_MouseEnter(object sender, EventArgs e)
        {
            this.Activate();
        }

        private void toolStrip1_MouseEnter(object sender, EventArgs e)
        {
            toolStrip1.Focus();
        }

        private void randomPlay_CheckedChanged(object sender, EventArgs e)
        {
            randomplay = !randomplay;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        #endregion

        #region mouse týkýyla istenilen süreye gitme

        public void trackBar2_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                double fark = Convert.ToInt64(Cursor.Position.X - this.Location.X - 22);
                if (fark < 0)
                    fark = 0;
                if (fark > trackBar2.Width - 20)
                    fark = trackBar2.Width - 20;
                double oran = m_obj_MediaPosition.Duration / (trackBar2.Width - 20);
                m_obj_MediaPosition.CurrentPosition = (oran * fark);
            }
            catch (Exception explain)
            {
                MessageBox.Show(explain.ToString());
            }
        }
        #endregion

        #region parça süresi kalan geçen süre gösterimi
        // parça süresi kalan geçen süre gösterimi
        public bool reverse = false;
        public void label3_Click(object sender, EventArgs e)
        {
            try
            {
                if (!reverse)
                {
                    int s = (int)(m_obj_MediaPosition.Duration - m_obj_MediaPosition.CurrentPosition);
                    int h = s / 3600;
                    int m = (s - (h * 3600)) / 60;
                    s = s - (h * 3600 + m * 60);
                    label3.Text = String.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);
                    reverse = true;
                }
                else
                {
                    int s = (int)m_obj_MediaPosition.CurrentPosition;
                    int h = s / 3600;
                    int m = (s - (h * 3600)) / 60;
                    s = s - (h * 3600 + m * 60);
                    label3.Text = String.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);
                    reverse = false;
                }
            }
            catch { }
        }
        // parça süresi kalan geçen süre gösterimi sonu
        #endregion

        public void DragDropEvent(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            int sayi = 0;
            for (sayi = 0; sayi < files.Length; sayi++)
            {
                string file = files[sayi].ToString();
                string[] fileName = file.Split('\\');
                string[] fileType = fileName[fileName.Length - 1].Split('.');
                string fullPath = files[sayi].ToString();
                string typeOffile = "";

                DirectoryInfo director = new DirectoryInfo(file);

                if (fileType[fileType.Length - 1].ToLower() == "cmpl")
                {
                    typeOffile = "cmpl";
                }
                else if (fileType[fileType.Length - 1].ToLower() == "wma" || fileType[fileType.Length - 1].ToLower() == "snd" || fileType[fileType.Length - 1].ToLower() == "aif" || fileType[fileType.Length - 1].ToLower() == "aiff" || fileType[fileType.Length - 1].ToLower() == "au" || fileType[fileType.Length - 1].ToLower() == "mp3" || fileType[fileType.Length - 1] == "mp4" || fileType[fileType.Length - 1].ToLower() == "avi" || fileType[fileType.Length - 1].ToLower() == "mpg" || fileType[fileType.Length - 1].ToLower() == "mpeg" || fileType[fileType.Length - 1].ToLower() == "wmv" || fileType[fileType.Length - 1].ToLower() == "mov" || fileType[fileType.Length - 1].ToLower() == "flv" || fileType[fileType.Length - 1].ToLower() == "wav" || fileType[fileType.Length - 1].ToLower() == "wma" || fileType[fileType.Length - 1].ToLower() == "vob" || fileType[fileType.Length - 1].ToLower() == "dat" || fileType[fileType.Length - 1].ToLower() == "3gp")
                {
                    typeOffile = "media";
                }
                else if (director.Attributes == director.Attributes)
                {
                    typeOffile = "director";
                }

                else
                    typeOffile = fileType[fileType.Length - 1].ToString();
                switch (typeOffile)
                {
                    case "cmpl":
                        int filesayi = 0;
                        StreamReader sr = new StreamReader(fullPath);
                        fileNames = new String[1000];
                        filesayi = 0;
                        while (!sr.EndOfStream)
                        {
                            fileNames[filesayi] = sr.ReadLine().ToString();
                            filesayi++;
                        }
                        addList(fileNames, filesayi);
                        break;
                    case "media":
                        string[] mp3files = new String[1000];
                        mp3files[0] = files[sayi].ToString();
                        addList(mp3files, 1);
                        break;
                    case "director":
                        this.filesayisi = 0;
                        this.NumOfDirs = 0;
                        DirectoryInfo basedirectory = new DirectoryInfo(files[sayi].ToString());
                        if (basedirectory.GetFiles().Length > 0)
                            fileadderfromdirectory(basedirectory);
                        MakeDecision(basedirectory.ToString());
                        break;
                    default:
                        MessageBox.Show(typeOffile + " format is not supported.\n Be sure that this file is a valid media file.");
                        break;
                }
            }
            searchform = new search(itemList, listoffilenames, fileIndex, filename);
            searchform.Refresh();
        }
    }
}