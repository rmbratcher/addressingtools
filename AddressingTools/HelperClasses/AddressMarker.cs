namespace AddressingTools
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Collections.Generic;
    using ESRI.ArcGIS.Carto;
    using ESRI.ArcGIS.Geometry;
    using AddressingTools.Properties;

    public enum MarkerType
    {
        none = 0,
        arrow,
        blue,
        blue_small,
        blue_dot,
        blue_pushpin,
        brown_small,
        gray_small,
        green,
        green_small,
        green_dot,
        green_pushpin,
        green_big_go,
        yellow,
        yellow_small,
        yellow_dot,
        yellow_big_pause,
        yellow_pushpin,
        lightblue,
        lightblue_dot,
        lightblue_pushpin,
        orange,
        orange_small,
        orange_dot,
        pink,
        pink_dot,
        pink_pushpin,
        purple,
        purple_small,
        purple_dot,
        purple_pushpin,
        red,
        red_small,
        red_dot,
        red_pushpin,
        red_big_stop,
        black_small,
        white_small,
    }

    public class AddressMarker : IDisposable
    {
        public float? Bearing;
        Bitmap Bitmap;
        Bitmap BitmapShadow;

        static Bitmap arrowshadow;
        static Bitmap msmarker_shadow;
        static Bitmap shadow_small;
        static Bitmap pushpin_shadow;


        IActiveView map = null;
        public string ToolTipText = "";
        AtlasToolTip mToolTip;
        MarkerType type = MarkerType.none;

        private bool IsVisable = false;

        public MarkerType markerType
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public AddressMarker(IActiveView theMap, IPoint loc, string toolTip, MarkerType markerType)
        {
            map = theMap;

            ToolTipText = toolTip;
            this.type = markerType;
            this.mToolTip = new AtlasToolTip(this);

            if (type != MarkerType.none)
            {
                LoadBitmap();
            }

            Location = loc;
        }
        public void Update(IPoint loc, string tooltip, MarkerType markertype)
        {

            ToolTipText = tooltip;
            if (markertype != this.type)
            {
                if (type != MarkerType.none)
                {
                    LoadBitmap();
                }
            }

            Location = loc;

        }

        System.Drawing.Point offset;
        public System.Drawing.Point Offset
        {
            get
            {
                return offset;
            }
            set
            {
                offset = value;
            }
        }

        private void LoadBitmap()
        {
            Bitmap = GetIcon(type.ToString());
            Size = new System.Drawing.Size(Bitmap.Width, Bitmap.Height);

            switch (type)
            {
                case MarkerType.arrow:
                    {
                        Offset = new System.Drawing.Point(-11, -Size.Height);

                        if (arrowshadow == null)
                        {
                            arrowshadow = Resources.arrowshadow;
                        }
                        BitmapShadow = arrowshadow;
                    }
                    break;

                case MarkerType.blue:
                case MarkerType.blue_dot:
                case MarkerType.green:
                case MarkerType.green_dot:
                case MarkerType.yellow:
                case MarkerType.yellow_dot:
                case MarkerType.lightblue:
                case MarkerType.lightblue_dot:
                case MarkerType.orange:
                case MarkerType.orange_dot:
                case MarkerType.pink:
                case MarkerType.pink_dot:
                case MarkerType.purple:
                case MarkerType.purple_dot:
                case MarkerType.red:
                case MarkerType.red_dot:
                    {
                        Offset = new System.Drawing.Point(-Size.Width / 2 + 1, -Size.Height + 1);

                        if (msmarker_shadow == null)
                        {
                            msmarker_shadow = Resources.msmarker_shadow;
                        }
                        BitmapShadow = msmarker_shadow;
                    }
                    break;

                case MarkerType.black_small:
                case MarkerType.blue_small:
                case MarkerType.brown_small:
                case MarkerType.gray_small:
                case MarkerType.green_small:
                case MarkerType.yellow_small:
                case MarkerType.orange_small:
                case MarkerType.purple_small:
                case MarkerType.red_small:
                case MarkerType.white_small:
                    {
                        Offset = new System.Drawing.Point(-Size.Width / 2, -Size.Height + 1);

                        if (shadow_small == null)
                        {
                            shadow_small = Resources.shadow_small;
                        }
                        BitmapShadow = shadow_small;
                    }
                    break;

                case MarkerType.green_big_go:
                case MarkerType.yellow_big_pause:
                case MarkerType.red_big_stop:
                    {
                        Offset = new System.Drawing.Point(-Size.Width / 2, -Size.Height + 1);
                        if (msmarker_shadow == null)
                        {
                            msmarker_shadow = Resources.msmarker_shadow;
                        }
                        BitmapShadow = msmarker_shadow;
                    }
                    break;

                case MarkerType.blue_pushpin:
                case MarkerType.green_pushpin:
                case MarkerType.yellow_pushpin:
                case MarkerType.lightblue_pushpin:
                case MarkerType.pink_pushpin:
                case MarkerType.purple_pushpin:
                case MarkerType.red_pushpin:
                    {
                        Offset = new System.Drawing.Point(-9, -Size.Height + 1);

                        if (pushpin_shadow == null)
                        {
                            pushpin_shadow = Resources.pushpin_shadow;
                        }
                        BitmapShadow = pushpin_shadow;
                    }
                    break;
            }
        }

        static readonly Dictionary<string, Bitmap> iconCache = new Dictionary<string, Bitmap>();

        internal static Bitmap GetIcon(string name)
        {
            Bitmap ret;
            ret = Resources.ResourceManager.GetObject(name, Resources.Culture) as Bitmap;
            return ret;
        }

        static readonly System.Drawing.Point[] Arrow = new System.Drawing.Point[] { new System.Drawing.Point(-7, 7), new System.Drawing.Point(0, -22), new System.Drawing.Point(7, 7), new System.Drawing.Point(0, 2) };

        public void Show()
        {
            IsVisable = true;
        }
        public void Hide()
        {
            IsVisable = false;
        }
        public void Delete()
        {
            this.Dispose();
        }

        public virtual void OnRender(Graphics g)
        {
            if (IsVisable)
            {
                IRelationalOperator ro = map.Extent as IRelationalOperator;
                if (ro.Contains(this.Location))
                {
                    int outX = 0;
                    int outY = 0;
                    map.ScreenDisplay.DisplayTransformation.FromMapPoint(this.Location, out outX, out outY);

                    localLocation = new System.Drawing.Point(outX, outY);

                    this.mToolTip.OnRender(g);
                    if (BitmapShadow != null)
                    {
                        g.DrawImage(BitmapShadow, localLocation.X, localLocation.Y, BitmapShadow.Width, BitmapShadow.Height);
                        g.DrawImage(Bitmap, localLocation.X, localLocation.Y, Size.Width, Size.Height);
                    }
                }
            }
        }

        Rectangle area;
        public System.Drawing.Point localLocation
        {
            get { return area.Location; }
            set
            {
                if (area.Location != value)
                {
                    area.Location = value;
                }
            }
        }

        public System.Drawing.Point ToolTipPosition
        {
            get
            {
                System.Drawing.Point ret = area.Location;
                ret.Offset(-Offset.X, -Offset.Y);
                return ret;
            }
        }

        public Size Size
        {
            get
            {
                return area.Size;
            }
            set
            {
                area.Size = value;
            }
        }

        private IPoint location;
        public IPoint Location
        {
            get { return location; }
            set
            {
                if (location != value)
                {
                    location = value;
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Bitmap.Dispose();
            this.BitmapShadow.Dispose();
            this.location = null;
            this.map = null;
            this.mToolTip.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

