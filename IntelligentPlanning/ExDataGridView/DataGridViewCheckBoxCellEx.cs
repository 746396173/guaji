namespace IntelligentPlanning.ExDataGridView
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    internal class DataGridViewCheckBoxCellEx : DataGridViewCheckBoxCell
    {
        private Rectangle _checkboxRegion;
        private bool _moveHot = false;

        private Rectangle GetCheckBoxRegion(Rectangle cellBounds, Size pCheckBoxSize) => 
            new Rectangle { 
                Size = pCheckBoxSize,
                X = (cellBounds.Width / 2) - (pCheckBoxSize.Width / 2),
                Y = (cellBounds.Height / 2) - (pCheckBoxSize.Height / 2)
            };

        private bool IsInCheckRegion(Point point) => 
            this._checkboxRegion.Contains(point);

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            if (this.IsInCheckRegion(e.Location))
            {
                this.Checked = !this.Checked;
                base.DataGridView.InvalidateCell(this);
                this.OwningExCheckBoxColumn.OnCheckValueChanged(this, e);
            }
            else
            {
                base.OnMouseClick(e);
            }
        }

        protected override void OnMouseLeave(int rowIndex)
        {
            this._moveHot = false;
            base.DataGridView.InvalidateCell(this);
        }

        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            this._moveHot = this.IsInCheckRegion(e.Location);
            base.DataGridView.InvalidateCell(this);
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            Color color = base.DataGridView.SelectedCells.Contains(this) ? cellStyle.SelectionBackColor : cellStyle.BackColor;
            graphics.FillRectangle(new SolidBrush(color), cellBounds.X, cellBounds.Y, cellBounds.Width, cellBounds.Height);
            base.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
            this._checkboxRegion = this.GetCheckBoxRegion(cellBounds, CheckBoxRenderer.GetGlyphSize(graphics, this._PaintState));
            CheckBoxRenderer.DrawCheckBox(graphics, new Point(cellBounds.X + this._checkboxRegion.X, cellBounds.Y + this._checkboxRegion.Y), this._PaintState);
        }

        public CheckBoxState _PaintState
        {
            get
            {
                if (this.Checked)
                {
                    return (this._moveHot ? CheckBoxState.CheckedHot : CheckBoxState.CheckedNormal);
                }
                return (this._moveHot ? CheckBoxState.UncheckedHot : CheckBoxState.UncheckedNormal);
            }
        }

        public bool Checked
        {
            get => 
                Convert.ToBoolean(base.Value);
            set
            {
                base.Value = value;
            }
        }

        public ExCheckBoxColumn OwningExCheckBoxColumn =>
            (base.OwningColumn as ExCheckBoxColumn);
    }
}

