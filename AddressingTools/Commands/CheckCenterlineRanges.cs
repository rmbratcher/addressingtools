using System;
using System.Collections.Generic;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;


namespace AddressingTools
{

    public class CheckCenterlineRanges// : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        //private Dictionary<string, List<Range>> CenterlineChains;
        //private List<string> ProcessingErrors;
        //private List<string> ChainErrors;
        private double tolerance = 0.01d;
        private CenterlineErrors Errors = new CenterlineErrors();

        private int _Numfeatures = 0;
        private int _CurrentCount = 0;
        private int _NumErrors = 0;

        public delegate void ProcessUpdateHandler(object sender, EventArgs e);
        public event ProcessUpdateHandler ProgressUpdate;

        public CheckCenterlineRanges()
        {
            QueryFilter qf = new QueryFilter();
            _Numfeatures = Globals.CenterlineLayer.FeatureClass.FeatureCount(qf);
        }

        private void OnProgressUpdate()
        {
            if (ProgressUpdate != null)
                ProgressUpdate(this, EventArgs.Empty);
        }

        public int NumFeatures
        {
            get { return _Numfeatures; }
        }

        public int CurrentCount
        {
            get { return _CurrentCount; }
        }

        public int NumErrors
        {
            get { return _NumErrors; }
        }

        private IFields CreateChainFields()
        {
            IFields flds = new FieldsClass();
            IFieldsEdit eflds = flds as IFieldsEdit;
            eflds.FieldCount_2 = 7;

            IField oid_field = new FieldClass();
            IFieldEdit eOid = (IFieldEdit)oid_field;
            eOid.Name_2 = "ObjectID";
            eOid.AliasName_2 = "ObjectID";
            eOid.Type_2 = esriFieldType.esriFieldTypeOID;

            eflds.set_Field(0, oid_field);

            //Create A geometry Definition for the layer
            IGeometryDef geomDef = new GeometryDefClass();
            IGeometryDefEdit egeomDef = (IGeometryDefEdit)geomDef;
            egeomDef.GeometryType_2 = esriGeometryType.esriGeometryPolyline;
            egeomDef.SpatialReference_2 = ArcMap.Document.FocusMap.SpatialReference;

            IField shap = new FieldClass();
            IFieldEdit eshape = (IFieldEdit)shap;
            eshape.Type_2 = esriFieldType.esriFieldTypeGeometry;
            eshape.Name_2 = "Shape";
            eshape.AliasName_2 = "Shape";
            eshape.GeometryDef_2 = geomDef;

            eflds.set_Field(1, shap);

            IField FromLeft = CreateIntegerField("FromLeft");

            eflds.set_Field(2, FromLeft);

            IField ToLeft = CreateIntegerField("ToLeft");

            eflds.set_Field(3, ToLeft);

            IField FromRight = CreateIntegerField("FromRight");

            eflds.set_Field(4, FromRight);

            IField ToRight = CreateIntegerField("ToRight");

            eflds.set_Field(5, ToRight);

            IField CLID = CreateIntegerField("CLID");

            eflds.set_Field(6, CLID);

            return flds;
        }

        private IField CreateIntegerField(string Name)
        {
            IField fkey = new FieldClass();
            IFieldEdit efkey = (IFieldEdit)fkey;
            efkey.Name_2 = Name;
            efkey.Type_2 = esriFieldType.esriFieldTypeInteger;
            efkey.AliasName_2 = Name;

            return fkey;
        }


        private IFields CreateErrorTableFields()
        {
            IFields flds = new FieldsClass();
            IFieldsEdit eflds = flds as IFieldsEdit;
            eflds.FieldCount_2 = 3;

            IField oid_field = new FieldClass();
            IFieldEdit eOid = (IFieldEdit)oid_field;
            eOid.Name_2 = "ObjectID";
            eOid.AliasName_2 = "ObjectID";
            eOid.Type_2 = esriFieldType.esriFieldTypeOID;

            eflds.set_Field(0, oid_field);

            IField fkey = new FieldClass();
            IFieldEdit efkey = (IFieldEdit)fkey;
            efkey.Name_2 = "FKEY";
            efkey.Type_2 = esriFieldType.esriFieldTypeInteger;
            efkey.AliasName_2 = "FKEY";

            eflds.set_Field(1, fkey);


            IField flderror = new FieldClass();
            IFieldEdit eflder = (IFieldEdit)flderror;
            eflder.AliasName_2 = "Error";
            eflder.Name_2 = "Error";
            eflder.Type_2 = esriFieldType.esriFieldTypeString;
            eflder.Length_2 = 255;

            eflds.set_Field(2, flderror);

            return flds;

        }

        public CenterlineErrors Check()
        {
            int rf = 2;

            _NumErrors = 0;

            switch (ArcMap.Document.FocusMap.DistanceUnits)
            {
                case ESRI.ArcGIS.esriSystem.esriUnits.esriCentimeters:
                    rf = 1;
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriDecimalDegrees:
                    rf = 7;
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriDecimeters:
                    rf = 3;
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriFeet:
                    rf = 1;
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriInches:
                    rf = 1;
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriKilometers:
                    rf = 5;
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriMeters:
                    rf = 1;
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriMiles:
                    rf = 5;
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriMillimeters:
                    rf = 1;
                    break;
                case ESRI.ArcGIS.esriSystem.esriUnits.esriNauticalMiles:
                    rf = 6;
                    break;
            }
            string s = "1";
            string stolerance = string.Format("0.{0}", s.PadLeft(rf, '0'));
            tolerance = Double.Parse(stolerance);



            IQueryFilter qf = new QueryFilterClass();
            _CurrentCount = 0;


            IFeatureCursor cursor = Globals.CenterlineLayer.Search(qf, false);
            

            Errors.CenterlineChains = new Dictionary<string, List<Range>>();
            Errors.ProcessingErrors = new List<string>();
            Errors.ChainErrors = new List<string>();
            Errors.SnappingErrors = new List<IPoint>();


            IFeature f = cursor.NextFeature();
            while (f != null)
            {
                try
                {
                    int id = f.OID;
                    int from_right = Convert.ToInt32(f.get_Value(Globals.CenterlineFields.FromRight));
                    int from_left = Convert.ToInt32(f.get_Value(Globals.CenterlineFields.FromLeft));
                    int to_right = Convert.ToInt32(f.get_Value(Globals.CenterlineFields.ToRight));
                    int to_left = Convert.ToInt32(f.get_Value(Globals.CenterlineFields.ToLeft));
                    IPolyline shape = (IPolyline)f.Shape;
                    string fullname = f.get_Value(Globals.CenterlineFields.FullStName).ToString();
                    if (Globals.CenterlineFields.EsnLeft > -1)
                        fullname += "-" +  f.get_Value(Globals.CenterlineFields.EsnLeft).ToString();
                    if (Globals.CenterlineFields.EsnRight > -1)
                        fullname += "-" + f.get_Value(Globals.CenterlineFields.EsnRight).ToString();

                    int min = Math.Min(from_right, from_left);
                    int max = Math.Max(to_left, to_right);

                    Range r = new Range(id, min, max, from_left, to_left, from_right, to_right, shape);
                    if (min > max)
                    {
                        r.Errors.Add(String.Format("Low > High : {0} - {1}", min, max));
                        _NumErrors++;
                    }

                    int high_delta = Math.Abs((to_right - to_left));
                    int low_delta = Math.Abs((from_right - from_left));

                    if (high_delta > 1)
                    {
                        r.Errors.Add(String.Format("Gap in To Range : {0} - {1} ", to_right, to_left));
                        _NumErrors++;
                    }
                    if (low_delta > 1)
                    {
                        r.Errors.Add(String.Format("Gap in From Range : {0} - {1}", from_right, from_left));
                        _NumErrors++;
                    }


                    if (Errors.CenterlineChains.ContainsKey(fullname))
                        Errors.CenterlineChains[fullname].Add(r);
                    else
                    {
                        Errors.CenterlineChains[fullname] = new List<Range>();
                        Errors.CenterlineChains[fullname].Add(r);
                    }
                }
                catch
                {
                    Errors.ProcessingErrors.Add(f.OID.ToString());
                }

                _CurrentCount += 1;

                if (_CurrentCount % 500 == 0)
                    OnProgressUpdate();

                f = cursor.NextFeature();
            }

            ProcessChains();

            List<string> names = new List<string>();

            foreach (KeyValuePair<string, List<Range>> kvp in Errors.CenterlineChains)
            {

                for (int i = 0; i < kvp.Value.Count; i++)
                {
                    Range r = kvp.Value[i];
                    if (r.Errors.Count < 1)
                        kvp.Value.Remove(r);
                }

                if (kvp.Value.Count < 1)
                    names.Add(kvp.Key);
            }

            foreach (string n in names)
            {
                Errors.CenterlineChains.Remove(n);
            }

            return Errors;
        }

        private void ProcessChains()
        {
            foreach (string key in Errors.CenterlineChains.Keys)
            {
                try
                {
                    Errors.CenterlineChains[key].Sort(delegate(Range r1, Range r2) { return r1.Low.CompareTo(r2.Low); });
                }
                catch { }
            }
            foreach (KeyValuePair<string, List<Range>> kvp in Errors.CenterlineChains)
            {
                try
                {
                    CheckGeometryOrder(kvp.Value);
                }
                catch { }
            }
        }

        private bool Snaped(IPoint p1, IPoint p2)
        {

            double deltax = Math.Abs((p1.X - p2.X));
            double deltay = Math.Abs((p1.Y - p2.Y));

            return deltax < tolerance && deltay < tolerance;
        }

        private double Distance(IPoint p1, IPoint p2)
        {
            return Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
        }

        private int GetNextSegment(int i, List<Range> chain)
        {
            int nearest = i;
            double dist = double.MaxValue;

            for (int j = 0; j < chain.Count; j++)
            {
                if (j != i)
                {
                    if (Distance(chain[i].Shape.ToPoint, chain[j].Shape.FromPoint) < dist)
                    {
                        dist = Distance(chain[i].Shape.ToPoint, chain[j].Shape.FromPoint);
                        nearest = j;
                    }
                }
            }



            return nearest;
        }



        private List<Range> SortChain(List<Range> chain)
        {
            int StartSegment = -1;
            SortedDictionary<int, double> order = new SortedDictionary<int, double>();
            List<Range> nChain = new List<Range>();

            double minDistance = double.MinValue;
            

            for (int i = 0; i < chain.Count; i++)
            {
                Range test = chain[i];

                //the largest minimum distance from a startpoint to every other endpoint.
               double maxMinDist = double.MaxValue;

                // loop through all the other segments and mesure the distance from the 
                //test start point to x endpoint and return the minum distance
                for (int j = 0; j < chain.Count; j++)
                {

                    if (j != i)
                    {
                        double dist = Distance(test.Shape.FromPoint, chain[j].Shape.ToPoint);
                        if (dist < maxMinDist)
                        {
                            maxMinDist = dist;
                        }
                    }
                }

                if (maxMinDist > minDistance)
                {
                    StartSegment = i;
                    minDistance = maxMinDist;
                }

            }

            // Add the start segment to the new chain
            nChain.Add(chain[StartSegment]);

            int lastSegment = StartSegment;

            for (int k = 0; k < chain.Count - 1; k++)
            {
                int b = GetNextSegment(lastSegment, chain);
                nChain.Add(chain[b]);
                lastSegment = b;
            }


            //List<KeyValuePair<int, double>> mylist = new List<KeyValuePair<int, double>>();

            //int lastSegment = StartSegment;
            
            //for (int h = 0; h < chain.Count; h++)
            //{
            //    if (h != lastSegment)
            //    {
            //        KeyValuePair<int, double> item = new KeyValuePair<int, double>(h, Distance(chain[StartSegment].Shape.FromPoint, chain[h].Shape.ToPoint));
            //        mylist.Add(item);
            //    }
            //}

            
            //mylist.Sort(
            //    delegate(KeyValuePair<int, double> a, KeyValuePair<int, double> b)
            //    {
            //        return a.Value.CompareTo(b.Value);
            //    }
            //    );

            //foreach (KeyValuePair<int, double> kvp in mylist)
            //{
            //    nChain.Add(chain[kvp.Key]);
            //}

            return nChain;
        }

        private void CheckGeometryOrder(List<Range> list1)
        {
            List<Range> list = null;
            if (list1.Count > 1)
                list = SortChain(list1);
            else
                return;

            IPoint last_start = null;
            IPoint last_end = null;
            IPoint start_point = null;
            IPoint end_point = null;
            int last_high = 0;
            int last_low = 0;
            string last_oid = "0";
            bool left_odd = true;

            int count = 0;

            if (list.Count > 1)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (i != list.Count - 1)
                    {
                        double d = Distance(list[i].Shape.ToPoint, list[i + 1].Shape.FromPoint);
                        if (d > tolerance)
                        {
                            //Errors.SnappingErrors.Add(list[i].Shape.ToPoint);
                            list[i].Errors.Add("Endpoint Not Snapped");
                            _NumErrors++;
                            //list[i + 1].Errors.Add("Startpoint Not Snapped");
                        }
                    }
                }
            }
            foreach (Range r in list)
            {


                if (count == 0)
                {
                    count += 1;
                    start_point = r.Shape.FromPoint;
                    end_point = r.Shape.ToPoint;
                    last_start = start_point;
                    last_end = end_point;
                    last_high = r.High;
                    last_low = r.Low;
                    last_oid = r.ID.ToString();
                    left_odd = r.FromLeft % 2 == 0 ? false : true;
                }

                else
                {
                    start_point = r.Shape.FromPoint;
                    end_point = r.Shape.ToPoint;

                    if (!Snaped(start_point, last_end))
                    {
                        if (Snaped(end_point, last_end))
                        {
                            r.Errors.Add("Segment Flipped");
                            _NumErrors++;
                        }
                        else
                            if (count < (list.Count - 1))
                            {
                                r.Errors.Add("Segment Not Connected to Other Segments");
                                _NumErrors++;
                            }
                    }

                    int delta_range = r.Low - last_high;

                    bool this_odd = r.FromLeft % 2 == 0 ? false : true;

                    if (!this_odd == left_odd)
                    {
                        r.Errors.Add("Even/Odd Flipped");
                        _NumErrors++;
                    }

                    if (delta_range != 1)
                    {
                        r.Errors.Add(String.Format("Gap in Range Between ObjectId {0} and ObjectId {1}", last_oid, r.ID.ToString()));
                        _NumErrors++;
                    }

                    if (r.Errors.Count > 0)
                        Errors.ChainErrors.Add(r.ID.ToString());

                    last_end = end_point;
                    last_start = start_point;
                    last_high = r.High;
                    last_low = r.Low;
                    last_oid = r.ID.ToString();
                }

            }
        }

        private List<Range> OrderChain(List<Range> list)
        {
            list.Sort(delegate(Range r1, Range r2) { return r1.Low.CompareTo(r2.Low); });
            return list;
        }

        //protected override void OnUpdate()
        //{
        //    Enabled = Globals.CenterlineLayer == null ? false : true;
        //}
    }

}
