namespace IntelligentPlanning.ExDataGridView
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    internal class ExCheckBoxColumn : DataGridViewCheckBoxColumn
    {
        [Category("Custom"), Description("复选框单元格的值更改时发生"), DefaultValue(true)]
        public event DataGridViewCellMouseEventHandler CheckValueChanged;

        [Category("Custom"), Description("表头复选框发生改变时"), DefaultValue(true)]
        public event DataGridViewCellMouseEventHandler HeaderCheckedChanged;

        public ExCheckBoxColumn(bool pVis = true, CheckState pDefult = CheckState.Checked, bool pChange = true)
        {
            base.HeaderCell = new ExCheckBoxColumnHeaderCell(pVis, pDefult, pChange);
            this.CellTemplate = new DataGridViewCheckBoxCellEx();
        }

        public void OnCheckValueChanged(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.ExHeaderCell._ChangeHeaderState)
            {
                int num = this.SelectNum();
                if (num == base.DataGridView.Rows.Count)
                {
                    this.ExHeaderCell._CheckState = CheckState.Checked;
                }
                else if (num == 0)
                {
                    this.ExHeaderCell._CheckState = CheckState.Unchecked;
                }
                else
                {
                    this.ExHeaderCell._CheckState = CheckState.Indeterminate;
                }
                base.DataGridView.InvalidateCell(this.ExHeaderCell);
            }
            if (this.CheckValueChanged != null)
            {
                this.CheckValueChanged(this, e);
            }
            base.DataGridView.EndEdit();
        }

        public void OnHeaderCheckedChanged(object sender, DataGridViewCellMouseEventArgs e)
        {
            foreach (DataGridViewRow row in (IEnumerable) base.DataGridView.Rows)
            {
                row.Cells[base.Index].Value = this.ExHeaderCell._CheckState == CheckState.Checked;
            }
            if (this.HeaderCheckedChanged != null)
            {
                this.HeaderCheckedChanged(this, e);
            }
            base.DataGridView.EndEdit();
        }

        public int SelectNum()
        {
            int num = 0;
            foreach (DataGridViewRow row in (IEnumerable) base.DataGridView.Rows)
            {
                if (Convert.ToBoolean(row.Cells[base.Index].Value))
                {
                    num++;
                }
            }
            return num;
        }

        public ExCheckBoxColumnHeaderCell ExHeaderCell =>
            (base.HeaderCell as ExCheckBoxColumnHeaderCell);
    }
}

