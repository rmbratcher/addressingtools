using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net;
using ESRI.ArcGIS.Geodatabase;
using System.IO;
using Gajatko.IniFiles;
using Lucene.Net.Store;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Documents;

namespace AddressingTools
{
    public delegate void IndexingCompleteHandler(object sender, EventArgs e);
    public delegate void IndexingStartedHandler(object sender, EventArgs e);
    public delegate void FinishedItemHandler(object sender, EventArgs e);

    public class LuceneIndexHelper
    {
        private List<IFeatureClass> FeatureClasses;
        private Dictionary<string, ITable> Tables;

        private static string dllpath = System.Reflection.Assembly.GetAssembly(typeof(Globals)).Location;
        private static string directory = Path.GetDirectoryName(dllpath);
        private static string inipath = Path.Combine(directory, "xindexes.ini");
        private static IniFile xIni = IniFile.FromFile(inipath);

        private Lucene.Net.Store.Directory StoreDirectory;
        private string DirectoryPath;
        private Analyzer analyzer;
        private IndexWriter writer;

        public int TotalClasses = 0;

        public event IndexingCompleteHandler IndexingComplete;
        public event IndexingStartedHandler IndexingStarted;
        public event FinishedItemHandler FinishedItem;


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

        public Lucene.Net.Store.Directory getDirectory(IWorkspace wrkspc)
        {
            string key = "";
            if (wrkspc.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
            {
                object oname;
                object oval;
                wrkspc.ConnectionProperties.GetAllProperties(out oname, out oval);

                string[] names = (string[])oname;
                object[] vals = (object[])oval;
                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i].ToUpper() == "DATABASE")
                    {
                        key = vals[i].ToString();
                    }
                }

            }
            else
            {
                key = wrkspc.PathName;
            }

            return getStore(key);
        }

        public void BuildIndex(IWorkspace wrkspc)
        {
            try
            {
                string key = "";
                if (wrkspc.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
                {
                    object oname;
                    object oval;
                    wrkspc.ConnectionProperties.GetAllProperties(out oname, out oval);

                    string[] names = (string[])oname;
                    object[] vals = (object[])oval;
                    for (int i = 0; i < names.Length; i++)
                    {
                        if (names[i].ToUpper() == "DATABASE")
                        {
                            key = vals[i].ToString();
                        }
                    }

                }
                else
                {
                    key = wrkspc.PathName;
                }

                setStore(key);


                FeatureClasses = new List<IFeatureClass>();
                Tables = new Dictionary<string, ITable>();

                IEnumDataset featureDataSets = wrkspc.get_Datasets(esriDatasetType.esriDTAny);

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
                                IFeatureClass _fc = (IFeatureClass)featc;
                                FeatureClasses.Add(_fc);
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
                            IFeatureClass _fc = (IFeatureClass)featureDataSet;
                            FeatureClasses.Add(_fc);
                        }
                        if (featureDataSet.Type == esriDatasetType.esriDTTable)
                        {
                            ITable _tbl = (ITable)featureDataSet;
                            Tables.Add(featureDataSet.Name, _tbl);
                        }
                    }

                    featureDataSet = featureDataSets.Next();
                }

                

                TotalClasses = (FeatureClasses.Count + Tables.Count);
                OnIndexingStarted(new EventArgs());

                foreach (IFeatureClass fc in FeatureClasses)
                {
                    IndexFeatureClass(fc);
                    OnFinishedItem(new EventArgs());
                }
                foreach (KeyValuePair<string, ITable> kvp in Tables)
                {
                    IndexTable(kvp.Key, kvp.Value);
                    OnFinishedItem(new EventArgs());
                }

                writer.Optimize();
                writer.Commit();

                writer.Close();
            }
            catch (Exception ex)
            {
            }

            OnIndexingComplete(new EventArgs());
        }


        private void setStore(string key)
        {
            StoreDirectory = getStore(key);
        }

        private Lucene.Net.Store.Directory getStore(string key)
        {
            IniFileSection store = xIni["STOREDIRECTORY"];
            if (store.elements.Count == 0)
            {
                string s = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                DirectoryPath = System.IO.Path.Combine(s, "AGD");
                if (!System.IO.Directory.Exists(DirectoryPath))
                {
                    System.IO.Directory.CreateDirectory(DirectoryPath);
                }
            }
            else
            {
                string s = store["PATH"];
                DirectoryPath = System.IO.Path.Combine(s, "AGD");
                if (!System.IO.Directory.Exists(DirectoryPath))
                {
                    System.IO.Directory.CreateDirectory(DirectoryPath);
                }
            }

            IniFileSection indexes = xIni["INDEXES"];
            System.Collections.ObjectModel.ReadOnlyCollection<string> keys = indexes.GetKeys();
            bool found = false;
            string val = "";
            foreach (string k in keys)
            {
                if (key == k)
                {
                    found = true;
                    val = indexes[k];
                }
            }

            if (found)
            {
                DirectoryPath = System.IO.Path.Combine(DirectoryPath, val);
                if (!System.IO.Directory.Exists(DirectoryPath))
                {
                    System.IO.Directory.CreateDirectory(DirectoryPath);
                }
            }
            else
            {
                string g = System.Guid.NewGuid().ToString("N");
                indexes[key] = g;
                xIni.Save(inipath);
                DirectoryPath = System.IO.Path.Combine(DirectoryPath, g);
                if (!System.IO.Directory.Exists(DirectoryPath))
                {
                    System.IO.Directory.CreateDirectory(DirectoryPath);
                }
            }

            analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
            System.IO.DirectoryInfo di = new DirectoryInfo(DirectoryPath);
            Lucene.Net.Store.Directory d = new SimpleFSDirectory(di);
            writer = new IndexWriter(d, analyzer, true, new IndexWriter.MaxFieldLength(255));
            return writer.GetDirectory();
        }

        private void IndexFeatureClass(IFeatureClass fc)
        {


            IFeatureCursor c = fc.Search(null, false);

            IFeature feat = c.NextFeature();

            while (feat != null)
            {
                IFields fields = feat.Fields;
                string id = feat.OID.ToString();
                string content = "";
                for (int i = 0; i < fields.FieldCount; i++)
                {


                    IField field = fields.get_Field(i);
                    if (field.Type != esriFieldType.esriFieldTypeGlobalID && field.Type != esriFieldType.esriFieldTypeBlob && field.Type != esriFieldType.esriFieldTypeGeometry && field.Type != esriFieldType.esriFieldTypeGUID && field.Type != esriFieldType.esriFieldTypeOID)
                    {
                        if (field.Name.IndexOf('.') <= 0)
                        {
                            Document doc = new Document();
                            doc.Add(new Lucene.Net.Documents.Field("oid", id, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NO));
                            doc.Add(new Lucene.Net.Documents.Field("fc", fc.AliasName, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NO));
                            doc.Add(new Lucene.Net.Documents.Field("fld", field.Name, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NO));
                            doc.Add(new Lucene.Net.Documents.Field("typ", "featureclass", Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NO));
                            doc.Add(new Lucene.Net.Documents.Field("content", content, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.ANALYZED));
                            writer.AddDocument(doc);
                        }
                    }
                }

                feat = c.NextFeature();
            }
        }


        private void IndexTable(string name, ITable tbl)
        {

            ICursor c = tbl.Search(null, false);

            IRow feat = c.NextRow();

            while (feat != null)
            {
                IFields fields = feat.Fields;
                string id = feat.OID.ToString();
                string content = "";
                for (int i = 0; i < fields.FieldCount; i++)
                {


                    IField field = fields.get_Field(i);
                    if (field.Type != esriFieldType.esriFieldTypeGlobalID && field.Type != esriFieldType.esriFieldTypeBlob && field.Type != esriFieldType.esriFieldTypeGeometry && field.Type != esriFieldType.esriFieldTypeGUID && field.Type != esriFieldType.esriFieldTypeOID)
                    {
                        if (field.Name.IndexOf('.') <= 0)
                        {
                            Document doc = new Document();
                            doc.Add(new Lucene.Net.Documents.Field("oid", id, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NO));
                            doc.Add(new Lucene.Net.Documents.Field("fc", name, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NO));
                            doc.Add(new Lucene.Net.Documents.Field("typ", "table", Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NO));
                            doc.Add(new Lucene.Net.Documents.Field("fld", field.Name, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NO));
                            doc.Add(new Lucene.Net.Documents.Field("content", content, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.ANALYZED));
                            writer.AddDocument(doc);
                        }
                    }
                }

                feat = c.NextRow();

            }
        }


    }
}