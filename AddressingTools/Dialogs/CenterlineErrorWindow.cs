using System.Collections.Generic;
using System.Windows.Forms;

namespace AddressingTools
{
    public partial class CenterlineErrorWindow : Form
    {
        public CenterlineErrorWindow(List<string> errors)
        {
            InitializeComponent();
            foreach (string s in errors)
            {
                listView1.Items.Add(s);
            }
        }
    }
}
