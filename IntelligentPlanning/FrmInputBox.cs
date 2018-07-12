namespace IntelligentPlanning
{
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FrmInputBox : ExForm
    {
        private CheckBox Ckb_Cancel;
        private CheckBox Ckb_Ok;
        private IContainer components = null;
        private Label Lbl_Hint;
        private Label Lbl_Value;
        public static string OutValue = "";
        private Panel Pnl_Bottom;
        private Panel Pnl_Main;
        private Timer Tim_NextExpect;
        private TextBox Txt_Value;

        public FrmInputBox(string pText, string pValue, string pTitle, bool pIsPW)
        {
            this.InitializeComponent();
            this.Lbl_Value.Text = pText;
            this.Txt_Value.Text = pValue;
            OutValue = "";
            this.Text = pTitle;
            List<CheckBox> list = new List<CheckBox> {
                this.Ckb_Ok,
                this.Ckb_Cancel
            };
            base.CheckBoxList = list;
            if (pIsPW)
            {
                this.Txt_Value.PasswordChar = '*';
            }
            base.ActiveControl = this.Txt_Value;
            this.Txt_Value.Focus();
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
                    this.Lbl_Value
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control> {
                    this.Lbl_Hint
                };
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
            }
        }

        private void Ckb_Cancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.No;
        }

        private void Ckb_Ok_Click(object sender, EventArgs e)
        {
            OutValue = this.Txt_Value.Text;
            base.DialogResult = DialogResult.Yes;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FrmInputBox_Load(object sender, EventArgs e)
        {
            this.Lbl_Hint.ForeColor = AppInfo.redForeColor;
            this.BeautifyInterface();
            this.Txt_Value.SelectAll();
            this.Txt_Value.Focus();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.Pnl_Bottom = new Panel();
            this.Ckb_Cancel = new CheckBox();
            this.Ckb_Ok = new CheckBox();
            this.Lbl_Hint = new Label();
            this.Pnl_Main = new Panel();
            this.Txt_Value = new TextBox();
            this.Lbl_Value = new Label();
            this.Tim_NextExpect = new Timer(this.components);
            this.Pnl_Bottom.SuspendLayout();
            this.Pnl_Main.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_Bottom.Controls.Add(this.Ckb_Cancel);
            this.Pnl_Bottom.Controls.Add(this.Ckb_Ok);
            this.Pnl_Bottom.Controls.Add(this.Lbl_Hint);
            this.Pnl_Bottom.Dock = DockStyle.Bottom;
            this.Pnl_Bottom.Location = new Point(0, 0x5b);
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
            this.Ckb_Cancel.Location = new Point(0xf1, 5);
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
            this.Ckb_Ok.Location = new Point(0xaf, 5);
            this.Ckb_Ok.Name = "Ckb_Ok";
            this.Ckb_Ok.Size = new Size(60, 0x19);
            this.Ckb_Ok.TabIndex = 0x9c;
            this.Ckb_Ok.Text = "确定";
            this.Ckb_Ok.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Ok.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Ok.UseVisualStyleBackColor = true;
            this.Ckb_Ok.Click += new EventHandler(this.Ckb_Ok_Click);
            this.Lbl_Hint.AutoSize = true;
            this.Lbl_Hint.Location = new Point(11, 9);
            this.Lbl_Hint.Name = "Lbl_Hint";
            this.Lbl_Hint.Size = new Size(0, 0x11);
            this.Lbl_Hint.TabIndex = 11;
            this.Pnl_Main.BackColor = Color.White;
            this.Pnl_Main.Controls.Add(this.Txt_Value);
            this.Pnl_Main.Controls.Add(this.Lbl_Value);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(0x134, 0x5b);
            this.Pnl_Main.TabIndex = 12;
            this.Txt_Value.Location = new Point(8, 0x38);
            this.Txt_Value.Name = "Txt_Value";
            this.Txt_Value.Size = new Size(0x123, 0x17);
            this.Txt_Value.TabIndex = 1;
            this.Lbl_Value.Location = new Point(10, 9);
            this.Lbl_Value.Name = "Lbl_Value";
            this.Lbl_Value.Size = new Size(0xec, 0x23);
            this.Lbl_Value.TabIndex = 2;
            this.Lbl_Value.Text = "是否要更新？";
            this.Tim_NextExpect.Interval = 0x3e8;
            base.AutoScaleDimensions = new SizeF(96f, 96f);
            base.AutoScaleMode = AutoScaleMode.Dpi;
            base.ClientSize = new Size(0x134, 0x7e);
            base.Controls.Add(this.Pnl_Main);
            base.Controls.Add(this.Pnl_Bottom);
            base.Name = "FrmInputBox";
            base.Load += new EventHandler(this.FrmInputBox_Load);
            this.Pnl_Bottom.ResumeLayout(false);
            this.Pnl_Bottom.PerformLayout();
            this.Pnl_Main.ResumeLayout(false);
            this.Pnl_Main.PerformLayout();
            base.ResumeLayout(false);
        }
    }
}

