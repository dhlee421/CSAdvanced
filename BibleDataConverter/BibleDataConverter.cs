using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace bibleDataConverter
{
    public partial class BibleDataConverter : Form
    {
        String currentPath = "";
        String currentSelFile = "";
        String outputFile = "";

        SQLiteConnection sqliteConn = null;
        SQLiteConnection sqlOutConn = null;
        public bool isConnDB { get; set; }

        public BibleDataConverter()
        {
            InitializeComponent();

            isConnDB = false;
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            /* 
            // Select Direcoty
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
            }*/

            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "db",
                Filter = "db files (*.db)|*.db|" + "All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textSelFile.Text = openFileDialog1.FileName;
                sqliteConn = CreateConnection(textSelFile.Text);
                currentSelFile = openFileDialog1.SafeFileName;
                currentPath = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);
                outputFile = System.IO.Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                outputFile = currentPath + "\\" + outputFile + ".db";
                sqlOutConn = CreateConnection(outputFile);
                
                CreateTable(sqlOutConn);
            }

        }

        SQLiteConnection CreateConnection(string fname)
        {
            SQLiteConnection sqlite_conn;

            // Create a new database connection:
            string conInfo = String.Format("Data Source={0}; Version = 3; New = True; Compress = True; ", fname);
            textSelFile.Text = conInfo;

            sqlite_conn = new SQLiteConnection(conInfo);
            // Open the connection:
            try
            {
                sqlite_conn.Open();
                isConnDB = true;
            }
            catch (Exception ex)
            {

            }

            return sqlite_conn;
        }


        void CloseConnection(SQLiteConnection conn)
        {
            conn.Close();
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if ( isConnDB)
                CloseConnection(sqliteConn);
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            //textSelFile.Text = currentPath;
            ConvertData(sqliteConn);
        }

        void CreateTable(SQLiteConnection conn)
        {

            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE bible(book NUMERIC, chapter NUMERIC, verse NUMERIC, content TEXT, PRIMARY KEY(book, chapter, verse))";

            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
        }

        void InsertData(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test Text ', 1); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test1 Text1 ', 2); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test2 Text2 ', 3); ";
            sqlite_cmd.ExecuteNonQuery();


            sqlite_cmd.CommandText = "INSERT INTO SampleTable1 (Col1, Col2) VALUES('Test3 Text3 ', 3); ";
            sqlite_cmd.ExecuteNonQuery();

        }

        void ConvertData(SQLiteConnection conn)
        {
            SQLiteDataReader sqlDataReader;
            SQLiteCommand sqlcmd;
            sqlcmd = conn.CreateCommand();
            sqlcmd.CommandText = "SELECT * FROM Bible";

            SQLiteCommand sqlwrcmd;
            sqlwrcmd = sqlOutConn.CreateCommand();

            sqlDataReader = sqlcmd.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string myreader = sqlDataReader["book"] + "," + sqlDataReader["chapter"] + "," + sqlDataReader["verse"] + "," + sqlDataReader["btext"];

                String data1 = sqlDataReader["btext"].ToString();
                string content = data1.Replace("'","''");


                string cmdfm = String.Format("INSERT INTO bible (book, chapter, verse, content) VALUES({0}, {1}, {2}, \'{3}\'); ", sqlDataReader["book"], sqlDataReader["chapter"], sqlDataReader["verse"], content);
                sqlwrcmd.CommandText = cmdfm;
                sqlwrcmd.ExecuteNonQuery();

                listBox1.Items.Add(myreader);
            }
        }
    }
}
