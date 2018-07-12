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

    public class BTCount : UserControl
    {
        private Button Btn_BTCount;
        private ComboBox Cbb_BTFN;
        private ComboBox Cbb_Cycle;
        private ComboBox Cbb_Times1;
        private ComboBox Cbb_Unit;
        private CheckBox Ckb_BTCopy;
        private CheckBox Ckb_BTImport;
        private IContainer components = null;
        private List<Control> ControlList = null;
        private ExpandGirdView Egv_TimesList;
        private Label Lbl_BTFN;
        private Label Lbl_Cycle;
        private Label Lbl_Money;
        private Label Lbl_Number;
        private Label Lbl_Prize;
        private Label Lbl_SingleValue;
        private Label Lbl_Times;
        private Label Lbl_Times1;
        private Label Lbl_Times3_1;
        private Label Lbl_Times3_2;
        private Panel Pnl_BTBottom;
        private Panel Pnl_BTCount;
        private Panel Pnl_BTLeft;
        private Panel Pnl_BTRight;
        private Panel Pnl_BTTop;
        private RadioButton Rdb_Times1;
        private RadioButton Rdb_Times2;
        private RadioButton Rdb_Times3;
        private RadioButton Rdb_Times4;
        private string RegConfigPath = "";
        private List<Control> SpecialControlList = null;
        private List<ConfigurationStatus.TimesData> TimesList = new List<ConfigurationStatus.TimesData>();
        private TextBox Txt_Number;
        private TextBox Txt_Prize;
        private TextBox Txt_Times2;
        private TextBox Txt_Times3_1;
        private TextBox Txt_Times3_2;
        private TextBox Txt_Times4;

        public BTCount()
        {
            this.InitializeComponent();
        }

        private void BeautifyInterface()
        {
            if (AppInfo.Account.Configuration.Beautify)
            {
                List<Control> pControlList = new List<Control> {
                    this.Pnl_BTTop,
                    this.Pnl_BTBottom
                };
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list2 = new List<Control> {
                    this.Btn_BTCount
                };
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Rdb_Times2,
                    this.Rdb_Times1,
                    this.Rdb_Times3,
                    this.Rdb_Times4,
                    this.Lbl_Times1,
                    this.Lbl_Times3_1,
                    this.Lbl_Times3_2,
                    this.Lbl_Money,
                    this.Lbl_SingleValue,
                    this.Lbl_Prize,
                    this.Lbl_Cycle,
                    this.Lbl_Number,
                    this.Ckb_BTCopy,
                    this.Btn_BTCount,
                    this.Ckb_BTImport,
                    this.Lbl_BTFN
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
                List<ComboBox> pComboBoxList = new List<ComboBox> {
                    this.Cbb_Times1,
                    this.Cbb_Cycle,
                    this.Cbb_Unit,
                    this.Cbb_BTFN
                };
                CommFunc.BeautifyComboBox(pComboBoxList);
            }
        }

        private void BTCount_Load(object sender, EventArgs e)
        {
            List<CheckBox> pCheckBoxList = new List<CheckBox> {
                this.Ckb_BTCopy,
                this.Ckb_BTImport
            };
            CommFunc.SetCheckBoxFormatFlat(pCheckBoxList);
            this.BeautifyInterface();
            this.LoadTimesList();
            this.LoadBTControl();
            this.SetControlInfoByReg();
        }

        private void Btn_BTCount_Click(object sender, EventArgs e)
        {
            ConfigurationStatus.TimesCount pTimesInfo = null;
            if (this.GetTimesCount(ref pTimesInfo))
            {
                this.CountTimesList(pTimesInfo);
                this.RefreshBTDataList();
            }
        }

        private void Btn_BTImport_Click(object sender, EventArgs e)
        {
            if (this.Egv_TimesList.RowCount == 0)
            {
                CommFunc.PublicMessageAll("无法导入空的直线倍投！", true, MessageBoxIcon.Asterisk, "");
            }
            else
            {
                string selectBTFN = this.GetSelectBTFN();
                List<string> bTList = this.GetBTList();
                if (AppInfo.BTImport != null)
                {
                    AppInfo.BTImport(selectBTFN, bTList);
                }
            }
        }

        private void Cbb_Unit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AppInfo.PTInfo != null)
            {
                ConfigurationStatus.SCUnitType selectedIndex = (ConfigurationStatus.SCUnitType) this.Cbb_Unit.SelectedIndex;
                this.Lbl_SingleValue.Text = AppInfo.PTInfo.IsBetsMoney1(selectedIndex) ? "1" : "2";
            }
        }

        private void Ckb_BTCopy_Click(object sender, EventArgs e)
        {
            string bTString = this.GetBTString();
            if (bTString == "")
            {
                CommFunc.PublicMessageAll("没有倍投可以复制！", true, MessageBoxIcon.Asterisk, "");
            }
            else
            {
                CommFunc.CopyText(bTString);
                CommFunc.PublicMessageAll("复制成功！", true, MessageBoxIcon.Asterisk, "");
            }
        }

        private int CountSingeTimes(ConfigurationStatus.TimesCount pTimesInfo, int pIndex, ConfigurationStatus.TimesData pTimes = null)
        {
            for (int i = 1; i < 0x989680; i++)
            {
                double num3 = (pTimesInfo.Money * pTimesInfo.Number) * i;
                double num4 = pTimesInfo.TotalOutput + num3;
                double num5 = pTimesInfo.Mode * i;
                double num6 = num5 - num4;
                double num7 = (num6 / num4) * 100.0;
                if (pTimesInfo.Type == ConfigurationStatus.TimesType.GainRatio)
                {
                    if (num7 < pTimesInfo.GainRatio)
                    {
                        continue;
                    }
                }
                else if (pTimesInfo.Type == ConfigurationStatus.TimesType.GainFix)
                {
                    if (num6 < pTimesInfo.GainFix)
                    {
                        continue;
                    }
                }
                else if (pTimesInfo.Type == ConfigurationStatus.TimesType.Sum)
                {
                    if (num6 < (pTimesInfo.SumBegin + pTimesInfo.SumStep))
                    {
                        continue;
                    }
                    pTimesInfo.SumBegin = num6;
                }
                else if ((pTimesInfo.Type == ConfigurationStatus.TimesType.Free) && (pTimesInfo.FreeList[pIndex - 1] != i))
                {
                    continue;
                }
                if (pTimes != null)
                {
                    pTimes.Times = i;
                    pTimes.Output = num3;
                    pTimes.TotalOutput = num4;
                    pTimes.Input = num5;
                    pTimes.Gain = num6;
                    pTimes.GainRatio = num7;
                }
                pTimesInfo.TotalOutput = num4;
                return i;
            }
            return -1;
        }

        private void CountTimesList(ConfigurationStatus.TimesCount pTimeInfo)
        {
            this.TimesList.Clear();
            for (int i = 1; i <= pTimeInfo.Cycle; i++)
            {
                ConfigurationStatus.TimesData pTimes = new ConfigurationStatus.TimesData {
                    Expect = i.ToString()
                };
                if (this.CountSingeTimes(pTimeInfo, i, pTimes) == -1)
                {
                    CommFunc.PublicMessageAll("该计划不适合投注！", true, MessageBoxIcon.Asterisk, "");
                    if (this.TimesList.Count == 0)
                    {
                        this.TimesList.Add(this.GetOneTimes(pTimeInfo));
                    }
                    break;
                }
                this.TimesList.Add(pTimes);
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

        private void Egv_TimesList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                if ((this.Egv_TimesList.RowCount != 0) && (this.TimesList.Count != 0))
                {
                    DataGridViewCell cell = this.Egv_TimesList.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    ConfigurationStatus.TimesData data = this.TimesList[e.RowIndex];
                    string expect = "";
                    if (e.ColumnIndex == 0)
                    {
                        expect = data.Expect;
                    }
                    else if (e.ColumnIndex == 1)
                    {
                        expect = data.Times.ToString();
                    }
                    else if (e.ColumnIndex == 2)
                    {
                        expect = data.Output.ToString("0.00");
                    }
                    else if (e.ColumnIndex == 3)
                    {
                        expect = data.TotalOutput.ToString("0.00");
                    }
                    else if (e.ColumnIndex == 4)
                    {
                        expect = data.Input.ToString("0.00");
                    }
                    else if (e.ColumnIndex == 5)
                    {
                        expect = data.Gain.ToString("0.00");
                    }
                    else if (e.ColumnIndex == 6)
                    {
                        expect = data.GainRatio.ToString("0.00") + "%";
                    }
                    e.Value = expect;
                }
            }
            catch
            {
            }
        }

        private List<string> GetBTList()
        {
            List<string> list = new List<string>();
            foreach (ConfigurationStatus.TimesData data in this.TimesList)
            {
                list.Add(data.Times.ToString());
            }
            return list;
        }

        private string GetBTString() => 
            CommFunc.Join(this.GetBTList(), ",");

        private int GetNumber() => 
            Convert.ToInt32(this.Txt_Number.Text);

        private ConfigurationStatus.TimesData GetOneTimes(ConfigurationStatus.TimesCount pTimesInfo)
        {
            ConfigurationStatus.TimesData data = new ConfigurationStatus.TimesData {
                Expect = "1",
                Times = 1.0
            };
            data.TotalOutput = data.Output = pTimesInfo.Money * pTimesInfo.Number;
            data.Input = pTimesInfo.Mode;
            data.Gain = data.Input - data.TotalOutput;
            data.GainRatio = (data.Gain / data.TotalOutput) * 100.0;
            return data;
        }

        public string GetSelectBTFN() => 
            this.Cbb_BTFN.Text;

        private bool GetTimesCount(ref ConfigurationStatus.TimesCount pTimesInfo)
        {
            pTimesInfo = new ConfigurationStatus.TimesCount();
            pTimesInfo.Cycle = Convert.ToInt32(this.Cbb_Cycle.Text);
            if (CommFunc.CheckTextBoxIsNull(this.Txt_Prize, this.Lbl_Prize.Text))
            {
                return false;
            }
            int selectedIndex = this.Cbb_Unit.SelectedIndex;
            ConfigurationStatus.SCUnitType pUnitType = (ConfigurationStatus.SCUnitType) selectedIndex;
            pTimesInfo.Money = ((AutoBetsWindow) Program.MainApp).GetBetMoney(pUnitType);
            pTimesInfo.Mode = Convert.ToDouble(this.Txt_Prize.Text) / Math.Pow(10.0, (double) selectedIndex);
            if (CommFunc.CheckTextBoxIsNull(this.Txt_Number, this.Lbl_Number.Text))
            {
                return false;
            }
            pTimesInfo.Number = this.GetNumber();
            RadioButton timesType = this.GetTimesType();
            if (timesType == this.Rdb_Times1)
            {
                pTimesInfo.GainRatio = Convert.ToDouble(this.Cbb_Times1.Text);
                pTimesInfo.Type = ConfigurationStatus.TimesType.GainRatio;
                pTimesInfo.TypeString = "收益率";
            }
            else if (timesType == this.Rdb_Times2)
            {
                if (CommFunc.CheckTextBoxIsNull(this.Txt_Times2, this.Rdb_Times2.Text.Replace("：", "")))
                {
                    return false;
                }
                pTimesInfo.GainFix = Convert.ToDouble(this.Txt_Times2.Text);
                pTimesInfo.Type = ConfigurationStatus.TimesType.GainFix;
                pTimesInfo.TypeString = "固定利润";
            }
            else if (timesType == this.Rdb_Times3)
            {
                if (CommFunc.CheckTextBoxIsNull(this.Txt_Times3_1, this.Lbl_Times3_1.Text))
                {
                    return false;
                }
                if (CommFunc.CheckTextBoxIsNull(this.Txt_Times3_2, this.Lbl_Times3_2.Text))
                {
                    return false;
                }
                pTimesInfo.SumBegin = Convert.ToDouble(this.Txt_Times3_1.Text);
                pTimesInfo.SumStep = Convert.ToDouble(this.Txt_Times3_2.Text);
                pTimesInfo.Type = ConfigurationStatus.TimesType.Sum;
                pTimesInfo.TypeString = "累加利润";
            }
            else if (timesType == this.Rdb_Times4)
            {
                if (CommFunc.CheckTextBoxIsNull(this.Txt_Times4, this.Rdb_Times4.Text.Replace("：", "")))
                {
                    return false;
                }
                List<string> list = CommFunc.SplitString(this.Txt_Times4.Text, ",", -1);
                foreach (string str in list)
                {
                    if (str != "")
                    {
                        pTimesInfo.FreeList.Add(Convert.ToInt32(str));
                    }
                }
                if (pTimesInfo.FreeList.Count != pTimesInfo.Cycle)
                {
                    CommFunc.PublicMessageAll("倍数的个数和周期不一致！", true, MessageBoxIcon.Asterisk, "");
                    this.Txt_Times4.Focus();
                    return false;
                }
                pTimesInfo.Type = ConfigurationStatus.TimesType.Free;
                pTimesInfo.TypeString = "自由倍数";
            }
            return true;
        }

        private RadioButton GetTimesType()
        {
            List<RadioButton> list = new List<RadioButton> {
                this.Rdb_Times1,
                this.Rdb_Times2,
                this.Rdb_Times3,
                this.Rdb_Times4
            };
            foreach (RadioButton button2 in list)
            {
                if (button2.Checked)
                {
                    return button2;
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
            this.Pnl_BTCount = new Panel();
            this.Egv_TimesList = new ExpandGirdView(this.components);
            this.Pnl_BTBottom = new Panel();
            this.Ckb_BTImport = new CheckBox();
            this.Ckb_BTCopy = new CheckBox();
            this.Cbb_BTFN = new ComboBox();
            this.Lbl_BTFN = new Label();
            this.Pnl_BTTop = new Panel();
            this.Pnl_BTRight = new Panel();
            this.Btn_BTCount = new Button();
            this.Pnl_BTLeft = new Panel();
            this.Cbb_Cycle = new ComboBox();
            this.Lbl_Cycle = new Label();
            this.Txt_Number = new TextBox();
            this.Lbl_Number = new Label();
            this.Txt_Times4 = new TextBox();
            this.Rdb_Times4 = new RadioButton();
            this.Lbl_Times3_2 = new Label();
            this.Txt_Times3_2 = new TextBox();
            this.Lbl_Times3_1 = new Label();
            this.Txt_Times3_1 = new TextBox();
            this.Rdb_Times3 = new RadioButton();
            this.Txt_Times2 = new TextBox();
            this.Rdb_Times2 = new RadioButton();
            this.Lbl_Times1 = new Label();
            this.Cbb_Times1 = new ComboBox();
            this.Rdb_Times1 = new RadioButton();
            this.Lbl_SingleValue = new Label();
            this.Txt_Prize = new TextBox();
            this.Cbb_Unit = new ComboBox();
            this.Lbl_Prize = new Label();
            this.Lbl_Money = new Label();
            this.Lbl_Times = new Label();
            this.Pnl_BTCount.SuspendLayout();
            ((ISupportInitialize) this.Egv_TimesList).BeginInit();
            this.Pnl_BTBottom.SuspendLayout();
            this.Pnl_BTTop.SuspendLayout();
            this.Pnl_BTRight.SuspendLayout();
            this.Pnl_BTLeft.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_BTCount.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_BTCount.Controls.Add(this.Egv_TimesList);
            this.Pnl_BTCount.Controls.Add(this.Pnl_BTBottom);
            this.Pnl_BTCount.Controls.Add(this.Pnl_BTTop);
            this.Pnl_BTCount.Dock = DockStyle.Fill;
            this.Pnl_BTCount.Location = new Point(0, 0);
            this.Pnl_BTCount.Name = "Pnl_BTCount";
            this.Pnl_BTCount.Size = new Size(0x355, 500);
            this.Pnl_BTCount.TabIndex = 14;
            this.Egv_TimesList.AllowUserToAddRows = false;
            this.Egv_TimesList.AllowUserToDeleteRows = false;
            this.Egv_TimesList.AllowUserToResizeColumns = false;
            this.Egv_TimesList.AllowUserToResizeRows = false;
            this.Egv_TimesList.BackgroundColor = Color.White;
            this.Egv_TimesList.BorderStyle = BorderStyle.Fixed3D;
            this.Egv_TimesList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style.BackColor = SystemColors.Window;
            style.Font = new Font("微软雅黑", 9f);
            style.ForeColor = SystemColors.ControlText;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.False;
            this.Egv_TimesList.ColumnHeadersDefaultCellStyle = style;
            this.Egv_TimesList.ColumnHeadersHeight = 30;
            this.Egv_TimesList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            style2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style2.BackColor = SystemColors.Control;
            style2.Font = new Font("微软雅黑", 9f);
            style2.ForeColor = SystemColors.ControlText;
            style2.SelectionBackColor = Color.SteelBlue;
            style2.SelectionForeColor = Color.White;
            style2.WrapMode = DataGridViewTriState.False;
            this.Egv_TimesList.DefaultCellStyle = style2;
            this.Egv_TimesList.Dock = DockStyle.Fill;
            this.Egv_TimesList.DragLineColor = Color.Silver;
            this.Egv_TimesList.ExternalVirtualMode = true;
            this.Egv_TimesList.GridColor = Color.Silver;
            this.Egv_TimesList.HeadersCheckDefult = CheckState.Checked;
            this.Egv_TimesList.Location = new Point(0, 0x7e);
            this.Egv_TimesList.MergeColumnHeaderBackColor = SystemColors.Control;
            this.Egv_TimesList.MergeColumnHeaderForeColor = Color.Black;
            this.Egv_TimesList.MultiSelect = false;
            this.Egv_TimesList.Name = "Egv_TimesList";
            this.Egv_TimesList.RowHeadersVisible = false;
            this.Egv_TimesList.RowNum = 0x11;
            style3.BackColor = Color.White;
            style3.SelectionBackColor = Color.FromArgb(0xde, 0xe8, 0xfc);
            style3.SelectionForeColor = Color.Black;
            this.Egv_TimesList.RowsDefaultCellStyle = style3;
            this.Egv_TimesList.RowTemplate.Height = 0x17;
            this.Egv_TimesList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Egv_TimesList.Size = new Size(0x353, 0x151);
            this.Egv_TimesList.TabIndex = 0x48;
            this.Egv_TimesList.VirtualMode = true;
            this.Pnl_BTBottom.Controls.Add(this.Ckb_BTImport);
            this.Pnl_BTBottom.Controls.Add(this.Ckb_BTCopy);
            this.Pnl_BTBottom.Controls.Add(this.Cbb_BTFN);
            this.Pnl_BTBottom.Controls.Add(this.Lbl_BTFN);
            this.Pnl_BTBottom.Dock = DockStyle.Bottom;
            this.Pnl_BTBottom.Location = new Point(0, 0x1cf);
            this.Pnl_BTBottom.Name = "Pnl_BTBottom";
            this.Pnl_BTBottom.Size = new Size(0x353, 0x23);
            this.Pnl_BTBottom.TabIndex = 0x4a;
            this.Ckb_BTImport.Appearance = Appearance.Button;
            this.Ckb_BTImport.AutoCheck = false;
            this.Ckb_BTImport.FlatAppearance.BorderSize = 0;
            this.Ckb_BTImport.FlatStyle = FlatStyle.Flat;
            this.Ckb_BTImport.Image = Resources.ImportFile;
            this.Ckb_BTImport.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_BTImport.Location = new Point(90, 5);
            this.Ckb_BTImport.Name = "Ckb_BTImport";
            this.Ckb_BTImport.Size = new Size(80, 0x19);
            this.Ckb_BTImport.TabIndex = 0xa8;
            this.Ckb_BTImport.Text = "直接导入";
            this.Ckb_BTImport.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_BTImport.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_BTImport.UseVisualStyleBackColor = true;
            this.Ckb_BTImport.Click += new EventHandler(this.Btn_BTImport_Click);
            this.Ckb_BTCopy.Appearance = Appearance.Button;
            this.Ckb_BTCopy.AutoCheck = false;
            this.Ckb_BTCopy.FlatAppearance.BorderSize = 0;
            this.Ckb_BTCopy.FlatStyle = FlatStyle.Flat;
            this.Ckb_BTCopy.Image = Resources.Copy;
            this.Ckb_BTCopy.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_BTCopy.Location = new Point(4, 5);
            this.Ckb_BTCopy.Name = "Ckb_BTCopy";
            this.Ckb_BTCopy.Size = new Size(80, 0x19);
            this.Ckb_BTCopy.TabIndex = 0xa7;
            this.Ckb_BTCopy.Text = "复制倍投";
            this.Ckb_BTCopy.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_BTCopy.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_BTCopy.UseVisualStyleBackColor = true;
            this.Ckb_BTCopy.Click += new EventHandler(this.Ckb_BTCopy_Click);
            this.Cbb_BTFN.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_BTFN.FormattingEnabled = true;
            this.Cbb_BTFN.Location = new Point(0xe0, 5);
            this.Cbb_BTFN.Name = "Cbb_BTFN";
            this.Cbb_BTFN.Size = new Size(0x7d, 0x19);
            this.Cbb_BTFN.TabIndex = 160;
            this.Lbl_BTFN.AutoSize = true;
            this.Lbl_BTFN.Location = new Point(0xb1, 9);
            this.Lbl_BTFN.Name = "Lbl_BTFN";
            this.Lbl_BTFN.Size = new Size(0x2c, 0x11);
            this.Lbl_BTFN.TabIndex = 0xa1;
            this.Lbl_BTFN.Text = "方案：";
            this.Pnl_BTTop.Controls.Add(this.Pnl_BTRight);
            this.Pnl_BTTop.Controls.Add(this.Pnl_BTLeft);
            this.Pnl_BTTop.Controls.Add(this.Lbl_Times);
            this.Pnl_BTTop.Dock = DockStyle.Top;
            this.Pnl_BTTop.Location = new Point(0, 0);
            this.Pnl_BTTop.Name = "Pnl_BTTop";
            this.Pnl_BTTop.Size = new Size(0x353, 0x7e);
            this.Pnl_BTTop.TabIndex = 0;
            this.Pnl_BTRight.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_BTRight.Controls.Add(this.Btn_BTCount);
            this.Pnl_BTRight.Dock = DockStyle.Left;
            this.Pnl_BTRight.Location = new Point(490, 0);
            this.Pnl_BTRight.Name = "Pnl_BTRight";
            this.Pnl_BTRight.Size = new Size(0x75, 0x7e);
            this.Pnl_BTRight.TabIndex = 0xaf;
            this.Btn_BTCount.Dock = DockStyle.Fill;
            this.Btn_BTCount.Font = new Font("微软雅黑", 14f, FontStyle.Bold);
            this.Btn_BTCount.Location = new Point(0, 0);
            this.Btn_BTCount.Name = "Btn_BTCount";
            this.Btn_BTCount.Size = new Size(0x73, 0x7c);
            this.Btn_BTCount.TabIndex = 0xaf;
            this.Btn_BTCount.Text = "计算倍投";
            this.Btn_BTCount.UseVisualStyleBackColor = true;
            this.Btn_BTCount.Click += new EventHandler(this.Btn_BTCount_Click);
            this.Pnl_BTLeft.BackColor = Color.Transparent;
            this.Pnl_BTLeft.Controls.Add(this.Cbb_Cycle);
            this.Pnl_BTLeft.Controls.Add(this.Lbl_Cycle);
            this.Pnl_BTLeft.Controls.Add(this.Txt_Number);
            this.Pnl_BTLeft.Controls.Add(this.Lbl_Number);
            this.Pnl_BTLeft.Controls.Add(this.Txt_Times4);
            this.Pnl_BTLeft.Controls.Add(this.Rdb_Times4);
            this.Pnl_BTLeft.Controls.Add(this.Lbl_Times3_2);
            this.Pnl_BTLeft.Controls.Add(this.Txt_Times3_2);
            this.Pnl_BTLeft.Controls.Add(this.Lbl_Times3_1);
            this.Pnl_BTLeft.Controls.Add(this.Txt_Times3_1);
            this.Pnl_BTLeft.Controls.Add(this.Rdb_Times3);
            this.Pnl_BTLeft.Controls.Add(this.Txt_Times2);
            this.Pnl_BTLeft.Controls.Add(this.Rdb_Times2);
            this.Pnl_BTLeft.Controls.Add(this.Lbl_Times1);
            this.Pnl_BTLeft.Controls.Add(this.Cbb_Times1);
            this.Pnl_BTLeft.Controls.Add(this.Rdb_Times1);
            this.Pnl_BTLeft.Controls.Add(this.Lbl_SingleValue);
            this.Pnl_BTLeft.Controls.Add(this.Txt_Prize);
            this.Pnl_BTLeft.Controls.Add(this.Cbb_Unit);
            this.Pnl_BTLeft.Controls.Add(this.Lbl_Prize);
            this.Pnl_BTLeft.Controls.Add(this.Lbl_Money);
            this.Pnl_BTLeft.Dock = DockStyle.Left;
            this.Pnl_BTLeft.Location = new Point(0, 0);
            this.Pnl_BTLeft.Name = "Pnl_BTLeft";
            this.Pnl_BTLeft.Size = new Size(490, 0x7e);
            this.Pnl_BTLeft.TabIndex = 0xae;
            this.Cbb_Cycle.ForeColor = SystemColors.ControlText;
            this.Cbb_Cycle.FormattingEnabled = true;
            this.Cbb_Cycle.Location = new Point(0x18b, 0x41);
            this.Cbb_Cycle.Name = "Cbb_Cycle";
            this.Cbb_Cycle.Size = new Size(80, 0x19);
            this.Cbb_Cycle.TabIndex = 0xb8;
            this.Lbl_Cycle.AutoSize = true;
            this.Lbl_Cycle.ForeColor = SystemColors.ControlText;
            this.Lbl_Cycle.Location = new Point(320, 70);
            this.Lbl_Cycle.Name = "Lbl_Cycle";
            this.Lbl_Cycle.Size = new Size(0x44, 0x11);
            this.Lbl_Cycle.TabIndex = 0xb7;
            this.Lbl_Cycle.Text = "计划周期：";
            this.Txt_Number.ForeColor = SystemColors.ControlText;
            this.Txt_Number.Location = new Point(0x18b, 0x61);
            this.Txt_Number.Name = "Txt_Number";
            this.Txt_Number.Size = new Size(80, 0x17);
            this.Txt_Number.TabIndex = 0xb6;
            this.Txt_Number.Text = "500";
            this.Lbl_Number.AutoSize = true;
            this.Lbl_Number.ForeColor = SystemColors.ControlText;
            this.Lbl_Number.Location = new Point(320, 100);
            this.Lbl_Number.Name = "Lbl_Number";
            this.Lbl_Number.Size = new Size(0x44, 0x11);
            this.Lbl_Number.TabIndex = 0xb5;
            this.Lbl_Number.Text = "号码注数：";
            this.Txt_Times4.Enabled = false;
            this.Txt_Times4.ForeColor = SystemColors.ControlText;
            this.Txt_Times4.Location = new Point(0x5f, 0x61);
            this.Txt_Times4.Name = "Txt_Times4";
            this.Txt_Times4.Size = new Size(0xce, 0x17);
            this.Txt_Times4.TabIndex = 180;
            this.Txt_Times4.Text = "10,20,30";
            this.Txt_Times4.KeyPress += new KeyPressEventHandler(this.Txt_Input_KeyPress);
            this.Rdb_Times4.AutoSize = true;
            this.Rdb_Times4.ForeColor = SystemColors.ControlText;
            this.Rdb_Times4.Location = new Point(4, 0x62);
            this.Rdb_Times4.Name = "Rdb_Times4";
            this.Rdb_Times4.Size = new Size(0x56, 0x15);
            this.Rdb_Times4.TabIndex = 0xb3;
            this.Rdb_Times4.Text = "自由倍数：";
            this.Rdb_Times4.UseVisualStyleBackColor = true;
            this.Rdb_Times4.CheckedChanged += new EventHandler(this.Rdb_Times_CheckedChanged);
            this.Lbl_Times3_2.AutoSize = true;
            this.Lbl_Times3_2.Enabled = false;
            this.Lbl_Times3_2.ForeColor = SystemColors.ControlText;
            this.Lbl_Times3_2.Location = new Point(0xcb, 0x47);
            this.Lbl_Times3_2.Name = "Lbl_Times3_2";
            this.Lbl_Times3_2.Size = new Size(0x20, 0x11);
            this.Lbl_Times3_2.TabIndex = 0xb2;
            this.Lbl_Times3_2.Text = "累进";
            this.Txt_Times3_2.Enabled = false;
            this.Txt_Times3_2.ForeColor = SystemColors.ControlText;
            this.Txt_Times3_2.Location = new Point(0xf2, 0x44);
            this.Txt_Times3_2.Name = "Txt_Times3_2";
            this.Txt_Times3_2.Size = new Size(60, 0x17);
            this.Txt_Times3_2.TabIndex = 0xb1;
            this.Txt_Times3_2.Text = "200";
            this.Lbl_Times3_1.AutoSize = true;
            this.Lbl_Times3_1.Enabled = false;
            this.Lbl_Times3_1.ForeColor = SystemColors.ControlText;
            this.Lbl_Times3_1.Location = new Point(0x5c, 0x47);
            this.Lbl_Times3_1.Name = "Lbl_Times3_1";
            this.Lbl_Times3_1.Size = new Size(0x20, 0x11);
            this.Lbl_Times3_1.TabIndex = 0xb0;
            this.Lbl_Times3_1.Text = "起步";
            this.Txt_Times3_1.Enabled = false;
            this.Txt_Times3_1.ForeColor = SystemColors.ControlText;
            this.Txt_Times3_1.Location = new Point(0x83, 0x44);
            this.Txt_Times3_1.Name = "Txt_Times3_1";
            this.Txt_Times3_1.Size = new Size(60, 0x17);
            this.Txt_Times3_1.TabIndex = 0xaf;
            this.Txt_Times3_1.Text = "1000";
            this.Rdb_Times3.AutoSize = true;
            this.Rdb_Times3.ForeColor = SystemColors.ControlText;
            this.Rdb_Times3.Location = new Point(4, 0x44);
            this.Rdb_Times3.Name = "Rdb_Times3";
            this.Rdb_Times3.Size = new Size(0x56, 0x15);
            this.Rdb_Times3.TabIndex = 0xae;
            this.Rdb_Times3.Text = "累加利润：";
            this.Rdb_Times3.UseVisualStyleBackColor = true;
            this.Rdb_Times3.CheckedChanged += new EventHandler(this.Rdb_Times_CheckedChanged);
            this.Txt_Times2.ForeColor = SystemColors.ControlText;
            this.Txt_Times2.Location = new Point(0x5f, 7);
            this.Txt_Times2.Name = "Txt_Times2";
            this.Txt_Times2.Size = new Size(0x60, 0x17);
            this.Txt_Times2.TabIndex = 0xad;
            this.Txt_Times2.Text = "1000";
            this.Rdb_Times2.AutoSize = true;
            this.Rdb_Times2.Checked = true;
            this.Rdb_Times2.ForeColor = SystemColors.ControlText;
            this.Rdb_Times2.Location = new Point(5, 8);
            this.Rdb_Times2.Name = "Rdb_Times2";
            this.Rdb_Times2.Size = new Size(0x56, 0x15);
            this.Rdb_Times2.TabIndex = 0xac;
            this.Rdb_Times2.TabStop = true;
            this.Rdb_Times2.Text = "固定利润：";
            this.Rdb_Times2.UseVisualStyleBackColor = true;
            this.Rdb_Times2.CheckedChanged += new EventHandler(this.Rdb_Times_CheckedChanged);
            this.Lbl_Times1.AutoSize = true;
            this.Lbl_Times1.Enabled = false;
            this.Lbl_Times1.ForeColor = SystemColors.ControlText;
            this.Lbl_Times1.Location = new Point(0xcb, 0x27);
            this.Lbl_Times1.Name = "Lbl_Times1";
            this.Lbl_Times1.Size = new Size(0x13, 0x11);
            this.Lbl_Times1.TabIndex = 0xab;
            this.Lbl_Times1.Text = "%";
            this.Cbb_Times1.Enabled = false;
            this.Cbb_Times1.ForeColor = SystemColors.ControlText;
            this.Cbb_Times1.Location = new Point(0x5f, 0x24);
            this.Cbb_Times1.Name = "Cbb_Times1";
            this.Cbb_Times1.Size = new Size(0x60, 0x19);
            this.Cbb_Times1.TabIndex = 170;
            this.Rdb_Times1.AutoSize = true;
            this.Rdb_Times1.ForeColor = SystemColors.ControlText;
            this.Rdb_Times1.Location = new Point(5, 0x26);
            this.Rdb_Times1.Name = "Rdb_Times1";
            this.Rdb_Times1.Size = new Size(0x56, 0x15);
            this.Rdb_Times1.TabIndex = 0xa9;
            this.Rdb_Times1.Text = "收益利率：";
            this.Rdb_Times1.UseVisualStyleBackColor = true;
            this.Rdb_Times1.CheckedChanged += new EventHandler(this.Rdb_Times_CheckedChanged);
            this.Lbl_SingleValue.AutoSize = true;
            this.Lbl_SingleValue.ForeColor = SystemColors.ControlText;
            this.Lbl_SingleValue.Location = new Point(0x187, 10);
            this.Lbl_SingleValue.Name = "Lbl_SingleValue";
            this.Lbl_SingleValue.Size = new Size(15, 0x11);
            this.Lbl_SingleValue.TabIndex = 0xa8;
            this.Lbl_SingleValue.Text = "2";
            this.Txt_Prize.ForeColor = SystemColors.ControlText;
            this.Txt_Prize.Location = new Point(0x18b, 0x25);
            this.Txt_Prize.Name = "Txt_Prize";
            this.Txt_Prize.Size = new Size(80, 0x17);
            this.Txt_Prize.TabIndex = 0xa7;
            this.Txt_Prize.Text = "1930";
            this.Cbb_Unit.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_Unit.ForeColor = SystemColors.ControlText;
            this.Cbb_Unit.FormattingEnabled = true;
            this.Cbb_Unit.Items.AddRange(new object[] { "元", "角", "分", "厘" });
            this.Cbb_Unit.Location = new Point(0x19b, 6);
            this.Cbb_Unit.Name = "Cbb_Unit";
            this.Cbb_Unit.Size = new Size(0x40, 0x19);
            this.Cbb_Unit.TabIndex = 0xa6;
            this.Cbb_Unit.SelectedIndexChanged += new EventHandler(this.Cbb_Unit_SelectedIndexChanged);
            this.Lbl_Prize.AutoSize = true;
            this.Lbl_Prize.ForeColor = SystemColors.ControlText;
            this.Lbl_Prize.Location = new Point(320, 40);
            this.Lbl_Prize.Name = "Lbl_Prize";
            this.Lbl_Prize.Size = new Size(0x44, 0x11);
            this.Lbl_Prize.TabIndex = 0xa5;
            this.Lbl_Prize.Text = "平台奖金：";
            this.Lbl_Money.AutoSize = true;
            this.Lbl_Money.ForeColor = SystemColors.ControlText;
            this.Lbl_Money.Location = new Point(320, 10);
            this.Lbl_Money.Name = "Lbl_Money";
            this.Lbl_Money.Size = new Size(0x44, 0x11);
            this.Lbl_Money.TabIndex = 0xa4;
            this.Lbl_Money.Text = "单注成本：";
            this.Lbl_Times.AutoSize = true;
            this.Lbl_Times.Location = new Point(-1000, 10);
            this.Lbl_Times.Name = "Lbl_Times";
            this.Lbl_Times.Size = new Size(0x2c, 0x11);
            this.Lbl_Times.TabIndex = 0x8b;
            this.Lbl_Times.Text = "倍数：";
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.Pnl_BTCount);
            this.Font = new Font("微软雅黑", 9f);
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "BTCount";
            base.Size = new Size(0x355, 500);
            base.Load += new EventHandler(this.BTCount_Load);
            this.Pnl_BTCount.ResumeLayout(false);
            ((ISupportInitialize) this.Egv_TimesList).EndInit();
            this.Pnl_BTBottom.ResumeLayout(false);
            this.Pnl_BTBottom.PerformLayout();
            this.Pnl_BTTop.ResumeLayout(false);
            this.Pnl_BTTop.PerformLayout();
            this.Pnl_BTRight.ResumeLayout(false);
            this.Pnl_BTLeft.ResumeLayout(false);
            this.Pnl_BTLeft.PerformLayout();
            base.ResumeLayout(false);
        }

        public void LoadBTControl()
        {
            CommFunc.FillComboBoxItem(this.Cbb_Cycle, 1, 50, 1);
            CommFunc.FillComboBoxItem(this.Cbb_Times1, 10, 90, 10);
        }

        private void LoadTimesList()
        {
            List<int> pType = new List<int> { 
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1
            };
            List<string> pText = new List<string> { 
                "期号",
                "倍数",
                "当期投入",
                "累计投入",
                "收益",
                "利润",
                "收益率%",
                ""
            };
            List<int> pWidth = new List<int> { 
                60,
                90,
                90,
                90,
                90,
                90,
                90,
                10
            };
            List<bool> pRead = new List<bool> { 
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true
            };
            List<bool> pVis = new List<bool> { 
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true
            };
            this.Egv_TimesList.LoadInitialization(pType, pText, pWidth, pRead, pVis, null);
            this.Egv_TimesList.MultiSelect = true;
            this.Egv_TimesList.VirtualMode = true;
            CommFunc.SetExpandGirdViewFormat(this.Egv_TimesList, 9);
            this.Egv_TimesList.CellValueNeeded += new DataGridViewCellValueEventHandler(this.Egv_TimesList_CellValueNeeded);
            for (int i = 0; i < this.Egv_TimesList.ColumnCount; i++)
            {
                this.Egv_TimesList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.Egv_TimesList.Columns[this.Egv_TimesList.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void Rdb_Times_CheckedChanged(object sender, EventArgs e)
        {
            this.Cbb_Times1.Enabled = this.Lbl_Times1.Enabled = this.Rdb_Times1.Checked;
            this.Txt_Times2.Enabled = this.Rdb_Times2.Checked;
            this.Lbl_Times3_1.Enabled = this.Txt_Times3_1.Enabled = this.Lbl_Times3_2.Enabled = this.Txt_Times3_2.Enabled = this.Rdb_Times3.Checked;
            this.Txt_Times4.Enabled = this.Rdb_Times4.Checked;
        }

        private void RefreshBTDataList()
        {
            CommFunc.RefreshDataGridView(this.Egv_TimesList, this.TimesList.Count);
        }

        public void RefreshBTList()
        {
            CommFunc.SetComboBoxList(this.Cbb_BTFN, AppInfo.SchemeList);
            if ((this.Cbb_BTFN.SelectedIndex == -1) && (this.Cbb_BTFN.Items.Count > 0))
            {
                this.Cbb_BTFN.SelectedIndex = 0;
            }
        }

        public void SaveControlInfoByReg()
        {
            CommFunc.SaveFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SaveSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
        }

        public void SetControlInfoByReg()
        {
            this.RegConfigPath = @"software\TUHAOPLUS\YXZXGJ\DlgConfig\永信在线挂机软件\" + base.Name;
            List<Control> list = new List<Control> {
                this.Txt_Prize,
                this.Rdb_Times1,
                this.Rdb_Times2,
                this.Rdb_Times3,
                this.Rdb_Times4,
                this.Txt_Times2,
                this.Txt_Times3_1,
                this.Txt_Times3_2,
                this.Txt_Times4,
                this.Txt_Number
            };
            this.ControlList = list;
            List<Control> list2 = new List<Control> {
                this.Cbb_Cycle,
                this.Cbb_Unit,
                this.Cbb_Times1
            };
            this.SpecialControlList = list2;
            CommFunc.SetFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SetSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
            CommFunc.AppHandler(this.Cbb_Unit);
            this.Cbb_Unit_SelectedIndexChanged(null, null);
        }

        public void SetSelectBTFN(string pFNName, string pFreeValue)
        {
            CommFunc.SetComboBoxSelectedIndex(this.Cbb_BTFN, pFNName);
            this.Rdb_Times4.Checked = true;
            this.Txt_Times4.Text = pFreeValue;
            CommFunc.SetComboBoxSelectedIndex(this.Cbb_Cycle, CommFunc.SplitString(pFreeValue, ",", -1).Count.ToString());
            this.Btn_BTCount_Click(null, null);
        }

        private void Txt_Input_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((((e.KeyChar != '\b') && !char.IsDigit(e.KeyChar)) && ((e.KeyChar != '-') && (e.KeyChar != ','))) && (e.KeyChar != 0xff0c))
            {
                e.Handled = true;
            }
        }
    }
}

