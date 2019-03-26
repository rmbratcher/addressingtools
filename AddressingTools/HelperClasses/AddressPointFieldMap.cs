using System.Collections.Generic;

namespace AddressingTools
{
    public class AddressPointFieldMap : IFieldMap
    {
        #region Private
        private int housenumber = -2;
        private int housenumsfx = -2;
        private int apid = -2;
        private int stpredir = -2;
        private int sttype = -2;
        private int stname = -2;
        private int stsufdir = -2;
        private int clid = -2;
        private int fulladdress = -2;
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
        public int HouseNumber
        {
            get { return housenumber; }
            set { housenumber = value; }
        }
        public int HouseNumberSfx
        {
            get { return housenumsfx; }
            set { housenumsfx = value; }
        }
        public int Apid
        {
            get { return apid; }
            set { apid = value; }
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
        public int FullAddress
        {
            get { return fulladdress; }
            set { fulladdress = value; }
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
            if (HouseNumber > -1 && HouseNumberSfx > -1 && Apid > -1 && StPreDir > -1 && StType > -1 && StSufDir > -1 && Clid > -1 && FullAddress > -1)
                return true;
            else
                return false;
        }

        public bool ManditorySet()
        {
            if (HouseNumber > -1 && Apid > -1 && Clid > -1)
                return true;
            return false;
        }

        public List<string> GetUnset()
        {
            List<string> unset = new List<string>();
            if (HouseNumber == -2)
                unset.Add("HouseNumber");
            if (HouseNumberSfx == -2)
                unset.Add("HouseNumberSfx");
            if (Apid == -2)
                unset.Add("AddressPointID");
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
            if (FullAddress == -2)
                unset.Add("FullAddress");

            return unset;
        }

        #region IFieldMap Members

        public FieldMapType getType()
        {
            return FieldMapType.AddressPoint;
        }

        #endregion
    }
}
