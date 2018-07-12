namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class Shrink2 : UserControl
    {
        private CheckBox Ckb_Range1;
        private CheckBox Ckb_Range10;
        private CheckBox Ckb_Range11;
        private CheckBox Ckb_Range12;
        private CheckBox Ckb_Range13;
        private CheckBox Ckb_Range14;
        private CheckBox Ckb_Range15;
        private CheckBox Ckb_Range16;
        private CheckBox Ckb_Range17;
        private CheckBox Ckb_Range18;
        private CheckBox Ckb_Range19;
        private CheckBox Ckb_Range2;
        private CheckBox Ckb_Range3;
        private CheckBox Ckb_Range4;
        private CheckBox Ckb_Range5;
        private CheckBox Ckb_Range6;
        private CheckBox Ckb_Range7;
        private CheckBox Ckb_Range8;
        private CheckBox Ckb_Range9;
        private CheckBox Ckb_Select;
        private CheckBox Ckb_UnSelect;
        private IContainer components = null;
        private List<Control> ControlList = null;
        private GroupBox Grp_Main;
        public List<CheckBox> RangeList = null;
        private string RegConfigPath = "";
        private List<Control> SpecialControlList = null;
        public List<CheckBox> StandardList = null;

        public Shrink2()
        {
            this.InitializeComponent();
            List<CheckBox> list = new List<CheckBox> {
                this.Ckb_Range1,
                this.Ckb_Range2,
                this.Ckb_Range3,
                this.Ckb_Range4,
                this.Ckb_Range5,
                this.Ckb_Range6,
                this.Ckb_Range7,
                this.Ckb_Range8,
                this.Ckb_Range9,
                this.Ckb_Range10,
                this.Ckb_Range11,
                this.Ckb_Range12,
                this.Ckb_Range13,
                this.Ckb_Range14,
                this.Ckb_Range15,
                this.Ckb_Range16,
                this.Ckb_Range17,
                this.Ckb_Range18,
                this.Ckb_Range19
            };
            this.RangeList = list;
        }

        private void BeautifyInterface()
        {
            if (AppInfo.Account.Configuration.Beautify)
            {
                List<Control> pControlList = new List<Control>();
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list2 = new List<Control>();
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Grp_Main,
                    this.Ckb_Range1,
                    this.Ckb_Range2,
                    this.Ckb_Range3,
                    this.Ckb_Range4,
                    this.Ckb_Range5,
                    this.Ckb_Range6,
                    this.Ckb_Range7,
                    this.Ckb_Range8,
                    this.Ckb_Range9,
                    this.Ckb_Range10,
                    this.Ckb_Range11,
                    this.Ckb_Range12,
                    this.Ckb_Range13,
                    this.Ckb_Range14,
                    this.Ckb_Range15,
                    this.Ckb_Range16,
                    this.Ckb_Range17,
                    this.Ckb_Range18,
                    this.Ckb_Range19,
                    this.Ckb_Select,
                    this.Ckb_UnSelect
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
            }
        }

        public void Ckb_Select_Click(object sender, EventArgs e)
        {
            foreach (CheckBox box in this.RangeList)
            {
                box.Checked = true;
            }
        }

        public void Ckb_UnSelect_Click(object sender, EventArgs e)
        {
            foreach (CheckBox box in this.RangeList)
            {
                box.Checked = false;
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

        public List<int> GetSelectRange()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < this.RangeList.Count; i++)
            {
                int item = Convert.ToInt32(CommFunc.GetNumber(this.RangeList[i].Text));
                if (this.RangeList[i].Checked)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        private void InitializeComponent()
        {
            this.Ckb_UnSelect = new CheckBox();
            this.Ckb_Select = new CheckBox();
            this.Ckb_Range11 = new CheckBox();
            this.Ckb_Range10 = new CheckBox();
            this.Ckb_Range9 = new CheckBox();
            this.Ckb_Range8 = new CheckBox();
            this.Ckb_Range7 = new CheckBox();
            this.Ckb_Range6 = new CheckBox();
            this.Ckb_Range5 = new CheckBox();
            this.Ckb_Range4 = new CheckBox();
            this.Ckb_Range3 = new CheckBox();
            this.Ckb_Range2 = new CheckBox();
            this.Ckb_Range1 = new CheckBox();
            this.Ckb_Range19 = new CheckBox();
            this.Ckb_Range18 = new CheckBox();
            this.Ckb_Range17 = new CheckBox();
            this.Ckb_Range16 = new CheckBox();
            this.Ckb_Range15 = new CheckBox();
            this.Ckb_Range14 = new CheckBox();
            this.Ckb_Range13 = new CheckBox();
            this.Ckb_Range12 = new CheckBox();
            this.Grp_Main = new GroupBox();
            this.Grp_Main.SuspendLayout();
            base.SuspendLayout();
            this.Ckb_UnSelect.Appearance = Appearance.Button;
            this.Ckb_UnSelect.AutoCheck = false;
            this.Ckb_UnSelect.FlatAppearance.BorderSize = 0;
            this.Ckb_UnSelect.FlatStyle = FlatStyle.Flat;
            this.Ckb_UnSelect.Location = new Point(0xce, 0x81);
            this.Ckb_UnSelect.Name = "Ckb_UnSelect";
            this.Ckb_UnSelect.Size = new Size(0x19, 0x19);
            this.Ckb_UnSelect.TabIndex = 0x81;
            this.Ckb_UnSelect.Text = "清";
            this.Ckb_UnSelect.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_UnSelect.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_UnSelect.UseVisualStyleBackColor = true;
            this.Ckb_UnSelect.Click += new EventHandler(this.Ckb_UnSelect_Click);
            this.Ckb_Select.Appearance = Appearance.Button;
            this.Ckb_Select.AutoCheck = false;
            this.Ckb_Select.FlatAppearance.BorderSize = 0;
            this.Ckb_Select.FlatStyle = FlatStyle.Flat;
            this.Ckb_Select.Location = new Point(180, 0x81);
            this.Ckb_Select.Name = "Ckb_Select";
            this.Ckb_Select.Size = new Size(0x19, 0x19);
            this.Ckb_Select.TabIndex = 0x80;
            this.Ckb_Select.Text = "全";
            this.Ckb_Select.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Select.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Select.UseVisualStyleBackColor = true;
            this.Ckb_Select.Click += new EventHandler(this.Ckb_Select_Click);
            this.Ckb_Range11.AutoSize = true;
            this.Ckb_Range11.Location = new Point(6, 0x4c);
            this.Ckb_Range11.Name = "Ckb_Range11";
            this.Ckb_Range11.Size = new Size(0x29, 0x15);
            this.Ckb_Range11.TabIndex = 140;
            this.Ckb_Range11.Text = "10";
            this.Ckb_Range11.UseVisualStyleBackColor = true;
            this.Ckb_Range10.AutoSize = true;
            this.Ckb_Range10.Location = new Point(0xc2, 0x31);
            this.Ckb_Range10.Name = "Ckb_Range10";
            this.Ckb_Range10.Size = new Size(0x22, 0x15);
            this.Ckb_Range10.TabIndex = 0x8b;
            this.Ckb_Range10.Text = "9";
            this.Ckb_Range10.UseVisualStyleBackColor = true;
            this.Ckb_Range9.AutoSize = true;
            this.Ckb_Range9.Location = new Point(0x93, 0x31);
            this.Ckb_Range9.Name = "Ckb_Range9";
            this.Ckb_Range9.Size = new Size(0x22, 0x15);
            this.Ckb_Range9.TabIndex = 0x8a;
            this.Ckb_Range9.Text = "8";
            this.Ckb_Range9.UseVisualStyleBackColor = true;
            this.Ckb_Range8.AutoSize = true;
            this.Ckb_Range8.Location = new Point(100, 0x31);
            this.Ckb_Range8.Name = "Ckb_Range8";
            this.Ckb_Range8.Size = new Size(0x22, 0x15);
            this.Ckb_Range8.TabIndex = 0x89;
            this.Ckb_Range8.Text = "7";
            this.Ckb_Range8.UseVisualStyleBackColor = true;
            this.Ckb_Range7.AutoSize = true;
            this.Ckb_Range7.Location = new Point(0x35, 0x31);
            this.Ckb_Range7.Name = "Ckb_Range7";
            this.Ckb_Range7.Size = new Size(0x22, 0x15);
            this.Ckb_Range7.TabIndex = 0x88;
            this.Ckb_Range7.Text = "6";
            this.Ckb_Range7.UseVisualStyleBackColor = true;
            this.Ckb_Range6.AutoSize = true;
            this.Ckb_Range6.Location = new Point(6, 0x31);
            this.Ckb_Range6.Name = "Ckb_Range6";
            this.Ckb_Range6.Size = new Size(0x22, 0x15);
            this.Ckb_Range6.TabIndex = 0x87;
            this.Ckb_Range6.Text = "5";
            this.Ckb_Range6.UseVisualStyleBackColor = true;
            this.Ckb_Range5.AutoSize = true;
            this.Ckb_Range5.Location = new Point(0xc2, 0x16);
            this.Ckb_Range5.Name = "Ckb_Range5";
            this.Ckb_Range5.Size = new Size(0x22, 0x15);
            this.Ckb_Range5.TabIndex = 0x86;
            this.Ckb_Range5.Text = "4";
            this.Ckb_Range5.UseVisualStyleBackColor = true;
            this.Ckb_Range4.AutoSize = true;
            this.Ckb_Range4.Location = new Point(0x93, 0x16);
            this.Ckb_Range4.Name = "Ckb_Range4";
            this.Ckb_Range4.Size = new Size(0x22, 0x15);
            this.Ckb_Range4.TabIndex = 0x85;
            this.Ckb_Range4.Text = "3";
            this.Ckb_Range4.UseVisualStyleBackColor = true;
            this.Ckb_Range3.AutoSize = true;
            this.Ckb_Range3.Location = new Point(100, 0x16);
            this.Ckb_Range3.Name = "Ckb_Range3";
            this.Ckb_Range3.Size = new Size(0x22, 0x15);
            this.Ckb_Range3.TabIndex = 0x84;
            this.Ckb_Range3.Text = "2";
            this.Ckb_Range3.UseVisualStyleBackColor = true;
            this.Ckb_Range2.AutoSize = true;
            this.Ckb_Range2.Location = new Point(0x35, 0x16);
            this.Ckb_Range2.Name = "Ckb_Range2";
            this.Ckb_Range2.Size = new Size(0x22, 0x15);
            this.Ckb_Range2.TabIndex = 0x83;
            this.Ckb_Range2.Text = "1";
            this.Ckb_Range2.UseVisualStyleBackColor = true;
            this.Ckb_Range1.AutoSize = true;
            this.Ckb_Range1.Location = new Point(6, 0x16);
            this.Ckb_Range1.Name = "Ckb_Range1";
            this.Ckb_Range1.Size = new Size(0x22, 0x15);
            this.Ckb_Range1.TabIndex = 130;
            this.Ckb_Range1.Text = "0";
            this.Ckb_Range1.UseVisualStyleBackColor = true;
            this.Ckb_Range19.AutoSize = true;
            this.Ckb_Range19.Location = new Point(0x93, 0x67);
            this.Ckb_Range19.Name = "Ckb_Range19";
            this.Ckb_Range19.Size = new Size(0x29, 0x15);
            this.Ckb_Range19.TabIndex = 0x94;
            this.Ckb_Range19.Text = "18";
            this.Ckb_Range19.UseVisualStyleBackColor = true;
            this.Ckb_Range18.AutoSize = true;
            this.Ckb_Range18.Location = new Point(100, 0x67);
            this.Ckb_Range18.Name = "Ckb_Range18";
            this.Ckb_Range18.Size = new Size(0x29, 0x15);
            this.Ckb_Range18.TabIndex = 0x93;
            this.Ckb_Range18.Text = "17";
            this.Ckb_Range18.UseVisualStyleBackColor = true;
            this.Ckb_Range17.AutoSize = true;
            this.Ckb_Range17.Location = new Point(0x35, 0x67);
            this.Ckb_Range17.Name = "Ckb_Range17";
            this.Ckb_Range17.Size = new Size(0x29, 0x15);
            this.Ckb_Range17.TabIndex = 0x92;
            this.Ckb_Range17.Text = "16";
            this.Ckb_Range17.UseVisualStyleBackColor = true;
            this.Ckb_Range16.AutoSize = true;
            this.Ckb_Range16.Location = new Point(6, 0x67);
            this.Ckb_Range16.Name = "Ckb_Range16";
            this.Ckb_Range16.Size = new Size(0x29, 0x15);
            this.Ckb_Range16.TabIndex = 0x91;
            this.Ckb_Range16.Text = "15";
            this.Ckb_Range16.UseVisualStyleBackColor = true;
            this.Ckb_Range15.AutoSize = true;
            this.Ckb_Range15.Location = new Point(0xc2, 0x4c);
            this.Ckb_Range15.Name = "Ckb_Range15";
            this.Ckb_Range15.Size = new Size(0x29, 0x15);
            this.Ckb_Range15.TabIndex = 0x90;
            this.Ckb_Range15.Text = "14";
            this.Ckb_Range15.UseVisualStyleBackColor = true;
            this.Ckb_Range14.AutoSize = true;
            this.Ckb_Range14.Location = new Point(0x93, 0x4c);
            this.Ckb_Range14.Name = "Ckb_Range14";
            this.Ckb_Range14.Size = new Size(0x29, 0x15);
            this.Ckb_Range14.TabIndex = 0x8f;
            this.Ckb_Range14.Text = "13";
            this.Ckb_Range14.UseVisualStyleBackColor = true;
            this.Ckb_Range13.AutoSize = true;
            this.Ckb_Range13.Location = new Point(100, 0x4c);
            this.Ckb_Range13.Name = "Ckb_Range13";
            this.Ckb_Range13.Size = new Size(0x29, 0x15);
            this.Ckb_Range13.TabIndex = 0x8e;
            this.Ckb_Range13.Text = "12";
            this.Ckb_Range13.UseVisualStyleBackColor = true;
            this.Ckb_Range12.AutoSize = true;
            this.Ckb_Range12.Location = new Point(0x35, 0x4c);
            this.Ckb_Range12.Name = "Ckb_Range12";
            this.Ckb_Range12.Size = new Size(0x29, 0x15);
            this.Ckb_Range12.TabIndex = 0x8d;
            this.Ckb_Range12.Text = "11";
            this.Ckb_Range12.UseVisualStyleBackColor = true;
            this.Grp_Main.Controls.Add(this.Ckb_Range1);
            this.Grp_Main.Controls.Add(this.Ckb_Select);
            this.Grp_Main.Controls.Add(this.Ckb_UnSelect);
            this.Grp_Main.Controls.Add(this.Ckb_Range2);
            this.Grp_Main.Controls.Add(this.Ckb_Range3);
            this.Grp_Main.Controls.Add(this.Ckb_Range4);
            this.Grp_Main.Controls.Add(this.Ckb_Range5);
            this.Grp_Main.Controls.Add(this.Ckb_Range6);
            this.Grp_Main.Controls.Add(this.Ckb_Range7);
            this.Grp_Main.Controls.Add(this.Ckb_Range8);
            this.Grp_Main.Controls.Add(this.Ckb_Range9);
            this.Grp_Main.Controls.Add(this.Ckb_Range10);
            this.Grp_Main.Controls.Add(this.Ckb_Range11);
            this.Grp_Main.Controls.Add(this.Ckb_Range12);
            this.Grp_Main.Controls.Add(this.Ckb_Range13);
            this.Grp_Main.Controls.Add(this.Ckb_Range19);
            this.Grp_Main.Controls.Add(this.Ckb_Range14);
            this.Grp_Main.Controls.Add(this.Ckb_Range18);
            this.Grp_Main.Controls.Add(this.Ckb_Range15);
            this.Grp_Main.Controls.Add(this.Ckb_Range17);
            this.Grp_Main.Controls.Add(this.Ckb_Range16);
            this.Grp_Main.Dock = DockStyle.Fill;
            this.Grp_Main.Location = new Point(0, 0);
            this.Grp_Main.Name = "Grp_Main";
            this.Grp_Main.Size = new Size(240, 160);
            this.Grp_Main.TabIndex = 0xa3;
            this.Grp_Main.TabStop = false;
            this.Grp_Main.Text = "和值排除";
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.Grp_Main);
            this.Font = new Font("微软雅黑", 9f);
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "Shrink2";
            base.Size = new Size(240, 160);
            base.Load += new EventHandler(this.Shrink2_Load);
            this.Grp_Main.ResumeLayout(false);
            this.Grp_Main.PerformLayout();
            base.ResumeLayout(false);
        }

        public void ResetDefult()
        {
            foreach (CheckBox box in this.RangeList)
            {
                box.Checked = false;
            }
        }

        public void SaveControlInfoByReg()
        {
            CommFunc.SaveFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SaveSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
        }

        public void SetControlInfoByReg()
        {
            this.RegConfigPath = @"software\TUHAOPLUS\YXZXGJ\DlgConfig\永信在线挂机软件\" + base.Name;
            List<Control> list = new List<Control> {
                this
            };
            this.ControlList = list;
            this.SpecialControlList = new List<Control>();
            foreach (CheckBox box in this.RangeList)
            {
                this.ControlList.Add(box);
            }
            CommFunc.SetFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SetSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
        }

        private void Shrink2_Load(object sender, EventArgs e)
        {
            List<CheckBox> list = new List<CheckBox> {
                this.Ckb_Select,
                this.Ckb_UnSelect
            };
            this.StandardList = list;
            CommFunc.SetCheckBoxFormatFlat(this.StandardList);
            this.BeautifyInterface();
            this.SetControlInfoByReg();
        }
    }
}

