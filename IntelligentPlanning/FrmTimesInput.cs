namespace IntelligentPlanning
{
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FrmTimesInput : ExForm
    {
        private ComboBox Cbb_NoOtherFN;
        private ComboBox Cbb_YesOtherFN;
        private CheckBox Ckb_Cancel;
        private CheckBox Ckb_More;
        private CheckBox Ckb_NoJK;
        private CheckBox Ckb_NoOtherFN;
        private CheckBox Ckb_Ok;
        private CheckBox Ckb_YesJK;
        private CheckBox Ckb_YesOtherFN;
        private IContainer components = null;
        private Label Lbl_Hin1;
        private Label Lbl_Hin2;
        private Label Lbl_Hin3;
        private Label Lbl_ID;
        private Label Lbl_NoAfter;
        private Label Lbl_Times;
        private Label Lbl_YesAfter;
        private NumericUpDown Nm_ID;
        private NumericUpDown Nm_NoAfter;
        private NumericUpDown Nm_Times;
        private NumericUpDown Nm_YesAfter;
        public static ConfigurationStatus.TimesScheme OutValue;
        private Panel Pnl_Bottom;
        private Panel Pnl_Top1;
        private Panel Pnl_Top2;
        private Panel Pnl_Top3;

        public FrmTimesInput(ConfigurationStatus.TimesScheme pInput, bool pIsAdd)
        {
            this.InitializeComponent();
            CommFunc.SetComboBoxList(this.Cbb_YesOtherFN, AppInfo.SchemeList);
            CommFunc.SetComboBoxList(this.Cbb_NoOtherFN, AppInfo.SchemeList);
            this.Nm_ID.Value = pInput.ID;
            this.Nm_Times.Value = pInput.Times;
            this.Nm_YesAfter.Value = pInput.YesAfter;
            this.Nm_NoAfter.Value = pInput.NoAfter;
            this.Ckb_YesJK.Checked = pInput.YesJK;
            this.Ckb_NoJK.Checked = pInput.NoJK;
            this.Ckb_YesOtherFN.Checked = pInput.YesOtherFNSelect;
            this.Ckb_NoOtherFN.Checked = pInput.NoOtherFNSelect;
            CommFunc.SetComboBoxSelectedIndex(this.Cbb_YesOtherFN, pInput.YesOtherFNValue);
            CommFunc.SetComboBoxSelectedIndex(this.Cbb_NoOtherFN, pInput.NoOtherFNValue);
            this.Nm_ID.Enabled = pIsAdd;
            List<Control> list = new List<Control> {
                this
            };
            base.ControlList = list;
            this.Text = pIsAdd ? "添加倍投" : "修改倍投";
            List<Control> list2 = new List<Control> {
                this.Ckb_More
            };
            base.ControlList = list2;
            List<CheckBox> list3 = new List<CheckBox> {
                this.Ckb_More,
                this.Ckb_Ok,
                this.Ckb_Cancel
            };
            base.CheckBoxList = list3;
        }

        private void BeautifyInterface()
        {
            if (AppInfo.Account.Configuration.Beautify)
            {
                List<Control> pControlList = new List<Control> {
                    this.Pnl_Bottom
                };
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list2 = new List<Control> {
                    this.Pnl_Top1,
                    this.Pnl_Top2,
                    this.Pnl_Top3
                };
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Lbl_ID,
                    this.Lbl_Hin1,
                    this.Lbl_Times,
                    this.Lbl_YesAfter,
                    this.Lbl_Hin2,
                    this.Ckb_YesJK,
                    this.Ckb_YesOtherFN,
                    this.Lbl_NoAfter,
                    this.Lbl_Hin3,
                    this.Ckb_NoJK,
                    this.Ckb_NoOtherFN,
                    this.Ckb_More,
                    this.Ckb_Ok,
                    this.Ckb_Cancel
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
                List<ComboBox> pComboBoxList = new List<ComboBox> {
                    this.Cbb_YesOtherFN,
                    this.Cbb_NoOtherFN
                };
                CommFunc.BeautifyComboBox(pComboBoxList);
                CommFunc.CheckBox_CheckedChanged(this.Ckb_More, null);
            }
        }

        private void Ckb_Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void Ckb_More_CheckedChanged(object sender, EventArgs e)
        {
            base.Width = this.Ckb_More.Checked ? 0x19c : 280;
            this.Ckb_YesJK.Visible = this.Ckb_NoJK.Visible = this.Ckb_YesOtherFN.Visible = this.Ckb_NoOtherFN.Visible = this.Cbb_YesOtherFN.Visible = this.Cbb_NoOtherFN.Visible = this.Ckb_More.Checked;
        }

        private void Ckb_NoOtherFN_CheckedChanged(object sender, EventArgs e)
        {
            this.Cbb_NoOtherFN.Enabled = this.Ckb_NoOtherFN.Checked;
        }

        private void Ckb_Ok_Click(object sender, EventArgs e)
        {
            OutValue = new ConfigurationStatus.TimesScheme();
            OutValue.ID = Convert.ToInt32(this.Nm_ID.Value);
            OutValue.Times = Convert.ToInt32(this.Nm_Times.Value);
            OutValue.YesAfter = Convert.ToInt32(this.Nm_YesAfter.Value);
            OutValue.NoAfter = Convert.ToInt32(this.Nm_NoAfter.Value);
            OutValue.YesJK = this.Ckb_YesJK.Checked;
            OutValue.NoJK = this.Ckb_NoJK.Checked;
            OutValue.YesOtherFNSelect = this.Ckb_YesOtherFN.Checked;
            OutValue.NoOtherFNSelect = this.Ckb_NoOtherFN.Checked;
            OutValue.YesOtherFNValue = this.Cbb_YesOtherFN.Text;
            OutValue.NoOtherFNValue = this.Cbb_NoOtherFN.Text;
            base.DialogResult = DialogResult.OK;
        }

        private void Ckb_YesOtherFN_CheckedChanged(object sender, EventArgs e)
        {
            this.Cbb_YesOtherFN.Enabled = this.Ckb_YesOtherFN.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FrmTimesInput_Load(object sender, EventArgs e)
        {
            this.BeautifyInterface();
        }

        private void InitializeComponent()
        {
            this.Lbl_Hin1 = new Label();
            this.Nm_ID = new NumericUpDown();
            this.Lbl_ID = new Label();
            this.Lbl_Hin3 = new Label();
            this.Nm_NoAfter = new NumericUpDown();
            this.Lbl_NoAfter = new Label();
            this.Lbl_Hin2 = new Label();
            this.Nm_YesAfter = new NumericUpDown();
            this.Lbl_YesAfter = new Label();
            this.Nm_Times = new NumericUpDown();
            this.Lbl_Times = new Label();
            this.Pnl_Bottom = new Panel();
            this.Ckb_Cancel = new CheckBox();
            this.Ckb_Ok = new CheckBox();
            this.Ckb_More = new CheckBox();
            this.Ckb_YesJK = new CheckBox();
            this.Pnl_Top1 = new Panel();
            this.Pnl_Top2 = new Panel();
            this.Cbb_YesOtherFN = new ComboBox();
            this.Ckb_YesOtherFN = new CheckBox();
            this.Pnl_Top3 = new Panel();
            this.Cbb_NoOtherFN = new ComboBox();
            this.Ckb_NoOtherFN = new CheckBox();
            this.Ckb_NoJK = new CheckBox();
            this.Nm_ID.BeginInit();
            this.Nm_NoAfter.BeginInit();
            this.Nm_YesAfter.BeginInit();
            this.Nm_Times.BeginInit();
            this.Pnl_Bottom.SuspendLayout();
            this.Pnl_Top1.SuspendLayout();
            this.Pnl_Top2.SuspendLayout();
            this.Pnl_Top3.SuspendLayout();
            base.SuspendLayout();
            this.Lbl_Hin1.AutoSize = true;
            this.Lbl_Hin1.Location = new Point(0x79, 8);
            this.Lbl_Hin1.Name = "Lbl_Hin1";
            this.Lbl_Hin1.Size = new Size(20, 0x11);
            this.Lbl_Hin1.TabIndex = 0x1a;
            this.Lbl_Hin1.Text = "局";
            this.Nm_ID.Location = new Point(0x37, 6);
            int[] bits = new int[4];
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
            this.Lbl_ID.AutoSize = true;
            this.Lbl_ID.Location = new Point(5, 8);
            this.Lbl_ID.Name = "Lbl_ID";
            this.Lbl_ID.Size = new Size(0x2c, 0x11);
            this.Lbl_ID.TabIndex = 0x18;
            this.Lbl_ID.Text = "局数：";
            this.Lbl_Hin3.AutoSize = true;
            this.Lbl_Hin3.Location = new Point(0x79, 8);
            this.Lbl_Hin3.Name = "Lbl_Hin3";
            this.Lbl_Hin3.Size = new Size(20, 0x11);
            this.Lbl_Hin3.TabIndex = 20;
            this.Lbl_Hin3.Text = "局";
            this.Nm_NoAfter.Location = new Point(0x37, 6);
            bits = new int[4];
            bits[0] = 0x7a120;
            this.Nm_NoAfter.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.Nm_NoAfter.Minimum = new decimal(bits);
            this.Nm_NoAfter.Name = "Nm_NoAfter";
            this.Nm_NoAfter.Size = new Size(60, 0x17);
            this.Nm_NoAfter.TabIndex = 0x13;
            bits = new int[4];
            bits[0] = 1;
            this.Nm_NoAfter.Value = new decimal(bits);
            this.Lbl_NoAfter.AutoSize = true;
            this.Lbl_NoAfter.Location = new Point(5, 8);
            this.Lbl_NoAfter.Name = "Lbl_NoAfter";
            this.Lbl_NoAfter.Size = new Size(0x2c, 0x11);
            this.Lbl_NoAfter.TabIndex = 0x12;
            this.Lbl_NoAfter.Text = "挂后：";
            this.Lbl_Hin2.AutoSize = true;
            this.Lbl_Hin2.Location = new Point(0x79, 8);
            this.Lbl_Hin2.Name = "Lbl_Hin2";
            this.Lbl_Hin2.Size = new Size(20, 0x11);
            this.Lbl_Hin2.TabIndex = 0x11;
            this.Lbl_Hin2.Text = "局";
            this.Nm_YesAfter.Location = new Point(0x37, 6);
            bits = new int[4];
            bits[0] = 0x7a120;
            this.Nm_YesAfter.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.Nm_YesAfter.Minimum = new decimal(bits);
            this.Nm_YesAfter.Name = "Nm_YesAfter";
            this.Nm_YesAfter.Size = new Size(60, 0x17);
            this.Nm_YesAfter.TabIndex = 0x10;
            bits = new int[4];
            bits[0] = 1;
            this.Nm_YesAfter.Value = new decimal(bits);
            this.Lbl_YesAfter.AutoSize = true;
            this.Lbl_YesAfter.Location = new Point(5, 8);
            this.Lbl_YesAfter.Name = "Lbl_YesAfter";
            this.Lbl_YesAfter.Size = new Size(0x2c, 0x11);
            this.Lbl_YesAfter.TabIndex = 15;
            this.Lbl_YesAfter.Text = "中后：";
            this.Nm_Times.Location = new Point(0xcb, 6);
            bits = new int[4];
            bits[0] = 0x7a120;
            this.Nm_Times.Maximum = new decimal(bits);
            this.Nm_Times.Name = "Nm_Times";
            this.Nm_Times.Size = new Size(60, 0x17);
            this.Nm_Times.TabIndex = 14;
            bits = new int[4];
            bits[0] = 1;
            this.Nm_Times.Value = new decimal(bits);
            this.Lbl_Times.AutoSize = true;
            this.Lbl_Times.Location = new Point(0x99, 8);
            this.Lbl_Times.Name = "Lbl_Times";
            this.Lbl_Times.Size = new Size(0x2c, 0x11);
            this.Lbl_Times.TabIndex = 13;
            this.Lbl_Times.Text = "倍数：";
            this.Pnl_Bottom.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Bottom.Controls.Add(this.Ckb_Cancel);
            this.Pnl_Bottom.Controls.Add(this.Ckb_Ok);
            this.Pnl_Bottom.Controls.Add(this.Ckb_More);
            this.Pnl_Bottom.Dock = DockStyle.Bottom;
            this.Pnl_Bottom.Location = new Point(0, 0x69);
            this.Pnl_Bottom.Name = "Pnl_Bottom";
            this.Pnl_Bottom.Size = new Size(0x112, 0x23);
            this.Pnl_Bottom.TabIndex = 12;
            this.Ckb_Cancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Ckb_Cancel.Appearance = Appearance.Button;
            this.Ckb_Cancel.AutoCheck = false;
            this.Ckb_Cancel.FlatAppearance.BorderSize = 0;
            this.Ckb_Cancel.FlatStyle = FlatStyle.Flat;
            this.Ckb_Cancel.Image = Resources.CancelRound;
            this.Ckb_Cancel.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Cancel.Location = new Point(0xce, 4);
            this.Ckb_Cancel.Name = "Ckb_Cancel";
            this.Ckb_Cancel.Size = new Size(60, 0x19);
            this.Ckb_Cancel.TabIndex = 210;
            this.Ckb_Cancel.Text = "取消";
            this.Ckb_Cancel.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Cancel.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Cancel.UseVisualStyleBackColor = true;
            this.Ckb_Cancel.Click += new EventHandler(this.Ckb_Cancel_Click);
            this.Ckb_Ok.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Ckb_Ok.Appearance = Appearance.Button;
            this.Ckb_Ok.AutoCheck = false;
            this.Ckb_Ok.FlatAppearance.BorderSize = 0;
            this.Ckb_Ok.FlatStyle = FlatStyle.Flat;
            this.Ckb_Ok.Image = Resources.OkRound;
            this.Ckb_Ok.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Ok.Location = new Point(140, 4);
            this.Ckb_Ok.Name = "Ckb_Ok";
            this.Ckb_Ok.Size = new Size(60, 0x19);
            this.Ckb_Ok.TabIndex = 0xd1;
            this.Ckb_Ok.Text = "确定";
            this.Ckb_Ok.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Ok.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Ok.UseVisualStyleBackColor = true;
            this.Ckb_Ok.Click += new EventHandler(this.Ckb_Ok_Click);
            this.Ckb_More.Appearance = Appearance.Button;
            this.Ckb_More.FlatAppearance.BorderSize = 0;
            this.Ckb_More.FlatStyle = FlatStyle.Flat;
            this.Ckb_More.Image = Resources.BtnMore2;
            this.Ckb_More.Location = new Point(5, 4);
            this.Ckb_More.Name = "Ckb_More";
            this.Ckb_More.Size = new Size(80, 0x19);
            this.Ckb_More.TabIndex = 0xd0;
            this.Ckb_More.Text = "更多设置";
            this.Ckb_More.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_More.UseVisualStyleBackColor = true;
            this.Ckb_More.CheckedChanged += new EventHandler(this.Ckb_More_CheckedChanged);
            this.Ckb_YesJK.AutoSize = true;
            this.Ckb_YesJK.Location = new Point(0x98, 7);
            this.Ckb_YesJK.Name = "Ckb_YesJK";
            this.Ckb_YesJK.Size = new Size(0x4b, 0x15);
            this.Ckb_YesJK.TabIndex = 0x1b;
            this.Ckb_YesJK.Text = "重新监控";
            this.Ckb_YesJK.UseVisualStyleBackColor = true;
            this.Ckb_YesJK.Visible = false;
            this.Pnl_Top1.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Top1.Controls.Add(this.Lbl_ID);
            this.Pnl_Top1.Controls.Add(this.Lbl_Times);
            this.Pnl_Top1.Controls.Add(this.Lbl_Hin1);
            this.Pnl_Top1.Controls.Add(this.Nm_Times);
            this.Pnl_Top1.Controls.Add(this.Nm_ID);
            this.Pnl_Top1.Dock = DockStyle.Top;
            this.Pnl_Top1.Location = new Point(0, 0);
            this.Pnl_Top1.Name = "Pnl_Top1";
            this.Pnl_Top1.Size = new Size(0x112, 0x23);
            this.Pnl_Top1.TabIndex = 0x1c;
            this.Pnl_Top2.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Top2.Controls.Add(this.Cbb_YesOtherFN);
            this.Pnl_Top2.Controls.Add(this.Ckb_YesOtherFN);
            this.Pnl_Top2.Controls.Add(this.Lbl_YesAfter);
            this.Pnl_Top2.Controls.Add(this.Nm_YesAfter);
            this.Pnl_Top2.Controls.Add(this.Ckb_YesJK);
            this.Pnl_Top2.Controls.Add(this.Lbl_Hin2);
            this.Pnl_Top2.Dock = DockStyle.Top;
            this.Pnl_Top2.Location = new Point(0, 0x23);
            this.Pnl_Top2.Name = "Pnl_Top2";
            this.Pnl_Top2.Size = new Size(0x112, 0x23);
            this.Pnl_Top2.TabIndex = 0x1d;
            this.Cbb_YesOtherFN.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_YesOtherFN.Enabled = false;
            this.Cbb_YesOtherFN.FormattingEnabled = true;
            this.Cbb_YesOtherFN.Location = new Point(0x12e, 5);
            this.Cbb_YesOtherFN.Name = "Cbb_YesOtherFN";
            this.Cbb_YesOtherFN.Size = new Size(0x5f, 0x19);
            this.Cbb_YesOtherFN.TabIndex = 0x12e;
            this.Cbb_YesOtherFN.Visible = false;
            this.Ckb_YesOtherFN.AutoSize = true;
            this.Ckb_YesOtherFN.Location = new Point(0xe9, 7);
            this.Ckb_YesOtherFN.Name = "Ckb_YesOtherFN";
            this.Ckb_YesOtherFN.Size = new Size(0x3f, 0x15);
            this.Ckb_YesOtherFN.TabIndex = 0x1c;
            this.Ckb_YesOtherFN.Text = "跳到：";
            this.Ckb_YesOtherFN.UseVisualStyleBackColor = true;
            this.Ckb_YesOtherFN.Visible = false;
            this.Ckb_YesOtherFN.CheckedChanged += new EventHandler(this.Ckb_YesOtherFN_CheckedChanged);
            this.Pnl_Top3.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Top3.Controls.Add(this.Cbb_NoOtherFN);
            this.Pnl_Top3.Controls.Add(this.Ckb_NoOtherFN);
            this.Pnl_Top3.Controls.Add(this.Ckb_NoJK);
            this.Pnl_Top3.Controls.Add(this.Lbl_NoAfter);
            this.Pnl_Top3.Controls.Add(this.Nm_NoAfter);
            this.Pnl_Top3.Controls.Add(this.Lbl_Hin3);
            this.Pnl_Top3.Dock = DockStyle.Top;
            this.Pnl_Top3.Location = new Point(0, 70);
            this.Pnl_Top3.Name = "Pnl_Top3";
            this.Pnl_Top3.Size = new Size(0x112, 0x23);
            this.Pnl_Top3.TabIndex = 30;
            this.Cbb_NoOtherFN.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_NoOtherFN.Enabled = false;
            this.Cbb_NoOtherFN.FormattingEnabled = true;
            this.Cbb_NoOtherFN.Location = new Point(0x12e, 5);
            this.Cbb_NoOtherFN.Name = "Cbb_NoOtherFN";
            this.Cbb_NoOtherFN.Size = new Size(0x5f, 0x19);
            this.Cbb_NoOtherFN.TabIndex = 0x12e;
            this.Cbb_NoOtherFN.Visible = false;
            this.Ckb_NoOtherFN.AutoSize = true;
            this.Ckb_NoOtherFN.Location = new Point(0xe9, 7);
            this.Ckb_NoOtherFN.Name = "Ckb_NoOtherFN";
            this.Ckb_NoOtherFN.Size = new Size(0x3f, 0x15);
            this.Ckb_NoOtherFN.TabIndex = 0x1c;
            this.Ckb_NoOtherFN.Text = "跳到：";
            this.Ckb_NoOtherFN.UseVisualStyleBackColor = true;
            this.Ckb_NoOtherFN.Visible = false;
            this.Ckb_NoOtherFN.CheckedChanged += new EventHandler(this.Ckb_NoOtherFN_CheckedChanged);
            this.Ckb_NoJK.AutoSize = true;
            this.Ckb_NoJK.Location = new Point(0x98, 7);
            this.Ckb_NoJK.Name = "Ckb_NoJK";
            this.Ckb_NoJK.Size = new Size(0x4b, 0x15);
            this.Ckb_NoJK.TabIndex = 0x1b;
            this.Ckb_NoJK.Text = "重新监控";
            this.Ckb_NoJK.UseVisualStyleBackColor = true;
            this.Ckb_NoJK.Visible = false;
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x112, 140);
            base.Controls.Add(this.Pnl_Top3);
            base.Controls.Add(this.Pnl_Top2);
            base.Controls.Add(this.Pnl_Top1);
            base.Controls.Add(this.Pnl_Bottom);
            base.Name = "FrmTimesInput";
            base.Load += new EventHandler(this.FrmTimesInput_Load);
            this.Nm_ID.EndInit();
            this.Nm_NoAfter.EndInit();
            this.Nm_YesAfter.EndInit();
            this.Nm_Times.EndInit();
            this.Pnl_Bottom.ResumeLayout(false);
            this.Pnl_Top1.ResumeLayout(false);
            this.Pnl_Top1.PerformLayout();
            this.Pnl_Top2.ResumeLayout(false);
            this.Pnl_Top2.PerformLayout();
            this.Pnl_Top3.ResumeLayout(false);
            this.Pnl_Top3.PerformLayout();
            base.ResumeLayout(false);
        }
    }
}

