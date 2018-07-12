namespace IntelligentPlanning.ExDataGridView
{
    using IntelligentPlanning.Properties;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public class TreeGridCell : DataGridViewTextBoxCell
    {
        private Rectangle _checkboxRegion;
        private bool _moveHot;
        private bool _RunCheckLate = false;
        private int CalculatedLeftPadding = 0;
        private int CheckSize = 15;
        public bool DrawCheckBox = true;
        private int GlyphWidth;
        private int ImageHeight = 0;
        private int ImageHeightOffset = 0;
        private int ImageWidth = 0;
        private const int INDENT_MARGIN = 5;
        private int INDENT_WIDTH;
        private bool IsHaveCheckBox = false;
        internal bool IsSited = false;
        private Rectangle LastKnownGlyphRect;
        private int NodeSize = 10;
        private Padding PreviousPadding;

        public override object Clone()
        {
            TreeGridCell cell = (TreeGridCell) base.Clone();
            cell.GlyphWidth = this.GlyphWidth;
            cell.CalculatedLeftPadding = this.CalculatedLeftPadding;
            return cell;
        }

        private Rectangle GetCheckBoxRegion(Rectangle glyphRect, Size pCheckBoxSize) => 
            new Rectangle { 
                Size = pCheckBoxSize,
                X = (glyphRect.X + this.NodeSize) + 4,
                Y = (glyphRect.Y + (glyphRect.Height / 2)) - 5
            };

        private bool IsInCheckRegion(Point point) => 
            (((point.X <= base.InheritedStyle.Padding.Left) && this.IsHaveCheckBox) && (point.X > ((base.InheritedStyle.Padding.Left - this.ImageWidth) - this.CheckSize)));

        protected override void OnMouseDoubleClick(DataGridViewCellMouseEventArgs e)
        {
            if (e.Location.X > base.InheritedStyle.Padding.Left)
            {
                TreeGridNode owningNode = this.OwningNode;
                if (owningNode != null)
                {
                    if (owningNode.HasChildren || owningNode.BaseTGV.VirtualNodes)
                    {
                        owningNode.BaseTGV.InExpandCollapseMouseCapture = true;
                        if (owningNode.IsExpanded)
                        {
                            owningNode.Collapse();
                        }
                        else
                        {
                            owningNode.Expand();
                        }
                    }
                    else
                    {
                        if (owningNode._CheckState == CheckState.Unchecked)
                        {
                            owningNode._CheckState = CheckState.Checked;
                        }
                        else
                        {
                            owningNode._CheckState = CheckState.Unchecked;
                        }
                        owningNode.BaseTGV.RefreshCheckStatus(owningNode, true, false);
                    }
                }
            }
        }

        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (e.Location.X <= base.InheritedStyle.Padding.Left)
            {
                TreeGridNode owningNode = this.OwningNode;
                if (owningNode != null)
                {
                    if ((this.IsHaveCheckBox && (e.Location.X > ((base.InheritedStyle.Padding.Left - this.ImageWidth) - this.CheckSize))) && this.DrawCheckBox)
                    {
                        if (owningNode._CheckState == CheckState.Unchecked)
                        {
                            owningNode._CheckState = CheckState.Checked;
                        }
                        else
                        {
                            owningNode._CheckState = CheckState.Unchecked;
                        }
                        this._RunCheckLate = true;
                        owningNode.BaseTGV.RefreshCheckStatus(owningNode, true, false);
                    }
                    else
                    {
                        owningNode.BaseTGV.InExpandCollapseMouseCapture = true;
                        if (owningNode.IsExpanded)
                        {
                            owningNode.Collapse();
                        }
                        else
                        {
                            owningNode.Expand();
                        }
                    }
                }
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

        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseUp(e);
            TreeGridNode owningNode = this.OwningNode;
            if (owningNode != null)
            {
                owningNode.BaseTGV.InExpandCollapseMouseCapture = false;
            }
            if (this._RunCheckLate)
            {
                this._RunCheckLate = false;
                TreeGridNodeEventBase base2 = new TreeGridNodeEventBase(owningNode);
                owningNode.BaseTGV.OnCheckedLate(this, base2);
            }
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            TreeGridNode owningNode = this.OwningNode;
            if (owningNode != null)
            {
                Image image = owningNode.Image;
                if ((this.ImageHeight == 0) && (image != null))
                {
                    this.UpdateStyle();
                }
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
                Rectangle glyphRect = new Rectangle(cellBounds.X + this.GlyphMargin, cellBounds.Y, this.INDENT_WIDTH, cellBounds.Height - 1);
                int num = glyphRect.Width / 2;
                int level = this.Level;
                if (image != null)
                {
                    Point point;
                    if (this.ImageHeight > cellBounds.Height)
                    {
                        point = new Point(glyphRect.X + this.GlyphWidth, cellBounds.Y + this.ImageHeightOffset);
                    }
                    else
                    {
                        point = new Point(glyphRect.X + this.GlyphWidth, ((cellBounds.Height / 2) - (this.ImageHeight / 2)) + cellBounds.Y);
                    }
                    GraphicsContainer container = graphics.BeginContainer();
                    graphics.SetClip(cellBounds);
                    graphics.DrawImageUnscaled(image, point);
                    graphics.EndContainer(container);
                }
                if (owningNode.BaseTGV.ShowLines)
                {
                    using (Pen pen = new Pen(SystemBrushes.ControlDark, 1f))
                    {
                        pen.DashStyle = DashStyle.Dot;
                        bool isLastSibling = owningNode.IsLastSibling;
                        bool isFirstSibling = owningNode.IsFirstSibling;
                        if (owningNode.Level == 1)
                        {
                            if (isFirstSibling && isLastSibling)
                            {
                                graphics.DrawLine(pen, glyphRect.X + 4, cellBounds.Top + (cellBounds.Height / 2), glyphRect.Right, cellBounds.Top + (cellBounds.Height / 2));
                            }
                            else if (isLastSibling)
                            {
                                graphics.DrawLine(pen, glyphRect.X + 4, cellBounds.Top + (cellBounds.Height / 2), glyphRect.Right, cellBounds.Top + (cellBounds.Height / 2));
                                graphics.DrawLine(pen, glyphRect.X + 4, cellBounds.Top, glyphRect.X + 4, cellBounds.Top + (cellBounds.Height / 2));
                            }
                            else if (isFirstSibling)
                            {
                                graphics.DrawLine(pen, glyphRect.X + 4, cellBounds.Top + (cellBounds.Height / 2), glyphRect.Right, cellBounds.Top + (cellBounds.Height / 2));
                                graphics.DrawLine(pen, glyphRect.X + 4, cellBounds.Top + (cellBounds.Height / 2), glyphRect.X + 4, cellBounds.Bottom);
                            }
                            else
                            {
                                graphics.DrawLine(pen, glyphRect.X + 4, cellBounds.Top + (cellBounds.Height / 2), glyphRect.Right, cellBounds.Top + (cellBounds.Height / 2));
                                graphics.DrawLine(pen, glyphRect.X + 4, cellBounds.Top, glyphRect.X + 4, cellBounds.Bottom);
                            }
                        }
                        else
                        {
                            if (isLastSibling)
                            {
                                graphics.DrawLine(pen, glyphRect.X + 4, cellBounds.Top + (cellBounds.Height / 2), glyphRect.Right, cellBounds.Top + (cellBounds.Height / 2));
                                graphics.DrawLine(pen, glyphRect.X + 4, cellBounds.Top, glyphRect.X + 4, cellBounds.Top + (cellBounds.Height / 2));
                            }
                            else
                            {
                                graphics.DrawLine(pen, glyphRect.X + 4, cellBounds.Top + (cellBounds.Height / 2), glyphRect.Right, cellBounds.Top + (cellBounds.Height / 2));
                                graphics.DrawLine(pen, glyphRect.X + 4, cellBounds.Top, glyphRect.X + 4, cellBounds.Bottom);
                            }
                            TreeGridNode parent = owningNode.Parent;
                            for (int i = (glyphRect.X + 4) - this.INDENT_WIDTH; !parent.IsRoot; i -= this.INDENT_WIDTH)
                            {
                                if (!(!parent.HasChildren || parent.IsLastSibling))
                                {
                                    graphics.DrawLine(pen, i, cellBounds.Top, i, cellBounds.Bottom);
                                }
                                parent = parent.Parent;
                            }
                        }
                    }
                }
                if (owningNode.HasChildren || owningNode.BaseTGV.VirtualNodes)
                {
                    if (owningNode.IsExpanded)
                    {
                        graphics.DrawImage(Resources.Expanded1, new Rectangle(glyphRect.X, (glyphRect.Y + (glyphRect.Height / 2)) - 4, this.NodeSize, this.NodeSize));
                    }
                    else
                    {
                        graphics.DrawImage(Resources.Collapse1, new Rectangle(glyphRect.X, (glyphRect.Y + (glyphRect.Height / 2)) - 4, this.NodeSize, this.NodeSize));
                    }
                }
                if (this.IsHaveCheckBox && this.DrawCheckBox)
                {
                    this._checkboxRegion = this.GetCheckBoxRegion(glyphRect, CheckBoxRenderer.GetGlyphSize(graphics, this._PaintState));
                    CheckBoxRenderer.DrawCheckBox(graphics, new Point(this._checkboxRegion.X, this._checkboxRegion.Y), this._PaintState);
                }
            }
        }

        protected internal virtual void Sited()
        {
            this.IsSited = true;
            this.PreviousPadding = base.Style.Padding;
            this.UpdateStyle();
        }

        protected internal virtual void UnSited()
        {
            this.IsSited = false;
            base.Style.Padding = this.PreviousPadding;
        }

        protected internal virtual void UpdateStyle()
        {
            if (this.IsSited)
            {
                Size size;
                this.IsHaveCheckBox = this.OwningNode.BaseTGV.CheckBoxes;
                this.GlyphWidth = this.IsHaveCheckBox ? (2 * this.CheckSize) : this.CheckSize;
                this.INDENT_WIDTH = this.IsHaveCheckBox ? (13 + this.CheckSize) : 13;
                int level = this.Level;
                Padding previousPadding = this.PreviousPadding;
                using (Graphics graphics = this.OwningNode.BaseTGV.CreateGraphics())
                {
                    size = this.GetPreferredSize(graphics, base.InheritedStyle, base.RowIndex, new Size(0, 0));
                }
                Image image = this.OwningNode.Image;
                if (image != null)
                {
                    this.ImageWidth = image.Width + 2;
                    this.ImageHeight = image.Height + 2;
                }
                else
                {
                    this.ImageWidth = 0;
                    this.ImageHeight = 0;
                }
                if (size.Height < this.ImageHeight)
                {
                    base.Style.Padding = new Padding(((previousPadding.Left + this.GlyphMargin) + this.INDENT_WIDTH) + this.ImageWidth, previousPadding.Top + (this.ImageHeight / 2), previousPadding.Right, previousPadding.Bottom + (this.ImageHeight / 2));
                    this.ImageHeightOffset = 2;
                }
                else
                {
                    base.Style.Padding = new Padding(((previousPadding.Left + this.GlyphMargin) + this.INDENT_WIDTH) + this.ImageWidth, previousPadding.Top, previousPadding.Right, previousPadding.Bottom);
                }
                this.CalculatedLeftPadding = (((level - 1) * this.GlyphWidth) + this.ImageWidth) + 5;
            }
        }

        public CheckBoxState _PaintState
        {
            get
            {
                switch (this.OwningNode._CheckState)
                {
                    case CheckState.Unchecked:
                        return (this._moveHot ? CheckBoxState.UncheckedHot : CheckBoxState.UncheckedNormal);

                    case CheckState.Checked:
                        return (this._moveHot ? CheckBoxState.CheckedHot : CheckBoxState.CheckedNormal);
                }
                return (this._moveHot ? CheckBoxState.MixedHot : CheckBoxState.MixedNormal);
            }
        }

        protected virtual int GlyphMargin =>
            (((this.Level - 1) * (this.INDENT_WIDTH - 5)) + 5);

        protected virtual int GlyphOffset =>
            ((this.Level - 1) * this.INDENT_WIDTH);

        public int Level
        {
            get
            {
                TreeGridNode owningNode = this.OwningNode;
                if (owningNode != null)
                {
                    return owningNode.Level;
                }
                return -1;
            }
        }

        public TreeGridNode OwningNode =>
            (base.OwningRow as TreeGridNode);
    }
}

