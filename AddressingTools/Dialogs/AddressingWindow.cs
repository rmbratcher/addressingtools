using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Framework;
using AddressingTools.Utils;
using ESRI.ArcGIS.Geometry;

namespace AddressingTools
{
    /// <summary>
    /// Designer class of the dockable window add-in. It contains user interfaces that
    /// make up the dockable window.
    /// </summary>
    ///
    public partial class AddressingWindow : UserControl
    {

        private const string TAG = "AddressingTools.AddressingWindow";

        private IActiveViewEvents_Event mActiveViewEvents;
        private IDocumentEvents_Event mDocumentEvents;
        private IMap mMap;
        private IMxDocument mDoc;
        private IApplicationStatusEvents_Event mApplicationEvents;
        private agdErrorLogger log;
        private IFeature CenterlineEditFeature;
        private CenterlineErrors Errors;
        private CheckCenterlineRanges ccr;
        int selected_error_oid = -1;
        string select_chain_name = "";

        private bool EventsSet = false;

        public AddressingWindow(object hook)
        {
            InitializeComponent();
            this.Hook = hook;

            log = new agdErrorLogger("errors.agd.cc");

            mApplicationEvents = (IApplicationStatusEvents_Event)ArcMap.Application;
            mApplicationEvents.Initialized += new IApplicationStatusEvents_InitializedEventHandler(mApplicationEvents_Initialized);
            this.MouseEnter += new EventHandler(AddressingWindow_MouseEnter);
            Globals.OnCenterlineSelected += new Globals.CenterlineSelected(Globals_OnCenterlineSelected);
            Globals.OnSettingsLoaded += new Globals.SettingsLoaded(Globals_OnSettingsLoaded);

            error_treeview.MouseUp += new MouseEventHandler(error_treeview_MouseUp);

            SetEvents();

        }

        void Globals_OnSettingsLoaded(object sender, EventArgs e)
        {
            setCurrentValues();
        }

        void Globals_OnCenterlineSelected(object sender, EventArgs e)
        {
            IFeature f = Globals.CenterlineLayer.FeatureClass.GetFeature(Globals.SelectedCenterlineID);
            SetCenterlineEditFeature(f);
        }

        void error_treeview_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                System.Drawing.Point p = new System.Drawing.Point(e.X, e.Y);

                TreeNode node = error_treeview.GetNodeAt(p);
                if (node != null)
                {
                    error_treeview.SelectedNode = node;
                    switch (Convert.ToString(node.Tag))
                    {
                        case "Segment":
                            selected_error_oid = Convert.ToInt32(node.Text);
                            contextMenuStrip1.Show(error_treeview, p);
                            break;
                        case "Chain":
                            select_chain_name = node.Name;
                            contextMenuStrip2.Show(error_treeview, p);
                            break;

                            
                    }
                }
            }
        }

        void AddressingWindow_MouseEnter(object sender, EventArgs e)
        {
            if (!EventsSet)
                SetEvents();
        }

        public void SetEvents()
        {
            mDoc = (IMxDocument)ArcMap.Application.Document;
            mMap = mDoc.FocusMap;

            if (mMap != null)
            {

                mActiveViewEvents = (IActiveViewEvents_Event)mMap;
                mActiveViewEvents.ItemAdded += new IActiveViewEvents_ItemAddedEventHandler(mActiveViewEvents_ItemAdded);
                mActiveViewEvents.ItemDeleted += new IActiveViewEvents_ItemDeletedEventHandler(mActiveViewEvents_ItemDeleted);
                mActiveViewEvents.FocusMapChanged += new IActiveViewEvents_FocusMapChangedEventHandler(mActiveViewEvents_FocusMapChanged);
                mDocumentEvents = (IDocumentEvents_Event)mDoc;
                mDocumentEvents.OpenDocument += new IDocumentEvents_OpenDocumentEventHandler(mDocumentEvents_OpenDocument);
                //mActiveViewEvents.SelectionChanged += new IActiveViewEvents_SelectionChangedEventHandler(mActiveViewEvents_SelectionChanged);
                setCurrentValues();
                EventsSet = true;
            }
        }

        private int getRangeValue(int i, IFeature f)
        {
            try
            {
                return Convert.ToInt32(f.get_Value(i));
            }
            catch { return 0; }
        }

        private void SetCenterlineEditFeature(IFeature f)
        {
            
            IFeature Centerline = f;


            try
            {

                IPolyline pline = Centerline.Shape as IPolyline;
                ICurve curve = pline as ICurve;
                double distance = curve.Length;

                int start_number = 0;
                int from_left = getRangeValue(Globals.CenterlineFields.FromLeft, f);
                int from_right = getRangeValue(Globals.CenterlineFields.FromRight,f);
                start_number = Math.Min(from_left, from_right);

                int to_left = getRangeValue(Globals.CenterlineFields.ToLeft,f);
                int to_right = getRangeValue(Globals.CenterlineFields.ToRight,f);
                int end_number = 0;
                end_number = Math.Max(to_left, to_right);

                int total_addresses = end_number - start_number;

                double apu = Globals.AddressesPerUnit;
                double lengthIndex = Math.Abs(distance / apu);
                double max = start_number + lengthIndex;
                int cal_max_housenumber = Convert.ToInt32(Math.Round(max, 0));

                centerline_range_data.CenterlineValues.Clear();
                centerline_range_data.CenterlineValues.AddCenterlineValuesRow("From Left", from_left);
                centerline_range_data.CenterlineValues.AddCenterlineValuesRow("From Right", from_right);
                centerline_range_data.CenterlineValues.AddCenterlineValuesRow("To Left", to_left);
                centerline_range_data.CenterlineValues.AddCenterlineValuesRow("To Right", to_right);

                //dataGridView1.DataSource = centerline_range_data.CenterlineValues;

                centerline_range_data.CenterlineValues.CenterlineValuesRowChanged += new AddressingTools.centerline_range_data.CenterlineValuesRowChangeEventHandler(CenterlineValues_CenterlineValuesRowChanged);


                low_range_tbx.Text = start_number.ToString();
                high_range_tbx.Text = end_number.ToString();
                calc_high_range_tbx.Text = cal_max_housenumber.ToString();


                CenterlineEditFeature = f;

            }
            catch { }
        }

        void CenterlineValues_CenterlineValuesRowChanged(object sender, centerline_range_data.CenterlineValuesRowChangeEvent e)
        {
            try
            {
                switch (e.Row.Name)
                {
                    case "From Left":
                        CenterlineEditFeature.set_Value(Globals.CenterlineFields.FromLeft, e.Row.Value);
                        break;
                    case "To Left":
                        CenterlineEditFeature.set_Value(Globals.CenterlineFields.ToLeft, e.Row.Value);
                        break;
                    case "From Right":
                        CenterlineEditFeature.set_Value(Globals.CenterlineFields.FromRight, e.Row.Value);
                        break;
                    case "To Right":
                        CenterlineEditFeature.set_Value(Globals.CenterlineFields.ToRight, e.Row.Value);
                        break;
                }

                ArcMap.Editor.StartOperation();
                CenterlineEditFeature.Store();
                ArcMap.Editor.StopOperation("Change Centerline Attr..");
            }
            catch { }
        }



        void mApplicationEvents_Initialized()
        {
            mDoc = (IMxDocument)ArcMap.Application.Document;
            mMap = mDoc.FocusMap;

            if (mMap != null)
            {

                mActiveViewEvents = (IActiveViewEvents_Event)mMap;
                mActiveViewEvents.ItemAdded += new IActiveViewEvents_ItemAddedEventHandler(mActiveViewEvents_ItemAdded);
                mActiveViewEvents.ItemDeleted += new IActiveViewEvents_ItemDeletedEventHandler(mActiveViewEvents_ItemDeleted);
                mActiveViewEvents.FocusMapChanged += new IActiveViewEvents_FocusMapChangedEventHandler(mActiveViewEvents_FocusMapChanged);
                mDocumentEvents = (IDocumentEvents_Event)mDoc;
                mDocumentEvents.OpenDocument += new IDocumentEvents_OpenDocumentEventHandler(mDocumentEvents_OpenDocument);
            }
            setCurrentValues();
        }

        void mDocumentEvents_OpenDocument()
        {
            setCurrentValues();
            if (Globals.Workspace != null)
            {
                changeWorkSpace(Globals.Workspace);
            }
        }

        public void setCurrentValues()
        {
            Globals.Map = ArcMap.Document.FocusMap;

            errorProvider1.Clear();

            Color fc = (Color)ColorTranslator.FromHtml(Globals.getLineFeedbackColorHex());
            linefeedbackColorSwatch.BackColor = fc;

            Color gc = Color.FromArgb(Globals.FlashFeatureColor.Red, Globals.FlashFeatureColor.Green, Globals.FlashFeatureColor.Blue);
            flashColorSwatch.BackColor = gc;

            //CLIDTextBox.Text = Globals.CLID.ToString();
            //APIDTextBox.Text = Globals.APID.ToString();

            flashDelaycomboBox.Text = Globals.FlashFeatureDelay.ToString();

            if (Globals.Workspace != null)
            {
                changeWorkSpace(Globals.Workspace);
            }

            if (Globals.CenterlineLayer != null)
                centerlineLayerComboBox.Text = Globals.CenterlineLayer.Name;
            if (Globals.AddressPointLayer != null)
                addressPointcomboBox.Text = Globals.AddressPointLayer.Name;

            centerlineLayerComboBox.Items.Clear();
            addressPointcomboBox.Items.Clear();

            for (int i = 0; i < ArcMap.Document.FocusMap.LayerCount; i++)
            {
                try
                {
                    ILayer l = ArcMap.Document.FocusMap.get_Layer(i);

                    if (l.SupportedDrawPhases > 5)
                    {

                        IFeatureLayer fl = (IFeatureLayer)l;

                        if (fl.FeatureClass.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryLine || fl.FeatureClass.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline)
                            centerlineLayerComboBox.Items.Add(l.Name);
                        if (fl.FeatureClass.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
                            addressPointcomboBox.Items.Add(l.Name);
                    }
                }
                catch(Exception ex)
                {
                    log.WriteError(ex, TAG, System.Security.Principal.WindowsIdentity.GetCurrent().Name, null);
                }
            }

            textBox1.Text = Globals.AddressesPerUnit.ToString();
        }

        private void setWorkSpace(IWorkspace wrkspc)
        {
            Globals.Workspace = wrkspc;
            changeWorkSpace(wrkspc);
        }

        private void changeWorkSpace(IWorkspace wrkspc)
        {
            centerlineLayerComboBox.Items.Clear();
            addressPointcomboBox.Items.Clear();

            for (int i = 0; i < ArcMap.Document.FocusMap.LayerCount; i++)
            {
                try
                {
                    ILayer l = ArcMap.Document.FocusMap.get_Layer(i);

                    IFeatureLayer fl = (IFeatureLayer)l;

                    if (fl.FeatureClass.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryLine || fl.FeatureClass.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline)
                        centerlineLayerComboBox.Items.Add(l.Name);
                    if (fl.FeatureClass.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
                        addressPointcomboBox.Items.Add(l.Name);
                }
                catch
                {
                }
            }



        }

        private IFeatureLayer getFeatureLayerFromMap(IFeatureClass fc)
        {
            for (int i = 0; i < mMap.LayerCount; i++)
            {
                try
                {
                    ILayer l = mMap.get_Layer(i);
                    IFeatureLayer fl = (IFeatureLayer)l;
                    if (fc.AliasName == fl.FeatureClass.AliasName)
                    {
                        if (fc.FeatureDataset.Workspace == fl.FeatureClass.FeatureDataset.Workspace)
                            return fl;
                    }
                }
                catch
                {
                }
            }

            return null;
        }

        private IFeatureLayer getFeatureLayerFromMap(string name)
        {
            for (int i = 0; i < mMap.LayerCount; i++)
            {
                try
                {
                    ILayer l = mMap.get_Layer(i);
                    IFeatureLayer fl = (IFeatureLayer)l;
                    if (fl.Name == name)
                        return fl;
                }
                catch
                {
                }
            }

            return null;
        }


        void mActiveViewEvents_FocusMapChanged()
        {
            setCurrentValues();
            if (Globals.Workspace != null)
            {
                changeWorkSpace(Globals.Workspace);
            }
        }

        void mActiveViewEvents_ItemDeleted(object Item)
        {
            changeWorkSpace(Globals.Workspace);
        }

        void mActiveViewEvents_ItemAdded(object Item)
        {
            changeWorkSpace(Globals.Workspace);
        }

        /// <summary>
        /// Host object of the dockable window
        /// </summary>
        private object Hook
        {
            get;
            set;
        }

        /// <summary>
        /// Implementation class of the dockable window add-in. It is responsible for 
        /// creating and disposing the user interface class of the dockable window.
        /// </summary>
        public class AddinImpl : ESRI.ArcGIS.Desktop.AddIns.DockableWindow
        {
            private AddressingWindow m_windowUI;

            public AddinImpl()
            {
            }

            protected override IntPtr OnCreateChild()
            {
                m_windowUI = new AddressingWindow(this.Hook);
                return m_windowUI.Handle;
            }

            protected override void Dispose(bool disposing)
            {
                if (m_windowUI != null)
                    m_windowUI.Dispose(disposing);

                base.Dispose(disposing);
            }

        }

        private void ChangeLineFeedBackColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() != DialogResult.Cancel)
            {
                IRgbColor c = new RgbColorClass();
                c.Red = Convert.ToInt32(colorDialog.Color.R);
                c.Green = Convert.ToInt32(colorDialog.Color.G);
                c.Blue = Convert.ToInt32(colorDialog.Color.B);

                Globals.setLineFeedBackColor(colorDialog.Color);

                linefeedbackColorSwatch.BackColor = colorDialog.Color;
            }
        }

        private void ChangeflashColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() != DialogResult.Cancel)
            {
                IRgbColor c = new RgbColorClass();
                c.Red = Convert.ToInt32(colorDialog.Color.R);
                c.Green = Convert.ToInt32(colorDialog.Color.G);
                c.Blue = Convert.ToInt32(colorDialog.Color.B);

                Globals.setFlashFeatureColor(colorDialog.Color);

                //Globals.FlashFeatureColor = c;

                flashColorSwatch.BackColor = colorDialog.Color;
            }
        }

        private void centerlineLayerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Globals.CenterlineLayerName = centerlineLayerComboBox.SelectedItem.ToString();
        }

        private void addressPointcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Globals.AddressPointLayerName = addressPointcomboBox.SelectedItem.ToString();
        }

        private void matchCenterlineFieldsButton_Click(object sender, EventArgs e)
        {
            if (Globals.CenterlineLayer != null)
            {
                Form centerlineForm = new AddressingTools.CenerlineFields(Globals.CenterlineLayer.FeatureClass);

                centerlineForm.ShowDialog();
            }
            else
            {
                errorProvider1.SetError(matchCenterlineFieldsButton, "Must Set Centerline FeatureClass First");
            }
        }

        private void matchAddressPointFieldsButton_Click(object sender, EventArgs e)
        {
            if (Globals.AddressPointLayer != null)
            {
                Form addressFieldMatchForm = new AddressPointFields(Globals.AddressPointLayer.FeatureClass);
                addressFieldMatchForm.ShowDialog();
            }
            else
            {
                errorProvider1.SetError(matchAddressPointFieldsButton, "Must Set AddressPoint FeatureClass First");
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"[a-zA-Z]"))
                e.Handled = true;
        }

        private void setAddressesPerUnit_Click(object sender, EventArgs e)
        {
            try
            {
                double apu = Double.Parse(textBox1.Text, System.Globalization.NumberStyles.Float);
                Globals.AddressesPerUnit = apu;
                errorProvider1.Clear();
            }
            catch
            {
                errorProvider1.SetError(textBox1, "A number must be entered ex.. 5.28");
            }
        }

        private void SetCenterlineValues(object sender, EventArgs e)
        {

        }

        private void check_errors_btn_Click(object sender, EventArgs e)
        {
            ccr = new CheckCenterlineRanges();

            CheckFeaturesProgressBar.Value = 0;
            CheckFeaturesProgressBar.Visible = true;
            CheckFeaturesProgressBar.Maximum = ccr.NumFeatures;

            error_treeview.Nodes.Clear();

            ccr.ProgressUpdate += new CheckCenterlineRanges.ProcessUpdateHandler(ccr_ProgressUpdate);

            Errors = ccr.Check();

            

            List<string> streets = new List<string>(Errors.CenterlineChains.Keys);

            errorCountLabel.Text = ccr.NumErrors.ToString();

            string exp = "OBJECTID in (";
            if (Errors.CenterlineChains.Count > 0)
            {
                foreach (KeyValuePair<string, List<Range>> kvp in Errors.CenterlineChains)
                {
                    foreach (Range r in kvp.Value)
                    {
                        if (r.Errors.Count > 0)
                        {
                            exp += r.ID.ToString() + ",";
                        }
                    }
                }
            }
            else
            {
                exp += "-1";
            }

            string exp2 = exp.TrimEnd(',');

            exp = exp2;
            exp += ")";

            bool AddStreetLayer = true;

            ILayer streetLayer = null;
            IFeatureLayerDefinition fld = null;

            for (int i = 0; i < ArcMap.Document.FocusMap.LayerCount; i++)
            {
                ILayer l = ArcMap.Document.FocusMap.get_Layer(i);
                if (l.Name == "StreetErrors")
                {
                    AddStreetLayer = false;//ArcMap.Document.FocusMap.DeleteLayer(l);
                    streetLayer = l;
                }

            }

            if (AddStreetLayer)
            {

                // Attempted to create a layer from the errors......... FAIL

                IFeatureLayer streetErrorsLayer = new FeatureLayerClass();
                streetErrorsLayer.FeatureClass = Globals.CenterlineLayer.FeatureClass;

                //ArcMap.Document.FocusMap.

                streetLayer = (ILayer)streetErrorsLayer;

                fld = (IFeatureLayerDefinition)streetLayer;

                //ILayerDescription ld = (ILayerDescription)streetLayer;
                //ILayerDescription3 layerDesc = (ILayerDescription)ld;


                fld.DefinitionExpression = exp;
                //layerDesc.DefinitionExpression = exp;

                streetLayer.Name = "StreetErrors";



                ArcMap.Document.AddLayer(streetLayer);
            }
            else
            {
                fld = (IFeatureLayerDefinition)streetLayer;
                fld.DefinitionExpression = exp;
            }


            streets.Sort();

            foreach (string str in streets)
            {
                string[] parts = str.Split('-');

                TreeNode treeNode = new TreeNode(parts[0]);
                treeNode.Tag = "Chain";
                treeNode.Name = str;
                //treeNode.ContextMenuStrip = contextMenuStrip1;
                List<Range> ranges = Errors.CenterlineChains[str];
                foreach (Range r in ranges)
                {
                    if (r.Errors.Count > 0)
                    {
                        TreeNode child = new TreeNode(r.ID.ToString());
                        child.Tag = "Segment";
                        //child.ContextMenuStrip = contextMenuStrip1;

                        foreach (String s in r.Errors)
                        {
                            child.Nodes.Add(s);
                        }

                        treeNode.Nodes.Add(child);
                    }
                }

                if (treeNode.Nodes.Count > 0)
                {
                    error_treeview.Nodes.Add(treeNode);
                }
            }

            CheckFeaturesProgressBar.Visible = false;
        }

        void ccr_ProgressUpdate(object sender, EventArgs e)
        {
            CheckFeaturesProgressBar.Value = ccr.CurrentCount;
            CheckFeaturesProgressBar.Invalidate();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                IPolyline chain_geom = new PolylineClass();

                ISegmentCollection path1 = new PathClass();

                foreach (Range r in Errors.CenterlineChains[select_chain_name])
                {
                    ISegmentCollection segcoll = r.Shape as ISegmentCollection;
                    path1.AddSegmentCollection(segcoll);
                }

                object obj = Type.Missing;

                IGeometryCollection gCollection = chain_geom as IGeometryCollection;

                gCollection.AddGeometry((IGeometry)path1, obj, obj);

                chain_geom = (IPolyline)gCollection;

                FlashGeometry(chain_geom);


            }
            catch { }
        }

        private void zoomToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IFeature f = Globals.CenterlineLayer.FeatureClass.GetFeature(selected_error_oid);

                IEnvelope env = f.Extent;
                env.Expand(2, 2, true);

                ArcMap.Document.ActiveView.Extent = env;

                FlashGeometryAndZoom(f.Shape);
                
                
            }
            catch { }
        }

        public void FlashGeometryAndZoom(ESRI.ArcGIS.Geometry.IGeometry geometry)
        {
            IEnvelope env = geometry.Envelope;
            env.Expand(2, 2, true);

            ArcMap.Document.ActiveView.Extent = env;

            ArcMap.Document.ActiveView.Refresh();

            FlashGeometry(geometry);
        }

        public void FlashGeometry(ESRI.ArcGIS.Geometry.IGeometry geometry)
        {

            IRgbColor color = Globals.FlashFeatureColor;

            IDisplay display = (IDisplay)mDoc.ActiveView.ScreenDisplay;

            Int32 delay = Globals.FlashFeatureDelay;

            if (geometry == null || color == null || display == null)
            {
                return;
            }
            display.StartDrawing(display.hDC, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache); // Explicit Cast

            switch (geometry.GeometryType)
            {
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon:
                    {
                        //Set the flash geometry's symbol.
                        ESRI.ArcGIS.Display.ISimpleFillSymbol simpleFillSymbol = new ESRI.ArcGIS.Display.SimpleFillSymbolClass();
                        simpleFillSymbol.Color = color;
                        ESRI.ArcGIS.Display.ISymbol symbol = simpleFillSymbol as ESRI.ArcGIS.Display.ISymbol; // Dynamic Cast
                        symbol.ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPNotXOrPen;
                        //Flash the input polygon geometry.
                        display.SetSymbol(symbol);
                        display.DrawPolygon(geometry);
                        System.Threading.Thread.Sleep(delay);
                        display.DrawPolygon(geometry);
                        break;
                    }
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline:
                    {
                        //Set the flash geometry's symbol.
                        ESRI.ArcGIS.Display.ISimpleLineSymbol simpleLineSymbol = new ESRI.ArcGIS.Display.SimpleLineSymbolClass();
                        simpleLineSymbol.Width = 4;
                        simpleLineSymbol.Color = color;
                        ESRI.ArcGIS.Display.ISymbol symbol = simpleLineSymbol as ESRI.ArcGIS.Display.ISymbol; // Dynamic Cast
                        symbol.ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPNotXOrPen;
                        //Flash the input polyline geometry.
                        display.SetSymbol(symbol);
                        display.DrawPolyline(geometry);
                        System.Threading.Thread.Sleep(delay);
                        display.DrawPolyline(geometry);
                        break;
                    }
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint:
                    {
                        //Set the flash geometry's symbol.
                        ESRI.ArcGIS.Display.ISimpleMarkerSymbol simpleMarkerSymbol = new ESRI.ArcGIS.Display.SimpleMarkerSymbolClass();
                        simpleMarkerSymbol.Style = ESRI.ArcGIS.Display.esriSimpleMarkerStyle.esriSMSCircle;
                        simpleMarkerSymbol.Size = 12;
                        simpleMarkerSymbol.Color = color;
                        ESRI.ArcGIS.Display.ISymbol symbol = simpleMarkerSymbol as ESRI.ArcGIS.Display.ISymbol; // Dynamic Cast
                        symbol.ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPNotXOrPen;
                        //Flash the input point geometry.
                        display.SetSymbol(symbol);
                        display.DrawPoint(geometry);
                        System.Threading.Thread.Sleep(delay);
                        display.DrawPoint(geometry);
                        break;
                    }
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryMultipoint:
                    {
                        //Set the flash geometry's symbol.
                        ESRI.ArcGIS.Display.ISimpleMarkerSymbol simpleMarkerSymbol = new ESRI.ArcGIS.Display.SimpleMarkerSymbolClass();
                        simpleMarkerSymbol.Style = ESRI.ArcGIS.Display.esriSimpleMarkerStyle.esriSMSCircle;
                        simpleMarkerSymbol.Size = 12;
                        simpleMarkerSymbol.Color = color;
                        ESRI.ArcGIS.Display.ISymbol symbol = simpleMarkerSymbol as ESRI.ArcGIS.Display.ISymbol; // Dynamic Cast
                        symbol.ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPNotXOrPen;
                        //Flash the input multipoint geometry.
                        display.SetSymbol(symbol);
                        display.DrawMultipoint(geometry);
                        System.Threading.Thread.Sleep(delay);
                        display.DrawMultipoint(geometry);
                        break;
                    }
            }
            display.FinishDrawing();
        }

        private void low_range_tbx_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                int start_number = Convert.ToInt32(low_range_tbx.Text);
                IPolyline pline = CenterlineEditFeature.Shape as IPolyline;
                ICurve curve = pline as ICurve;
                double distance = curve.Length;

                double apu = Globals.AddressesPerUnit;
                double lengthIndex = Math.Abs(distance / apu);
                double max = start_number + lengthIndex;
                int cal_max_housenumber = Convert.ToInt32(Math.Round(max, 0));

                calc_high_range_tbx.Text = cal_max_housenumber.ToString();
            }
            catch{}


        }

        private void low_range_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
             int isNumber = 0;
             e.Handled = !int.TryParse(e.KeyChar.ToString(), out isNumber);
        }

        private void flashDelaycomboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                double interval = Double.Parse(flashDelaycomboBox.SelectedItem.ToString()) * 100;
                int int_int = Convert.ToInt32(interval);
                Globals.FlashFeatureDelay = int_int;
            }
            catch { }
        }

        private void flashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IFeature f = Globals.CenterlineLayer.FeatureClass.GetFeature(selected_error_oid);
                FlashGeometry(f.Shape);
            }
            catch { }
        }



    }
}
