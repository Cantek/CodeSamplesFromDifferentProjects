using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace CircassMediaToolSet
{
    public partial class FileFinder : Form
    {
        public FileFinder()
        {
            InitializeComponent();
            string[] strDrives = getDrives();
            AddComboBoxItem(strDrives);

        }

        private int nItems = 0;
        public void AddComboBoxItem(String[] strDrives)
        {
            // listBox1.InsertItem(nItems,oItem);
            for (int i = 0; i < strDrives.Length; i++)
            {
                comboBox1.Items.Add(strDrives[i]);
                ++nItems;
            }
        }
        public void AddListBoxItem()
        {
            listBox1.Items.Insert(0, comboBox1.Text);
        }

        // Event Handler for buttons for getting the folders.
        protected void button1_Click(object sender, System.EventArgs e)
        {
        }
        public String[] getDrives()
        {
            return Directory.GetLogicalDrives();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string strMessage = "No Matches";
            listBox1.Items.Clear();
            if ((textBox1.Text == "") && (comboBox1.Text == ""))
            {
                MessageBox.Show("Select the drive and Enter the file type to be searched!");
            }
            else if (textBox1.Text == "")
            {
                MessageBox.Show("Enter the file type to be searched!");
            }
            else if (comboBox1.Text == "")
            {
                MessageBox.Show("Select the drive to search the files!");
            }
            else
            {
                try
                {
                    int i = 0;
                    int iVal;
                    DirectoryInfo[] ChildDirs = this.getDirectories(comboBox1.Text);
                    //If the Search file type has no extention then add one.
                    iVal = textBox1.Text.IndexOf(".");
                    if (iVal == -1)
                    {
                        textBox1.Text += ".*";
                    }
                    //Get the Child Directories.
                    foreach (DirectoryInfo ChildDir1 in ChildDirs)
                    {
                        //recurse through the child directories.
                        while (ChildDir1.GetDirectories().Length > 0)
                        {
                            DirectoryInfo[] GrandChilds = ChildDir1.GetDirectories();
                            foreach (DirectoryInfo GrandChild in GrandChilds)
                            {
                                FileInfo[] Files = GrandChild.GetFiles(textBox1.Text);
                                if (Files.Length == 0)
                                {
                                    // Do nothing.
                                }
                                else
                                {
                                    foreach (FileInfo DirFile in Files)
                                    {
                                        listBox1.Items.Insert(i, DirFile.Name);
                                        i++;
                                    }
                                }
                                //ChildDir1 = GrandChild;
                            }
                        }
                    }
                }
                catch (IOException E)
                {
                    strMessage = E.Message;
                    listBox1.Items.Insert(0, strMessage);
                    strMessage = "";
                }
                if (listBox1.Items.Count == 0)
                {
                    listBox1.Items.Insert(0, strMessage);
                }
            }
        }
        public DirectoryInfo[] getDirectories(String strDrive)
        {
            DirectoryInfo dir = new DirectoryInfo(strDrive);
            DirectoryInfo[] childDirs = dir.GetDirectories();
            return childDirs;
        }

        //register the event handler.
    }
}
