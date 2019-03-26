using System;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace AddressingTools
{
    public class SelectCenterlineTool : ESRI.ArcGIS.Desktop.AddIns.Tool
    {
        private const string TAG = "AddressingTools.SelectCenterlineTool";

        private AddressingTools.Utils.agdErrorLogger log = new Utils.agdErrorLogger("errors.agd.cc");

        public SelectCenterlineTool()
        {
        }

        protected override void OnMouseUp(ESRI.ArcGIS.Desktop.AddIns.Tool.MouseEventArgs arg)
        {
            base.OnMouseUp(arg);

            try
            {

                IPoint p = ArcMap.Document.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(arg.X, arg.Y);

                ITopologicalOperator to = (ITopologicalOperator)p;
                IGeometry poly = to.Buffer(20.0d);

                ISpatialFilter sp = new SpatialFilterClass();
                sp.Geometry = poly;
                sp.GeometryField = Globals.CenterlineLayer.FeatureClass.ShapeFieldName;
                sp.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                sp.SearchOrder = esriSearchOrder.esriSearchOrderSpatial;

                ArcMap.Document.FocusMap.ClearSelection();

                IFeatureSelection fsel = (IFeatureSelection)Globals.CenterlineLayer;
                fsel.SelectFeatures(sp, esriSelectionResultEnum.esriSelectionResultNew, true);

                ISelectionSet selset = fsel.SelectionSet;
                if (selset.Count > 0)
                {
                    IEnumIDs pEnumIDs = selset.IDs;
                    pEnumIDs.Reset();
                    int oid = pEnumIDs.Next();

                    if (oid > -1)
                    {
                        Globals.SelectedCenterlineID = oid;
                        ArcMap.Document.ActiveView.Refresh();
                    }
                }

            }
            catch (Exception ex) { log.WriteError(ex, TAG, System.Security.Principal.WindowsIdentity.GetCurrent().Name, null); }

        }

        protected override void OnUpdate()
        {
            if (Globals.CenterlineLayer != null && ArcMap.Editor.EditState == ESRI.ArcGIS.Editor.esriEditState.esriStateEditing && Globals.IsLicensed)
                Enabled = true;
            else
                Enabled = false;
        }
    }

}
