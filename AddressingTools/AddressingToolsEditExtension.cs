using System;
using System.Security.Principal;
using AddressingTools.Utils;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Editor;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;

namespace AddressingTools
{
    /// <summary>
    /// AddressingToolsEditExtension class implementing custom ESRI Editor Extension functionalities.
    /// </summary>
    /// 
    public class AddressingToolsEditExtension : ESRI.ArcGIS.Desktop.AddIns.Extension
    {
        private IEditor theEditor;
        private ITable SettingsTable = null;
        private agdErrorLogger log;
        private const string AddressingSettingsGUID = "{7A566981-C114-11D2-8A28-006097AFF44E}";
        private bool UseExtension = true;
        private int LastOID = 1;
        private string SettingsTableName = "AddressingSettings";

        public AddressingToolsEditExtension()
        {
            Globals.IsReady = true;
            log = new agdErrorLogger("errors.agd.cc");
        }

        protected override void OnStartup()
        {
            theEditor = ArcMap.Editor;
            WireEditorEvents();
        }

        private ITable getSettingsTable(IFeatureWorkspace fwspc)
        {
            ITable tbl = null;
            try
            {
                Workspace mb_ws = (Workspace)fwspc;
                IEnumDatasetName names = mb_ws.get_DatasetNames(esriDatasetType.esriDTTable);
                names.Reset();

                IDatasetName name = names.Next();
                while (name != null)
                {
                    if (name.Name.Contains("AddressingSettings"))
                    {
                        SettingsTableName = name.Name;
                    }
                    name = names.Next();
                }

                tbl = fwspc.OpenTable(SettingsTableName);
            }
            catch(Exception ex)
            {
                tbl = null;
            }

            return tbl;
        }

        private void CreateSettingsTable(IFeatureWorkspace fwspc)
        {
            if (MessageBox.Show(String.Format("There is no settings table in this workspace.{0}Would you like to create one?", Environment.NewLine), "Settings Table", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    IFields flds = new FieldsClass();
                    IFieldsEdit fldsEdit = (IFieldsEdit)flds;
                    fldsEdit.FieldCount_2 = 3;

                    IField oidfld = new FieldClass();
                    IFieldEdit oidfldEdit = (IFieldEdit)oidfld;
                    oidfldEdit.Name_2 = "OBJECTID";
                    oidfldEdit.AliasName_2 = "ObjectID";
                    oidfldEdit.Type_2 = esriFieldType.esriFieldTypeOID;

                    IField keyfld = new FieldClass();
                    IFieldEdit keyfldEdit = (IFieldEdit)keyfld;
                    keyfldEdit.AliasName_2 = "AKEY";
                    keyfldEdit.Name_2 = "AKEY";
                    keyfldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    keyfldEdit.Length_2 = 255;

                    IField valuefld = new FieldClass();
                    IFieldEdit valuefldEdit = (IFieldEdit)valuefld;
                    valuefldEdit.AliasName_2 = "AVALUE";
                    valuefldEdit.Name_2 = "AVALUE";
                    valuefldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    valuefldEdit.Length_2 = 255;

                    fldsEdit.set_Field(0, oidfld);
                    fldsEdit.set_Field(1, keyfld);
                    fldsEdit.set_Field(2, valuefld);

                    ESRI.ArcGIS.esriSystem.UID AddressingSettingsUID = new ESRI.ArcGIS.esriSystem.UID();
                    AddressingSettingsUID.Value = AddressingSettingsGUID;

                    ESRI.ArcGIS.esriSystem.UID extclsid = new ESRI.ArcGIS.esriSystem.UID();
                    extclsid.Value = "esriGeoDatabase.Object";

                    IFeatureClassDescription fcDesc = new FeatureClassDescriptionClass();
                    IObjectClassDescription ocDesc = (IObjectClassDescription)fcDesc;

                    SettingsTable = fwspc.CreateTable("AddressingSettings", flds, AddressingSettingsUID, null, null);

                }
                catch (Exception ex)
                {
                    throw (ex);
                }





                Globals.SettingsTable = SettingsTable;

                ICursor cursor = SettingsTable.Search(null, false);
                try
                {
                    IRow row = cursor.NextRow();
                    System.Collections.Generic.Dictionary<string, string> settings = new System.Collections.Generic.Dictionary<string, string>();
                    while (row != null)
                    {
                        settings[row.get_Value(1).ToString()] = row.get_Value(2).ToString();
                        row = cursor.NextRow();
                    }

                    Globals.Settings = settings;
                }
                catch
                {
                    cursor = null;
                }
            }
            else
            {
                UseExtension = false;
            }
        }

        private void SetSettingsTable()
        {
            try
            {
                IFeatureWorkspace fwspc = (IFeatureWorkspace)theEditor.EditWorkspace;
                SettingsTable = getSettingsTable(fwspc);
                if (SettingsTable == null)
                {
                    CreateSettingsTable(fwspc);
                }
                else
                {
                    Globals.SettingsTable = SettingsTable;

                    ICursor cursor = SettingsTable.Search(null, false);
                    try
                    {
                        IRow row = cursor.NextRow();
                        System.Collections.Generic.Dictionary<string, string> settings = new System.Collections.Generic.Dictionary<string, string>();
                        while (row != null)
                        {
                            settings[row.get_Value(1).ToString()] = row.get_Value(2).ToString();
                            row = cursor.NextRow();
                        }

                        Globals.Settings = settings;
                    }
                    catch
                    {
                        cursor = null;
                    }
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Settings Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                UseExtension = false;
            }
        }

        private void CheckForSettingsTable()
        {

        }

        protected override void OnShutdown()
        {

        }

        #region Editor Events

        #region Shortcut properties to the various editor event interfaces
        private IEditEvents_Event Events
        {
            get { return ArcMap.Editor as IEditEvents_Event; }
        }
        private IEditEvents2_Event Events2
        {
            get { return ArcMap.Editor as IEditEvents2_Event; }
        }
        private IEditEvents3_Event Events3
        {
            get { return ArcMap.Editor as IEditEvents3_Event; }
        }
        private IEditEvents4_Event Events4
        {
            get { return ArcMap.Editor as IEditEvents4_Event; }
        }
        private IDocumentEvents_Event DocEvents
        {
            get{return ArcMap.Document as IDocumentEvents_Event;}
        }
        #endregion
        void WireEditorEvents()
        {
            //
            //  TODO: Sample code demonstrating editor event wiring
            //
            Events.OnCurrentTaskChanged += delegate
            {
                if (ArcMap.Editor.CurrentTask != null)
                    System.Diagnostics.Debug.WriteLine(ArcMap.Editor.CurrentTask.Name);
            };
            Events.OnStartEditing += new IEditEvents_OnStartEditingEventHandler(Events_OnStartEditing);
            Events2.BeforeStopEditing += delegate(bool save) { OnBeforeStopEditing(save); };

            Events.OnCreateFeature += new IEditEvents_OnCreateFeatureEventHandler(Events_OnCreateFeature);
            Events.OnChangeFeature += new IEditEvents_OnChangeFeatureEventHandler(Events_OnChangeFeature);
        }

        void Events_OnStartEditing()
        {
            SetSettingsTable();
        }

        void Events_OnChangeFeature(IObject obj)
        {
            //if (!UseExtension)
            //    return;
            try
            {
                IFeature inFeature = (IFeature)obj;
                IFeatureClass fc = (IFeatureClass)inFeature.Class;

                int editor_fld_id = fc.FindField("EDITOR");
                int date_modified_id = fc.FindField("DATE_MODIFIED");

                if (editor_fld_id > -1)
                {
                    try
                    {
                        inFeature.set_Value(editor_fld_id, WindowsIdentity.GetCurrent().Name);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.EventLog.WriteEntry("AddressingToolsEditExtension", ex.StackTrace, System.Diagnostics.EventLogEntryType.Error);
                    }
                }
                if (date_modified_id > -1)
                {
                    try
                    {
                        inFeature.set_Value(date_modified_id, DateTime.Now.ToString());
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.EventLog.WriteEntry("AddressingToolsEditExtension", ex.StackTrace, System.Diagnostics.EventLogEntryType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("AddressingToolsEditExtension", ex.StackTrace, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        void Events_OnCreateFeature(ESRI.ArcGIS.Geodatabase.IObject obj)
        {
            //if (!UseExtension)
            //    return;

            IFeature inFeature = null;
            try
            {
                inFeature = (IFeature)obj;
            }
            catch { return; }
            IFeatureClass fc = (IFeatureClass)inFeature.Class;

            int date_created_id = fc.Fields.FindField("DATE_CREATED");

            if (date_created_id > -1)
            {
                try
                {
                    inFeature.set_Value(date_created_id, DateTime.Now.ToString());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("AddressingToolsEditExtension", ex.StackTrace, System.Diagnostics.EventLogEntryType.Error);
                }
            }

            if (fc.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryLine || fc.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline)
            {
                int clid = fc.Fields.FindField("CLID");
                if (clid >= 0)
                {
                    if (fc.Fields.get_Field(clid).Type == esriFieldType.esriFieldTypeInteger || fc.Fields.get_Field(clid).Type == esriFieldType.esriFieldTypeSmallInteger)
                    {
                        int newVal = getNextID(fc, clid, inFeature);
                        inFeature.set_Value(clid, newVal);
                    }
                }
            }

            if (fc.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
            {
                int clid = fc.Fields.FindField("APID");
                if (clid >= 0)
                {
                    if (fc.Fields.get_Field(clid).Type == esriFieldType.esriFieldTypeSmallInteger || fc.Fields.get_Field(clid).Type == esriFieldType.esriFieldTypeInteger)
                    {
                        int newVal = getNextID(fc, clid, inFeature);
                        inFeature.set_Value(clid, newVal);
                    }
                }
            }
        }

        private int getNextID(IFeatureClass fc, int clid, IFeature inFeature)
        {
            IDataStatistics stats = new DataStatisticsClass();
            stats.Field = fc.Fields.get_Field(clid).Name;
            IQueryFilter qf = new QueryFilterClass();
            qf.WhereClause = String.Format("{0} > {1}", fc.OIDFieldName, LastOID -100);
            ICursor cursor = (ICursor)fc.Search(qf, false);
            stats.Cursor = cursor;
            int currentMax = Convert.ToInt32(stats.Statistics.Maximum);
            LastOID = inFeature.OID;
            return currentMax + 1;
            
        }

        void OnBeforeStopEditing(bool save)
        {
            try
            {
                ICursor cursor = SettingsTable.Search(null, false);

                try
                {
                    IRow row = cursor.NextRow();

                    bool found = false;

                    foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in Globals.Settings)
                    {
                        while (row != null && found == false)
                        {
                            if (row.get_Value(1).ToString() == kvp.Key)
                            {
                                row.set_Value(2, kvp.Value);
                                row.Store();
                                found = true;
                            }
                            row = cursor.NextRow();
                        }

                        if (found == false)
                        {
                            IRow r1 = SettingsTable.CreateRow();
                            r1.set_Value(1, kvp.Key);
                            r1.set_Value(2, kvp.Value);
                            r1.Store();
                        }

                        found = false;
                    }
                }
                catch
                {

                    foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in Globals.Settings)
                    {
                        IRow r1 = SettingsTable.CreateRow();
                        r1.set_Value(1, kvp.Key);
                        r1.set_Value(2, kvp.Value);
                        r1.Store();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            LastOID = 100;
        }
        #endregion

    }

}
