namespace IntelligentPlanning
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class FrmRunProgress : Form
    {
        private long _HandCount = 0L;
        private bool _NeedShowBar = false;
        private Form _ParentForm = null;
        private Button Btn_Cancel;
        private IContainer components = null;
        private double CurIndex = 0.0;
        private bool DoCancel = false;
        private Label Lbl_Hint;
        private ProgressBar Pro_ViewProgerss;

        public event EventHandler<EventArgs> ButtonClick;

        public FrmRunProgress(string pHint)
        {
            this.InitializeComponent();
            this.Lbl_Hint.Text = pHint;
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            if (this.ButtonClick != null)
            {
                this.ButtonClick(sender, e);
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

        public void EndProcess()
        {
            base.Close();
        }

        private void InitializeComponent()
        {
            this.Pro_ViewProgerss = new ProgressBar();
            this.Lbl_Hint = new Label();
            this.Btn_Cancel = new Button();
            base.SuspendLayout();
            this.Pro_ViewProgerss.Location = new Point(12, 0x24);
            this.Pro_ViewProgerss.MarqueeAnimationSpeed = 50;
            this.Pro_ViewProgerss.Name = "Pro_ViewProgerss";
            this.Pro_ViewProgerss.Size = new Size(0xe3, 0x10);
            this.Pro_ViewProgerss.Style = ProgressBarStyle.Marquee;
            this.Pro_ViewProgerss.TabIndex = 0;
            this.Lbl_Hint.AutoSize = true;
            this.Lbl_Hint.Location = new Point(12, 15);
            this.Lbl_Hint.Name = "Lbl_Hint";
            this.Lbl_Hint.Size = new Size(0x47, 0x11);
            this.Lbl_Hint.TabIndex = 1;
            this.Lbl_Hint.Text = "Processing";
            this.Btn_Cancel.Location = new Point(0xf5, 0x20);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new Size(0x4b, 0x17);
            this.Btn_Cancel.TabIndex = 3;
            this.Btn_Cancel.Text = "取消";
            this.Btn_Cancel.UseVisualStyleBackColor = true;
            this.Btn_Cancel.Click += new EventHandler(this.Btn_Cancel_Click);
            base.AutoScaleDimensions = new SizeF(96f, 96f);
            base.AutoScaleMode = AutoScaleMode.Dpi;
            base.ClientSize = new Size(0x14b, 0x4a);
            base.ControlBox = false;
            base.Controls.Add(this.Btn_Cancel);
            base.Controls.Add(this.Lbl_Hint);
            base.Controls.Add(this.Pro_ViewProgerss);
            this.Font = new Font("微软雅黑", 9f);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "FrmRunProgress";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void SetProgressBarValue(int pValue)
        {
            this.Pro_ViewProgerss.Value = pValue;
        }

        public long HandCount
        {
            get => 
                this._HandCount;
            set
            {
                this._HandCount = value;
            }
        }

        public bool NeedShowBar
        {
            get => 
                this._NeedShowBar;
            set
            {
                this._NeedShowBar = value;
            }
        }

        public Form ParentWindow
        {
            get => 
                this._ParentForm;
            set
            {
                if (value != null)
                {
                    this._ParentForm = value;
                }
            }
        }
    }
}

