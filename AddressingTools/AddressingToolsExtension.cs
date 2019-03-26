using System.Collections.Generic;
using System.IO;
using ESRI.ArcGIS.ArcMapUI;
using System;
using System.Windows.Forms;

namespace AddressingTools
{
    public class AddressingToolsExtension : ESRI.ArcGIS.Desktop.AddIns.Extension
    {

        private IDocumentEvents_Event DocEvents
        {
            get { return ArcMap.Document as IDocumentEvents_Event; }
        }


        protected override void OnStartup()
        {
            base.OnStartup();
        }

        protected override void OnLoad(Stream inStrm)
        {
            base.OnLoad(inStrm);
        }

        protected override void OnSave(Stream outStrm)
        {
            base.OnSave(outStrm);
        }
    }
}
