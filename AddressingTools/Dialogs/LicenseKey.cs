using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AddressingTools.Dialogs
{
    public partial class LicenseKey : Form
    {
        public LicenseKey()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int lic_number = Convert.ToInt32(textBox1.Text);
            if (HelperClasses.LicenseHelper.IsValid(lic_number))
            {
                string assmblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string assemblyDir = System.IO.Path.GetDirectoryName(assmblyPath);
                string licensePath = System.IO.Path.Combine(assemblyDir, "License.lic");
                System.IO.StreamWriter sr;
                if (System.IO.File.Exists(licensePath))
                {
                    sr = new System.IO.StreamWriter(licensePath);
                }
                else
                {
                    sr = System.IO.File.CreateText(licensePath);
                }
                sr.WriteLine(textBox1.Text);
                sr.Flush();
                sr.Close();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Globals.IsLicensed = true;
                this.Close();
            }
            else
            {
                ErrorProvider ep = new ErrorProvider();
                ep.SetError(textBox1, "Invalid License #");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
