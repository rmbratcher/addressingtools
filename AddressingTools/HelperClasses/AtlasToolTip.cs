namespace AddressingTools
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class AtlasToolTip : IDisposable
    {
        public readonly StringFormat Format = new StringFormat();
        public Point Offset;
        public Font Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold, GraphicsUnit.Pixel);
        public Pen Stroke = new Pen(Color.FromArgb(140, Color.MidnightBlue));
        public Brush Fill = new SolidBrush(Color.FromArgb(222, Color.AliceBlue));
        public Brush Foreground = new SolidBrush(Color.Navy);
        public Size TextPadding = new Size(10, 10);

        AddressMarker Marker;

        public AtlasToolTip(AddressMarker marker)
        {
            this.Marker = marker;
            this.Offset = new Point(14, -44);
            this.Stroke.Width = 2;
            this.Stroke.LineJoin = LineJoin.Round;
            this.Stroke.StartCap = LineCap.RoundAnchor;
            this.Format.LineAlignment = StringAlignment.Center;
            this.Format.Alignment = StringAlignment.Center;
        }

        public virtual void OnRender(Graphics g)
        {
            System.Drawing.Size st = g.MeasureString(Marker.ToolTipText, Font).ToSize();
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y - st.Height, st.Width + TextPadding.Width, st.Height + TextPadding.Height);
            rect.Offset(Offset.X, Offset.Y);

            g.DrawLine(Stroke, Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y, rect.X, rect.Y + rect.Height / 2);

            g.FillRectangle(Fill, rect);
            g.DrawRectangle(Stroke, rect);

            g.DrawString(Marker.ToolTipText, Font, Foreground, rect, Format);
        }

        #region IDisposable Members
        bool disposed = false;
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;

                Format.Dispose();

                Font.Dispose();
                Font = null;

                Stroke.Dispose();
                Stroke = null;

                Fill.Dispose();
                Fill = null;

                Foreground.Dispose();
                Foreground = null;
            }
        }

        #endregion
    }
}

