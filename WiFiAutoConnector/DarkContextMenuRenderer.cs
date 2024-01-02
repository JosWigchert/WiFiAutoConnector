using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiFiAutoConnector
{
    public class DarkContextMenuRenderer : ToolStripProfessionalRenderer
    {
        private readonly Color DarkBackgroundColor = Color.FromArgb(31, 31, 31);
        private readonly Color DarkBorderColor = Color.FromArgb(0, 0, 0);

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = e.Item.Enabled ? Color.White : Color.Gray;
            base.OnRenderArrow(e);
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle rectangle = new Rectangle(Point.Empty, e.Item.Size);
            e.Graphics.FillRectangle(new SolidBrush(DarkBackgroundColor), rectangle);
            e.Graphics.DrawRectangle(new Pen(DarkBorderColor), rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
        }

        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
        {
            OnRenderButtonBackground(e);
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(DarkBackgroundColor), e.ImageRectangle);
            base.OnRenderItemCheck(e);
        }

        protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle rectangle = new Rectangle(Point.Empty, e.Item.Size);
            e.Graphics.FillRectangle(new SolidBrush(DarkBackgroundColor), rectangle);
            e.Graphics.DrawRectangle(new Pen(DarkBorderColor), rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
        }

        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(DarkBackgroundColor), e.ImageRectangle);
            base.OnRenderItemImage(e);
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = e.Item.Enabled ? Color.White : Color.Gray;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(DarkBorderColor), e.Item.ContentRectangle.X + 30, e.Item.ContentRectangle.Y + 2, e.Item.ContentRectangle.Width - 30, 1);
        }
    }
}
