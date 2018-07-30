namespace IntelligentPlanning.ExDataGridView
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class TreeGridView : DataGridView
    {
        private bool _CheckBox = true;
        private bool _CheckDefult = true;
        private bool _disposing = false;
        public bool _drawContent = true;
        private TreeGridColumn _expandableColumn;
        [Description("指示是否在标题旁边显示复选框"), Category("外观")]
        private bool _HeaderCheckBoxes = true;
        internal System.Windows.Forms.ImageList _imageList;
        private int _indentWidth;
        private bool _inExpandCollapse = false;
        private bool _IsWork = false;
        public TreeGridNode _root;
        private bool _showLines = true;
        private int _treedGridColumnsIndex = -1;
        private bool _virtualNodes = false;
        private IContainer components = null;
        private Control hideScrollBarControl;
        internal bool InExpandCollapseMouseCapture = false;
        private int[] LeftList;

        [Description("复选框发生变化时发生")]
        public event TreeGridNodeEventHandler CheckedChanged;

        [Description("复选框发生变化后发生")]
        public event TreeGridNodeEventHandler CheckedLate;

        [Description("标题复选框改变时发生")]
        public event EventHandler HeaderCheckedChanged;

        [Description("收缩结束后触发")]
        public event CollapsedEventHandler NodeCollapsed;

        [Description("收缩过程中触发")]
        public event CollapsingEventHandler NodeCollapsing;

        [Description("伸展结束后触发")]
        public event ExpandedEventHandler NodeExpanded;

        [Description("伸展过程中触发")]
        public event ExpandingEventHandler NodeExpanding;

        public TreeGridView()
        {
            this.InitializeComponent();
            this.RowTemplate = new TreeGridNode();
            base.AllowUserToAddRows = false;
            base.AllowUserToDeleteRows = false;
            this._root = new TreeGridNode(this);
            this._root.IsRoot = true;
            base.Rows.CollectionChanged += delegate (object sender, CollectionChangeEventArgs e) {
            };
            this.SetDgvDoubleBuffered();
            this._IsWork = true;
        }

        public void AddCheckBox()
        {
            try
            {
                CheckBox box = new CheckBox();
                base.Controls.Add(box);
                box.Checked = this._CheckDefult;
                box.Size = new Size(15, 14);
                box.Location = new Point(4, 9);
                box.Visible = this._HeaderCheckBoxes;
                box.CheckedChanged += new EventHandler(this.Ckb_Selcet_CheckedChanged);
            }
            catch
            {
            }
        }

        public void Ckb_Selcet_CheckedChanged(object sender, EventArgs e)
        {
            if (this._IsWork)
            {
                try
                {
                    CheckState checkState = ((CheckBox)sender).CheckState;
                    this._root._CheckState = checkState;
                    this.RefreshCheckStatus(this._root, true, false);
                    this.OnHeaderCheckedChanged(sender, e);
                }
                catch
                {
                }
            }
        }

        protected internal virtual bool CollapseNode(TreeGridNode node)
        {
            if (node.IsExpanded)
            {
                CollapsingEventArgs e = new CollapsingEventArgs(node);
                this.OnNodeCollapsing(e);
                if (!e.Cancel)
                {
                    this.LockVerticalScrollBarUpdate(true);
                    base.SuspendLayout();
                    this._inExpandCollapse = true;
                    node.IsExpanded = false;
                    foreach (TreeGridNode node2 in node.Nodes)
                    {
                        this.UnSiteNode(node2);
                    }
                    CollapsedEventArgs args2 = new CollapsedEventArgs(node);
                    this.OnNodeCollapsed(args2);
                    this._inExpandCollapse = false;
                    this.LockVerticalScrollBarUpdate(false);
                    base.ResumeLayout(true);
                }
                return !e.Cancel;
            }
            return false;
        }

        private void CountChildren(TreeGridNode node, ref int pCount)
        {
            if (node != null)
            {
                if (!(node.HasChildren || this.VirtualNodes))
                {
                    pCount++;
                }
                foreach (TreeGridNode node2 in node.Nodes)
                {
                    this.CountChildren(node2, ref pCount);
                }
            }
        }

        public void DeleteNode(TreeGridNode deleteNode)
        {
            if ((deleteNode != null) && (deleteNode != this._root))
            {
                TreeGridNode parentNode = deleteNode.ParentNode;
                parentNode.Nodes.Remove(deleteNode);
                int num = 0;
                foreach (TreeGridNode node2 in parentNode.Nodes)
                {
                    node2.Index = num++;
                }
                this.RefreshCheckStatus(parentNode, false, false);
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                this._disposing = true;
                if (disposing && (this.components != null))
                {
                    this.components.Dispose();
                }
                base.Dispose(disposing);
                this.Nodes.Clear();
            }
            catch
            {
            }
        }

        protected internal virtual bool ExpandNode(TreeGridNode node)
        {
            if (!node.IsExpanded || this._virtualNodes)
            {
                ExpandingEventArgs e = new ExpandingEventArgs(node);
                this.OnNodeExpanding(e);
                if (!e.Cancel)
                {
                    this.LockVerticalScrollBarUpdate(true);
                    base.SuspendLayout();
                    this._inExpandCollapse = true;
                    node.IsExpanded = true;
                    foreach (TreeGridNode node2 in node.Nodes)
                    {
                        this.SiteNode(node2);
                    }
                    ExpandedEventArgs args2 = new ExpandedEventArgs(node);
                    this.OnNodeExpanded(args2);
                    this._inExpandCollapse = false;
                    this.LockVerticalScrollBarUpdate(false);
                    base.ResumeLayout(true);
                }
                return !e.Cancel;
            }
            return false;
        }

        public Dictionary<string, CheckState> GetCheckList(int pIndex)
        {
            Dictionary<string, CheckState> dictionary = new Dictionary<string, CheckState>();
            foreach (TreeGridNode node in this.Nodes)
            {
                string str = node.Cells[pIndex].Value.ToString();
                dictionary[str] = node._CheckState;
                foreach (TreeGridNode node2 in node.Nodes)
                {
                    string str2 = node2.Cells[pIndex].Value.ToString();
                    dictionary[str2] = node2._CheckState;
                    foreach (TreeGridNode node3 in node2.Nodes)
                    {
                        string str3 = node3.Cells[pIndex].Value.ToString();
                        dictionary[str3] = node3._CheckState;
                    }
                }
            }
            return dictionary;
        }

        public List<string> GetExpandList()
        {
            List<string> list = new List<string>();
            if (this.Nodes.Count != 0)
            {
                foreach (TreeGridNode node in this.Nodes)
                {
                    if (node.IsExpanded)
                    {
                        string item = node.Cells[0].Value.ToString();
                        list.Add(item);
                        foreach (TreeGridNode node2 in node.Nodes)
                        {
                            if (node2.IsExpanded)
                            {
                                string str2 = item + node2.Cells[0].Value.ToString();
                                list.Add(str2);
                                foreach (TreeGridNode node3 in node2.Nodes)
                                {
                                    if (node3.IsExpanded)
                                    {
                                        string str3 = str2 + node3.Cells[0].Value.ToString();
                                        list.Add(str3);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }

        public CheckBox GetHeaderCheckBox()
        {
            foreach (Control control in base.Controls)
            {
                if (control is CheckBox)
                {
                    return (control as CheckBox);
                }
            }
            return null;
        }

        [Description("返回指定索引对应的结点")]
        public TreeGridNode GetNodeForRow(int index) =>
            this.GetNodeForRow(base.Rows[index]);

        [Description("返回指定行对应的结点")]
        public TreeGridNode GetNodeForRow(DataGridViewRow row) =>
            (row as TreeGridNode);

        public List<string> GetSelectList(int pIndex)
        {
            List<string> list = new List<string>();
            foreach (TreeGridNode node in this.Nodes)
            {
                foreach (TreeGridNode node2 in node.Nodes)
                {
                    string str;
                    if (node2.HasChildren)
                    {
                        foreach (TreeGridNode node3 in node2.Nodes)
                        {
                            if (node3._CheckState == CheckState.Checked)
                            {
                                str = node3.Cells[pIndex].Value.ToString();
                                list.Add(str);
                            }
                        }
                    }
                    else if (node2._CheckState == CheckState.Checked)
                    {
                        str = node2.Cells[pIndex].Value.ToString();
                        list.Add(str);
                    }
                }
            }
            return list;
        }

        public string GetSelectValue(int pColumnIndex, bool pGetTag = false)
        {
            string str = "";
            if (this.Nodes.Count <= 0)
            {
                return str;
            }
            TreeGridNode selectedNode = this.SelectedNode;
            if (selectedNode.HasChildren)
            {
                return str;
            }
            if (pGetTag)
            {
                return selectedNode.Tag.ToString();
            }
            return selectedNode.Cells[pColumnIndex].Value.ToString();
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            ((ISupportInitialize)this).BeginInit();
            base.SuspendLayout();
            base.AllowUserToAddRows = false;
            base.AllowUserToDeleteRows = false;
            base.AllowUserToResizeRows = false;
            base.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            base.BackgroundColor = SystemColors.Window;
            base.BorderStyle = BorderStyle.Fixed3D;
            base.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            base.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            base.GridColor = Color.Silver;
            base.MultiSelect = false;
            base.RowHeadersVisible = false;
            style.SelectionBackColor = Color.FromArgb(0xde, 0xe8, 0xfc);
            style.SelectionForeColor = Color.Black;
            base.RowsDefaultCellStyle = style;
            this.RowTemplate.Height = 0x17;
            base.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ((ISupportInitialize)this).EndInit();
            base.ResumeLayout(false);
        }

        public void LoadInitialization(List<int> pType, List<string> pText, List<int> pWidth, List<bool> pRead = null, List<bool> pVis = null)
        {
            if (this._IsWork)
            {
                try
                {
                    this._IsWork = false;
                    int num = 0;
                    using (List<int>.Enumerator enumerator = pType.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            switch (enumerator.Current)
                            {
                                case 0:
                                    base.Columns.Add(new TreeGridColumn());
                                    this._treedGridColumnsIndex = num;
                                    break;

                                case 1:
                                    base.Columns.Add(new DataGridViewTextBoxColumn());
                                    break;

                                case 2:
                                    base.Columns.Add(new DataGridViewComboBoxColumn());
                                    break;
                            }
                            num++;
                        }
                    }
                    int num3 = 0;
                    foreach (DataGridViewColumn column in base.Columns)
                    {
                        column.Name = "Column" + num3;
                        num3++;
                    }
                    num3 = 0;
                    foreach (DataGridViewColumn column in base.Columns)
                    {
                        if (column is DataGridViewCheckBoxColumn)
                        {
                            column.HeaderText = "";
                        }
                        else
                        {
                            column.HeaderText = pText[num3];
                            num3++;
                        }
                    }
                    num3 = 0;
                    foreach (DataGridViewColumn column in base.Columns)
                    {
                        column.Width = pWidth[num3];
                        column.Tag = pWidth[num3];
                        num3++;
                    }
                    if (pVis != null)
                    {
                        num3 = 0;
                        foreach (DataGridViewColumn column in base.Columns)
                        {
                            column.Visible = pVis[num3];
                            num3++;
                        }
                    }
                    if (pRead != null)
                    {
                        num3 = 0;
                        foreach (DataGridViewColumn column in base.Columns)
                        {
                            column.ReadOnly = pRead[num3];
                            num3++;
                        }
                    }
                    if (this._CheckBox)
                    {
                        this.AddCheckBox();
                    }
                    this.LeftList = new int[pType.Count];
                }
                catch
                {
                }
                this._IsWork = true;
            }
        }

        private void LockVerticalScrollBarUpdate(bool lockUpdate)
        {
            if (!this._inExpandCollapse)
            {
                if (lockUpdate)
                {
                    base.VerticalScrollBar.Parent = this.hideScrollBarControl;
                }
                else
                {
                    base.VerticalScrollBar.Parent = this;
                }
            }
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            int left = e.CellBounds.Left;
            int top = e.CellBounds.Top;
            int width = e.CellBounds.Width;
            int height = e.CellBounds.Height;
            e.Handled = true;
            e.PaintBackground(e.CellBounds, true);
            if (this._drawContent)
            {
                e.PaintContent(e.CellBounds);
                Brush brush = new SolidBrush(base.GridColor);
                Pen pen = new Pen(brush);
                if (!((e.RowIndex != 0) || base.ColumnHeadersVisible))
                {
                    e.Graphics.DrawLine(pen, left, top, left + width, top);
                }
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == this._treedGridColumnsIndex)
                    {
                        TreeGridCell cell = this.Rows[e.RowIndex].Cells[e.ColumnIndex] as TreeGridCell;
                        left = cell.Style.Padding.Left;
                    }
                    e.Graphics.DrawLine(pen, left, (top + height) - 1, left + width, (top + height) - 1);
                }
            }
        }

        public virtual void OnCheckedChanged(object sender, TreeGridNodeEventBase e)
        {
            if (this.CheckedChanged != null)
            {
                this.CheckedChanged(sender, e);
            }
        }

        public virtual void OnCheckedLate(object sender, TreeGridNodeEventBase e)
        {
            if (this.CheckedLate != null)
            {
                this.CheckedLate(sender, e);
            }
        }

        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        {
            if (typeof(TreeGridColumn).IsAssignableFrom(e.Column.GetType()) && (this._expandableColumn == null))
            {
                this._expandableColumn = (TreeGridColumn)e.Column;
            }
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
            base.OnColumnAdded(e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.hideScrollBarControl = new Control();
            this.hideScrollBarControl.Visible = false;
            this.hideScrollBarControl.Enabled = false;
            this.hideScrollBarControl.TabStop = false;
            base.Controls.Add(this.hideScrollBarControl);
        }

        public virtual void OnHeaderCheckedChanged(object sender, EventArgs e)
        {
            if (this.HeaderCheckedChanged != null)
            {
                this.HeaderCheckedChanged(sender, e);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!this.InExpandCollapseMouseCapture)
            {
                base.OnMouseMove(e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.InExpandCollapseMouseCapture = false;
        }

        protected virtual void OnNodeCollapsed(CollapsedEventArgs e)
        {
            if (this.NodeCollapsed != null)
            {
                this.NodeCollapsed(this, e);
            }
        }

        protected virtual void OnNodeCollapsing(CollapsingEventArgs e)
        {
            if (this.NodeCollapsing != null)
            {
                this.NodeCollapsing(this, e);
            }
        }

        protected virtual void OnNodeExpanded(ExpandedEventArgs e)
        {
            if (this.NodeExpanded != null)
            {
                this.NodeExpanded(this, e);
            }
        }

        protected virtual void OnNodeExpanding(ExpandingEventArgs e)
        {
            if (this.NodeExpanding != null)
            {
                this.NodeExpanding(this, e);
            }
        }

        protected override void OnRowEnter(DataGridViewCellEventArgs e)
        {
        }

        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
        {
        }

        protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
        {
            base.OnRowsAdded(e);
            for (int i = e.RowCount - 1; i >= 0; i--)
            {
                TreeGridNode node = base.Rows[e.RowIndex + i] as TreeGridNode;
                if (node != null)
                {
                    node.Sited();
                }
            }
        }

        public void RefreshCheckStatus(TreeGridNode node, bool SetChiderNode = true, bool pChange = false)
        {
            try
            {
                this._IsWork = false;
                int pCount = 0;
                this.CountChildren(node, ref pCount);
                if (SetChiderNode)
                {
                    this.SetChildrenCheckStatus(node, node._CheckState, pChange);
                }
                this.SetParentCheckStatus(node);
                this.GetHeaderCheckBox().CheckState = this._root._CheckState;
                base.Invalidate();
            }
            catch
            {
            }
            this._IsWork = true;
        }

        public void SelectFirstNode()
        {
            TreeGridNode node = this.Nodes[0];
            node.Expand();
            node = node.Nodes[0];
            if (node.HasChildren)
            {
                node.Expand();
                node = node.Nodes[0];
            }
            node.Selected = true;
        }

        public void SetCheckBoxVisible(bool pVisible)
        {
            foreach (Control control in base.Controls)
            {
                if (control is CheckBox)
                {
                    CheckBox box = control as CheckBox;
                    box.Visible = pVisible;
                    break;
                }
            }
        }

        private void SetChildrenCheckStatus(TreeGridNode node, CheckState pCheckState, bool pChange = false)
        {
            if (node != null)
            {
                if (node.HasChildren || this.VirtualNodes)
                {
                    if (pChange)
                    {
                        this.InExpandCollapseMouseCapture = true;
                        if (pCheckState == CheckState.Unchecked)
                        {
                            node.Collapse();
                        }
                        else
                        {
                            node.Expand();
                        }
                    }
                }
                else
                {
                    TreeGridNodeEventBase e = new TreeGridNodeEventBase(node);
                    this.OnCheckedChanged(this, e);
                }
                base.Invalidate();
                foreach (TreeGridNode node2 in node.Nodes)
                {
                    node2._CheckState = pCheckState;
                    this.SetChildrenCheckStatus(node2, pCheckState, pChange);
                }
            }
        }

        public void SetColor(int pIndex, string pValue, Color pColor)
        {
            foreach (TreeGridNode node in this.Nodes)
            {
                foreach (TreeGridNode node2 in node.Nodes)
                {
                    if (node2.Cells[pIndex].Value.ToString() == pValue)
                    {
                        node2.Cells[pIndex].Style.ForeColor = node2.Cells[pIndex].Style.SelectionForeColor = pColor;
                        break;
                    }
                }
            }
        }

        public void SetDgvDoubleBuffered()
        {
            try
            {
                base.GetType().GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, true, null);
            }
            catch
            {
            }
        }

        private void SetHeadCheckState()
        {
            try
            {
                this._IsWork = false;
                if (this._CheckBox)
                {
                    CheckBox headerCheckBox = this.GetHeaderCheckBox();
                    int num = 0;
                    bool flag = false;
                    if (this.Nodes.Count != 0)
                    {
                        foreach (TreeGridNode node in this.Nodes)
                        {
                            if (node._CheckState != CheckState.Unchecked)
                            {
                                num++;
                            }
                            if (node._CheckState == CheckState.Indeterminate)
                            {
                                flag = true;
                            }
                        }
                        if (num == this.Nodes.Count)
                        {
                            headerCheckBox.CheckState = flag ? CheckState.Indeterminate : CheckState.Checked;
                        }
                        else if (num == 0)
                        {
                            headerCheckBox.CheckState = CheckState.Unchecked;
                        }
                        else
                        {
                            headerCheckBox.CheckState = CheckState.Indeterminate;
                        }
                    }
                    headerCheckBox.Visible = this.Nodes.Count != 0;
                }
            }
            catch
            {
            }
            this._IsWork = true;
        }

        private void SetParentCheckStatus(TreeGridNode node)
        {
            int num = 0;
            bool flag = false;
            if (node.Nodes.Count != 0)
            {
                foreach (TreeGridNode node2 in node.Nodes)
                {
                    if (node2._CheckState != CheckState.Unchecked)
                    {
                        num++;
                    }
                    if (node2._CheckState == CheckState.Indeterminate)
                    {
                        flag = true;
                    }
                }
                if (num == node.Nodes.Count)
                {
                    node._CheckState = flag ? CheckState.Indeterminate : CheckState.Checked;
                }
                else if (num == 0)
                {
                    node._CheckState = CheckState.Unchecked;
                }
                else
                {
                    node._CheckState = CheckState.Indeterminate;
                }
            }
            if (node.Parent != null)
            {
                this.SetParentCheckStatus(node.Parent);
            }
        }

        public void SetSelected(int pIndex, string pValue)
        {
            foreach (TreeGridNode node in this.Nodes)
            {
                foreach (TreeGridNode node2 in node.Nodes)
                {
                    if (node2.Cells[pIndex].Value.ToString() == pValue)
                    {
                        node.Expand();
                        node2.Selected = true;
                        break;
                    }
                }
            }
        }

        public void SetSelectList(int pIndex, List<string> pList)
        {
            foreach (TreeGridNode node in this.Nodes)
            {
                foreach (TreeGridNode node2 in node.Nodes)
                {
                    string str;
                    if (node2.HasChildren)
                    {
                        foreach (TreeGridNode node3 in node2.Nodes)
                        {
                            str = node3.Cells[pIndex].Value.ToString();
                            if (pList.Contains(str))
                            {
                                node3._CheckState = CheckState.Checked;
                                this.RefreshCheckStatus(node3, false, false);
                            }
                        }
                    }
                    else
                    {
                        str = node2.Cells[pIndex].Value.ToString();
                        if (pList.Contains(str))
                        {
                            node2._CheckState = CheckState.Checked;
                            this.RefreshCheckStatus(node2, false, false);
                        }
                    }
                }
            }
        }

        protected internal virtual void SiteNode(TreeGridNode node)
        {
            TreeGridNode parent;
            int index = -1;
            node.BaseTGV = this;
            if ((node.Parent != null) && !node.Parent.IsRoot)
            {
                if (node.Index > 0)
                {
                    parent = node.Parent.Nodes[node.Index - 1];
                }
                else
                {
                    parent = node.Parent;
                }
            }
            else if (node.Index > 0)
            {
                parent = node.Parent.Nodes[node.Index - 1];
            }
            else
            {
                parent = null;
            }
            if (parent == null)
            {
                index = 0;
            }
            else
            {
                while (parent.Level >= node.Level)
                {
                    if (parent.RowIndex >= (base.Rows.Count - 1))
                    {
                        break;
                    }
                    parent = base.Rows[parent.RowIndex + 1] as TreeGridNode;
                }
                if (parent == node.Parent)
                {
                    index = parent.RowIndex + 1;
                }
                else if (parent.Level < node.Level)
                {
                    index = parent.RowIndex;
                }
                else
                {
                    index = parent.RowIndex + 1;
                }
            }
            this.SiteNode(node, index);
            if (node.IsExpanded)
            {
                foreach (TreeGridNode node3 in node.Nodes)
                {
                    this.SiteNode(node3);
                }
            }
        }

        protected internal virtual void SiteNode(TreeGridNode node, int index)
        {
            if (index < base.Rows.Count)
            {
                base.Rows.Insert(index, node);
            }
            else
            {
                base.Rows.Add(node);
            }
        }

        public void UnSiteAll()
        {
            this.UnSiteNode(this._root);
        }

        protected internal virtual void UnSiteNode(TreeGridNode node)
        {
            if (node.IsSited || node.IsRoot)
            {
                foreach (TreeGridNode node2 in node.Nodes)
                {
                    this.UnSiteNode(node2);
                }
                if (!node.IsRoot)
                {
                    base.Rows.Remove(node);
                    node.UnSited();
                }
            }
        }

        [Category("外观"), Description("指示是否在结点旁边显示复选框")]
        public bool CheckBoxes
        {
            get =>
                this._CheckBox;
            set
            {
                this._CheckBox = value;
            }
        }

        [Description("标题复选框的默认值"), Category("外观"), DefaultValue(true)]
        public bool CheckDefult
        {
            get =>
                this._CheckDefult;
            set
            {
                this._CheckDefult = value;
            }
        }

        public TreeGridNode CurrentNode =>
            this.CurrentRow;

        public TreeGridNode CurrentRow =>
            (base.CurrentRow as TreeGridNode);

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public object DataMember
        {
            get =>
                null;
            set
            {
                throw new NotSupportedException("The TreeGridView does not support databinding");
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object DataSource
        {
            get =>
                null;
            set
            {
                throw new NotSupportedException("The TreeGridView does not support databinding");
            }
        }

        public bool HeaderCheckBoxes
        {
            get =>
                this._HeaderCheckBoxes;
            set
            {
                this._HeaderCheckBoxes = value;
            }
        }

        public System.Windows.Forms.ImageList ImageList
        {
            get =>
                this._imageList;
            set
            {
                this._imageList = value;
            }
        }

        [Category("Data"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("返回根结点")]
        public TreeGridNodeCollection Nodes =>
            this._root.Nodes;

        public int RowCount
        {
            get =>
                this.Nodes.Count;
            set
            {
                for (int i = 0; i < value; i++)
                {
                    this.Nodes.Add(new TreeGridNode());
                }
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public DataGridViewRowCollection Rows =>
            base.Rows;

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewRow RowTemplate
        {
            get =>
                base.RowTemplate;
            set
            {
                base.RowTemplate = value;
            }
        }

        public TreeGridNode SelectedNode =>
            (base.SelectedRows[0] as TreeGridNode);

        [Description("显示根节点和子节点之间的虚线"), DefaultValue(true)]
        public bool ShowLines
        {
            get =>
                this._showLines;
            set
            {
                if (value != this._showLines)
                {
                    this._showLines = value;
                    base.Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public bool VirtualMode
        {
            get =>
                false;
            set
            {
                throw new NotSupportedException("The TreeGridView does not support virtual mode");
            }
        }

        [DefaultValue(false), Description("Causes nodes to always show as expandable. Use the NodeExpanding event to add nodes.")]
        public bool VirtualNodes
        {
            get =>
                this._virtualNodes;
            set
            {
                this._virtualNodes = value;
            }
        }

        private static class Win32Helper
        {
            public const int WM_KEYDOWN = 0x100;
            public const int WM_SETREDRAW = 11;
            public const int WM_SYSKEYDOWN = 260;

            [DllImport("USER32.DLL", CharSet = CharSet.Auto)]
            public static extern bool PostMessage(HandleRef hwnd, int msg, IntPtr wparam, IntPtr lparam);
            [DllImport("USER32.DLL", CharSet = CharSet.Auto)]
            public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);
            [DllImport("USER32.DLL", CharSet = CharSet.Auto)]
            public static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);
        }
    }
}

