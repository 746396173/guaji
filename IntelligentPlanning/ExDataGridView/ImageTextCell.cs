namespace IntelligentPlanning.ExDataGridView
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class ImageTextCell : DataGridViewTextBoxCell
    {
        private Image btnImage;
        private Padding defaultPadding;
        private int ImageHeight = 0;
        private int ImageWidth = 0;
        private bool moveHot = false;
        private const int Right_MARGIN = 3;

        public override object Clone()
        {
            ImageTextCell cell = (ImageTextCell) base.Clone();
            cell.btnImage = this.btnImage;
            cell.defaultPadding = this.defaultPadding;
            return cell;
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            this.defaultPadding = base.Style.Padding;
            Padding padding = base.Style.Padding;
            base.Style.Padding = new Padding(padding.Left, padding.Top, padding.Right + this.OffsetMargin, padding.Bottom);
        }

        protected override void OnLeave(int rowIndex, bool throughMouseClick)
        {
            base.OnLeave(rowIndex, throughMouseClick);
            base.Style.Padding = this.defaultPadding;
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            if (e.X >= (base.OwningColumn.Width - this.ImageWidth))
            {
                this.OwningColumnImageText.onButtonClick(this, e);
            }
            else
            {
                base.OnMouseClick(e);
            }
        }

        protected override void OnMouseDoubleClick(DataGridViewCellMouseEventArgs e)
        {
            if (this.OwningColumnImageText.EnableDoubleClick && (e.X < (base.OwningColumn.Width - this.ImageWidth)))
            {
                this.OwningColumnImageText.onButtonClick(this, e);
            }
            else
            {
                base.OnMouseDoubleClick(e);
            }
        }

        protected override void OnMouseLeave(int rowIndex)
        {
            this.moveHot = false;
            base.DataGridView.InvalidateCell(this);
        }

        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            this.moveHot = e.X >= (base.OwningColumn.Width - this.OffsetMargin);
            base.DataGridView.InvalidateCell(this);
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            if (this.moveHot || base.DataGridView.SelectedRows.Contains(base.OwningRow))
            {
                this.btnImage = this.OwningColumnImageText.HotImage;
            }
            else
            {
                this.btnImage = this.OwningColumnImageText.DefaultImage;
            }
            this.ImageWidth = this.btnImage.Width;
            this.ImageHeight = this.btnImage.Height;
            graphics.DrawImage(this.btnImage, new Rectangle((cellBounds.X + cellBounds.Width) - this.OffsetMargin, ((cellBounds.Height / 2) - (this.ImageHeight / 2)) + cellBounds.Y, this.ImageWidth, this.ImageHeight));
        }

        public int OffsetMargin =>
            (this.ImageWidth + 3);

        public ImageTextColumn OwningColumnImageText =>
            (base.OwningColumn as ImageTextColumn);
    }
}

