namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class CodeLine : UserControl
    {
        private List<Bitmap> CodeImageList = null;
        private IContainer components = null;
        private List<Label> LableList = null;
        private Label Lbl_Code1LH;
        private Label Lbl_Code2LH;
        private Label Lbl_Code3LH;
        private Label Lbl_Code4LH;
        private Label Lbl_Code5LH;
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
        private Label Lbl_GYH1;
        private Label Lbl_GYH2;
        private Label Lbl_GYH3;
        private Label Lbl_Time;
        private Panel Pnl_CurrentCode;
        private List<string> RedTextList = null;

        public CodeLine()
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
            List<Label> list2 = new List<Label> {
                this.Lbl_GYH1,
                this.Lbl_GYH2,
                this.Lbl_GYH3,
                this.Lbl_Code1LH,
                this.Lbl_Code2LH,
                this.Lbl_Code3LH,
                this.Lbl_Code4LH,
                this.Lbl_Code5LH
            };
            this.LableList = list2;
            List<string> list3 = new List<string> { 
                "大",
                "双",
                "龙"
            };
            this.RedTextList = list3;
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
            this.Lbl_Time = new Label();
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
            this.Lbl_GYH1 = new Label();
            this.Lbl_GYH2 = new Label();
            this.Lbl_GYH3 = new Label();
            this.Lbl_Code1LH = new Label();
            this.Lbl_Code2LH = new Label();
            this.Lbl_Code3LH = new Label();
            this.Lbl_Code4LH = new Label();
            this.Lbl_Code5LH = new Label();
            this.Pnl_CurrentCode.SuspendLayout();
            base.SuspendLayout();
            this.Lbl_Time.BorderStyle = BorderStyle.FixedSingle;
            this.Lbl_Time.Dock = DockStyle.Left;
            this.Lbl_Time.Location = new Point(0, 0);
            this.Lbl_Time.Name = "Lbl_Time";
            this.Lbl_Time.Size = new Size(0x41, 0x2c);
            this.Lbl_Time.TabIndex = 0;
            this.Lbl_Time.TextAlign = ContentAlignment.MiddleCenter;
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
            this.Pnl_CurrentCode.Location = new Point(0x41, 0);
            this.Pnl_CurrentCode.Name = "Pnl_CurrentCode";
            this.Pnl_CurrentCode.Size = new Size(0x12e, 0x2c);
            this.Pnl_CurrentCode.TabIndex = 12;
            this.Lbl_CurrentCode10.Dock = DockStyle.Left;
            this.Lbl_CurrentCode10.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode10.ForeColor = Color.White;
            this.Lbl_CurrentCode10.Location = new Point(270, 0);
            this.Lbl_CurrentCode10.Name = "Lbl_CurrentCode10";
            this.Lbl_CurrentCode10.Size = new Size(30, 0x2a);
            this.Lbl_CurrentCode10.TabIndex = 9;
            this.Lbl_CurrentCode10.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode9.Dock = DockStyle.Left;
            this.Lbl_CurrentCode9.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode9.ForeColor = Color.White;
            this.Lbl_CurrentCode9.Location = new Point(240, 0);
            this.Lbl_CurrentCode9.Name = "Lbl_CurrentCode9";
            this.Lbl_CurrentCode9.Size = new Size(30, 0x2a);
            this.Lbl_CurrentCode9.TabIndex = 8;
            this.Lbl_CurrentCode9.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode8.Dock = DockStyle.Left;
            this.Lbl_CurrentCode8.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode8.ForeColor = Color.White;
            this.Lbl_CurrentCode8.Location = new Point(210, 0);
            this.Lbl_CurrentCode8.Name = "Lbl_CurrentCode8";
            this.Lbl_CurrentCode8.Size = new Size(30, 0x2a);
            this.Lbl_CurrentCode8.TabIndex = 7;
            this.Lbl_CurrentCode8.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode7.Dock = DockStyle.Left;
            this.Lbl_CurrentCode7.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode7.ForeColor = Color.White;
            this.Lbl_CurrentCode7.Location = new Point(180, 0);
            this.Lbl_CurrentCode7.Name = "Lbl_CurrentCode7";
            this.Lbl_CurrentCode7.Size = new Size(30, 0x2a);
            this.Lbl_CurrentCode7.TabIndex = 6;
            this.Lbl_CurrentCode7.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode6.Dock = DockStyle.Left;
            this.Lbl_CurrentCode6.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode6.ForeColor = Color.White;
            this.Lbl_CurrentCode6.Location = new Point(150, 0);
            this.Lbl_CurrentCode6.Name = "Lbl_CurrentCode6";
            this.Lbl_CurrentCode6.Size = new Size(30, 0x2a);
            this.Lbl_CurrentCode6.TabIndex = 5;
            this.Lbl_CurrentCode6.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode5.Dock = DockStyle.Left;
            this.Lbl_CurrentCode5.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode5.ForeColor = Color.White;
            this.Lbl_CurrentCode5.Location = new Point(120, 0);
            this.Lbl_CurrentCode5.Name = "Lbl_CurrentCode5";
            this.Lbl_CurrentCode5.Size = new Size(30, 0x2a);
            this.Lbl_CurrentCode5.TabIndex = 4;
            this.Lbl_CurrentCode5.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode4.Dock = DockStyle.Left;
            this.Lbl_CurrentCode4.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode4.ForeColor = Color.White;
            this.Lbl_CurrentCode4.Location = new Point(90, 0);
            this.Lbl_CurrentCode4.Name = "Lbl_CurrentCode4";
            this.Lbl_CurrentCode4.Size = new Size(30, 0x2a);
            this.Lbl_CurrentCode4.TabIndex = 3;
            this.Lbl_CurrentCode4.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode3.Dock = DockStyle.Left;
            this.Lbl_CurrentCode3.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode3.ForeColor = Color.White;
            this.Lbl_CurrentCode3.Location = new Point(60, 0);
            this.Lbl_CurrentCode3.Name = "Lbl_CurrentCode3";
            this.Lbl_CurrentCode3.Size = new Size(30, 0x2a);
            this.Lbl_CurrentCode3.TabIndex = 2;
            this.Lbl_CurrentCode3.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode2.Dock = DockStyle.Left;
            this.Lbl_CurrentCode2.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode2.ForeColor = Color.White;
            this.Lbl_CurrentCode2.Location = new Point(30, 0);
            this.Lbl_CurrentCode2.Name = "Lbl_CurrentCode2";
            this.Lbl_CurrentCode2.Size = new Size(30, 0x2a);
            this.Lbl_CurrentCode2.TabIndex = 1;
            this.Lbl_CurrentCode2.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_CurrentCode1.Dock = DockStyle.Left;
            this.Lbl_CurrentCode1.Font = new Font("微软雅黑", 26.25f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Lbl_CurrentCode1.ForeColor = Color.White;
            this.Lbl_CurrentCode1.Location = new Point(0, 0);
            this.Lbl_CurrentCode1.Name = "Lbl_CurrentCode1";
            this.Lbl_CurrentCode1.Size = new Size(30, 0x2a);
            this.Lbl_CurrentCode1.TabIndex = 0;
            this.Lbl_CurrentCode1.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_GYH1.BorderStyle = BorderStyle.FixedSingle;
            this.Lbl_GYH1.Dock = DockStyle.Left;
            this.Lbl_GYH1.Location = new Point(0x16f, 0);
            this.Lbl_GYH1.Name = "Lbl_GYH1";
            this.Lbl_GYH1.Size = new Size(40, 0x2c);
            this.Lbl_GYH1.TabIndex = 13;
            this.Lbl_GYH1.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_GYH2.BorderStyle = BorderStyle.FixedSingle;
            this.Lbl_GYH2.Dock = DockStyle.Left;
            this.Lbl_GYH2.Location = new Point(0x197, 0);
            this.Lbl_GYH2.Name = "Lbl_GYH2";
            this.Lbl_GYH2.Size = new Size(40, 0x2c);
            this.Lbl_GYH2.TabIndex = 14;
            this.Lbl_GYH2.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_GYH3.BorderStyle = BorderStyle.FixedSingle;
            this.Lbl_GYH3.Dock = DockStyle.Left;
            this.Lbl_GYH3.Location = new Point(0x1bf, 0);
            this.Lbl_GYH3.Name = "Lbl_GYH3";
            this.Lbl_GYH3.Size = new Size(40, 0x2c);
            this.Lbl_GYH3.TabIndex = 15;
            this.Lbl_GYH3.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_Code1LH.BorderStyle = BorderStyle.FixedSingle;
            this.Lbl_Code1LH.Dock = DockStyle.Left;
            this.Lbl_Code1LH.Location = new Point(0x1e7, 0);
            this.Lbl_Code1LH.Name = "Lbl_Code1LH";
            this.Lbl_Code1LH.Size = new Size(40, 0x2c);
            this.Lbl_Code1LH.TabIndex = 0x10;
            this.Lbl_Code1LH.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_Code2LH.BorderStyle = BorderStyle.FixedSingle;
            this.Lbl_Code2LH.Dock = DockStyle.Left;
            this.Lbl_Code2LH.Location = new Point(0x20f, 0);
            this.Lbl_Code2LH.Name = "Lbl_Code2LH";
            this.Lbl_Code2LH.Size = new Size(40, 0x2c);
            this.Lbl_Code2LH.TabIndex = 0x11;
            this.Lbl_Code2LH.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_Code3LH.BorderStyle = BorderStyle.FixedSingle;
            this.Lbl_Code3LH.Dock = DockStyle.Left;
            this.Lbl_Code3LH.Location = new Point(0x237, 0);
            this.Lbl_Code3LH.Name = "Lbl_Code3LH";
            this.Lbl_Code3LH.Size = new Size(40, 0x2c);
            this.Lbl_Code3LH.TabIndex = 0x12;
            this.Lbl_Code3LH.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_Code4LH.BorderStyle = BorderStyle.FixedSingle;
            this.Lbl_Code4LH.Dock = DockStyle.Left;
            this.Lbl_Code4LH.Location = new Point(0x25f, 0);
            this.Lbl_Code4LH.Name = "Lbl_Code4LH";
            this.Lbl_Code4LH.Size = new Size(40, 0x2c);
            this.Lbl_Code4LH.TabIndex = 0x13;
            this.Lbl_Code4LH.TextAlign = ContentAlignment.MiddleCenter;
            this.Lbl_Code5LH.BorderStyle = BorderStyle.FixedSingle;
            this.Lbl_Code5LH.Dock = DockStyle.Left;
            this.Lbl_Code5LH.Location = new Point(0x287, 0);
            this.Lbl_Code5LH.Name = "Lbl_Code5LH";
            this.Lbl_Code5LH.Size = new Size(40, 0x2c);
            this.Lbl_Code5LH.TabIndex = 20;
            this.Lbl_Code5LH.TextAlign = ContentAlignment.MiddleCenter;
            base.AutoScaleDimensions = new SizeF(96f, 96f);
            base.AutoScaleMode = AutoScaleMode.Dpi;
            base.Controls.Add(this.Lbl_Code5LH);
            base.Controls.Add(this.Lbl_Code4LH);
            base.Controls.Add(this.Lbl_Code3LH);
            base.Controls.Add(this.Lbl_Code2LH);
            base.Controls.Add(this.Lbl_Code1LH);
            base.Controls.Add(this.Lbl_GYH3);
            base.Controls.Add(this.Lbl_GYH2);
            base.Controls.Add(this.Lbl_GYH1);
            base.Controls.Add(this.Pnl_CurrentCode);
            base.Controls.Add(this.Lbl_Time);
            this.Font = new Font("微软雅黑", 14f);
            base.Name = "CodeLine";
            base.Size = new Size(690, 0x2c);
            this.Pnl_CurrentCode.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void LoadOpenData(ConfigurationStatus.OpenData pData)
        {
            List<string> codeList = pData.CodeList;
            this.SetCodeImage(this.Lbl_CurrentCode1, codeList[0]);
            this.SetCodeImage(this.Lbl_CurrentCode2, codeList[1]);
            this.SetCodeImage(this.Lbl_CurrentCode3, codeList[2]);
            this.SetCodeImage(this.Lbl_CurrentCode4, codeList[3]);
            this.SetCodeImage(this.Lbl_CurrentCode5, codeList[4]);
            this.SetCodeImage(this.Lbl_CurrentCode6, codeList[5]);
            this.SetCodeImage(this.Lbl_CurrentCode7, codeList[6]);
            this.SetCodeImage(this.Lbl_CurrentCode8, codeList[7]);
            this.SetCodeImage(this.Lbl_CurrentCode9, codeList[8]);
            this.SetCodeImage(this.Lbl_CurrentCode10, codeList[9]);
            string pNum = (Convert.ToInt32(codeList[0]) + Convert.ToInt32(codeList[1])).ToString();
            this.Lbl_GYH1.Text = pNum;
            this.Lbl_GYH2.Text = CommFunc.CountDX(pNum, 11);
            this.Lbl_GYH3.Text = CommFunc.CountDS(pNum);
            for (int i = 0; i < 5; i++)
            {
                this.LableList[3 + i].Text = CommFunc.CountLH(codeList[i], codeList[9 - i]);
            }
            foreach (Label label in this.LableList)
            {
                if (this.RedTextList.Contains(label.Text))
                {
                    label.ForeColor = AppInfo.redForeColor;
                }
                else
                {
                    label.ForeColor = AppInfo.blackColor;
                }
            }
        }

        private void SetCodeImage(Label pLabel, string pCode)
        {
            int num = Convert.ToInt32(pCode) - 1;
            Bitmap bitmap = this.CodeImageList[num];
            pLabel.Image = bitmap;
        }
    }
}

