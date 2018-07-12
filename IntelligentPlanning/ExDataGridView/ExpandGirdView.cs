namespace IntelligentPlanning.ExDataGridView
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    internal class ExpandGirdView : DataGridView
    {
        private bool _ChangeState;
        private CheckState _CheckDefult;
        private bool _CheckVis;
        private Color _mergecolumnheaderbackcolor;
        private Color _mergecolumnheaderforecolor;
        private bool _RunEvent;
        private List<int> _SortColumns;
        private IContainer components;
        private Rectangle dragBoxFromMouseDown;
        private bool externalVirtualMode;
        private bool HeaderRow;
        private int indexOfItemUnderMouseOver;
        private int indexOfItemUnderMouseToDrag;
        private int indexOfItemUnderMouseToDrop;
        public bool IsPaint;
        private bool pDoubleBuf;
        private bool pDragState;
        private Color pLineColor;
        private int pLineHeight;
        private int pRows;
        private Dictionary<int, SpanInfo> SpanRows;
        public DataTable xVirtualTable;

        [Category("Custom"), Description("自定义单元格需要绘制时发生")]
        public event DataGridViewCellPaintingEventHandler CustomCellPainting;

        [Category("Custom"), Description("自定义拖放完成后发生")]
        public event EventHandler CustomDragDrop;

        [Description("自定义单元格绘制结束时发生"), Category("Custom")]
        public event PaintEventHandler CustomPaintingAfter;

        public ExpandGirdView()
        {
            this.components = null;
            this.pDoubleBuf = true;
            this.pRows = 5;
            this._CheckVis = true;
            this._CheckDefult = CheckState.Checked;
            this._ChangeState = true;
            this.pDragState = false;
            this.pLineHeight = 3;
            this.pLineColor = Color.Silver;
            this.externalVirtualMode = false;
            this.indexOfItemUnderMouseToDrag = -1;
            this.indexOfItemUnderMouseToDrop = -1;
            this.indexOfItemUnderMouseOver = -1;
            this.dragBoxFromMouseDown = Rectangle.Empty;
            this.HeaderRow = false;
            this.xVirtualTable = new DataTable();
            this._RunEvent = false;
            this.IsPaint = true;
            this._mergecolumnheaderbackcolor = SystemColors.Control;
            this._mergecolumnheaderforecolor = Color.Black;
            this.SpanRows = new Dictionary<int, SpanInfo>();
            this.InitializeComponent();
        }

        public ExpandGirdView(IContainer container)
        {
            this.components = null;
            this.pDoubleBuf = true;
            this.pRows = 5;
            this._CheckVis = true;
            this._CheckDefult = CheckState.Checked;
            this._ChangeState = true;
            this.pDragState = false;
            this.pLineHeight = 3;
            this.pLineColor = Color.Silver;
            this.externalVirtualMode = false;
            this.indexOfItemUnderMouseToDrag = -1;
            this.indexOfItemUnderMouseToDrop = -1;
            this.indexOfItemUnderMouseOver = -1;
            this.dragBoxFromMouseDown = Rectangle.Empty;
            this.HeaderRow = false;
            this.xVirtualTable = new DataTable();
            this._RunEvent = false;
            this.IsPaint = true;
            this._mergecolumnheaderbackcolor = SystemColors.Control;
            this._mergecolumnheaderforecolor = Color.Black;
            this.SpanRows = new Dictionary<int, SpanInfo>();
            container.Add(this);
            this.InitializeComponent();
            this.SetDgvDoubleBuffered();
            this._RunEvent = true;
        }

        public void AddRow(object[] pValue = null, int pCol = -1, bool pSelect = true, int pInsert = -1)
        {
            try
            {
                if (pInsert == -1)
                {
                    pInsert = base.Rows.Count;
                }
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                dataGridViewRow.CreateCells(this);
                if (pValue != null)
                {
                    for (int i = 0; i < pValue.Length; i++)
                    {
                        dataGridViewRow.Cells[i].Value = pValue[i];
                    }
                }
                base.Rows.Insert(pInsert, dataGridViewRow);
                if (base.MultiSelect)
                {
                    base.ClearSelection();
                }
                if (pSelect)
                {
                    base.Rows[pInsert].Selected = true;
                }
                if (pCol != -1)
                {
                    this.RefreshColWidth(pCol);
                }
                this.RefreshScrollingRowIndex(false);
                this.HeaderCheckBoxHide();
            }
            catch
            {
            }
        }

        public void AddSpanHeader(int ColIndex, int ColCount, string Text)
        {
            int right = (ColIndex + ColCount) - 1;
            this.SpanRows[ColIndex] = new SpanInfo(Text, 1, ColIndex, right);
            this.SpanRows[right] = new SpanInfo(Text, 3, ColIndex, right);
            for (int i = ColIndex + 1; i < right; i++)
            {
                this.SpanRows[i] = new SpanInfo(Text, 2, ColIndex, right);
            }
        }

        public void ClearSpanInfo()
        {
            this.SpanRows.Clear();
        }

        public void DeleteRows(int pindex, int pCol = -1)
        {
            try
            {
                int index = base.Rows[pindex].Index;
                base.Rows.RemoveAt(index);
                if (base.Rows.Count != 0)
                {
                    if (index != base.Rows.Count)
                    {
                        base.Rows[index].Selected = true;
                    }
                    else
                    {
                        base.Rows[base.Rows.Count - 1].Selected = true;
                    }
                    if (pCol != -1)
                    {
                        this.RefreshColWidth(pCol);
                    }
                    this.RefreshScrollingRowIndex(true);
                }
                this.HeaderCheckBoxHide();
            }
            catch
            {
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ExpandGirdView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                e.Value = this.xVirtualTable.Rows[e.RowIndex][e.ColumnIndex];
            }
            catch
            {
            }
        }

        private void ExpandGirdView_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            this.xVirtualTable.Rows[e.RowIndex][e.ColumnIndex] = e.Value;
        }

        private void ExpandGirdView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
        }

        private void ExpandGirdView_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                this.RefreshHeader();
                Point point = base.PointToClient(new Point(e.X, e.Y));
                DataGridView.HitTestInfo info = base.HitTest(point.X, point.Y);
                if ((info.RowIndex != this.indexOfItemUnderMouseToDrag) && (info.RowIndex != (this.indexOfItemUnderMouseToDrag - 1)))
                {
                    this.indexOfItemUnderMouseToDrop = info.RowIndex;
                    DataGridViewRow dataGridViewRow = base.Rows[this.indexOfItemUnderMouseToDrag];
                    base.Rows.RemoveAt(this.indexOfItemUnderMouseToDrag);
                    if (this.indexOfItemUnderMouseToDrag < this.indexOfItemUnderMouseToDrop)
                    {
                        this.indexOfItemUnderMouseToDrop--;
                    }
                    base.Rows.Insert(this.indexOfItemUnderMouseToDrop + 1, dataGridViewRow);
                    base.Rows[this.indexOfItemUnderMouseToDrop + 1].Selected = true;
                    this.indexOfItemUnderMouseToDrag = -1;
                    this.RefreshScrollingRowIndex(false);
                    if (this.CustomDragDrop != null)
                    {
                        this.CustomDragDrop(this, e);
                    }
                }
            }
            catch
            {
            }
        }

        private void ExpandGirdView_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                this.RefreshHeader();
                Point point = base.PointToClient(new Point(e.X, e.Y));
                DataGridView.HitTestInfo info = base.HitTest(point.X, point.Y);
                this.HeaderRow = (info.Type == DataGridViewHitTestType.ColumnHeader) || (point.Y == 1);
                if (!((info.Type == DataGridViewHitTestType.Cell) || this.HeaderRow))
                {
                    e.Effect = DragDropEffects.None;
                    this.OnRowDragOver(-1);
                }
                else
                {
                    e.Effect = DragDropEffects.Move;
                    this.OnRowDragOver(info.RowIndex);
                }
            }
            catch
            {
            }
        }

        private void ExpandGirdView_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo info = base.HitTest(e.X, e.Y);
                if (info.Type == DataGridViewHitTestType.Cell)
                {
                    this.indexOfItemUnderMouseToDrag = info.RowIndex;
                    if (this.indexOfItemUnderMouseToDrag > -1)
                    {
                        Size dragSize = SystemInformation.DragSize;
                        this.dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
                    }
                    else
                    {
                        this.dragBoxFromMouseDown = Rectangle.Empty;
                    }
                }
            }
            catch
            {
            }
        }

        private void ExpandGirdView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if ((((e.Button & MouseButtons.Left) == MouseButtons.Left) && ((this.dragBoxFromMouseDown != Rectangle.Empty) && !this.dragBoxFromMouseDown.Contains(e.X, e.Y))) && (this.indexOfItemUnderMouseToDrag >= 0))
                {
                    DataGridViewRow data = base.Rows[this.indexOfItemUnderMouseToDrag];
                    DragDropEffects effects = base.DoDragDrop(data, DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll);
                    this.OnRowDragOver(-1);
                }
            }
            catch
            {
            }
        }

        private void ExpandGirdView_MouseUp(object sender, MouseEventArgs e)
        {
            this.dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void ExpandGirdView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                Brush brush = new SolidBrush(this.pLineColor);
                if ((e.RowIndex == 0) && this.HeaderRow)
                {
                    e.Graphics.FillRectangle(brush, e.RowBounds.X, e.RowBounds.Y, e.RowBounds.Width, this.pLineHeight);
                }
                else if (e.RowIndex == this.indexOfItemUnderMouseOver)
                {
                    int num = base.Rows[e.RowIndex].Height - 3;
                    e.Graphics.FillRectangle(brush, e.RowBounds.X, e.RowBounds.Y + num, e.RowBounds.Width, this.pLineHeight);
                }
            }
            catch
            {
            }
        }

        private void ExpandGirdView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            try
            {
                if (this._SortColumns.Contains(e.Column.Index))
                {
                    e.SortResult = ((Convert.ToDouble(e.CellValue1) - Convert.ToDouble(e.CellValue2)) > 0.0) ? 1 : (((Convert.ToDouble(e.CellValue1) - Convert.ToDouble(e.CellValue2)) < 0.0) ? -1 : 0);
                }
                else
                {
                    e.SortResult = string.Compare(Convert.ToString(e.CellValue1), Convert.ToString(e.CellValue2));
                }
                e.Handled = true;
            }
            catch
            {
            }
        }

        public int GetTotalWidth()
        {
            int num = 0;
            try
            {
                foreach (DataGridViewColumn column in base.Columns)
                {
                    num += column.Width + 1;
                }
            }
            catch
            {
            }
            return (num - 1);
        }

        public void HeaderCheckBoxHide()
        {
            foreach (DataGridViewColumn column in base.Columns)
            {
                if (column is ExCheckBoxColumn)
                {
                    ((ExCheckBoxColumn) column).ExHeaderCell._CheckVis = base.Rows.Count > 0;
                    ((ExCheckBoxColumn) column).OnCheckValueChanged(null, null);
                    break;
                }
            }
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            ((ISupportInitialize) this).BeginInit();
            base.SuspendLayout();
            base.AllowUserToAddRows = false;
            base.AllowUserToDeleteRows = false;
            base.AllowUserToResizeRows = false;
            base.BackgroundColor = SystemColors.Window;
            base.BorderStyle = BorderStyle.Fixed3D;
            base.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style.BackColor = SystemColors.Control;
            style.Font = new Font("Tahoma", 9f);
            style.ForeColor = SystemColors.WindowText;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.True;
            base.ColumnHeadersDefaultCellStyle = style;
            base.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            base.GridColor = Color.Silver;
            base.MultiSelect = false;
            base.RowHeadersVisible = false;
            style2.SelectionBackColor = Color.FromArgb(0xde, 0xe8, 0xfc);
            style2.SelectionForeColor = Color.Black;
            base.RowsDefaultCellStyle = style2;
            base.RowTemplate.Height = 0x17;
            base.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ((ISupportInitialize) this).EndInit();
            base.ResumeLayout(false);
        }

        public void LoadDrag()
        {
            try
            {
                this.AllowDrop = true;
                base.MultiSelect = false;
                base.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                base.MouseDown += new MouseEventHandler(this.ExpandGirdView_MouseDown);
                base.MouseUp += new MouseEventHandler(this.ExpandGirdView_MouseUp);
                base.MouseMove += new MouseEventHandler(this.ExpandGirdView_MouseMove);
                base.DragDrop += new DragEventHandler(this.ExpandGirdView_DragDrop);
                base.DragOver += new DragEventHandler(this.ExpandGirdView_DragOver);
                base.RowPostPaint += new DataGridViewRowPostPaintEventHandler(this.ExpandGirdView_RowPostPaint);
            }
            catch
            {
            }
        }

        public void LoadInitialization(List<int> pType, List<string> pText, List<int> pWidth, List<bool> pRead = null, List<bool> pVis = null, List<int> pSort = null)
        {
            if (this._RunEvent)
            {
                try
                {
                    this._RunEvent = false;
                    using (List<int>.Enumerator enumerator = pType.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            switch (enumerator.Current)
                            {
                                case 0:
                                    base.Columns.Add(new ExCheckBoxColumn(this._CheckVis, this._CheckDefult, this._ChangeState));
                                    this.HeaderCheckBoxHide();
                                    break;

                                case 1:
                                    base.Columns.Add(new DataGridViewTextBoxColumn());
                                    break;

                                case 2:
                                    base.Columns.Add(new ImageTextColumn());
                                    break;

                                case 3:
                                    base.Columns.Add(new DataGridViewComboBoxColumn());
                                    break;

                                case 4:
                                    base.Columns.Add(new DataGridViewImageColumn());
                                    break;
                            }
                        }
                    }
                    int num2 = 0;
                    foreach (DataGridViewColumn column in base.Columns)
                    {
                        column.Name = "Column" + num2;
                        num2++;
                    }
                    num2 = 0;
                    foreach (DataGridViewColumn column in base.Columns)
                    {
                        if (column is DataGridViewCheckBoxColumn)
                        {
                            column.HeaderText = "";
                        }
                        else
                        {
                            column.HeaderText = pText[num2];
                            num2++;
                        }
                    }
                    num2 = 0;
                    foreach (DataGridViewColumn column in base.Columns)
                    {
                        column.Width = pWidth[num2];
                        column.Tag = pWidth[num2];
                        num2++;
                    }
                    if (pRead != null)
                    {
                        num2 = 0;
                        foreach (DataGridViewColumn column in base.Columns)
                        {
                            column.ReadOnly = pRead[num2];
                            num2++;
                        }
                    }
                    if (pVis != null)
                    {
                        num2 = 0;
                        foreach (DataGridViewColumn column in base.Columns)
                        {
                            column.Visible = pVis[num2];
                            num2++;
                        }
                    }
                    this._SortColumns = pSort;
                    if (this._SortColumns != null)
                    {
                        base.SortCompare += new DataGridViewSortCompareEventHandler(this.ExpandGirdView_SortCompare);
                    }
                    if (this.pDragState)
                    {
                        this.LoadDrag();
                    }
                    if (base.VirtualMode)
                    {
                        this.LoadVirtualMode();
                    }
                }
                catch
                {
                }
                this._RunEvent = true;
            }
        }

        public void LoadShape(int pCols, Image pdefault, Image pHot, Image pPressed, int pImageSize)
        {
            if (this._RunEvent)
            {
                try
                {
                    this._RunEvent = false;
                    for (int i = 0; i < pCols; i++)
                    {
                        base.Columns.Add(new ShapeTextColumn(pdefault, pHot, pPressed, pImageSize));
                    }
                    int num2 = 0;
                    foreach (DataGridViewColumn column in base.Columns)
                    {
                        column.Name = "Column" + num2;
                        num2++;
                    }
                    base.ColumnHeadersVisible = false;
                    base.SelectionMode = DataGridViewSelectionMode.CellSelect;
                    base.AllowUserToResizeColumns = false;
                    base.RowTemplate.Height = pImageSize + 4;
                    num2 = 0;
                    foreach (DataGridViewColumn column in base.Columns)
                    {
                        column.Width = pImageSize + 4;
                        num2++;
                    }
                    num2 = 0;
                    foreach (DataGridViewColumn column in base.Columns)
                    {
                        column.ReadOnly = true;
                        num2++;
                    }
                    num2 = 0;
                    foreach (DataGridViewColumn column in base.Columns)
                    {
                        column.Visible = true;
                        num2++;
                    }
                }
                catch
                {
                }
                this._RunEvent = true;
            }
        }

        public void LoadVirtualMode()
        {
            try
            {
                if (!this.ExternalVirtualMode)
                {
                    foreach (DataGridViewColumn column in base.Columns)
                    {
                        if (column is DataGridViewCheckBoxColumn)
                        {
                            this.xVirtualTable.Columns.Add("", typeof(bool));
                        }
                        else if (column is DataGridViewTextBoxColumn)
                        {
                            this.xVirtualTable.Columns.Add(column.HeaderText, typeof(string));
                        }
                    }
                    base.CellValueNeeded += new DataGridViewCellValueEventHandler(this.ExpandGirdView_CellValueNeeded);
                    base.CellValuePushed += new DataGridViewCellValueEventHandler(this.ExpandGirdView_CellValuePushed);
                }
            }
            catch
            {
            }
        }

        public bool MoveDown()
        {
            bool flag = false;
            try
            {
                int index = base.SelectedRows[0].Index;
                if ((index != (base.Rows.Count - 1)) && (index != -1))
                {
                    DataGridViewRow dataGridViewRow = base.Rows[index];
                    base.Rows.RemoveAt(index);
                    index++;
                    base.Rows.Insert(index, dataGridViewRow);
                    base.Rows[index].Selected = true;
                    this.RefreshScrollingRowIndex(false);
                    flag = index != (base.Rows.Count - 1);
                }
            }
            catch
            {
            }
            return flag;
        }

        public bool MoveUp()
        {
            bool flag = false;
            try
            {
                int index = base.SelectedRows[0].Index;
                if (index > 0)
                {
                    DataGridViewRow dataGridViewRow = base.Rows[index];
                    base.Rows.RemoveAt(index);
                    index--;
                    base.Rows.Insert(index, dataGridViewRow);
                    base.Rows[index].Selected = true;
                    this.RefreshScrollingRowIndex(true);
                    flag = index > 0;
                }
            }
            catch
            {
            }
            return flag;
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == -1)
                {
                    if (this.SpanRows.ContainsKey(e.ColumnIndex))
                    {
                        Graphics graphics = e.Graphics;
                        e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                        int num = e.CellBounds.Left;
                        int num2 = e.CellBounds.Top + 2;
                        int num3 = e.CellBounds.Right;
                        int bottom = e.CellBounds.Bottom;
                        switch (this.SpanRows[e.ColumnIndex].Position)
                        {
                            case 1:
                                num += 2;
                                break;
                            case 3:
                                num3 -= 2;
                                break;
                        }
                        graphics.FillRectangle(new SolidBrush(this._mergecolumnheaderbackcolor), num, num2, num3 - num, (bottom - num2) / 2);
                        graphics.DrawLine(new Pen(base.GridColor), num, (num2 + bottom) / 2, num3, (num2 + bottom) / 2);
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        SolidBrush brush = new SolidBrush(this._mergecolumnheaderforecolor);
                        graphics.DrawString(string.Concat(e.Value), e.CellStyle.Font, brush, new Rectangle(num, (num2 + bottom) / 2, num3 - num, (bottom - num2) / 2), stringFormat);
                        num = base.GetColumnDisplayRectangle(this.SpanRows[e.ColumnIndex].Left, true).Left - 2;
                        if (num < 0)
                        {
                            num = base.GetCellDisplayRectangle(-1, -1, true).Width;
                        }
                        num3 = base.GetColumnDisplayRectangle(this.SpanRows[e.ColumnIndex].Right, true).Right - 2;
                        if (num3 < 0)
                        {
                            num3 = base.Width;
                        }
                        graphics.DrawString(this.SpanRows[e.ColumnIndex].Text, e.CellStyle.Font, brush, new Rectangle(num, num2, num3 - num, (bottom - num2) / 2), stringFormat);
                        e.Handled = true;
                    }
                }
                else
                {
                    e.PaintBackground(e.CellBounds, true);
                    e.PaintContent(e.CellBounds);
                    if (this.CustomCellPainting != null)
                    {
                        this.CustomCellPainting(this, e);
                    }
                    e.Handled = true;
                }
                base.OnCellPainting(e);
            }
            catch
            {
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (this.CustomPaintingAfter != null)
            {
                this.CustomPaintingAfter(this, pe);
            }
        }

        private void OnRowDragOver(int rowIndex)
        {
            try
            {
                if (this.indexOfItemUnderMouseOver != rowIndex)
                {
                    int indexOfItemUnderMouseOver = this.indexOfItemUnderMouseOver;
                    this.indexOfItemUnderMouseOver = rowIndex;
                    if (indexOfItemUnderMouseOver > -1)
                    {
                        base.InvalidateRow(indexOfItemUnderMouseOver);
                    }
                    if (rowIndex > -1)
                    {
                        base.InvalidateRow(rowIndex);
                    }
                }
            }
            catch
            {
            }
        }

        public void ReDrawHead()
        {
            try
            {
                foreach (int num in this.SpanRows.Keys)
                {
                    base.Invalidate(base.GetCellDisplayRectangle(num, -1, true));
                }
            }
            catch
            {
            }
        }

        public void RefreshColWidth(int pCol)
        {
            try
            {
                int num = Convert.ToInt32(base.Columns[pCol].Tag);
                base.Columns[pCol].Width = (base.Rows.Count > this.pRows) ? (num - 0x11) : num;
            }
            catch
            {
            }
        }

        private void RefreshHeader()
        {
            try
            {
                if (this.HeaderRow)
                {
                    this.HeaderRow = false;
                    base.InvalidateRow(0);
                }
            }
            catch
            {
            }
        }

        public void RefreshScrollingRowIndex(bool MoveUp)
        {
            try
            {
                if (MoveUp)
                {
                    if ((base.FirstDisplayedScrollingRowIndex > base.SelectedRows[0].Index) || ((base.SelectedRows[0].Index - base.FirstDisplayedScrollingRowIndex) >= this.pRows))
                    {
                        base.FirstDisplayedScrollingRowIndex = base.SelectedRows[0].Index;
                    }
                }
                else if (((base.SelectedRows[0].Index - base.FirstDisplayedScrollingRowIndex) >= this.pRows) || (base.FirstDisplayedScrollingRowIndex > base.SelectedRows[0].Index))
                {
                    base.FirstDisplayedScrollingRowIndex = ((base.SelectedRows[0].Index - this.pRows) < 0) ? 0 : ((base.SelectedRows[0].Index - this.pRows) + 1);
                }
            }
            catch
            {
            }
        }

        public void SetControlEnable(List<Control> pControl)
        {
            try
            {
                bool flag = base.Rows.Count > 0;
                foreach (Control control in pControl)
                {
                    control.Enabled = flag;
                }
            }
            catch
            {
            }
        }

        public void SetDgvDoubleBuffered()
        {
            try
            {
                if (this.DgvDoubleBuffered)
                {
                    base.GetType().GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, true, null);
                }
            }
            catch
            {
            }
        }

        [Description("表头复选框的状态改变"), DefaultValue(true), Category("Custom")]
        public bool ChangeHeaderState
        {
            get => 
                this._ChangeState;
            set
            {
                this._ChangeState = value;
                base.Invalidate();
            }
        }

        [Category("Custom"), DefaultValue(true), Description("设置双缓存")]
        public bool DgvDoubleBuffered
        {
            get => 
                this.pDoubleBuf;
            set
            {
                this.pDoubleBuf = value;
                base.Invalidate();
            }
        }

        [Description("拖拉线条的颜色"), DefaultValue(3), Category("DragRow")]
        public Color DragLineColor
        {
            get => 
                this.pLineColor;
            set
            {
                this.pLineColor = value;
                base.Invalidate();
            }
        }

        [Category("DragRow"), DefaultValue(3), Description("拖拉线条的高度")]
        public int DragLineHeight
        {
            get => 
                this.pLineHeight;
            set
            {
                this.pLineHeight = value;
                base.Invalidate();
            }
        }

        [DefaultValue(false), Description("支持拖拉效果"), Category("DragRow")]
        public bool DragState
        {
            get => 
                this.pDragState;
            set
            {
                this.pDragState = value;
                base.Invalidate();
            }
        }

        [Category("Custom"), Description("是否启动外置的虚拟模式"), DefaultValue(false)]
        public bool ExternalVirtualMode
        {
            get => 
                this.externalVirtualMode;
            set
            {
                this.externalVirtualMode = value;
            }
        }

        [DefaultValue(true), Description("表头复选框的默认状态"), Category("Custom")]
        public CheckState HeadersCheckDefult
        {
            get => 
                this._CheckDefult;
            set
            {
                this._CheckDefult = value;
                base.Invalidate();
            }
        }

        [Category("Custom"), DefaultValue(true), Description("表头复选框的可见性")]
        public bool HeadersCheckVisible
        {
            get => 
                this._CheckVis;
            set
            {
                this._CheckVis = value;
                base.Invalidate();
            }
        }

        [Browsable(true), Category("二维表头"), Description("二维表头的背景颜色")]
        public Color MergeColumnHeaderBackColor
        {
            get => 
                this._mergecolumnheaderbackcolor;
            set
            {
                this._mergecolumnheaderbackcolor = value;
            }
        }

        [Description("二维表头的字体颜色"), Category("二维表头"), Browsable(true)]
        public Color MergeColumnHeaderForeColor
        {
            get => 
                this._mergecolumnheaderforecolor;
            set
            {
                this._mergecolumnheaderforecolor = value;
            }
        }

        [Category("Custom"), DefaultValue(true), Description("设置行的数量")]
        public int RowNum
        {
            get => 
                this.pRows;
            set
            {
                this.pRows = value;
                base.Height = (base.ColumnHeadersVisible ? 0x19 : 3) + (0x17 * this.pRows);
                base.Invalidate();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SpanInfo
        {
            public string Text;
            public int Position;
            public int Left;
            public int Right;
            public SpanInfo(string Text, int Position, int Left, int Right)
            {
                this.Text = Text;
                this.Position = Position;
                this.Left = Left;
                this.Right = Right;
            }
        }
    }
}

