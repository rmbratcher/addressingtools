using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace AddressingTools
{
    public partial class CenerlineFields : Form
    {
        private IFeatureClass CenterlineFC;
        private List<IField> FieldList = new List<IField>();

        public CenerlineFields(IFeatureClass FC)
        {
            InitializeComponent();
            CenterlineFC = FC;
            for (int i = 0; i < CenterlineFC.Fields.FieldCount; i++)
            {
                IField f = CenterlineFC.Fields.get_Field(i);
                if(f.Type != esriFieldType.esriFieldTypeBlob && f.Type != esriFieldType.esriFieldTypeGeometry && f.Type != esriFieldType.esriFieldTypeGlobalID && f.Type != esriFieldType.esriFieldTypeRaster && f.Type != esriFieldType.esriFieldTypeOID && f.Type != esriFieldType.esriFieldTypeXML)
                    FieldList.Add(f);
            }
            try
            {
                if (Globals.CenterlineFields.FromLeft > -1)
                    cB_FromLeft.Text = CenterlineFC.Fields.get_Field(Globals.CenterlineFields.FromLeft).Name;
                if (Globals.CenterlineFields.FromRight > -1)
                    cB_FromRight.Text = CenterlineFC.Fields.get_Field(Globals.CenterlineFields.FromRight).Name;
                if (Globals.CenterlineFields.Clid > -1)
                    cB_Clid.Text = CenterlineFC.Fields.get_Field(Globals.CenterlineFields.Clid).Name;
                if (Globals.CenterlineFields.StName > -1)
                    cB_StName.Text = CenterlineFC.Fields.get_Field(Globals.CenterlineFields.StName).Name;
                if (Globals.CenterlineFields.StPreDir > -1)
                    cB_StPreDir.Text = CenterlineFC.Fields.get_Field(Globals.CenterlineFields.StPreDir).Name;
                if (Globals.CenterlineFields.StSufDir > -1)
                    cB_StSufDir.Text = CenterlineFC.Fields.get_Field(Globals.CenterlineFields.StSufDir).Name;
                if (Globals.CenterlineFields.StType > -1)
                    cB_StType.Text = CenterlineFC.Fields.get_Field(Globals.CenterlineFields.StType).Name;
                if (Globals.CenterlineFields.ToLeft > -1)
                    cB_ToLeft.Text = CenterlineFC.Fields.get_Field(Globals.CenterlineFields.ToLeft).Name;
                if (Globals.CenterlineFields.ToRight > -1)
                    cB_ToRight.Text = CenterlineFC.Fields.get_Field(Globals.CenterlineFields.ToRight).Name;
                if (Globals.CenterlineFields.FullStName > -1)
                    cB_FullStName.Text = CenterlineFC.Fields.get_Field(Globals.CenterlineFields.FullStName).Name;
                if (Globals.CenterlineFields.EsnLeft > -1)
                    cb_EsnLeft.Text = CenterlineFC.Fields.get_Field(Globals.CenterlineFields.EsnLeft).Name;
                else
                    cb_EsnLeft.Text = "None";
                if (Globals.CenterlineFields.EsnRight > -1)
                    cB_EsnRight.Text = CenterlineFC.Fields.get_Field(Globals.CenterlineFields.EsnRight).Name;
                else
                    cB_EsnRight.Text = "None";
                if (Globals.CenterlineFields.StTypeSfx > -1)
                    cB_StTypeSfx.Text = CenterlineFC.Fields.get_Field(Globals.CenterlineFields.StTypeSfx).Name;
            }
            catch
            {
            }
        }

        private void CenerlineFields_Load(object sender, EventArgs e)
        {
            foreach (IField f in FieldList)
            {
                if (f.Type == esriFieldType.esriFieldTypeInteger || f.Type == esriFieldType.esriFieldTypeSmallInteger || f.Type == esriFieldType.esriFieldTypeDouble)
                {
                    cB_ToRight.Items.Add(f.Name);
                    cB_ToLeft.Items.Add(f.Name);
                    cB_FromRight.Items.Add(f.Name);
                    cB_FromLeft.Items.Add(f.Name);
                    cB_Clid.Items.Add(f.Name);
                    cB_StPreDir.Items.Add(f.Name);
                    cB_StSufDir.Items.Add(f.Name);
                    cB_StType.Items.Add(f.Name);
                    cB_EsnRight.Items.Add(f.Name);
                    cb_EsnLeft.Items.Add(f.Name);
                    cb_ZipCode.Items.Add(f.Name);
                }
                if (f.Type == esriFieldType.esriFieldTypeString)
                {
                    cB_FullStName.Items.Add(f.Name);
                    cB_StName.Items.Add(f.Name);
                    cB_StPreDir.Items.Add(f.Name);
                    cB_StSufDir.Items.Add(f.Name);
                    cB_StType.Items.Add(f.Name);
                    cB_EsnRight.Items.Add(f.Name);
                    cb_EsnLeft.Items.Add(f.Name);
                    cB_StTypeSfx.Items.Add(f.Name);
                    cB_Community.Items.Add(f.Name);
                    cb_ZipCode.Items.Add(f.Name);
                }
            }

            cB_Clid.Items.Add("None");
            cB_StPreDir.Items.Add("None");
            cB_StSufDir.Items.Add("None");
            cB_FullStName.Items.Add("None");
            cB_StType.Items.Add("None");
            cB_StTypeSfx.Items.Add("None");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Globals.setCenterlineValues();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void cB_Clid_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.CenterlineFields.Clid = CenterlineFC.Fields.FindField(cB_Clid.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
        }

        private void cB_FromLeft_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.CenterlineFields.FromLeft = CenterlineFC.Fields.FindField(cB_FromLeft.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
        }

        private void cB_ToLeft_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.CenterlineFields.ToLeft = CenterlineFC.Fields.FindField(cB_ToLeft.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
        }

        private void cB_FromRight_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.CenterlineFields.FromRight = CenterlineFC.Fields.FindField(cB_FromRight.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
        }

        private void cB_ToRight_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.CenterlineFields.ToRight = CenterlineFC.Fields.FindField(cB_ToRight.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
        }

        private void cB_StPreDir_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.CenterlineFields.StPreDir = CenterlineFC.Fields.FindField(cB_StPreDir.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
        }

        private void cB_StName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.CenterlineFields.StName = CenterlineFC.Fields.FindField(cB_StName.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
        }

        private void cB_StType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Globals.CenterlineFields.StType = CenterlineFC.Fields.FindField(cB_StType.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
        }

        private void cB_StSufDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Globals.CenterlineFields.StSufDir = CenterlineFC.Fields.FindField(cB_StSufDir.SelectedItem.ToString());
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
                Globals.CenterlineFields.StTypeSfx = CenterlineFC.Fields.FindField(cB_StTypeSfx.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
        }

        private void cB_FullStName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.CenterlineFields.FullStName = CenterlineFC.Fields.FindField(cB_FullStName.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
        }

        private void cb_EsnLeft_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.CenterlineFields.EsnLeft = CenterlineFC.Fields.FindField(cb_EsnLeft.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
        }

        private void cB_EsnRight_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.CenterlineFields.EsnRight = CenterlineFC.Fields.FindField(cB_EsnRight.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
        }

        private void cB_StSufDir_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.CenterlineFields.StSufDir = CenterlineFC.Fields.FindField(cB_StSufDir.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
        }

        private void cB_StType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.CenterlineFields.StType = CenterlineFC.Fields.FindField(cB_StType.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void cb_ZipCode_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.CenterlineFields.ZipCode = CenterlineFC.Fields.FindField(cb_ZipCode.SelectedItem.ToString());
            }
            catch(Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
        }

        private void cB_Community_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Globals.CenterlineFields.Community = CenterlineFC.Fields.FindField(cB_Community.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
        }
    }
}
