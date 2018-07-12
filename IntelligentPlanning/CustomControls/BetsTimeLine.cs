namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class BetsTimeLine : UserControl
    {
        private ComboBox Cbb_DJSTimeType;
        private ComboBox Cbb_FWTime2Type;
        private ComboBox Cbb_TimeType;
        private CheckBox Ckb_FWTime1;
        private CheckBox Ckb_FWTime2;
        private CheckBox Ckb_Time;
        private IContainer components = null;
        private DateTimePicker Dtp_DJSTime;
        private DateTimePicker Dtp_FWTime1;
        private DateTimePicker Dtp_FWTime2;
        private Label Lbl_DJSTime1;
        private Label Lbl_DJSTime2;
        private Label Lbl_TimeHint;
        private Panel Pnl_Main;
        private Panel Pnl_TimeDJS;
        private Panel Pnl_TimeFW;

        public BetsTimeLine()
        {
            this.InitializeComponent();
        }

        private void BeautifyInterface()
        {
            if (AppInfo.Account.Configuration.Beautify)
            {
                List<Control> pControlList = new List<Control>();
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list2 = new List<Control>();
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Ckb_Time,
                    this.Lbl_TimeHint,
                    this.Ckb_FWTime1,
                    this.Ckb_FWTime2
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
                List<ComboBox> pComboBoxList = new List<ComboBox> {
                    this.Cbb_TimeType,
                    this.Cbb_FWTime2Type
                };
                CommFunc.BeautifyComboBox(pComboBoxList);
            }
        }

        private void BetsTimeLine_Load(object sender, EventArgs e)
        {
            this.BeautifyInterface();
        }

        private void Cbb_TimeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = this.Cbb_TimeType.SelectedIndex;
            this.Pnl_TimeFW.Visible = selectedIndex == 0;
            this.Pnl_TimeDJS.Visible = selectedIndex == 1;
        }

        private void Ckb_FWTime1_CheckedChanged(object sender, EventArgs e)
        {
            this.Dtp_FWTime1.Enabled = this.Ckb_FWTime1.Checked && this.Ckb_Time.Checked;
        }

        private void Ckb_FWTime2_CheckedChanged(object sender, EventArgs e)
        {
            this.Dtp_FWTime2.Enabled = this.Cbb_FWTime2Type.Enabled = this.Ckb_FWTime2.Checked && this.Ckb_Time.Checked;
        }

        private void Ckb_Time_CheckedChanged(object sender, EventArgs e)
        {
            this.Cbb_TimeType.Enabled = this.Lbl_TimeHint.Enabled = this.Pnl_TimeFW.Enabled = this.Pnl_TimeDJS.Enabled = this.Ckb_Time.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void GetControlValue(ref ConfigurationStatus.FNBase pInfo)
        {
            pInfo.BetsTime = this.Ckb_Time.Checked;
            pInfo.BetsTimeType = this.GetTimeType();
            pInfo.FWBeginTimeSelect = this.Ckb_FWTime1.Checked;
            pInfo.FWBeginTimeValue = this.Dtp_FWTime1.Value.ToString("HH:mm:ss");
            pInfo.FWEndTimeSelect = this.Ckb_FWTime2.Checked;
            pInfo.FWEndTimeValue = this.Dtp_FWTime2.Value.ToString("HH:mm:ss");
            pInfo.FWEndType = this.GetFWEndType();
            pInfo.DJSEndTimeValue = this.Dtp_DJSTime.Value.ToString("HH:mm:ss");
            pInfo.DJSEndType = this.GetDJSEndType();
        }

        private ConfigurationStatus.StopBetsType GetDJSEndType() => 
            ((ConfigurationStatus.StopBetsType) this.Cbb_DJSTimeType.SelectedIndex);

        private ConfigurationStatus.StopBetsType GetFWEndType() => 
            ((ConfigurationStatus.StopBetsType) this.Cbb_FWTime2Type.SelectedIndex);

        private ConfigurationStatus.TimeType GetTimeType() => 
            ((ConfigurationStatus.TimeType) this.Cbb_TimeType.SelectedIndex);

        private void InitializeComponent()
        {
            this.Pnl_Main = new Panel();
            this.Pnl_TimeFW = new Panel();
            this.Cbb_FWTime2Type = new ComboBox();
            this.Ckb_FWTime2 = new CheckBox();
            this.Dtp_FWTime2 = new DateTimePicker();
            this.Ckb_FWTime1 = new CheckBox();
            this.Dtp_FWTime1 = new DateTimePicker();
            this.Pnl_TimeDJS = new Panel();
            this.Lbl_DJSTime2 = new Label();
            this.Cbb_DJSTimeType = new ComboBox();
            this.Dtp_DJSTime = new DateTimePicker();
            this.Lbl_DJSTime1 = new Label();
            this.Lbl_TimeHint = new Label();
            this.Cbb_TimeType = new ComboBox();
            this.Ckb_Time = new CheckBox();
            this.Pnl_Main.SuspendLayout();
            this.Pnl_TimeFW.SuspendLayout();
            this.Pnl_TimeDJS.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_Main.Controls.Add(this.Pnl_TimeFW);
            this.Pnl_Main.Controls.Add(this.Pnl_TimeDJS);
            this.Pnl_Main.Controls.Add(this.Lbl_TimeHint);
            this.Pnl_Main.Controls.Add(this.Cbb_TimeType);
            this.Pnl_Main.Controls.Add(this.Ckb_Time);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(520, 0x23);
            this.Pnl_Main.TabIndex = 0;
            this.Pnl_TimeFW.Controls.Add(this.Cbb_FWTime2Type);
            this.Pnl_TimeFW.Controls.Add(this.Ckb_FWTime2);
            this.Pnl_TimeFW.Controls.Add(this.Dtp_FWTime2);
            this.Pnl_TimeFW.Controls.Add(this.Ckb_FWTime1);
            this.Pnl_TimeFW.Controls.Add(this.Dtp_FWTime1);
            this.Pnl_TimeFW.Enabled = false;
            this.Pnl_TimeFW.Location = new Point(0xa8, 0);
            this.Pnl_TimeFW.Name = "Pnl_TimeFW";
            this.Pnl_TimeFW.Size = new Size(350, 0x23);
            this.Pnl_TimeFW.TabIndex = 0x143;
            this.Cbb_FWTime2Type.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_FWTime2Type.Enabled = false;
            this.Cbb_FWTime2Type.FormattingEnabled = true;
            this.Cbb_FWTime2Type.Items.AddRange(new object[] { "投注", "新增投注" });
            this.Cbb_FWTime2Type.Location = new Point(0xcc, 6);
            this.Cbb_FWTime2Type.Name = "Cbb_FWTime2Type";
            this.Cbb_FWTime2Type.Size = new Size(80, 0x19);
            this.Cbb_FWTime2Type.TabIndex = 0x14c;
            this.Ckb_FWTime2.AutoSize = true;
            this.Ckb_FWTime2.Location = new Point(0x93, 8);
            this.Ckb_FWTime2.Name = "Ckb_FWTime2";
            this.Ckb_FWTime2.Size = new Size(0x33, 0x15);
            this.Ckb_FWTime2.TabIndex = 330;
            this.Ckb_FWTime2.Text = "停止";
            this.Ckb_FWTime2.UseVisualStyleBackColor = true;
            this.Ckb_FWTime2.CheckedChanged += new EventHandler(this.Ckb_FWTime2_CheckedChanged);
            this.Dtp_FWTime2.CustomFormat = "HH:mm";
            this.Dtp_FWTime2.Enabled = false;
            this.Dtp_FWTime2.Format = DateTimePickerFormat.Custom;
            this.Dtp_FWTime2.Location = new Point(0x120, 7);
            this.Dtp_FWTime2.Name = "Dtp_FWTime2";
            this.Dtp_FWTime2.ShowUpDown = true;
            this.Dtp_FWTime2.Size = new Size(60, 0x17);
            this.Dtp_FWTime2.TabIndex = 0x14b;
            this.Dtp_FWTime2.Value = new DateTime(0x7df, 7, 20, 0x15, 0x20, 0, 0);
            this.Ckb_FWTime1.AutoSize = true;
            this.Ckb_FWTime1.Location = new Point(3, 8);
            this.Ckb_FWTime1.Name = "Ckb_FWTime1";
            this.Ckb_FWTime1.Size = new Size(0x4b, 0x15);
            this.Ckb_FWTime1.TabIndex = 0x148;
            this.Ckb_FWTime1.Text = "开始投注";
            this.Ckb_FWTime1.UseVisualStyleBackColor = true;
            this.Ckb_FWTime1.CheckedChanged += new EventHandler(this.Ckb_FWTime1_CheckedChanged);
            this.Dtp_FWTime1.CustomFormat = "HH:mm";
            this.Dtp_FWTime1.Enabled = false;
            this.Dtp_FWTime1.Format = DateTimePickerFormat.Custom;
            this.Dtp_FWTime1.Location = new Point(0x52, 7);
            this.Dtp_FWTime1.Name = "Dtp_FWTime1";
            this.Dtp_FWTime1.ShowUpDown = true;
            this.Dtp_FWTime1.Size = new Size(60, 0x17);
            this.Dtp_FWTime1.TabIndex = 0x149;
            this.Dtp_FWTime1.Value = new DateTime(0x7df, 7, 20, 9, 1, 0, 0);
            this.Pnl_TimeDJS.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Pnl_TimeDJS.Controls.Add(this.Lbl_DJSTime2);
            this.Pnl_TimeDJS.Controls.Add(this.Cbb_DJSTimeType);
            this.Pnl_TimeDJS.Controls.Add(this.Dtp_DJSTime);
            this.Pnl_TimeDJS.Controls.Add(this.Lbl_DJSTime1);
            this.Pnl_TimeDJS.Enabled = false;
            this.Pnl_TimeDJS.Location = new Point(0xa8, 0);
            this.Pnl_TimeDJS.Name = "Pnl_TimeDJS";
            this.Pnl_TimeDJS.Size = new Size(350, 0x23);
            this.Pnl_TimeDJS.TabIndex = 0x144;
            this.Lbl_DJSTime2.AutoSize = true;
            this.Lbl_DJSTime2.Location = new Point(0x65, 10);
            this.Lbl_DJSTime2.Name = "Lbl_DJSTime2";
            this.Lbl_DJSTime2.Size = new Size(0x2c, 0x11);
            this.Lbl_DJSTime2.TabIndex = 0x14c;
            this.Lbl_DJSTime2.Text = "后停止";
            this.Cbb_DJSTimeType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_DJSTimeType.FormattingEnabled = true;
            this.Cbb_DJSTimeType.Items.AddRange(new object[] { "投注", "新增投注" });
            this.Cbb_DJSTimeType.Location = new Point(0x97, 6);
            this.Cbb_DJSTimeType.Name = "Cbb_DJSTimeType";
            this.Cbb_DJSTimeType.Size = new Size(80, 0x19);
            this.Cbb_DJSTimeType.TabIndex = 0x14b;
            this.Dtp_DJSTime.CustomFormat = "HH:mm";
            this.Dtp_DJSTime.Format = DateTimePickerFormat.Custom;
            this.Dtp_DJSTime.Location = new Point(0x25, 7);
            this.Dtp_DJSTime.Name = "Dtp_DJSTime";
            this.Dtp_DJSTime.ShowUpDown = true;
            this.Dtp_DJSTime.Size = new Size(60, 0x17);
            this.Dtp_DJSTime.TabIndex = 330;
            this.Dtp_DJSTime.Value = new DateTime(0x7df, 7, 20, 2, 0, 0, 0);
            this.Lbl_DJSTime1.AutoSize = true;
            this.Lbl_DJSTime1.Location = new Point(1, 9);
            this.Lbl_DJSTime1.Name = "Lbl_DJSTime1";
            this.Lbl_DJSTime1.Size = new Size(0x20, 0x11);
            this.Lbl_DJSTime1.TabIndex = 0;
            this.Lbl_DJSTime1.Text = "投注";
            this.Lbl_TimeHint.AutoSize = true;
            this.Lbl_TimeHint.Enabled = false;
            this.Lbl_TimeHint.Location = new Point(0x8f, 9);
            this.Lbl_TimeHint.Name = "Lbl_TimeHint";
            this.Lbl_TimeHint.Size = new Size(20, 0x11);
            this.Lbl_TimeHint.TabIndex = 0x142;
            this.Lbl_TimeHint.Text = "内";
            this.Cbb_TimeType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_TimeType.Enabled = false;
            this.Cbb_TimeType.FormattingEnabled = true;
            this.Cbb_TimeType.Items.AddRange(new object[] { "时间范围", "倒计时" });
            this.Cbb_TimeType.Location = new Point(0x39, 6);
            this.Cbb_TimeType.Name = "Cbb_TimeType";
            this.Cbb_TimeType.Size = new Size(80, 0x19);
            this.Cbb_TimeType.TabIndex = 0x141;
            this.Cbb_TimeType.SelectedIndexChanged += new EventHandler(this.Cbb_TimeType_SelectedIndexChanged);
            this.Ckb_Time.AutoSize = true;
            this.Ckb_Time.Location = new Point(5, 8);
            this.Ckb_Time.Name = "Ckb_Time";
            this.Ckb_Time.Size = new Size(0x33, 0x15);
            this.Ckb_Time.TabIndex = 0;
            this.Ckb_Time.Text = "方案";
            this.Ckb_Time.UseVisualStyleBackColor = true;
            this.Ckb_Time.CheckedChanged += new EventHandler(this.Ckb_Time_CheckedChanged);
            base.AutoScaleDimensions = new SizeF(96f, 96f);
            base.AutoScaleMode = AutoScaleMode.Dpi;
            base.Controls.Add(this.Pnl_Main);
            this.Font = new Font("微软雅黑", 9f);
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "BetsTimeLine";
            base.Size = new Size(520, 0x23);
            base.Load += new EventHandler(this.BetsTimeLine_Load);
            this.Pnl_Main.ResumeLayout(false);
            this.Pnl_Main.PerformLayout();
            this.Pnl_TimeFW.ResumeLayout(false);
            this.Pnl_TimeFW.PerformLayout();
            this.Pnl_TimeDJS.ResumeLayout(false);
            this.Pnl_TimeDJS.PerformLayout();
            base.ResumeLayout(false);
        }

        public void SetControlValue(ConfigurationStatus.FNBase pInfo)
        {
            this.Ckb_Time.Checked = pInfo.BetsTime;
            this.Cbb_TimeType.SelectedIndex = (int) pInfo.BetsTimeType;
            this.Ckb_FWTime1.Checked = pInfo.FWBeginTimeSelect;
            this.Dtp_FWTime1.Value = Convert.ToDateTime(pInfo.FWBeginTimeValue);
            this.Ckb_FWTime2.Checked = pInfo.FWEndTimeSelect;
            this.Dtp_FWTime2.Value = Convert.ToDateTime(pInfo.FWEndTimeValue);
            this.Cbb_FWTime2Type.SelectedIndex = (int) pInfo.FWEndType;
            this.Dtp_DJSTime.Value = Convert.ToDateTime(pInfo.DJSEndTimeValue);
            this.Cbb_DJSTimeType.SelectedIndex = (int) pInfo.DJSEndType;
        }
    }
}

