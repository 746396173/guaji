namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FrmVerifyCode : ExForm
    {
        private Button Btn_Ok;
        private IContainer components = null;
        private Label Lbl_VerifyCode;
        public static string OutValue = "";
        private PictureBox Pic_Main;
        private Panel Pnl_Bottom;
        private Panel Pnl_Main;
        private TextBox Txt_VerifyCode;

        public FrmVerifyCode(string pFile)
        {
            this.InitializeComponent();
            base.ControlList = new List<Control>();
            this.Pic_Main.ImageLocation = pFile;
        }

        private void Btn_Ok_Click(object sender, EventArgs e)
        {
            if (this.Txt_VerifyCode.Text == "")
            {
                CommFunc.PublicMessageAll("验证码输入不能为空！", true, MessageBoxIcon.Asterisk, "");
                this.Txt_VerifyCode.Focus();
            }
            else
            {
                OutValue = this.Txt_VerifyCode.Text.Trim();
                base.DialogResult = DialogResult.OK;
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

        private void FrmVerifyCode_Load(object sender, EventArgs e)
        {
            CommFunc.SetForegroundWindow(base.Handle);
        }

        private void InitializeComponent()
        {
            this.Pnl_Main = new Panel();
            this.Pic_Main = new PictureBox();
            this.Txt_VerifyCode = new TextBox();
            this.Lbl_VerifyCode = new Label();
            this.Pnl_Bottom = new Panel();
            this.Btn_Ok = new Button();
            this.Pnl_Main.SuspendLayout();
            ((ISupportInitialize) this.Pic_Main).BeginInit();
            this.Pnl_Bottom.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_Main.Controls.Add(this.Pic_Main);
            this.Pnl_Main.Controls.Add(this.Txt_VerifyCode);
            this.Pnl_Main.Controls.Add(this.Lbl_VerifyCode);
            this.Pnl_Main.Controls.Add(this.Pnl_Bottom);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(0x10a, 0x8a);
            this.Pnl_Main.TabIndex = 12;
            this.Pic_Main.Location = new Point(15, 0x2e);
            this.Pic_Main.Name = "Pic_Main";
            this.Pic_Main.Size = new Size(0xef, 50);
            this.Pic_Main.SizeMode = PictureBoxSizeMode.CenterImage;
            this.Pic_Main.TabIndex = 0xb8;
            this.Pic_Main.TabStop = false;
            this.Txt_VerifyCode.Location = new Point(0x9e, 15);
            this.Txt_VerifyCode.Name = "Txt_VerifyCode";
            this.Txt_VerifyCode.Size = new Size(0x60, 0x17);
            this.Txt_VerifyCode.TabIndex = 0;
            this.Lbl_VerifyCode.AutoSize = true;
            this.Lbl_VerifyCode.BackColor = Color.Transparent;
            this.Lbl_VerifyCode.Location = new Point(12, 0x12);
            this.Lbl_VerifyCode.Name = "Lbl_VerifyCode";
            this.Lbl_VerifyCode.Size = new Size(140, 0x11);
            this.Lbl_VerifyCode.TabIndex = 0xb7;
            this.Lbl_VerifyCode.Text = "请输入下图中的验证码：";
            this.Pnl_Bottom.BackColor = Color.Transparent;
            this.Pnl_Bottom.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Bottom.Controls.Add(this.Btn_Ok);
            this.Pnl_Bottom.Dock = DockStyle.Bottom;
            this.Pnl_Bottom.Location = new Point(0, 0x67);
            this.Pnl_Bottom.Name = "Pnl_Bottom";
            this.Pnl_Bottom.Size = new Size(0x10a, 0x23);
            this.Pnl_Bottom.TabIndex = 10;
            this.Btn_Ok.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Btn_Ok.Font = new Font("微软雅黑", 9f);
            this.Btn_Ok.Location = new Point(0xc1, 5);
            this.Btn_Ok.Name = "Btn_Ok";
            this.Btn_Ok.Size = new Size(60, 0x19);
            this.Btn_Ok.TabIndex = 0x9a;
            this.Btn_Ok.Text = "确定";
            this.Btn_Ok.UseVisualStyleBackColor = true;
            this.Btn_Ok.Click += new EventHandler(this.Btn_Ok_Click);
            base.AcceptButton = this.Btn_Ok;
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x10a, 0x8a);
            base.Controls.Add(this.Pnl_Main);
            base.Name = "FrmVerifyCode";
            this.Text = "输入验证码";
            base.TopMost = true;
            base.Load += new EventHandler(this.FrmVerifyCode_Load);
            this.Pnl_Main.ResumeLayout(false);
            this.Pnl_Main.PerformLayout();
            ((ISupportInitialize) this.Pic_Main).EndInit();
            this.Pnl_Bottom.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}

