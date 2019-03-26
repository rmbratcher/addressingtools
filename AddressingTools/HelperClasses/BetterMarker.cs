using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;

namespace AddressingTools
{
    class BetterMarker
    {
        private IBalloonCallout _Callout;
        private IElement _Element;

        public BetterMarker(ESRI.ArcGIS.Geometry.IPoint mapPoint, string Text, double MapScale)
        {

            _Callout = new BalloonCallout();

            _Callout.AnchorPoint = mapPoint;

            ITextElement pTextElement = new TextElementClass();
            IElement pElement;
            IFillSymbol pFill = new SimpleFillSymbolClass();
            ILineSymbol pLine = new SimpleLineSymbolClass();
            IFormattedTextSymbol pLabelSymbol = new TextSymbolClass();

            IRgbColor c = new RgbColorClass();
            c.Red = 0;
            c.Green = 0;
            c.Blue = 0;

            IRgbColor d = new RgbColorClass();
            d.Red = 255;
            d.Green = 255;
            d.Blue = 255;

            IRgbColor e = new RgbColorClass();
            e.Red = 205;
            e.Green = 192;
            e.Blue = 176;

            pLine.Color = c;
            pFill.Color = d;
            pFill.Outline = pLine;

            this._Callout.Symbol = pFill;
            this._Callout.Style = esriBalloonCalloutStyle.esriBCSRoundedRectangle;


            pLabelSymbol.Background = this._Callout as ITextBackground;
            pLabelSymbol.Size = 10.5d;
            pLabelSymbol.ShadowColor = e;
            pLabelSymbol.ShadowXOffset = 1.0d;
            pLabelSymbol.ShadowYOffset = 1.0d;

            pTextElement.Text = Text;
            pTextElement.Symbol = pLabelSymbol as ITextSymbol;

            pElement = pTextElement as IElement;
            double delta = (.1 * MapScale) / 2;
            //switch (mMap.MapScale)
            //{
            //    case 
            //}

            ESRI.ArcGIS.Geometry.IPoint p1 = new ESRI.ArcGIS.Geometry.PointClass();
            p1.X = mapPoint.X + delta;
            p1.Y = mapPoint.Y + delta;


            pElement.Geometry = p1;

            this._Element = pElement;
        }

        public void Update(double MapScale)
        {
            ESRI.ArcGIS.Geometry.IPoint pnt = new ESRI.ArcGIS.Geometry.PointClass();
            double delta = (.1 * MapScale) / 2;

            pnt.X = this._Callout.AnchorPoint.X + delta;
            pnt.Y = this._Callout.AnchorPoint.Y + delta;

            this._Element.Geometry = pnt;
        }

        public IElement MarkerElement
        {
            get { return this._Element; }
        }
    }
}
