namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class FrmDLUserLogin : ExForm
    {
        public ConfigurationStatus.SCAccountData AccountData;
        private Button Btn_Ok;
        private IContainer components = null;
        private Label Lbl_LoginID;
        private Label Lbl_LoginPW;
        private Panel Pnl_Main;
        private TabControl Tab_Main;
        private TabPage Tap_Login;
        private TextBox Txt_LoginID;
        private TextBox Txt_LoginPW;

        public FrmDLUserLogin(ConfigurationStatus.SCAccountData pAccountData)
        {
            this.InitializeComponent();
            this.AccountData = pAccountData;
            List<Control> list = new List<Control> {
                this,
                this.Txt_LoginID,
                this.Txt_LoginPW
            };
            base.ControlList = list;
        }

        private void Btn_Ok_Click(object sender, EventArgs e)
        {
            if (this.Txt_LoginID.Text == "")
            {
                CommFunc.PublicMessageAll("输入代理账号不能为空！", true, MessageBoxIcon.Asterisk, "");
                this.Txt_LoginID.Focus();
            }
            else if (this.Txt_LoginPW.Text == "")
            {
                CommFunc.PublicMessageAll("输入代理密码不能为空！", true, MessageBoxIcon.Asterisk, "");
                this.Txt_LoginPW.Focus();
            }
            else
            {
                string pHint = "";
                if (SQLData.DLUserLogin(this.Txt_LoginID.Text, this.Txt_LoginPW.Text, this.AccountData, ref pHint))
                {
                    base.DialogResult = DialogResult.OK;
                }
                else
                {
                    CommFunc.PublicMessageAll(pHint, true, MessageBoxIcon.Asterisk, "");
                    this.Txt_LoginID.SelectAll();
                    this.Txt_LoginID.Focus();
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

        private void FrmDLUserLogin_Load(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FrmDLUserLogin));
            this.Btn_Ok = new Button();
            this.Pnl_Main = new Panel();
            this.Tab_Main = new TabControl();
            this.Tap_Login = new TabPage();
            this.Lbl_LoginID = new Label();
            this.Lbl_LoginPW = new Label();
            this.Txt_LoginID = new TextBox();
            this.Txt_LoginPW = new TextBox();
            this.Pnl_Main.SuspendLayout();
            this.Tab_Main.SuspendLayout();
            this.Tap_Login.SuspendLayout();
            base.SuspendLayout();
            this.Btn_Ok.FlatAppearance.BorderSize = 0;
            this.Btn_Ok.Location = new Point(0xee, 0x71);
            this.Btn_Ok.Name = "Btn_Ok";
            this.Btn_Ok.Size = new Size(80, 0x19);
            this.Btn_Ok.TabIndex = 13;
            this.Btn_Ok.Text = "登录";
            this.Btn_Ok.UseVisualStyleBackColor = true;
            this.Btn_Ok.Click += new EventHandler(this.Btn_Ok_Click);
            this.Pnl_Main.BorderStyle = BorderStyle.Fixed3D;
            this.Pnl_Main.Controls.Add(this.Tab_Main);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(0x15a, 210);
            this.Pnl_Main.TabIndex = 0;
            this.Tab_Main.Controls.Add(this.Tap_Login);
            this.Tab_Main.Dock = DockStyle.Fill;
            this.Tab_Main.ItemSize = new Size(0x41, 30);
            this.Tab_Main.Location = new Point(0, 0);
            this.Tab_Main.Name = "Tab_Main";
            this.Tab_Main.SelectedIndex = 0;
            this.Tab_Main.Size = new Size(0x156, 0xce);
            this.Tab_Main.SizeMode = TabSizeMode.Fixed;
            this.Tab_Main.TabIndex = 0x1d;
            this.Tap_Login.BackColor = SystemColors.Control;
            this.Tap_Login.Controls.Add(this.Lbl_LoginID);
            this.Tap_Login.Controls.Add(this.Btn_Ok);
            this.Tap_Login.Controls.Add(this.Lbl_LoginPW);
            this.Tap_Login.Controls.Add(this.Txt_LoginID);
            this.Tap_Login.Controls.Add(this.Txt_LoginPW);
            this.Tap_Login.Location = new Point(4, 0x22);
            this.Tap_Login.Name = "Tap_Login";
            this.Tap_Login.Padding = new Padding(3);
            this.Tap_Login.Size = new Size(0x14e, 0xa8);
            this.Tap_Login.TabIndex = 0;
            this.Tap_Login.Text = "登录使用";
            this.Lbl_LoginID.AutoSize = true;
            this.Lbl_LoginID.Location = new Point(8, 0x2a);
            this.Lbl_LoginID.Name = "Lbl_LoginID";
            this.Lbl_LoginID.Size = new Size(0x44, 0x11);
            this.Lbl_LoginID.TabIndex = 0;
            this.Lbl_LoginID.Text = "代理账号：";
            this.Lbl_LoginPW.AutoSize = true;
            this.Lbl_LoginPW.Location = new Point(8, 0x54);
            this.Lbl_LoginPW.Name = "Lbl_LoginPW";
            this.Lbl_LoginPW.Size = new Size(0x44, 0x11);
            this.Lbl_LoginPW.TabIndex = 2;
            this.Lbl_LoginPW.Text = "代理密码：";
            this.Txt_LoginID.Location = new Point(0x52, 0x27);
            this.Txt_LoginID.Name = "Txt_LoginID";
            this.Txt_LoginID.Size = new Size(0xec, 0x17);
            this.Txt_LoginID.TabIndex = 11;
            this.Txt_LoginPW.Location = new Point(0x52, 0x51);
            this.Txt_LoginPW.Name = "Txt_LoginPW";
            this.Txt_LoginPW.PasswordChar = '*';
            this.Txt_LoginPW.Size = new Size(0xec, 0x17);
            this.Txt_LoginPW.TabIndex = 12;
            base.AutoScaleDimensions = new SizeF(96f, 96f);
            base.AutoScaleMode = AutoScaleMode.Dpi;
            base.ClientSize = new Size(0x15a, 210);
            base.Controls.Add(this.Pnl_Main);
            base.Name = "FrmDLUserLogin";
            base.ShowInTaskbar = true;
            this.Text = "代理登录";
            base.Load += new EventHandler(this.FrmDLUserLogin_Load);
            this.Pnl_Main.ResumeLayout(false);
            this.Tab_Main.ResumeLayout(false);
            this.Tap_Login.ResumeLayout(false);
            this.Tap_Login.PerformLayout();
            base.ResumeLayout(false);
        }
    }
}

