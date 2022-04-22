using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Path = System.IO.Path;
using System.Data.SQLite;

namespace SearchFile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SQLiteConnection sqlconn = null;
        String currentPath = "";
        private int countFiles = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchDirectory()
        {
            var files = Directory.EnumerateFiles(currentPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".pdf") || s.EndsWith(".chm") || s.EndsWith(".epub") || s.EndsWith(".mobi"));

            try
            {
                foreach (string currentFile in files)
                {
                    string fileName = Path.GetFileName(currentFile);
                    string dirName = Path.GetDirectoryName(currentFile);
                    countFiles++;
                    //listBox.Items.Add(dirName + " [" + fileName + "]");
                    listFiles.Items.Add(currentFile);
                }
                String mText = String.Format("{0}", countFiles);
                textFileCounter.Text = mText;
            }
            catch (UnauthorizedAccessException UAEx)
            {
                Console.WriteLine(UAEx.Message);
            }

            //String msg = String.Format("{0}", cnt);
            //MessageBox.Show(msg);
        }

        private void BtSearchDirectory_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    currentPath = dialog.SelectedPath;
                    textDirectory.Text = currentPath;

                    if (ckClearList.IsChecked.Value)
                    {
                        listFiles.Items.Clear();
                        countFiles = 0;
                    }
                }
            }

            SearchDirectory();
        }

        private void BtSaveFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                string mainBuf = "";
                string pr = "";

                foreach (string o in listFiles.Items)
                {
                    pr = Path.GetPathRoot(o);
                    string fileName = Path.GetFileName(o);
                    string dirName = Path.GetDirectoryName(o);

                    mainBuf = mainBuf + dirName + '\t' + fileName + '\t' + System.Environment.NewLine;
                }

                File.WriteAllText(saveFileDialog.FileName, mainBuf);
            }
        }

        private void CreateTable ()
        {
            if (sqlconn != null)
            {
                string createQuery =
                    @"CREATE TABLE IF NOT EXISTS
                    [filelists] (
                    [id]           INTEGER      NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [filename]     VARCHAR(256) NOT NULL,
                    [directory]    VARCHAR(256) NOT NULL,
                    [desc]         VARCHAR(256),
                    [class1]       INTEGER,
                    [class2]       INTEGER,
                    [class3]       INTEGER
                    )";
                //string query = "create table filelists (filename varchar(255), directory varchar(255))";
                SQLiteCommand cmd = new SQLiteCommand(createQuery, sqlconn);
                int result = cmd.ExecuteNonQuery();
            }
            else
            {
                MessageBox.Show("DB is not connected", "Error");
            }
        }

        private void ConnectDB()
        {
            sqlconn = new SQLiteConnection("Data Source=fileList.sqlite;Version=3;");
            sqlconn.Open();
        }

        private void insertRecord(string fname, string dirname)
        {
            string query = String.Format("INSERT INTO filelists (filename, directory) VALUES (\"{0}\", \"{1}\")", fname, dirname);
            SQLiteCommand cmd = new SQLiteCommand(query, sqlconn);
            cmd.ExecuteNonQuery();
        }

        private void deleteRecord(int id)
        {
            string query = String.Format("DELETE FROM filelists WHERE ID ={0}", id);
          
            SQLiteCommand cmd = new SQLiteCommand(query, sqlconn);
            cmd.ExecuteNonQuery();
        }

        private void readRecord ()
        {
            string query = "SELECT * FROM filelists";

            SQLiteCommand cmd = new SQLiteCommand(query, sqlconn);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            string rdBuf;
            while (rdr.Read())
            {
                rdBuf = String.Format("{0}{1}", rdr["filename"], rdr["directory"]);

                listFiles.Items.Add(rdBuf);
            }


        }

        private void queryData()
        {
            String sql = "select * from members";

            SQLiteCommand cmd = new SQLiteCommand(sql, sqlconn);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                MessageBox.Show(rdr["name"] + " " + rdr["age"]);
            }

            rdr.Close();
        }

        private void BtConnectDB_Click(object sender, RoutedEventArgs e)
        {
            ConnectDB();
        }

        private void BtInsertDB_Click(object sender, RoutedEventArgs e)
        {
            if ( sqlconn==null)
            {
                MessageBox.Show("DB is not connected!", "Warning");
                return;
            }

            // string pr = "";

            foreach (string o in listFiles.Items)
            {
                // pr = Path.GetPathRoot(o);
                string fileName = Path.GetFileName(o);
                string dirName = Path.GetDirectoryName(o);

                insertRecord(fileName, dirName);
            }

        }

        private void BtCloseDB_Click(object sender, RoutedEventArgs e)
        {
            sqlconn.Close();
        }

        private void BtCreateDB_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection.CreateFile("filelist.sqlite");
        }

        private void BtCreateTable_Click(object sender, RoutedEventArgs e)
        {
            CreateTable();
        }

        private void BtMove_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
