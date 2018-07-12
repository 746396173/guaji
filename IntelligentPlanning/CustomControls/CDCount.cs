namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class CDCount : UserControl
    {
        private bool _RunEvent = false;
        private Button Btn_CDCount;
        private ComboBox Cbb_PlayName;
        private ComboBox Cbb_PlayType;
        private ComboBox Cbb_Unit;
        private List<ConfigurationStatus.CDMoneyLimit> CDMoneyLimitList = null;
        private CheckBox Ckb_InputClear;
        private CheckBox Ckb_InputPaste;
        private CheckBox Ckb_TimesCopy;
        private CheckBox Ckb_WZ1;
        private CheckBox Ckb_WZ2;
        private CheckBox Ckb_WZ3;
        private CheckBox Ckb_WZ4;
        private CheckBox Ckb_WZ5;
        private IContainer components = null;
        private List<Control> ControlList = null;
        private Label Lbl_BetsTotalMoneyKey;
        private Label Lbl_BetsTotalMoneyValue;
        private Label Lbl_NumberCount;
        private Label Lbl_PlayName;
        private Label Lbl_PlayType;
        private Label Lbl_Random;
        private Label Lbl_Times;
        private Label Lbl_Unit;
        private NumericUpDown Nm_Times;
        private List<int> OutTimesList = new List<int>();
        private Panel Pnl_CDCount;
        private Panel Pnl_InputBottom;
        private Panel Pnl_Left;
        private Panel Pnl_LeftSetting;
        private Panel Pnl_Out;
        private Panel Pnl_OutBottom;
        private Panel Pnl_RightSetting;
        private Panel Pnl_RX;
        private string RegConfigPath = "";
        private List<CheckBox> RXWZList = null;
        private List<Control> SpecialControlList = null;
        private RichTextBox Txt_Input;
        private RichTextBox Txt_Out;

        public CDCount()
        {
            this.InitializeComponent();
        }

        private void Btn_CDCount_Click(object sender, EventArgs e)
        {
            string pErrorHint = "";
            List<string> pCodeList = CommFunc.FilterNumber(CommFunc.ReplaceText(this.Txt_Input.Text, this.PlayInfo, true), this.PlayInfo.CodeCount, this.PlayInfo.Play, ref pErrorHint);
            double money = this.GetMoney();
            int pNumber = CommFunc.GetBetsCodeCount(pCodeList, this.PlayInfo.Play, this.GetRXWZList());
            int num3 = Convert.ToInt32(this.Nm_Times.Value);
            double num4 = (money * pNumber) * num3;
            this.OutTimesList.Clear();
            int pTimes = num3;
            while (true)
            {
                int item = this.GetMinTimes(money, pNumber, pTimes);
                this.OutTimesList.Add(item);
                if (item == pTimes)
                {
                    string str3 = $"总注数：{pNumber} 注，倍数：{num3}，单注金额：{money}，总金额：{num4}，拆分倍数如下：";
                    List<string> pList = new List<string>();
                    for (int i = 0; i < this.OutTimesList.Count; i++)
                    {
                        int num8 = this.OutTimesList[i];
                        double num9 = (money * pNumber) * num8;
                        string str4 = $"{num8} 倍（{num9}）";
                        pList.Add(str4);
                    }
                    this.Txt_Out.Text = str3 + CommFunc.Join(pList, "\r\n");
                    return;
                }
                pTimes -= item;
            }
        }

        private void Cbb_PlayName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Ckb_WZ1.Visible = this.Ckb_WZ2.Visible = this.Ckb_WZ3.Visible = this.Ckb_WZ4.Visible = this.Ckb_WZ5.Visible = CommFunc.CheckPlayIsRXDS(this.Play);
            if (this._RunEvent)
            {
                this.RefreshNumCount();
            }
        }

        private void Cbb_PlayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> playNameList = CommFunc.GetPlayNameList(this.Cbb_PlayType.Text);
            CommFunc.SetComboBoxList(this.Cbb_PlayName, playNameList);
            this.Cbb_PlayName.SelectedIndex = 0;
        }

        private void Cbb_Unit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
                this.RefreshNumCount();
            }
        }

        private void CDCount_Load(object sender, EventArgs e)
        {
            List<CheckBox> pCheckBoxList = new List<CheckBox> {
                this.Ckb_InputPaste,
                this.Ckb_InputClear,
                this.Ckb_TimesCopy
            };
            CommFunc.SetCheckBoxFormatFlat(pCheckBoxList);
            List<Label> pLabelList = new List<Label> {
                this.Lbl_NumberCount,
                this.Lbl_BetsTotalMoneyValue
            };
            CommFunc.SetLabelFormat(pLabelList);
            List<CheckBox> list8 = new List<CheckBox> {
                this.Ckb_WZ1,
                this.Ckb_WZ2,
                this.Ckb_WZ3,
                this.Ckb_WZ4,
                this.Ckb_WZ5
            };
            this.RXWZList = list8;
            List<string> dicKeyList = CommFunc.GetDicKeyList<List<ConfigurationStatus.PlayBase>>(AppInfo.PlayDic);
            CommFunc.SetComboBoxList(this.Cbb_PlayType, dicKeyList);
            this.SetControlInfoByReg();
            this.RefreshNumCount();
            List<string> list4 = new List<string> { 
                "0-165",
                "165-270",
                "270-440",
                "440-710",
                "710-1150",
                "1150-1870",
                "1870-3020",
                "3020-4900",
                "4900-7900",
                "7900-12800",
                "12800-20800",
                "20800-33800",
                "33800-54800",
                "54800-89000",
                "89000-144000",
                "144000-233000",
                "233000-378000",
                "378000-613000",
                "613000-996000",
                "996000-1610000"
            };
            this.CDMoneyLimitList = new List<ConfigurationStatus.CDMoneyLimit>();
            foreach (string str in list4)
            {
                List<int> list5 = CommFunc.SplitInt(str, "-");
                ConfigurationStatus.CDMoneyLimit item = new ConfigurationStatus.CDMoneyLimit(list5[0], list5[1]);
                this.CDMoneyLimitList.Add(item);
            }
            this._RunEvent = true;
        }

        private void Ckb_InputClear_Click(object sender, EventArgs e)
        {
            this.Txt_Input.Text = "";
            this.Txt_Input.Focus();
        }

        private void Ckb_InputPaste_Click(object sender, EventArgs e)
        {
            CommFunc.PasteText(this.Txt_Input);
        }

        private void Ckb_TimesCopy_Click(object sender, EventArgs e)
        {
            string pText = CommFunc.Join(this.OutTimesList, ",");
            if (pText == "")
            {
                CommFunc.PublicMessageAll("没有倍数可以复制！", true, MessageBoxIcon.Asterisk, "");
            }
            else
            {
                CommFunc.CopyText(pText);
                CommFunc.PublicMessageAll("复制成功！", true, MessageBoxIcon.Asterisk, "");
            }
        }

        private void Ckb_WZ_CheckedChanged(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
                this.RefreshNumCount();
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

        private double GetBetsTotalMoney()
        {
            string pErrorHint = "";
            int num = CommFunc.GetBetsCodeCount(CommFunc.FilterNumber(CommFunc.ReplaceText(this.Txt_Input.Text, this.PlayInfo, true), this.PlayInfo.CodeCount, this.PlayInfo.Play, ref pErrorHint), this.PlayInfo.Play, this.GetRXWZList());
            return ((this.GetMoney() * num) * Convert.ToInt32(this.Nm_Times.Value));
        }

        private int GetMinTimes(double pMoney, int pNumber, int pTimes)
        {
            int num = -1;
            double num2 = (pMoney * pNumber) * pTimes;
            int min = 0;
            foreach (ConfigurationStatus.CDMoneyLimit limit in this.CDMoneyLimitList)
            {
                if ((num2 >= limit.Min) && (num2 < limit.Max))
                {
                    min = limit.Min;
                    break;
                }
            }
            if (min == 0)
            {
                return pTimes;
            }
            for (int i = pTimes - 1; i >= 1; i--)
            {
                num2 = (pMoney * pNumber) * i;
                if (num2 < min)
                {
                    num = i + 1;
                    break;
                }
            }
            if (num == -1)
            {
                num = 1;
            }
            return num;
        }

        private double GetMoney()
        {
            switch (this.Cbb_Unit.SelectedIndex)
            {
                case 0:
                    return 2.0;

                case 1:
                    return 0.2;

                case 2:
                    return 0.02;

                case 3:
                    return 0.002;

                case 4:
                    return 1.0;

                case 5:
                    return 0.1;

                case 6:
                    return 0.01;

                case 7:
                    return 0.001;
            }
            return 0.0;
        }

        private List<int> GetRXWZList()
        {
            if (!CommFunc.CheckPlayIsRXDS(this.Play))
            {
                return null;
            }
            List<int> list = new List<int>();
            for (int i = 0; i < this.RXWZList.Count; i++)
            {
                if (this.RXWZList[i].Checked)
                {
                    list.Add(AppInfo.FiveDic[this.RXWZList[i].Text]);
                }
            }
            return list;
        }

        private void InitializeComponent()
        {
            this.Pnl_CDCount = new Panel();
            this.Pnl_Out = new Panel();
            this.Txt_Out = new RichTextBox();
            this.Pnl_OutBottom = new Panel();
            this.Ckb_TimesCopy = new CheckBox();
            this.Pnl_Left = new Panel();
            this.Pnl_LeftSetting = new Panel();
            this.Txt_Input = new RichTextBox();
            this.Pnl_RightSetting = new Panel();
            this.Btn_CDCount = new Button();
            this.Nm_Times = new NumericUpDown();
            this.Lbl_Times = new Label();
            this.Pnl_RX = new Panel();
            this.Ckb_WZ5 = new CheckBox();
            this.Ckb_WZ4 = new CheckBox();
            this.Ckb_WZ3 = new CheckBox();
            this.Ckb_WZ2 = new CheckBox();
            this.Ckb_WZ1 = new CheckBox();
            this.Cbb_Unit = new ComboBox();
            this.Lbl_Unit = new Label();
            this.Cbb_PlayName = new ComboBox();
            this.Cbb_PlayType = new ComboBox();
            this.Lbl_PlayType = new Label();
            this.Lbl_PlayName = new Label();
            this.Pnl_InputBottom = new Panel();
            this.Lbl_NumberCount = new Label();
            this.Lbl_Random = new Label();
            this.Ckb_InputPaste = new CheckBox();
            this.Ckb_InputClear = new CheckBox();
            this.Lbl_BetsTotalMoneyValue = new Label();
            this.Lbl_BetsTotalMoneyKey = new Label();
            this.Pnl_CDCount.SuspendLayout();
            this.Pnl_Out.SuspendLayout();
            this.Pnl_OutBottom.SuspendLayout();
            this.Pnl_Left.SuspendLayout();
            this.Pnl_LeftSetting.SuspendLayout();
            this.Pnl_RightSetting.SuspendLayout();
            this.Nm_Times.BeginInit();
            this.Pnl_RX.SuspendLayout();
            this.Pnl_InputBottom.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_CDCount.Controls.Add(this.Pnl_Out);
            this.Pnl_CDCount.Controls.Add(this.Pnl_Left);
            this.Pnl_CDCount.Dock = DockStyle.Fill;
            this.Pnl_CDCount.Location = new Point(0, 0);
            this.Pnl_CDCount.Name = "Pnl_CDCount";
            this.Pnl_CDCount.Size = new Size(940, 0x201);
            this.Pnl_CDCount.TabIndex = 0x10;
            this.Pnl_Out.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Out.Controls.Add(this.Txt_Out);
            this.Pnl_Out.Controls.Add(this.Pnl_OutBottom);
            this.Pnl_Out.Dock = DockStyle.Fill;
            this.Pnl_Out.Location = new Point(0x209, 0);
            this.Pnl_Out.Name = "Pnl_Out";
            this.Pnl_Out.Size = new Size(0x1a3, 0x201);
            this.Pnl_Out.TabIndex = 0x11;
            this.Txt_Out.Dock = DockStyle.Fill;
            this.Txt_Out.Location = new Point(0, 0);
            this.Txt_Out.Name = "Txt_Out";
            this.Txt_Out.ReadOnly = true;
            this.Txt_Out.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.Txt_Out.Size = new Size(0x1a1, 0x1dc);
            this.Txt_Out.TabIndex = 0x141;
            this.Txt_Out.Text = "";
            this.Pnl_OutBottom.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_OutBottom.Controls.Add(this.Ckb_TimesCopy);
            this.Pnl_OutBottom.Dock = DockStyle.Bottom;
            this.Pnl_OutBottom.Location = new Point(0, 0x1dc);
            this.Pnl_OutBottom.Name = "Pnl_OutBottom";
            this.Pnl_OutBottom.Size = new Size(0x1a1, 0x23);
            this.Pnl_OutBottom.TabIndex = 0x4a;
            this.Ckb_TimesCopy.Appearance = Appearance.Button;
            this.Ckb_TimesCopy.AutoCheck = false;
            this.Ckb_TimesCopy.FlatAppearance.BorderSize = 0;
            this.Ckb_TimesCopy.FlatStyle = FlatStyle.Flat;
            this.Ckb_TimesCopy.Image = Resources.Copy;
            this.Ckb_TimesCopy.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_TimesCopy.Location = new Point(4, 5);
            this.Ckb_TimesCopy.Name = "Ckb_TimesCopy";
            this.Ckb_TimesCopy.Size = new Size(80, 0x19);
            this.Ckb_TimesCopy.TabIndex = 0xa8;
            this.Ckb_TimesCopy.Text = "复制倍数";
            this.Ckb_TimesCopy.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_TimesCopy.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_TimesCopy.UseVisualStyleBackColor = true;
            this.Ckb_TimesCopy.Click += new EventHandler(this.Ckb_TimesCopy_Click);
            this.Pnl_Left.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Left.Controls.Add(this.Pnl_LeftSetting);
            this.Pnl_Left.Controls.Add(this.Pnl_RightSetting);
            this.Pnl_Left.Controls.Add(this.Pnl_InputBottom);
            this.Pnl_Left.Dock = DockStyle.Left;
            this.Pnl_Left.Location = new Point(0, 0);
            this.Pnl_Left.Name = "Pnl_Left";
            this.Pnl_Left.Size = new Size(0x209, 0x201);
            this.Pnl_Left.TabIndex = 0x10;
            this.Pnl_LeftSetting.Controls.Add(this.Txt_Input);
            this.Pnl_LeftSetting.Dock = DockStyle.Fill;
            this.Pnl_LeftSetting.Location = new Point(0, 0);
            this.Pnl_LeftSetting.Name = "Pnl_LeftSetting";
            this.Pnl_LeftSetting.Size = new Size(0x13a, 0x1dc);
            this.Pnl_LeftSetting.TabIndex = 0x4d;
            this.Txt_Input.Dock = DockStyle.Fill;
            this.Txt_Input.Location = new Point(0, 0);
            this.Txt_Input.Name = "Txt_Input";
            this.Txt_Input.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.Txt_Input.Size = new Size(0x13a, 0x1dc);
            this.Txt_Input.TabIndex = 320;
            this.Txt_Input.Text = "";
            this.Txt_Input.TextChanged += new EventHandler(this.Txt_Input_TextChanged);
            this.Pnl_RightSetting.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_RightSetting.Controls.Add(this.Btn_CDCount);
            this.Pnl_RightSetting.Controls.Add(this.Nm_Times);
            this.Pnl_RightSetting.Controls.Add(this.Lbl_Times);
            this.Pnl_RightSetting.Controls.Add(this.Pnl_RX);
            this.Pnl_RightSetting.Controls.Add(this.Cbb_Unit);
            this.Pnl_RightSetting.Controls.Add(this.Lbl_Unit);
            this.Pnl_RightSetting.Controls.Add(this.Cbb_PlayName);
            this.Pnl_RightSetting.Controls.Add(this.Cbb_PlayType);
            this.Pnl_RightSetting.Controls.Add(this.Lbl_PlayType);
            this.Pnl_RightSetting.Controls.Add(this.Lbl_PlayName);
            this.Pnl_RightSetting.Dock = DockStyle.Right;
            this.Pnl_RightSetting.Location = new Point(0x13a, 0);
            this.Pnl_RightSetting.Name = "Pnl_RightSetting";
            this.Pnl_RightSetting.Size = new Size(0xcd, 0x1dc);
            this.Pnl_RightSetting.TabIndex = 0x4c;
            this.Btn_CDCount.Font = new Font("微软雅黑", 14f, FontStyle.Bold);
            this.Btn_CDCount.Location = new Point(6, 0xde);
            this.Btn_CDCount.Name = "Btn_CDCount";
            this.Btn_CDCount.Size = new Size(0xb8, 60);
            this.Btn_CDCount.TabIndex = 0x150;
            this.Btn_CDCount.Text = "计算倍数";
            this.Btn_CDCount.UseVisualStyleBackColor = true;
            this.Btn_CDCount.Click += new EventHandler(this.Btn_CDCount_Click);
            this.Nm_Times.Location = new Point(80, 0xb2);
            int[] bits = new int[4];
            bits[0] = 0x186a0;
            this.Nm_Times.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.Nm_Times.Minimum = new decimal(bits);
            this.Nm_Times.Name = "Nm_Times";
            this.Nm_Times.Size = new Size(110, 0x17);
            this.Nm_Times.TabIndex = 0x14f;
            bits = new int[4];
            bits[0] = 10;
            this.Nm_Times.Value = new decimal(bits);
            this.Nm_Times.ValueChanged += new EventHandler(this.Nm_Times_ValueChanged);
            this.Lbl_Times.AutoSize = true;
            this.Lbl_Times.ForeColor = SystemColors.ControlText;
            this.Lbl_Times.Location = new Point(6, 0xb5);
            this.Lbl_Times.Name = "Lbl_Times";
            this.Lbl_Times.Size = new Size(0x44, 0x11);
            this.Lbl_Times.TabIndex = 0x14e;
            this.Lbl_Times.Text = "投注倍数：";
            this.Pnl_RX.Controls.Add(this.Ckb_WZ5);
            this.Pnl_RX.Controls.Add(this.Ckb_WZ4);
            this.Pnl_RX.Controls.Add(this.Ckb_WZ3);
            this.Pnl_RX.Controls.Add(this.Ckb_WZ2);
            this.Pnl_RX.Controls.Add(this.Ckb_WZ1);
            this.Pnl_RX.Location = new Point(5, 0x72);
            this.Pnl_RX.Name = "Pnl_RX";
            this.Pnl_RX.Size = new Size(0xc3, 0x23);
            this.Pnl_RX.TabIndex = 0x14d;
            this.Ckb_WZ5.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.Ckb_WZ5.AutoSize = true;
            this.Ckb_WZ5.Location = new Point(0x99, 8);
            this.Ckb_WZ5.Name = "Ckb_WZ5";
            this.Ckb_WZ5.Size = new Size(0x27, 0x15);
            this.Ckb_WZ5.TabIndex = 0x14d;
            this.Ckb_WZ5.Text = "个";
            this.Ckb_WZ5.UseVisualStyleBackColor = true;
            this.Ckb_WZ5.Visible = false;
            this.Ckb_WZ5.CheckedChanged += new EventHandler(this.Ckb_WZ_CheckedChanged);
            this.Ckb_WZ4.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.Ckb_WZ4.AutoSize = true;
            this.Ckb_WZ4.Location = new Point(0x74, 8);
            this.Ckb_WZ4.Name = "Ckb_WZ4";
            this.Ckb_WZ4.Size = new Size(0x27, 0x15);
            this.Ckb_WZ4.TabIndex = 0x14c;
            this.Ckb_WZ4.Text = "十";
            this.Ckb_WZ4.UseVisualStyleBackColor = true;
            this.Ckb_WZ4.Visible = false;
            this.Ckb_WZ4.CheckedChanged += new EventHandler(this.Ckb_WZ_CheckedChanged);
            this.Ckb_WZ3.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.Ckb_WZ3.AutoSize = true;
            this.Ckb_WZ3.Location = new Point(0x4f, 8);
            this.Ckb_WZ3.Name = "Ckb_WZ3";
            this.Ckb_WZ3.Size = new Size(0x27, 0x15);
            this.Ckb_WZ3.TabIndex = 0x14b;
            this.Ckb_WZ3.Text = "百";
            this.Ckb_WZ3.UseVisualStyleBackColor = true;
            this.Ckb_WZ3.Visible = false;
            this.Ckb_WZ3.CheckedChanged += new EventHandler(this.Ckb_WZ_CheckedChanged);
            this.Ckb_WZ2.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.Ckb_WZ2.AutoSize = true;
            this.Ckb_WZ2.Location = new Point(0x2a, 8);
            this.Ckb_WZ2.Name = "Ckb_WZ2";
            this.Ckb_WZ2.Size = new Size(0x27, 0x15);
            this.Ckb_WZ2.TabIndex = 330;
            this.Ckb_WZ2.Text = "千";
            this.Ckb_WZ2.UseVisualStyleBackColor = true;
            this.Ckb_WZ2.Visible = false;
            this.Ckb_WZ2.CheckedChanged += new EventHandler(this.Ckb_WZ_CheckedChanged);
            this.Ckb_WZ1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.Ckb_WZ1.AutoSize = true;
            this.Ckb_WZ1.Location = new Point(5, 8);
            this.Ckb_WZ1.Name = "Ckb_WZ1";
            this.Ckb_WZ1.Size = new Size(0x27, 0x15);
            this.Ckb_WZ1.TabIndex = 0x149;
            this.Ckb_WZ1.Text = "万";
            this.Ckb_WZ1.UseVisualStyleBackColor = true;
            this.Ckb_WZ1.Visible = false;
            this.Ckb_WZ1.CheckedChanged += new EventHandler(this.Ckb_WZ_CheckedChanged);
            this.Cbb_Unit.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_Unit.FormattingEnabled = true;
            this.Cbb_Unit.Items.AddRange(new object[] { "2圆", "2角", "2分", "2厘", "1圆", "1角", "1分", "1厘" });
            this.Cbb_Unit.Location = new Point(80, 0x4f);
            this.Cbb_Unit.Name = "Cbb_Unit";
            this.Cbb_Unit.Size = new Size(110, 0x19);
            this.Cbb_Unit.TabIndex = 0x139;
            this.Cbb_Unit.SelectedIndexChanged += new EventHandler(this.Cbb_Unit_SelectedIndexChanged);
            this.Lbl_Unit.AutoSize = true;
            this.Lbl_Unit.Location = new Point(6, 0x52);
            this.Lbl_Unit.Name = "Lbl_Unit";
            this.Lbl_Unit.Size = new Size(0x44, 0x11);
            this.Lbl_Unit.TabIndex = 0x138;
            this.Lbl_Unit.Text = "金额模式：";
            this.Cbb_PlayName.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_PlayName.FormattingEnabled = true;
            this.Cbb_PlayName.Location = new Point(80, 0x2c);
            this.Cbb_PlayName.Name = "Cbb_PlayName";
            this.Cbb_PlayName.Size = new Size(110, 0x19);
            this.Cbb_PlayName.TabIndex = 0x137;
            this.Cbb_PlayName.SelectedIndexChanged += new EventHandler(this.Cbb_PlayName_SelectedIndexChanged);
            this.Cbb_PlayType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_PlayType.FormattingEnabled = true;
            this.Cbb_PlayType.Location = new Point(80, 9);
            this.Cbb_PlayType.Name = "Cbb_PlayType";
            this.Cbb_PlayType.Size = new Size(110, 0x19);
            this.Cbb_PlayType.TabIndex = 310;
            this.Cbb_PlayType.SelectedIndexChanged += new EventHandler(this.Cbb_PlayType_SelectedIndexChanged);
            this.Lbl_PlayType.AutoSize = true;
            this.Lbl_PlayType.Location = new Point(6, 12);
            this.Lbl_PlayType.Name = "Lbl_PlayType";
            this.Lbl_PlayType.Size = new Size(0x44, 0x11);
            this.Lbl_PlayType.TabIndex = 0x135;
            this.Lbl_PlayType.Text = "玩法类型：";
            this.Lbl_PlayName.AutoSize = true;
            this.Lbl_PlayName.Location = new Point(6, 0x2f);
            this.Lbl_PlayName.Name = "Lbl_PlayName";
            this.Lbl_PlayName.Size = new Size(0x44, 0x11);
            this.Lbl_PlayName.TabIndex = 0x134;
            this.Lbl_PlayName.Text = "玩法名称：";
            this.Pnl_InputBottom.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_InputBottom.Controls.Add(this.Lbl_BetsTotalMoneyValue);
            this.Pnl_InputBottom.Controls.Add(this.Lbl_BetsTotalMoneyKey);
            this.Pnl_InputBottom.Controls.Add(this.Lbl_NumberCount);
            this.Pnl_InputBottom.Controls.Add(this.Lbl_Random);
            this.Pnl_InputBottom.Controls.Add(this.Ckb_InputPaste);
            this.Pnl_InputBottom.Controls.Add(this.Ckb_InputClear);
            this.Pnl_InputBottom.Dock = DockStyle.Bottom;
            this.Pnl_InputBottom.Location = new Point(0, 0x1dc);
            this.Pnl_InputBottom.Name = "Pnl_InputBottom";
            this.Pnl_InputBottom.Size = new Size(0x207, 0x23);
            this.Pnl_InputBottom.TabIndex = 0x4a;
            this.Lbl_NumberCount.AutoSize = true;
            this.Lbl_NumberCount.Location = new Point(0xc5, 9);
            this.Lbl_NumberCount.Name = "Lbl_NumberCount";
            this.Lbl_NumberCount.Size = new Size(0x16, 0x11);
            this.Lbl_NumberCount.TabIndex = 0x142;
            this.Lbl_NumberCount.Text = "00";
            this.Lbl_Random.AutoSize = true;
            this.Lbl_Random.Location = new Point(0x7e, 9);
            this.Lbl_Random.Name = "Lbl_Random";
            this.Lbl_Random.Size = new Size(0x44, 0x11);
            this.Lbl_Random.TabIndex = 0x141;
            this.Lbl_Random.Text = "号码注数：";
            this.Ckb_InputPaste.Appearance = Appearance.Button;
            this.Ckb_InputPaste.AutoCheck = false;
            this.Ckb_InputPaste.FlatAppearance.BorderSize = 0;
            this.Ckb_InputPaste.FlatStyle = FlatStyle.Flat;
            this.Ckb_InputPaste.Image = Resources.Paste;
            this.Ckb_InputPaste.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_InputPaste.Location = new Point(4, 5);
            this.Ckb_InputPaste.Name = "Ckb_InputPaste";
            this.Ckb_InputPaste.Size = new Size(60, 0x19);
            this.Ckb_InputPaste.TabIndex = 0x138;
            this.Ckb_InputPaste.Text = "粘贴";
            this.Ckb_InputPaste.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_InputPaste.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_InputPaste.UseVisualStyleBackColor = true;
            this.Ckb_InputPaste.Click += new EventHandler(this.Ckb_InputPaste_Click);
            this.Ckb_InputClear.Appearance = Appearance.Button;
            this.Ckb_InputClear.AutoCheck = false;
            this.Ckb_InputClear.FlatAppearance.BorderSize = 0;
            this.Ckb_InputClear.FlatStyle = FlatStyle.Flat;
            this.Ckb_InputClear.Image = Resources.ClearAll;
            this.Ckb_InputClear.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_InputClear.Location = new Point(60, 5);
            this.Ckb_InputClear.Name = "Ckb_InputClear";
            this.Ckb_InputClear.Size = new Size(60, 0x19);
            this.Ckb_InputClear.TabIndex = 0x137;
            this.Ckb_InputClear.Text = "清空";
            this.Ckb_InputClear.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_InputClear.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_InputClear.UseVisualStyleBackColor = true;
            this.Ckb_InputClear.Click += new EventHandler(this.Ckb_InputClear_Click);
            this.Lbl_BetsTotalMoneyValue.AutoSize = true;
            this.Lbl_BetsTotalMoneyValue.Location = new Point(0x163, 9);
            this.Lbl_BetsTotalMoneyValue.Name = "Lbl_BetsTotalMoneyValue";
            this.Lbl_BetsTotalMoneyValue.Size = new Size(0x16, 0x11);
            this.Lbl_BetsTotalMoneyValue.TabIndex = 0x144;
            this.Lbl_BetsTotalMoneyValue.Text = "00";
            this.Lbl_BetsTotalMoneyKey.AutoSize = true;
            this.Lbl_BetsTotalMoneyKey.Location = new Point(0x11c, 9);
            this.Lbl_BetsTotalMoneyKey.Name = "Lbl_BetsTotalMoneyKey";
            this.Lbl_BetsTotalMoneyKey.Size = new Size(0x44, 0x11);
            this.Lbl_BetsTotalMoneyKey.TabIndex = 0x143;
            this.Lbl_BetsTotalMoneyKey.Text = "投注金额：";
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.Pnl_CDCount);
            this.Font = new Font("微软雅黑", 9f);
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "CDCount";
            base.Size = new Size(940, 0x201);
            base.Load += new EventHandler(this.CDCount_Load);
            this.Pnl_CDCount.ResumeLayout(false);
            this.Pnl_Out.ResumeLayout(false);
            this.Pnl_OutBottom.ResumeLayout(false);
            this.Pnl_Left.ResumeLayout(false);
            this.Pnl_LeftSetting.ResumeLayout(false);
            this.Pnl_RightSetting.ResumeLayout(false);
            this.Pnl_RightSetting.PerformLayout();
            this.Nm_Times.EndInit();
            this.Pnl_RX.ResumeLayout(false);
            this.Pnl_RX.PerformLayout();
            this.Pnl_InputBottom.ResumeLayout(false);
            this.Pnl_InputBottom.PerformLayout();
            base.ResumeLayout(false);
        }

        private void Nm_Times_ValueChanged(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
                this.RefreshNumCount();
            }
        }

        private void RefreshNumCount()
        {
            string pErrorHint = "";
            List<string> pCodeList = CommFunc.FilterNumber(CommFunc.ReplaceText(this.Txt_Input.Text, this.PlayInfo, true), this.PlayInfo.CodeCount, this.PlayInfo.Play, ref pErrorHint);
            this.Lbl_NumberCount.Text = $"{CommFunc.GetBetsCodeCount(pCodeList, this.PlayInfo.Play, this.GetRXWZList())} 注";
            this.Lbl_BetsTotalMoneyValue.Text = this.GetBetsTotalMoney().ToString();
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
                this.Txt_Input,
                this.Nm_Times,
                this.Ckb_WZ1,
                this.Ckb_WZ2,
                this.Ckb_WZ3,
                this.Ckb_WZ4,
                this.Ckb_WZ5
            };
            this.ControlList = list;
            List<Control> list2 = new List<Control> {
                this.Cbb_PlayType,
                this.Cbb_PlayName,
                this.Cbb_Unit
            };
            this.SpecialControlList = list2;
            CommFunc.SetFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SetSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
        }

        private void Txt_Input_TextChanged(object sender, EventArgs e)
        {
            if (this._RunEvent)
            {
                this.RefreshNumCount();
            }
        }

        public string Play
        {
            get
            {
                string text = this.Cbb_PlayType.Text;
                string str2 = this.Cbb_PlayName.Text;
                return (text + str2);
            }
        }

        public ConfigurationStatus.PlayBase PlayInfo
        {
            get
            {
                string text = this.Cbb_PlayType.Text;
                string playName = this.Cbb_PlayName.Text;
                return CommFunc.GetPlayInfo(text, playName);
            }
        }
    }
}

