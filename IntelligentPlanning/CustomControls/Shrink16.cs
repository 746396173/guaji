namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class Shrink16 : UserControl
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
        private CheckBox Ckb_Range20;
        private CheckBox Ckb_Range21;
        private CheckBox Ckb_Range22;
        private CheckBox Ckb_Range23;
        private CheckBox Ckb_Range24;
        private CheckBox Ckb_Range25;
        private CheckBox Ckb_Range26;
        private CheckBox Ckb_Range27;
        private CheckBox Ckb_Range28;
        private CheckBox Ckb_Range29;
        private CheckBox Ckb_Range3;
        private CheckBox Ckb_Range30;
        private CheckBox Ckb_Range4;
        private CheckBox Ckb_Range5;
        private CheckBox Ckb_Range6;
        private CheckBox Ckb_Range7;
        private CheckBox Ckb_Range8;
        private CheckBox Ckb_Range9;
        private CheckBox Ckb_Select1;
        private CheckBox Ckb_Select2;
        private CheckBox Ckb_Select3;
        private CheckBox Ckb_UnSelect1;
        private CheckBox Ckb_UnSelect2;
        private CheckBox Ckb_UnSelect3;
        private IContainer components = null;
        private List<Control> ControlList = null;
        private GroupBox Grp_Main;
        private Label Lbl_Hint1;
        private Label Lbl_Hint2;
        private Label Lbl_Hint3;
        public List<CheckBox> RangeList1 = null;
        public List<CheckBox> RangeList2 = null;
        public List<CheckBox> RangeList3 = null;
        private string RegConfigPath = "";
        private List<Control> SpecialControlList = null;
        public List<CheckBox> StandardList = null;

        public Shrink16()
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
                this.Ckb_Range10
            };
            this.RangeList1 = list;
            List<CheckBox> list2 = new List<CheckBox> {
                this.Ckb_Range11,
                this.Ckb_Range12,
                this.Ckb_Range13,
                this.Ckb_Range14,
                this.Ckb_Range15,
                this.Ckb_Range16,
                this.Ckb_Range17,
                this.Ckb_Range18,
                this.Ckb_Range19,
                this.Ckb_Range20
            };
            this.RangeList2 = list2;
            List<CheckBox> list3 = new List<CheckBox> {
                this.Ckb_Range21,
                this.Ckb_Range22,
                this.Ckb_Range23,
                this.Ckb_Range24,
                this.Ckb_Range25,
                this.Ckb_Range26,
                this.Ckb_Range27,
                this.Ckb_Range28,
                this.Ckb_Range29,
                this.Ckb_Range30
            };
            this.RangeList3 = list3;
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
                    this.Ckb_Range20,
                    this.Ckb_Range21,
                    this.Ckb_Range22,
                    this.Ckb_Range23,
                    this.Ckb_Range24,
                    this.Ckb_Range25,
                    this.Ckb_Range26,
                    this.Ckb_Range27,
                    this.Ckb_Range28,
                    this.Ckb_Range29,
                    this.Ckb_Range30,
                    this.Ckb_Select1,
                    this.Ckb_UnSelect1,
                    this.Ckb_Select2,
                    this.Ckb_UnSelect2,
                    this.Ckb_Select3,
                    this.Ckb_UnSelect3
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
            }
        }

        public void Ckb_Select1_Click(object sender, EventArgs e)
        {
            foreach (CheckBox box in this.RangeList1)
            {
                box.Checked = true;
            }
        }

        private void Ckb_Select2_Click(object sender, EventArgs e)
        {
            foreach (CheckBox box in this.RangeList2)
            {
                box.Checked = true;
            }
        }

        private void Ckb_Select3_Click(object sender, EventArgs e)
        {
            foreach (CheckBox box in this.RangeList3)
            {
                box.Checked = true;
            }
        }

        public void Ckb_UnSelect1_Click(object sender, EventArgs e)
        {
            foreach (CheckBox box in this.RangeList1)
            {
                box.Checked = false;
            }
        }

        private void Ckb_UnSelect2_Click(object sender, EventArgs e)
        {
            foreach (CheckBox box in this.RangeList2)
            {
                box.Checked = false;
            }
        }

        private void Ckb_UnSelect3_Click(object sender, EventArgs e)
        {
            foreach (CheckBox box in this.RangeList3)
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

        public List<int> GetSelectRange1()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < this.RangeList1.Count; i++)
            {
                int item = Convert.ToInt32(CommFunc.GetNumber(this.RangeList1[i].Text));
                if (this.RangeList1[i].Checked)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public List<int> GetSelectRange2()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < this.RangeList2.Count; i++)
            {
                int item = Convert.ToInt32(CommFunc.GetNumber(this.RangeList2[i].Text));
                if (this.RangeList2[i].Checked)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public List<int> GetSelectRange3()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < this.RangeList3.Count; i++)
            {
                int item = Convert.ToInt32(CommFunc.GetNumber(this.RangeList3[i].Text));
                if (this.RangeList3[i].Checked)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        private void InitializeComponent()
        {
            this.Ckb_UnSelect1 = new CheckBox();
            this.Ckb_Select1 = new CheckBox();
            this.Ckb_Range9 = new CheckBox();
            this.Ckb_Range8 = new CheckBox();
            this.Ckb_Range7 = new CheckBox();
            this.Ckb_Range6 = new CheckBox();
            this.Ckb_Range5 = new CheckBox();
            this.Ckb_Range4 = new CheckBox();
            this.Ckb_Range3 = new CheckBox();
            this.Ckb_Range2 = new CheckBox();
            this.Ckb_Range1 = new CheckBox();
            this.Grp_Main = new GroupBox();
            this.Lbl_Hint3 = new Label();
            this.Ckb_Range30 = new CheckBox();
            this.Ckb_Range21 = new CheckBox();
            this.Ckb_Select3 = new CheckBox();
            this.Ckb_UnSelect3 = new CheckBox();
            this.Ckb_Range22 = new CheckBox();
            this.Ckb_Range23 = new CheckBox();
            this.Ckb_Range24 = new CheckBox();
            this.Ckb_Range25 = new CheckBox();
            this.Ckb_Range26 = new CheckBox();
            this.Ckb_Range27 = new CheckBox();
            this.Ckb_Range28 = new CheckBox();
            this.Ckb_Range29 = new CheckBox();
            this.Lbl_Hint2 = new Label();
            this.Ckb_Range20 = new CheckBox();
            this.Ckb_Range11 = new CheckBox();
            this.Ckb_Select2 = new CheckBox();
            this.Ckb_UnSelect2 = new CheckBox();
            this.Ckb_Range12 = new CheckBox();
            this.Ckb_Range13 = new CheckBox();
            this.Ckb_Range14 = new CheckBox();
            this.Ckb_Range15 = new CheckBox();
            this.Ckb_Range16 = new CheckBox();
            this.Ckb_Range17 = new CheckBox();
            this.Ckb_Range18 = new CheckBox();
            this.Ckb_Range19 = new CheckBox();
            this.Lbl_Hint1 = new Label();
            this.Ckb_Range10 = new CheckBox();
            this.Grp_Main.SuspendLayout();
            base.SuspendLayout();
            this.Ckb_UnSelect1.Appearance = Appearance.Button;
            this.Ckb_UnSelect1.AutoCheck = false;
            this.Ckb_UnSelect1.FlatAppearance.BorderSize = 0;
            this.Ckb_UnSelect1.FlatStyle = FlatStyle.Flat;
            this.Ckb_UnSelect1.Location = new Point(0x1e4, 0x13);
            this.Ckb_UnSelect1.Name = "Ckb_UnSelect1";
            this.Ckb_UnSelect1.Size = new Size(0x19, 0x19);
            this.Ckb_UnSelect1.TabIndex = 0x81;
            this.Ckb_UnSelect1.Text = "清";
            this.Ckb_UnSelect1.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_UnSelect1.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_UnSelect1.UseVisualStyleBackColor = true;
            this.Ckb_UnSelect1.Click += new EventHandler(this.Ckb_UnSelect1_Click);
            this.Ckb_Select1.Appearance = Appearance.Button;
            this.Ckb_Select1.AutoCheck = false;
            this.Ckb_Select1.FlatAppearance.BorderSize = 0;
            this.Ckb_Select1.FlatStyle = FlatStyle.Flat;
            this.Ckb_Select1.Location = new Point(0x1ca, 0x13);
            this.Ckb_Select1.Name = "Ckb_Select1";
            this.Ckb_Select1.Size = new Size(0x19, 0x19);
            this.Ckb_Select1.TabIndex = 0x80;
            this.Ckb_Select1.Text = "全";
            this.Ckb_Select1.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Select1.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Select1.UseVisualStyleBackColor = true;
            this.Ckb_Select1.Click += new EventHandler(this.Ckb_Select1_Click);
            this.Ckb_Range9.AutoSize = true;
            this.Ckb_Range9.Location = new Point(0x178, 0x16);
            this.Ckb_Range9.Name = "Ckb_Range9";
            this.Ckb_Range9.Size = new Size(0x22, 0x15);
            this.Ckb_Range9.TabIndex = 0x8a;
            this.Ckb_Range9.Text = "8";
            this.Ckb_Range9.UseVisualStyleBackColor = true;
            this.Ckb_Range8.AutoSize = true;
            this.Ckb_Range8.Location = new Point(0x150, 0x16);
            this.Ckb_Range8.Name = "Ckb_Range8";
            this.Ckb_Range8.Size = new Size(0x22, 0x15);
            this.Ckb_Range8.TabIndex = 0x89;
            this.Ckb_Range8.Text = "7";
            this.Ckb_Range8.UseVisualStyleBackColor = true;
            this.Ckb_Range7.AutoSize = true;
            this.Ckb_Range7.Location = new Point(0x128, 0x16);
            this.Ckb_Range7.Name = "Ckb_Range7";
            this.Ckb_Range7.Size = new Size(0x22, 0x15);
            this.Ckb_Range7.TabIndex = 0x88;
            this.Ckb_Range7.Text = "6";
            this.Ckb_Range7.UseVisualStyleBackColor = true;
            this.Ckb_Range6.AutoSize = true;
            this.Ckb_Range6.Location = new Point(0x100, 0x16);
            this.Ckb_Range6.Name = "Ckb_Range6";
            this.Ckb_Range6.Size = new Size(0x22, 0x15);
            this.Ckb_Range6.TabIndex = 0x87;
            this.Ckb_Range6.Text = "5";
            this.Ckb_Range6.UseVisualStyleBackColor = true;
            this.Ckb_Range5.AutoSize = true;
            this.Ckb_Range5.Location = new Point(0xd8, 0x16);
            this.Ckb_Range5.Name = "Ckb_Range5";
            this.Ckb_Range5.Size = new Size(0x22, 0x15);
            this.Ckb_Range5.TabIndex = 0x86;
            this.Ckb_Range5.Text = "4";
            this.Ckb_Range5.UseVisualStyleBackColor = true;
            this.Ckb_Range4.AutoSize = true;
            this.Ckb_Range4.Location = new Point(0xb0, 0x16);
            this.Ckb_Range4.Name = "Ckb_Range4";
            this.Ckb_Range4.Size = new Size(0x22, 0x15);
            this.Ckb_Range4.TabIndex = 0x85;
            this.Ckb_Range4.Text = "3";
            this.Ckb_Range4.UseVisualStyleBackColor = true;
            this.Ckb_Range3.AutoSize = true;
            this.Ckb_Range3.Location = new Point(0x88, 0x16);
            this.Ckb_Range3.Name = "Ckb_Range3";
            this.Ckb_Range3.Size = new Size(0x22, 0x15);
            this.Ckb_Range3.TabIndex = 0x84;
            this.Ckb_Range3.Text = "2";
            this.Ckb_Range3.UseVisualStyleBackColor = true;
            this.Ckb_Range2.AutoSize = true;
            this.Ckb_Range2.Location = new Point(0x60, 0x16);
            this.Ckb_Range2.Name = "Ckb_Range2";
            this.Ckb_Range2.Size = new Size(0x22, 0x15);
            this.Ckb_Range2.TabIndex = 0x83;
            this.Ckb_Range2.Text = "1";
            this.Ckb_Range2.UseVisualStyleBackColor = true;
            this.Ckb_Range1.AutoSize = true;
            this.Ckb_Range1.Location = new Point(0x38, 0x16);
            this.Ckb_Range1.Name = "Ckb_Range1";
            this.Ckb_Range1.Size = new Size(0x22, 0x15);
            this.Ckb_Range1.TabIndex = 130;
            this.Ckb_Range1.Text = "0";
            this.Ckb_Range1.UseVisualStyleBackColor = true;
            this.Grp_Main.Controls.Add(this.Lbl_Hint3);
            this.Grp_Main.Controls.Add(this.Ckb_Range30);
            this.Grp_Main.Controls.Add(this.Ckb_Range21);
            this.Grp_Main.Controls.Add(this.Ckb_Select3);
            this.Grp_Main.Controls.Add(this.Ckb_UnSelect3);
            this.Grp_Main.Controls.Add(this.Ckb_Range22);
            this.Grp_Main.Controls.Add(this.Ckb_Range23);
            this.Grp_Main.Controls.Add(this.Ckb_Range24);
            this.Grp_Main.Controls.Add(this.Ckb_Range25);
            this.Grp_Main.Controls.Add(this.Ckb_Range26);
            this.Grp_Main.Controls.Add(this.Ckb_Range27);
            this.Grp_Main.Controls.Add(this.Ckb_Range28);
            this.Grp_Main.Controls.Add(this.Ckb_Range29);
            this.Grp_Main.Controls.Add(this.Lbl_Hint2);
            this.Grp_Main.Controls.Add(this.Ckb_Range20);
            this.Grp_Main.Controls.Add(this.Ckb_Range11);
            this.Grp_Main.Controls.Add(this.Ckb_Select2);
            this.Grp_Main.Controls.Add(this.Ckb_UnSelect2);
            this.Grp_Main.Controls.Add(this.Ckb_Range12);
            this.Grp_Main.Controls.Add(this.Ckb_Range13);
            this.Grp_Main.Controls.Add(this.Ckb_Range14);
            this.Grp_Main.Controls.Add(this.Ckb_Range15);
            this.Grp_Main.Controls.Add(this.Ckb_Range16);
            this.Grp_Main.Controls.Add(this.Ckb_Range17);
            this.Grp_Main.Controls.Add(this.Ckb_Range18);
            this.Grp_Main.Controls.Add(this.Ckb_Range19);
            this.Grp_Main.Controls.Add(this.Lbl_Hint1);
            this.Grp_Main.Controls.Add(this.Ckb_Range10);
            this.Grp_Main.Controls.Add(this.Ckb_Range1);
            this.Grp_Main.Controls.Add(this.Ckb_Select1);
            this.Grp_Main.Controls.Add(this.Ckb_UnSelect1);
            this.Grp_Main.Controls.Add(this.Ckb_Range2);
            this.Grp_Main.Controls.Add(this.Ckb_Range3);
            this.Grp_Main.Controls.Add(this.Ckb_Range4);
            this.Grp_Main.Controls.Add(this.Ckb_Range5);
            this.Grp_Main.Controls.Add(this.Ckb_Range6);
            this.Grp_Main.Controls.Add(this.Ckb_Range7);
            this.Grp_Main.Controls.Add(this.Ckb_Range8);
            this.Grp_Main.Controls.Add(this.Ckb_Range9);
            this.Grp_Main.Dock = DockStyle.Fill;
            this.Grp_Main.Location = new Point(0, 0);
            this.Grp_Main.Name = "Grp_Main";
            this.Grp_Main.Size = new Size(0x203, 0x69);
            this.Grp_Main.TabIndex = 0xa3;
            this.Grp_Main.TabStop = false;
            this.Grp_Main.Text = "定位排除";
            this.Lbl_Hint3.AutoSize = true;
            this.Lbl_Hint3.Location = new Point(3, 0x4f);
            this.Lbl_Hint3.Name = "Lbl_Hint3";
            this.Lbl_Hint3.Size = new Size(0x2c, 0x11);
            this.Lbl_Hint3.TabIndex = 0xa6;
            this.Lbl_Hint3.Text = "个位：";
            this.Ckb_Range30.AutoSize = true;
            this.Ckb_Range30.Location = new Point(0x1a0, 0x4e);
            this.Ckb_Range30.Name = "Ckb_Range30";
            this.Ckb_Range30.Size = new Size(0x22, 0x15);
            this.Ckb_Range30.TabIndex = 0xa5;
            this.Ckb_Range30.Text = "9";
            this.Ckb_Range30.UseVisualStyleBackColor = true;
            this.Ckb_Range21.AutoSize = true;
            this.Ckb_Range21.Location = new Point(0x38, 0x4e);
            this.Ckb_Range21.Name = "Ckb_Range21";
            this.Ckb_Range21.Size = new Size(0x22, 0x15);
            this.Ckb_Range21.TabIndex = 0x9c;
            this.Ckb_Range21.Text = "0";
            this.Ckb_Range21.UseVisualStyleBackColor = true;
            this.Ckb_Select3.Appearance = Appearance.Button;
            this.Ckb_Select3.AutoCheck = false;
            this.Ckb_Select3.FlatAppearance.BorderSize = 0;
            this.Ckb_Select3.FlatStyle = FlatStyle.Flat;
            this.Ckb_Select3.Location = new Point(0x1ca, 0x4b);
            this.Ckb_Select3.Name = "Ckb_Select3";
            this.Ckb_Select3.Size = new Size(0x19, 0x19);
            this.Ckb_Select3.TabIndex = 0x9a;
            this.Ckb_Select3.Text = "全";
            this.Ckb_Select3.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Select3.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Select3.UseVisualStyleBackColor = true;
            this.Ckb_Select3.Click += new EventHandler(this.Ckb_Select3_Click);
            this.Ckb_UnSelect3.Appearance = Appearance.Button;
            this.Ckb_UnSelect3.AutoCheck = false;
            this.Ckb_UnSelect3.FlatAppearance.BorderSize = 0;
            this.Ckb_UnSelect3.FlatStyle = FlatStyle.Flat;
            this.Ckb_UnSelect3.Location = new Point(0x1e4, 0x4b);
            this.Ckb_UnSelect3.Name = "Ckb_UnSelect3";
            this.Ckb_UnSelect3.Size = new Size(0x19, 0x19);
            this.Ckb_UnSelect3.TabIndex = 0x9b;
            this.Ckb_UnSelect3.Text = "清";
            this.Ckb_UnSelect3.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_UnSelect3.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_UnSelect3.UseVisualStyleBackColor = true;
            this.Ckb_UnSelect3.Click += new EventHandler(this.Ckb_UnSelect3_Click);
            this.Ckb_Range22.AutoSize = true;
            this.Ckb_Range22.Location = new Point(0x60, 0x4e);
            this.Ckb_Range22.Name = "Ckb_Range22";
            this.Ckb_Range22.Size = new Size(0x22, 0x15);
            this.Ckb_Range22.TabIndex = 0x9d;
            this.Ckb_Range22.Text = "1";
            this.Ckb_Range22.UseVisualStyleBackColor = true;
            this.Ckb_Range23.AutoSize = true;
            this.Ckb_Range23.Location = new Point(0x88, 0x4e);
            this.Ckb_Range23.Name = "Ckb_Range23";
            this.Ckb_Range23.Size = new Size(0x22, 0x15);
            this.Ckb_Range23.TabIndex = 0x9e;
            this.Ckb_Range23.Text = "2";
            this.Ckb_Range23.UseVisualStyleBackColor = true;
            this.Ckb_Range24.AutoSize = true;
            this.Ckb_Range24.Location = new Point(0xb0, 0x4e);
            this.Ckb_Range24.Name = "Ckb_Range24";
            this.Ckb_Range24.Size = new Size(0x22, 0x15);
            this.Ckb_Range24.TabIndex = 0x9f;
            this.Ckb_Range24.Text = "3";
            this.Ckb_Range24.UseVisualStyleBackColor = true;
            this.Ckb_Range25.AutoSize = true;
            this.Ckb_Range25.Location = new Point(0xd8, 0x4e);
            this.Ckb_Range25.Name = "Ckb_Range25";
            this.Ckb_Range25.Size = new Size(0x22, 0x15);
            this.Ckb_Range25.TabIndex = 160;
            this.Ckb_Range25.Text = "4";
            this.Ckb_Range25.UseVisualStyleBackColor = true;
            this.Ckb_Range26.AutoSize = true;
            this.Ckb_Range26.Location = new Point(0x100, 0x4e);
            this.Ckb_Range26.Name = "Ckb_Range26";
            this.Ckb_Range26.Size = new Size(0x22, 0x15);
            this.Ckb_Range26.TabIndex = 0xa1;
            this.Ckb_Range26.Text = "5";
            this.Ckb_Range26.UseVisualStyleBackColor = true;
            this.Ckb_Range27.AutoSize = true;
            this.Ckb_Range27.Location = new Point(0x128, 0x4e);
            this.Ckb_Range27.Name = "Ckb_Range27";
            this.Ckb_Range27.Size = new Size(0x22, 0x15);
            this.Ckb_Range27.TabIndex = 0xa2;
            this.Ckb_Range27.Text = "6";
            this.Ckb_Range27.UseVisualStyleBackColor = true;
            this.Ckb_Range28.AutoSize = true;
            this.Ckb_Range28.Location = new Point(0x150, 0x4e);
            this.Ckb_Range28.Name = "Ckb_Range28";
            this.Ckb_Range28.Size = new Size(0x22, 0x15);
            this.Ckb_Range28.TabIndex = 0xa3;
            this.Ckb_Range28.Text = "7";
            this.Ckb_Range28.UseVisualStyleBackColor = true;
            this.Ckb_Range29.AutoSize = true;
            this.Ckb_Range29.Location = new Point(0x178, 0x4e);
            this.Ckb_Range29.Name = "Ckb_Range29";
            this.Ckb_Range29.Size = new Size(0x22, 0x15);
            this.Ckb_Range29.TabIndex = 0xa4;
            this.Ckb_Range29.Text = "8";
            this.Ckb_Range29.UseVisualStyleBackColor = true;
            this.Lbl_Hint2.AutoSize = true;
            this.Lbl_Hint2.Location = new Point(3, 0x33);
            this.Lbl_Hint2.Name = "Lbl_Hint2";
            this.Lbl_Hint2.Size = new Size(0x2c, 0x11);
            this.Lbl_Hint2.TabIndex = 0x99;
            this.Lbl_Hint2.Text = "十位：";
            this.Ckb_Range20.AutoSize = true;
            this.Ckb_Range20.Location = new Point(0x1a0, 50);
            this.Ckb_Range20.Name = "Ckb_Range20";
            this.Ckb_Range20.Size = new Size(0x22, 0x15);
            this.Ckb_Range20.TabIndex = 0x98;
            this.Ckb_Range20.Text = "9";
            this.Ckb_Range20.UseVisualStyleBackColor = true;
            this.Ckb_Range11.AutoSize = true;
            this.Ckb_Range11.Location = new Point(0x38, 50);
            this.Ckb_Range11.Name = "Ckb_Range11";
            this.Ckb_Range11.Size = new Size(0x22, 0x15);
            this.Ckb_Range11.TabIndex = 0x8f;
            this.Ckb_Range11.Text = "0";
            this.Ckb_Range11.UseVisualStyleBackColor = true;
            this.Ckb_Select2.Appearance = Appearance.Button;
            this.Ckb_Select2.AutoCheck = false;
            this.Ckb_Select2.FlatAppearance.BorderSize = 0;
            this.Ckb_Select2.FlatStyle = FlatStyle.Flat;
            this.Ckb_Select2.Location = new Point(0x1ca, 0x2f);
            this.Ckb_Select2.Name = "Ckb_Select2";
            this.Ckb_Select2.Size = new Size(0x19, 0x19);
            this.Ckb_Select2.TabIndex = 0x8d;
            this.Ckb_Select2.Text = "全";
            this.Ckb_Select2.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Select2.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Select2.UseVisualStyleBackColor = true;
            this.Ckb_Select2.Click += new EventHandler(this.Ckb_Select2_Click);
            this.Ckb_UnSelect2.Appearance = Appearance.Button;
            this.Ckb_UnSelect2.AutoCheck = false;
            this.Ckb_UnSelect2.FlatAppearance.BorderSize = 0;
            this.Ckb_UnSelect2.FlatStyle = FlatStyle.Flat;
            this.Ckb_UnSelect2.Location = new Point(0x1e4, 0x2f);
            this.Ckb_UnSelect2.Name = "Ckb_UnSelect2";
            this.Ckb_UnSelect2.Size = new Size(0x19, 0x19);
            this.Ckb_UnSelect2.TabIndex = 0x8e;
            this.Ckb_UnSelect2.Text = "清";
            this.Ckb_UnSelect2.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_UnSelect2.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_UnSelect2.UseVisualStyleBackColor = true;
            this.Ckb_UnSelect2.Click += new EventHandler(this.Ckb_UnSelect2_Click);
            this.Ckb_Range12.AutoSize = true;
            this.Ckb_Range12.Location = new Point(0x60, 50);
            this.Ckb_Range12.Name = "Ckb_Range12";
            this.Ckb_Range12.Size = new Size(0x22, 0x15);
            this.Ckb_Range12.TabIndex = 0x90;
            this.Ckb_Range12.Text = "1";
            this.Ckb_Range12.UseVisualStyleBackColor = true;
            this.Ckb_Range13.AutoSize = true;
            this.Ckb_Range13.Location = new Point(0x88, 50);
            this.Ckb_Range13.Name = "Ckb_Range13";
            this.Ckb_Range13.Size = new Size(0x22, 0x15);
            this.Ckb_Range13.TabIndex = 0x91;
            this.Ckb_Range13.Text = "2";
            this.Ckb_Range13.UseVisualStyleBackColor = true;
            this.Ckb_Range14.AutoSize = true;
            this.Ckb_Range14.Location = new Point(0xb0, 50);
            this.Ckb_Range14.Name = "Ckb_Range14";
            this.Ckb_Range14.Size = new Size(0x22, 0x15);
            this.Ckb_Range14.TabIndex = 0x92;
            this.Ckb_Range14.Text = "3";
            this.Ckb_Range14.UseVisualStyleBackColor = true;
            this.Ckb_Range15.AutoSize = true;
            this.Ckb_Range15.Location = new Point(0xd8, 50);
            this.Ckb_Range15.Name = "Ckb_Range15";
            this.Ckb_Range15.Size = new Size(0x22, 0x15);
            this.Ckb_Range15.TabIndex = 0x93;
            this.Ckb_Range15.Text = "4";
            this.Ckb_Range15.UseVisualStyleBackColor = true;
            this.Ckb_Range16.AutoSize = true;
            this.Ckb_Range16.Location = new Point(0x100, 50);
            this.Ckb_Range16.Name = "Ckb_Range16";
            this.Ckb_Range16.Size = new Size(0x22, 0x15);
            this.Ckb_Range16.TabIndex = 0x94;
            this.Ckb_Range16.Text = "5";
            this.Ckb_Range16.UseVisualStyleBackColor = true;
            this.Ckb_Range17.AutoSize = true;
            this.Ckb_Range17.Location = new Point(0x128, 50);
            this.Ckb_Range17.Name = "Ckb_Range17";
            this.Ckb_Range17.Size = new Size(0x22, 0x15);
            this.Ckb_Range17.TabIndex = 0x95;
            this.Ckb_Range17.Text = "6";
            this.Ckb_Range17.UseVisualStyleBackColor = true;
            this.Ckb_Range18.AutoSize = true;
            this.Ckb_Range18.Location = new Point(0x150, 50);
            this.Ckb_Range18.Name = "Ckb_Range18";
            this.Ckb_Range18.Size = new Size(0x22, 0x15);
            this.Ckb_Range18.TabIndex = 150;
            this.Ckb_Range18.Text = "7";
            this.Ckb_Range18.UseVisualStyleBackColor = true;
            this.Ckb_Range19.AutoSize = true;
            this.Ckb_Range19.Location = new Point(0x178, 50);
            this.Ckb_Range19.Name = "Ckb_Range19";
            this.Ckb_Range19.Size = new Size(0x22, 0x15);
            this.Ckb_Range19.TabIndex = 0x97;
            this.Ckb_Range19.Text = "8";
            this.Ckb_Range19.UseVisualStyleBackColor = true;
            this.Lbl_Hint1.AutoSize = true;
            this.Lbl_Hint1.Location = new Point(3, 0x17);
            this.Lbl_Hint1.Name = "Lbl_Hint1";
            this.Lbl_Hint1.Size = new Size(0x2c, 0x11);
            this.Lbl_Hint1.TabIndex = 140;
            this.Lbl_Hint1.Text = "百位：";
            this.Ckb_Range10.AutoSize = true;
            this.Ckb_Range10.Location = new Point(0x1a0, 0x16);
            this.Ckb_Range10.Name = "Ckb_Range10";
            this.Ckb_Range10.Size = new Size(0x22, 0x15);
            this.Ckb_Range10.TabIndex = 0x8b;
            this.Ckb_Range10.Text = "9";
            this.Ckb_Range10.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.Grp_Main);
            this.Font = new Font("微软雅黑", 9f);
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "Shrink16";
            base.Size = new Size(0x203, 0x69);
            base.Load += new EventHandler(this.Shrink16_Load);
            this.Grp_Main.ResumeLayout(false);
            this.Grp_Main.PerformLayout();
            base.ResumeLayout(false);
        }

        public void ResetDefult()
        {
            foreach (CheckBox box in this.RangeList1)
            {
                box.Checked = false;
            }
            foreach (CheckBox box in this.RangeList2)
            {
                box.Checked = false;
            }
            foreach (CheckBox box in this.RangeList3)
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
            foreach (CheckBox box in this.RangeList1)
            {
                this.ControlList.Add(box);
            }
            foreach (CheckBox box in this.RangeList2)
            {
                this.ControlList.Add(box);
            }
            foreach (CheckBox box in this.RangeList3)
            {
                this.ControlList.Add(box);
            }
            CommFunc.SetFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SetSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
        }

        private void Shrink16_Load(object sender, EventArgs e)
        {
            List<CheckBox> list = new List<CheckBox> {
                this.Ckb_Select1,
                this.Ckb_UnSelect1,
                this.Ckb_Select2,
                this.Ckb_UnSelect2,
                this.Ckb_Select3,
                this.Ckb_UnSelect3
            };
            this.StandardList = list;
            CommFunc.SetCheckBoxFormatFlat(this.StandardList);
            this.BeautifyInterface();
            this.SetControlInfoByReg();
        }
    }
}

