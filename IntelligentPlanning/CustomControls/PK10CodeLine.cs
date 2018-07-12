namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PK10CodeLine : UserControl
    {
        private List<Bitmap> CodeImageList = null;
        private IContainer components = null;
        private Label Lbl_CurrentCode1;
        private Label Lbl_CurrentCode10;
        private Label Lbl_CurrentCode2;
        private Label Lbl_CurrentCode3;
        private Label Lbl_CurrentCode4;
        private Label Lbl_CurrentCode5;
        private Label Lbl_CurrentCode6;
        private Label Lbl_CurrentCode7;
        private Label Lbl_CurrentCode8;
        private Label Lbl_CurrentCode9;
        private Panel Pnl_CurrentCode;

        public PK10CodeLine()
        {
            this.InitializeComponent();
            List<Bitmap> list = new List<Bitmap> {
                Resources.PK101,
                Resources.PK102,
                Resources.PK103,
                Resources.PK104,
                Resources.PK105,
                Resources.PK106,
                Resources.PK107,
                Resources.PK108,
                Resources.PK109,
                Resources.PK1010
            };
            this.CodeImageList = list;
        }

        private void BeautifyInterface()
        {
            if (AppInfo.Account.Configuration.Beautify)
            {
                List<Control> pControlList = new List<Control>();
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list2 = new List<Control> {
                    this.Pnl_CurrentCode
                };
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control>();
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
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

        private void InitializeComponent()
        {
            this.Pnl_CurrentCode = new Panel();
            this.Lbl_CurrentCode10 = new Label();
            this.Lbl_CurrentCode9 = new Label();
            this.Lbl_CurrentCode8 = new Label();
            this.Lbl_CurrentCode7 = new Label();
            this.Lbl_CurrentCode6 = new Label();
            this.Lbl_CurrentCode5 = new Label();
            this.Lbl_CurrentCode4 = new Label();
            this.Lbl_CurrentCode3 = new Label();
            this.Lbl_CurrentCode2 = new Label();
            this.Lbl_CurrentCode1 = new Label();
            this.Pnl_CurrentCode.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_CurrentCode.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_CurrentCode.Controls.Add(this.Lbl_CurrentCode10);
            this.Pnl_CurrentCode.Controls.Add(this.Lbl_CurrentCode9);
            this.Pnl_CurrentCode.Controls.Add(this.Lbl_CurrentCode8);
            this.Pnl_CurrentCode.Controls.Add(this.Lbl_CurrentCode7);
            this.Pnl_CurrentCode.Controls.Add(this.Lbl_CurrentCode6);
            this.Pnl_CurrentCode.Controls.Add(this.Lbl_CurrentCode5);
            this.Pnl_CurrentCode.Controls.Add(this.Lbl_CurrentCode4);
            this.Pnl_CurrentCode.Controls.Add(this.Lbl_CurrentCode3);
            this.Pnl_CurrentCode.Controls.Add(this.Lbl_CurrentCode2);
            this.Pnl_CurrentCode.Controls.Add(this.Lbl_CurrentCode1);
            this.Pnl_CurrentCode.Dock = DockStyle.Left;
            this.Pnl_CurrentCode.Location = new Point(0, 0);
            this.Pnl_CurrentCode.Name = "Pnl_CurrentCode";
            this.Pnl_CurrentCode.Size = new Size(0x161, 0x45);
            this.Pnl_CurrentCode.TabIndex = 12;
            this.Lbl_CurrentCode10.BackColor = Color.Transparent;
            this.Lbl_CurrentCode10.Dock = DockStyle.Left;
            this.Lbl_CurrentCode10.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode10.ForeColor = Color.White;
            this.Lbl_CurrentCode10.Location = new Point(0x13b, 0);
            this.Lbl_CurrentCode10.Name = "Lbl_CurrentCode10";
            this.Lbl_CurrentCode10.Size = new Size(0x23, 0x43);
            this.Lbl_CurrentCode10.TabIndex = 9;
            this.Lbl_CurrentCode10.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode9.BackColor = Color.Transparent;
            this.Lbl_CurrentCode9.Dock = DockStyle.Left;
            this.Lbl_CurrentCode9.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode9.ForeColor = Color.White;
            this.Lbl_CurrentCode9.Location = new Point(280, 0);
            this.Lbl_CurrentCode9.Name = "Lbl_CurrentCode9";
            this.Lbl_CurrentCode9.Size = new Size(0x23, 0x43);
            this.Lbl_CurrentCode9.TabIndex = 8;
            this.Lbl_CurrentCode9.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode8.BackColor = Color.Transparent;
            this.Lbl_CurrentCode8.Dock = DockStyle.Left;
            this.Lbl_CurrentCode8.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode8.ForeColor = Color.White;
            this.Lbl_CurrentCode8.Location = new Point(0xf5, 0);
            this.Lbl_CurrentCode8.Name = "Lbl_CurrentCode8";
            this.Lbl_CurrentCode8.Size = new Size(0x23, 0x43);
            this.Lbl_CurrentCode8.TabIndex = 7;
            this.Lbl_CurrentCode8.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode7.BackColor = Color.Transparent;
            this.Lbl_CurrentCode7.Dock = DockStyle.Left;
            this.Lbl_CurrentCode7.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode7.ForeColor = Color.White;
            this.Lbl_CurrentCode7.Location = new Point(210, 0);
            this.Lbl_CurrentCode7.Name = "Lbl_CurrentCode7";
            this.Lbl_CurrentCode7.Size = new Size(0x23, 0x43);
            this.Lbl_CurrentCode7.TabIndex = 6;
            this.Lbl_CurrentCode7.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode6.BackColor = Color.Transparent;
            this.Lbl_CurrentCode6.Dock = DockStyle.Left;
            this.Lbl_CurrentCode6.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode6.ForeColor = Color.White;
            this.Lbl_CurrentCode6.Location = new Point(0xaf, 0);
            this.Lbl_CurrentCode6.Name = "Lbl_CurrentCode6";
            this.Lbl_CurrentCode6.Size = new Size(0x23, 0x43);
            this.Lbl_CurrentCode6.TabIndex = 5;
            this.Lbl_CurrentCode6.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode5.BackColor = Color.Transparent;
            this.Lbl_CurrentCode5.Dock = DockStyle.Left;
            this.Lbl_CurrentCode5.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode5.ForeColor = Color.White;
            this.Lbl_CurrentCode5.Location = new Point(140, 0);
            this.Lbl_CurrentCode5.Name = "Lbl_CurrentCode5";
            this.Lbl_CurrentCode5.Size = new Size(0x23, 0x43);
            this.Lbl_CurrentCode5.TabIndex = 4;
            this.Lbl_CurrentCode5.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode4.BackColor = Color.Transparent;
            this.Lbl_CurrentCode4.Dock = DockStyle.Left;
            this.Lbl_CurrentCode4.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode4.ForeColor = Color.White;
            this.Lbl_CurrentCode4.Location = new Point(0x69, 0);
            this.Lbl_CurrentCode4.Name = "Lbl_CurrentCode4";
            this.Lbl_CurrentCode4.Size = new Size(0x23, 0x43);
            this.Lbl_CurrentCode4.TabIndex = 3;
            this.Lbl_CurrentCode4.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode3.BackColor = Color.Transparent;
            this.Lbl_CurrentCode3.Dock = DockStyle.Left;
            this.Lbl_CurrentCode3.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode3.ForeColor = Color.White;
            this.Lbl_CurrentCode3.Location = new Point(70, 0);
            this.Lbl_CurrentCode3.Name = "Lbl_CurrentCode3";
            this.Lbl_CurrentCode3.Size = new Size(0x23, 0x43);
            this.Lbl_CurrentCode3.TabIndex = 2;
            this.Lbl_CurrentCode3.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode2.BackColor = Color.Transparent;
            this.Lbl_CurrentCode2.Dock = DockStyle.Left;
            this.Lbl_CurrentCode2.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode2.ForeColor = Color.White;
            this.Lbl_CurrentCode2.Location = new Point(0x23, 0);
            this.Lbl_CurrentCode2.Name = "Lbl_CurrentCode2";
            this.Lbl_CurrentCode2.Size = new Size(0x23, 0x43);
            this.Lbl_CurrentCode2.TabIndex = 1;
            this.Lbl_CurrentCode2.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode1.BackColor = Color.Transparent;
            this.Lbl_CurrentCode1.Dock = DockStyle.Left;
            this.Lbl_CurrentCode1.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode1.ForeColor = Color.White;
            this.Lbl_CurrentCode1.Location = new Point(0, 0);
            this.Lbl_CurrentCode1.Name = "Lbl_CurrentCode1";
            this.Lbl_CurrentCode1.Size = new Size(0x23, 0x43);
            this.Lbl_CurrentCode1.TabIndex = 0;
            this.Lbl_CurrentCode1.TextAlign = ContentAlignment.MiddleCenter;
            base.AutoScaleDimensions = new SizeF(96f, 96f);
            base.AutoScaleMode = AutoScaleMode.Dpi;
            base.Controls.Add(this.Pnl_CurrentCode);
            this.Font = new Font("微软雅黑", 14f);
            base.Name = "PK10CodeLine";
            base.Size = new Size(0x161, 0x45);
            this.Pnl_CurrentCode.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void LoadOpenData(List<string> pCodeList)
        {
            this.BeautifyInterface();
            this.SetCodeImage(this.Lbl_CurrentCode1, pCodeList[0]);
            this.SetCodeImage(this.Lbl_CurrentCode2, pCodeList[1]);
            this.SetCodeImage(this.Lbl_CurrentCode3, pCodeList[2]);
            this.SetCodeImage(this.Lbl_CurrentCode4, pCodeList[3]);
            this.SetCodeImage(this.Lbl_CurrentCode5, pCodeList[4]);
            this.SetCodeImage(this.Lbl_CurrentCode6, pCodeList[5]);
            this.SetCodeImage(this.Lbl_CurrentCode7, pCodeList[6]);
            this.SetCodeImage(this.Lbl_CurrentCode8, pCodeList[7]);
            this.SetCodeImage(this.Lbl_CurrentCode9, pCodeList[8]);
            this.SetCodeImage(this.Lbl_CurrentCode10, pCodeList[9]);
        }

        private void SetCodeImage(Label pLabel, string pCode)
        {
            int num = Convert.ToInt32(pCode) - 1;
            Bitmap bitmap = this.CodeImageList[num];
            pLabel.Image = bitmap;
        }
    }
}

