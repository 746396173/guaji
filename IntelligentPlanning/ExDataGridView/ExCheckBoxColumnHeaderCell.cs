namespace IntelligentPlanning.ExDataGridView
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    internal class ExCheckBoxColumnHeaderCell : DataGridViewColumnHeaderCell
    {
        public bool _ChangeHeaderState;
        private Rectangle _checkboxRegion;
        public CheckState _CheckState;
        public bool _CheckVis;
        private bool _moveHot;

        public ExCheckBoxColumnHeaderCell(bool pVis, CheckState pDefult, bool pChange)
        {
            this._CheckVis = pVis;
            this._CheckState = pDefult;
            this._ChangeHeaderState = pChange;
            this._moveHot = false;
        }

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
            if (!this.IsInCheckRegion(e.Location))
            {
                base.OnMouseClick(e);
            }
            else
            {
                switch (this._CheckState)
                {
                    case CheckState.Unchecked:
                        this._CheckState = CheckState.Checked;
                        break;

                    case CheckState.Checked:
                        this._CheckState = CheckState.Unchecked;
                        break;

                    default:
                        this._CheckState = CheckState.Unchecked;
                        break;
                }
                base.DataGridView.InvalidateCell(this);
                this.OwningExCheckBoxColumn.OnHeaderCheckedChanged(this, e);
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

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState, "", "", errorText, cellStyle, advancedBorderStyle, paintParts);
            if (this._CheckVis)
            {
                this._checkboxRegion = this.GetCheckBoxRegion(cellBounds, CheckBoxRenderer.GetGlyphSize(graphics, this._PaintState));
                CheckBoxRenderer.DrawCheckBox(graphics, new Point(cellBounds.X + this._checkboxRegion.X, cellBounds.Y + this._checkboxRegion.Y), this._PaintState);
            }
        }

        public CheckBoxState _PaintState
        {
            get
            {
                switch (this._CheckState)
                {
                    case CheckState.Unchecked:
                        return (this._moveHot ? CheckBoxState.UncheckedHot : CheckBoxState.UncheckedNormal);

                    case CheckState.Checked:
                        return (this._moveHot ? CheckBoxState.CheckedHot : CheckBoxState.CheckedNormal);
                }
                return (this._moveHot ? CheckBoxState.MixedHot : CheckBoxState.MixedNormal);
            }
        }

        public ExCheckBoxColumn OwningExCheckBoxColumn =>
            (base.OwningColumn as ExCheckBoxColumn);
    }
}

