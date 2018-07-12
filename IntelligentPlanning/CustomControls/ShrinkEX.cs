namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ShrinkEX : UserControl
    {
        private CheckBox Ckb_Copy;
        private CheckBox Ckb_Reset;
        private CheckBox Ckb_Shrink;
        private IContainer components = null;
        private List<Control> ControlList = null;
        private GroupBox Grp_NumList;
        private List<string> NumberList = new List<string>();
        private Panel Pnl_Main;
        private Panel Pnl_Num;
        private Panel Pnl_ReturnBottom;
        private Panel Pnl_Shrink;
        private Panel Pnl_Shrink1;
        private Panel Pnl_Shrink2;
        private Panel Pnl_Shrink3;
        private Panel Pnl_ShrinkMain;
        private string RegConfigPath = "";
        private List<string> ShinkList = new List<string>();
        private ConfigurationStatus.ShrinkData Shrink = null;
        private Shrink8 Sk_AndEndExclude;
        private Shrink2 Sk_AndExclude;
        private Shrink1 Sk_DM;
        private Shrink9 Sk_DWExclude;
        private Shrink4 Sk_DZXExclude;
        private Shrink5 Sk_JOExclude;
        private Shrink3 Sk_L012Exclude;
        private Shrink7 Sk_OutExclude;
        private Shrink6 Sk_ZHExclude;
        private List<Control> SpecialControlList = null;
        private RichTextBox Txt_Number;

        public ShrinkEX()
        {
            this.InitializeComponent();
        }

        private void BeautifyInterface()
        {
            if (AppInfo.Account.Configuration.Beautify)
            {
                List<Control> pControlList = new List<Control> {
                    this.Pnl_ReturnBottom
                };
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list2 = new List<Control> {
                    this.Pnl_Main
                };
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Ckb_Shrink,
                    this.Ckb_Copy,
                    this.Ckb_Reset,
                    this.Grp_NumList
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
            }
        }

        private bool CheckAndEndExclude(List<int> pCodeList)
        {
            ConfigurationStatus.ShrinkIntExclude andEndExclude = this.Shrink.AndEndExclude;
            int item = CommFunc.CountAndEnd(pCodeList);
            if ((andEndExclude.RangeList.Count != 0) && andEndExclude.RangeList.Contains(item))
            {
                return false;
            }
            return true;
        }

        private bool CheckAndExclude(List<int> pCodeList)
        {
            ConfigurationStatus.ShrinkIntExclude andExclude = this.Shrink.AndExclude;
            int item = CommFunc.CountAnd(pCodeList);
            if ((andExclude.RangeList.Count != 0) && andExclude.RangeList.Contains(item))
            {
                return false;
            }
            return true;
        }

        private bool CheckDM(List<int> pCodeList)
        {
            ConfigurationStatus.ShrinkDM dM = this.Shrink.DM;
            foreach (int num in pCodeList)
            {
                if (dM.CodeList.Contains(num))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckDWExclude(List<int> pCodeList)
        {
            ConfigurationStatus.ShrinkDWExclude dWExclude = this.Shrink.DWExclude;
            if ((dWExclude.RangeList1.Count != 0) && dWExclude.RangeList1.Contains(pCodeList[0]))
            {
                return false;
            }
            if ((dWExclude.RangeList2.Count != 0) && dWExclude.RangeList2.Contains(pCodeList[1]))
            {
                return false;
            }
            return true;
        }

        private bool CheckDZXExclude(List<int> pCodeList)
        {
            ConfigurationStatus.ShrinkStringExclude dZXExclude = this.Shrink.DZXExclude;
            string item = CommFunc.CountDZX(pCodeList);
            if ((dZXExclude.RangeList.Count != 0) && dZXExclude.RangeList.Contains(item))
            {
                return false;
            }
            return true;
        }

        private bool CheckJOExclude(List<int> pCodeList)
        {
            ConfigurationStatus.ShrinkStringExclude jOExclude = this.Shrink.JOExclude;
            string item = CommFunc.CountJO(pCodeList);
            if ((jOExclude.RangeList.Count != 0) && jOExclude.RangeList.Contains(item))
            {
                return false;
            }
            return true;
        }

        private bool CheckL012Exclude(List<int> pCodeList)
        {
            ConfigurationStatus.ShrinkStringExclude exclude = this.Shrink.L012Exclude;
            string item = CommFunc.CountL012(pCodeList);
            if ((exclude.RangeList.Count != 0) && exclude.RangeList.Contains(item))
            {
                return false;
            }
            return true;
        }

        private bool CheckOutExclude(List<int> pCodeList)
        {
            ConfigurationStatus.ShrinkIntExclude outExclude = this.Shrink.OutExclude;
            int item = CommFunc.CountOut(pCodeList);
            if ((outExclude.RangeList.Count != 0) && outExclude.RangeList.Contains(item))
            {
                return false;
            }
            return true;
        }

        public bool CheckShrinkCode(List<int> pCodeList)
        {
            if (!this.CheckDM(pCodeList))
            {
                return false;
            }
            if (!this.CheckAndExclude(pCodeList))
            {
                return false;
            }
            if (!this.CheckL012Exclude(pCodeList))
            {
                return false;
            }
            if (!this.CheckDZXExclude(pCodeList))
            {
                return false;
            }
            if (!this.CheckJOExclude(pCodeList))
            {
                return false;
            }
            if (!this.CheckZHExclude(pCodeList))
            {
                return false;
            }
            if (!this.CheckOutExclude(pCodeList))
            {
                return false;
            }
            if (!this.CheckAndEndExclude(pCodeList))
            {
                return false;
            }
            if (!this.CheckDWExclude(pCodeList))
            {
                return false;
            }
            return true;
        }

        private bool CheckZHExclude(List<int> pCodeList)
        {
            ConfigurationStatus.ShrinkStringExclude zHExclude = this.Shrink.ZHExclude;
            string item = CommFunc.CountZH(pCodeList);
            if ((zHExclude.RangeList.Count != 0) && zHExclude.RangeList.Contains(item))
            {
                return false;
            }
            return true;
        }

        private void Ckb_Copy_Click(object sender, EventArgs e)
        {
            CommFunc.CopyText(this.Txt_Number.Text);
            CommFunc.PublicMessageAll("复制号码成功！", true, MessageBoxIcon.Asterisk, "");
        }

        private void Ckb_Reset_Click(object sender, EventArgs e)
        {
            this.Sk_DM.ResetDefult();
            this.Sk_AndExclude.ResetDefult();
            this.Sk_L012Exclude.ResetDefult();
            this.Sk_DZXExclude.ResetDefult();
            this.Sk_JOExclude.ResetDefult();
            this.Sk_ZHExclude.ResetDefult();
            this.Sk_OutExclude.ResetDefult();
            this.Sk_AndEndExclude.ResetDefult();
            this.Sk_DWExclude.ResetDefult();
        }

        private void Ckb_Shrink_Click(object sender, EventArgs e)
        {
            this.ShrinkMain();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private ConfigurationStatus.ShrinkData GetShrinkData() => 
            new ConfigurationStatus.ShrinkData { 
                DM = new ConfigurationStatus.ShrinkDM(this.Sk_DM.GetSelectRange()),
                AndExclude = new ConfigurationStatus.ShrinkIntExclude(this.Sk_AndExclude.GetSelectRange()),
                L012Exclude = new ConfigurationStatus.ShrinkStringExclude(this.Sk_L012Exclude.GetSelectRange()),
                DZXExclude = new ConfigurationStatus.ShrinkStringExclude(this.Sk_DZXExclude.GetSelectRange()),
                JOExclude = new ConfigurationStatus.ShrinkStringExclude(this.Sk_JOExclude.GetSelectRange()),
                ZHExclude = new ConfigurationStatus.ShrinkStringExclude(this.Sk_ZHExclude.GetSelectRange()),
                OutExclude = new ConfigurationStatus.ShrinkIntExclude(this.Sk_OutExclude.GetSelectRange()),
                AndEndExclude = new ConfigurationStatus.ShrinkIntExclude(this.Sk_AndEndExclude.GetSelectRange()),
                DWExclude = new ConfigurationStatus.ShrinkDWExclude(this.Sk_DWExclude.GetSelectRange1(), this.Sk_DWExclude.GetSelectRange2())
            };

        private void InitializeComponent()
        {
            this.Pnl_Main = new Panel();
            this.Pnl_Num = new Panel();
            this.Grp_NumList = new GroupBox();
            this.Txt_Number = new RichTextBox();
            this.Pnl_ReturnBottom = new Panel();
            this.Ckb_Copy = new CheckBox();
            this.Ckb_Shrink = new CheckBox();
            this.Ckb_Reset = new CheckBox();
            this.Pnl_Shrink = new Panel();
            this.Pnl_Shrink3 = new Panel();
            this.Pnl_ShrinkMain = new Panel();
            this.Pnl_Shrink2 = new Panel();
            this.Pnl_Shrink1 = new Panel();
            this.Sk_DWExclude = new Shrink9();
            this.Sk_AndEndExclude = new Shrink8();
            this.Sk_OutExclude = new Shrink7();
            this.Sk_ZHExclude = new Shrink6();
            this.Sk_JOExclude = new Shrink5();
            this.Sk_DZXExclude = new Shrink4();
            this.Sk_L012Exclude = new Shrink3();
            this.Sk_AndExclude = new Shrink2();
            this.Sk_DM = new Shrink1();
            this.Pnl_Main.SuspendLayout();
            this.Pnl_Num.SuspendLayout();
            this.Grp_NumList.SuspendLayout();
            this.Pnl_ReturnBottom.SuspendLayout();
            this.Pnl_Shrink.SuspendLayout();
            this.Pnl_Shrink3.SuspendLayout();
            this.Pnl_ShrinkMain.SuspendLayout();
            this.Pnl_Shrink2.SuspendLayout();
            this.Pnl_Shrink1.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_Main.Controls.Add(this.Pnl_Num);
            this.Pnl_Main.Controls.Add(this.Pnl_Shrink);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(0x3e8, 0x218);
            this.Pnl_Main.TabIndex = 0;
            this.Pnl_Num.BackColor = Color.Transparent;
            this.Pnl_Num.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Num.Controls.Add(this.Grp_NumList);
            this.Pnl_Num.Dock = DockStyle.Fill;
            this.Pnl_Num.Location = new Point(0x205, 0);
            this.Pnl_Num.Name = "Pnl_Num";
            this.Pnl_Num.Size = new Size(0x1e3, 0x218);
            this.Pnl_Num.TabIndex = 1;
            this.Grp_NumList.Controls.Add(this.Txt_Number);
            this.Grp_NumList.Controls.Add(this.Pnl_ReturnBottom);
            this.Grp_NumList.Dock = DockStyle.Fill;
            this.Grp_NumList.Location = new Point(0, 0);
            this.Grp_NumList.Name = "Grp_NumList";
            this.Grp_NumList.Size = new Size(0x1e1, 0x216);
            this.Grp_NumList.TabIndex = 0x43;
            this.Grp_NumList.TabStop = false;
            this.Grp_NumList.Text = "我的号码";
            this.Txt_Number.BorderStyle = BorderStyle.FixedSingle;
            this.Txt_Number.Dock = DockStyle.Fill;
            this.Txt_Number.Location = new Point(3, 0x13);
            this.Txt_Number.Name = "Txt_Number";
            this.Txt_Number.Size = new Size(0x1db, 0x1dd);
            this.Txt_Number.TabIndex = 0x79;
            this.Pnl_ReturnBottom.Controls.Add(this.Ckb_Copy);
            this.Pnl_ReturnBottom.Controls.Add(this.Ckb_Shrink);
            this.Pnl_ReturnBottom.Controls.Add(this.Ckb_Reset);
            this.Pnl_ReturnBottom.Dock = DockStyle.Bottom;
            this.Pnl_ReturnBottom.Location = new Point(3, 0x1f0);
            this.Pnl_ReturnBottom.Name = "Pnl_ReturnBottom";
            this.Pnl_ReturnBottom.Size = new Size(0x1db, 0x23);
            this.Pnl_ReturnBottom.TabIndex = 120;
            this.Ckb_Copy.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Ckb_Copy.Appearance = Appearance.Button;
            this.Ckb_Copy.AutoCheck = false;
            this.Ckb_Copy.FlatAppearance.BorderSize = 0;
            this.Ckb_Copy.FlatStyle = FlatStyle.Flat;
            this.Ckb_Copy.Image = Resources.Copy;
            this.Ckb_Copy.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Copy.Location = new Point(300, 5);
            this.Ckb_Copy.Name = "Ckb_Copy";
            this.Ckb_Copy.Size = new Size(80, 0x19);
            this.Ckb_Copy.TabIndex = 0xa8;
            this.Ckb_Copy.Text = "复制号码";
            this.Ckb_Copy.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Copy.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Copy.UseVisualStyleBackColor = true;
            this.Ckb_Copy.Click += new EventHandler(this.Ckb_Copy_Click);
            this.Ckb_Shrink.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Ckb_Shrink.Appearance = Appearance.Button;
            this.Ckb_Shrink.AutoCheck = false;
            this.Ckb_Shrink.FlatAppearance.BorderSize = 0;
            this.Ckb_Shrink.FlatStyle = FlatStyle.Flat;
            this.Ckb_Shrink.Image = Resources.Filter;
            this.Ckb_Shrink.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Shrink.Location = new Point(0xd6, 5);
            this.Ckb_Shrink.Name = "Ckb_Shrink";
            this.Ckb_Shrink.Size = new Size(80, 0x19);
            this.Ckb_Shrink.TabIndex = 0xa6;
            this.Ckb_Shrink.Text = "生成号码";
            this.Ckb_Shrink.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Shrink.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_Shrink.UseVisualStyleBackColor = true;
            this.Ckb_Shrink.Click += new EventHandler(this.Ckb_Shrink_Click);
            this.Ckb_Reset.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Ckb_Reset.Appearance = Appearance.Button;
            this.Ckb_Reset.AutoCheck = false;
            this.Ckb_Reset.FlatAppearance.BorderSize = 0;
            this.Ckb_Reset.FlatStyle = FlatStyle.Flat;
            this.Ckb_Reset.Image = Resources.Reset;
            this.Ckb_Reset.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Reset.Location = new Point(0x182, 5);
            this.Ckb_Reset.Name = "Ckb_Reset";
            this.Ckb_Reset.Size = new Size(80, 0x19);
            this.Ckb_Reset.TabIndex = 0xa7;
            this.Ckb_Reset.Text = "重置条件";
            this.Ckb_Reset.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Reset.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_Reset.UseVisualStyleBackColor = true;
            this.Ckb_Reset.Click += new EventHandler(this.Ckb_Reset_Click);
            this.Pnl_Shrink.BackColor = Color.Transparent;
            this.Pnl_Shrink.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Shrink.Controls.Add(this.Pnl_Shrink3);
            this.Pnl_Shrink.Controls.Add(this.Pnl_ShrinkMain);
            this.Pnl_Shrink.Dock = DockStyle.Left;
            this.Pnl_Shrink.Location = new Point(0, 0);
            this.Pnl_Shrink.Name = "Pnl_Shrink";
            this.Pnl_Shrink.Size = new Size(0x205, 0x218);
            this.Pnl_Shrink.TabIndex = 2;
            this.Pnl_Shrink3.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Shrink3.Controls.Add(this.Sk_DWExclude);
            this.Pnl_Shrink3.Dock = DockStyle.Top;
            this.Pnl_Shrink3.Location = new Point(0, 0x171);
            this.Pnl_Shrink3.Name = "Pnl_Shrink3";
            this.Pnl_Shrink3.Size = new Size(0x203, 0x55);
            this.Pnl_Shrink3.TabIndex = 3;
            this.Pnl_ShrinkMain.Controls.Add(this.Pnl_Shrink2);
            this.Pnl_ShrinkMain.Controls.Add(this.Pnl_Shrink1);
            this.Pnl_ShrinkMain.Dock = DockStyle.Top;
            this.Pnl_ShrinkMain.Location = new Point(0, 0);
            this.Pnl_ShrinkMain.Name = "Pnl_ShrinkMain";
            this.Pnl_ShrinkMain.Size = new Size(0x203, 0x171);
            this.Pnl_ShrinkMain.TabIndex = 4;
            this.Pnl_Shrink2.Controls.Add(this.Sk_AndEndExclude);
            this.Pnl_Shrink2.Controls.Add(this.Sk_OutExclude);
            this.Pnl_Shrink2.Controls.Add(this.Sk_ZHExclude);
            this.Pnl_Shrink2.Controls.Add(this.Sk_JOExclude);
            this.Pnl_Shrink2.Controls.Add(this.Sk_DZXExclude);
            this.Pnl_Shrink2.Dock = DockStyle.Left;
            this.Pnl_Shrink2.Location = new Point(0xef, 0);
            this.Pnl_Shrink2.Name = "Pnl_Shrink2";
            this.Pnl_Shrink2.Size = new Size(0x113, 0x171);
            this.Pnl_Shrink2.TabIndex = 1;
            this.Pnl_Shrink1.Controls.Add(this.Sk_L012Exclude);
            this.Pnl_Shrink1.Controls.Add(this.Sk_AndExclude);
            this.Pnl_Shrink1.Controls.Add(this.Sk_DM);
            this.Pnl_Shrink1.Dock = DockStyle.Left;
            this.Pnl_Shrink1.Location = new Point(0, 0);
            this.Pnl_Shrink1.Name = "Pnl_Shrink1";
            this.Pnl_Shrink1.Size = new Size(0xef, 0x171);
            this.Pnl_Shrink1.TabIndex = 0;
            this.Sk_DWExclude.Dock = DockStyle.Top;
            this.Sk_DWExclude.Font = new Font("微软雅黑", 9f);
            this.Sk_DWExclude.Location = new Point(0, 0);
            this.Sk_DWExclude.Margin = new Padding(3, 4, 3, 4);
            this.Sk_DWExclude.Name = "Sk_DWExclude";
            this.Sk_DWExclude.Size = new Size(0x201, 80);
            this.Sk_DWExclude.TabIndex = 0;
            this.Sk_AndEndExclude.BorderStyle = BorderStyle.FixedSingle;
            this.Sk_AndEndExclude.Dock = DockStyle.Top;
            this.Sk_AndEndExclude.Font = new Font("微软雅黑", 9f);
            this.Sk_AndEndExclude.Location = new Point(0, 0x123);
            this.Sk_AndEndExclude.Margin = new Padding(3, 4, 3, 4);
            this.Sk_AndEndExclude.Name = "Sk_AndEndExclude";
            this.Sk_AndEndExclude.Size = new Size(0x113, 0x4c);
            this.Sk_AndEndExclude.TabIndex = 4;
            this.Sk_OutExclude.BorderStyle = BorderStyle.FixedSingle;
            this.Sk_OutExclude.Dock = DockStyle.Top;
            this.Sk_OutExclude.Font = new Font("微软雅黑", 9f);
            this.Sk_OutExclude.Location = new Point(0, 0xd7);
            this.Sk_OutExclude.Margin = new Padding(3, 4, 3, 4);
            this.Sk_OutExclude.Name = "Sk_OutExclude";
            this.Sk_OutExclude.Size = new Size(0x113, 0x4c);
            this.Sk_OutExclude.TabIndex = 3;
            this.Sk_ZHExclude.BorderStyle = BorderStyle.FixedSingle;
            this.Sk_ZHExclude.Dock = DockStyle.Top;
            this.Sk_ZHExclude.Font = new Font("微软雅黑", 9f);
            this.Sk_ZHExclude.Location = new Point(0, 160);
            this.Sk_ZHExclude.Margin = new Padding(3, 4, 3, 4);
            this.Sk_ZHExclude.Name = "Sk_ZHExclude";
            this.Sk_ZHExclude.Size = new Size(0x113, 0x37);
            this.Sk_ZHExclude.TabIndex = 2;
            this.Sk_JOExclude.BorderStyle = BorderStyle.FixedSingle;
            this.Sk_JOExclude.Dock = DockStyle.Top;
            this.Sk_JOExclude.Font = new Font("微软雅黑", 9f);
            this.Sk_JOExclude.Location = new Point(0, 0x69);
            this.Sk_JOExclude.Margin = new Padding(3, 4, 3, 4);
            this.Sk_JOExclude.Name = "Sk_JOExclude";
            this.Sk_JOExclude.Size = new Size(0x113, 0x37);
            this.Sk_JOExclude.TabIndex = 1;
            this.Sk_DZXExclude.BorderStyle = BorderStyle.FixedSingle;
            this.Sk_DZXExclude.Dock = DockStyle.Top;
            this.Sk_DZXExclude.Font = new Font("微软雅黑", 9f);
            this.Sk_DZXExclude.Location = new Point(0, 0);
            this.Sk_DZXExclude.Margin = new Padding(3, 4, 3, 4);
            this.Sk_DZXExclude.Name = "Sk_DZXExclude";
            this.Sk_DZXExclude.Size = new Size(0x113, 0x69);
            this.Sk_DZXExclude.TabIndex = 0;
            this.Sk_L012Exclude.BorderStyle = BorderStyle.FixedSingle;
            this.Sk_L012Exclude.Dock = DockStyle.Top;
            this.Sk_L012Exclude.Font = new Font("微软雅黑", 9f);
            this.Sk_L012Exclude.Location = new Point(0, 0x109);
            this.Sk_L012Exclude.Margin = new Padding(3, 4, 3, 4);
            this.Sk_L012Exclude.Name = "Sk_L012Exclude";
            this.Sk_L012Exclude.Size = new Size(0xef, 0x67);
            this.Sk_L012Exclude.TabIndex = 2;
            this.Sk_AndExclude.BorderStyle = BorderStyle.FixedSingle;
            this.Sk_AndExclude.Dock = DockStyle.Top;
            this.Sk_AndExclude.Font = new Font("微软雅黑", 9f);
            this.Sk_AndExclude.Location = new Point(0, 0x69);
            this.Sk_AndExclude.Margin = new Padding(3, 4, 3, 4);
            this.Sk_AndExclude.Name = "Sk_AndExclude";
            this.Sk_AndExclude.Size = new Size(0xef, 160);
            this.Sk_AndExclude.TabIndex = 1;
            this.Sk_DM.BorderStyle = BorderStyle.FixedSingle;
            this.Sk_DM.Dock = DockStyle.Top;
            this.Sk_DM.Font = new Font("微软雅黑", 9f);
            this.Sk_DM.Location = new Point(0, 0);
            this.Sk_DM.Margin = new Padding(3, 4, 3, 4);
            this.Sk_DM.Name = "Sk_DM";
            this.Sk_DM.Size = new Size(0xef, 0x69);
            this.Sk_DM.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(96f, 96f);
            base.AutoScaleMode = AutoScaleMode.Dpi;
            base.Controls.Add(this.Pnl_Main);
            this.Font = new Font("微软雅黑", 9f);
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "ShrinkEX";
            base.Size = new Size(0x3e8, 0x218);
            base.Load += new EventHandler(this.ShrinkEX_Load);
            this.Pnl_Main.ResumeLayout(false);
            this.Pnl_Num.ResumeLayout(false);
            this.Grp_NumList.ResumeLayout(false);
            this.Pnl_ReturnBottom.ResumeLayout(false);
            this.Pnl_Shrink.ResumeLayout(false);
            this.Pnl_Shrink3.ResumeLayout(false);
            this.Pnl_ShrinkMain.ResumeLayout(false);
            this.Pnl_Shrink2.ResumeLayout(false);
            this.Pnl_Shrink1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void SaveControlInfoByReg()
        {
            CommFunc.SaveFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SaveSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
            this.Sk_DM.SaveControlInfoByReg();
            this.Sk_AndExclude.SaveControlInfoByReg();
            this.Sk_L012Exclude.SaveControlInfoByReg();
            this.Sk_DZXExclude.SaveControlInfoByReg();
            this.Sk_JOExclude.SaveControlInfoByReg();
            this.Sk_ZHExclude.SaveControlInfoByReg();
            this.Sk_OutExclude.SaveControlInfoByReg();
            this.Sk_AndEndExclude.SaveControlInfoByReg();
            this.Sk_DWExclude.SaveControlInfoByReg();
        }

        public void SetControlInfoByReg()
        {
            this.RegConfigPath = @"software\TUHAOPLUS\YXZXGJ\DlgConfig\永信在线挂机软件\" + base.Name;
            this.ControlList = new List<Control>();
            this.SpecialControlList = new List<Control>();
            CommFunc.SetFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SetSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
        }

        private void SetNumberHint()
        {
            string str = "我的号码";
            if (this.ShinkList.Count > 0)
            {
                str = str + $"：{this.ShinkList.Count} 注";
            }
            this.Grp_NumList.Text = str;
        }

        private void ShrinkEX_Load(object sender, EventArgs e)
        {
            List<CheckBox> pCheckBoxList = new List<CheckBox> {
                this.Ckb_Shrink,
                this.Ckb_Copy,
                this.Ckb_Reset
            };
            CommFunc.SetCheckBoxFormatFlat(pCheckBoxList);
            this.BeautifyInterface();
            this.SetControlInfoByReg();
        }

        private void ShrinkMain()
        {
            this.ShinkList.Clear();
            this.Shrink = this.GetShrinkData();
            if (this.Shrink.DM.CodeList.Count == 0)
            {
                CommFunc.PublicMessageAll("至少要选择一个胆码！", true, MessageBoxIcon.Asterisk, "");
            }
            else
            {
                if (AppInfo.Current != null)
                {
                    this.NumberList = CommFunc.GetCombinaList(ConfigurationStatus.CombinaType.ZX, 2, -1, -1);
                }
                foreach (string str in this.NumberList)
                {
                    List<int> pCodeList = CommFunc.ConvertSameListInt(str);
                    if (this.CheckShrinkCode(pCodeList))
                    {
                        this.ShinkList.Add(str);
                    }
                }
                this.SetNumberHint();
                this.Txt_Number.Text = CommFunc.Join(this.ShinkList, " ");
            }
        }
    }
}

