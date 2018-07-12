namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FrmInput : ExForm
    {
        private Button Btn_Close;
        private Button Btn_Copy;
        private IContainer components = null;
        private Panel Pnl_Bottom;
        private TextBox Txt_Input;

        public FrmInput(string pInput, string pTitleId)
        {
            this.InitializeComponent();
            List<Control> list = new List<Control> {
                this
            };
            base.ControlList = list;
            this.Txt_Input.Text = pInput;
            this.Text = pTitleId;
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void Btn_Copy_Click(object sender, EventArgs e)
        {
            CommFunc.CopyText(this.Txt_Input.Text);
            CommFunc.PublicMessageAll("复制成功！", true, MessageBoxIcon.Asterisk, "");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FrmInput_Load(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            this.Btn_Copy = new Button();
            this.Btn_Close = new Button();
            this.Txt_Input = new TextBox();
            this.Pnl_Bottom = new Panel();
            this.Pnl_Bottom.SuspendLayout();
            base.SuspendLayout();
            this.Btn_Copy.Location = new Point(0x150, 5);
            this.Btn_Copy.Name = "Btn_Copy";
            this.Btn_Copy.Size = new Size(80, 0x19);
            this.Btn_Copy.TabIndex = 150;
            this.Btn_Copy.Text = "复制";
            this.Btn_Copy.UseVisualStyleBackColor = true;
            this.Btn_Copy.Click += new EventHandler(this.Btn_Copy_Click);
            this.Btn_Close.DialogResult = DialogResult.Cancel;
            this.Btn_Close.Location = new Point(0x1a6, 5);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new Size(80, 0x19);
            this.Btn_Close.TabIndex = 0x97;
            this.Btn_Close.Text = "关闭";
            this.Btn_Close.UseVisualStyleBackColor = true;
            this.Btn_Close.Click += new EventHandler(this.Btn_Close_Click);
            this.Txt_Input.Dock = DockStyle.Fill;
            this.Txt_Input.Location = new Point(0, 0);
            this.Txt_Input.Multiline = true;
            this.Txt_Input.Name = "Txt_Input";
            this.Txt_Input.Size = new Size(0x1ff, 0x13c);
            this.Txt_Input.TabIndex = 1;
            this.Pnl_Bottom.Controls.Add(this.Btn_Close);
            this.Pnl_Bottom.Controls.Add(this.Btn_Copy);
            this.Pnl_Bottom.Dock = DockStyle.Bottom;
            this.Pnl_Bottom.Location = new Point(0, 0x13c);
            this.Pnl_Bottom.Name = "Pnl_Bottom";
            this.Pnl_Bottom.Size = new Size(0x1ff, 0x23);
            this.Pnl_Bottom.TabIndex = 10;
            base.AcceptButton = this.Btn_Copy;
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.Btn_Close;
            base.ClientSize = new Size(0x1ff, 0x15f);
            base.Controls.Add(this.Txt_Input);
            base.Controls.Add(this.Pnl_Bottom);
            base.MaximizeBox = true;
            base.MinimizeBox = true;
            base.Name = "FrmInput";
            base.Load += new EventHandler(this.FrmInput_Load);
            this.Pnl_Bottom.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

