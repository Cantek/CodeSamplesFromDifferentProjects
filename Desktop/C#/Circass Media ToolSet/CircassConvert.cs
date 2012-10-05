using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;

using DirectShowLib;

namespace CircassMediaToolSet
{
    public partial class CircassConvert : Form
    {

        // The main com object
        FilterGraph fg = null;
        // The graphbuilder interface ref
        IGraphBuilder gb = null;
        // The mediacontrol interface ref
        IMediaControl mc = null;
        // The mediaevent interface ref
        IMediaEventEx me = null;

        // Matroska support filter interface
        IBaseFilter matroska_mux = null;

        // variable to store the filename and extension
        string fName, fExt;
        // one "global" hr
        int hr;

        
        public CircassConvert()
        {
            InitializeComponent();
        }

        private void start_Click(object sender, EventArgs e)
        {
            topProgressBar.Value = 0;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Media Files|*.mp4;*.vob;*.flv;*.avi;*.mpg;*.mp3;*.wmv;*.wav|All Formats|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(ofd.FileName);
                fExt = fi.Extension;
                fName = fi.FullName.Substring(0, fi.FullName.Length - fExt.Length);
                string[] curFileName = fName.Split('\\');
                fileName.Text = curFileName[curFileName.Length - 1].ToString() + fExt;
                CloseInterfaces();
                InitInterfaces();
            }

        }

        private void convert_Click(object sender, EventArgs e)
        {
            if (toMkv.Checked)
                Convert2Mkv(fName);
            else if (toWmv.Checked)
                Convert2Wmv(fName);
        }

        void InitInterfaces()
        {
            try
            {
                fg = new FilterGraph();
                gb = (IGraphBuilder)fg;
                mc = (IMediaControl)fg;
                me = (IMediaEventEx)fg;
            }
            catch (Exception)
            {
                MessageBox.Show("Baslatilamadi");
            }
        }

        //
        //This method stops the filter graph 
        //
        int WM_GRAPHNOTIFY = 0x00008001;
        void CloseInterfaces()
        {
            if (me != null)
            {
                hr = mc.Stop();
                DsError.ThrowExceptionForHR(hr);

                hr = me.SetNotifyWindow(IntPtr.Zero, WM_GRAPHNOTIFY, IntPtr.Zero);
                DsError.ThrowExceptionForHR(hr);
            }
            mc = null;
            me = null;
            gb = null;
            if (matroska_mux != null)
                Marshal.ReleaseComObject(matroska_mux);
            matroska_mux = null;
            if (fg != null)
                Marshal.ReleaseComObject(fg);
            fg = null;
        }


        //
        // This method converts the input file to a mkv file
        //
        void Convert2Mkv(string fileName)
        {
            try
            {

                progressText.Text = "Dönüþtürme iþlemi devam ediyor";
                start.Enabled = false;
                convert.Enabled = false;
                SaveFileDialog saveFile = new SaveFileDialog();
                if (saveFile.ShowDialog() == DialogResult.OK)
                {

                    hr = me.SetNotifyWindow(this.Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
                    DsError.ThrowExceptionForHR(hr);

                    // we want to add a filewriter filter to the filter graph
                    FileWriter file_writer = new FileWriter();

                    // make sure we access the IFileSinkFilter interface to
                    // set the file name
                    IFileSinkFilter fs = (IFileSinkFilter)file_writer;

                    fs.SetFileName(saveFile.FileName + ".mkv", null);
                    string[] fileNewName = saveFile.FileName.Split('\\');
                    outPutText.Text = fileNewName[fileNewName.Length - 1].ToString()+".mkv";

                    DsError.ThrowExceptionForHR(hr);

                    // add the filter to the graph
                    hr = gb.AddFilter((IBaseFilter)file_writer, "File Writer");
                    DsError.ThrowExceptionForHR(hr);

                    // create an instance of the matroska multiplex filter and add it
                    // Matroska Mux Clsid = {1E1299A2-9D42-4F12-8791-D79E376F4143}	
                    Guid guid = new Guid("1E1299A2-9D42-4F12-8791-D79E376F4143");
                    Type comtype = Type.GetTypeFromCLSID(guid);
                    matroska_mux = (IBaseFilter)Activator.CreateInstance(comtype);

                    hr = gb.AddFilter((IBaseFilter)matroska_mux, "Matroska Muxer");
                    DsError.ThrowExceptionForHR(hr);

                    // use Intelligent connect to build the rest of the graph
                    hr = gb.RenderFile(fileName + fExt, null);
                    DsError.ThrowExceptionForHR(hr);

                    // we are ready to convert
                    hr = mc.Run();
                    DsError.ThrowExceptionForHR(hr);
                }
                else
                    convert.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Mkv ye çevirilirken hata olustu " + ex.Message);
                timerThread.Stop();
                topProgressBar.Value = 0;
                fileName.Text = "";
                outPutText.Text = "";
                start.Enabled = true;
                convert.Enabled = true;
                progressText.Text = "Hata olustu, dönüstürme islemi devam ettirilemiyor";
            }
        }

        //
        // This method convert the input file to an wmv file
        //
        void Convert2Wmv(string fileName)
        {
            try
            {
                progressText.Text = "Dönüstürme islemi devam ediyor";
                start.Enabled = false;
                convert.Enabled = false;
                SaveFileDialog saveFile = new SaveFileDialog();
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    hr = me.SetNotifyWindow(this.Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
                    DsError.ThrowExceptionForHR(hr);

                    // here we use the asf writer to create wmv files
                    WMAsfWriter asf_filter = new WMAsfWriter();
                    IFileSinkFilter fs = (IFileSinkFilter)asf_filter;
                    hr = fs.SetFileName(saveFile.FileName + ".wmv", null);
                    string[] fileNewName = saveFile.FileName.Split('\\');
                    outPutText.Text = fileNewName[fileNewName.Length - 1].ToString()+".wmv";
                    DsError.ThrowExceptionForHR(hr);

                    hr = gb.AddFilter((IBaseFilter)asf_filter, "WM Asf Writer");
                    DsError.ThrowExceptionForHR(hr);

                    hr = gb.RenderFile(fileName + fExt, null);
                    DsError.ThrowExceptionForHR(hr);

                    hr = mc.Run();
                    DsError.ThrowExceptionForHR(hr);
                }
                else
                    convert.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Wmv ye çevirilirken hata olustu " + ex.Message);
                timerThread.Stop();
                topProgressBar.Value = 0;
                fileName.Text = "";
                outPutText.Text = "";
                start.Enabled = true;
                convert.Enabled = true;
                progressText.Text = "Hata oluþtu, dönüþtürme iþlemi devam ettirilemiyor";
            }
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_GRAPHNOTIFY)
            {
                timerThread.Enabled = true;
                timerThread.Start();
                int p1, p2;
                EventCode code;
                if (me == null)
                    return;
                while (me.GetEvent(out code, out p1, out p2, 0) == 0)
                {
                    if (code == EventCode.Complete)
                    {
                        timerThread.Stop();
                        timerThread.Enabled = false;
                        while (topProgressBar.Maximum != topProgressBar.Value)
                        {
                            topProgressBar.PerformStep();
                            topProgressBar.Refresh();
                        }
                        progressText.Text = "Dönüþtürme iþlemi tamamlandý";
                        start.Enabled = true;
                        convert.Enabled = true;
                        mc.Stop();
                    }
                    me.FreeEventParams(code, p1, p2);
                }
                return;
            }
            base.WndProc(ref m);
        }

        private void timerThread_Tick(object sender, EventArgs e)
        {
            if(topProgressBar.Value < topProgressBar.Maximum-100)
                topProgressBar.PerformStep();
            topProgressBar.Refresh();
        }

    }
}