using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Threading;

namespace CircassDesktopToolBar
{
    public partial class Form7 : Form
    {
        int X = 0;
        int Y = 0;
        Form1 frm = new Form1();
        public MSNPlug.ChangeText msn = new MSNPlug.ChangeText();
        ListBox adresler = new ListBox();
        ListBox albumAdresler = new ListBox();
        ListBox resimAdresler = new ListBox();
        string[] filelistelementrekarray;
        string standart = Application.StartupPath.ToString(); 
        StreamReader sr;
        int index = 0;
        public Form7(int x, int y,Form1 form1)
        {
            InitializeComponent();
            X = x;
            Y = y;
            frm = form1;
        }

        
        private void Form2_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            for (int i = 0; i < 502; i = i + 40)
            {
                this.Width = i;
                this.SetDesktopLocation(X - i, Y);
            }
            this.Width = 502;
            this.SetDesktopLocation(X - this.Width, Y);
            Thread listeGetirth = new Thread(listeGetir);
            listeGetirth.Start();

            Thread reklamth = new Thread(reklamGetir);
            reklamth.Start();
        }

        public void listeGetir()
        {
            try
            {
                Uri connect = new Uri("http://www.saricaovaliyiz.biz/urlliste.cmpl");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(connect);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-9"));
                string filelistelement;
                while ((filelistelement = sr.ReadLine()) != null)
                {
                    if (filelistelement != "")
                    {
                        string[] filenames = filelistelement.Split('#');
                        string realfileaddress = filenames[0];
                        string filename = filenames[filenames.Length - 1];
                        listBox1.Items.Add(filename);
                        adresler.Items.Add(realfileaddress);
                    }
                }
            }
            catch { }
        }

        public void reklamGetir()
        {
            try
            {
                try
                {
                    webBrowser1.ScriptErrorsSuppressed = true;
                    webBrowser1.BringToFront();
                    Uri connectreklam = new Uri("http://www.saricaovaliyiz.biz/filmOncesiReklamList.cmpl");
                    HttpWebRequest requestrek = (HttpWebRequest)WebRequest.Create(connectreklam);
                    HttpWebResponse responserek = (HttpWebResponse)requestrek.GetResponse();
                    StreamReader srrek = new StreamReader(responserek.GetResponseStream(), Encoding.GetEncoding("ISO-8859-9"));
                    //srrek2 = new StreamReader(responserek.GetResponseStream(), Encoding.GetEncoding("ISO-8859-9"));
                    string filelistelementrek = "";
                    int reksayi = 0;
                    int i = 0;
                    string[] tempreklam = new string[100];
                    while ((filelistelementrek = srrek.ReadLine()) != null)
                    {
                        if (filelistelementrek != "")
                        {
                            tempreklam[i] = filelistelementrek;
                            i++;
                        }
                    }

                    filelistelementrekarray = new string[i];
                    for (int j = 0; j < i; j++)
                    {
                        filelistelementrekarray[j] = tempreklam[j];
                    }
                }
                catch { }
                Random randomresim = new Random();
                string reklam = filelistelementrekarray[randomresim.Next(0, filelistelementrekarray.Length)].ToString();
                StreamWriter sw = new StreamWriter(standart + "/reklam.html", false, Encoding.GetEncoding("iso-8859-9"));
                sw.WriteLine("<html><head><title>" + "reklam" + "</title></head><body bgcolor = black><table cellpadding = 0 cellspacing = 0 width = 100%><tr valign = top><td align = center width = '100%' height = '323'>" + reklam + "</td></tr></table></body></html>");
                sw.Flush();
                sw.Close();
                Uri fileuri = new Uri(standart + "/reklam.html");
                webBrowser1.Url = fileuri;
            }
            catch { }
        }


        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            frm.yaratildi7 = false;
            for (int i = 502; i > 0; i = i - 50)
            {
                this.Width = i;
                this.SetDesktopLocation(X - i, Y);
            }
            this.Width = 0;
            this.SetDesktopLocation(X, Y);
        }

        private void panel5_MouseClick(object sender, MouseEventArgs e)
        {
            this.Visible = false;
            this.SetBounds(X - this.Width, Y, this.Width, this.Height);
            this.Visible = true;
            this.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                listBox2.Items.Clear();
                albumAdresler.Items.Clear();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(adresler.Items[listBox1.SelectedIndex].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-9"));
                string filelistelement;
                while ((filelistelement = sr.ReadLine()) != null)
                {
                    if (filelistelement != "")
                    {
                        string[] filenames = filelistelement.Split('#');
                        string realfileaddress = filenames[0];
                        string filename = filenames[filenames.Length - 1];
                        albumAdresler.Items.Add(realfileaddress);
                        listBox2.Items.Add(filename);
                    }
                }
                sr.Dispose();
            }
            catch { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            index = listBox3.SelectedIndex;
            ThreadPool.QueueUserWorkItem(new WaitCallback(SetBackground), new Uri(resimAdresler.Items[index].ToString()));
            string strIcon = "Music";
            string msntext = "Circass Media Toolbar Duvar Kağıtları -> " + listBox2.SelectedItem.ToString();
            string ikinci = "Circass Media Toolbar Duvar Kağıtları -> ";
            string ucuncu = listBox2.SelectedItem.ToString();
            bool onay = true;

            msn.Send(ref msntext, ref ucuncu, ref ikinci, ref strIcon, ref onay);

        }

        private void SetBackground(object url)
        {
            SetBackground((Uri)url);
        }

        public static void SetBackground(Uri newimage)
        {
            string s = "Stretched";
            Wallpaper.Style s2 = (Wallpaper.Style)Enum.Parse(typeof(Wallpaper.Style), s, false);
            Wallpaper.Set(newimage, s2);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                listBox3.Items.Clear();
                resimAdresler.Items.Clear();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(albumAdresler.Items[listBox2.SelectedIndex].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-9"));
                string filelistelement;
                while ((filelistelement = sr.ReadLine()) != null)
                {
                    if (filelistelement != "")
                    {
                        string[] filenames = filelistelement.Split('#');
                        string realfileaddress = filenames[0];
                        string filename = filenames[filenames.Length - 1];
                        resimAdresler.Items.Add(realfileaddress);
                        listBox3.Items.Add(filename);
                    }
                }
                sr.Dispose();
            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread setPanelBGIth = new Thread(setPanelBGI);
            setPanelBGIth.Start();
            webBrowser1.Visible = false;
            listBox3.Enabled = false;
        }

        public void setPanelBGI()
        {
            try
            {
                panel2.BackgroundImage = null;
                System.Drawing.Image img = null;
                System.IO.Stream s = new WebClient().OpenRead(resimAdresler.Items[listBox3.SelectedIndex].ToString());
                img = System.Drawing.Image.FromStream(s);
                panel2.BackgroundImageLayout = ImageLayout.Stretch;
                panel2.BackgroundImage = img;
                s.Dispose();
            }
            catch
            {
            }
            listBox3.Enabled = true;
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button1_Click(sender,e);
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            button2_Click(sender,e);
        }

        private void listBox3_DoubleClick(object sender, EventArgs e)
        {
            button3_Click(sender,e);
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button2_Click(sender,e);
        }

        private void listBox3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button3_Click(sender,e);
        }

        private void listBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(sender,e);
        }

        private void listBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button2_Click(sender,e);
        }

        private void listBox3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button3_Click(sender,e);
        }
    }
    public sealed class Wallpaper
    {
        Wallpaper() { }
        ~Wallpaper() { }

        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public enum Style : int
        {
            Tiled,
            Centered,
            Stretched
        }

        public static void Set(Uri uri, Style style)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "wallpaper.bmp");
            try
            {
                File.Delete(tempPath);
            }
            catch { }
            System.IO.Stream s = new WebClient().OpenRead(uri.ToString());
            System.Drawing.Image img = System.Drawing.Image.FromStream(s);
            img.Save(tempPath, System.Drawing.Imaging.ImageFormat.Bmp);
            s.Dispose();
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            if (style == Style.Stretched)
            {
                key.SetValue(@"WallpaperStyle", 2.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            if (style == Style.Centered)
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            if (style == Style.Tiled)
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 1.ToString());
            }

            SystemParametersInfo(SPI_SETDESKWALLPAPER,
                0,
                tempPath,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}
