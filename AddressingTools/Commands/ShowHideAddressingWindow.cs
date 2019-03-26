using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.esriSystem;
using System.Windows.Forms;


namespace AddressingTools
{
    public class ShowHideAddressingWindow : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        private UID addWinUID = new UIDClass();
        private IDockableWindow addWin; 
        public ShowHideAddressingWindow()
        {
            addWinUID.Value = ThisAddIn.IDs.AddressingWindow;
            addWin = ArcMap.DockableWindowManager.GetDockableWindow(addWinUID);
            addWin.Show(false);
        }

        protected override void OnClick()
        {
            if (Globals.IsLicensed)
            {
                if (addWin.IsVisible())
                {
                    addWin.Show(false);
                }
                else
                {
                    addWin.Show(true);
                }
            }
            else
            {
                MessageBox.Show("This product is not licensed", "License Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        protected override void OnUpdate()
        {
            if (ArcMap.Editor.EditState == ESRI.ArcGIS.Editor.esriEditState.esriStateEditing)
            {
                Enabled = true;
            }
        }
    }
}
