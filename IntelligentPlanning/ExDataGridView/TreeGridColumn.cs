namespace IntelligentPlanning.ExDataGridView
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class TreeGridColumn : DataGridViewTextBoxColumn
    {
        internal Image pImage;

        public TreeGridColumn()
        {
            this.CellTemplate = new TreeGridCell();
        }

        public override object Clone()
        {
            TreeGridColumn column = (TreeGridColumn) base.Clone();
            column.pImage = this.pImage;
            return column;
        }

        public Image DefaultNodeImage
        {
            get => 
                this.pImage;
            set
            {
                this.pImage = value;
            }
        }
    }
}

