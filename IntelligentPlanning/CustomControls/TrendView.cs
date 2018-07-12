namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using IntelligentPlanning.ExDataGridView;
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml;

    public class TrendView : UserControl
    {
        private bool _RunEvent = false;
        private CheckBox Ckb_OmissionColor;
        private CheckBox Ckb_ViewLine;
        private CheckBox Ckb_ViewOmission;
        private IContainer components = null;
        private List<Control> ControlList = null;
        public const string cTrendName = "走势";
        public const string cTrendName1 = "号码分布";
        public const string cTrendName1_1 = "五星走势";
        public const string cTrendName1_2 = "四星走势";
        public const string cTrendName1_3 = "前三走势";
        public const string cTrendName1_4 = "后三走势";
        public const string cTrendName1_5 = "前二走势";
        public const string cTrendName1_6 = "后二走势";
        private ExpandGirdView Egv_TrendList;
        private ImageList imageStrip = null;
        private Label Lbl_SelectExpect;
        private Panel Pnl_Main;
        private Panel Pnl_TrendInfo;
        private Panel Pnl_TrendList;
        private Panel Pnl_TrendTab;
        private Panel Pnl_ViewTrend;
        private RadioButton Rdb_Expect1;
        private RadioButton Rdb_Expect2;
        private RadioButton Rdb_Expect3;
        private string RegConfigPath = "";
        private List<Control> SpecialControlList = null;
        private MdiTabControl.TabControl Tab_Trend;
        private TreeGridView Tgv_TrendList;
        private TrendCell[,] TrendCellList = null;
        private TrendHeader TrendHeaderInfo = new TrendHeader();

        public TrendView()
        {
            this.InitializeComponent();
        }

        private void AddTabControl(string pText)
        {
            Form form = new Form {
                Text = pText,
                Icon = Resources.TabTrend
            };
            MdiTabControl.TabPage page = this.Tab_Trend.TabPages.Add(form);
        }

        private void BeautifyInterface()
        {
            if (AppInfo.Account.Configuration.Beautify)
            {
                List<Control> pControlList = new List<Control>();
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list2 = new List<Control> {
                    this.Pnl_ViewTrend
                };
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Ckb_ViewOmission,
                    this.Ckb_ViewLine,
                    this.Ckb_OmissionColor,
                    this.Lbl_SelectExpect,
                    this.Rdb_Expect1,
                    this.Rdb_Expect2,
                    this.Rdb_Expect3
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
                this.Tab_Trend.BackHighColor = this.Tab_Trend.BackLowColor = AppInfo.appBackColor;
                this.Tab_Trend.TabBackHighColor = this.Tab_Trend.TabBackLowColor = AppInfo.hotColor;
                this.Tab_Trend.TabBackHighColorDisabled = this.Tab_Trend.TabBackLowColorDisabled = AppInfo.beaBackColor;
                this.Tab_Trend.ForeColor = AppInfo.defaultForeColor;
                this.Tab_Trend.ForeColorDisabled = AppInfo.beaForeColor;
            }
        }

        private void Btn_Trend_Click(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
                this.TrendSelectChange();
            }
        }

        private void Ckb_OmissionColor_CheckedChanged(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
                this.RefreshTrendList(true);
            }
        }

        private void Ckb_ViewLine_CheckedChanged(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
                this.RefreshTrendList(true);
            }
        }

        private void Ckb_ViewOmission_CheckedChanged(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
                this.RefreshTrendList(true);
            }
        }

        public void CountTrend(int pRowCount, int pTendRowCount)
        {
            try
            {
                this.TrendCellList[pRowCount, 0].Value = "出现总次数";
                this.TrendCellList[pRowCount + 1, 0].Value = "平均遗漏值";
                this.TrendCellList[pRowCount + 2, 0].Value = "最大遗漏值";
                this.TrendCellList[pRowCount + 3, 0].Value = "最大连出值";
                for (int i = 0; i < pTendRowCount; i++)
                {
                    if (!this.TrendHeaderInfo.OmissionList.Contains(i))
                    {
                        continue;
                    }
                    int num2 = 0;
                    int num3 = 0;
                    int num4 = 0;
                    int num5 = 0;
                    int num6 = 0;
                    int num7 = 0;
                    int num8 = 0;
                    int num9 = 0;
                    while (num9 < pRowCount)
                    {
                        string str = this.TrendCellList[num9, i].Value;
                        if ((str != null) && (str != ""))
                        {
                            num8++;
                            if (num2 > num3)
                            {
                                num3 = num2;
                            }
                            num6 += num2;
                            num2 = 0;
                            num4++;
                        }
                        else
                        {
                            if (num4 > num5)
                            {
                                num5 = num4;
                            }
                            num4 = 0;
                            num2++;
                            this.TrendCellList[num9, i].Value = num2.ToString();
                            this.TrendCellList[num9, i].ForeColor = AppInfo.darkGrayForeColor;
                            this.TrendCellList[num9, i].IsOmission = true;
                        }
                        num9++;
                    }
                    for (num9 = pRowCount - 1; num9 >= 0; num9--)
                    {
                        if (this.TrendHeaderInfo.OmissionColorList.Contains(i))
                        {
                            if (!this.TrendCellList[num9, i].IsOmission)
                            {
                                break;
                            }
                            this.TrendCellList[num9, i].BackColor = AppInfo.omissionColor;
                        }
                    }
                    num6 += num2;
                    if (num2 > num3)
                    {
                        num3 = num2;
                    }
                    if (num4 > num5)
                    {
                        num5 = num4;
                    }
                    if (num8 > 0)
                    {
                        num7 = Convert.ToInt32((int) (num6 / num8)) + 1;
                    }
                    this.TrendCellList[pRowCount, i].Value = num8.ToString();
                    this.TrendCellList[pRowCount + 1, i].Value = num7.ToString();
                    this.TrendCellList[pRowCount + 2, i].Value = num3.ToString();
                    this.TrendCellList[pRowCount + 3, i].Value = num5.ToString();
                }
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

        private void Egv_Trend_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            TrendView.TrendCell trendCell = this.TrendCellList[e.RowIndex, e.ColumnIndex];
            if (trendCell.IsHide || (trendCell.IsOmission && !this.Ckb_ViewOmission.Checked))
            {
                e.Value = null;
            }
            else
            {
                e.Value = trendCell.Value;
            }
        }

        private void Egv_Trend_CustomCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            int left = e.CellBounds.Left;
            int top = e.CellBounds.Top;
            int width = e.CellBounds.Width;
            int height = e.CellBounds.Height;
            DataGridViewCell dataGridViewCell = this.Egv_TrendList.Rows[e.RowIndex].Cells[e.ColumnIndex];
            TrendView.TrendCell trendCell = this.TrendCellList[e.RowIndex, e.ColumnIndex];
            if (this.Ckb_OmissionColor.Checked)
            {
                if (!trendCell.BackColor.IsEmpty)
                {
                    dataGridViewCell.Style.BackColor = trendCell.BackColor;
                }
            }
            else
            {
                dataGridViewCell.Style.BackColor = Color.Empty;
            }
            if (!trendCell.ForeColor.IsEmpty)
            {
                dataGridViewCell.Style.ForeColor = trendCell.ForeColor;
            }
            if (this.TrendHeaderInfo.LineList.Contains(e.ColumnIndex))
            {
                Brush brush = new SolidBrush(Color.Black);
                Pen pen = new Pen(brush);
                e.Graphics.DrawLine(pen, left, top, left, top + height);
            }
        }

        private void Egv_Trend_CustomPaintingAfter(object sender, PaintEventArgs e)
        {
            int firstDisplayedScrollingRowIndex = this.Egv_TrendList.FirstDisplayedScrollingRowIndex;
            int firstDisplayedScrollingColumnIndex = this.Egv_TrendList.FirstDisplayedScrollingColumnIndex;
            int num = this.Egv_TrendList.DisplayedRowCount(true);
            int num2 = this.Egv_TrendList.DisplayedColumnCount(true);
            if (num != 0 && num2 != 0)
            {
                foreach (TrendView.DrawLine drawLine in this.TrendHeaderInfo.DrawLineList)
                {
                    int row = drawLine.Row;
                    int column = drawLine.Column;
                    TrendView.TrendCell trendCell = this.TrendCellList[row, column];
                    if (row >= firstDisplayedScrollingRowIndex + 1 && row < firstDisplayedScrollingRowIndex + num && column >= firstDisplayedScrollingColumnIndex && column < firstDisplayedScrollingColumnIndex + num2 && trendCell.IsDrawLine && this.Ckb_ViewLine.Checked)
                    {
                        Rectangle cellDisplayRectangle = this.Egv_TrendList.GetCellDisplayRectangle(column, row, false);
                        Rectangle cellDisplayRectangle2 = this.Egv_TrendList.GetCellDisplayRectangle(drawLine.OffsetX, row - 1, false);
                        int left = cellDisplayRectangle.Left;
                        int top = cellDisplayRectangle.Top;
                        int width = cellDisplayRectangle.Width;
                        int height = cellDisplayRectangle.Height;
                        int left2 = cellDisplayRectangle2.Left;
                        int top2 = cellDisplayRectangle2.Top;
                        int width2 = cellDisplayRectangle2.Width;
                        int height2 = cellDisplayRectangle2.Height;
                        if (left2 != 0)
                        {
                            int x = left + width / 2;
                            int y = top + height / 2;
                            int x2 = left2 + width2 / 2;
                            int y2 = top2 + height2 / 2;
                            Brush brush = new SolidBrush(AppInfo.lineColor);
                            Pen pen = new Pen(brush, 2f);
                            e.Graphics.DrawLine(pen, x, y, x2, y2);
                        }
                    }
                    if (row >= firstDisplayedScrollingRowIndex && row < firstDisplayedScrollingRowIndex + num && column >= firstDisplayedScrollingColumnIndex && column < firstDisplayedScrollingColumnIndex + num2)
                    {
                        Rectangle cellDisplayRectangle = this.Egv_TrendList.GetCellDisplayRectangle(column, row, false);
                        int left = cellDisplayRectangle.Left;
                        int top = cellDisplayRectangle.Top;
                        int width = cellDisplayRectangle.Width;
                        int height = cellDisplayRectangle.Height;
                        int bottom = cellDisplayRectangle.Bottom;
                        int right = cellDisplayRectangle.Right;
                        Brush brush2 = new SolidBrush(trendCell.BackExpandColor);
                        if (width < 40)
                        {
                            Rectangle rect = new Rectangle(left + 4, top, height - 2, height - 2);
                            e.Graphics.FillEllipse(brush2, rect);
                        }
                        else
                        {
                            Rectangle rect = new Rectangle(left + 10, top, width - 20, height - 2);
                            e.Graphics.FillEllipse(brush2, rect);
                        }
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        Brush brush3 = new SolidBrush(trendCell.ForeExpandColor);
                        e.Graphics.DrawString(trendCell.Value, this.Egv_TrendList.DefaultCellStyle.Font, brush3, cellDisplayRectangle, stringFormat);
                    }
                }
            }
        }

        private void Egv_Trend_Scroll(object sender, ScrollEventArgs e)
        {
            this.RefreshTrendList(false);
        }

        public List<ConfigurationStatus.OpenData> FilterOpenData()
        {
            int num2;
            List<ConfigurationStatus.OpenData> list = new List<ConfigurationStatus.OpenData>();
            if (this.Rdb_Expect1.Checked || this.Rdb_Expect2.Checked)
            {
                int count = this.Rdb_Expect1.Checked ? 30 : 50;
                if (count > AppInfo.DataList.Count)
                {
                    count = AppInfo.DataList.Count;
                }
                for (num2 = 0; num2 < count; num2++)
                {
                    list.Add(AppInfo.DataList[num2]);
                }
            }
            else
            {
                DateTime date = DateTime.Now.Date;
                for (num2 = 0; num2 < AppInfo.DataList.Count; num2++)
                {
                    string expect = AppInfo.DataList[num2].Expect;
                    DateTime time2 = Convert.ToDateTime(string.Concat(new object[] { 20, expect.Substring(0, 2), "-", expect.Substring(2, 2), "-", expect.Substring(4, 2) }));
                    if (this.Rdb_Expect3.Checked)
                    {
                        if (time2 == date)
                        {
                            list.Add(AppInfo.DataList[num2]);
                        }
                        else if (time2 < date)
                        {
                            break;
                        }
                    }
                }
            }
            list.Reverse();
            return list;
        }

        private TrendHeader GetHeaderData(XmlNode pTrendNode)
        {
            TrendHeader header = new TrendHeader();
            string pName = this.Tgv_TrendList.SelectedNode.Cells[0].Value.ToString();
            XmlNode itemNode = CommFunc.GetItemNode(pTrendNode, pName);
            string pKey = "Name";
            string str3 = "Width";
            string str4 = "Omission";
            string str5 = "OmissionColor";
            foreach (XmlNode node3 in itemNode.ChildNodes)
            {
                string item = CommFunc.GetAttributeString(node3, pKey, "");
                int num = CommFunc.GetAttributeInt(node3, str3, 0x20);
                bool flag = CommFunc.GetAttributeBoolean(node3, str4, false);
                bool flag2 = CommFunc.GetAttributeBoolean(node3, str5, true);
                if (!node3.HasChildNodes)
                {
                    header.TextList.Add(item);
                    header.WidthList.Add(num);
                }
                else
                {
                    XmlNode pNode = node3.ChildNodes[0];
                    List<string> list = CommFunc.SplitString(CommFunc.LottrySplitStr(CommFunc.GetAttributeString(pNode, pKey, ""), ','), ",", -1);
                    TrendHeaderSecond second = new TrendHeaderSecond {
                        Text = item,
                        ColIndex = header.TextList.Count,
                        ColCount = list.Count
                    };
                    header.TextSecondList.Add(second);
                    foreach (string str8 in list)
                    {
                        if (flag)
                        {
                            header.OmissionList.Add(header.TextList.Count);
                        }
                        if (flag2)
                        {
                            header.OmissionColorList.Add(header.TextList.Count);
                        }
                        header.TextList.Add(str8);
                        header.WidthList.Add(num);
                    }
                }
                header.LineList.Add(header.TextList.Count);
            }
            return header;
        }

        private MdiTabControl.TabPage GetTabControlIndex(string pText)
        {
            foreach (MdiTabControl.TabPage page in this.Tab_Trend.TabPages)
            {
                if (((Form) page.Form).Text == pText)
                {
                    return page;
                }
            }
            return null;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            DataGridViewCellStyle style3 = new DataGridViewCellStyle();
            DataGridViewCellStyle style4 = new DataGridViewCellStyle();
            DataGridViewCellStyle style5 = new DataGridViewCellStyle();
            this.Pnl_Main = new Panel();
            this.Pnl_ViewTrend = new Panel();
            this.Egv_TrendList = new ExpandGirdView(this.components);
            this.Pnl_TrendTab = new Panel();
            this.Pnl_TrendList = new Panel();
            this.Tgv_TrendList = new TreeGridView();
            this.Pnl_TrendInfo = new Panel();
            this.Ckb_OmissionColor = new CheckBox();
            this.Rdb_Expect1 = new RadioButton();
            this.Rdb_Expect2 = new RadioButton();
            this.Rdb_Expect3 = new RadioButton();
            this.Lbl_SelectExpect = new Label();
            this.Ckb_ViewLine = new CheckBox();
            this.Ckb_ViewOmission = new CheckBox();
            this.Pnl_Main.SuspendLayout();
            this.Pnl_ViewTrend.SuspendLayout();
            ((ISupportInitialize) this.Egv_TrendList).BeginInit();
            this.Pnl_TrendList.SuspendLayout();
            ((ISupportInitialize) this.Tgv_TrendList).BeginInit();
            this.Pnl_TrendInfo.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_Main.Controls.Add(this.Pnl_ViewTrend);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(0x3f4, 570);
            this.Pnl_Main.TabIndex = 0;
            this.Pnl_ViewTrend.Controls.Add(this.Egv_TrendList);
            this.Pnl_ViewTrend.Controls.Add(this.Pnl_TrendTab);
            this.Pnl_ViewTrend.Controls.Add(this.Pnl_TrendList);
            this.Pnl_ViewTrend.Controls.Add(this.Pnl_TrendInfo);
            this.Pnl_ViewTrend.Dock = DockStyle.Fill;
            this.Pnl_ViewTrend.Location = new Point(0, 0);
            this.Pnl_ViewTrend.Name = "Pnl_ViewTrend";
            this.Pnl_ViewTrend.Size = new Size(0x3f4, 570);
            this.Pnl_ViewTrend.TabIndex = 5;
            this.Egv_TrendList.AllowUserToAddRows = false;
            this.Egv_TrendList.AllowUserToDeleteRows = false;
            this.Egv_TrendList.AllowUserToResizeColumns = false;
            this.Egv_TrendList.AllowUserToResizeRows = false;
            this.Egv_TrendList.BackgroundColor = SystemColors.Window;
            this.Egv_TrendList.BorderStyle = BorderStyle.Fixed3D;
            this.Egv_TrendList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style.BackColor = SystemColors.Control;
            style.Font = new Font("微软雅黑", 9f);
            style.ForeColor = SystemColors.WindowText;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.True;
            this.Egv_TrendList.ColumnHeadersDefaultCellStyle = style;
            this.Egv_TrendList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            style2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style2.BackColor = SystemColors.Window;
            style2.Font = new Font("微软雅黑", 9f);
            style2.ForeColor = SystemColors.ControlText;
            style2.SelectionBackColor = SystemColors.Highlight;
            style2.SelectionForeColor = SystemColors.HighlightText;
            style2.WrapMode = DataGridViewTriState.False;
            this.Egv_TrendList.DefaultCellStyle = style2;
            this.Egv_TrendList.Dock = DockStyle.Fill;
            this.Egv_TrendList.DragLineColor = Color.Silver;
            this.Egv_TrendList.ExternalVirtualMode = true;
            this.Egv_TrendList.GridColor = Color.Silver;
            this.Egv_TrendList.HeadersCheckDefult = CheckState.Checked;
            this.Egv_TrendList.Location = new Point(200, 0x43);
            this.Egv_TrendList.MergeColumnHeaderBackColor = SystemColors.Control;
            this.Egv_TrendList.MergeColumnHeaderForeColor = Color.Black;
            this.Egv_TrendList.MultiSelect = false;
            this.Egv_TrendList.Name = "Egv_TrendList";
            this.Egv_TrendList.RowHeadersVisible = false;
            this.Egv_TrendList.RowNum = 15;
            style3.SelectionBackColor = Color.FromArgb(0xde, 0xe8, 0xfc);
            style3.SelectionForeColor = Color.Black;
            this.Egv_TrendList.RowsDefaultCellStyle = style3;
            this.Egv_TrendList.RowTemplate.Height = 0x17;
            this.Egv_TrendList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Egv_TrendList.Size = new Size(0x32c, 0x1f7);
            this.Egv_TrendList.TabIndex = 0x43;
            this.Pnl_TrendTab.BackColor = Color.Transparent;
            this.Pnl_TrendTab.Dock = DockStyle.Top;
            this.Pnl_TrendTab.Location = new Point(200, 0x23);
            this.Pnl_TrendTab.Name = "Pnl_TrendTab";
            this.Pnl_TrendTab.Size = new Size(0x32c, 0x20);
            this.Pnl_TrendTab.TabIndex = 0x8b;
            this.Pnl_TrendList.BackColor = Color.Transparent;
            this.Pnl_TrendList.Controls.Add(this.Tgv_TrendList);
            this.Pnl_TrendList.Dock = DockStyle.Left;
            this.Pnl_TrendList.Location = new Point(0, 0x23);
            this.Pnl_TrendList.Name = "Pnl_TrendList";
            this.Pnl_TrendList.Size = new Size(200, 0x217);
            this.Pnl_TrendList.TabIndex = 0x44;
            this.Tgv_TrendList.AllowUserToAddRows = false;
            this.Tgv_TrendList.AllowUserToDeleteRows = false;
            this.Tgv_TrendList.AllowUserToResizeRows = false;
            this.Tgv_TrendList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.Tgv_TrendList.BackgroundColor = SystemColors.Control;
            this.Tgv_TrendList.BorderStyle = BorderStyle.Fixed3D;
            this.Tgv_TrendList.CheckBoxes = false;
            this.Tgv_TrendList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.Tgv_TrendList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Tgv_TrendList.ColumnHeadersVisible = false;
            style4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style4.BackColor = SystemColors.Control;
            style4.Font = new Font("微软雅黑", 9f);
            style4.ForeColor = SystemColors.ControlText;
            style4.SelectionBackColor = SystemColors.Highlight;
            style4.SelectionForeColor = SystemColors.HighlightText;
            style4.WrapMode = DataGridViewTriState.False;
            this.Tgv_TrendList.DefaultCellStyle = style4;
            this.Tgv_TrendList.Dock = DockStyle.Fill;
            this.Tgv_TrendList.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.Tgv_TrendList.GridColor = Color.Silver;
            this.Tgv_TrendList.HeaderCheckBoxes = true;
            this.Tgv_TrendList.ImageList = null;
            this.Tgv_TrendList.Location = new Point(0, 0);
            this.Tgv_TrendList.MultiSelect = false;
            this.Tgv_TrendList.Name = "Tgv_TrendList";
            this.Tgv_TrendList.RowHeadersVisible = false;
            style5.SelectionBackColor = Color.SteelBlue;
            style5.SelectionForeColor = Color.Black;
            this.Tgv_TrendList.RowsDefaultCellStyle = style5;
            this.Tgv_TrendList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Tgv_TrendList.ShowLines = false;
            this.Tgv_TrendList.Size = new Size(200, 0x217);
            this.Tgv_TrendList.TabIndex = 0;
            this.Tgv_TrendList.CellMouseClick += new DataGridViewCellMouseEventHandler(this.Tgv_TrendList_CellMouseClick);
            this.Pnl_TrendInfo.BackColor = Color.Transparent;
            this.Pnl_TrendInfo.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_TrendInfo.Controls.Add(this.Ckb_OmissionColor);
            this.Pnl_TrendInfo.Controls.Add(this.Rdb_Expect1);
            this.Pnl_TrendInfo.Controls.Add(this.Rdb_Expect2);
            this.Pnl_TrendInfo.Controls.Add(this.Rdb_Expect3);
            this.Pnl_TrendInfo.Controls.Add(this.Lbl_SelectExpect);
            this.Pnl_TrendInfo.Controls.Add(this.Ckb_ViewLine);
            this.Pnl_TrendInfo.Controls.Add(this.Ckb_ViewOmission);
            this.Pnl_TrendInfo.Dock = DockStyle.Top;
            this.Pnl_TrendInfo.Location = new Point(0, 0);
            this.Pnl_TrendInfo.Name = "Pnl_TrendInfo";
            this.Pnl_TrendInfo.Size = new Size(0x3f4, 0x23);
            this.Pnl_TrendInfo.TabIndex = 0x41;
            this.Ckb_OmissionColor.AutoSize = true;
            this.Ckb_OmissionColor.Checked = true;
            this.Ckb_OmissionColor.CheckState = CheckState.Checked;
            this.Ckb_OmissionColor.Location = new Point(0xa8, 8);
            this.Ckb_OmissionColor.Name = "Ckb_OmissionColor";
            this.Ckb_OmissionColor.Size = new Size(0x3f, 0x15);
            this.Ckb_OmissionColor.TabIndex = 0x59;
            this.Ckb_OmissionColor.Text = "遗漏条";
            this.Ckb_OmissionColor.UseVisualStyleBackColor = true;
            this.Ckb_OmissionColor.CheckedChanged += new EventHandler(this.Ckb_OmissionColor_CheckedChanged);
            this.Rdb_Expect1.AutoSize = true;
            this.Rdb_Expect1.Checked = true;
            this.Rdb_Expect1.Location = new Point(310, 7);
            this.Rdb_Expect1.Name = "Rdb_Expect1";
            this.Rdb_Expect1.Size = new Size(0x34, 0x15);
            this.Rdb_Expect1.TabIndex = 40;
            this.Rdb_Expect1.TabStop = true;
            this.Rdb_Expect1.Text = "30期";
            this.Rdb_Expect1.UseVisualStyleBackColor = true;
            this.Rdb_Expect1.Click += new EventHandler(this.Rdb_Expect_Click);
            this.Rdb_Expect2.AutoSize = true;
            this.Rdb_Expect2.Location = new Point(370, 7);
            this.Rdb_Expect2.Name = "Rdb_Expect2";
            this.Rdb_Expect2.Size = new Size(0x34, 0x15);
            this.Rdb_Expect2.TabIndex = 0x29;
            this.Rdb_Expect2.Text = "50期";
            this.Rdb_Expect2.UseVisualStyleBackColor = true;
            this.Rdb_Expect2.Click += new EventHandler(this.Rdb_Expect_Click);
            this.Rdb_Expect3.AutoSize = true;
            this.Rdb_Expect3.Location = new Point(430, 7);
            this.Rdb_Expect3.Name = "Rdb_Expect3";
            this.Rdb_Expect3.Size = new Size(0x4a, 0x15);
            this.Rdb_Expect3.TabIndex = 0x2a;
            this.Rdb_Expect3.Text = "今日数据";
            this.Rdb_Expect3.UseVisualStyleBackColor = true;
            this.Rdb_Expect3.Click += new EventHandler(this.Rdb_Expect_Click);
            this.Lbl_SelectExpect.AutoSize = true;
            this.Lbl_SelectExpect.Location = new Point(0xec, 9);
            this.Lbl_SelectExpect.Name = "Lbl_SelectExpect";
            this.Lbl_SelectExpect.Size = new Size(0x44, 0x11);
            this.Lbl_SelectExpect.TabIndex = 0x27;
            this.Lbl_SelectExpect.Text = "数据期数：";
            this.Ckb_ViewLine.AutoSize = true;
            this.Ckb_ViewLine.Checked = true;
            this.Ckb_ViewLine.CheckState = CheckState.Checked;
            this.Ckb_ViewLine.Location = new Point(0x57, 8);
            this.Ckb_ViewLine.Name = "Ckb_ViewLine";
            this.Ckb_ViewLine.Size = new Size(0x4b, 0x15);
            this.Ckb_ViewLine.TabIndex = 0x26;
            this.Ckb_ViewLine.Text = "显示走势";
            this.Ckb_ViewLine.UseVisualStyleBackColor = true;
            this.Ckb_ViewLine.CheckedChanged += new EventHandler(this.Ckb_ViewLine_CheckedChanged);
            this.Ckb_ViewOmission.AutoSize = true;
            this.Ckb_ViewOmission.Checked = true;
            this.Ckb_ViewOmission.CheckState = CheckState.Checked;
            this.Ckb_ViewOmission.Location = new Point(6, 8);
            this.Ckb_ViewOmission.Name = "Ckb_ViewOmission";
            this.Ckb_ViewOmission.Size = new Size(0x4b, 0x15);
            this.Ckb_ViewOmission.TabIndex = 0x25;
            this.Ckb_ViewOmission.Text = "显示遗漏";
            this.Ckb_ViewOmission.UseVisualStyleBackColor = true;
            this.Ckb_ViewOmission.CheckedChanged += new EventHandler(this.Ckb_ViewOmission_CheckedChanged);
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.Pnl_Main);
            this.Font = new Font("微软雅黑", 9f);
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "TrendView";
            base.Size = new Size(0x3f4, 570);
            base.Load += new EventHandler(this.TrendView_Load);
            this.Pnl_Main.ResumeLayout(false);
            this.Pnl_ViewTrend.ResumeLayout(false);
            ((ISupportInitialize) this.Egv_TrendList).EndInit();
            this.Pnl_TrendList.ResumeLayout(false);
            ((ISupportInitialize) this.Tgv_TrendList).EndInit();
            this.Pnl_TrendInfo.ResumeLayout(false);
            this.Pnl_TrendInfo.PerformLayout();
            base.ResumeLayout(false);
        }

        public void LineTrend(int pRowCount, int pColIndex, int pColCount, Color pBack, Color pFore, bool pDrawLine = true)
        {
            for (int i = pRowCount - 1; i >= 0; i--)
            {
                for (int j = pColIndex; j < (pColIndex + pColCount); j++)
                {
                    switch (this.TrendCellList[i, j].Value)
                    {
                        case null:
                        case "":
                        {
                            continue;
                        }
                        default:
                            for (int k = pColIndex; k < (pColIndex + pColCount); k++)
                            {
                                int num4 = ((i - 1) >= 0) ? (i - 1) : 0;
                                string str2 = this.TrendCellList[num4, k].Value;
                                if ((str2 != null) && (str2 != ""))
                                {
                                    this.TrendCellList[i, j].IsDrawLine = pDrawLine;
                                    if (this.TrendCellList[i, j].BackExpandColor.IsEmpty)
                                    {
                                        this.TrendCellList[i, j].BackExpandColor = pBack;
                                    }
                                    if (this.TrendCellList[i, j].ForeExpandColor.IsEmpty)
                                    {
                                        this.TrendCellList[i, j].ForeExpandColor = pFore;
                                    }
                                    DrawLine item = new DrawLine {
                                        Row = i,
                                        Column = j,
                                        OffsetX = k
                                    };
                                    this.TrendHeaderInfo.DrawLineList.Add(item);
                                    if (pDrawLine)
                                    {
                                        break;
                                    }
                                }
                            }
                            break;
                    }
                    if (pDrawLine)
                    {
                        break;
                    }
                }
            }
        }

        private void LoadTabControl(ref MdiTabControl.TabControl pTabControl, Panel panel)
        {
            pTabControl = new MdiTabControl.TabControl();
            panel.Controls.Add(pTabControl);
            pTabControl.Dock = DockStyle.Fill;
            pTabControl.TabOffset = 2;
            pTabControl.TabBackHighColor = SystemColors.Control;
            pTabControl.TabBackLowColor = SystemColors.Control;
        }

        private void LoadTabControlOption()
        {
            this.Tab_Trend.TabOffset = 2;
            this.Tab_Trend.TabBackHighColor = SystemColors.Control;
            this.Tab_Trend.TabBackLowColor = SystemColors.Control;
        }

        private void LoadTrendList()
        {
            try
            {
                if (AppInfo.Current.Lottery != null)
                {
                    this.LoadTrendList(this.Egv_TrendList, AppInfo.Current.Lottery.XmlTrendNode, ref this.TrendHeaderInfo);
                    if (!this.Egv_TrendList.VirtualMode)
                    {
                        this.Egv_TrendList.Scroll += new ScrollEventHandler(this.Egv_Trend_Scroll);
                        this.Egv_TrendList.CellValueNeeded += new DataGridViewCellValueEventHandler(this.Egv_Trend_CellValueNeeded);
                        this.Egv_TrendList.CustomCellPainting += new DataGridViewCellPaintingEventHandler(this.Egv_Trend_CustomCellPainting);
                        this.Egv_TrendList.CustomPaintingAfter += new PaintEventHandler(this.Egv_Trend_CustomPaintingAfter);
                    }
                    this.Egv_TrendList.VirtualMode = true;
                }
            }
            catch
            {
            }
        }

        private void LoadTrendList(ExpandGirdView pDataGridView, XmlNode pTrendNode, ref TrendHeader TrendHeaderInfo)
        {
            int num2;
            pDataGridView.Columns.Clear();
            pDataGridView.ClearSpanInfo();
            TrendHeaderInfo = this.GetHeaderData(pTrendNode);
            int count = TrendHeaderInfo.TextList.Count;
            List<int> pType = new List<int>();
            List<string> textList = TrendHeaderInfo.TextList;
            List<int> widthList = TrendHeaderInfo.WidthList;
            List<bool> pRead = new List<bool>();
            List<bool> pVis = new List<bool>();
            for (num2 = 0; num2 < count; num2++)
            {
                pType.Add(1);
            }
            for (num2 = 0; num2 < count; num2++)
            {
                pRead.Add(true);
            }
            for (num2 = 0; num2 < count; num2++)
            {
                pVis.Add(true);
            }
            pDataGridView.LoadInitialization(pType, textList, widthList, pRead, pVis, null);
            pDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            pDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            pDataGridView.MultiSelect = true;
            CommFunc.SetExpandGirdViewFormat(pDataGridView, 9);
            pDataGridView.MergeColumnHeaderBackColor = AppInfo.appBackColor;
            pDataGridView.MergeColumnHeaderForeColor = AppInfo.whiteColor;
            pDataGridView.ColumnHeadersHeight = 40;
            foreach (TrendHeaderSecond second in TrendHeaderInfo.TextSecondList)
            {
                pDataGridView.AddSpanHeader(second.ColIndex, second.ColCount, second.Text);
            }
            for (num2 = 0; num2 < pDataGridView.ColumnCount; num2++)
            {
                pDataGridView.Columns[num2].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void LoadTrendSelectList()
        {
            try
            {
                this.imageStrip = new ImageList();
                this.imageStrip.ImageSize = new Size(0x10, 0x10);
                this.imageStrip.Images.Add(Resources.CollapseFold);
                this.imageStrip.Images.Add(Resources.Trend1);
                this.imageStrip.Images.Add(Resources.Trend2);
                this.Tgv_TrendList.ImageList = this.imageStrip;
                List<int> pType = new List<int> { 0 };
                List<string> pText = new List<string> { "" };
                List<int> pWidth = new List<int> { 200 };
                List<bool> pRead = new List<bool> { true };
                List<bool> pVis = new List<bool> { true };
                this.Tgv_TrendList.LoadInitialization(pType, pText, pWidth, pRead, pVis);
                CommFunc.SetExpandGirdViewFormat(this.Tgv_TrendList, 9);
                TreeGridNode node = this.Tgv_TrendList.Nodes.Add("号码分布");
                node.ImageIndex = 0;
                node.Height = 30;
                node.DefaultCellStyle.BackColor = AppInfo.appBackColor;
                node.DefaultCellStyle.ForeColor = AppInfo.whiteColor;
                List<string> list6 = new List<string> { 
                    "五星走势",
                    "四星走势",
                    "前三走势",
                    "后三走势",
                    "前二走势",
                    "后二走势"
                };
                foreach (string str in list6)
                {
                    TreeGridNode node2 = node.Nodes.Add(str);
                    node2.ImageIndex = 1;
                    node2.Height = 0x19;
                }
                node.Expand();
                node.Nodes[0].Selected = true;
            }
            catch
            {
            }
        }

        private void MainWindow_SizeChanged(object sender, EventArgs e)
        {
            this.RefreshTrendList(false);
        }

        private void Rdb_Expect_Click(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
                this.TrendSelectChange();
            }
        }

        private void RefreshTrendList(bool pRrefreshAll)
        {
            if (pRrefreshAll)
            {
                this.Egv_TrendList.Invalidate();
            }
            else
            {
                this.Egv_TrendList.ReDrawHead();
            }
        }

        public void SaveControlInfoByReg()
        {
            CommFunc.SaveFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SaveSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
        }

        private void SelectTrendRow(string pText)
        {
            foreach (TreeGridNode node in this.Tgv_TrendList.Nodes)
            {
                if (node.HasChildren)
                {
                    foreach (TreeGridNode node2 in node.Nodes)
                    {
                        if (node2.Cells[0].Value.ToString() == pText)
                        {
                            if (!node.IsExpanded)
                            {
                                node.Expand();
                            }
                            node2.Selected = true;
                            this.TrendSelectChange();
                            break;
                        }
                    }
                }
            }
        }

        public void SetControlInfoByReg()
        {
            this.RegConfigPath = @"software\TUHAOPLUS\YXZXGJ\DlgConfig\永信在线挂机软件\" + base.Name;
            List<Control> list = new List<Control> {
                this.Ckb_ViewOmission,
                this.Ckb_OmissionColor,
                this.Ckb_ViewLine,
                this.Rdb_Expect1,
                this.Rdb_Expect2,
                this.Rdb_Expect3
            };
            this.ControlList = list;
            this.SpecialControlList = new List<Control>();
            CommFunc.SetFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SetSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
        }

        private void Tab_Trend_SelectedTabChanged(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
                this.SelectTrendRow(((Form) this.Tab_Trend.SelectedForm).Text);
            }
        }

        public void Tgv_TrendList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this._RunEvent)
            {
                this.TrendSelectChange();
            }
        }

        private void TrendMain(string pText)
        {
            List<ConfigurationStatus.OpenData> list = this.FilterOpenData();
            int count = list.Count;
            this.TrendCellList = new TrendCell[count + 4, this.Egv_TrendList.ColumnCount];
            int num2 = this.TrendCellList.GetUpperBound(0) + 1;
            this.TrendHeaderInfo.DrawLineList.Clear();
            for (int i = 0; i < count; i++)
            {
                int num5;
                string expect = list[i].Expect;
                string code = list[i].Code;
                this.TrendCellList[i, 0].Value = expect;
                List<int> list2 = CommFunc.ConvertSameListInt(code);
                int num4 = 0;
                while (num4 < 5)
                {
                    num5 = list2[num4];
                    this.TrendCellList[i, num4 + 1].Value = num5.ToString();
                    num4++;
                }
                string str3 = pText;
                if (str3 != null)
                {
                    if (str3 != "五星走势")
                    {
                        if (str3 == "四星走势")
                        {
                            goto Label_01BA;
                        }
                        if (str3 == "前三走势")
                        {
                            goto Label_023C;
                        }
                        if (str3 == "后三走势")
                        {
                            goto Label_02BC;
                        }
                        if (str3 == "前二走势")
                        {
                            goto Label_033E;
                        }
                        if (str3 == "后二走势")
                        {
                            goto Label_03BB;
                        }
                    }
                    else
                    {
                        num4 = 0;
                        while (num4 < 5)
                        {
                            num5 = list2[num4];
                            this.TrendCellList[i, list2[num4] + 6].Value = num5.ToString();
                            num5 = list2[num4];
                            this.TrendCellList[i, (list2[num4] + 0x10) + (num4 * 10)].Value = num5.ToString();
                            num4++;
                        }
                    }
                }
                continue;
            Label_01BA:
                num4 = 1;
                while (num4 < 5)
                {
                    num5 = list2[num4];
                    this.TrendCellList[i, list2[num4] + 6].Value = num5.ToString();
                    num5 = list2[num4];
                    this.TrendCellList[i, (list2[num4] + 0x10) + ((num4 - 1) * 10)].Value = num5.ToString();
                    num4++;
                }
                continue;
            Label_023C:
                num4 = 0;
                while (num4 < 3)
                {
                    num5 = list2[num4];
                    this.TrendCellList[i, list2[num4] + 6].Value = num5.ToString();
                    num5 = list2[num4];
                    this.TrendCellList[i, (list2[num4] + 0x10) + (num4 * 10)].Value = num5.ToString();
                    num4++;
                }
                continue;
            Label_02BC:
                num4 = 2;
                while (num4 < 5)
                {
                    num5 = list2[num4];
                    this.TrendCellList[i, list2[num4] + 6].Value = num5.ToString();
                    num5 = list2[num4];
                    this.TrendCellList[i, (list2[num4] + 0x10) + ((num4 - 2) * 10)].Value = num5.ToString();
                    num4++;
                }
                continue;
            Label_033E:
                num4 = 0;
                while (num4 < 2)
                {
                    num5 = list2[num4];
                    this.TrendCellList[i, list2[num4] + 6].Value = num5.ToString();
                    num5 = list2[num4];
                    this.TrendCellList[i, (list2[num4] + 0x10) + (num4 * 10)].Value = num5.ToString();
                    num4++;
                }
                continue;
            Label_03BB:
                num4 = 3;
                while (num4 < 5)
                {
                    num5 = list2[num4];
                    this.TrendCellList[i, list2[num4] + 6].Value = num5.ToString();
                    this.TrendCellList[i, (list2[num4] + 0x10) + ((num4 - 3) * 10)].Value = list2[num4].ToString();
                    num4++;
                }
            }
            switch (pText)
            {
                case "五星走势":
                    this.LineTrend(count, 6, 10, AppInfo.redBackColor, AppInfo.whiteColor, false);
                    this.LineTrend(count, 0x10, 10, AppInfo.greenBackColor, AppInfo.whiteColor, true);
                    this.LineTrend(count, 0x1a, 10, AppInfo.blueBackColor, AppInfo.whiteColor, true);
                    this.LineTrend(count, 0x24, 10, AppInfo.darkCyanBackColor, AppInfo.whiteColor, true);
                    this.LineTrend(count, 0x2e, 10, AppInfo.mediumBlueBackColor, AppInfo.whiteColor, true);
                    this.LineTrend(count, 0x38, 10, AppInfo.redBackColor, AppInfo.whiteColor, true);
                    break;

                case "四星走势":
                    this.LineTrend(count, 6, 10, AppInfo.redBackColor, AppInfo.whiteColor, false);
                    this.LineTrend(count, 0x10, 10, AppInfo.blueBackColor, AppInfo.whiteColor, true);
                    this.LineTrend(count, 0x1a, 10, AppInfo.darkCyanBackColor, AppInfo.whiteColor, true);
                    this.LineTrend(count, 0x24, 10, AppInfo.mediumBlueBackColor, AppInfo.whiteColor, true);
                    this.LineTrend(count, 0x2e, 10, AppInfo.redBackColor, AppInfo.whiteColor, true);
                    break;

                case "前三走势":
                    this.LineTrend(count, 6, 10, AppInfo.redBackColor, AppInfo.whiteColor, false);
                    this.LineTrend(count, 0x10, 10, AppInfo.greenBackColor, AppInfo.whiteColor, true);
                    this.LineTrend(count, 0x1a, 10, AppInfo.blueBackColor, AppInfo.whiteColor, true);
                    this.LineTrend(count, 0x24, 10, AppInfo.darkCyanBackColor, AppInfo.whiteColor, true);
                    break;

                case "后三走势":
                    this.LineTrend(count, 6, 10, AppInfo.redBackColor, AppInfo.whiteColor, false);
                    this.LineTrend(count, 0x10, 10, AppInfo.darkCyanBackColor, AppInfo.whiteColor, true);
                    this.LineTrend(count, 0x1a, 10, AppInfo.mediumBlueBackColor, AppInfo.whiteColor, true);
                    this.LineTrend(count, 0x24, 10, AppInfo.redBackColor, AppInfo.whiteColor, true);
                    break;

                case "前二走势":
                    this.LineTrend(count, 6, 10, AppInfo.redBackColor, AppInfo.whiteColor, false);
                    this.LineTrend(count, 0x10, 10, AppInfo.greenBackColor, AppInfo.whiteColor, true);
                    this.LineTrend(count, 0x1a, 10, AppInfo.blueBackColor, AppInfo.whiteColor, true);
                    break;

                case "后二走势":
                    this.LineTrend(count, 6, 10, AppInfo.redBackColor, AppInfo.whiteColor, false);
                    this.LineTrend(count, 0x10, 10, AppInfo.mediumBlueBackColor, AppInfo.whiteColor, true);
                    this.LineTrend(count, 0x1a, 10, AppInfo.redBackColor, AppInfo.whiteColor, true);
                    break;
            }
            this.CountTrend(count, this.TrendCellList.GetUpperBound(1) + 1);
            this.Egv_TrendList.RowsDefaultCellStyle.SelectionBackColor = Color.White;
            this.Egv_TrendList.RowCount = num2;
            this.Egv_TrendList.Refresh();
            this.Egv_TrendList.FirstDisplayedScrollingRowIndex = num2 - 1;
            this.Egv_TrendList.RowsDefaultCellStyle.SelectionBackColor = AppInfo.hotColor;
        }

        private void TrendSelectChange()
        {
            this._RunEvent = false;
            try
            {
                TreeGridNode selectedNode = this.Tgv_TrendList.SelectedNode;
                if (!selectedNode.HasChildren)
                {
                    string pText = selectedNode.Cells[0].Value.ToString();
                    MdiTabControl.TabPage tabControlIndex = this.GetTabControlIndex(pText);
                    if (tabControlIndex == null)
                    {
                        this.AddTabControl(pText);
                    }
                    else
                    {
                        tabControlIndex.Select();
                    }
                    if (AppInfo.DataList.Count != 0)
                    {
                        this.LoadTrendList();
                        this.TrendMain(pText);
                    }
                }
            }
            catch
            {
            }
            this._RunEvent = true;
        }

        private void TrendView_Load(object sender, EventArgs e)
        {
            try
            {
                this.LoadTabControl(ref this.Tab_Trend, this.Pnl_TrendTab);
                this.Tab_Trend.SelectedTabChanged += new EventHandler(this.Tab_Trend_SelectedTabChanged);
                this.LoadTrendList();
                this.LoadTrendSelectList();
                this.SetControlInfoByReg();
                this.BeautifyInterface();
            }
            catch
            {
            }
            this._RunEvent = true;
            this.Btn_Trend_Click(null, null);
        }

        private class DrawLine
        {
            public int Column = 0;
            public int OffsetX = 0;
            public int Row = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct TrendCell
        {
            public string Value;
            public Color BackColor;
            public Color ForeColor;
            public Color BackExpandColor;
            public Color ForeExpandColor;
            public bool IsOmission;
            public bool IsDrawLine;
            public bool IsHide;
        }

        private class TrendHeader
        {
            public List<TrendView.DrawLine> DrawLineList = new List<TrendView.DrawLine>();
            public List<int> LineList = new List<int>();
            public List<int> OmissionColorList = new List<int>();
            public List<int> OmissionList = new List<int>();
            public List<string> TextList = new List<string>();
            public List<TrendView.TrendHeaderSecond> TextSecondList = new List<TrendView.TrendHeaderSecond>();
            public List<int> WidthList = new List<int>();
        }

        private class TrendHeaderSecond
        {
            public int ColCount = 0;
            public int ColIndex = 0;
            public string Text = "";
        }
    }
}

