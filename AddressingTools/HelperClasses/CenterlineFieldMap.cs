using System.Collections.Generic;

namespace AddressingTools
{
    public class CenterlineFieldMap : IFieldMap
    {
        #region Private
        private int fromleft = -2;
        private int toleft = -2;
        private int fromright = -2;
        private int toright = -2;
        private int stpredir = -2;
        private int sttype = -2;
        private int stname = -2;
        private int stsufdir = -2;
        private int clid = -2;
        private int fullstname = -2;
        private int esnleft = -2;
        private int esnright = -2;
        private int editor = -2;
        private int modified = -2;
        private int sttypesfx = -2;
        private int zipcode = -2;
        private int community = -2;
        #endregion

        public int StTypeSfx
        {
            get { return sttypesfx; }
            set { sttypesfx = value; }
        }

        public int FromLeft
        {
            get { return fromleft; }
            set { fromleft = value; }
        }
        public int ToLeft
        {
            get { return toleft; }
            set { toleft = value; }
        }
        public int FromRight
        {
            get { return fromright; }
            set { fromright = value; }
        }
        public int ToRight
        {
            get { return toright; }
            set { toright = value; }
        }
        public int StPreDir
        {
            get { return stpredir; }
            set { stpredir = value; }
        }
        public int StType
        {
            get { return sttype; }
            set { sttype = value; }
        }
        public int StName
        {
            get { return stname; }
            set { stname = value; }
        }
        public int StSufDir
        {
            get { return stsufdir; }
            set { stsufdir = value; }
        }
        public int Clid
        {
            get { return clid; }
            set { clid = value; }
        }
        public int FullStName
        {
            get { return fullstname; }
            set { fullstname = value; }
        }
        public int EsnLeft
        {
            get { return esnleft; }
            set { esnleft = value; }
        }

        public int EsnRight
        {
            get { return esnright; }
            set { esnright = value; }
        }

        public int Editor
        {
            get { return editor; }
            set { editor = value; }
        }

        public int Modified
        {
            get { return modified; }
            set { modified = value; }
        }

        public int ZipCode
        {
            get { return zipcode; }
            set { zipcode = value; }
        }

        public int Community
        {
            get { return community; }
            set { community = value; }
        }


        public bool AllSet()
        {
            if (FromLeft > -1 && ToLeft > -1 && FromRight > -1 && ToRight > -1 && StPreDir > -1 && StName > -1 && StSufDir > -1 && Clid > -1 && FullStName > -1)
                return true;
            else
                return false;
        }

        public bool ManditorySet()
        {
            if (Clid > -1)
                return true;
            return false;
        }

        public List<string> GetUnset()
        {
            List<string> unset = new List<string>();
            if (FromLeft == -2)
                unset.Add("FromLeft");
            if (ToLeft == -2)
                unset.Add("ToLeft");
            if (FromRight == -2)
                unset.Add("FromRight");
            if (ToRight == -2)
                unset.Add("ToRight");
            if (StPreDir == -2)
                unset.Add("StPreDir");
            if (StType == -2)
                unset.Add("StType");
            if (StName == -2)
                unset.Add("StName");
            if (StSufDir == -2)
                unset.Add("StSufDir");
            if (Clid == -2)
                unset.Add("CenterlineID");
            if (FullStName == -2)
                unset.Add("FullStName");

            return unset;
        }

        #region IFieldMap Members

        public FieldMapType getType()
        {
            return FieldMapType.Centerline;
        }

        #endregion
    }
}
