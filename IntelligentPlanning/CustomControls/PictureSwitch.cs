namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class PictureSwitch : UserControl
    {
        private Button Btn_Switch1;
        private Button Btn_Switch2;
        private List<Button> ButtonList = null;
        private IContainer components = null;
        private int ImageIndex = -1;
        private Dictionary<Image, string> ImageLinkDic = null;
        private List<Image> ImageList = null;
        private bool IsMove = false;
        private PictureBox Pic_Image;
        private Panel Pnl_Main;
        private Timer Tim_Switch;

        public PictureSwitch()
        {
            this.InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern bool AnimateWindow(IntPtr hwnd, uint dwTime, AnimationType dwFlags);
        private void Btn_Switch1_Click(object sender, EventArgs e)
        {
            this.SetImage(false);
        }

        private void Btn_Switch2_Click(object sender, EventArgs e)
        {
            this.SetImage(true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public int GetImageIndex(bool pIsAdd)
        {
            if (pIsAdd)
            {
                this.ImageIndex++;
            }
            else
            {
                this.ImageIndex--;
            }
            if (this.ImageIndex < 0)
            {
                this.ImageIndex = 0;
            }
            this.ImageIndex = this.ImageIndex % this.ImageList.Count;
            return this.ImageIndex;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.Pnl_Main = new Panel();
            this.Btn_Switch2 = new Button();
            this.Btn_Switch1 = new Button();
            this.Pic_Image = new PictureBox();
            this.Tim_Switch = new Timer(this.components);
            this.Pnl_Main.SuspendLayout();
            ((ISupportInitialize) this.Pic_Image).BeginInit();
            base.SuspendLayout();
            this.Pnl_Main.Controls.Add(this.Btn_Switch2);
            this.Pnl_Main.Controls.Add(this.Btn_Switch1);
            this.Pnl_Main.Controls.Add(this.Pic_Image);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(0x2ba, 0x6a);
            this.Pnl_Main.TabIndex = 0;
            this.Btn_Switch2.BackColor = Color.Transparent;
            this.Btn_Switch2.FlatAppearance.BorderSize = 0;
            this.Btn_Switch2.FlatStyle = FlatStyle.Flat;
            this.Btn_Switch2.ForeColor = SystemColors.ControlText;
            this.Btn_Switch2.Image = Resources.SwitchRight32;
            this.Btn_Switch2.Location = new Point(0x295, 0x24);
            this.Btn_Switch2.Name = "Btn_Switch2";
            this.Btn_Switch2.Size = new Size(0x20, 0x20);
            this.Btn_Switch2.TabIndex = 3;
            this.Btn_Switch2.UseVisualStyleBackColor = false;
            this.Btn_Switch2.Visible = false;
            this.Btn_Switch2.Click += new EventHandler(this.Btn_Switch2_Click);
            this.Btn_Switch1.BackColor = Color.Transparent;
            this.Btn_Switch1.FlatAppearance.BorderSize = 0;
            this.Btn_Switch1.FlatStyle = FlatStyle.Flat;
            this.Btn_Switch1.ForeColor = SystemColors.ControlText;
            this.Btn_Switch1.Image = Resources.SwitchLeft32;
            this.Btn_Switch1.Location = new Point(5, 0x24);
            this.Btn_Switch1.Name = "Btn_Switch1";
            this.Btn_Switch1.Size = new Size(0x20, 0x20);
            this.Btn_Switch1.TabIndex = 2;
            this.Btn_Switch1.UseVisualStyleBackColor = false;
            this.Btn_Switch1.Visible = false;
            this.Btn_Switch1.Click += new EventHandler(this.Btn_Switch1_Click);
            this.Pic_Image.Dock = DockStyle.Fill;
            this.Pic_Image.Location = new Point(0, 0);
            this.Pic_Image.Name = "Pic_Image";
            this.Pic_Image.Size = new Size(0x2ba, 0x6a);
            this.Pic_Image.TabIndex = 0;
            this.Pic_Image.TabStop = false;
            this.Pic_Image.Click += new EventHandler(this.Pic_Image_Click);
            this.Pic_Image.MouseLeave += new EventHandler(this.Pic_Image_MouseLeave);
            this.Pic_Image.MouseMove += new MouseEventHandler(this.Pic_Image_MouseMove);
            this.Tim_Switch.Interval = 0x3e8;
            base.AutoScaleDimensions = new SizeF(96f, 96f);
            base.AutoScaleMode = AutoScaleMode.Dpi;
            base.Controls.Add(this.Pnl_Main);
            this.Font = new Font("微软雅黑", 9f);
            base.Name = "PictureSwitch";
            base.Size = new Size(0x2ba, 0x6a);
            base.Load += new EventHandler(this.PictureSwitch_Load);
            this.Pnl_Main.ResumeLayout(false);
            ((ISupportInitialize) this.Pic_Image).EndInit();
            base.ResumeLayout(false);
        }

        public void LoadImageList(List<Image> pImageList, Dictionary<string, string> pImageLinkDic)
        {
            this.ImageList = pImageList;
            if (this.ImageList != null)
            {
                int num = 0;
                this.ImageLinkDic = new Dictionary<Image, string>();
                foreach (string str in pImageLinkDic.Keys)
                {
                    string str2 = pImageLinkDic[str];
                    this.ImageLinkDic[pImageList[num++]] = str2;
                }
                this.SetImage(true);
                this.Tim_Switch.Interval = 0x4e20;
                this.Tim_Switch.Start();
                this.Tim_Switch.Tick += new EventHandler(this.Tim_Switch_Tick);
            }
        }

        private void Pic_Image_Click(object sender, EventArgs e)
        {
            if (this.Pic_Image.Image != null)
            {
                string pUrl = this.ImageLinkDic[this.Pic_Image.Image];
                if (pUrl != "")
                {
                    CommFunc.OpenWeb(pUrl);
                }
            }
        }

        private void Pic_Image_MouseLeave(object sender, EventArgs e)
        {
            if (this.ImageList != null)
            {
                this.IsMove = false;
            }
        }

        private void Pic_Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.ImageList != null)
            {
                this.IsMove = true;
            }
        }

        private void PictureSwitch_Load(object sender, EventArgs e)
        {
            this.Btn_Switch1.Parent = this.Pic_Image;
            this.Btn_Switch2.Parent = this.Pic_Image;
            List<Button> list = new List<Button> {
                this.Btn_Switch1,
                this.Btn_Switch2
            };
            this.ButtonList = list;
            CommFunc.SetButtonFormatFlat(this.ButtonList);
        }

        public void SetImage(bool pIsAdd)
        {
            int imageIndex = this.GetImageIndex(pIsAdd);
            this.Pic_Image.Image = this.ImageList[imageIndex];
        }

        public void Tim_Switch_Tick(object sender, EventArgs e)
        {
            if (!this.IsMove)
            {
                this.SetImage(true);
            }
        }

        public enum AnimationType
        {
            AW_BLEND = 0x80000,
            AW_CENTER = 0x10,
            AW_HOR_NEGATIVE = 2,
            AW_HOR_POSITIVE = 1,
            AW_VER_NEGATIVE = 8,
            AW_VER_POSITIVE = 4
        }
    }
}

