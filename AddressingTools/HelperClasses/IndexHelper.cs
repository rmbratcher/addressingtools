using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
using Gajatko.IniFiles;
using System.IO;
using AddressingTools.Utils;

namespace AddressingTools
{
    public delegate void IndexingCompleteHandler(object sender, EventArgs e);
    public delegate void IndexingStartedHandler(object sender, EventArgs e);
    public delegate void FinishedItemHandler(object sender, EventArgs e);
    public delegate void HasMessageHandler(object sender, EventArgs e);


    public class DBIndexer
    {
        private SQLiteConnection conn = new SQLiteConnection();
        private IWorkspace fw = null;
        private List<string> inserts = new List<string>();
        private int main_count = 0;
        private int word_count = 0;
        private int word_index_count = 0;
        private int content_count = 0;
        private Dictionary<string,List<int>> rIndex;
        private Dictionary<string, int> cIndex;

        private Dictionary<string, ITable> Tables;

        private static string dllpath = System.Reflection.Assembly.GetAssembly(typeof(Globals)).Location;
        private static string directory = Path.GetDirectoryName(dllpath);
        //private static string inipath = Path.Combine(directory, "xindexes.ini");
        //private static IniFile xIni = IniFile.FromFile(inipath);

        string createSearchIndex = "CREATE TABLE \"SearchIndex\" (\"id\" INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL ,\"field\" TEXT, \"content\" TEXT, \"db\" INTEGER, \"oid\" INTEGER)";
        string createDBIndex = "CREATE TABLE [DBIndex] ([id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,[tablename] string NOT NULL UNIQUE ON CONFLICT REPLACE)";
        string createWords = "CREATE TABLE \"Words\" (\"wid\" INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL , \"word\" TEXT)";
        string createWordIndex = "CREATE TABLE \"WordIndex\" (\"id\" INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL , \"wid\" INTEGER, \"oid\" INTEGER NOT NULL )";
        private string dbname = "";

        private Dictionary<int, List<string>> insertVals = new Dictionary<int, List<string>>();
        private int step = 1000;

        private agdErrorLogger log;

        private int max_results = 500;//Convert.ToInt32(pSection2["MaxResults"].ToString());

        public int TotalClasses { get; set; }
        public string Message {get;set;}


        public event IndexingCompleteHandler IndexingComplete;
        public event IndexingStartedHandler IndexingStarted;
        public event FinishedItemHandler FinishedItem;
        public event HasMessageHandler HasMessage;


        protected virtual void OnHasMessage(EventArgs e)
        {
            if(HasMessage != null)
            {
                HasMessage(this,e);
            }
        }

        protected virtual void OnFinishedItem(EventArgs e)
        {
            if (FinishedItem != null)
            {
                if (TotalClasses > 0)
                    TotalClasses -= 1;
                FinishedItem(this, e);
            }
        }

        protected virtual void OnIndexingStarted(EventArgs e)
        {
            if (IndexingStarted != null)
                IndexingStarted(this, e);
        }

        protected virtual void OnIndexingComplete(EventArgs e)
        {
            if (IndexingComplete != null)
                IndexingComplete(this, e);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="wrkspc">IFeatureWorkspace</param>
        public DBIndexer(IWorkspace wrkspc)
        {
            dbname = "AddressingTools.db";
            string con_str = "Data Source=" + directory + "\\" + dbname + "; Version=3";
            conn.ConnectionString = con_str;

            if (wrkspc != null)
                fw = wrkspc;
            try
            {
                conn.Open();
                CheckTables();
                //conn.Close();
            }
            catch (Exception ex)
            {
                log.WriteError(ex, "AddressingTools", System.Security.Principal.WindowsIdentity.GetCurrent().Name, null);
                IndexError(ex.Message, "DBIndexer Construction Error");
                //MessageBox.Show(ex.Message, "DBIndexer Constructor Error");
            }
        }

        private void IndexError(string p, string p_2)
        {
            //MobileAVL.SplashScreen.Splasher.Status = p + "  " + p_2;
            //MessageBox.Show(p, p_2);
        }

        private void CheckTables()
        {
            try
            {
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM SearchIndex WHERE id > -99999 LIMIT 1", conn);
                SQLiteDataReader sr = cmd.ExecuteReader();
            }
            catch (SQLiteException ex)
            {
                SQLiteCommand cmd = new SQLiteCommand(createSearchIndex, conn);
                cmd.ExecuteNonQuery();
            }
            try
            {
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM DBIndex WHERE id > -99999 LIMIT 1", conn);
                SQLiteDataReader sr = cmd.ExecuteReader();
            }
            catch (SQLiteException ex)
            {
                //IndexError(ex.Message, "CheckTables Errors");
                SQLiteCommand cmd = new SQLiteCommand(createDBIndex, conn);
                cmd.ExecuteNonQuery();
            }
            try
            {
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Words WHERE wid > -99999 LIMIT 1", conn);
                SQLiteDataReader sr = cmd.ExecuteReader();
            }
            catch (SQLiteException ex)
            {
                SQLiteCommand cmd = new SQLiteCommand(createWords, conn);
                cmd.ExecuteNonQuery();
            }
            try
            {
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM WordIndex WHERE id > -99999 LIMIT 1", conn);
                SQLiteDataReader sr = cmd.ExecuteReader();
            }
            catch (SQLiteException ex)
            {
                SQLiteCommand cmd = new SQLiteCommand(createWordIndex, conn);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// The IFeatureWorkspace for the Indexed Items
        /// </summary>
        public IWorkspace Workspc
        {
            get { return fw; }
            set { fw = value; }
        }
        /// <summary>
        /// Search Index for String
        /// </summary>
        /// <param name="s"></param>
        /// <returns>SearchResult</returns>
        public SearchResult Search(string s)
        {
            //conn.Open();
            SearchResult xResults = new SearchResult();

            try
            {
                string cmdText = String.Format("SELECT content,db,oid FROM SearchIndex WHERE content GLOB '*{0}*' LIMIT {1}", s.ToUpper(), max_results.ToString());
                SQLiteCommand cmd = new SQLiteCommand(cmdText, conn);
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DBIndexItem dbi = new DBIndexItem(reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2));
                    xResults.AddItem(dbi);
                }
            }
            catch (Exception ex)
            {
                //IndexError(ex.Message, "Search Error");
                System.Diagnostics.EventLog.WriteEntry("AdressingTools", ex.Message + " @ " + ex.TargetSite, System.Diagnostics.EventLogEntryType.Error);
            }
            //conn.Close();
            return xResults;
        }
        /// <summary>
        /// Search Index for String
        /// </summary>
        /// <param name="s">Search String</param>
        /// <returns>AutoCompleteStringCollection</returns>
        public List<string> BuildAutoCompSrc(string s)
        {

            List<string> asc = new List<string>();
            try
            {
                string sndx = Soundex.EncodeString(s,4);
                foreach (string t in sndx.Split('-'))
                {
                    string sql = String.Format("SELECT content FROM SearchIndex WHERE id IN (SELECT oid FROM WordIndex WHERE wid IN (SELECT wid FROM Words WHERE word GLOB '{0}*' LIMIT 20)) LIMIT 20", t);
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string g = reader.GetString(0);
                        asc.Add(g);
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, "AddressingTools", System.Security.Principal.WindowsIdentity.GetCurrent().Name, null);
                //IndexError(ex.Message, "BuildAutoCompSrc Error");
                //MessageBox.Show(ex.Message, "BuildAutoCompSrc Error");
                //System.Diagnostics.EventLog.WriteEntry("AtlasDx", ex.Message + " @ " + ex.TargetSite, System.Diagnostics.EventLogEntryType.Error);
            }
            //conn.Close();
            return asc;
        }
        /// <summary>
        /// TO DO
        /// </summary>
        public void SyncTables()
        {
            if (1 == 1)
            {
                try
                {
                    //conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand("DELETE FROM SearchIndex WHERE id > -99999", conn);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM Words WHERE wid > -99999";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM WordIndex WHERE id > -99999";
                    cmd.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    log.WriteError(ex, "AddressingTools", System.Security.Principal.WindowsIdentity.GetCurrent().Name, null);
                    IndexError(ex.Message, "SyncTables Error");
                    //conn.Close();
                }
            }

            try
            {

                Tables = new Dictionary<string, ITable>();

                IEnumDataset featureDataSets = Workspc.get_Datasets(esriDatasetType.esriDTAny);

                IDataset featureDataSet = featureDataSets.Next();
                while (featureDataSet != null)
                {
                    if (featureDataSet.Type == esriDatasetType.esriDTFeatureDataset)
                    {
                        IEnumDataset fcs = featureDataSet.Subsets;
                        IDataset featc = fcs.Next();
                        while (featc != null)
                        {
                            if (featc.Type == esriDatasetType.esriDTFeatureClass)
                            {
                                ITable _fc = (ITable)featc;
                                Tables.Add(featc.Name,_fc);
                            }
                            if (featc.Type == esriDatasetType.esriDTTable)
                            {
                                ITable _tbl = (ITable)featc;
                                Tables.Add(featc.Name, _tbl);
                            }
                            featc = fcs.Next();
                        }
                    }
                    else
                    {
                        if (featureDataSet.Type == esriDatasetType.esriDTFeatureClass)
                        {
                            ITable _fc = (ITable)featureDataSet;
                            Tables.Add(featureDataSet.Name,_fc);
                        }
                        if (featureDataSet.Type == esriDatasetType.esriDTTable)
                        {
                            ITable _tbl = (ITable)featureDataSet;
                            Tables.Add(featureDataSet.Name, _tbl);
                        }
                    }

                    featureDataSet = featureDataSets.Next();
                }

                //IniFileSection xSection = xIni["Tables"];
                //System.Collections.ObjectModel.ReadOnlyCollection<string> Tables = xSection.GetKeys();

                TotalClasses = Tables.Count;
                OnIndexingStarted(new EventArgs());

                foreach (KeyValuePair<string, ITable> kvp in Tables)
                //foreach (string s in Tables)
                {
                    List<string>flds = new List<string>();

                    IFields Fds = kvp.Value.Fields;

                    for (int i = 0;i < Fds.FieldCount;i++)
                    {
                        IField field = Fds.get_Field(i);
                        if (field.Type != esriFieldType.esriFieldTypeGlobalID && field.Type != esriFieldType.esriFieldTypeBlob && field.Type != esriFieldType.esriFieldTypeGeometry && field.Type != esriFieldType.esriFieldTypeGUID && field.Type != esriFieldType.esriFieldTypeOID)
                        {
                            flds.Add(field.Name);
                        }
                    }

                    rIndex = new Dictionary<string,List<int>>();

                    IndexTableMulti(kvp.Key, kvp.Value, flds.ToArray());
                    Insert_Items();
                }

                WriteWordIndexes();
                
                OnIndexingComplete(new EventArgs());
            }
            catch (Exception ex2)
            {
                log.WriteError(ex2, "AddressingTools", System.Security.Principal.WindowsIdentity.GetCurrent().Name, null);
                IndexError(ex2.Message, "SyncTables Error");
                //System.Diagnostics.EventLog.WriteEntry("AtlasDx", ex2.Message + " @ " + ex2.TargetSite, System.Diagnostics.EventLogEntryType.Error);
            }
        }
        /// <summary>
        /// Load the list of indexed Items into the database
        /// </summary>
        private void Insert_Items()
        {
            try
            {
                //conn.Open();
                //MobileAVL.SplashScreen.Splasher.Status = "Indexed " + count.ToString() + " items";
                using (SQLiteTransaction trans = conn.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        SQLiteParameter param1 = new SQLiteParameter();
                        SQLiteParameter param2 = new SQLiteParameter();
                        SQLiteParameter param3 = new SQLiteParameter();
                        SQLiteParameter param4 = new SQLiteParameter();
                        SQLiteParameter param5 = new SQLiteParameter();

                        cmd.CommandText = "INSERT INTO SearchIndex (id,field,content,db,oid) VALUES(?,?,?,?,?)";
                        cmd.Parameters.Add(param1);
                        cmd.Parameters.Add(param2);
                        cmd.Parameters.Add(param3);
                        cmd.Parameters.Add(param4);
                        cmd.Parameters.Add(param5);

                        foreach (KeyValuePair<int, List<string>> kvp in insertVals)
                        {
                            param1.Value = kvp.Value[0];
                            param2.Value = kvp.Value[1];
                            param3.Value = kvp.Value[2];
                            param4.Value = kvp.Value[3];
                            param5.Value = kvp.Value[4];
                            cmd.ExecuteNonQuery();
                        }
                    }
                    trans.Commit();
                } 
                //conn.Close();
            }
            catch (Exception ex)
            {
                log.WriteError(ex, "AddressingTools", System.Security.Principal.WindowsIdentity.GetCurrent().Name, null);
                IndexError(ex.Message, "Insert_Items Error");
                //System.Diagnostics.EventLog.WriteEntry("AtlasDx", ex.Message + " @ " + ex.TargetSite, System.Diagnostics.EventLogEntryType.Error);
                //conn.Close();
            }

            insertVals.Clear();
        }

        private void WriteWordIndexes()
        {
            using (SQLiteTransaction trans = conn.BeginTransaction())
            {
                SQLiteParameter param1 = new SQLiteParameter();
                SQLiteParameter param2 = new SQLiteParameter();
                SQLiteParameter param3 = new SQLiteParameter();
                SQLiteParameter param4 = new SQLiteParameter();
                SQLiteParameter param5 = new SQLiteParameter();


                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {

                    cmd.CommandText = "INSERT INTO Words (wid,word) VALUES(?,?)";
                    cmd.Parameters.Add(param1);
                    cmd.Parameters.Add(param2);

                    foreach (KeyValuePair<string, List<int>> kvp in rIndex)
                    {
                        param1.Value = word_count;
                        param2.Value = kvp.Key;
                        cmd.ExecuteNonQuery();

                        using (SQLiteCommand cmd2 = new SQLiteCommand(conn))
                        {
                            cmd2.CommandText = "INSERT INTO WordIndex(id,wid,oid) VALUES(?,?,?)";
                            cmd2.Parameters.Add(param3);
                            cmd2.Parameters.Add(param4);
                            cmd2.Parameters.Add(param5);

                            foreach (int i in kvp.Value)
                            {
                                param3.Value = word_index_count;
                                param4.Value = word_count;
                                param5.Value = i;
                                cmd2.ExecuteNonQuery();
                                word_index_count++;
                            }
                            word_count++;
                        }


                    }
                }

                trans.Commit();
            }

            rIndex.Clear();
        }

        /// <summary>
        /// Indexes Concantinates multiple fields from an ESRI ITable into a single search field
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="tbl"></param>
        /// <param name="fields2"></param>
        private void IndexTableMulti(string Name, ITable tbl, string[] fields2)
        {
            try
            {
                string status = "Indexing " + Name + " ... ";
                Message = status;
                OnHasMessage(new EventArgs());

                int i = SetDBIndex(Name);
                string dbID = i.ToString();
                ICursor xCursor = tbl.Search(null, true);
                int[] flds = new int[fields2.Length];

                Dictionary<int,string> fld_Ids = new Dictionary<int,string>();

                for (int x = 0; x < fields2.Length; x++)
                {
                    fld_Ids.Add(xCursor.Fields.FindField(fields2[x]),fields2[x]);
                }
                IRow row = xCursor.NextRow();
                while (row != null)
                {
                    try
                    {
                        string oid = row.OID.ToString();
                        foreach(KeyValuePair<int,string> Kvp in fld_Ids)
                        {
                            string xval = row.get_Value(Kvp.Key).ToString().Trim();

                            if (!String.IsNullOrEmpty(xval))
                            {
                                string sval = Soundex.EncodeString(xval);
                                string[] spl = sval.Split('-');
                                foreach(string s in spl)
                                {
                                    if(rIndex.ContainsKey(s))
                                    {
                                        rIndex[s].Add(main_count);
                                    }
                                    else
                                    {
                                        rIndex[s] = new List<int>();
                                        rIndex[s].Add(main_count);
                                    }
                                }


                                List<string> inserts = new List<string>();
                                inserts.Add(main_count.ToString());
                                inserts.Add(Kvp.Value);
                                inserts.Add(xval);
                                inserts.Add(dbID);
                                inserts.Add(oid);
                                insertVals.Add(main_count, inserts);
                            }

                            main_count += 1;
                        }

                        row = xCursor.NextRow();
                        
                        //if (count % 1500 == 0)
                        //{
                        //    Message = status + count.ToString();
                        //    OnHasMessage(new EventArgs());
                        //}
                                //MobileAVL.SplashScreen.Splasher.Status = status + count.ToString();
                    }
                    catch (SQLiteException ex)
                    {
                        IndexError(ex.Message, "Error Indexing " + Name);
                        //System.Diagnostics.EventLog.WriteEntry("AtlasDx", ex.Message + " @ " + ex.TargetSite, System.Diagnostics.EventLogEntryType.Error);
                    }
                }

            }
            catch (Exception ex2)
            {
                IndexError(ex2.Message, "IndexTable Error");
            }

            Message = "Indexed " + main_count.ToString() + " items";
            OnHasMessage(new EventArgs());
        }

        private int SetDBIndex(string Name)
        {
            int x = GetDBIndex(Name);
            if (x < 0)
            {
                //conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("INSERT INTO DBIndex (tablename) VALUES ('" + Name + "')", conn);
                int h = cmd.ExecuteNonQuery();
                int i = GetDBIndex(Name);
                //conn.Close();
                return i;
            }
            else
                return x;

        }
        /// <summary>
        /// Get the ID of the Indexed table from the Index Database by the table name
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>int ID</returns>
        private int GetDBIndex(string Name)
        {
            //conn.Open();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM DBIndex WHERE tablename='" + Name + "'", conn);
            int x = cmd.ExecuteNonQuery();
            try
            {
                SQLiteDataReader reader = cmd.ExecuteReader();
                int r = -1;
                while (reader.Read())
                {
                    r = reader.GetInt32(0);
                }
                //conn.Close();
                return r;
            }
            catch (Exception)
            {
                //conn.Close();
                return -1;
            }
        }
        /// <summary>
        /// Get the name of an indexed table from the index database by ID
        /// </summary>
        /// <param name="i"></param>
        /// <returns>string TableName</returns>
        public string GetFeatureClassName(int i)
        {
            //conn.Open();
            SQLiteCommand cmd = new SQLiteCommand("SELECT tablename FROM DBIndex WHERE id=" + i.ToString() + " LIMIT 1", conn);
            SQLiteDataReader reader = cmd.ExecuteReader();
            string name = "";
            while (reader.Read())
            {
                name = reader.GetString(0);
            }

            //conn.Close();
            return name;
        }

    }

    public class SearchResult : List<DBIndexItem>
    {
        public bool AddItem(DBIndexItem di)
        {
            if (Contains(di) == false)
                Add(di);
            return Contains(di);
        }
    }
    public class DBIndexItem
    {
        private string searchstring;
        private int dbindex;
        private int oid;

        public DBIndexItem(string s, int i, int o)
        {
            searchstring = s;
            dbindex = i;
            oid = o;
        }

        public string SearchString
        {
            get { return searchstring; }
            set { searchstring = value; }
        }
        public int DBIndex
        {
            get { return dbindex; }
            set { dbindex = value; }
        }

        public int OID
        {
            get { return oid; }
            set { oid = value; }
        }
    }
}
