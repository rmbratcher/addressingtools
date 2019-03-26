using System.Collections.Generic;
using ESRI.ArcGIS.Geometry;

namespace AddressingTools
{
    public class CenterlineErrors
    {
        private Dictionary<string, List<Range>> _CenterlineChains;
        private List<string> _ProcessingErrors;
        private List<string> _ChainErrors;
        private List<IPoint> _SnappingErrors;

        public Dictionary<string, List<Range>> CenterlineChains
        {
            get { return _CenterlineChains; }
            set { _CenterlineChains = value; }
        }
        public List<string> ProcessingErrors
        {
            get { return _ProcessingErrors; }
            set { _ProcessingErrors = value; }
        }
        public List<string> ChainErrors
        {
            get { return _ChainErrors; }
            set { _ChainErrors = value; }
        }

        public List<IPoint> SnappingErrors
        {
            get { return _SnappingErrors; }
            set { _SnappingErrors = value; }
        }
    }

    public struct Chain
    {
        public int ChainID;
        public string Name;
        public List<Range> Segemnts;
    }

    public struct Range
    {
        public int ID;
        public int Low;
        public int High;
        public int FromRight;
        public int ToRight;
        public int FromLeft;
        public int ToLeft;
        public IPolyline Shape;
        public List<string> Errors;

        public Range(int id, int low, int high, int fromleft, int toleft, int fromright, int toright, IPolyline shape)
        {
            ID = id;
            Low = low;
            High = high;
            FromLeft = fromleft;
            ToLeft = toleft;
            FromRight = fromright;
            ToRight = toright;
            Shape = shape;
            Errors = new List<string>();
        }
    }
}
