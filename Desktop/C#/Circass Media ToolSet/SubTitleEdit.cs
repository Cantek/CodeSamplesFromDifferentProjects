using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using QuartzTypeLib;
using System.Threading;
using System.Text;

namespace subTitleEditor
{
    public class Form1 : System.Windows.Forms.Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        # region deðiþkenler
        string standart = Application.StartupPath;
        ListBox kopyalist = new ListBox();
        FileStream fsubstream = null;
        private delegate void UpdateProgessCallback();
        private Panel panel1;
        private TextBox textBox1;
        private Button button4;
        private TrackBar trackBar1;
        private Panel panel3;
        private IContainer components;
        double endpos = 0;
        private Button button5;
        private Button button6;
        double startpos = 0;
        public static QuartzTypeLib.IMediaPosition m_obj_MediaPosition = null;
        public static QuartzTypeLib.IMediaControl m_obj_MediaControl = null;
        public static QuartzTypeLib.IVideoWindow m_obj_VideoWindow = null;
        public static QuartzTypeLib.FilgraphManager m_obj_FilterGraph = null;
        public static QuartzTypeLib.IBasicAudio m_obj_BasicAudio = null;
        public static QuartzTypeLib.IMediaEvent m_obj_MediaEvent = null;
        public static QuartzTypeLib.IMediaEventEx m_obj_MediaEventEx = null;
        public static int WM_APP = 0x8000;
        public static int WM_GRAPHNOTIFY = WM_APP + 1;
        public static int WM_GRAPHNOTIFYCON = 0x00008001;
        public static int EC_COMPLETE = 0x01;
        public static int WS_CHILD = 0x40000000;
        public static int WS_CLIPCHILDREN = 0x200000;
        public ToolStrip toolStrip1;
        public ToolStripButton toolStripButton1;
        public ToolStripButton toolStripButton2;
        public ToolStripButton toolStripButton3;
        private System.Windows.Forms.Timer timer1;
        private Label label2;
        private Label label3;
        private Button button7;
        public static MediaStatus m_CurrentStatus = MediaStatus.None;
        public enum MediaStatus { None, Stopped, Paused, Running };
        public delegate void OpenFileFuncDel(int fileindex);
        public bool videoplaying = false;
        public bool running = false;
        string openedfile = "";
        private TextBox textBox2;
        private Button button1;
        private ListBox listBox1;
        private Button button2;
        private ListBox listBox2;
        private ListBox listBox3;
        private Label label1;
        private Label label4;
        private Label label5;
        private Button button3;
        private Button button8;
        private Button button9;
        private Button button10;
        private Button button12;
        private FontDialog fontDialog1;
        private Button button13;
        private Button button14;
        private Button button15;
        private Button button11;
        private TextBox textBox3;
        private TextBox textBox4;
        private Label label6;
        private Panel panel2;
        private Panel panel4;
        private RadioButton radioButton4;
        private RadioButton radioButton3;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private Label label8;
        private Button button16;
        private Button button17;
        private TextBox textBox5;
        private Label label9;
        private Label label10;
        private Panel panel5;
        private TextBox textBox6;
        private Label label11;
        private Label label12;
        private Panel panel6;
        private Label menu;
        private WebBrowser webBrowser1;
        private Label label7;
        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.panel3 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button12 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button7 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.button16 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.menu = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.panel3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(12, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(320, 225);
            this.panel1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(366, 20);
            this.textBox1.TabIndex = 6;
            // 
            // button4
            // 
            this.button4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button4.BackgroundImage")));
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button4.Location = new System.Drawing.Point(373, 15);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(111, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "Select Video";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.BackColor = System.Drawing.SystemColors.ControlText;
            this.trackBar1.Location = new System.Drawing.Point(54, 274);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(288, 45);
            this.trackBar1.TabIndex = 8;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Black;
            this.panel3.Controls.Add(this.toolStrip1);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(25, 289);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(342, 30);
            this.panel3.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(30, 26);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(224, -2);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(105, 31);
            this.toolStrip1.TabIndex = 29;
            this.toolStrip1.Text = "MediaControlMenu";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked_1);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(34, 28);
            this.toolStripButton1.Text = "Play";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(34, 28);
            this.toolStripButton2.Text = "Pause";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(34, 28);
            this.toolStripButton3.Text = "Stop";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(3, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "00:00:00";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(108, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "00:00:00";
            // 
            // button12
            // 
            this.button12.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button12.BackgroundImage")));
            this.button12.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button12.Enabled = false;
            this.button12.Location = new System.Drawing.Point(373, 147);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(111, 37);
            this.button12.TabIndex = 46;
            this.button12.Text = "Create Subtitle for selected Video";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button5
            // 
            this.button5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button5.BackgroundImage")));
            this.button5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button5.Enabled = false;
            this.button5.Location = new System.Drawing.Point(22, 321);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(111, 23);
            this.button5.TabIndex = 13;
            this.button5.Text = "Mark as Start Point";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button6.BackgroundImage")));
            this.button6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button6.Enabled = false;
            this.button6.Location = new System.Drawing.Point(139, 321);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(111, 23);
            this.button6.TabIndex = 14;
            this.button6.Text = "Mark as End Point";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button7
            // 
            this.button7.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button7.BackgroundImage")));
            this.button7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button7.Enabled = false;
            this.button7.Location = new System.Drawing.Point(258, 321);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(111, 23);
            this.button7.TabIndex = 15;
            this.button7.Text = "Enter New Point(s)";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(22, 349);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(228, 57);
            this.textBox2.TabIndex = 32;
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(258, 350);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 23);
            this.button1.TabIndex = 33;
            this.button1.Text = "Add to subtitle";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(4, 445);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(304, 134);
            this.listBox1.TabIndex = 34;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button2.BackgroundImage")));
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(37, 583);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(96, 23);
            this.button2.TabIndex = 35;
            this.button2.Text = "Save as...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(314, 445);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(86, 134);
            this.listBox2.TabIndex = 36;
            // 
            // listBox3
            // 
            this.listBox3.FormattingEnabled = true;
            this.listBox3.Location = new System.Drawing.Point(403, 445);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(89, 134);
            this.listBox3.TabIndex = 37;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(9, 429);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 38;
            this.label1.Text = "Added Texts ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.Location = new System.Drawing.Point(321, 429);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 39;
            this.label4.Text = "Start Points";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label5.Location = new System.Drawing.Point(417, 429);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 40;
            this.label5.Text = "End Points";
            // 
            // button3
            // 
            this.button3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button3.BackgroundImage")));
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button3.Location = new System.Drawing.Point(324, 583);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(30, 23);
            this.button3.TabIndex = 41;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // button8
            // 
            this.button8.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button8.BackgroundImage")));
            this.button8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button8.Location = new System.Drawing.Point(360, 583);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(30, 23);
            this.button8.TabIndex = 42;
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button9.BackgroundImage")));
            this.button9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button9.Location = new System.Drawing.Point(450, 582);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(30, 23);
            this.button9.TabIndex = 44;
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button10.BackgroundImage")));
            this.button10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button10.Location = new System.Drawing.Point(414, 582);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(30, 23);
            this.button10.TabIndex = 43;
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button13
            // 
            this.button13.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button13.BackgroundImage")));
            this.button13.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button13.Enabled = false;
            this.button13.Location = new System.Drawing.Point(185, 583);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(96, 23);
            this.button13.TabIndex = 46;
            this.button13.Text = "Save";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button14
            // 
            this.button14.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button14.BackgroundImage")));
            this.button14.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button14.Location = new System.Drawing.Point(374, 190);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(111, 36);
            this.button14.TabIndex = 47;
            this.button14.Text = "Open an Existing Subtitle for Editing";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button15
            // 
            this.button15.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button15.BackgroundImage")));
            this.button15.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button15.Enabled = false;
            this.button15.Location = new System.Drawing.Point(258, 379);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(111, 23);
            this.button15.TabIndex = 48;
            this.button15.Text = "Update This Line";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button11
            // 
            this.button11.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button11.BackgroundImage")));
            this.button11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button11.Location = new System.Drawing.Point(0, 41);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(111, 22);
            this.button11.TabIndex = 49;
            this.button11.Text = "Convert Subtitle";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(1, 2);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(45, 20);
            this.textBox3.TabIndex = 50;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(66, 2);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(45, 20);
            this.textBox4.TabIndex = 51;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(47, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 13);
            this.label6.TabIndex = 32;
            this.label6.Text = "to";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label7.Location = new System.Drawing.Point(6, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 52);
            this.label7.TabIndex = 53;
            this.label7.Text = "Use this For effect\r\nall points.Enter how \r\nmany seconds do you\r\nwant to add or d" +
                "elete.\r\n";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.textBox5);
            this.panel2.Controls.Add(this.button16);
            this.panel2.Controls.Add(this.button17);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Enabled = false;
            this.panel2.Location = new System.Drawing.Point(371, 296);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(117, 124);
            this.panel2.TabIndex = 54;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label10.Location = new System.Drawing.Point(60, 108);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 13);
            this.label10.TabIndex = 59;
            this.label10.Text = "Forward";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label9.Location = new System.Drawing.Point(13, 108);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 56;
            this.label9.Text = "Back";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(9, 64);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 20);
            this.textBox5.TabIndex = 58;
            // 
            // button16
            // 
            this.button16.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button16.BackgroundImage")));
            this.button16.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button16.Location = new System.Drawing.Point(17, 85);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(30, 23);
            this.button16.TabIndex = 56;
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // button17
            // 
            this.button17.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button17.BackgroundImage")));
            this.button17.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button17.Location = new System.Drawing.Point(70, 85);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(30, 23);
            this.button17.TabIndex = 57;
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.Controls.Add(this.label8);
            this.panel4.Controls.Add(this.radioButton4);
            this.panel4.Controls.Add(this.radioButton3);
            this.panel4.Controls.Add(this.radioButton2);
            this.panel4.Controls.Add(this.radioButton1);
            this.panel4.Location = new System.Drawing.Point(374, 33);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(116, 115);
            this.panel4.TabIndex = 55;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label8.Location = new System.Drawing.Point(4, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(116, 26);
            this.label8.TabIndex = 56;
            this.label8.Text = "  Default Charset\r\niso-8859-9(Turkish)";
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(7, 95);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(65, 17);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Unicode";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.Click += new System.EventHandler(this.radioButton4_Click);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(7, 76);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(52, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "UTF8";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.Click += new System.EventHandler(this.radioButton3_Click);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(7, 56);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(60, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Turkish";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Click += new System.EventHandler(this.radioButton2_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(7, 35);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(59, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Europa";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Click += new System.EventHandler(this.radioButton1_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Transparent;
            this.panel5.Controls.Add(this.label12);
            this.panel5.Controls.Add(this.button11);
            this.panel5.Controls.Add(this.textBox3);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.textBox4);
            this.panel5.Enabled = false;
            this.panel5.Location = new System.Drawing.Point(372, 228);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(113, 65);
            this.panel5.TabIndex = 60;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Location = new System.Drawing.Point(4, 26);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 13);
            this.label12.TabIndex = 52;
            this.label12.Text = "Ex :23     to     25";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(205, 426);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(103, 20);
            this.textBox6.TabIndex = 61;
            this.textBox6.TextChanged += new System.EventHandler(this.textBox6_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label11.Location = new System.Drawing.Point(133, 429);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(66, 13);
            this.label11.TabIndex = 62;
            this.label11.Text = "Search for";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Lavender;
            this.panel6.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel6.BackgroundImage")));
            this.panel6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel6.Controls.Add(this.panel1);
            this.panel6.Controls.Add(this.webBrowser1);
            this.panel6.Location = new System.Drawing.Point(25, 39);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(342, 244);
            this.panel6.TabIndex = 63;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(85, 48);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(198, 160);
            this.webBrowser1.TabIndex = 1;
            this.webBrowser1.Url = new System.Uri("http://www.saricaovaliyiz.biz/index.php", System.UriKind.Absolute);
            // 
            // menu
            // 
            this.menu.AutoSize = true;
            this.menu.BackColor = System.Drawing.Color.Transparent;
            this.menu.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.menu.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.menu.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.menu.Location = new System.Drawing.Point(1, 1);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(40, 13);
            this.menu.TabIndex = 64;
            this.menu.Text = "About";
            this.menu.Click += new System.EventHandler(this.menu_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(498, 605);
            this.Controls.Add(this.menu);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox3);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.panel4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Circass Subtitle Editor - Creator";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region formload
        private void Form1_Load(object sender, System.EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            if (!Directory.Exists(standart + "\\subs"))
                Directory.CreateDirectory(standart + "\\subs");
        }
        #endregion

        #region çalýnacak dosyanýn açýlmasý
        public void OpenFileFunc()
        {
            ReLoad();
            string openedfile = textBox1.Text;
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
                m_obj_BasicAudio = m_obj_FilterGraph as QuartzTypeLib.IBasicAudio;
                m_obj_BasicAudio.Volume = trackBar1.Value;
            }
            catch { }
            try
            {
                m_obj_VideoWindow = m_obj_FilterGraph as QuartzTypeLib.IVideoWindow;
                m_obj_VideoWindow.Owner = (int)panel1.Handle;
                m_obj_VideoWindow.WindowStyle = WS_CHILD;
                m_obj_VideoWindow.SetWindowPosition(panel1.ClientRectangle.Left,
                    panel1.ClientRectangle.Top,
                    panel1.ClientRectangle.Right,
                    panel1.ClientRectangle.Bottom);
                videoplaying = true;
                timer1.Start();
            }
            catch (Exception)
            {
                m_obj_VideoWindow = null;
                videoplaying = false;
            }

            if (videoplaying)
            {
                trackBar1.Enabled = true;
                timer1.Start();
            }
            else
            {
                MessageBox.Show("Choose a Valid Video File");
            }
            running = true;
            m_obj_MediaEvent = m_obj_FilterGraph as QuartzTypeLib.IMediaEvent;
            m_obj_MediaEventEx = m_obj_FilterGraph as QuartzTypeLib.IMediaEventEx;
            m_obj_MediaEventEx.SetNotifyWindow((int)this.Handle, WM_GRAPHNOTIFY, 0);
            m_obj_MediaPosition = m_obj_FilterGraph as QuartzTypeLib.IMediaPosition;
            m_obj_MediaControl = m_obj_FilterGraph as QuartzTypeLib.IMediaControl;
            trackBar1.Maximum = (int)m_obj_MediaPosition.Duration;
            trackBar1.Minimum = 0;
            trackBar1.Value = (int)m_obj_MediaPosition.CurrentPosition;
            m_obj_MediaControl.Run();
            m_CurrentStatus = MediaStatus.Running;
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
            UpdateToolStrip();
            timer1.Stop();
            running = false;
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
                            timer1.Stop();
                            UpdateToolStrip();
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
            switch (clicked)
            {
                case 0:
                    if (m_CurrentStatus == MediaStatus.None)
                    {
                        OpenFileFunc();
                    }

                    if (m_CurrentStatus == MediaStatus.Stopped)
                    {
                        if (CompleteAndReOpen)
                        {
                            OpenFileFunc();
                            CompleteAndReOpen = false;
                        }
                        else
                        {
                            try
                            {
                                m_obj_MediaPosition.CurrentPosition = 0;
                                m_obj_MediaControl.Run();
                                m_CurrentStatus = MediaStatus.Running;
                            }
                            catch { }
                        }
                    }
                    else if (m_CurrentStatus == MediaStatus.Paused)
                    {
                        m_obj_MediaControl.Run();
                        m_CurrentStatus = MediaStatus.Running;
                    }
                    break;
                case 1:
                    m_obj_MediaControl.Pause();
                    m_CurrentStatus = MediaStatus.Paused;
                    break;
                case 2:
                    m_obj_MediaControl.Stop();
                    trackBar1.Value = 0;
                    m_obj_MediaPosition.CurrentPosition = 0;
                    m_CurrentStatus = MediaStatus.Stopped;
                    break;
                default:
                    MessageBox.Show("There is no file to play");
                    break;
            }
            UpdateToolStrip();
        }

        public void UpdateToolStrip()
        {
            switch (m_CurrentStatus)
            {
                case MediaStatus.None:
                    toolStripButton1.Enabled = false;
                    toolStripButton2.Enabled = false;
                    toolStripButton3.Enabled = false;
                    label2.Text = "00:00:00";
                    label3.Text = "00:00:00";
                    running = false;
                    break;
                case MediaStatus.Paused:
                    toolStripButton1.Enabled = true;
                    toolStripButton2.Enabled = false;
                    toolStripButton3.Enabled = true;
                    running = false;
                    break;
                case MediaStatus.Running: toolStripButton1.Enabled = false;
                    toolStripButton2.Enabled = true;
                    toolStripButton3.Enabled = true;
                    break;
                case MediaStatus.Stopped: toolStripButton1.Enabled = true;
                    toolStripButton2.Enabled = false;
                    toolStripButton3.Enabled = false;
                    label2.Text = "00:00:00";
                    label3.Text = "00:00:00";
                    running = false;
                    break;
            }
        }
        //Toolstrip sonu...
        #endregion

        #region timertick
        //Zamanla ilgili kýsýmlar...
        public int trackMax = 0;
        public void timer1_Tick(object sender, EventArgs e)
        {
            if (MediaStatus.Running == m_CurrentStatus)
            {
                trackBar1.Value = (int)m_obj_MediaPosition.CurrentPosition;
                fileSituation = trackBar1.Value;
                trackMax = trackBar1.Maximum;
                int s = (int)m_obj_MediaPosition.Duration;
                int h = s / 3600;
                int m = (s - (h * 3600)) / 60;
                s = s - (h * 3600 + m * 60);
                label2.Text = String.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);
                s = (int)m_obj_MediaPosition.CurrentPosition;
                h = s / 3600;
                m = (s - (h * 3600)) / 60;
                s = s - (h * 3600 + m * 60);
                label3.Text = String.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);
            }
        }
        #endregion

        #region trackbars scrool
        int fileSituation = 0;
        public void trackBar2_Scroll(object sender, EventArgs e)
        {
            trackBar1.Maximum = (int)m_obj_MediaPosition.Duration;
            m_obj_MediaPosition.CurrentPosition = trackBar1.Value;
            fileSituation = trackBar1.Value;
        }
        #endregion

        #region buttons
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Media Files|*.mp4;*.vob;*.flv;*.avi;*.mpg;*.mp3;*.wmv;*.wav*;*.wma";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                openedfile = opf.FileName;
                textBox1.Text = openedfile;
                textBox1.Enabled = false;
                ReLoad();
                timer1.Start();
                UpdateToolStrip();
                OpenFileFunc();
                button12.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
            startpos = m_obj_MediaPosition.CurrentPosition;
            button5.Enabled = false;
            button7.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            endpos = m_obj_MediaPosition.CurrentPosition;
            button6.Enabled = false;
            button7.Enabled = true;
            m_obj_MediaControl.Pause();
            m_CurrentStatus = MediaStatus.Paused;
            UpdateToolStrip();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            endpos = -1;
            startpos = -1;
            button1.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = false;
            if (m_CurrentStatus == MediaStatus.None)
            {
                OpenFileFunc();
            }

            if (m_CurrentStatus == MediaStatus.Stopped)
            {
                if (CompleteAndReOpen)
                {
                    OpenFileFunc();
                    CompleteAndReOpen = false;
                }
                else
                {
                    try
                    {
                        m_obj_MediaPosition.CurrentPosition = 0;
                        m_obj_MediaControl.Run();
                        m_CurrentStatus = MediaStatus.Running;
                    }
                    catch { }
                }
            }
            else if (m_CurrentStatus == MediaStatus.Paused)
            {
                m_obj_MediaControl.Run();
                m_CurrentStatus = MediaStatus.Running;
            }
            UpdateToolStrip();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                int itemscount = listBox2.Items.Count;
                int i = 0;
                for (i = 0; i < itemscount; i++)
                {
                    tw.WriteLine(Convert.ToString(i + 1));
                    tw.Write(listBox2.Items[i].ToString());
                    tw.Write(" --> ");
                    tw.Write(listBox3.Items[i].ToString());
                    tw.WriteLine();
                    string[] rows = listBox1.Items[i].ToString().Split('\0');
                    int j = 0;
                    for (j = 0; j < rows.Length; j++)
                    {
                        tw.WriteLine(rows[j]);
                    }
                    tw.WriteLine();
                    tw.Flush();
                }
                //tw.Close();
                File.Copy(standart + "\\subs\\" + filename + ".srt", sfd.FileName + ".srt");
                //File.Delete(standart + "\\subs\\" + filename + ".srt");
            }
        }
        TextWriter tw = null;
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() != "")
            {
                double s = startpos;
                int h = (int)(s / 3600);
                int m = (int)(s - (h * 3600)) / 60;
                s = s - (h * 3600 + m * 60);
                string start = "00:00:00";
                start = String.Format("{0:D2}:{1:D2}:{2:00.000}", h, m, s);
                start = start.Replace('.', ',');
                double se = endpos;
                int he = (int)se / 3600;
                int me = (int)(se - (he * 3600)) / 60;
                se = se - (he * 3600 + me * 60);
                string end = "00:00:00";
                end = String.Format("{0:D2}:{1:D2}:{2:00.000}", he, me, se);
                end = end.Replace('.', ',');
                listBox1.Items.Add(textBox2.Text);
                listBox2.Items.Add(start);
                listBox3.Items.Add(end);
                textBox2.Text = "";
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "")
            {
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                listBox3.Items.Clear();
                string[] filenametake = textBox1.Text.Split('\\');
                filename = filenametake[filenametake.Length - 1];
                filename = filename.Substring(0, filename.Length - 4);
                fsubstream = new FileStream(standart + "\\subs\\" + filename + ".srt", FileMode.Create);
                fsubstream.Close();
                tw = new StreamWriter(standart + "\\subs\\" + filename + ".srt", false, Encoding.UTF8);
                MessageBox.Show("Now you can start to write subtitles for " + filename);

                button12.Enabled = false;
                button1.Enabled = true;
                button2.Enabled = true;
                button13.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                panel5.Enabled = true;
                panel2.Enabled = true;
            }
            else
                MessageBox.Show("Select a video file");
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex > -1 && listBox2.Items.Count > listBox2.SelectedIndex)
            {

                int selected = listBox2.SelectedIndex;
                string newstart = listBox2.Items[listBox2.SelectedIndex].ToString();
                string[] time = newstart.Split(':');
                int h = Convert.ToInt32(time[0]);
                int m = Convert.ToInt32(time[1]);
                double s = Convert.ToDouble(time[2]);
                if (s - 0.1 >= 0)
                {
                    s -= 0.1;
                }
                else if (s - 0.1 < 0)
                {
                    if (m - 1 >= 0)
                    {
                        m -= 1;
                        s = 59.9;
                    }
                    else if (m - 1 < 0)
                    {
                        if (h - 1 >= 0)
                        {
                            h -= 1;
                            m = 59;
                            s = 59.9;
                        }
                        else if (h - 1 < 0)
                        {
                            h = 0;
                            m = 0;
                            s = 0;
                        }
                    }
                }
                string start = "00:00:00";
                s.ToString().Replace('.', ',');
                start = String.Format("{0:D2}:{1:D2}:{2:00.000}", h, m, s);
                listBox2.Items.Insert(selected, start);
                listBox2.Items.RemoveAt(selected + 1);
                listBox2.SelectedIndex = selected;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex > -1 && listBox2.Items.Count > listBox2.SelectedIndex)
            {

                int selected = listBox2.SelectedIndex;
                string newstart = listBox2.Items[listBox2.SelectedIndex].ToString();
                string[] time = newstart.Split(':');
                int h = Convert.ToInt32(time[0]);
                int m = Convert.ToInt32(time[1]);
                double s = Convert.ToDouble(time[2]);
                if (s + 0.1 < 60)
                {
                    s += 0.1;
                }
                else if (s + 0.1 >= 60)
                {
                    s = 0;
                    if (m + 1 < 60)
                    {
                        m += 1;
                    }
                    else if (m + 1 >= 60)
                    {
                        h += 1;
                        m = 0;
                    }
                }
                string start = "00:00:00";
                s.ToString().Replace('.', ',');
                start = String.Format("{0:D2}:{1:D2}:{2:00.000}", h, m, s);
                listBox2.Items.Insert(selected, start);
                listBox2.Items.RemoveAt(selected + 1);
                listBox2.SelectedIndex = selected;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex > -1 && listBox3.Items.Count > listBox3.SelectedIndex)
            {
                int selected = listBox3.SelectedIndex;
                string newstart = listBox3.Items[listBox3.SelectedIndex].ToString();
                string[] time = newstart.Split(':');
                int h = Convert.ToInt32(time[0]);
                int m = Convert.ToInt32(time[1]);
                double s = Convert.ToDouble(time[2]);
                if (s - 0.1 >= 0)
                {
                    s -= 0.1;
                }
                else if (s - 0.1 < 0)
                {
                    s = 0.9;
                    if (m - 1 >= 0)
                    {
                        m -= 1;
                        s = 59.9;
                    }
                    else if (m - 1 < 0)
                    {
                        if (h - 1 >= 0)
                        {
                            h -= 1;
                            m = 59;
                            s = 59.9;
                        }
                        else if (h - 1 < 0)
                        {
                            h = 0;
                            m = 0;
                            s = 0;
                        }
                    }
                }
                string start = "00:00:00";
                s.ToString().Replace('.', ',');
                start = String.Format("{0:D2}:{1:D2}:{2:00.000}", h, m, s);
                listBox3.Items.Insert(selected, start);
                listBox3.Items.RemoveAt(selected + 1);
                listBox3.SelectedIndex = selected;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex > -1 && listBox3.Items.Count > listBox3.SelectedIndex)
            {
                int selected = listBox3.SelectedIndex;
                string newstart = listBox3.Items[listBox3.SelectedIndex].ToString();
                string[] time = newstart.Split(':');
                int h = Convert.ToInt32(time[0]);
                int m = Convert.ToInt32(time[1]);
                double s = Convert.ToDouble(time[2]);
                if (s + 0.1 < 60)
                {
                    s += 0.1;
                }
                else if (s + 0.1 >= 60)
                {
                    s = 0;
                    if (m + 1 < 60)
                    {
                        m += 1;
                    }
                    else if (m + 1 >= 60)
                    {
                        h += 1;
                        m = 0;
                    }
                }
                string start = "00:00:00";
                s.ToString().Replace('.', ',');
                start = String.Format("{0:D2}:{1:D2}:{2:00.000}", h, m, s);
                listBox3.Items.Insert(selected, start);
                listBox3.Items.RemoveAt(selected + 1);
                listBox3.SelectedIndex = selected;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int itemscount = listBox2.Items.Count;
            int i = 0;
            for (i = 0; i < itemscount; i++)
            {
                tw.WriteLine(Convert.ToString(i + 1));
                tw.Write(listBox2.Items[i].ToString());
                tw.Write(" --> ");
                tw.Write(listBox3.Items[i].ToString());
                tw.WriteLine();
                string[] rows = listBox1.Items[i].ToString().Split('\0');
                int j = 0;
                for (j = 0; j < rows.Length; j++)
                {
                    tw.WriteLine(rows[j]);
                }
                tw.WriteLine();
                tw.Flush();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenSubFile = new OpenFileDialog();
            OpenSubFile.Filter = "Srt Files|*.srt;";
            if (OpenSubFile.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = OpenSubFile.FileName;
                textBox1.Enabled = false;
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                listBox3.Items.Clear();
                loadSubtitle(OpenSubFile.FileName);
                textBox3.Enabled = true;
                button2.Enabled = true;
                button13.Enabled = true;
                textBox4.Enabled = true;
                button11.Enabled = true;
                string[] filenametake = textBox1.Text.Split('\\');
                filename = filenametake[filenametake.Length - 1];
                filename = filename.Substring(0, filename.Length - 4);
                fsubstream = new FileStream(standart + "\\subs\\" + filename + ".srt", FileMode.Create);
                fsubstream.Close();
                tw = new StreamWriter(standart + "\\subs\\" + filename + ".srt", false, EncType(FontCharSet));
                kopyalist.Items.AddRange(listBox1.Items);
                MessageBox.Show("Now you can start to edit this Subtitle");
                panel5.Enabled = true;
                panel2.Enabled = true;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            bool notnumber = false;
            double ilkoran;
            double sonoran;

            if (textBox3.Text.Trim() != "" && textBox4.Text.Trim() != "")
            {
                try
                {
                    ilkoran = 1000 / Convert.ToDouble(textBox3.Text);
                    sonoran = 1000 / Convert.ToDouble(textBox4.Text);
                }
                catch
                {
                    notnumber = true;
                }

                if (!notnumber)
                {
                    ilkoran = 1000 / Convert.ToDouble(textBox3.Text);
                    sonoran = 1000 / Convert.ToDouble(textBox4.Text);
                    double ilkdeger = 0;
                    double sondeger = 0;
                    int i = 0;
                    for (i = 0; i < listBox2.Items.Count; i++)
                    {
                        string[] time = listBox2.Items[i].ToString().Split(':');
                        int h = Convert.ToInt32(time[0]);
                        int m = Convert.ToInt32(time[1]);
                        double s = Convert.ToDouble(time[2]);
                        ilkdeger = h * 3600 + m * 60 + s;
                        sondeger = (ilkdeger / ilkoran) * sonoran;
                        h = (int)sondeger / 3600;
                        m = ((int)sondeger - (h * 3600)) / 60;
                        sondeger = sondeger - (h * 3600 + m * 60);
                        sondeger.ToString().Replace('.', ',');
                        string start = "00:00:00";
                        start = String.Format("{0:D2}:{1:D2}:{2:00.000}", h, m, sondeger);
                        listBox2.Items.Insert(i, start);
                        listBox2.Items.RemoveAt(i + 1);
                    }
                    for (i = 0; i < listBox3.Items.Count; i++)
                    {
                        string[] time = listBox3.Items[i].ToString().Split(':');
                        int h = Convert.ToInt32(time[0]);
                        int m = Convert.ToInt32(time[1]);
                        double s = Convert.ToDouble(time[2]);
                        ilkdeger = h * 3600 + m * 60 + s;
                        sondeger = (ilkdeger / ilkoran) * sonoran;
                        h = (int)sondeger / 3600;
                        m = ((int)sondeger - (h * 3600)) / 60;
                        sondeger = sondeger - (h * 3600 + m * 60);
                        sondeger.ToString().Replace('.', ',');
                        string end = "00:00:00";
                        end = String.Format("{0:D2}:{1:D2}:{2:00.000}", h, m, sondeger);
                        listBox3.Items.Insert(i, end);
                        listBox3.Items.RemoveAt(i + 1);
                    }
                }
                else
                {
                    MessageBox.Show("Enter Number into textboxes!");
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < listBox1.Items.Count && listBox1.SelectedIndex > -1)
            {
                int selected = listBox1.SelectedIndex;
                listBox1.Items.Insert(selected, textBox2.Text);
                listBox1.Items.RemoveAt(selected + 1);
                int degisen = 0;
                int disstart = 0;
                if (kopyalist.Items.Count > listBox1.Items.Count)
                {
                    for (disstart = 0; disstart < listBox1.Items.Count; disstart++)
                    {
                        degisen = index[disstart];
                        for (start = 0; start < kopyalist.Items.Count; start++)
                        {
                            try
                            {
                                if (degisen == start)
                                {
                                    kopyalist.Items.Insert(degisen, listBox1.Items[disstart]);
                                    kopyalist.Items.RemoveAt(start + 1);
                                }
                            }
                            catch { }
                        }
                    }
                    listBox1.Items.Clear();
                    listBox1.Items.AddRange(kopyalist.Items);
                }
                kopyalist.Items.Clear();
                kopyalist.Items.AddRange(listBox1.Items);
            }
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            FontCharSet = 123;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            FontCharSet = 182;
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            FontCharSet = 100;
        }

        private void radioButton4_Click(object sender, EventArgs e)
        {
            FontCharSet = 200;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            double eklecikar;
            if (listBox2.Items.Count > 0 && listBox3.Items.Count > 0)
            {
                try
                {
                    eklecikar = Convert.ToDouble(textBox5.Text);
                    int i = 0;
                    for (i = 0; i < listBox2.Items.Count; i++)
                    {
                        string newstart = listBox2.Items[i].ToString();
                        string[] time = newstart.Split(':');
                        int h = Convert.ToInt32(time[0]);
                        int m = Convert.ToInt32(time[1]);
                        double s = Convert.ToDouble(time[2]);
                        eklecikar = Convert.ToDouble(textBox5.Text);
                        int eklesaat = (int)eklecikar / 3600;
                        int ekledakka = ((int)eklecikar - (eklesaat * 3600)) / 60;
                        double eklesaniye = eklecikar - (eklesaat * 3600 + ekledakka * 60);
                        if (s - eklesaniye >= 0)
                        {
                            s -= eklesaniye;
                        }
                        else if (s - eklesaniye < 0)
                        {
                            if (m - 1 >= 0)
                            {
                                m -= 1;
                                s = 60 - (eklesaniye - s);
                            }
                            else if (m - 1 < 0)
                            {
                                if (h - 1 >= 0)
                                {
                                    h -= 1;
                                    m = 59;
                                    s = 60 - (eklesaniye - s);
                                }
                                else if (h - 1 < 0)
                                {
                                    h = 0;
                                    m = 0;
                                    s = 0;
                                }
                            }
                        }
                        if (m - ekledakka >= 0)
                        {
                            m -= ekledakka;
                        }
                        else if (m - ekledakka < 0)
                        {
                            if (h - 1 >= 0)
                            {
                                h -= 1;
                                m = 60 - (ekledakka - m);
                            }
                            else if (h - 1 < 0)
                            {
                                h = 0;
                                m = 0;
                            }
                        }
                        if (h - eklesaat >= 0)
                        {
                            h -= eklesaat;
                        }
                        else if (h - eklesaat < 0)
                        {
                            h = 0;
                        }
                        string start = "00:00:00";
                        s.ToString().Replace('.', ',');
                        start = String.Format("{0:D2}:{1:D2}:{2:00.000}", h, m, s);
                        listBox2.Items.Insert(i, start);
                        listBox2.Items.RemoveAt(i + 1);

                        string newend = listBox3.Items[i].ToString();
                        string[] timeend = newend.Split(':');
                        int hs = Convert.ToInt32(timeend[0]);
                        int ms = Convert.ToInt32(timeend[1]);
                        double ss = Convert.ToDouble(timeend[2]);

                        if (ss - eklesaniye >= 0)
                        {
                            ss -= eklesaniye;
                        }
                        else if (ss - eklesaniye < 0)
                        {
                            if (ms - 1 >= 0)
                            {
                                ms -= 1;
                                ss = 60 - (eklesaniye - ss);
                            }
                            else if (ms - 1 < 0)
                            {
                                if (hs - 1 >= 0)
                                {
                                    hs -= 1;
                                    ms = 59;
                                    ss = 60 - (eklesaniye - ss);
                                }
                                else if (hs - 1 < 0)
                                {
                                    hs = 0;
                                    ms = 0;
                                    ss = 0;
                                }
                            }
                        }
                        if (ms - ekledakka >= 0)
                        {
                            ms -= ekledakka;
                        }
                        else if (ms - ekledakka < 0)
                        {
                            if (hs - 1 >= 0)
                            {
                                hs -= 1;
                                ms = 60 - (ekledakka - ms);
                            }
                            else if (hs - 1 < 0)
                            {
                                hs = 0;
                                ms = 0;
                            }
                        }
                        if (hs - eklesaat >= 0)
                        {
                            hs -= eklesaat;
                        }
                        else if (hs - eklesaat < 0)
                        {
                            hs = 0;
                        }
                        string end = "00:00:00";
                        s.ToString().Replace('.', ',');
                        end = String.Format("{0:D2}:{1:D2}:{2:00.000}", hs, ms, ss);
                        listBox3.Items.Insert(i, end);
                        listBox3.Items.RemoveAt(i + 1);
                    }
                }
                catch
                {
                    MessageBox.Show("Enter a number into textbox!");
                }
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            double eklecikar;
            int i = 0;
            try
            {
                eklecikar = Convert.ToDouble(textBox5.Text);
                for (i = 0; i < listBox2.Items.Count; i++)
                {
                    string newstart = listBox2.Items[i].ToString();
                    string[] time = newstart.Split(':');
                    int h = Convert.ToInt32(time[0]);
                    int m = Convert.ToInt32(time[1]);
                    double s = Convert.ToDouble(time[2]);
                    eklecikar = Convert.ToDouble(textBox5.Text);
                    int eklesaat = (int)eklecikar / 3600;
                    int ekledakka = ((int)eklecikar - (eklesaat * 3600)) / 60;
                    double eklesaniye = eklecikar - (eklesaat * 3600 + ekledakka * 60);
                    if (s + eklesaniye < 60)
                    {
                        s += eklesaniye;
                    }
                    else if (s + eklesaniye >= 60)
                    {
                        s = (s + eklesaniye) % 60;
                        if (m + 1 < 60)
                        {
                            m += 1;
                        }
                        else if (m + 1 > 60)
                        {
                            h += 1;
                        }
                    }

                    if (m + ekledakka < 60)
                    {
                        m += ekledakka;
                    }
                    else if (m + ekledakka >= 60)
                    {
                        m = (m + ekledakka) % 60;
                        h += 1;
                    }
                    h += eklesaat;
                    string start = "00:00:00";
                    s.ToString().Replace('.', ',');
                    start = String.Format("{0:D2}:{1:D2}:{2:00.000}", h, m, s);
                    listBox2.Items.Insert(i, start);
                    listBox2.Items.RemoveAt(i + 1);

                    string newend = listBox3.Items[i].ToString();
                    string[] timeend = newend.Split(':');
                    int hs = Convert.ToInt32(timeend[0]);
                    int ms = Convert.ToInt32(timeend[1]);
                    double ss = Convert.ToDouble(timeend[2]);

                    if (ss + eklesaniye < 60)
                    {
                        ss += eklesaniye;
                    }
                    else if (ss + eklesaniye >= 60)
                    {
                        ss = (ss + eklesaniye) % 60;
                        if (ms + 1 < 60)
                        {
                            ms += 1;
                        }
                        else if (ms + 1 > 60)
                        {
                            hs += 1;
                        }
                    }

                    if (ms + ekledakka < 60)
                    {
                        ms += ekledakka;
                    }
                    else if (ms + ekledakka >= 60)
                    {
                        ms = (ms + ekledakka) % 60;
                        hs += 1;
                    }
                    hs += eklesaat;
                    string end = "00:00:00";
                    s.ToString().Replace('.', ',');
                    end = String.Format("{0:D2}:{1:D2}:{2:00.000}", hs, ms, ss);
                    listBox3.Items.Insert(i, end);
                    listBox3.Items.RemoveAt(i + 1);
                }
            }
            catch
            {
                MessageBox.Show("Enter a number into textbox!");
            }
        }

        #endregion

        public void initializeform()
        {
            button1.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1 && listBox1.Items.Count > listBox1.SelectedIndex)
            {
                listBox2.SelectedIndex = listBox1.SelectedIndex;
                listBox3.SelectedIndex = listBox1.SelectedIndex;
                textBox2.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
                if (listBox1.Items.Count < kopyalist.Items.Count)
                {
                    listBox2.SelectedIndex = index[listBox1.SelectedIndex];
                    listBox3.SelectedIndex = index[listBox1.SelectedIndex];
                }
                button15.Enabled = true;
            }
        }

        string filename = "";

        public ArrayList arr = new ArrayList();

        public Encoding EncType(byte CharSet)
        {
            if (CharSet == (byte)204)
                return Encoding.GetEncoding("windows-1251");
            else if (CharSet == 182 || CharSet == 0)
                return Encoding.GetEncoding("iso-8859-9");
            else if (CharSet == 161)
                return Encoding.GetEncoding("ISO-8859-7");
            else if (CharSet == 178)
                return Encoding.GetEncoding("WINDOWS-1256");
            else if (CharSet == 123)
                return Encoding.GetEncoding("iso-8859-1");
            else if (CharSet == 100)
                return Encoding.UTF8;
            else if (CharSet == 200)
                return Encoding.Unicode;
            else
                return Encoding.GetEncoding("iso-8859-9");
        }

        public byte FontCharSet = 182;

        public void loadSubtitle(string pathOfSub)
        {
            arr.Clear();
            using (StreamReader sr = new StreamReader(pathOfSub, EncType(FontCharSet)))
            {
                String line;
                String str = "";
                line l = new line();
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Trim() == "")
                        continue;

                    if (line.Length < 5)
                    {
                        bool isnum = true;
                        for (int i = 0; i < line.Length; i++)
                        {
                            if (line[i] < '0' || line[i] > '9')
                            {
                                isnum = false;
                                break;
                            }
                        }

                        if (isnum)
                        {
                            continue;
                        }
                    }

                    int loc1 = 0;
                    if ((loc1 = line.IndexOf("-->")) > 0)
                    {
                        if (l != null)
                        {
                            if (str.Trim() != "")
                            {
                                listBox1.Items.Add(str);
                                str = "";
                            }
                        }

                        int loc = line.IndexOf(" ");
                        string s1 = line.Substring(0, loc);
                        string[] time = s1.Split(':');

                        int h = Convert.ToInt32(time[0]);
                        int m = Convert.ToInt32(time[1]);
                        double s = Convert.ToDouble(time[2]);

                        string start = "00:00:00";
                        s.ToString().Replace('.', ',');
                        start = String.Format("{0:D2}:{1:D2}:{2:00.000}", h, m, s);
                        listBox2.Items.Add(start);

                        string s2 = line.Substring(loc1 + 4, line.Length - (loc1 + 5));
                        string[] endtime = s2.Split(':');
                        int he = Convert.ToInt32(endtime[0]);
                        int me = Convert.ToInt32(endtime[1]);
                        double se = Convert.ToDouble(endtime[2]);
                        string end = "00:00:00";
                        se.ToString().Replace('.', ',');
                        end = String.Format("{0:D2}:{1:D2}:{2:00.000}", he, me, se);
                        listBox3.Items.Add(end);
                    }
                    else
                    {
                        if (str != "")
                            str += "\n";
                        str += line;
                    }
                }
                if (l != null)
                {
                    if (str.Trim() != "")
                    {
                        listBox1.Items.Add(str);
                        str = "";
                    }
                }
            }
            int k = 0;
            line subline = null;
            for (k = 0; k < arr.Count; k++)
            {
                subline = (line)arr[k];
                listBox1.Items.Add(subline.str);
                subline = null;
            }

        }

        public class line
        {
            public line() { }
            ~line() { }
            public string str;
            public TimeSpan tm1, tm2;
        };

        private void menu_Click(object sender, EventArgs e)
        {
            infoForm info = new infoForm();
            info.Show();
        }
        int sayi = 0;
        int[] index = new int[20000];
        int start = 0;
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            int sayi = 0;
            listBox1.Items.Clear();
            sayi = 0;
            for (start = 0; start < kopyalist.Items.Count; start++)
            {
                if (kopyalist.Items[start].ToString().Contains(textBox6.Text))
                {
                    listBox1.Items.Add(kopyalist.Items[start]);
                    index[sayi] = start;
                    sayi++;
                }
            }
        }
    }
}