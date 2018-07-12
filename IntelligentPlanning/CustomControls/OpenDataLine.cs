namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class OpenDataLine : UserControl
    {
        private CheckBox Ckb_Name;
        private IContainer components = null;
        private List<Control> ControlList = null;
        private int expect = 0;
        public List<Label> LabelList = null;
        private Label Lbl_CurrentCode;
        private Label Lbl_CurrentCode1;
        private Label Lbl_CurrentCode2;
        private Label Lbl_CurrentCode3;
        private Label Lbl_CurrentCode4;
        private Label Lbl_CurrentCode5;
        private Label Lbl_CurrentExpect;
        private Label Lbl_NumKey;
        private Label Lbl_NumValue;
        public string LotteryID = "";
        private Panel Pnl_CurrentCode1;
        private Panel Pnl_CurrentCode2;
        private Panel Pnl_Main;
        private Panel Pnl_Top;
        public PTBase PTInfo = null;
        private string RegScenarionConfigPath = "";
        private List<Control> SpecialControlList = null;
        private ConfigurationStatus.LotteryType type = ConfigurationStatus.LotteryType.CQSSC;

        public OpenDataLine()
        {
            this.InitializeComponent();
        }

        public void ClearForm()
        {
            this.Lbl_CurrentExpect.Text = "";
            this.Lbl_CurrentCode1.Text = "";
            this.Lbl_CurrentCode2.Text = "";
            this.Lbl_CurrentCode3.Text = "";
            this.Lbl_CurrentCode4.Text = "";
            this.Lbl_CurrentCode5.Text = "";
            this.Lbl_CurrentCode.Text = "";
            this.Lbl_NumValue.Text = "";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public List<string> GetCodeList(string pCode)
        {
            List<string> list = new List<string>();
            if (pCode.Contains(","))
            {
                return CommFunc.SplitString(pCode, ",", -1);
            }
            if (pCode.Contains(" "))
            {
                return CommFunc.SplitString(pCode, " ", -1);
            }
            return CommFunc.ConvertSameListString(pCode);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(OpenDataLine));
            this.Ckb_Name = new CheckBox();
            this.Lbl_NumKey = new Label();
            this.Lbl_NumValue = new Label();
            this.Lbl_CurrentCode5 = new Label();
            this.Lbl_CurrentCode4 = new Label();
            this.Lbl_CurrentCode3 = new Label();
            this.Lbl_CurrentCode2 = new Label();
            this.Lbl_CurrentExpect = new Label();
            this.Lbl_CurrentCode1 = new Label();
            this.Pnl_Top = new Panel();
            this.Pnl_Main = new Panel();
            this.Pnl_CurrentCode1 = new Panel();
            this.Pnl_CurrentCode2 = new Panel();
            this.Lbl_CurrentCode = new Label();
            this.Pnl_Top.SuspendLayout();
            this.Pnl_Main.SuspendLayout();
            this.Pnl_CurrentCode1.SuspendLayout();
            this.Pnl_CurrentCode2.SuspendLayout();
            base.SuspendLayout();
            this.Ckb_Name.AutoSize = true;
            this.Ckb_Name.Checked = true;
            this.Ckb_Name.CheckState = CheckState.Checked;
            this.Ckb_Name.Location = new Point(6, 7);
            this.Ckb_Name.Name = "Ckb_Name";
            this.Ckb_Name.Size = new Size(0x57, 0x15);
            this.Ckb_Name.TabIndex = 1;
            this.Ckb_Name.Text = "重庆时时彩";
            this.Ckb_Name.UseVisualStyleBackColor = true;
            this.Lbl_NumKey.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Lbl_NumKey.AutoSize = true;
            this.Lbl_NumKey.Location = new Point(0x6c, 8);
            this.Lbl_NumKey.Name = "Lbl_NumKey";
            this.Lbl_NumKey.Size = new Size(0x2c, 0x11);
            this.Lbl_NumKey.TabIndex = 0x17;
            this.Lbl_NumKey.Text = "期数：";
            this.Lbl_NumValue.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Lbl_NumValue.AutoSize = true;
            this.Lbl_NumValue.Location = new Point(150, 8);
            this.Lbl_NumValue.Name = "Lbl_NumValue";
            this.Lbl_NumValue.Size = new Size(15, 0x11);
            this.Lbl_NumValue.TabIndex = 0x18;
            this.Lbl_NumValue.Text = "0";
            this.Lbl_CurrentCode5.Font = new Font("微软雅黑", 9.5f, FontStyle.Bold);
            this.Lbl_CurrentCode5.ForeColor = Color.White;
            this.Lbl_CurrentCode5.Image = (Image) manager.GetObject("Lbl_CurrentCode5.Image");
            this.Lbl_CurrentCode5.ImageAlign = ContentAlignment.BottomCenter;
            this.Lbl_CurrentCode5.Location = new Point(0xa4, 3);
            this.Lbl_CurrentCode5.Name = "Lbl_CurrentCode5";
            this.Lbl_CurrentCode5.Size = new Size(0x21, 0x23);
            this.Lbl_CurrentCode5.TabIndex = 5;
            this.Lbl_CurrentCode5.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode4.Font = new Font("微软雅黑", 9.5f, FontStyle.Bold);
            this.Lbl_CurrentCode4.ForeColor = Color.White;
            this.Lbl_CurrentCode4.Image = (Image) manager.GetObject("Lbl_CurrentCode4.Image");
            this.Lbl_CurrentCode4.ImageAlign = ContentAlignment.BottomCenter;
            this.Lbl_CurrentCode4.Location = new Point(0x7c, 3);
            this.Lbl_CurrentCode4.Name = "Lbl_CurrentCode4";
            this.Lbl_CurrentCode4.Size = new Size(0x21, 0x23);
            this.Lbl_CurrentCode4.TabIndex = 4;
            this.Lbl_CurrentCode4.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode3.Font = new Font("微软雅黑", 9.5f, FontStyle.Bold);
            this.Lbl_CurrentCode3.ForeColor = Color.White;
            this.Lbl_CurrentCode3.Image = (Image) manager.GetObject("Lbl_CurrentCode3.Image");
            this.Lbl_CurrentCode3.ImageAlign = ContentAlignment.BottomCenter;
            this.Lbl_CurrentCode3.Location = new Point(0x54, 3);
            this.Lbl_CurrentCode3.Name = "Lbl_CurrentCode3";
            this.Lbl_CurrentCode3.Size = new Size(0x21, 0x23);
            this.Lbl_CurrentCode3.TabIndex = 3;
            this.Lbl_CurrentCode3.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode2.Font = new Font("微软雅黑", 9.5f, FontStyle.Bold);
            this.Lbl_CurrentCode2.ForeColor = Color.White;
            this.Lbl_CurrentCode2.Image = (Image) manager.GetObject("Lbl_CurrentCode2.Image");
            this.Lbl_CurrentCode2.ImageAlign = ContentAlignment.BottomCenter;
            this.Lbl_CurrentCode2.Location = new Point(0x2c, 3);
            this.Lbl_CurrentCode2.Name = "Lbl_CurrentCode2";
            this.Lbl_CurrentCode2.Size = new Size(0x21, 0x23);
            this.Lbl_CurrentCode2.TabIndex = 2;
            this.Lbl_CurrentCode2.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentExpect.Dock = DockStyle.Top;
            this.Lbl_CurrentExpect.Location = new Point(0, 0);
            this.Lbl_CurrentExpect.Name = "Lbl_CurrentExpect";
            this.Lbl_CurrentExpect.Size = new Size(0xcc, 20);
            this.Lbl_CurrentExpect.TabIndex = 1;
            this.Lbl_CurrentExpect.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode1.Font = new Font("微软雅黑", 9.5f, FontStyle.Bold);
            this.Lbl_CurrentCode1.ForeColor = Color.White;
            this.Lbl_CurrentCode1.Image = (Image) manager.GetObject("Lbl_CurrentCode1.Image");
            this.Lbl_CurrentCode1.ImageAlign = ContentAlignment.BottomCenter;
            this.Lbl_CurrentCode1.Location = new Point(4, 3);
            this.Lbl_CurrentCode1.Name = "Lbl_CurrentCode1";
            this.Lbl_CurrentCode1.Size = new Size(0x21, 0x23);
            this.Lbl_CurrentCode1.TabIndex = 0;
            this.Lbl_CurrentCode1.TextAlign = ContentAlignment.MiddleCenter;
            this.Pnl_Top.Controls.Add(this.Ckb_Name);
            this.Pnl_Top.Controls.Add(this.Lbl_NumKey);
            this.Pnl_Top.Controls.Add(this.Lbl_NumValue);
            this.Pnl_Top.Dock = DockStyle.Top;
            this.Pnl_Top.Location = new Point(0, 0);
            this.Pnl_Top.Name = "Pnl_Top";
            this.Pnl_Top.Size = new Size(0xcc, 0x23);
            this.Pnl_Top.TabIndex = 0x1a;
            this.Pnl_Main.Controls.Add(this.Pnl_CurrentCode1);
            this.Pnl_Main.Controls.Add(this.Pnl_CurrentCode2);
            this.Pnl_Main.Controls.Add(this.Lbl_CurrentExpect);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0x23);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(0xcc, 0x41);
            this.Pnl_Main.TabIndex = 0x1b;
            this.Pnl_CurrentCode1.Controls.Add(this.Lbl_CurrentCode1);
            this.Pnl_CurrentCode1.Controls.Add(this.Lbl_CurrentCode5);
            this.Pnl_CurrentCode1.Controls.Add(this.Lbl_CurrentCode2);
            this.Pnl_CurrentCode1.Controls.Add(this.Lbl_CurrentCode3);
            this.Pnl_CurrentCode1.Controls.Add(this.Lbl_CurrentCode4);
            this.Pnl_CurrentCode1.Dock = DockStyle.Fill;
            this.Pnl_CurrentCode1.Location = new Point(0, 20);
            this.Pnl_CurrentCode1.Name = "Pnl_CurrentCode1";
            this.Pnl_CurrentCode1.Size = new Size(0xcc, 0x2d);
            this.Pnl_CurrentCode1.TabIndex = 6;
            this.Pnl_CurrentCode2.Controls.Add(this.Lbl_CurrentCode);
            this.Pnl_CurrentCode2.Dock = DockStyle.Fill;
            this.Pnl_CurrentCode2.Location = new Point(0, 20);
            this.Pnl_CurrentCode2.Name = "Pnl_CurrentCode2";
            this.Pnl_CurrentCode2.Size = new Size(0xcc, 0x2d);
            this.Pnl_CurrentCode2.TabIndex = 7;
            this.Lbl_CurrentCode.Dock = DockStyle.Fill;
            this.Lbl_CurrentCode.Location = new Point(0, 0);
            this.Lbl_CurrentCode.Name = "Lbl_CurrentCode";
            this.Lbl_CurrentCode.Size = new Size(0xcc, 0x2d);
            this.Lbl_CurrentCode.TabIndex = 0;
            this.Lbl_CurrentCode.TextAlign = ContentAlignment.MiddleCenter;
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.BorderStyle = BorderStyle.FixedSingle;
            base.Controls.Add(this.Pnl_Main);
            base.Controls.Add(this.Pnl_Top);
            this.Font = new Font("微软雅黑", 9f);
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "OpenDataLine";
            base.Size = new Size(0xcc, 100);
            base.Load += new EventHandler(this.OpenDataLine_Load);
            this.Pnl_Top.ResumeLayout(false);
            this.Pnl_Top.PerformLayout();
            this.Pnl_Main.ResumeLayout(false);
            this.Pnl_CurrentCode1.ResumeLayout(false);
            this.Pnl_CurrentCode2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void OpenDataLine_Load(object sender, EventArgs e)
        {
            List<Label> list = new List<Label> {
                this.Lbl_NumValue
            };
            this.LabelList = list;
            CommFunc.SetLabelFormat(this.LabelList);
        }

        public void RefreshNewData(ConfigurationStatus.OpenData pData)
        {
            string expect = pData.Expect;
            this.Lbl_CurrentExpect.Text = $"{this.Hint} 第 {expect} 期";
            List<string> codeList = this.GetCodeList(pData.Code);
            this.Lbl_CurrentCode1.Text = codeList[0];
            this.Lbl_CurrentCode2.Text = codeList[1];
            this.Lbl_CurrentCode3.Text = codeList[2];
            if (codeList.Count > 3)
            {
                this.Lbl_CurrentCode4.Text = codeList[3];
                this.Lbl_CurrentCode5.Text = codeList[4];
            }
        }

        public void SaveControlInfoByReg()
        {
            CommFunc.SaveFormUseingInfo(this.ControlList, this.RegScenarionConfigPath);
            CommFunc.SaveSpecialControlInfo(this.SpecialControlList, this.RegScenarionConfigPath);
        }

        public void SetControlInfoByReg(string pRegScenarionConfigPath)
        {
            this.RegScenarionConfigPath = pRegScenarionConfigPath + @"\" + this.LotteryID;
            List<Control> list = new List<Control> {
                this
            };
            this.ControlList = list;
            this.SpecialControlList = new List<Control>();
            CommFunc.SetFormUseingInfo(this.ControlList, this.RegScenarionConfigPath);
            CommFunc.SetSpecialControlInfo(this.SpecialControlList, this.RegScenarionConfigPath);
        }

        private void Txt_Input_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((((e.KeyChar != '\b') && !char.IsDigit(e.KeyChar)) && (e.KeyChar != '-')) && (e.KeyChar != ','))
            {
                e.Handled = true;
            }
        }

        public int Count
        {
            get => 
                Convert.ToInt32(this.Lbl_NumValue.Text);
            set
            {
                this.Lbl_NumValue.Text = value.ToString();
            }
        }

        public int Expect
        {
            get => 
                this.expect;
            set
            {
                this.expect = value;
            }
        }

        public CheckBox GetLotterySelect =>
            this.Ckb_Name;

        public string Hint
        {
            get => 
                this.Ckb_Name.Text;
            set
            {
                this.Ckb_Name.Text = value;
            }
        }

        public bool SelectEnable
        {
            get => 
                this.Ckb_Name.Enabled;
            set
            {
                this.Ckb_Name.Enabled = value;
            }
        }

        public bool SelectState
        {
            get => 
                this.Ckb_Name.Checked;
            set
            {
                this.Ckb_Name.Checked = value;
            }
        }

        public ConfigurationStatus.LotteryType Type
        {
            get => 
                this.type;
            set
            {
                this.type = value;
                List<ConfigurationStatus.LotteryType> list = new List<ConfigurationStatus.LotteryType>();
                if (list.Contains(this.type))
                {
                    this.Pnl_CurrentCode1.Visible = false;
                    this.Pnl_CurrentCode2.Visible = true;
                }
                else
                {
                    this.Pnl_CurrentCode1.Visible = true;
                    this.Pnl_CurrentCode2.Visible = false;
                }
                base.Invalidate();
            }
        }
    }
}

