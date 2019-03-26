using System;
using System.Collections.Generic;
using System.Drawing;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace AddressingTools
{

    public static class Globals
    {
        public delegate void CenterlineSelected(object sender, EventArgs e);
        public delegate void SettingsLoaded(object sender, EventArgs e);

        private static bool islicensed = true;

        private static IWorkspace workspace;
        private static IMap map;
        private static Dictionary<string, string> settings = new Dictionary<string, string>();
        private static CenterlineFieldMap centerlineFieldMap = new CenterlineFieldMap();
        private static AddressPointFieldMap addresspointFieldMap = new AddressPointFieldMap();
        private static int selectedcenterlineoid = -1;
        private static ITable _SettingsTable;

        public static bool IsReady { get; set; }


        public static event CenterlineSelected OnCenterlineSelected;
        public static event SettingsLoaded OnSettingsLoaded;


        public static bool IsLicensed
        {
            get { return islicensed; }
            set { islicensed = value; }
        }

        public static int SelectedCenterlineID
        {
            get { return selectedcenterlineoid; }
            set
            {
                selectedcenterlineoid = value;
                EventArgs e = EventArgs.Empty;
                //e.OID = selectedcenterlineoid;
                object sender = Type.Missing;
                Globals.OnCenterlineSelectedHandler(sender, e);
            }
        }

        public static void OnSettingsLoadedHandler(object sender, EventArgs e)
        {
            if (OnSettingsLoaded != null)
                OnSettingsLoaded(sender, e);
        }

        public static void OnCenterlineSelectedHandler(object sender, EventArgs e)
        {
            if (OnCenterlineSelected != null)
                OnCenterlineSelected(sender, e);
        }

        public static ITable SettingsTable
        {
            set { _SettingsTable = value; }
        }
        public static Dictionary<string, string> Settings
        {
            get { return settings; }
            set 
            { 
                settings = value;
                LoadSettings();
                IsReady = true;
                OnSettingsLoadedHandler(settings, EventArgs.Empty);
            }
        }

        private static void LoadSettings()
        {
            getCenterlineValues();
            getAddressPointValues();
        }

        public static IWorkspace Workspace
        {
            get { return workspace; }
            set { workspace = value; }
        }

        public static IMap Map
        {
            get { return map; }
            set { map = value; }
        }

        public static CenterlineFieldMap CenterlineFields
        {
            get
            {
                return centerlineFieldMap;
            }
            set
            {
                centerlineFieldMap = value;
                setCenterlineValues();
            }
        }

        public static void setCenterlineValues()
        {
            try
            {
                settings[Names.CLclid] = centerlineFieldMap.Clid.ToString();
                settings[Names.CLeditor] = centerlineFieldMap.Editor.ToString();
                settings[Names.CLesnleft] = centerlineFieldMap.EsnLeft.ToString();
                settings[Names.CLesnright] = centerlineFieldMap.EsnRight.ToString();
                settings[Names.CLfromleft] = centerlineFieldMap.FromLeft.ToString();
                settings[Names.CLfromright] = centerlineFieldMap.FromRight.ToString();
                settings[Names.CLfullstname] = centerlineFieldMap.FullStName.ToString();
                settings[Names.CLmodified] = centerlineFieldMap.Modified.ToString();
                settings[Names.CLstname] = centerlineFieldMap.StName.ToString();
                settings[Names.CLstpredir] = centerlineFieldMap.StPreDir.ToString();
                settings[Names.CLstsufdir] = centerlineFieldMap.StSufDir.ToString();
                settings[Names.CLsttype] = centerlineFieldMap.StType.ToString();
                settings[Names.CLsttypesfx] = centerlineFieldMap.StTypeSfx.ToString();
                settings[Names.CLtoleft] = centerlineFieldMap.ToLeft.ToString();
                settings[Names.CLtoright] = centerlineFieldMap.ToRight.ToString();
                UpdateSettingsTable();
            }
            catch
            {
            }
        }

        private static void getCenterlineValues()
        {
            try
            {
                centerlineFieldMap.Clid = getIntSetting(Names.CLclid);
                centerlineFieldMap.Editor = getIntSetting(Names.CLeditor);
                centerlineFieldMap.EsnLeft = getIntSetting(Names.CLesnleft);
                centerlineFieldMap.EsnRight = getIntSetting(Names.CLesnright);
                centerlineFieldMap.FromLeft = getIntSetting(Names.CLfromleft);
                centerlineFieldMap.FromRight = getIntSetting(Names.CLfromright);
                centerlineFieldMap.FullStName = getIntSetting(Names.CLfullstname);
                centerlineFieldMap.Modified = getIntSetting(Names.CLmodified);
                centerlineFieldMap.StName = getIntSetting(Names.CLstname);
                centerlineFieldMap.StPreDir = getIntSetting(Names.CLstpredir);
                centerlineFieldMap.StSufDir = getIntSetting(Names.CLstsufdir);
                centerlineFieldMap.StType = getIntSetting(Names.CLsttype);
                centerlineFieldMap.StTypeSfx = getIntSetting(Names.CLsttypesfx);
                centerlineFieldMap.ToLeft = getIntSetting(Names.CLtoleft);
                centerlineFieldMap.ToRight = getIntSetting(Names.CLtoright);
                UpdateSettingsTable();
            }
            catch
            {

            }
        }

        public static AddressPointFieldMap AddressPointFields
        {
            get
            {
                return addresspointFieldMap;
            }
            set
            {
                addresspointFieldMap = value;
                setAddressPointValues();
            }
        }

        public static void setAddressPointValues()
        {
            try
            {
                settings[Names.APapid] = addresspointFieldMap.Apid.ToString();
                settings[Names.APclid] = addresspointFieldMap.Clid.ToString();
                settings[Names.APeditor] = addresspointFieldMap.Editor.ToString();
                settings[Names.APfulladdress] = addresspointFieldMap.FullAddress.ToString();
                settings[Names.APhousenumber] = addresspointFieldMap.HouseNumber.ToString();
                settings[Names.APhousenumsfx] = addresspointFieldMap.HouseNumberSfx.ToString();
                settings[Names.APmodified] = addresspointFieldMap.Modified.ToString();
                settings[Names.APstname] = addresspointFieldMap.StName.ToString();
                settings[Names.APstpredir] = addresspointFieldMap.StPreDir.ToString();
                settings[Names.APstsufdir] = addresspointFieldMap.StSufDir.ToString();
                settings[Names.APsttype] = addresspointFieldMap.StType.ToString();
                settings[Names.APsttypesfx] = addresspointFieldMap.StTypeSfx.ToString();
                UpdateSettingsTable();
            }
            catch
            {
            }
        }

        private static void UpdateSettingsTable()
        {
            try
            {
                ICursor cursor = _SettingsTable.Search(null, false);

                try
                {
                    IRow row = cursor.NextRow();

                    bool found = false;

                    foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in settings)
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
                            IRow r1 = _SettingsTable.CreateRow();
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
                        IRow r1 = _SettingsTable.CreateRow();
                        r1.set_Value(1, kvp.Key);
                        r1.set_Value(2, kvp.Value);
                        r1.Store();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private static void getAddressPointValues()
        {
            try
            {
                addresspointFieldMap.Apid = getIntSetting(Names.APapid);
                addresspointFieldMap.Clid = getIntSetting(Names.APclid);
                addresspointFieldMap.Editor = getIntSetting(Names.APeditor);
                addresspointFieldMap.FullAddress = getIntSetting(Names.APfulladdress);
                addresspointFieldMap.HouseNumber = getIntSetting(Names.APhousenumber);
                addresspointFieldMap.HouseNumberSfx = getIntSetting(Names.APhousenumsfx);
                addresspointFieldMap.Modified = getIntSetting(Names.APmodified);
                addresspointFieldMap.StName = getIntSetting(Names.APstname);
                addresspointFieldMap.StPreDir = getIntSetting(Names.APstpredir);
                addresspointFieldMap.StSufDir = getIntSetting(Names.APstsufdir);
                addresspointFieldMap.StType = getIntSetting(Names.APsttype);
                addresspointFieldMap.StTypeSfx = getIntSetting(Names.APsttypesfx);
            }
            catch
            {
            }
        }

        public static IFeatureLayer AddressPointLayer
        {
            get
            {
                return getFeatureLayerByName(getStringSetting(Names.APLayerName));
            }
        }

        public static string AddressPointLayerName
        {
            get
            {
                return getStringSetting(Names.APLayerName);
            }
            set
            {
                settings[Names.APLayerName] = value;
                UpdateSettingsTable();
            }
        }

        public static IFeatureLayer CenterlineLayer
        {
            get
            {
                return getFeatureLayerByName(getStringSetting(Names.CLLayerName));
            }
        }

        public static string CenterlineLayerName
        {
            get
            {
                return getStringSetting(Names.CLLayerName);
            }
            set
            {
                settings[Names.CLLayerName] = value;
                UpdateSettingsTable();
            }
        }


        public static double AddressesPerUnit 
        {
            get
            {
                double d = getDoubleSetting(Names.AddressesPerUnit);
                if (d < 1)
                    return 5.28d;
                return d;
            }
            set
            {
                settings[Names.AddressesPerUnit] = value.ToString();
                UpdateSettingsTable();
            }
        }


        public static int FlashFeatureDelay
        {
            get
            {
                int i = getIntSetting(Names.FlashFeatureDelay);
                if (i < 600)
                {
                    i = 600;
                    settings[Names.FlashFeatureDelay] = "600";
                    UpdateSettingsTable();
                }
                return i;
            }
            set
            {
                settings[Names.FlashFeatureDelay] = value.ToString();
                UpdateSettingsTable();
            }
        }

        public static IRgbColor FlashFeatureColor 
        {
            get
            {
                return getColorSetting(Names.FlashFeatureColor);
            }
        }

        

        public static string getFlashFeatureColorHex()
        {
            return getStringSetting(Names.FlashFeatureColor);
        }
        public static string getLineFeedbackColorHex()
        {
            return getStringSetting(Names.LineFeedbackColor);
        }

        public static void setFlashFeatureColor(Color color)
        {
            settings[Names.FlashFeatureColor] = ColorTranslator.ToHtml(color);
            UpdateSettingsTable();
        }

        public static void setLineFeedBackColor(Color color)
        {
            settings[Names.LineFeedbackColor] = ColorTranslator.ToHtml(color);
            UpdateSettingsTable();
        }

        public static void setLineFeedBackLineStyle(int i)
        {
            settings[Names.LineFeedbackSyle] = i.ToString();
            UpdateSettingsTable();
        }

        private static string getStringSetting(string key)
        {
            if (settings.ContainsKey(key))
                return settings[key];

            return null;
        }

        private static double getDoubleSetting(string key)
        {
            try
            {
                if (settings.ContainsKey(key))
                    return Double.Parse(settings[key]);
            }
            catch
            {
            }
            return 0.0d;
        }

        private static int getIntSetting(string key)
        {
            try
            {
                if (settings.ContainsKey(key))
                    return Convert.ToInt32(settings[key]);
            }
            catch {}
            return -1;
        }

        private static IFeatureLayer getFeatureLayerSetting(string key)
        {
            try
            {
                if (settings.ContainsKey(key))
                {
                    return getFeatureLayerByName(settings[key]);
                }
            }
            catch {}
            return null;
        }

        private static IPoint getPointSetting(string key)
        {
            try
            {
                if (settings.ContainsKey(key))
                {
                    return String2Point(settings[key]);
                }
            }
            catch {}
            return null;
        }

        private static string Point2String(IPoint p)
        {
            string s = p.X.ToString() + " " + p.Y.ToString();
            return s;
        }

        private static IPoint String2Point(string s)
        {
            IPoint p = new PointClass();
            try
            {
                string[] parts = s.Split(' ');
                p.X = Convert.ToDouble(parts[0]);
                p.Y = Convert.ToDouble(parts[1]);
            }
            catch
            {
            }
            return p;
        }

        public static List<string> getSymbolNames(IFeatureClass Anno)
        {
            List<string> names = new List<string>();
            IAnnoClass aClas = (IAnnoClass)Anno.Extension;
            ISymbolCollection2 isc = (ISymbolCollection2)aClas.SymbolCollection;
            isc.Reset();
            ISymbolIdentifier2 sId = (ISymbolIdentifier2)isc.Next();

            while (sId != null)
            {
                names.Add(sId.Name);
                sId = (ISymbolIdentifier2)aClas.SymbolCollection.Next();
            }

            names.Sort();
            return names;
        }

        public static string getSymbolNameById(IFeatureClass Anno, int id)
        {
            IAnnoClass aClas = (IAnnoClass)Anno.Extension;
            ISymbolCollection2 isc = (ISymbolCollection2)aClas.SymbolCollection;
            isc.Reset();
            ISymbolIdentifier2 sId = (ISymbolIdentifier2)isc.Next();

            while (sId != null)
            {
                if (sId.ID == id)
                {
                    return sId.Name;
                }
                sId = (ISymbolIdentifier2)isc.Next();
            }
            return null;
        }

        public static int getSymbolIDByName(IFeatureClass Anno, string name)
        {
            IAnnoClass aClas = (IAnnoClass)Anno.Extension;
            ISymbolCollection2 isc = (ISymbolCollection2)aClas.SymbolCollection;
            isc.Reset();// very important to call Reset() if not you get no result from Next()
            ISymbolIdentifier2 sId = (ISymbolIdentifier2)isc.Next();

            while (sId != null)
            {
                if (sId.Name.ToUpper() == name.ToUpper())
                {
                    return sId.ID;
                }
                sId = (ISymbolIdentifier2)isc.Next();
            }
            return -1;

        }

        public static List<string> getClassNames(IFeatureClass Anno)
        {
            List<string> names = new List<string>();
            ISubtypes subtypes = (ISubtypes)Anno;
            if (subtypes.HasSubtype)
            {
                IEnumSubtype enumSubs = subtypes.Subtypes;
                int subTypeCode;
                string subTypeName = enumSubs.Next(out subTypeCode);
                while (subTypeName != null)
                {
                    names.Add(subTypeName);

                    subTypeName = enumSubs.Next(out subTypeCode);
                }
            }
            names.Sort();
            return names;
        }

        public static string getClassNameById(IFeatureClass Anno, int id)
        {
            ISubtypes subtypes = (ISubtypes)Anno;
            if (subtypes.HasSubtype)
            {
                IEnumSubtype enumSubs = subtypes.Subtypes;
                int subTypeCode;
                string subTypeName = enumSubs.Next(out subTypeCode);
                while (subTypeName != null)
                {
                    if (subTypeCode == id)
                        return subTypeName;

                    subTypeName = enumSubs.Next(out subTypeCode);
                }
            }

            return null;
        }

        public static int getClassIDByName(IFeatureClass Anno, string name)
        {
            ISubtypes subtypes = (ISubtypes)Anno;
            if (subtypes.HasSubtype)
            {
                IEnumSubtype enumSubs = subtypes.Subtypes;
                int subTypeCode;
                string subTypeName = enumSubs.Next(out subTypeCode);
                while (subTypeName != null)
                {
                    if (subTypeName.ToUpper() == name.ToUpper())
                        return subTypeCode;

                    subTypeName = enumSubs.Next(out subTypeCode);
                }
            }

            return -1;
        }

        public static IFeatureLayer getFeatureLayerByName(string name)
        {
            try
            {
                for (int i = 0; i < Map.LayerCount; i++)
                {
                    ILayer l = Map.get_Layer(i);
                    if (l.Name.ToUpper() == name.ToUpper())
                    {
                        IFeatureLayer fl = (IFeatureLayer)l;
                        return fl;
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        public static IFeatureLayer getFeatureLayerByName(IMap m, string name)
        {
            for (int i = 0; i < m.LayerCount; i++)
            {
                try
                {
                    ILayer l = m.get_Layer(i);
                    if (l.Name.ToUpper() == name.ToUpper())
                    {
                        IFeatureLayer fl = (IFeatureLayer)l;
                        return fl;
                    }
                }
                catch
                {
                }
            }
            return null;
        }

        public static IFeatureClass getFeatureClassByName(IMap m, string name)
        {
            for (int i = 0; i < m.LayerCount; i++)
            {
                try
                {
                    ILayer l = m.get_Layer(i);
                    if (l.Name.ToUpper() == name.ToUpper())
                    {
                        IFeatureLayer fl = (IFeatureLayer)l;
                        return fl.FeatureClass;
                    }
                }
                catch
                {
                }
            }
            return null;
        }

        private static IFeatureClass getFeatureClassByName(string name)
        {
            try
            {
                IEnumDataset featureDataSets = workspace.get_Datasets(esriDatasetType.esriDTAny);

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
                                if (featc.Name.ToUpper() == name.ToUpper())
                                {
                                    IFeatureClass fc = (IFeatureClass)featc;
                                    return fc;
                                }
                            }
                            featc = fcs.Next();
                        }
                    }
                    else
                    {
                        if (featureDataSet.Type == esriDatasetType.esriDTFeatureClass)
                        {
                            if (featureDataSet.Name.ToUpper() == name.ToUpper())
                            {
                                IFeatureClass _fc = (IFeatureClass)featureDataSet;
                                return _fc;
                            }
                        }
                    }

                    featureDataSet = featureDataSets.Next();
                }
            }
            catch
            {
            }

            return null;

        }

        public static bool LayerOnMap(string LayerName)
        {
            try
            {
                for (int i = 0; i < Map.LayerCount; i++)
                {
                    if (Map.get_Layer(i).Name == LayerName)
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }


        private static bool getBoolFromString(string p)
        {
            if (p == Names.True)
                return true;
            else
                return false;
        }

        private static IRgbColor getColorSetting(string key)
        {
            IRgbColor color = new RgbColorClass();
            try
            {
                Color c = (Color)ColorTranslator.FromHtml(getStringSetting(key));
                color.Red = c.R;
                color.Green = c.G;
                color.Blue = c.B;
            }
            catch
            {
                color.Blue = 123;
                color.Red = 222;
                color.Green = 10;
            }
            return color;
        }

        private static esriSimpleLineStyle getLineStyleSetting(string key)
        {
            try
            {
                int style = getIntSetting(key);
                switch (style)
                {
                    case 0:
                        return esriSimpleLineStyle.esriSLSDash;
                        //break;
                    case 1:
                        return esriSimpleLineStyle.esriSLSDashDot;
                        //break;
                    case 2:
                        return esriSimpleLineStyle.esriSLSDashDotDot;
                        //break;
                    case 3:
                        return esriSimpleLineStyle.esriSLSDot;
                        //break;
                    case 4:
                        return esriSimpleLineStyle.esriSLSInsideFrame;
                        //break;
                    case 5:
                        return esriSimpleLineStyle.esriSLSSolid;
                        //break;
                    case 6:
                        return esriSimpleLineStyle.esriSLSSolid;
                        //break;
                    default:
                        return esriSimpleLineStyle.esriSLSSolid;
                        //break;
                }
            }
            catch
            {
                return esriSimpleLineStyle.esriSLSSolid;
            }
        }

        internal static ISymbol getFeedBackLineSymbol(INewLineFeedback lfb)
        {
            ISimpleLineSymbol sym = lfb.Symbol as ISimpleLineSymbol;
            sym.Color = getColorSetting(Names.LineFeedbackColor);
            sym.Style = esriSimpleLineStyle.esriSLSSolid;
            sym.Width = 2.0d;
            return (ISymbol)sym;
        }
    }

    public static class Names
    {
        public static readonly string True = "TRUE";
        public static readonly string False = "FALSE";
        public static readonly string APhousenumber = "APhousenumber";
        public static readonly string APhousenumsfx =  "APhousenumsfx";
        public static readonly string APapid = "APapid";
        public static readonly string APstpredir = "APstpredir";
        public static readonly string APsttype = "APsttype";
        public static readonly string APstname = "APstname";
        public static readonly string APstsufdir = "APstsufdir";
        public static readonly string APclid = "APclid";
        public static readonly string APfulladdress = "APfulladdress ";
        public static readonly string APeditor = "APeditor";
        public static readonly string APmodified = "APmodified";
        public static readonly string APsttypesfx = "APsttypesfx";
        public static readonly string APzipcode = "APzipcode";
        public static readonly string APcommunity = "APcommunity";

        public static readonly string CLfromleft = "CLfromleft";
        public static readonly string CLtoleft = "CLtoleft";
        public static readonly string CLfromright = "CLfromright";
        public static readonly string CLtoright = "CLtoright";
        public static readonly string CLstpredir = "CLstpredir";
        public static readonly string CLsttype = "CLsttype";
        public static readonly string CLstname = "CLstname";
        public static readonly string CLstsufdir = "CLstsufdir";
        public static readonly string CLclid = "CLclid";
        public static readonly string CLfullstname = "CLfullstname";
        public static readonly string CLesnleft = "CLesnleft";
        public static readonly string CLesnright = "CLesnright";
        public static readonly string CLeditor = "CLeditor";
        public static readonly string CLmodified = "CLmodified";
        public static readonly string CLsttypesfx = "CLsttypesfx";
        public static readonly string CLzipcode = "CLzipcode";
        public static readonly string CLcommunity = "CLcommunity";

        public static readonly string CLLayerName = "CLLayerName";
        public static readonly string APLayerName = "APLayerName";

        public static readonly string AddressesPerUnit = "AddressesPerUnit";

        public static readonly string LineFeedbackColor = "LineFeedBackColor";
        public static readonly string LineFeedbackSyle = "LineFeedBackStyle";

        public static readonly string FlashFeatureDelay = "FlashFeatureDelay";
        public static readonly string FlashFeatureColor = "FlashFeatureColor";
    }
}
