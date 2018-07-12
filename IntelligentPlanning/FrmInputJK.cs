namespace IntelligentPlanning
{
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FrmInputJK : ExForm
    {
        private CheckBox Ckb_Cancel;
        private CheckBox Ckb_Ok;
        private IContainer components = null;
        private Label Lbl_JKExpect;
        private Label Lbl_JKType;
        private NumericUpDown Nm_JKExpect;
        public static string OutValue;
        private Panel Pnl_Bottom;
        private Panel Pnl_Top;
        private RadioButton Rdb_JKNo;
        private RadioButton Rdb_JKYes;

        public FrmInputJK()
        {
            this.InitializeComponent();
            List<Control> list = new List<Control> {
                this,
                this.Rdb_JKNo,
                this.Rdb_JKYes,
                this.Nm_JKExpect
            };
            base.ControlList = list;
            List<CheckBox> list2 = new List<CheckBox> {
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
                    this.Pnl_Bottom
                };
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list2 = new List<Control> {
                    this.Pnl_Top
                };
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Lbl_JKType,
                    this.Rdb_JKNo,
                    this.Rdb_JKYes,
                    this.Lbl_JKExpect,
                    this.Ckb_Ok,
                    this.Ckb_Cancel
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
            }
        }

        private void Ckb_Close_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void Ckb_Ok_Click(object sender, EventArgs e)
        {
            OutValue = "";
            string str = this.Rdb_JKNo.Checked ? "0" : "1";
            int num = Convert.ToInt32(this.Nm_JKExpect.Value);
            for (int i = 0; i < num; i++)
            {
                OutValue = OutValue + str;
            }
            base.DialogResult = DialogResult.OK;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FrmInputJK_Load(object sender, EventArgs e)
        {
            this.BeautifyInterface();
        }

        private void InitializeComponent()
        {
            this.Lbl_JKType = new Label();
            this.Pnl_Bottom = new Panel();
            this.Ckb_Cancel = new CheckBox();
            this.Ckb_Ok = new CheckBox();
            this.Nm_JKExpect = new NumericUpDown();
            this.Lbl_JKExpect = new Label();
            this.Rdb_JKNo = new RadioButton();
            this.Rdb_JKYes = new RadioButton();
            this.Pnl_Top = new Panel();
            this.Pnl_Bottom.SuspendLayout();
            this.Nm_JKExpect.BeginInit();
            this.Pnl_Top.SuspendLayout();
            base.SuspendLayout();
            this.Lbl_JKType.AutoSize = true;
            this.Lbl_JKType.Location = new Point(7, 14);
            this.Lbl_JKType.Name = "Lbl_JKType";
            this.Lbl_JKType.Size = new Size(0x44, 0x11);
            this.Lbl_JKType.TabIndex = 11;
            this.Lbl_JKType.Text = "监控类型：";
            this.Pnl_Bottom.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Bottom.Controls.Add(this.Ckb_Cancel);
            this.Pnl_Bottom.Controls.Add(this.Ckb_Ok);
            this.Pnl_Bottom.Dock = DockStyle.Bottom;
            this.Pnl_Bottom.Location = new Point(0, 0x54);
            this.Pnl_Bottom.Name = "Pnl_Bottom";
            this.Pnl_Bottom.Size = new Size(0xfd, 0x23);
            this.Pnl_Bottom.TabIndex = 10;
            this.Ckb_Cancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Ckb_Cancel.Appearance = Appearance.Button;
            this.Ckb_Cancel.AutoCheck = false;
            this.Ckb_Cancel.FlatAppearance.BorderSize = 0;
            this.Ckb_Cancel.FlatStyle = FlatStyle.Flat;
            this.Ckb_Cancel.Image = Resources.CancelRound;
            this.Ckb_Cancel.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Cancel.Location = new Point(0xbb, 4);
            this.Ckb_Cancel.Name = "Ckb_Cancel";
            this.Ckb_Cancel.Size = new Size(60, 0x19);
            this.Ckb_Cancel.TabIndex = 0x9f;
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
            this.Ckb_Ok.Location = new Point(0x79, 4);
            this.Ckb_Ok.Name = "Ckb_Ok";
            this.Ckb_Ok.Size = new Size(60, 0x19);
            this.Ckb_Ok.TabIndex = 0x9e;
            this.Ckb_Ok.Text = "确定";
            this.Ckb_Ok.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Ok.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Ok.UseVisualStyleBackColor = true;
            this.Ckb_Ok.Click += new EventHandler(this.Ckb_Ok_Click);
            this.Nm_JKExpect.Location = new Point(0x51, 0x30);
            int[] bits = new int[4];
            bits[0] = 0x3e8;
            this.Nm_JKExpect.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.Nm_JKExpect.Minimum = new decimal(bits);
            this.Nm_JKExpect.Name = "Nm_JKExpect";
            this.Nm_JKExpect.Size = new Size(70, 0x17);
            this.Nm_JKExpect.TabIndex = 0x4c;
            bits = new int[4];
            bits[0] = 10;
            this.Nm_JKExpect.Value = new decimal(bits);
            this.Lbl_JKExpect.AutoSize = true;
            this.Lbl_JKExpect.Location = new Point(7, 50);
            this.Lbl_JKExpect.Name = "Lbl_JKExpect";
            this.Lbl_JKExpect.Size = new Size(0x44, 0x11);
            this.Lbl_JKExpect.TabIndex = 0x4a;
            this.Lbl_JKExpect.Text = "监控期数：";
            this.Rdb_JKNo.AutoSize = true;
            this.Rdb_JKNo.Checked = true;
            this.Rdb_JKNo.Location = new Point(0x51, 12);
            this.Rdb_JKNo.Name = "Rdb_JKNo";
            this.Rdb_JKNo.Size = new Size(0x26, 0x15);
            this.Rdb_JKNo.TabIndex = 0xa2;
            this.Rdb_JKNo.TabStop = true;
            this.Rdb_JKNo.Text = "挂";
            this.Rdb_JKNo.UseVisualStyleBackColor = true;
            this.Rdb_JKYes.AutoSize = true;
            this.Rdb_JKYes.Location = new Point(0x7d, 12);
            this.Rdb_JKYes.Name = "Rdb_JKYes";
            this.Rdb_JKYes.Size = new Size(0x26, 0x15);
            this.Rdb_JKYes.TabIndex = 0xa3;
            this.Rdb_JKYes.Text = "中";
            this.Rdb_JKYes.UseVisualStyleBackColor = true;
            this.Pnl_Top.Controls.Add(this.Lbl_JKType);
            this.Pnl_Top.Controls.Add(this.Rdb_JKYes);
            this.Pnl_Top.Controls.Add(this.Lbl_JKExpect);
            this.Pnl_Top.Controls.Add(this.Rdb_JKNo);
            this.Pnl_Top.Controls.Add(this.Nm_JKExpect);
            this.Pnl_Top.Dock = DockStyle.Fill;
            this.Pnl_Top.Location = new Point(0, 0);
            this.Pnl_Top.Name = "Pnl_Top";
            this.Pnl_Top.Size = new Size(0xfd, 0x54);
            this.Pnl_Top.TabIndex = 0xa4;
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0xfd, 0x77);
            base.Controls.Add(this.Pnl_Top);
            base.Controls.Add(this.Pnl_Bottom);
            base.MaximizeBox = true;
            base.MinimizeBox = true;
            base.Name = "FrmInputJK";
            this.Text = "快速输入监控";
            base.Load += new EventHandler(this.FrmInputJK_Load);
            this.Pnl_Bottom.ResumeLayout(false);
            this.Nm_JKExpect.EndInit();
            this.Pnl_Top.ResumeLayout(false);
            this.Pnl_Top.PerformLayout();
            base.ResumeLayout(false);
        }
    }
}

