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

    public class TBCount : UserControl
    {
        private Button Btn_TBCount;
        private ComboBox Cbb_Cycle;
        private ComboBox Cbb_TBCycle;
        private ComboBox Cbb_Times1;
        private ComboBox Cbb_Unit;
        private CheckBox Ckb_TBCopy;
        private IContainer components = null;
        private List<Control> ControlList = null;
        private ExpandGirdView Egv_TimesList;
        private Label Lbl_Cycle;
        private Label Lbl_Money;
        private Label Lbl_Number;
        private Label Lbl_Prize;
        private Label Lbl_SingleValue;
        private Label Lbl_Start;
        private Label Lbl_TBCycle;
        private Label Lbl_Times;
        private Label Lbl_Times1;
        private Label Lbl_Times3_1;
        private Label Lbl_Times3_2;
        private Panel Pnl_BTBottom;
        private Panel Pnl_BTCount;
        private Panel Pnl_BTRight;
        private Panel Pnl_TBLeft;
        private Panel Pnl_TBTop;
        private RadioButton Rdb_Times1;
        private RadioButton Rdb_Times2;
        private RadioButton Rdb_Times3;
        private string RegConfigPath = "";
        private List<Control> SpecialControlList = null;
        private List<ConfigurationStatus.TimesData> TimesList = new List<ConfigurationStatus.TimesData>();
        private TextBox Txt_Number;
        private TextBox Txt_Prize;
        private TextBox Txt_Start;
        private TextBox Txt_Times2;
        private TextBox Txt_Times3_1;
        private TextBox Txt_Times3_2;

        public TBCount()
        {
            this.InitializeComponent();
        }

        private void BeautifyInterface()
        {
            if (AppInfo.Account.Configuration.Beautify)
            {
                List<Control> pControlList = new List<Control> {
                    this.Pnl_TBTop,
                    this.Pnl_BTBottom
                };
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list2 = new List<Control> {
                    this.Btn_TBCount
                };
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Rdb_Times2,
                    this.Rdb_Times1,
                    this.Rdb_Times3,
                    this.Lbl_Times1,
                    this.Lbl_TBCycle,
                    this.Lbl_Times3_1,
                    this.Lbl_Times3_2,
                    this.Lbl_Start,
                    this.Lbl_Money,
                    this.Lbl_SingleValue,
                    this.Lbl_Prize,
                    this.Lbl_Cycle,
                    this.Lbl_Number,
                    this.Ckb_TBCopy,
                    this.Btn_TBCount
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
                List<ComboBox> pComboBoxList = new List<ComboBox> {
                    this.Cbb_Times1,
                    this.Cbb_Cycle,
                    this.Cbb_Unit
                };
                CommFunc.BeautifyComboBox(pComboBoxList);
            }
        }

        private void Btn_TBCount_Click(object sender, EventArgs e)
        {
            ConfigurationStatus.TimesCount pTimesInfo = null;
            if (this.GetTimesCount(ref pTimesInfo))
            {
                this.CountTimesList(pTimesInfo);
                this.RefreshBTDataList();
            }
        }

        private void Ckb_TBCopy_Click(object sender, EventArgs e)
        {
            string bTString = this.GetBTString();
            CommFunc.CopyText(bTString);
            if (bTString == "")
            {
                CommFunc.PublicMessageAll("没有推波可以复制！", true, MessageBoxIcon.Asterisk, "");
            }
            else
            {
                CommFunc.CopyText(bTString);
                CommFunc.PublicMessageAll("复制成功！", true, MessageBoxIcon.Asterisk, "");
            }
        }

        private double CountSingeTimes(ConfigurationStatus.TimesCount pTimesInfo, ref List<ConfigurationStatus.TimesData> pTimesList, int pIndex)
        {
            double pNum = -1.0;
            double getTBGain = -1.0;
            if ((pTimesInfo.Type == ConfigurationStatus.TimesType.GainFix) || (pTimesInfo.Type == ConfigurationStatus.TimesType.Sum))
            {
                getTBGain = pTimesInfo.GetTBGain;
            }
            else if (pTimesInfo.Type == ConfigurationStatus.TimesType.GainRatio)
            {
                getTBGain = pTimesInfo.GetTBGain / 100.0;
            }
            double x = pTimesInfo.Money * pTimesInfo.Number;
            double num4 = (getTBGain + pTimesInfo.TotalOutput) * Math.Pow(x, (double) (pTimesInfo.TBCycle - 1));
            double num5 = Math.Pow(pTimesInfo.Mode, (double) pTimesInfo.TBCycle) - Math.Pow(x, (double) pTimesInfo.TBCycle);
            if (pTimesInfo.Type == ConfigurationStatus.TimesType.GainRatio)
            {
                num4 = ((getTBGain + 1.0) * pTimesInfo.TotalOutput) * Math.Pow(x, (double) (pTimesInfo.TBCycle - 1));
                num5 = (Math.Pow(pTimesInfo.Mode, (double) pTimesInfo.TBCycle) - Math.Pow(x, (double) pTimesInfo.TBCycle)) - Math.Pow(x, (double) pTimesInfo.TBCycle);
            }
            pNum = num4 / num5;
            pNum = CommFunc.ChinaRound(pNum, 2);
            if (pNum < 0.0)
            {
                return -1.0;
            }
            if (pNum > 10000000.0)
            {
                return -1.0;
            }
            if (pNum < pTimesInfo.Start)
            {
                pNum = pTimesInfo.Start;
            }
            while (!this.CountSingeTimes(pTimesInfo, ref pTimesList, pIndex, pNum))
            {
                pNum += 0.01;
                pNum = CommFunc.ChinaRound(pNum, 2);
                for (int i = 0; i < pTimesInfo.TBCycle; i++)
                {
                    pTimesList.RemoveAt(pTimesList.Count - 1);
                }
            }
            ConfigurationStatus.TimesData data = pTimesList[pTimesList.Count - 1];
            pTimesInfo.TotalOutput = data.TotalOutput;
            if (pTimesInfo.Type == ConfigurationStatus.TimesType.Sum)
            {
                pTimesInfo.SumBegin = data.Gain;
            }
            return pNum;
        }

        private bool CountSingeTimes(ConfigurationStatus.TimesCount pTimesInfo, ref List<ConfigurationStatus.TimesData> pTimesList, int pIndex, double pTimes)
        {
            ConfigurationStatus.TimesData item = new ConfigurationStatus.TimesData {
                Expect = pIndex.ToString(),
                Output = CommFunc.ChinaRound((pTimesInfo.Money * pTimesInfo.Number) * pTimes, 2)
            };
            item.TotalOutput = pTimesInfo.TotalOutput + item.Output;
            item.Times = pTimes;
            item.Input = CommFunc.ChinaRound(pTimesInfo.Mode * pTimes, 2);
            item.Gain = CommFunc.ChinaRound(item.Input - item.TotalOutput, 2);
            item.GainRatio = CommFunc.ChinaRound((item.Gain / item.TotalOutput) * 100.0, 2);
            item.TBCycle = pTimesInfo.TBCycle;
            pTimesList.Add(item);
            for (int i = 2; i <= item.TBCycle; i++)
            {
                this.CountTBList(pTimesInfo, i, ref pTimesList);
            }
            ConfigurationStatus.TimesData data2 = pTimesList[pTimesList.Count - 1];
            double getTBGain = -1.0;
            if ((pTimesInfo.Type == ConfigurationStatus.TimesType.GainFix) || (pTimesInfo.Type == ConfigurationStatus.TimesType.Sum))
            {
                getTBGain = pTimesInfo.GetTBGain;
                return (data2.Gain >= getTBGain);
            }
            if (pTimesInfo.Type == ConfigurationStatus.TimesType.GainRatio)
            {
                getTBGain = pTimesInfo.GetTBGain;
                return (data2.GainRatio >= getTBGain);
            }
            return false;
        }

        private int CountSingeTimes1(ConfigurationStatus.TimesCount pTimesInfo, int pIndex, ConfigurationStatus.TimesData pTimes = null)
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

        private void CountTBList(ConfigurationStatus.TimesCount pTimesInfo, int pIndex, ref List<ConfigurationStatus.TimesData> pTimesList)
        {
            ConfigurationStatus.TimesData item = new ConfigurationStatus.TimesData {
                Expect = "",
                Output = pTimesList[pTimesList.Count - 1].Input
            };
            double num = CommFunc.ChinaRound(item.Output / (pTimesInfo.Money * pTimesInfo.Number), 2);
            item.TotalOutput = pTimesList[pTimesList.Count - 1].TotalOutput;
            item.Times = num;
            item.Input = CommFunc.ChinaRound(pTimesInfo.Mode * num, 2);
            item.Gain = CommFunc.ChinaRound(item.Input - item.TotalOutput, 2);
            item.GainRatio = CommFunc.ChinaRound((item.Gain / item.TotalOutput) * 100.0, 2);
            item.TBCycle = pTimesInfo.TBCycle;
            pTimesList.Add(item);
        }

        private void CountTimesList(ConfigurationStatus.TimesCount pTimeInfo)
        {
            this.TimesList.Clear();
            for (int i = 1; i <= pTimeInfo.Cycle; i++)
            {
                if (this.CountSingeTimes(pTimeInfo, ref this.TimesList, i) == -1.0)
                {
                    CommFunc.PublicMessageAll("该计划不适合投注！", true, MessageBoxIcon.Asterisk, "");
                    break;
                }
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
                        expect = data.Output.ToString();
                    }
                    else if (e.ColumnIndex == 3)
                    {
                        expect = data.TotalOutput.ToString();
                    }
                    else if (e.ColumnIndex == 4)
                    {
                        expect = data.Input.ToString();
                    }
                    else if (e.ColumnIndex == 5)
                    {
                        expect = data.Gain.ToString();
                    }
                    else if (e.ColumnIndex == 6)
                    {
                        expect = data.GainRatio + "%";
                    }
                    int num = e.RowIndex + 1;
                    cell.Style.BackColor = ((num % data.TBCycle) == 0) ? AppInfo.appBackColor : AppInfo.beaBackColor;
                    cell.Style.ForeColor = ((num % data.TBCycle) == 0) ? AppInfo.whiteColor : AppInfo.beaForeColor;
                    e.Value = expect;
                }
            }
            catch
            {
            }
        }

        private double GetBetMoney()
        {
            double num = 0.0;
            switch (this.Cbb_Unit.Text)
            {
                case "元":
                    return 2.0;

                case "角":
                    return 0.2;

                case "分":
                    return 0.02;

                case "厘":
                    num = 0.002;
                    break;
            }
            return num;
        }

        private string GetBTString() => 
            CommFunc.Join(this.GetTBList(), ",");

        private int GetNumber() => 
            Convert.ToInt32(this.Txt_Number.Text);

        private List<string> GetTBList()
        {
            List<string> list = new List<string>();
            foreach (ConfigurationStatus.TimesData data in this.TimesList)
            {
                list.Add(data.Times.ToString());
            }
            return list;
        }

        private bool GetTimesCount(ref ConfigurationStatus.TimesCount pTimesInfo)
        {
            pTimesInfo = new ConfigurationStatus.TimesCount();
            int num = Convert.ToInt32(this.Cbb_Cycle.Text);
            pTimesInfo.TBCycle = Convert.ToInt32(this.Cbb_TBCycle.Text);
            int num2 = num / pTimesInfo.TBCycle;
            if ((num % pTimesInfo.TBCycle) != 0)
            {
                num2++;
            }
            pTimesInfo.Cycle = num2;
            pTimesInfo.Start = Convert.ToDouble(this.Txt_Start.Text);
            pTimesInfo.Money = this.GetBetMoney();
            if (CommFunc.CheckTextBoxIsNull(this.Txt_Prize, this.Lbl_Prize.Text))
            {
                return false;
            }
            int selectedIndex = this.Cbb_Unit.SelectedIndex;
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
            return true;
        }

        private RadioButton GetTimesType()
        {
            List<RadioButton> list = new List<RadioButton> {
                this.Rdb_Times1,
                this.Rdb_Times2,
                this.Rdb_Times3
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
            this.Ckb_TBCopy = new CheckBox();
            this.Pnl_TBTop = new Panel();
            this.Pnl_BTRight = new Panel();
            this.Btn_TBCount = new Button();
            this.Pnl_TBLeft = new Panel();
            this.Cbb_TBCycle = new ComboBox();
            this.Lbl_TBCycle = new Label();
            this.Txt_Start = new TextBox();
            this.Lbl_Start = new Label();
            this.Cbb_Cycle = new ComboBox();
            this.Lbl_Cycle = new Label();
            this.Txt_Number = new TextBox();
            this.Lbl_Number = new Label();
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
            this.Pnl_TBTop.SuspendLayout();
            this.Pnl_BTRight.SuspendLayout();
            this.Pnl_TBLeft.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_BTCount.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_BTCount.Controls.Add(this.Egv_TimesList);
            this.Pnl_BTCount.Controls.Add(this.Pnl_BTBottom);
            this.Pnl_BTCount.Controls.Add(this.Pnl_TBTop);
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
            this.Pnl_BTBottom.Controls.Add(this.Ckb_TBCopy);
            this.Pnl_BTBottom.Dock = DockStyle.Bottom;
            this.Pnl_BTBottom.Location = new Point(0, 0x1cf);
            this.Pnl_BTBottom.Name = "Pnl_BTBottom";
            this.Pnl_BTBottom.Size = new Size(0x353, 0x23);
            this.Pnl_BTBottom.TabIndex = 0x49;
            this.Ckb_TBCopy.Appearance = Appearance.Button;
            this.Ckb_TBCopy.AutoCheck = false;
            this.Ckb_TBCopy.FlatAppearance.BorderSize = 0;
            this.Ckb_TBCopy.FlatStyle = FlatStyle.Flat;
            this.Ckb_TBCopy.Image = Resources.Copy;
            this.Ckb_TBCopy.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_TBCopy.Location = new Point(4, 5);
            this.Ckb_TBCopy.Name = "Ckb_TBCopy";
            this.Ckb_TBCopy.Size = new Size(80, 0x19);
            this.Ckb_TBCopy.TabIndex = 0xa8;
            this.Ckb_TBCopy.Text = "复制倍投";
            this.Ckb_TBCopy.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_TBCopy.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_TBCopy.UseVisualStyleBackColor = true;
            this.Ckb_TBCopy.Click += new EventHandler(this.Ckb_TBCopy_Click);
            this.Pnl_TBTop.Controls.Add(this.Pnl_BTRight);
            this.Pnl_TBTop.Controls.Add(this.Pnl_TBLeft);
            this.Pnl_TBTop.Controls.Add(this.Lbl_Times);
            this.Pnl_TBTop.Dock = DockStyle.Top;
            this.Pnl_TBTop.Location = new Point(0, 0);
            this.Pnl_TBTop.Name = "Pnl_TBTop";
            this.Pnl_TBTop.Size = new Size(0x353, 0x7e);
            this.Pnl_TBTop.TabIndex = 0;
            this.Pnl_BTRight.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_BTRight.Controls.Add(this.Btn_TBCount);
            this.Pnl_BTRight.Dock = DockStyle.Left;
            this.Pnl_BTRight.Location = new Point(490, 0);
            this.Pnl_BTRight.Name = "Pnl_BTRight";
            this.Pnl_BTRight.Size = new Size(0x75, 0x7e);
            this.Pnl_BTRight.TabIndex = 0xaf;
            this.Btn_TBCount.Dock = DockStyle.Fill;
            this.Btn_TBCount.Font = new Font("微软雅黑", 14f, FontStyle.Bold);
            this.Btn_TBCount.Location = new Point(0, 0);
            this.Btn_TBCount.Name = "Btn_TBCount";
            this.Btn_TBCount.Size = new Size(0x73, 0x7c);
            this.Btn_TBCount.TabIndex = 0xaf;
            this.Btn_TBCount.Text = "计算推波";
            this.Btn_TBCount.UseVisualStyleBackColor = true;
            this.Btn_TBCount.Click += new EventHandler(this.Btn_TBCount_Click);
            this.Pnl_TBLeft.Controls.Add(this.Cbb_TBCycle);
            this.Pnl_TBLeft.Controls.Add(this.Lbl_TBCycle);
            this.Pnl_TBLeft.Controls.Add(this.Txt_Start);
            this.Pnl_TBLeft.Controls.Add(this.Lbl_Start);
            this.Pnl_TBLeft.Controls.Add(this.Cbb_Cycle);
            this.Pnl_TBLeft.Controls.Add(this.Lbl_Cycle);
            this.Pnl_TBLeft.Controls.Add(this.Txt_Number);
            this.Pnl_TBLeft.Controls.Add(this.Lbl_Number);
            this.Pnl_TBLeft.Controls.Add(this.Lbl_Times3_2);
            this.Pnl_TBLeft.Controls.Add(this.Txt_Times3_2);
            this.Pnl_TBLeft.Controls.Add(this.Lbl_Times3_1);
            this.Pnl_TBLeft.Controls.Add(this.Txt_Times3_1);
            this.Pnl_TBLeft.Controls.Add(this.Rdb_Times3);
            this.Pnl_TBLeft.Controls.Add(this.Txt_Times2);
            this.Pnl_TBLeft.Controls.Add(this.Rdb_Times2);
            this.Pnl_TBLeft.Controls.Add(this.Lbl_Times1);
            this.Pnl_TBLeft.Controls.Add(this.Cbb_Times1);
            this.Pnl_TBLeft.Controls.Add(this.Rdb_Times1);
            this.Pnl_TBLeft.Controls.Add(this.Lbl_SingleValue);
            this.Pnl_TBLeft.Controls.Add(this.Txt_Prize);
            this.Pnl_TBLeft.Controls.Add(this.Cbb_Unit);
            this.Pnl_TBLeft.Controls.Add(this.Lbl_Prize);
            this.Pnl_TBLeft.Controls.Add(this.Lbl_Money);
            this.Pnl_TBLeft.Dock = DockStyle.Left;
            this.Pnl_TBLeft.Location = new Point(0, 0);
            this.Pnl_TBLeft.Name = "Pnl_TBLeft";
            this.Pnl_TBLeft.Size = new Size(490, 0x7e);
            this.Pnl_TBLeft.TabIndex = 0xae;
            this.Cbb_TBCycle.ForeColor = SystemColors.ControlText;
            this.Cbb_TBCycle.FormattingEnabled = true;
            this.Cbb_TBCycle.Location = new Point(0x4e, 0x5f);
            this.Cbb_TBCycle.Name = "Cbb_TBCycle";
            this.Cbb_TBCycle.Size = new Size(60, 0x19);
            this.Cbb_TBCycle.TabIndex = 0xbc;
            this.Lbl_TBCycle.AutoSize = true;
            this.Lbl_TBCycle.ForeColor = SystemColors.ControlText;
            this.Lbl_TBCycle.Location = new Point(3, 100);
            this.Lbl_TBCycle.Name = "Lbl_TBCycle";
            this.Lbl_TBCycle.Size = new Size(0x44, 0x11);
            this.Lbl_TBCycle.TabIndex = 0xbb;
            this.Lbl_TBCycle.Text = "推波周期：";
            this.Txt_Start.ForeColor = SystemColors.ControlText;
            this.Txt_Start.Location = new Point(0xf2, 0x61);
            this.Txt_Start.Name = "Txt_Start";
            this.Txt_Start.Size = new Size(60, 0x17);
            this.Txt_Start.TabIndex = 0xba;
            this.Txt_Start.Text = "0.01";
            this.Txt_Start.KeyPress += new KeyPressEventHandler(this.Txt_Input_KeyPress);
            this.Lbl_Start.AutoSize = true;
            this.Lbl_Start.ForeColor = SystemColors.ControlText;
            this.Lbl_Start.Location = new Point(0xa7, 100);
            this.Lbl_Start.Name = "Lbl_Start";
            this.Lbl_Start.Size = new Size(0x44, 0x11);
            this.Lbl_Start.TabIndex = 0xb9;
            this.Lbl_Start.Text = "开始倍数：";
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
            base.Name = "TBCount";
            base.Size = new Size(0x355, 500);
            base.Load += new EventHandler(this.TBCount_Load);
            this.Pnl_BTCount.ResumeLayout(false);
            ((ISupportInitialize) this.Egv_TimesList).EndInit();
            this.Pnl_BTBottom.ResumeLayout(false);
            this.Pnl_TBTop.ResumeLayout(false);
            this.Pnl_TBTop.PerformLayout();
            this.Pnl_BTRight.ResumeLayout(false);
            this.Pnl_TBLeft.ResumeLayout(false);
            this.Pnl_TBLeft.PerformLayout();
            base.ResumeLayout(false);
        }

        public void LoadTBControl()
        {
            CommFunc.FillComboBoxItem(this.Cbb_Cycle, 1, 50, 1);
            CommFunc.FillComboBoxItem(this.Cbb_TBCycle, 1, 50, 1);
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
                "波数",
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
        }

        private void RefreshBTDataList()
        {
            CommFunc.RefreshDataGridView(this.Egv_TimesList, this.TimesList.Count);
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
                this.Txt_Start,
                this.Txt_Times2,
                this.Txt_Times3_1,
                this.Txt_Times3_2,
                this.Txt_Number
            };
            this.ControlList = list;
            List<Control> list2 = new List<Control> {
                this.Cbb_Cycle,
                this.Cbb_Unit,
                this.Cbb_Times1,
                this.Cbb_TBCycle
            };
            this.SpecialControlList = list2;
            CommFunc.SetFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SetSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
        }

        private void TBCount_Load(object sender, EventArgs e)
        {
            List<CheckBox> pCheckBoxList = new List<CheckBox> {
                this.Ckb_TBCopy
            };
            CommFunc.SetCheckBoxFormatFlat(pCheckBoxList);
            this.BeautifyInterface();
            this.LoadTimesList();
            this.LoadTBControl();
            this.SetControlInfoByReg();
        }

        private void Txt_Input_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((((e.KeyChar != '\b') && !char.IsDigit(e.KeyChar)) && (e.KeyChar != '.')) && (e.KeyChar != ','))
            {
                e.Handled = true;
            }
            else
            {
                TextBox box = sender as TextBox;
                if ((box.Text == "") && (e.KeyChar == '.'))
                {
                    e.Handled = true;
                }
                if (box.Text.Contains(".") && (e.KeyChar == '.'))
                {
                    e.Handled = true;
                }
                if ((box.Text == "0") && (e.KeyChar == '0'))
                {
                    e.Handled = true;
                }
            }
        }
    }
}

