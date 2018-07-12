namespace IntelligentPlanning.ExDataGridView
{
    using System;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    internal class ShapeTextCell : DataGridViewTextBoxCell
    {
        private Rectangle _cellBounds;
        public bool _check = false;
        private bool _moveHot = false;
        private Image btnImage;

        public override object Clone()
        {
            ShapeTextCell cell = (ShapeTextCell) base.Clone();
            cell.btnImage = this.btnImage;
            return cell;
        }

        public int GetTextSize(string pText)
        {
            int length = pText.Length;
            for (int i = 0; i < pText.Length; i++)
            {
                if (Encoding.GetEncoding("gb2312").GetBytes(pText.Substring(i, 1)).Length == 2)
                {
                    length++;
                }
            }
            return length;
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseClick(e);
            this._check = !this._check;
        }

        protected override void OnMouseLeave(int rowIndex)
        {
            this._moveHot = false;
            base.DataGridView.InvalidateCell(this);
        }

        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            this._moveHot = true;
            base.DataGridView.InvalidateCell(this);
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            Color backgroundColor = base.DataGridView.BackgroundColor;
            graphics.FillRectangle(new SolidBrush(backgroundColor), cellBounds.X, cellBounds.Y, cellBounds.Width, cellBounds.Height);
            if (base.Value != null)
            {
                if (this._check)
                {
                    this.btnImage = this.OwningColumnShapeText.PressedImage;
                }
                else
                {
                    this.btnImage = this._moveHot ? this.OwningColumnShapeText.HotImage : this.OwningColumnShapeText.DefaultImage;
                }
                graphics.DrawImage(this.btnImage, new Rectangle(cellBounds.Left, cellBounds.Top, this.OwningColumnShapeText.ImageSize, this.OwningColumnShapeText.ImageSize));
                this._cellBounds = cellBounds;
                graphics.DrawString(base.Value.ToString(), this.OwningColumnShapeText._font, new SolidBrush(this._check ? Color.White : Color.Black), new PointF((float) (cellBounds.Left + this.OffsetLeft), (float) (cellBounds.Top + this.OffsetTop)));
            }
        }

        public int OffsetLeft =>
            (Convert.ToInt32((float) ((this.OwningColumnShapeText.ImageSize / 2) - ((this.GetTextSize(base.Value.ToString()) * this.OwningColumnShapeText._font.Size) / 2f))) + (this.GetTextSize(base.Value.ToString()) - 2));

        public int OffsetTop =>
            (Convert.ToInt32((int) ((this.OwningColumnShapeText.ImageSize / 2) - (this.OwningColumnShapeText._font.Height / 2))) - 1);

        public ShapeTextColumn OwningColumnShapeText =>
            (base.OwningColumn as ShapeTextColumn);
    }
}

