using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace renpy_savedata_backuper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //global variables
        string[] folders;
        string path;

        private void Form1_Load(object sender, EventArgs e)
        {

            //get renpy savedata path
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = Path.Combine(appDataPath, "RenPy");
            folders = Directory.GetDirectories(path);
            
            //add folders to listBox
            foreach (string folder in folders)
            {
                listBox1.Items.Add(Path.GetFileName(folder));
            }


            
        }

        //save backup
        private void saveButton_Click(object sender, EventArgs e)
        {

            if (listBox1.SelectedIndex != -1)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Zip files (*.zip)|*.zip";
                saveFileDialog.Title = "Save Backup File";
                saveFileDialog.FileName = listBox1.Text;
                saveFileDialog.FileOk += SaveFileDialog_FileOk;
                saveFileDialog.ShowDialog();
            }
            else {
                MessageBox.Show("ERROR: Select a Game to save it's data");
            }
            
            


        }

        //save backup when pressed OK
        private void SaveFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveFileDialog saveFileDialog = (SaveFileDialog)sender;
            string fileName = saveFileDialog.FileName;

            string folder = Path.Combine(path, listBox1.Text);
            
            
            try {
                ZipFile.CreateFromDirectory(folder, fileName);
            } catch (System.IO.IOException) {
                File.Delete(fileName);
                ZipFile.CreateFromDirectory(folder, fileName);
            }

            MessageBox.Show("Backup Saved Correctly");
        }



        //load backup game
        private void loadButton_Click(object sender, EventArgs e)
        {

            if (listBox1.SelectedIndex != -1)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Zip Files (*.zip)|*.zip";
                openFileDialog.Title = "Open Backup File";
                openFileDialog.ShowDialog();

                if (openFileDialog.FileName != "")
                {
                    string output_folder = Path.Combine(path, listBox1.Text);

                    try
                    {
                        ZipFile.ExtractToDirectory(openFileDialog.FileName, output_folder);
                    }
                    catch (System.IO.IOException)
                    {
                        string[] files = Directory.GetFiles(output_folder);
                        foreach (string file in files)
                        {
                            File.Delete(file);
                        }
                        ZipFile.ExtractToDirectory(openFileDialog.FileName, output_folder);
                    }

                    MessageBox.Show("Backup loaded correctly");
                }
            }
            else {

                MessageBox.Show("ERROR: Select a Game to load it's data");

            }
            

        }

       
    }
}
