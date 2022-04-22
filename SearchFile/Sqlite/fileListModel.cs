using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchFile.Sqlite
{
    public class fileListModel
    {
        #region Constructors
        public fileListModel (
            int id,
            string filename,
            string directory,
            string author,
            string company,
            string desc,
            int class1,
            int class2,
            int class3
            ) : this()
        {
            ID = id;
            FILENAME = filename;
            DIRNAME = directory;
            DESCRIPTION = desc;
            AUTHOR = author;
            COMPANY = company;
            C1 = class1;
            C2 = class2;
            C3 = class3;
        }
        public fileListModel()
        {
        }
        #endregion Constructors

        #region properties
        public int ID { get; set; }
        public string FILENAME { get; set; }
        public string DIRNAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string AUTHOR { get; set; }
        public string COMPANY { get; set; }
        public int C1 { get; set; }
        public int C2 { get; set; }
        public int C3 { get; set; }
        #endregion properties
    }
}
