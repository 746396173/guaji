namespace IntelligentPlanning.ExDataGridView
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Globalization;
    using System.Text;
    using System.Windows.Forms;

    [DesignTimeVisible(false), ToolboxItem(false)]
    public class TreeGridNode : DataGridViewRow
    {
        public CheckState _CheckState;
        internal System.Drawing.Image _image;
        internal int _imageIndex;
        private int _index;
        internal bool _isFirstSibling;
        internal bool _isLastSibling;
        internal bool _isSited;
        private int _level;
        private TreeGridCell _treeCell;
        internal TreeGridView BaseTGV;
        private bool childCellsCreated;
        private TreeGridNodeCollection childrenNodes;
        internal bool IsExpanded;
        internal bool IsRoot;
        internal TreeGridNodeCollection Owner;
        internal TreeGridNode ParentNode;
        private Random rndSeed;
        private ISite site;
        public int UniqueValue;

        [EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false)]
        public event EventHandler Disposed;

        public TreeGridNode()
        {
            this._CheckState = CheckState.Indeterminate;
            this.rndSeed = new Random();
            this.UniqueValue = -1;
            this.childCellsCreated = false;
            this.site = null;
            //this.disposed = null;
            this._index = -1;
            this._level = -1;
            this.IsExpanded = false;
            this.UniqueValue = this.rndSeed.Next();
            this._isSited = false;
            this._isFirstSibling = false;
            this._isLastSibling = false;
            this._imageIndex = -1;
        }

        internal TreeGridNode(TreeGridView owner) : this()
        {
            this.BaseTGV = owner;
            this.IsExpanded = true;
        }

        protected internal virtual bool AddChildNode(TreeGridNode node)
        {
            node.ParentNode = this;
            node.BaseTGV = this.BaseTGV;
            if (this.BaseTGV != null)
            {
                this.UpdateChildNodes(node);
            }
            if (!(((!this._isSited && !this.IsRoot) || !this.IsExpanded) || node._isSited))
            {
                this.BaseTGV.SiteNode(node);
            }
            return true;
        }

        protected internal virtual bool AddChildNodes(params TreeGridNode[] nodes)
        {
            foreach (TreeGridNode node in nodes)
            {
                this.AddChildNode(node);
            }
            return true;
        }

        private void cells_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            if ((this._treeCell == null) && ((e.Action == CollectionChangeAction.Add) || (e.Action == CollectionChangeAction.Refresh)))
            {
                TreeGridCell element = null;
                if (e.Element == null)
                {
                    foreach (DataGridViewCell cell2 in base.Cells)
                    {
                        if (cell2.GetType().IsAssignableFrom(typeof(TreeGridCell)))
                        {
                            element = (TreeGridCell) cell2;
                            break;
                        }
                    }
                }
                else
                {
                    element = e.Element as TreeGridCell;
                }
                if (element != null)
                {
                    this._treeCell = element;
                }
            }
        }

        protected internal virtual bool ClearNodes()
        {
            if (this.HasChildren)
            {
                for (int i = this.Nodes.Count - 1; i >= 0; i--)
                {
                    this.Nodes.RemoveAt(i);
                }
            }
            return true;
        }

        public override object Clone()
        {
            TreeGridNode node = (TreeGridNode) base.Clone();
            node.UniqueValue = -1;
            node._level = this._level;
            node._CheckState = this._CheckState;
            node.BaseTGV = this.BaseTGV;
            node.ParentNode = this.Parent;
            node._imageIndex = this._imageIndex;
            if (node._imageIndex == -1)
            {
                node.Image = this.Image;
            }
            node.IsExpanded = this.IsExpanded;
            return node;
        }

        public virtual bool Collapse() => 
            this.BaseTGV.CollapseNode(this);

        protected override DataGridViewCellCollection CreateCellsInstance()
        {
            DataGridViewCellCollection cells = base.CreateCellsInstance();
            cells.CollectionChanged += new CollectionChangeEventHandler(this.cells_CollectionChanged);
            return cells;
        }

        public virtual bool Expand()
        {
            if (this.BaseTGV != null)
            {
                return this.BaseTGV.ExpandNode(this);
            }
            this.IsExpanded = true;
            return true;
        }

        protected internal virtual bool InsertChildNode(int index, TreeGridNode node)
        {
            node.ParentNode = this;
            node.BaseTGV = this.BaseTGV;
            if (this.BaseTGV != null)
            {
                this.UpdateChildNodes(node);
            }
            if ((this._isSited || this.IsRoot) && this.IsExpanded)
            {
                this.BaseTGV.SiteNode(node);
            }
            return true;
        }

        protected internal virtual bool InsertChildNodes(int index, params TreeGridNode[] nodes)
        {
            foreach (TreeGridNode node in nodes)
            {
                this.InsertChildNode(index, node);
            }
            return true;
        }

        protected internal virtual bool RemoveChildNode(TreeGridNode node)
        {
            if ((this.IsRoot || this._isSited) && this.IsExpanded)
            {
                this.BaseTGV.UnSiteNode(node);
            }
            node.BaseTGV = null;
            node.ParentNode = null;
            return true;
        }

        private bool ShouldSerializeImage() => 
            ((this._imageIndex == -1) && (this._image != null));

        private bool ShouldSerializeImageIndex() => 
            ((this._imageIndex != -1) && (this._image == null));

        protected internal virtual void Sited()
        {
            this._isSited = true;
            this.childCellsCreated = true;
            foreach (DataGridViewCell cell2 in this.Cells)
            {
                TreeGridCell cell = cell2 as TreeGridCell;
                if (cell != null)
                {
                    cell.Sited();
                }
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(0x24);
            builder.Append("TreeGridNode { Index=");
            builder.Append(this.RowIndex.ToString(CultureInfo.CurrentCulture));
            builder.Append(" }");
            return builder.ToString();
        }

        protected internal virtual void UnSited()
        {
            foreach (DataGridViewCell cell2 in this.Cells)
            {
                TreeGridCell cell = cell2 as TreeGridCell;
                if (cell != null)
                {
                    cell.UnSited();
                }
            }
            this._isSited = false;
        }

        private void UpdateChildNodes(TreeGridNode node)
        {
            if (node.HasChildren)
            {
                foreach (TreeGridNode node2 in node.Nodes)
                {
                    node2.BaseTGV = node.BaseTGV;
                    this.UpdateChildNodes(node2);
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewCellCollection Cells
        {
            get
            {
                if (!this.childCellsCreated && (base.DataGridView == null))
                {
                    if (this.BaseTGV == null)
                    {
                        return null;
                    }
                    base.CreateCells(this.BaseTGV);
                    this.childCellsCreated = true;
                }
                return base.Cells;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool HasChildren =>
            ((this.childrenNodes != null) && (this.Nodes.Count != 0));

        public System.Drawing.Image Image
        {
            get
            {
                if ((this._image == null) && (this._imageIndex != -1))
                {
                    if ((this.ImageList != null) && (this._imageIndex < this.ImageList.Images.Count))
                    {
                        return this.ImageList.Images[this._imageIndex];
                    }
                    return null;
                }
                return this._image;
            }
            set
            {
                this._image = value;
                if (this._image != null)
                {
                    this._imageIndex = -1;
                }
                if (this._isSited)
                {
                    this._treeCell.UpdateStyle();
                    if (this.Displayed)
                    {
                        this.BaseTGV.InvalidateRow(this.RowIndex);
                    }
                }
            }
        }

        [Editor("System.Windows.Forms.Design.ImageIndexEditor", typeof(UITypeEditor)), DefaultValue(-1), TypeConverter(typeof(ImageIndexConverter)), Description("图片在集合中的索引"), Category("Appearance")]
        public int ImageIndex
        {
            get => 
                this._imageIndex;
            set
            {
                this._imageIndex = value;
                if (this._imageIndex != -1)
                {
                    this._image = null;
                }
                if (this._isSited)
                {
                    this._treeCell.UpdateStyle();
                    if (this.Displayed)
                    {
                        this.BaseTGV.InvalidateRow(this.RowIndex);
                    }
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public System.Windows.Forms.ImageList ImageList
        {
            get
            {
                if (this.BaseTGV != null)
                {
                    return this.BaseTGV.ImageList;
                }
                return null;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Index
        {
            get
            {
                if (this._index == -1)
                {
                    this._index = this.Owner.IndexOf(this);
                }
                return this._index;
            }
            internal set
            {
                this._index = value;
            }
        }

        [Browsable(false)]
        public bool IsFirstSibling =>
            (this.Index == 0);

        [Browsable(false)]
        public bool IsLastSibling
        {
            get
            {
                TreeGridNode parent = this.Parent;
                if ((parent != null) && parent.HasChildren)
                {
                    return (this.Index == (parent.Nodes.Count - 1));
                }
                return true;
            }
        }

        [Browsable(false)]
        public bool IsSited =>
            this._isSited;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int Level
        {
            get
            {
                if (this._level == -1)
                {
                    int num = 0;
                    for (TreeGridNode node = this.Parent; node != null; node = node.Parent)
                    {
                        num++;
                    }
                    this._level = num;
                }
                return this._level;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Data"), Description("返回当前行中的子结点")]
        public TreeGridNodeCollection Nodes
        {
            get
            {
                if (this.childrenNodes == null)
                {
                    this.childrenNodes = new TreeGridNodeCollection(this);
                }
                return this.childrenNodes;
            }
            set
            {
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TreeGridNode Parent =>
            this.ParentNode;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("行在网格中的索引"), EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false)]
        public int RowIndex =>
            base.Index;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISite Site
        {
            get => 
                this.site;
            set
            {
                this.site = value;
            }
        }
    }
}

