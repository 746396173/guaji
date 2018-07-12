namespace IntelligentPlanning
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FrmNotice : ExForm
    {
        private Button Btn_Close;
        private IContainer components = null;
        private Panel Pnl_AppName;
        private Panel Pnl_Main;
        private TextBox Txt_Value;

        public FrmNotice(string pKey, string pValue)
        {
            this.InitializeComponent();
            this.Text = pKey;
            this.Txt_Value.Text = pValue;
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FrmNotice_Load(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FrmNotice));
            this.Pnl_Main = new Panel();
            this.Txt_Value = new TextBox();
            this.Pnl_AppName = new Panel();
            this.Btn_Close = new Button();
            this.Pnl_Main.SuspendLayout();
            this.Pnl_AppName.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_Main.Controls.Add(this.Txt_Value);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(0x160, 0xf8);
            this.Pnl_Main.TabIndex = 0x4f;
            this.Txt_Value.Dock = DockStyle.Fill;
            this.Txt_Value.Location = new Point(0, 0);
            this.Txt_Value.Multiline = true;
            this.Txt_Value.Name = "Txt_Value";
            this.Txt_Value.ReadOnly = true;
            this.Txt_Value.ScrollBars = ScrollBars.Vertical;
            this.Txt_Value.Size = new Size(0x160, 0xf8);
            this.Txt_Value.TabIndex = 1;
            this.Pnl_AppName.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_AppName.Controls.Add(this.Btn_Close);
            this.Pnl_AppName.Dock = DockStyle.Bottom;
            this.Pnl_AppName.Location = new Point(0, 0xf8);
            this.Pnl_AppName.Name = "Pnl_AppName";
            this.Pnl_AppName.Size = new Size(0x160, 0x23);
            this.Pnl_AppName.TabIndex = 0x4e;
            this.Btn_Close.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Btn_Close.Location = new Point(0x11f, 4);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new Size(60, 0x19);
            this.Btn_Close.TabIndex = 0xb8;
            this.Btn_Close.Text = "关闭";
            this.Btn_Close.UseVisualStyleBackColor = true;
            this.Btn_Close.Click += new EventHandler(this.Btn_Close_Click);
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x160, 0x11b);
            base.Controls.Add(this.Pnl_Main);
            base.Controls.Add(this.Pnl_AppName);
            base.Name = "FrmNotice";
            base.Load += new EventHandler(this.FrmNotice_Load);
            this.Pnl_Main.ResumeLayout(false);
            this.Pnl_Main.PerformLayout();
            this.Pnl_AppName.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}

