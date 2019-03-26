using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace AddressingTools
{
    public partial class AddressPointFields : Form
    {
        private IFeatureClass AddressPointFC;
        private List<IField> FieldList = new List<IField>();
        //private List<agd_FieldMap> FieldMapList = new List<agd_FieldMap>();
        public AddressPointFields(IFeatureClass FC)
        {
            InitializeComponent();
            AddressPointFC = FC;
            for (int i = 0; i < AddressPointFC.Fields.FieldCount; i++)
            {
                IField f = AddressPointFC.Fields.get_Field(i);
                if (f.Type != esriFieldType.esriFieldTypeBlob && f.Type != esriFieldType.esriFieldTypeGeometry && f.Type != esriFieldType.esriFieldTypeGlobalID && f.Type != esriFieldType.esriFieldTypeRaster && f.Type != esriFieldType.esriFieldTypeOID && f.Type != esriFieldType.esriFieldTypeXML)
                    FieldList.Add(f);
            }
        }

        private void AddressPointFields_Load(object sender, EventArgs e)
        {
            try
            {
                if (Globals.AddressPointFields.FullAddress > -1)
                    cB_FullAddress.Text = AddressPointFC.Fields.get_Field(Globals.AddressPointFields.FullAddress).Name;
                if (Globals.AddressPointFields.HouseNumber > -1)
                    cb_HouseNumber.Text = AddressPointFC.Fields.get_Field(Globals.AddressPointFields.HouseNumber).Name;
                if (Globals.AddressPointFields.Apid > -1)
                    cb_APID.Text = AddressPointFC.Fields.get_Field(Globals.AddressPointFields.Apid).Name;
                if (Globals.AddressPointFields.StName > -1)
                    cB_STName.Text = AddressPointFC.Fields.get_Field(Globals.AddressPointFields.StName).Name;
                if (Globals.AddressPointFields.StType > -1)
                    cB_STType.Text = AddressPointFC.Fields.get_Field(Globals.AddressPointFields.StType).Name;
                if (Globals.AddressPointFields.HouseNumberSfx > -1)
                    cB_HouseNumberSfx.Text = AddressPointFC.Fields.get_Field(Globals.AddressPointFields.HouseNumberSfx).Name;
                if (Globals.AddressPointFields.StPreDir > -1)
                    cB_STPreDir.Text = AddressPointFC.Fields.get_Field(Globals.AddressPointFields.StPreDir).Name;
                if (Globals.AddressPointFields.StSufDir > -1)
                    cB_STSfxDir.Text = AddressPointFC.Fields.get_Field(Globals.AddressPointFields.StSufDir).Name;
                if (Globals.AddressPointFields.Clid > -1)
                    cB_CenterlineID.Text = AddressPointFC.Fields.get_Field(Globals.AddressPointFields.Clid).Name;
                if (Globals.AddressPointFields.StTypeSfx > -1)
                    cB_StTypeSfx.Text = AddressPointFC.Fields.get_Field(Globals.AddressPointFields.StTypeSfx).Name;
            }
            catch
            {
            }

            foreach (IField f in FieldList)
            {
                if (f.Type == esriFieldType.esriFieldTypeInteger || f.Type == esriFieldType.esriFieldTypeOID || f.Type == esriFieldType.esriFieldTypeSmallInteger || f.Type == esriFieldType.esriFieldTypeDouble)
                {
                    cb_APID.Items.Add(f.Name);
                    cb_HouseNumber.Items.Add(f.Name);
                    cB_CenterlineID.Items.Add(f.Name);
                    cb_HouseNumber.Items.Add(f.Name);
                    cb_ZipCode.Items.Add(f.Name);
                }
                if (f.Type == esriFieldType.esriFieldTypeString)
                {
                    cB_FullAddress.Items.Add(f.Name);
                    cb_HouseNumber.Items.Add(f.Name);
                    cB_HouseNumberSfx.Items.Add(f.Name);
                    cB_STName.Items.Add(f.Name);
                    cB_STPreDir.Items.Add(f.Name);
                    cB_STSfxDir.Items.Add(f.Name);
                    cB_STType.Items.Add(f.Name);
                    cB_StTypeSfx.Items.Add(f.Name);
                    cB_Community.Items.Add(f.Name);
                    cb_ZipCode.Items.Add(f.Name);
                }
            }

            cB_HouseNumberSfx.Items.Add("None");
            cB_STPreDir.Items.Add("None");
            cB_STSfxDir.Items.Add("None");
            cB_CenterlineID.Items.Add("None");
            cB_StTypeSfx.Items.Add("None");
        }

        private void cb_APID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.AddressPointFields.Apid = AddressPointFC.Fields.FindField(cb_APID.SelectedItem.ToString());
            }
            catch(Exception ex) { errorLabel.Text = ex.Message;}
        }

        private void cb_HouseNumber_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.AddressPointFields.HouseNumber = AddressPointFC.Fields.FindField(cb_HouseNumber.SelectedItem.ToString());
            }
            catch(Exception ex) { errorLabel.Text = ex.Message;}
        }

        private void cB_HouseNumberSfx_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.AddressPointFields.HouseNumberSfx = AddressPointFC.Fields.FindField(cB_HouseNumberSfx.SelectedItem.ToString());
            }
            catch(Exception ex) { errorLabel.Text = ex.Message;}
        }

        private void cB_STPreDir_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.AddressPointFields.StPreDir = AddressPointFC.Fields.FindField(cB_STPreDir.SelectedItem.ToString());
            }
            catch(Exception ex) { errorLabel.Text = ex.Message;}
        }

        private void cB_STName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.AddressPointFields.StName = AddressPointFC.Fields.FindField(cB_STName.SelectedItem.ToString());
            }
            catch(Exception ex) { errorLabel.Text = ex.Message;}
        }

        private void cB_STType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.AddressPointFields.StType = AddressPointFC.Fields.FindField(cB_STType.SelectedItem.ToString());
            }
            catch(Exception ex) { errorLabel.Text = ex.Message;}
        }

        private void cB_STSfxDir_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.AddressPointFields.StSufDir = AddressPointFC.Fields.FindField(cB_STSfxDir.SelectedItem.ToString());
            }
            catch(Exception ex) { errorLabel.Text = ex.Message;}
        }

        private void cB_FullAddress_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.AddressPointFields.FullAddress = AddressPointFC.Fields.FindField(cB_FullAddress.SelectedItem.ToString());
            }
            catch(Exception ex) { errorLabel.Text = ex.Message;}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Globals.setAddressPointValues();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cB_CenterlineID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.AddressPointFields.Clid = AddressPointFC.Fields.FindField(cB_CenterlineID.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
        }

        private void cB_StTypeSfx_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.AddressPointFields.StTypeSfx = AddressPointFC.Fields.FindField(cB_StTypeSfx.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void cb_ZipCode_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.AddressPointFields.ZipCode = AddressPointFC.Fields.FindField(cb_ZipCode.SelectedItem.ToString());
            }
            catch(Exception ex) { errorLabel.Text = ex.Message;}
        }

        private void cB_Community_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.AddressPointFields.Community = AddressPointFC.Fields.FindField(cB_Community.SelectedItem.ToString());
            }
            catch(Exception ex) { errorLabel.Text = ex.Message;}
        }

    }
}
