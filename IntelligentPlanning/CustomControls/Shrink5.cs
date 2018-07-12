namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class Shrink5 : UserControl
    {
        private CheckBox Ckb_Range1;
        private CheckBox Ckb_Range2;
        private CheckBox Ckb_Range3;
        private CheckBox Ckb_Range4;
        private CheckBox Ckb_Select;
        private CheckBox Ckb_UnSelect;
        private IContainer components = null;
        private List<Control> ControlList = null;
        private GroupBox Grp_Main;
        public List<CheckBox> RangeList = null;
        private string RegConfigPath = "";
        private List<Control> SpecialControlList = null;
        public List<CheckBox> StandardList = null;

        public Shrink5()
        {
            this.InitializeComponent();
            List<CheckBox> list = new List<CheckBox> {
                this.Ckb_Range1,
                this.Ckb_Range2,
                this.Ckb_Range3,
                this.Ckb_Range4
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

        public List<string> GetSelectRange()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < this.RangeList.Count; i++)
            {
                string text = this.RangeList[i].Text;
                if (this.RangeList[i].Checked)
                {
                    list.Add(text);
                }
            }
            return list;
        }

        private void InitializeComponent()
        {
            this.Ckb_UnSelect = new CheckBox();
            this.Ckb_Select = new CheckBox();
            this.Ckb_Range4 = new CheckBox();
            this.Ckb_Range3 = new CheckBox();
            this.Ckb_Range2 = new CheckBox();
            this.Ckb_Range1 = new CheckBox();
            this.Grp_Main = new GroupBox();
            this.Grp_Main.SuspendLayout();
            base.SuspendLayout();
            this.Ckb_UnSelect.Appearance = Appearance.Button;
            this.Ckb_UnSelect.AutoCheck = false;
            this.Ckb_UnSelect.FlatAppearance.BorderSize = 0;
            this.Ckb_UnSelect.FlatStyle = FlatStyle.Flat;
            this.Ckb_UnSelect.Location = new Point(0xf4, 0x13);
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
            this.Ckb_Select.Location = new Point(0xda, 0x13);
            this.Ckb_Select.Name = "Ckb_Select";
            this.Ckb_Select.Size = new Size(0x19, 0x19);
            this.Ckb_Select.TabIndex = 0x80;
            this.Ckb_Select.Text = "全";
            this.Ckb_Select.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Select.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Select.UseVisualStyleBackColor = true;
            this.Ckb_Select.Click += new EventHandler(this.Ckb_Select_Click);
            this.Ckb_Range4.AutoSize = true;
            this.Ckb_Range4.Location = new Point(0xa5, 0x16);
            this.Ckb_Range4.Name = "Ckb_Range4";
            this.Ckb_Range4.Size = new Size(0x33, 0x15);
            this.Ckb_Range4.TabIndex = 0x85;
            this.Ckb_Range4.Text = "奇奇";
            this.Ckb_Range4.UseVisualStyleBackColor = true;
            this.Ckb_Range3.AutoSize = true;
            this.Ckb_Range3.Location = new Point(0x70, 0x16);
            this.Ckb_Range3.Name = "Ckb_Range3";
            this.Ckb_Range3.Size = new Size(0x33, 0x15);
            this.Ckb_Range3.TabIndex = 0x84;
            this.Ckb_Range3.Text = "奇偶";
            this.Ckb_Range3.UseVisualStyleBackColor = true;
            this.Ckb_Range2.AutoSize = true;
            this.Ckb_Range2.Location = new Point(0x3b, 0x16);
            this.Ckb_Range2.Name = "Ckb_Range2";
            this.Ckb_Range2.Size = new Size(0x33, 0x15);
            this.Ckb_Range2.TabIndex = 0x83;
            this.Ckb_Range2.Text = "偶奇";
            this.Ckb_Range2.UseVisualStyleBackColor = true;
            this.Ckb_Range1.AutoSize = true;
            this.Ckb_Range1.Location = new Point(6, 0x16);
            this.Ckb_Range1.Name = "Ckb_Range1";
            this.Ckb_Range1.Size = new Size(0x33, 0x15);
            this.Ckb_Range1.TabIndex = 130;
            this.Ckb_Range1.Text = "偶偶";
            this.Ckb_Range1.UseVisualStyleBackColor = true;
            this.Grp_Main.Controls.Add(this.Ckb_Range1);
            this.Grp_Main.Controls.Add(this.Ckb_Select);
            this.Grp_Main.Controls.Add(this.Ckb_UnSelect);
            this.Grp_Main.Controls.Add(this.Ckb_Range2);
            this.Grp_Main.Controls.Add(this.Ckb_Range3);
            this.Grp_Main.Controls.Add(this.Ckb_Range4);
            this.Grp_Main.Dock = DockStyle.Fill;
            this.Grp_Main.Location = new Point(0, 0);
            this.Grp_Main.Name = "Grp_Main";
            this.Grp_Main.Size = new Size(0x113, 0x37);
            this.Grp_Main.TabIndex = 0xa3;
            this.Grp_Main.TabStop = false;
            this.Grp_Main.Text = "奇偶排除";
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.Grp_Main);
            this.Font = new Font("微软雅黑", 9f);
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "Shrink5";
            base.Size = new Size(0x113, 0x37);
            base.Load += new EventHandler(this.Shrink5_Load);
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

        private void Shrink5_Load(object sender, EventArgs e)
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

