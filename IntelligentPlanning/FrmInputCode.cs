namespace IntelligentPlanning
{
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class FrmInputCode : ExForm
    {
        private CheckBox Ckb_Cancel;
        private CheckBox Ckb_Clear;
        private CheckBox Ckb_Ok;
        private CheckBox Ckb_Random;
        private IContainer components = null;
        private GroupBox Grp_Input;
        public List<int> IDList;
        private Label Lbl_Hin1;
        private Label Lbl_Hin2;
        private Label Lbl_Hin3;
        private Label Lbl_ID;
        private Label Lbl_NoAfter;
        private Label Lbl_Random;
        private Label Lbl_YesAfter;
        private NumericUpDown Nm_ID;
        private NumericUpDown Nm_NoAfter;
        private NumericUpDown Nm_Random;
        private NumericUpDown Nm_YesAfter;
        public static ConfigurationStatus.GJDMLH OutValue;
        public ConfigurationStatus.PlayBase PlayInfo;
        private Panel Pnl_Bottom;
        private Panel Pnl_Top;
        private RichTextBox Txt_Input;

        public FrmInputCode(ConfigurationStatus.GJDMLH pInput, ConfigurationStatus.PlayBase playInfo, bool pIsAdd, List<int> pIDList = null)
        {
            this.InitializeComponent();
            List<Control> list = new List<Control> {
                this,
                this.Nm_Random
            };
            base.ControlList = list;
            this.PlayInfo = playInfo;
            this.IDList = pIDList;
            this.Nm_ID.Value = pInput.ID;
            this.Nm_YesAfter.Value = pInput.YesAfter;
            this.Nm_NoAfter.Value = pInput.NoAfter;
            this.Txt_Input.Text = pInput.Value;
            CommFunc.ConvertInputText(this.Txt_Input, this.PlayInfo);
            this.Nm_ID.Enabled = pIsAdd;
            this.Text = pIsAdd ? "添加组号" : "修改组号";
            this.Text = this.Text + "-" + this.PlayInfo.Play;
            List<CheckBox> list2 = new List<CheckBox> {
                this.Ckb_Random,
                this.Ckb_Clear,
                this.Ckb_Ok,
                this.Ckb_Cancel
            };
            base.CheckBoxList = list2;
        }

        private void BeautifyInterface()
        {
            if (AppInfo.Account.Configuration.Beautify)
            {
                List<Control> pControlList = new List<Control> {
                    this.Pnl_Top,
                    this.Pnl_Bottom
                };
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list2 = new List<Control> {
                    this.Grp_Input
                };
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Lbl_ID,
                    this.Lbl_Hin1,
                    this.Lbl_YesAfter,
                    this.Lbl_Hin2,
                    this.Lbl_NoAfter,
                    this.Lbl_Hin3,
                    this.Grp_Input,
                    this.Lbl_Random,
                    this.Ckb_Random,
                    this.Ckb_Clear,
                    this.Ckb_Ok,
                    this.Ckb_Cancel
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
            }
        }

        private void Ckb_Clear_Click(object sender, EventArgs e)
        {
            this.Txt_Input.Text = "";
        }

        private void Ckb_Close_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void Ckb_Ok_Click(object sender, EventArgs e)
        {
            string pErrorHint = "";
            string pInput = CommFunc.ReplaceText(this.Txt_Input.Text, this.PlayInfo, true);
            List<string> list = CommFunc.FilterNumber(pInput, this.PlayInfo.CodeCount, this.PlayInfo.Play, ref pErrorHint);
            if (pErrorHint != "")
            {
                CommFunc.PublicMessageAll(pErrorHint, true, MessageBoxIcon.Asterisk, "");
            }
            else
            {
                OutValue = new ConfigurationStatus.GJDMLH();
                OutValue.ID = Convert.ToInt32(this.Nm_ID.Value);
                if ((this.IDList != null) && this.IDList.Contains(OutValue.ID))
                {
                    CommFunc.PublicMessageAll($"局数【{OutValue.ID}】已经存在，请重新输入！", true, MessageBoxIcon.Asterisk, "");
                }
                else
                {
                    OutValue.Value = pInput;
                    OutValue.YesAfter = Convert.ToInt32(this.Nm_YesAfter.Value);
                    OutValue.NoAfter = Convert.ToInt32(this.Nm_NoAfter.Value);
                    base.DialogResult = DialogResult.OK;
                }
            }
        }

        private void Ckb_Random_Click(object sender, EventArgs e)
        {
            int pCycle = 1;
            int pCodeCount = Convert.ToInt32(this.Nm_Random.Value);
            this.Txt_Input.Text = CommFunc.Join(CommFunc.GetRandomByPlay(this.PlayInfo.PlayType, this.PlayInfo.PlayName, "", "", pCycle, pCodeCount), "\r\n");
            CommFunc.ConvertInputText(this.Txt_Input, this.PlayInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FrmInputCode_Load(object sender, EventArgs e)
        {
            this.BeautifyInterface();
            this.Txt_Input_TextChanged(null, null);
            this.Txt_Input.Select(this.Txt_Input.TextLength, 1);
            this.Txt_Input.Focus();
        }

        private void InitializeComponent()
        {
            this.Grp_Input = new GroupBox();
            this.Txt_Input = new RichTextBox();
            this.Pnl_Top = new Panel();
            this.Lbl_NoAfter = new Label();
            this.Nm_NoAfter = new NumericUpDown();
            this.Lbl_Hin3 = new Label();
            this.Lbl_YesAfter = new Label();
            this.Nm_YesAfter = new NumericUpDown();
            this.Lbl_Hin2 = new Label();
            this.Lbl_ID = new Label();
            this.Lbl_Hin1 = new Label();
            this.Nm_ID = new NumericUpDown();
            this.Pnl_Bottom = new Panel();
            this.Ckb_Clear = new CheckBox();
            this.Ckb_Random = new CheckBox();
            this.Ckb_Cancel = new CheckBox();
            this.Ckb_Ok = new CheckBox();
            this.Nm_Random = new NumericUpDown();
            this.Lbl_Random = new Label();
            this.Grp_Input.SuspendLayout();
            this.Pnl_Top.SuspendLayout();
            this.Nm_NoAfter.BeginInit();
            this.Nm_YesAfter.BeginInit();
            this.Nm_ID.BeginInit();
            this.Pnl_Bottom.SuspendLayout();
            this.Nm_Random.BeginInit();
            base.SuspendLayout();
            this.Grp_Input.Controls.Add(this.Txt_Input);
            this.Grp_Input.Dock = DockStyle.Fill;
            this.Grp_Input.Location = new Point(0, 0x23);
            this.Grp_Input.Name = "Grp_Input";
            this.Grp_Input.Size = new Size(0x1ff, 0x134);
            this.Grp_Input.TabIndex = 11;
            this.Grp_Input.TabStop = false;
            this.Grp_Input.Text = "注数：";
            this.Txt_Input.Dock = DockStyle.Fill;
            this.Txt_Input.Location = new Point(3, 0x13);
            this.Txt_Input.Name = "Txt_Input";
            this.Txt_Input.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.Txt_Input.Size = new Size(0x1f9, 0x11e);
            this.Txt_Input.TabIndex = 10;
            this.Txt_Input.Text = "";
            this.Txt_Input.TextChanged += new EventHandler(this.Txt_Input_TextChanged);
            this.Pnl_Top.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Top.Controls.Add(this.Lbl_NoAfter);
            this.Pnl_Top.Controls.Add(this.Nm_NoAfter);
            this.Pnl_Top.Controls.Add(this.Lbl_Hin3);
            this.Pnl_Top.Controls.Add(this.Lbl_YesAfter);
            this.Pnl_Top.Controls.Add(this.Nm_YesAfter);
            this.Pnl_Top.Controls.Add(this.Lbl_Hin2);
            this.Pnl_Top.Controls.Add(this.Lbl_ID);
            this.Pnl_Top.Controls.Add(this.Lbl_Hin1);
            this.Pnl_Top.Controls.Add(this.Nm_ID);
            this.Pnl_Top.Dock = DockStyle.Top;
            this.Pnl_Top.Location = new Point(0, 0);
            this.Pnl_Top.Name = "Pnl_Top";
            this.Pnl_Top.Size = new Size(0x1ff, 0x23);
            this.Pnl_Top.TabIndex = 0x1d;
            this.Lbl_NoAfter.AutoSize = true;
            this.Lbl_NoAfter.Location = new Point(0x16e, 8);
            this.Lbl_NoAfter.Name = "Lbl_NoAfter";
            this.Lbl_NoAfter.Size = new Size(0x2c, 0x11);
            this.Lbl_NoAfter.TabIndex = 30;
            this.Lbl_NoAfter.Text = "挂后：";
            this.Nm_NoAfter.Location = new Point(0x1a0, 6);
            int[] bits = new int[4];
            bits[0] = 0x7a120;
            this.Nm_NoAfter.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.Nm_NoAfter.Minimum = new decimal(bits);
            this.Nm_NoAfter.Name = "Nm_NoAfter";
            this.Nm_NoAfter.Size = new Size(60, 0x17);
            this.Nm_NoAfter.TabIndex = 0x1f;
            bits = new int[4];
            bits[0] = 1;
            this.Nm_NoAfter.Value = new decimal(bits);
            this.Lbl_Hin3.AutoSize = true;
            this.Lbl_Hin3.Location = new Point(0x1e2, 8);
            this.Lbl_Hin3.Name = "Lbl_Hin3";
            this.Lbl_Hin3.Size = new Size(20, 0x11);
            this.Lbl_Hin3.TabIndex = 0x20;
            this.Lbl_Hin3.Text = "局";
            this.Lbl_YesAfter.AutoSize = true;
            this.Lbl_YesAfter.Location = new Point(0xb5, 8);
            this.Lbl_YesAfter.Name = "Lbl_YesAfter";
            this.Lbl_YesAfter.Size = new Size(0x2c, 0x11);
            this.Lbl_YesAfter.TabIndex = 0x1b;
            this.Lbl_YesAfter.Text = "中后：";
            this.Nm_YesAfter.Location = new Point(0xe7, 6);
            bits = new int[4];
            bits[0] = 0x7a120;
            this.Nm_YesAfter.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.Nm_YesAfter.Minimum = new decimal(bits);
            this.Nm_YesAfter.Name = "Nm_YesAfter";
            this.Nm_YesAfter.Size = new Size(60, 0x17);
            this.Nm_YesAfter.TabIndex = 0x1c;
            bits = new int[4];
            bits[0] = 1;
            this.Nm_YesAfter.Value = new decimal(bits);
            this.Lbl_Hin2.AutoSize = true;
            this.Lbl_Hin2.Location = new Point(0x129, 8);
            this.Lbl_Hin2.Name = "Lbl_Hin2";
            this.Lbl_Hin2.Size = new Size(20, 0x11);
            this.Lbl_Hin2.TabIndex = 0x1d;
            this.Lbl_Hin2.Text = "局";
            this.Lbl_ID.AutoSize = true;
            this.Lbl_ID.Location = new Point(5, 8);
            this.Lbl_ID.Name = "Lbl_ID";
            this.Lbl_ID.Size = new Size(0x2c, 0x11);
            this.Lbl_ID.TabIndex = 0x18;
            this.Lbl_ID.Text = "局数：";
            this.Lbl_Hin1.AutoSize = true;
            this.Lbl_Hin1.Location = new Point(0x79, 8);
            this.Lbl_Hin1.Name = "Lbl_Hin1";
            this.Lbl_Hin1.Size = new Size(20, 0x11);
            this.Lbl_Hin1.TabIndex = 0x1a;
            this.Lbl_Hin1.Text = "局";
            this.Nm_ID.Location = new Point(0x37, 6);
            bits = new int[4];
            bits[0] = 0x7a120;
            this.Nm_ID.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.Nm_ID.Minimum = new decimal(bits);
            this.Nm_ID.Name = "Nm_ID";
            this.Nm_ID.Size = new Size(60, 0x17);
            this.Nm_ID.TabIndex = 0x19;
            bits = new int[4];
            bits[0] = 1;
            this.Nm_ID.Value = new decimal(bits);
            this.Pnl_Bottom.Controls.Add(this.Ckb_Clear);
            this.Pnl_Bottom.Controls.Add(this.Ckb_Random);
            this.Pnl_Bottom.Controls.Add(this.Ckb_Cancel);
            this.Pnl_Bottom.Controls.Add(this.Ckb_Ok);
            this.Pnl_Bottom.Controls.Add(this.Nm_Random);
            this.Pnl_Bottom.Controls.Add(this.Lbl_Random);
            this.Pnl_Bottom.Dock = DockStyle.Bottom;
            this.Pnl_Bottom.Location = new Point(0, 0x157);
            this.Pnl_Bottom.Name = "Pnl_Bottom";
            this.Pnl_Bottom.Size = new Size(0x1ff, 0x23);
            this.Pnl_Bottom.TabIndex = 10;
            this.Ckb_Clear.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Ckb_Clear.Appearance = Appearance.Button;
            this.Ckb_Clear.AutoCheck = false;
            this.Ckb_Clear.FlatAppearance.BorderSize = 0;
            this.Ckb_Clear.FlatStyle = FlatStyle.Flat;
            this.Ckb_Clear.Image = Resources.ClearAll;
            this.Ckb_Clear.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Clear.Location = new Point(0xd1, 5);
            this.Ckb_Clear.Name = "Ckb_Clear";
            this.Ckb_Clear.Size = new Size(60, 0x19);
            this.Ckb_Clear.TabIndex = 0xa3;
            this.Ckb_Clear.Text = "清空";
            this.Ckb_Clear.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Clear.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Clear.UseVisualStyleBackColor = true;
            this.Ckb_Clear.Click += new EventHandler(this.Ckb_Clear_Click);
            this.Ckb_Random.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Ckb_Random.Appearance = Appearance.Button;
            this.Ckb_Random.AutoCheck = false;
            this.Ckb_Random.FlatAppearance.BorderSize = 0;
            this.Ckb_Random.FlatStyle = FlatStyle.Flat;
            this.Ckb_Random.Image = Resources.FiveCode;
            this.Ckb_Random.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Random.Location = new Point(0x8f, 5);
            this.Ckb_Random.Name = "Ckb_Random";
            this.Ckb_Random.Size = new Size(60, 0x19);
            this.Ckb_Random.TabIndex = 0xa2;
            this.Ckb_Random.Text = "随机";
            this.Ckb_Random.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Random.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Random.UseVisualStyleBackColor = true;
            this.Ckb_Random.Click += new EventHandler(this.Ckb_Random_Click);
            this.Ckb_Cancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Ckb_Cancel.Appearance = Appearance.Button;
            this.Ckb_Cancel.AutoCheck = false;
            this.Ckb_Cancel.FlatAppearance.BorderSize = 0;
            this.Ckb_Cancel.FlatStyle = FlatStyle.Flat;
            this.Ckb_Cancel.Image = Resources.CancelRound;
            this.Ckb_Cancel.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Cancel.Location = new Point(0x1bb, 5);
            this.Ckb_Cancel.Name = "Ckb_Cancel";
            this.Ckb_Cancel.Size = new Size(60, 0x19);
            this.Ckb_Cancel.TabIndex = 0xa1;
            this.Ckb_Cancel.Text = "取消";
            this.Ckb_Cancel.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Cancel.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Cancel.UseVisualStyleBackColor = true;
            this.Ckb_Cancel.Click += new EventHandler(this.Ckb_Close_Click);
            this.Ckb_Ok.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Ckb_Ok.Appearance = Appearance.Button;
            this.Ckb_Ok.AutoCheck = false;
            this.Ckb_Ok.FlatAppearance.BorderSize = 0;
            this.Ckb_Ok.FlatStyle = FlatStyle.Flat;
            this.Ckb_Ok.Image = Resources.OkRound;
            this.Ckb_Ok.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Ok.Location = new Point(0x179, 5);
            this.Ckb_Ok.Name = "Ckb_Ok";
            this.Ckb_Ok.Size = new Size(60, 0x19);
            this.Ckb_Ok.TabIndex = 160;
            this.Ckb_Ok.Text = "确定";
            this.Ckb_Ok.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Ok.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Ok.UseVisualStyleBackColor = true;
            this.Ckb_Ok.Click += new EventHandler(this.Ckb_Ok_Click);
            this.Nm_Random.Location = new Point(0x4d, 6);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.Nm_Random.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.Nm_Random.Minimum = new decimal(bits);
            this.Nm_Random.Name = "Nm_Random";
            this.Nm_Random.Size = new Size(60, 0x17);
            this.Nm_Random.TabIndex = 0x99;
            bits = new int[4];
            bits[0] = 10;
            this.Nm_Random.Value = new decimal(bits);
            this.Lbl_Random.AutoSize = true;
            this.Lbl_Random.Location = new Point(5, 8);
            this.Lbl_Random.Name = "Lbl_Random";
            this.Lbl_Random.Size = new Size(0x44, 0x11);
            this.Lbl_Random.TabIndex = 0x94;
            this.Lbl_Random.Text = "开始输入：";
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1ff, 0x17a);
            base.Controls.Add(this.Grp_Input);
            base.Controls.Add(this.Pnl_Top);
            base.Controls.Add(this.Pnl_Bottom);
            base.MaximizeBox = true;
            base.MinimizeBox = true;
            base.Name = "FrmInputCode";
            base.Load += new EventHandler(this.FrmInputCode_Load);
            this.Grp_Input.ResumeLayout(false);
            this.Pnl_Top.ResumeLayout(false);
            this.Pnl_Top.PerformLayout();
            this.Nm_NoAfter.EndInit();
            this.Nm_YesAfter.EndInit();
            this.Nm_ID.EndInit();
            this.Pnl_Bottom.ResumeLayout(false);
            this.Pnl_Bottom.PerformLayout();
            this.Nm_Random.EndInit();
            base.ResumeLayout(false);
        }

        private void Txt_Input_TextChanged(object sender, EventArgs e)
        {
            string pErrorHint = "";
            int num = CommFunc.GetBetsCodeCount(CommFunc.FilterNumber(CommFunc.ReplaceText(this.Txt_Input.Text, this.PlayInfo, true), this.PlayInfo.CodeCount, this.PlayInfo.Play, ref pErrorHint), this.PlayInfo.Play, null);
            this.Grp_Input.Text = $"注数：{num} ";
            this.Ckb_Ok.Enabled = num > 0;
        }
    }
}

