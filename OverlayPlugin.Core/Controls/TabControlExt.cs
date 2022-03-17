using System;
using System.Drawing;
using System.Windows.Forms;

namespace RainbowMage.OverlayPlugin
{
    public class TabControlExt : TabControl
    {
        protected float dpi;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(SystemColors.ControlLightLight);
            e.Graphics.FillRectangle(SystemBrushes.ControlLight, 4, 4, (ItemSize.Height * RowCount) - 4, Height - 8);

            int inc = 0;

            foreach (TabPage tp in TabPages)
            {
                Color fore = Color.Black;
                Font fontF = Font;
                Font fontFSmall = new Font(Font.FontFamily, (float)(Font.Size * 0.85));
                Rectangle tabrect = GetTabRect(inc);
                Rectangle rect = new Rectangle(tabrect.X + WithDpi(4), tabrect.Y + WithDpi(4), tabrect.Width - WithDpi(8), tabrect.Height - WithDpi(2));
                Rectangle textrect1 = new Rectangle(tabrect.X + WithDpi(4), tabrect.Y + WithDpi(4), tabrect.Width - WithDpi(8), tabrect.Height - WithDpi(20));
                Rectangle textrect2 = new Rectangle(tabrect.X + WithDpi(4), tabrect.Y + WithDpi(20), tabrect.Width - WithDpi(8), tabrect.Height - WithDpi(20));

                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;

                if (inc == SelectedIndex)
                {
                    e.Graphics.FillRectangle(new SolidBrush(SystemColors.Highlight), rect);
                    fore = SystemColors.HighlightText;
                    fontF = new Font(Font, FontStyle.Bold);
                }
                else
                {
                    e.Graphics.FillRectangle(Brushes.White, rect);
                }

                e.Graphics.DrawString(tp.Name, fontF, new SolidBrush(fore), textrect1, sf);
                e.Graphics.DrawString(tp.Text, fontFSmall, new SolidBrush(fore), textrect2, sf);
                inc++;
            }
        }

        protected override void OnTabIndexChanged(EventArgs e)
        {
            base.OnTabIndexChanged(e);
            Invalidate();
        }

        protected int WithDpi(int number)
        {
            return (int)(number * dpi);
        }

        public TabControlExt() : base()
        {
            Alignment = TabAlignment.Left;

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);

            DoubleBuffered = true;

            using (var g = CreateGraphics())
            {
                dpi = g.DpiX / 96;
            }

            ItemSize = new Size(WithDpi(48), WithDpi(140));
            SizeMode = TabSizeMode.Fixed;
            BackColor = Color.Transparent;
        }
    }
}