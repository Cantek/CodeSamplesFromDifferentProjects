using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AviFile;

namespace CircassMediaToolSet
{
    public partial class cutFile : Form
    {
        string fileName;
        VideoStream aviStream;
        string path;
        string newfileName;
        public cutFile()
        {
            InitializeComponent();
        }

        private void openFile_Click(object sender, EventArgs e)
        {
            inputFileDlg.Filter = "Media Files|*.avi;|All files|*.*;";
            if (DialogResult.OK == inputFileDlg.ShowDialog())
            {
                fileNameText.Text = inputFileDlg.FileName;
                try
                {
                    AviManager aviManager = new AviManager(fileNameText.Text, true);
                    VideoStream stream = aviManager.GetVideoStream();
                    startPointTracker.Maximum = stream.CountFrames;
                    endPointTracker.Maximum = stream.CountFrames;
                    ShowFrameTrackOne();
                    ShowFrameTrackTwo();
                }
                catch(Exception ew)
                {
                    MessageBox.Show("Lütfen avi formatinda bis dosya seçin");
                }
            }

        }

        public void makeAvi()
        {
            FrameRateForm dlg = new FrameRateForm();
            if (DialogResult.OK == dlg.ShowDialog())
            {
                int framelength = 0;
                SaveFileDialog saveAvi = new SaveFileDialog();
                Bitmap bmp = null;
                if (saveAvi.ShowDialog() == DialogResult.OK)
                {
                    bmp = (Bitmap)Image.FromFile(pathBox.Items[0].ToString());
                    AviManager manageAVIFile = new AviManager(saveAvi.FileName.ToString()+ ".avi", false);
                    aviStream = manageAVIFile.AddVideoStream(true, dlg.Rate, bmp);
                    Bitmap bitmap;
                    for (framelength = 1; framelength < pathBox.Items.Count - 1; framelength++)
                    {
                        bitmap = (Bitmap)Bitmap.FromFile(pathBox.Items[framelength].ToString());
                        aviStream.AddFrame(bitmap);
                        bitmap.Dispose();
                    }
                    manageAVIFile.Close();
                }
            }
        }

        private void ShowFrameTrackOne()
        {
            if (System.IO.File.Exists(fileNameText.Text))
            {
                try
                {
                    AviManager aviManager = new AviManager(fileNameText.Text, true);
                    VideoStream aviStream = aviManager.GetVideoStream();
                    aviStream.GetFrameOpen();

                    Bitmap bmp = aviStream.GetBitmap(Convert.ToInt32(startPointTracker.Value));
                    bmp.SetResolution(smplPicBox.Width, smplPicBox.Height);
                    rlPicBox.Image = bmp;

                    aviStream.GetFrameClose();
                    aviManager.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        private void ShowFrameTrackTwo()
        {
            if (System.IO.File.Exists(fileNameText.Text))
            {
                try
                {
                    AviManager aviManager = new AviManager(fileNameText.Text, true);
                    VideoStream aviStream = aviManager.GetVideoStream();
                    aviStream.GetFrameOpen();
                    Bitmap bmp = aviStream.GetBitmap(Convert.ToInt32(endPointTracker.Value));
                    bmp.SetResolution(rlPicBox.Width, rlPicBox.Height);
                    smplPicBox.Image = bmp;
                    aviStream.GetFrameClose();
                    aviManager.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void startPointTracker_Scroll(object sender, EventArgs e)
        {
            ShowFrameTrackOne();
            startPointTrackerText.Text = startPointTracker.Value.ToString();
        }

        private void endPointTracker_Scroll(object sender, EventArgs e)
        {
            ShowFrameTrackTwo();
            endPointTrackerText.Text = endPointTracker.Value.ToString();
        }

        private void startToConvert_Click(object sender, EventArgs e)
        {
            AviManager aviManager = new AviManager(fileNameText.Text, true);
            VideoStream stream = aviManager.GetVideoStream();
            stream.GetFrameOpen();
            fileName = fileNameText.Text;
            int length = fileName.Length, j;
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
            fileName = fileName.ToLower();
            j = 0;
            path = "";
            for (j = 0; j < fileNameText.Text.Length - fileName.Length; j++)
                path += fileNameText.Text[j];
            int startFrame = Convert.ToInt32(startPointTrackerText.Text);
            int endFrame = Convert.ToInt32(endPointTrackerText.Text);
            string bmpname = "";
            int ali = 0;
            for (int n = startFrame; n < endFrame; n++)
            {
                bmpname = ali.ToString();
                stream.ExportBitmap(n, "c:/temp/" + bmpname + ".bmp");
                pathBox.Items.Add("c:/temp/" + bmpname + ".bmp");
                ali++;
                startPointTracker.Value = n;
                textBox3.Text = n.ToString();
            }
            stream.GetFrameClose();
            aviManager.Close();
            makeAvi();
        }

        private void startPointTrackerText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                ShowFrameTrackOnefromtext();
        }
        private void ShowFrameTrackOnefromtext()
        {
            if (System.IO.File.Exists(fileNameText.Text))
            {
                try
                {
                    AviManager aviManager = new AviManager(fileNameText.Text, true);
                    VideoStream aviStream = aviManager.GetVideoStream();
                    aviStream.GetFrameOpen();

                    Bitmap bmp = aviStream.GetBitmap(Convert.ToInt32(startPointTrackerText.Text));
                    bmp.SetResolution(smplPicBox.Width, smplPicBox.Height);
                    rlPicBox.Image = bmp;

                    aviStream.GetFrameClose();
                    aviManager.Close();
                    startPointTracker.Value = Convert.ToInt32(startPointTrackerText.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        private void ShowFrameTrackTwofromtext()
        {
            if (System.IO.File.Exists(fileNameText.Text))
            {
                try
                {
                    AviManager aviManager = new AviManager(fileNameText.Text, true);
                    VideoStream aviStream = aviManager.GetVideoStream();
                    aviStream.GetFrameOpen();
                    Bitmap bmp = aviStream.GetBitmap(Convert.ToInt32(endPointTrackerText.Text));
                    bmp.SetResolution(rlPicBox.Width, rlPicBox.Height);
                    smplPicBox.Image = bmp;
                    aviStream.GetFrameClose();
                    aviManager.Close();
                    endPointTracker.Value = Convert.ToInt32(endPointTrackerText.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void endPointTrackerText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                ShowFrameTrackTwofromtext();
        }

        private void clear_Click(object sender, EventArgs e)
        {
            pathBox.Items.Clear();
        }

        private void selectPics_Click(object sender, EventArgs e)
        {
            OpenFileDialog openbmpfile = new OpenFileDialog();
            openbmpfile.Multiselect = true;
            openbmpfile.Filter = "Bmp files| *.bmp;";
            if (openbmpfile.ShowDialog() == DialogResult.OK)
            {
                int i = 0;
                int lenght = openbmpfile.FileNames.Length;
                for (i = 0; i < lenght; i++)
                {
                    pathBox.Items.Add(openbmpfile.FileNames[i].ToString());
                }
            }
        }

        private void saveWithVoiceStart_Click(object sender, EventArgs e)
        {
            AviManager aviManager = new AviManager(fileNameText.Text, true);
            VideoStream aviStream = aviManager.GetVideoStream();

			CopyForm dialog = new CopyForm(0, (int)Math.Floor(aviStream.CountFrames / aviStream.FrameRate));
            if (dialog.ShowDialog() == DialogResult.OK) {
                int startSecond = dialog.Start;
                int stopSecond = dialog.Stop;
                SaveFileDialog saveWithVoice = new SaveFileDialog();
                if (saveWithVoice.ShowDialog() == DialogResult.OK)
                {
                    noticeText.Text = fileNameText.Text+ " dosyasinin " + saveWithVoice.FileName.ToString() +" dosyasina dönüsümü basladi\r\n";
                    AviManager newFile = aviManager.CopyTo(saveWithVoice.FileName.ToString()+".avi", startSecond, stopSecond);
                    newFile.Close();
                    noticeText.Text += "...finished.";
                }
            }
            aviManager.Close();
        }

        private void openMKVConverter_Click(object sender, EventArgs e)
        {
            M2Mkv convert = new M2Mkv();
            convert.Show();
        }



    }
}