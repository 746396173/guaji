namespace IntelligentPlanning
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class FrmProgress : Form
    {
        private float _HandCount = 0f;
        private bool _NeedShowBar = false;
        private Form _ParentForm = null;
        private Button Btn_Cancel;
        private Button Btn_Continue;
        private Button Btn_Pause;
        private IContainer components = null;
        private double CurIndex = 0.0;
        private bool DoCancel = false;
        public const int HTCAPTION = 2;
        private Label Lbl_Hint;
        private Label Lbl_Total;
        private ProgressBar progressBar;
        public const int SC_MOVE = 0xf010;
        private Timer timer1;
        public string TotalHint = "";
        public const int WM_SYSCOMMAND = 0x112;

        public FrmProgress()
        {
            this.InitializeComponent();
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DoCancel = true;
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

        private void FrmProgress_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(base.Handle.ToInt32(), 0x112, 0xf012, 0);
        }

        public void IncValue(ref bool Cancel)
        {
            double num2;
            Cancel = this.DoCancel;
            int num = 0;
            if (!Cancel)
            {
                this.CurIndex = (num2 = this.CurIndex) + 1.0;
                num = Convert.ToInt32((double) (this.progressBar.Maximum * (num2 / ((double) this.HandCount))));
            }
            else
            {
                this.CurIndex = (num2 = this.CurIndex) - 1.0;
                num = Convert.ToInt32((double) (this.progressBar.Maximum * (num2 / ((double) this.HandCount))));
            }
            if ((num > 0) && (((num <= this.progressBar.Maximum) && (num > this.progressBar.Value)) && ((this.CurIndex % 100.0) == 0.0)))
            {
                this.progressBar.Value = num;
                Application.DoEvents();
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.progressBar = new ProgressBar();
            this.Lbl_Hint = new Label();
            this.timer1 = new Timer(this.components);
            this.Btn_Cancel = new Button();
            this.Btn_Continue = new Button();
            this.Btn_Pause = new Button();
            this.Lbl_Total = new Label();
            base.SuspendLayout();
            this.progressBar.Location = new Point(12, 0x24);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new Size(310, 0x10);
            this.progressBar.TabIndex = 0;
            this.Lbl_Hint.AutoSize = true;
            this.Lbl_Hint.Location = new Point(12, 12);
            this.Lbl_Hint.Name = "Lbl_Hint";
            this.Lbl_Hint.Size = new Size(0x47, 0x11);
            this.Lbl_Hint.TabIndex = 1;
            this.Lbl_Hint.Text = "Processing";
            this.timer1.Interval = 20;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            this.Btn_Cancel.Location = new Point(0xf8, 60);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new Size(0x4b, 0x19);
            this.Btn_Cancel.TabIndex = 2;
            this.Btn_Cancel.Text = "取消";
            this.Btn_Cancel.UseVisualStyleBackColor = true;
            this.Btn_Cancel.Click += new EventHandler(this.Btn_Cancel_Click);
            this.Btn_Continue.Location = new Point(-1000, 60);
            this.Btn_Continue.Name = "Btn_Continue";
            this.Btn_Continue.Size = new Size(0x4b, 0x19);
            this.Btn_Continue.TabIndex = 3;
            this.Btn_Continue.Text = "继续";
            this.Btn_Continue.UseVisualStyleBackColor = true;
            this.Btn_Continue.Visible = false;
            this.Btn_Pause.Location = new Point(-1000, 60);
            this.Btn_Pause.Name = "Btn_Pause";
            this.Btn_Pause.Size = new Size(0x4b, 0x19);
            this.Btn_Pause.TabIndex = 4;
            this.Btn_Pause.Text = "暂停";
            this.Btn_Pause.UseVisualStyleBackColor = true;
            this.Btn_Pause.Visible = false;
            this.Lbl_Total.AutoSize = true;
            this.Lbl_Total.Location = new Point(11, 0x3e);
            this.Lbl_Total.Name = "Lbl_Total";
            this.Lbl_Total.Size = new Size(0x2c, 0x11);
            this.Lbl_Total.TabIndex = 6;
            this.Lbl_Total.Text = "进度：";
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(0x14f, 0x5e);
            base.ControlBox = false;
            base.Controls.Add(this.Lbl_Total);
            base.Controls.Add(this.Btn_Pause);
            base.Controls.Add(this.Btn_Continue);
            base.Controls.Add(this.Btn_Cancel);
            base.Controls.Add(this.Lbl_Hint);
            base.Controls.Add(this.progressBar);
            this.Font = new Font("微软雅黑", 9f);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "FrmProgress";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            base.MouseDown += new MouseEventHandler(this.FrmProgress_MouseDown);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(int hwnd, int wMsg, int wParam, int lParam);
        public void SetHint(string pHintId)
        {
            this.CurIndex = 0.0;
            this.Lbl_Hint.Text = pHintId;
            this.TotalHint = this.Lbl_Total.Text;
        }

        public bool SetValue()
        {
            double num2;
            if (this.CurIndex == 0.0)
            {
                this.progressBar.Value = 0;
                Application.DoEvents();
            }
            this.CurIndex = (num2 = this.CurIndex) + 1.0;
            int num = Convert.ToInt32((double) (this.progressBar.Maximum * (num2 / ((double) this.HandCount))));
            if ((num > 0) && (((num <= this.progressBar.Maximum) && (num > this.progressBar.Value)) && (this.progressBar.Value != num)))
            {
                this.progressBar.Value = num;
                this.Lbl_Total.Text = this.TotalHint + num.ToString() + "%";
                Application.DoEvents();
            }
            return this.DoCancel;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool cancel = false;
            this.IncValue(ref cancel);
        }

        public float HandCount
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

