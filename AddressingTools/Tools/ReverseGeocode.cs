using System;
using System.Windows.Forms;
using AddressingTools.Utils;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Editor;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Collections.Generic;

namespace AddressingTools
{
    public class ReverseGeocode : ESRI.ArcGIS.Desktop.AddIns.Tool
    {

        private const string TAG = "AddressingTools.ReverseGeocode";

        private IFeature SelectedPoint = null;
        private IFeature Centerline = null;
        private IPoint intersectionPoint = null;
        private int HouseNumber = 0;
        private IPolyline trackgeom = null;
        private INewLineFeedback LineFeedBack = null;
        private IPoint CurrentPos = null;
        private IGraphicsContainer graphicsContainer = null;
        private IMap mMap = null;
        private IMxDocument mDoc = null;
        private IEditor mEditor;
        private agdErrorLogger log;
        private IActiveView mActiveView;
        private IActiveViewEvents_Event mActiveViewEvents;
        private List<BetterMarker> mMarkers = new List<BetterMarker>();

        public ReverseGeocode()
        {
            mDoc = (IMxDocument)ArcMap.Application.Document;
            mMap = mDoc.FocusMap;
            mActiveView = mDoc.ActiveView;
            mActiveViewEvents = mActiveView as IActiveViewEvents_Event;
            mActiveViewEvents.AfterDraw += new IActiveViewEvents_AfterDrawEventHandler(mActiveViewEvents_AfterDraw);
            graphicsContainer = mDoc.ActiveView as IGraphicsContainer;
            log = new agdErrorLogger("errors.agd.cc");
        }

        void mActiveViewEvents_AfterDraw(IDisplay Display, esriViewDrawPhase phase)
        {
            if (phase != esriViewDrawPhase.esriViewGraphics)
            {
                graphicsContainer.DeleteAllElements();


                foreach (BetterMarker marker in mMarkers)
                {
                    marker.Update(this.mMap.MapScale);
                    IElement element = marker.MarkerElement;
                    graphicsContainer.AddElement(element, 0);
                    element.Activate(mDoc.ActiveView.ScreenDisplay);

                    mDoc.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, element, null);
                }
            }
        }

        protected override void OnUpdate()
        {
            if (mEditor == null)
                mEditor = ArcMap.Editor;

            if (ArcMap.Application != null &&  mEditor != null && Globals.IsLicensed)
            {
                Enabled = mEditor.EditState == ESRI.ArcGIS.Editor.esriEditState.esriStateEditing;
            }
            else
            {
                Enabled = false;
            }
        }

        protected override void OnMouseMove(ESRI.ArcGIS.Desktop.AddIns.Tool.MouseEventArgs arg)
        {
            base.OnMouseMove(arg);

            try
            {

                if (LineFeedBack != null)
                {
                    IDisplay display = mDoc.ActiveView.ScreenDisplay;
                    IPoint p = mDoc.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(arg.X, arg.Y);
                    LineFeedBack.MoveTo(p);
                }

                CurrentPos = mDoc.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(arg.X, arg.Y);
            }
            catch { }
        }

        protected override void OnDoubleClick()
        {
            base.OnDoubleClick();


            try
            {
                if (LineFeedBack != null)
                {
                    trackgeom = LineFeedBack.Stop();
                    LineFeedBack = null;

                    if (GetDriveCenterlineIntersection(trackgeom))
                    {
                        if (CalculateHouseNumber())
                        {
                            SetAttributes();
                        }
                    }

                    mMap.ClearSelection();
                    ITopologicalOperator to = SelectedPoint.Shape as ITopologicalOperator;
                    IGeometry g = to.Buffer(10);
                    mMap.SelectByShape(g, ArcMap.ThisApplication.SelectionEnvironment, true);
                    IFeatureSelection fsel = Globals.AddressPointLayer as IFeatureSelection;


                    ArcMap.Document.FocusMap.ClearSelection();
                    ArcMap.Document.ActiveView.Refresh();
                    ArcMap.Document.FocusMap.ClearSelection();
                }
            }
            catch { }
        }

        protected override void OnKeyUp(ESRI.ArcGIS.Desktop.AddIns.Tool.KeyEventArgs arg)
        {
            base.OnKeyUp(arg);

            try
            {

                if (arg.KeyCode == Keys.A)
                {

                    int hn = CalculateHouseNumber(CurrentPos);
                    if (hn > 0)
                    {
                        AddCallout(intersectionPoint, hn.ToString());
                        //ShowCallout(CurrentPos, hn.ToString());
                    }
                }
                if (arg.KeyCode == Keys.E)
                {
                    RemoveCallouts();
                }
            }
            catch { }
        }

        private void RemoveCallouts()
        {
            mMarkers.Clear();
            mDoc.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, new object(), mActiveView.Extent);
        }

        private void AddCallout(IPoint CurrentPos, string p)
        {
            BetterMarker marker = new BetterMarker(CurrentPos, p, mMap.MapScale);
            IElement element = marker.MarkerElement;
            graphicsContainer.AddElement(element, 0);
            element.Activate(mDoc.ActiveView.ScreenDisplay);

            mMarkers.Add(marker);

            mDoc.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, element, null);

            mMap.ClearSelection();

        }

        protected override void OnMouseUp(MouseEventArgs arg)
        {
            base.OnMouseUp(arg);
            try
            {

                IPoint p = mDoc.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(arg.X, arg.Y);
                if (LineFeedBack == null)
                {
                    ITopologicalOperator to = (ITopologicalOperator)p;
                    double buffsize = .01 * mMap.MapScale;
                    IGeometry g = to.Buffer(buffsize);
                    FlashGeometry(g);

                    mMap.SelectByShape(g, ArcMap.ThisApplication.SelectionEnvironment, true);

                    if (mMap.SelectionCount > 0)
                    {
                        IFeatureSelection fsel = Globals.AddressPointLayer as IFeatureSelection;
                        ISelectionSet selset = fsel.SelectionSet;
                        if (selset.Count == 1)
                        {
                            IEnumIDs pEnum = selset.IDs;
                            int oid = pEnum.Next();
                            SelectedPoint = Globals.AddressPointLayer.FeatureClass.GetFeature(oid);
                            mDoc.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                            //IPolyline pline = GetPolylineFromMouseClicks();

                            LineFeedBack = CreateLineFeedBack();
                            LineFeedBack.Start(p);

                            //if (GetDriveCenterlineIntersection(pline))
                            //{
                            //    if (CalculateHouseNumber())
                            //    {
                            //        SetAttributes();
                            //    }
                            //}
                        }

                        //mMap.ClearSelection();
                        //mMap.SelectByShape(g, ArcMap.ThisApplication.SelectionEnvironment, true);
                        //mDoc.ActiveView.Refresh();
                    }
                    else
                    {
                        if (arg.ModifierKeys == Keys.Control)
                        {
                            IFeature fx1 = Globals.AddressPointLayer.FeatureClass.CreateFeature();
                            fx1.Shape = p;
                            fx1.Store();
                            int fxOID = fx1.OID;

                            ArcMap.Document.ActiveView.Selection.Clear();

                            mMap.SelectFeature(Globals.AddressPointLayer as ILayer, fx1);

                            SelectedPoint = Globals.AddressPointLayer.FeatureClass.GetFeature(fxOID);

                            mDoc.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                            //IPolyline pline = GetPolylineFromMouseClicks();

                            LineFeedBack = CreateLineFeedBack();
                            LineFeedBack.Start(p);
                        }

                    }
                }
                else
                {
                    LineFeedBack.AddPoint(p);
                }
            }
            catch { }

        }


        private INewLineFeedback CreateLineFeedBack()
        {
            try
            {
                INewLineFeedback lfb = new NewLineFeedbackClass();

                lfb.Symbol = Globals.getFeedBackLineSymbol(lfb);
                lfb.Display = mDoc.ActiveView.ScreenDisplay;

                return lfb;
            }
            catch { return null; }
        }


        private void SetAttributes()
        {
            mEditor.StartOperation();
            try
            {

                SelectedPoint.set_Value(Globals.AddressPointFields.HouseNumber, HouseNumber);
                MoveAttr(Globals.CenterlineFields.StName, Globals.AddressPointFields.StName);
                MoveAttr(Globals.CenterlineFields.StPreDir, Globals.AddressPointFields.StPreDir);
                MoveAttr(Globals.CenterlineFields.StSufDir, Globals.AddressPointFields.StSufDir);
                MoveAttr(Globals.CenterlineFields.StType, Globals.AddressPointFields.StType);
                MoveAttr(Globals.CenterlineFields.Clid, Globals.AddressPointFields.Clid);
                MoveAttr(Globals.CenterlineFields.StTypeSfx, Globals.AddressPointFields.StTypeSfx);
                MoveAttr(Globals.CenterlineFields.ZipCode, Globals.AddressPointFields.ZipCode);
                MoveAttr(Globals.CenterlineFields.Community, Globals.AddressPointFields.Community);

                try
                {
                    if (Globals.AddressPointFields.Editor > -1)
                    {
                        string usr = System.Windows.Forms.SystemInformation.UserName;
                        SelectedPoint.set_Value(Globals.AddressPointFields.Editor, usr);
                    }

                    if (Globals.AddressPointFields.Modified > -1)
                    {
                        object ts;
                        if (SelectedPoint.Fields.get_Field(Globals.AddressPointFields.Modified).Type == esriFieldType.esriFieldTypeDate)
                            ts = DateTime.Now;
                        else
                            ts = DateTime.Now.ToString();

                        SelectedPoint.set_Value(Globals.AddressPointFields.Modified, ts);
                    }
                }
                catch(Exception ex)
                {
                    log.WriteError(ex, TAG, System.Security.Principal.WindowsIdentity.GetCurrent().Name, null);
                }


                if (Globals.AddressPointFields.FullAddress > -1)
                {
                    string fulladdr_x = string.Format("{0} {1} {2} {3} {4}", HouseNumber, Centerline.get_Value(Globals.CenterlineFields.StPreDir).ToString(),
                                                                                        Centerline.get_Value(Globals.CenterlineFields.StName).ToString(),
                                                                                        Centerline.get_Value(Globals.CenterlineFields.StType).ToString(),
                                                                                        Centerline.get_Value(Globals.CenterlineFields.StSufDir).ToString());

                    string fulladdr = fulladdr_x.Replace("  ", " ");
                    fulladdr.Replace("  ", " ");
                    fulladdr.Trim();

                    SelectedPoint.set_Value(Globals.AddressPointFields.FullAddress, fulladdr);
                }

                SelectedPoint.Store();
            }
            catch(Exception ex)
            {
                log.WriteError(ex, TAG, System.Security.Principal.WindowsIdentity.GetCurrent().Name, null);
                MessageBox.Show(string.Format("House Number: {0}", HouseNumber), "Unable to store point");
            }

            try
            {
                mEditor.StopOperation("Add Address to Point");
            }
            catch (Exception ex)
            {
                log.WriteError(ex, TAG, System.Security.Principal.WindowsIdentity.GetCurrent().Name, null);
            }
        }

        public bool MoveAttr(int from, int to)
        {
            try
            {
                IField from_field = SelectedPoint.Fields.get_Field(to);
                IField to_field = Centerline.Fields.get_Field(from);

                if (from_field.Type == esriFieldType.esriFieldTypeString)
                {
                    if (to_field.Length > from_field.Length)
                    {
                        string val = Centerline.get_Value(from).ToString();
                        if (val.Length > from_field.Length)
                        {
                            string val1 = val.Substring(0, from_field.Length - 1);
                            SelectedPoint.set_Value(to, val1);
                            return true;
                        }
                        else
                        {
                            SelectedPoint.set_Value(to, val);
                            return true;
                        }
                    }
                    else
                    {
                        string val = Centerline.get_Value(from).ToString();
                        SelectedPoint.set_Value(to, val);
                        return true;
                    }

                }
                else
                {
                    object val = Centerline.get_Value(from);
                    SelectedPoint.set_Value(to, val);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, TAG, System.Security.Principal.WindowsIdentity.GetCurrent().Name, null);
                return false;
            }
        }



        private void ShowCallout(IPoint loc, string text)
        {
            try
            {

                ITextElement pTextElement = new TextElementClass();
                IElement pElement;
                IBalloonCallout pCallout = new BalloonCalloutClass();
                IFillSymbol pFill = new SimpleFillSymbolClass();
                ILineSymbol pLine = new SimpleLineSymbolClass();
                IFormattedTextSymbol pLabelSymbol = new TextSymbolClass();

                IRgbColor c = new RgbColorClass();
                c.Red = 0;
                c.Green = 0;
                c.Blue = 0;

                IRgbColor d = new RgbColorClass();
                d.Red = 255;
                d.Green = 255;
                d.Blue = 255;

                IRgbColor e = new RgbColorClass();
                e.Red = 205;
                e.Green = 192;
                e.Blue = 176;

                pLine.Color = c;
                pFill.Color = d;
                pFill.Outline = pLine;

                pCallout.Symbol = pFill;
                pCallout.Style = esriBalloonCalloutStyle.esriBCSRoundedRectangle;
                pCallout.AnchorPoint = loc;

                pLabelSymbol.Background = pCallout as ITextBackground;
                pLabelSymbol.Size = 10.5d;
                pLabelSymbol.ShadowColor = e;
                pLabelSymbol.ShadowXOffset = 1.0d;
                pLabelSymbol.ShadowYOffset = 1.0d;

                pTextElement.Text = text;
                pTextElement.Symbol = pLabelSymbol as ITextSymbol;

                pElement = pTextElement as IElement;
                double delta = (.1 * mMap.MapScale) / 2;
                //switch (mMap.MapScale)
                //{
                //    case 
                //}

                IPoint p1 = new PointClass();
                p1.X = loc.X + delta;
                p1.Y = loc.Y + delta;


                pElement.Geometry = p1;



                graphicsContainer = mDoc.ActiveView as IGraphicsContainer;

                graphicsContainer.AddElement(pElement, 0);
                pElement.Activate(mDoc.ActiveView.ScreenDisplay);

                mDoc.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, pElement, null);
                mMap.ClearSelection();

                Timer t = new Timer();
                t.Interval = 2000;
                t.Tick += new EventHandler(t_Tick);
                t.Start();
            }
            catch(Exception ex){ log.WriteError(ex, TAG, System.Security.Principal.WindowsIdentity.GetCurrent().Name, null); }

        }

        void t_Tick(object sender, EventArgs e)
        {
            try
            {
                Timer x = sender as Timer;
                x.Stop();
                x.Dispose();
                graphicsContainer.DeleteAllElements();
                mMap.ClearSelection();
                mDoc.ActiveView.Refresh();
            }
            catch { }
        }

        public void FlashGeometry(ESRI.ArcGIS.Geometry.IGeometry geometry)
        {
            try
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
            catch (Exception ex) { log.WriteError(ex, TAG, System.Security.Principal.WindowsIdentity.GetCurrent().Name, null); }
        }

        private bool GetDriveCenterlineIntersection(IPolyline drive)
        {
            try
            {
                ISpatialFilter sp = new SpatialFilterClass();
                sp.Geometry = drive;
                sp.GeometryField = Globals.CenterlineLayer.FeatureClass.ShapeFieldName;
                sp.SearchOrder = esriSearchOrder.esriSearchOrderSpatial;
                sp.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                IFeatureSelection fs = (IFeatureSelection)Globals.CenterlineLayer;
                fs.SelectFeatures(sp, esriSelectionResultEnum.esriSelectionResultNew, true);
                ISelectionSet selset = fs.SelectionSet;
                if (selset.Count < 1)
                {
                    return false;
                }
                else
                {
                    IEnumIDs penum = selset.IDs;
                    int xid = penum.Next();
                    IFeature f = Globals.CenterlineLayer.FeatureClass.GetFeature(xid);
                    Centerline = f;
                    ITopologicalOperator to = (ITopologicalOperator)drive;
                    IGeometry g = to.Intersect(f.Shape, esriGeometryDimension.esriGeometry0Dimension);
                    IMultipoint mp = (IMultipoint)g;
                    IPointCollection pc = (IPointCollection)mp;
                    IPoint p = pc.get_Point(0);
                    //IPoint p = (IPoint)g;
                    intersectionPoint = p;
                    return true;

                }
            }
            catch(Exception ex)
            {
                log.WriteError(ex, TAG, System.Security.Principal.WindowsIdentity.GetCurrent().Name, null);
                return false;
            }
        }

        private int CalculateHouseNumber(IPoint p)
        {
            try
            {
                ITopologicalOperator to = (ITopologicalOperator)p;
                double buffsize = .01 * mDoc.FocusMap.MapScale;
                IGeometry g = to.Buffer(buffsize);
                FlashGeometry(g);

                mMap.SelectByShape(g, ArcMap.ThisApplication.SelectionEnvironment, true);

                IFeatureSelection fsel = Globals.CenterlineLayer as IFeatureSelection;
                if (fsel.SelectionSet.Count > 0)
                {
                    IEnumIDs pEnum = fsel.SelectionSet.IDs;
                    int oid = pEnum.Next();
                    IFeature feat = Globals.CenterlineLayer.FeatureClass.GetFeature(oid);
                    Centerline = feat;

                    IPolyline pline = feat.Shape as IPolyline;
                    ICurve curve = pline as ICurve;
                    esriSegmentExtension segEx = esriSegmentExtension.esriNoExtension;
                    ITopologicalOperator to2 = g as ITopologicalOperator;
                    IMultipoint mp = to2.Intersect(curve, esriGeometryDimension.esriGeometry0Dimension) as IMultipoint;
                    IPointCollection pc = mp as IPointCollection;

                    IPoint ObservationPoint = p;
                    IPoint inPoint = pc.get_Point(0);
                    IPoint outPoint = new PointClass();
                    double distance = 0.0d;
                    double distancefrom = 0.0d;
                    bool onright = false;
                    curve.QueryPointAndDistance(segEx, ObservationPoint, false, outPoint, ref distance, ref distancefrom, ref onright);


                    intersectionPoint = outPoint;
                    //double a = GetAreaOfTriangle(pline.FromPoint, pline.ToPoint, inPoint, outPoint, distancefrom);
                    //double a = GetThreePointAngle(pline.FromPoint.X, pline.FromPoint.Y, pline.ToPoint.X, pline.ToPoint.Y, inPoint.X, inPoint.Y);
                    //onright = (a > 0);

                    int start_number = 0;
                    int n1 = Convert.ToInt32(Centerline.get_Value(Globals.CenterlineFields.FromLeft).ToString());
                    int n2 = Convert.ToInt32(Centerline.get_Value(Globals.CenterlineFields.FromRight).ToString());
                    start_number = Math.Min(n1, n2);

                    int n3 = Convert.ToInt32(Centerline.get_Value(Globals.CenterlineFields.ToLeft).ToString());
                    int n4 = Convert.ToInt32(Centerline.get_Value(Globals.CenterlineFields.ToRight).ToString());
                    int end_number = 0;
                    end_number = Math.Max(n3, n4);

                    int total_addresses = end_number - start_number;

                    double apu = curve.Length / total_addresses;
                    double lengthIndex = Math.Abs(distance / apu);
                    int housenumberstoadd = Convert.ToInt32(Math.Round(lengthIndex, 0));

                    IPolyline pline1 = (IPolyline)feat.Shape;

                    FlashGeometry(ObservationPoint);


                    int hn = 0;

                    if (onright == false)
                    {
                        int x = start_number + housenumberstoadd;
                        if (x % 2 == 0)
                        {
                            hn = x - 1;
                        }
                        else
                        {
                            hn = x;
                        }
                    }
                    else
                    {
                        int x = start_number + housenumberstoadd;
                        if (x % 2 == 0)
                        {
                            hn = x;
                        }
                        else
                        {
                            hn = x - 1;
                        }
                    }

                    return hn;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                log.WriteError(ex, TAG, System.Security.Principal.WindowsIdentity.GetCurrent().Name, null);
                return 0;
            }

        }

        private bool CalculateHouseNumber()
        {
            try
            {
                IPolyline pline = Centerline.Shape as IPolyline;
                ICurve curve = pline as ICurve;
                esriSegmentExtension segEx = esriSegmentExtension.esriNoExtension;
                IFeature f = SelectedPoint;
                IPoint ObservationPoint = trackgeom.ToPoint;
                IPoint inPoint = intersectionPoint;
                IPoint outPoint = new PointClass();
                double distance = 0.0d;
                double distancefrom = 0.0d;
                bool onright = false;
                curve.QueryPointAndDistance(segEx, ObservationPoint, false, outPoint, ref distance, ref distancefrom, ref onright);


                //double a = GetAreaOfTriangle(pline.FromPoint, pline.ToPoint, inPoint, outPoint, distancefrom);
                //double a = GetThreePointAngle(pline.FromPoint.X, pline.FromPoint.Y, pline.ToPoint.X, pline.ToPoint.Y, inPoint.X, inPoint.Y);
                //onright = (a > 0);

                int start_number = 0;
                int n1 = Convert.ToInt32(Centerline.get_Value(Globals.CenterlineFields.FromLeft).ToString());
                int n2 = Convert.ToInt32(Centerline.get_Value(Globals.CenterlineFields.FromRight).ToString());
                start_number = Math.Min(n1, n2);

                int n3 = Convert.ToInt32(Centerline.get_Value(Globals.CenterlineFields.ToLeft).ToString());
                int n4 = Convert.ToInt32(Centerline.get_Value(Globals.CenterlineFields.ToRight).ToString());
                int end_number = 0;
                end_number = Math.Max(n3, n4);

                int total_addresses = end_number - start_number;

                double apu = curve.Length / total_addresses;
                double lengthIndex = Math.Abs(distance / apu);
                int housenumberstoadd = Convert.ToInt32(Math.Round(lengthIndex, 0));



                IPolyline p = (IPolyline)Centerline.Shape;

                FlashGeometry(ObservationPoint);

                if (onright)
                {
                    int x = start_number + housenumberstoadd;
                    if (x % 2 == 0)
                    {
                        HouseNumber = x - 1;
                        return true;
                    }
                    else
                    {
                        HouseNumber = x;
                        return true;
                    }
                }
                else
                {
                    int x = start_number + housenumberstoadd;
                    if (x % 2 == 0)
                    {
                        HouseNumber = x;
                        return true;
                    }
                    else
                    {
                        HouseNumber = x - 1;
                        return true;
                    }
                }
                //return start_number + housenumberstoadd;
            }
            catch (Exception ex)
            {
                log.WriteError(ex, TAG, System.Security.Principal.WindowsIdentity.GetCurrent().Name, null);
                MessageBox.Show(ex.Message);
                return false;
            }
        }

    }

}
