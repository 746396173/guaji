namespace IntelligentPlanning
{
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FrmFollowBets : ExForm
    {
        private CheckBox Ckb_Cancel;
        private CheckBox Ckb_Ok;
        private CheckBox Ckb_SharePaste;
        private IContainer components = null;
        private Label Lbl_Value;
        private Panel Pnl_Bottom;
        private Panel Pnl_Main;
        private string RegConfigPath1 = "";
        private TextBox Txt_SharePaste;

        public FrmFollowBets(string pRegConfigPath1)
        {
            this.InitializeComponent();
            this.RegConfigPath1 = pRegConfigPath1;
            List<CheckBox> list = new List<CheckBox> {
                this.Ckb_Ok,
                this.Ckb_Cancel,
                this.Ckb_SharePaste
            };
            base.CheckBoxList = list;
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
                    this.Pnl_Main
                };
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Ckb_Ok,
                    this.Ckb_Cancel,
                    this.Ckb_SharePaste,
                    this.Lbl_Value
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
            }
        }

        private void Ckb_Cancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.No;
        }

        private void Ckb_Ok_Click(object sender, EventArgs e)
        {
            string regValue = this.Txt_SharePaste.Text.Trim();
            CommFunc.WriteRegValue(this.RegConfigPath1, "ShareCode", regValue);
            base.DialogResult = DialogResult.Yes;
        }

        private void Ckb_SharePaste_Click(object sender, EventArgs e)
        {
            CommFunc.PasteText(this.Txt_SharePaste);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FrmFollowBets_Load(object sender, EventArgs e)
        {
            string str = CommFunc.ReadRegString(this.RegConfigPath1, "ShareCode", "");
            this.Txt_SharePaste.Text = str;
            this.BeautifyInterface();
            base.ActiveControl = this.Txt_SharePaste;
            this.Txt_SharePaste.SelectAll();
            this.Txt_SharePaste.Focus();
        }

        private void InitializeComponent()
        {
            this.Pnl_Bottom = new Panel();
            this.Ckb_Cancel = new CheckBox();
            this.Ckb_Ok = new CheckBox();
            this.Pnl_Main = new Panel();
            this.Ckb_SharePaste = new CheckBox();
            this.Txt_SharePaste = new TextBox();
            this.Lbl_Value = new Label();
            this.Pnl_Bottom.SuspendLayout();
            this.Pnl_Main.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_Bottom.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Bottom.Controls.Add(this.Ckb_Cancel);
            this.Pnl_Bottom.Controls.Add(this.Ckb_Ok);
            this.Pnl_Bottom.Dock = DockStyle.Bottom;
            this.Pnl_Bottom.Location = new Point(0, 0x49);
            this.Pnl_Bottom.Name = "Pnl_Bottom";
            this.Pnl_Bottom.Size = new Size(0x134, 0x23);
            this.Pnl_Bottom.TabIndex = 11;
            this.Ckb_Cancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Ckb_Cancel.Appearance = Appearance.Button;
            this.Ckb_Cancel.AutoCheck = false;
            this.Ckb_Cancel.FlatAppearance.BorderSize = 0;
            this.Ckb_Cancel.FlatStyle = FlatStyle.Flat;
            this.Ckb_Cancel.Image = Resources.CancelRound;
            this.Ckb_Cancel.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Cancel.Location = new Point(0xef, 3);
            this.Ckb_Cancel.Name = "Ckb_Cancel";
            this.Ckb_Cancel.Size = new Size(60, 0x19);
            this.Ckb_Cancel.TabIndex = 0x9d;
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
            this.Ckb_Ok.Location = new Point(0xad, 3);
            this.Ckb_Ok.Name = "Ckb_Ok";
            this.Ckb_Ok.Size = new Size(60, 0x19);
            this.Ckb_Ok.TabIndex = 0x9c;
            this.Ckb_Ok.Text = "确定";
            this.Ckb_Ok.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Ok.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Ok.UseVisualStyleBackColor = true;
            this.Ckb_Ok.Click += new EventHandler(this.Ckb_Ok_Click);
            this.Pnl_Main.BackColor = SystemColors.Control;
            this.Pnl_Main.Controls.Add(this.Ckb_SharePaste);
            this.Pnl_Main.Controls.Add(this.Txt_SharePaste);
            this.Pnl_Main.Controls.Add(this.Lbl_Value);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(0x134, 0x49);
            this.Pnl_Main.TabIndex = 12;
            this.Ckb_SharePaste.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Ckb_SharePaste.Appearance = Appearance.Button;
            this.Ckb_SharePaste.AutoCheck = false;
            this.Ckb_SharePaste.FlatAppearance.BorderSize = 0;
            this.Ckb_SharePaste.FlatStyle = FlatStyle.Flat;
            this.Ckb_SharePaste.Image = Resources.Paste;
            this.Ckb_SharePaste.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_SharePaste.Location = new Point(0xf1, 0x25);
            this.Ckb_SharePaste.Name = "Ckb_SharePaste";
            this.Ckb_SharePaste.Size = new Size(60, 0x19);
            this.Ckb_SharePaste.TabIndex = 0x138;
            this.Ckb_SharePaste.Text = "粘贴";
            this.Ckb_SharePaste.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_SharePaste.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_SharePaste.UseVisualStyleBackColor = true;
            this.Ckb_SharePaste.Click += new EventHandler(this.Ckb_SharePaste_Click);
            this.Txt_SharePaste.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Txt_SharePaste.Location = new Point(8, 0x26);
            this.Txt_SharePaste.Name = "Txt_SharePaste";
            this.Txt_SharePaste.Size = new Size(0xe3, 0x17);
            this.Txt_SharePaste.TabIndex = 1;
            this.Lbl_Value.AutoSize = true;
            this.Lbl_Value.Location = new Point(10, 9);
            this.Lbl_Value.Name = "Lbl_Value";
            this.Lbl_Value.Size = new Size(0xa4, 0x11);
            this.Lbl_Value.TabIndex = 2;
            this.Lbl_Value.Text = "请输入上级发给你的共享码：";
            base.AutoScaleDimensions = new SizeF(96f, 96f);
            base.AutoScaleMode = AutoScaleMode.Dpi;
            base.ClientSize = new Size(0x134, 0x6c);
            base.Controls.Add(this.Pnl_Main);
            base.Controls.Add(this.Pnl_Bottom);
            base.Name = "FrmFollowBets";
            base.Load += new EventHandler(this.FrmFollowBets_Load);
            this.Pnl_Bottom.ResumeLayout(false);
            this.Pnl_Main.ResumeLayout(false);
            this.Pnl_Main.PerformLayout();
            base.ResumeLayout(false);
        }
    }
}

