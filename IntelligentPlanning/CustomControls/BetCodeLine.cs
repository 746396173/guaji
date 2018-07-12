namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class BetCodeLine : UserControl
    {
        private List<CheckBox> ActionList = new List<CheckBox>();
        private CheckBox Ckb_Code1;
        private CheckBox Ckb_Code10;
        private CheckBox Ckb_Code2;
        private CheckBox Ckb_Code3;
        private CheckBox Ckb_Code4;
        private CheckBox Ckb_Code5;
        private CheckBox Ckb_Code6;
        private CheckBox Ckb_Code7;
        private CheckBox Ckb_Code8;
        private CheckBox Ckb_Code9;
        private CheckBox Ckb_Da;
        private CheckBox Ckb_Ji;
        private CheckBox Ckb_Ou;
        private CheckBox Ckb_Select;
        private CheckBox Ckb_UnSelect;
        private CheckBox Ckb_Xiao;
        private List<CheckBox> CodeList = new List<CheckBox>();
        private IContainer components = null;
        private Color hotActionColor = Color.DarkGray;
        private Color hotCodeColor = AppInfo.hotColor;
        private Label Lbl_Hint;
        private Panel Pnl_Main;
        private Color pressedActionColor = Color.DimGray;
        private Color pressedCodeColor = AppInfo.appBackColor;

        public event EventHandler<EventArgs> CodeClick;

        public BetCodeLine()
        {
            this.InitializeComponent();
        }

        private void BetCodeLine_Load(object sender, EventArgs e)
        {
            List<CheckBox> list = new List<CheckBox> {
                this.Ckb_Code1,
                this.Ckb_Code2,
                this.Ckb_Code3,
                this.Ckb_Code4,
                this.Ckb_Code5,
                this.Ckb_Code6,
                this.Ckb_Code7,
                this.Ckb_Code8,
                this.Ckb_Code9,
                this.Ckb_Code10
            };
            this.CodeList = list;
            foreach (CheckBox box in this.CodeList)
            {
                box.CheckedChanged += new EventHandler(this.CodeList_CheckedChanged);
                box.FlatAppearance.BorderSize = 1;
                box.FlatAppearance.BorderColor = AppInfo.blackColor;
                box.FlatAppearance.MouseDownBackColor = box.FlatAppearance.CheckedBackColor = this.pressedCodeColor;
                box.FlatAppearance.MouseOverBackColor = this.hotCodeColor;
            }
            List<CheckBox> list2 = new List<CheckBox> {
                this.Ckb_Select,
                this.Ckb_Da,
                this.Ckb_Xiao,
                this.Ckb_Ji,
                this.Ckb_Ou,
                this.Ckb_UnSelect
            };
            this.ActionList = list2;
            foreach (CheckBox box in this.ActionList)
            {
                box.FlatAppearance.BorderSize = 1;
                box.FlatAppearance.BorderColor = AppInfo.blackColor;
                box.ForeColor = AppInfo.whiteColor;
                box.BackColor = this.pressedActionColor;
                box.FlatAppearance.MouseDownBackColor = box.FlatAppearance.CheckedBackColor = this.pressedActionColor;
                box.FlatAppearance.MouseOverBackColor = this.hotActionColor;
            }
        }

        private void Ckb_Da_Click(object sender, EventArgs e)
        {
            foreach (CheckBox box in this.CodeList)
            {
                int num = Convert.ToInt32(box.Text);
                box.Checked = num >= 6;
            }
        }

        private void Ckb_Ji_Click(object sender, EventArgs e)
        {
            foreach (CheckBox box in this.CodeList)
            {
                int num = Convert.ToInt32(box.Text);
                box.Checked = (num % 2) == 1;
            }
        }

        private void Ckb_Ou_Click(object sender, EventArgs e)
        {
            foreach (CheckBox box in this.CodeList)
            {
                int num = Convert.ToInt32(box.Text);
                box.Checked = (num % 2) == 0;
            }
        }

        private void Ckb_Select_Click(object sender, EventArgs e)
        {
            foreach (CheckBox box in this.CodeList)
            {
                box.Checked = true;
            }
        }

        private void Ckb_UnSelect_Click(object sender, EventArgs e)
        {
            foreach (CheckBox box in this.CodeList)
            {
                box.Checked = false;
            }
        }

        private void Ckb_Xiao_Click(object sender, EventArgs e)
        {
            foreach (CheckBox box in this.CodeList)
            {
                int num = Convert.ToInt32(box.Text);
                box.Checked = num <= 5;
            }
        }

        private void CodeList_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox box = sender as CheckBox;
            box.ForeColor = box.Checked ? AppInfo.whiteColor : AppInfo.blackColor;
            box.FlatAppearance.MouseOverBackColor = box.Checked ? this.pressedCodeColor : this.hotCodeColor;
            box.FlatAppearance.BorderColor = box.Checked ? this.pressedCodeColor : AppInfo.blackColor;
            if (this.CodeClick != null)
            {
                this.CodeClick(sender, e);
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
            this.Pnl_Main = new Panel();
            this.Ckb_UnSelect = new CheckBox();
            this.Ckb_Ou = new CheckBox();
            this.Ckb_Ji = new CheckBox();
            this.Ckb_Xiao = new CheckBox();
            this.Ckb_Da = new CheckBox();
            this.Ckb_Select = new CheckBox();
            this.Ckb_Code10 = new CheckBox();
            this.Ckb_Code9 = new CheckBox();
            this.Ckb_Code8 = new CheckBox();
            this.Ckb_Code7 = new CheckBox();
            this.Ckb_Code6 = new CheckBox();
            this.Ckb_Code5 = new CheckBox();
            this.Ckb_Code4 = new CheckBox();
            this.Ckb_Code3 = new CheckBox();
            this.Ckb_Code2 = new CheckBox();
            this.Ckb_Code1 = new CheckBox();
            this.Lbl_Hint = new Label();
            this.Pnl_Main.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_Main.Controls.Add(this.Ckb_UnSelect);
            this.Pnl_Main.Controls.Add(this.Ckb_Ou);
            this.Pnl_Main.Controls.Add(this.Ckb_Ji);
            this.Pnl_Main.Controls.Add(this.Ckb_Xiao);
            this.Pnl_Main.Controls.Add(this.Ckb_Da);
            this.Pnl_Main.Controls.Add(this.Ckb_Select);
            this.Pnl_Main.Controls.Add(this.Ckb_Code10);
            this.Pnl_Main.Controls.Add(this.Ckb_Code9);
            this.Pnl_Main.Controls.Add(this.Ckb_Code8);
            this.Pnl_Main.Controls.Add(this.Ckb_Code7);
            this.Pnl_Main.Controls.Add(this.Ckb_Code6);
            this.Pnl_Main.Controls.Add(this.Ckb_Code5);
            this.Pnl_Main.Controls.Add(this.Ckb_Code4);
            this.Pnl_Main.Controls.Add(this.Ckb_Code3);
            this.Pnl_Main.Controls.Add(this.Ckb_Code2);
            this.Pnl_Main.Controls.Add(this.Ckb_Code1);
            this.Pnl_Main.Controls.Add(this.Lbl_Hint);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0);
            this.Pnl_Main.Margin = new Padding(3, 4, 3, 4);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(520, 0x23);
            this.Pnl_Main.TabIndex = 0;
            this.Ckb_UnSelect.Appearance = Appearance.Button;
            this.Ckb_UnSelect.AutoCheck = false;
            this.Ckb_UnSelect.FlatAppearance.BorderSize = 0;
            this.Ckb_UnSelect.FlatStyle = FlatStyle.Flat;
            this.Ckb_UnSelect.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.Ckb_UnSelect.Location = new Point(490, 4);
            this.Ckb_UnSelect.Name = "Ckb_UnSelect";
            this.Ckb_UnSelect.Size = new Size(0x17, 0x1a);
            this.Ckb_UnSelect.TabIndex = 0x16;
            this.Ckb_UnSelect.Text = "清";
            this.Ckb_UnSelect.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_UnSelect.UseVisualStyleBackColor = true;
            this.Ckb_UnSelect.Click += new EventHandler(this.Ckb_UnSelect_Click);
            this.Ckb_Ou.Appearance = Appearance.Button;
            this.Ckb_Ou.AutoCheck = false;
            this.Ckb_Ou.FlatAppearance.BorderSize = 0;
            this.Ckb_Ou.FlatStyle = FlatStyle.Flat;
            this.Ckb_Ou.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.Ckb_Ou.Location = new Point(460, 4);
            this.Ckb_Ou.Name = "Ckb_Ou";
            this.Ckb_Ou.Size = new Size(0x17, 0x1a);
            this.Ckb_Ou.TabIndex = 0x15;
            this.Ckb_Ou.Text = "双";
            this.Ckb_Ou.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Ou.UseVisualStyleBackColor = true;
            this.Ckb_Ou.Click += new EventHandler(this.Ckb_Ou_Click);
            this.Ckb_Ji.Appearance = Appearance.Button;
            this.Ckb_Ji.AutoCheck = false;
            this.Ckb_Ji.FlatAppearance.BorderSize = 0;
            this.Ckb_Ji.FlatStyle = FlatStyle.Flat;
            this.Ckb_Ji.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.Ckb_Ji.Location = new Point(430, 4);
            this.Ckb_Ji.Name = "Ckb_Ji";
            this.Ckb_Ji.Size = new Size(0x17, 0x1a);
            this.Ckb_Ji.TabIndex = 20;
            this.Ckb_Ji.Text = "单";
            this.Ckb_Ji.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Ji.UseVisualStyleBackColor = true;
            this.Ckb_Ji.Click += new EventHandler(this.Ckb_Ji_Click);
            this.Ckb_Xiao.Appearance = Appearance.Button;
            this.Ckb_Xiao.AutoCheck = false;
            this.Ckb_Xiao.FlatAppearance.BorderSize = 0;
            this.Ckb_Xiao.FlatStyle = FlatStyle.Flat;
            this.Ckb_Xiao.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.Ckb_Xiao.Location = new Point(400, 4);
            this.Ckb_Xiao.Name = "Ckb_Xiao";
            this.Ckb_Xiao.Size = new Size(0x17, 0x1a);
            this.Ckb_Xiao.TabIndex = 0x13;
            this.Ckb_Xiao.Text = "小";
            this.Ckb_Xiao.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Xiao.UseVisualStyleBackColor = true;
            this.Ckb_Xiao.Click += new EventHandler(this.Ckb_Xiao_Click);
            this.Ckb_Da.Appearance = Appearance.Button;
            this.Ckb_Da.AutoCheck = false;
            this.Ckb_Da.FlatAppearance.BorderSize = 0;
            this.Ckb_Da.FlatStyle = FlatStyle.Flat;
            this.Ckb_Da.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.Ckb_Da.Location = new Point(370, 4);
            this.Ckb_Da.Name = "Ckb_Da";
            this.Ckb_Da.Size = new Size(0x17, 0x1a);
            this.Ckb_Da.TabIndex = 0x12;
            this.Ckb_Da.Text = "大";
            this.Ckb_Da.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Da.UseVisualStyleBackColor = true;
            this.Ckb_Da.Click += new EventHandler(this.Ckb_Da_Click);
            this.Ckb_Select.Appearance = Appearance.Button;
            this.Ckb_Select.AutoCheck = false;
            this.Ckb_Select.FlatAppearance.BorderSize = 0;
            this.Ckb_Select.FlatStyle = FlatStyle.Flat;
            this.Ckb_Select.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.Ckb_Select.Location = new Point(340, 4);
            this.Ckb_Select.Name = "Ckb_Select";
            this.Ckb_Select.Size = new Size(0x17, 0x1a);
            this.Ckb_Select.TabIndex = 0x11;
            this.Ckb_Select.Text = "全";
            this.Ckb_Select.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Select.UseVisualStyleBackColor = true;
            this.Ckb_Select.Click += new EventHandler(this.Ckb_Select_Click);
            this.Ckb_Code10.Appearance = Appearance.Button;
            this.Ckb_Code10.FlatAppearance.BorderSize = 0;
            this.Ckb_Code10.FlatStyle = FlatStyle.Flat;
            this.Ckb_Code10.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.Ckb_Code10.Location = new Point(300, 4);
            this.Ckb_Code10.Name = "Ckb_Code10";
            this.Ckb_Code10.Size = new Size(0x20, 0x1a);
            this.Ckb_Code10.TabIndex = 0x10;
            this.Ckb_Code10.Text = "10";
            this.Ckb_Code10.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Code10.UseVisualStyleBackColor = true;
            this.Ckb_Code9.Appearance = Appearance.Button;
            this.Ckb_Code9.FlatAppearance.BorderSize = 0;
            this.Ckb_Code9.FlatStyle = FlatStyle.Flat;
            this.Ckb_Code9.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.Ckb_Code9.Location = new Point(0x110, 4);
            this.Ckb_Code9.Name = "Ckb_Code9";
            this.Ckb_Code9.Size = new Size(0x15, 0x1a);
            this.Ckb_Code9.TabIndex = 15;
            this.Ckb_Code9.Text = "9";
            this.Ckb_Code9.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Code9.UseVisualStyleBackColor = true;
            this.Ckb_Code8.Appearance = Appearance.Button;
            this.Ckb_Code8.FlatAppearance.BorderSize = 0;
            this.Ckb_Code8.FlatStyle = FlatStyle.Flat;
            this.Ckb_Code8.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.Ckb_Code8.Location = new Point(0xf4, 4);
            this.Ckb_Code8.Name = "Ckb_Code8";
            this.Ckb_Code8.Size = new Size(0x15, 0x1a);
            this.Ckb_Code8.TabIndex = 14;
            this.Ckb_Code8.Text = "8";
            this.Ckb_Code8.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Code8.UseVisualStyleBackColor = true;
            this.Ckb_Code7.Appearance = Appearance.Button;
            this.Ckb_Code7.FlatAppearance.BorderSize = 0;
            this.Ckb_Code7.FlatStyle = FlatStyle.Flat;
            this.Ckb_Code7.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.Ckb_Code7.Location = new Point(0xd8, 4);
            this.Ckb_Code7.Name = "Ckb_Code7";
            this.Ckb_Code7.Size = new Size(0x15, 0x1a);
            this.Ckb_Code7.TabIndex = 13;
            this.Ckb_Code7.Text = "7";
            this.Ckb_Code7.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Code7.UseVisualStyleBackColor = true;
            this.Ckb_Code6.Appearance = Appearance.Button;
            this.Ckb_Code6.FlatAppearance.BorderSize = 0;
            this.Ckb_Code6.FlatStyle = FlatStyle.Flat;
            this.Ckb_Code6.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.Ckb_Code6.Location = new Point(0xbc, 4);
            this.Ckb_Code6.Name = "Ckb_Code6";
            this.Ckb_Code6.Size = new Size(0x15, 0x1a);
            this.Ckb_Code6.TabIndex = 12;
            this.Ckb_Code6.Text = "6";
            this.Ckb_Code6.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Code6.UseVisualStyleBackColor = true;
            this.Ckb_Code5.Appearance = Appearance.Button;
            this.Ckb_Code5.FlatAppearance.BorderSize = 0;
            this.Ckb_Code5.FlatStyle = FlatStyle.Flat;
            this.Ckb_Code5.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.Ckb_Code5.Location = new Point(160, 4);
            this.Ckb_Code5.Name = "Ckb_Code5";
            this.Ckb_Code5.Size = new Size(0x15, 0x1a);
            this.Ckb_Code5.TabIndex = 11;
            this.Ckb_Code5.Text = "5";
            this.Ckb_Code5.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Code5.UseVisualStyleBackColor = true;
            this.Ckb_Code4.Appearance = Appearance.Button;
            this.Ckb_Code4.FlatAppearance.BorderSize = 0;
            this.Ckb_Code4.FlatStyle = FlatStyle.Flat;
            this.Ckb_Code4.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.Ckb_Code4.Location = new Point(0x84, 4);
            this.Ckb_Code4.Name = "Ckb_Code4";
            this.Ckb_Code4.Size = new Size(0x15, 0x1a);
            this.Ckb_Code4.TabIndex = 10;
            this.Ckb_Code4.Text = "4";
            this.Ckb_Code4.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Code4.UseVisualStyleBackColor = true;
            this.Ckb_Code3.Appearance = Appearance.Button;
            this.Ckb_Code3.FlatAppearance.BorderSize = 0;
            this.Ckb_Code3.FlatStyle = FlatStyle.Flat;
            this.Ckb_Code3.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.Ckb_Code3.Location = new Point(0x68, 4);
            this.Ckb_Code3.Name = "Ckb_Code3";
            this.Ckb_Code3.Size = new Size(0x15, 0x1a);
            this.Ckb_Code3.TabIndex = 9;
            this.Ckb_Code3.Text = "3";
            this.Ckb_Code3.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Code3.UseVisualStyleBackColor = true;
            this.Ckb_Code2.Appearance = Appearance.Button;
            this.Ckb_Code2.FlatAppearance.BorderSize = 0;
            this.Ckb_Code2.FlatStyle = FlatStyle.Flat;
            this.Ckb_Code2.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.Ckb_Code2.Location = new Point(0x4c, 4);
            this.Ckb_Code2.Name = "Ckb_Code2";
            this.Ckb_Code2.Size = new Size(0x15, 0x1a);
            this.Ckb_Code2.TabIndex = 8;
            this.Ckb_Code2.Text = "2";
            this.Ckb_Code2.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Code2.UseVisualStyleBackColor = true;
            this.Ckb_Code1.Appearance = Appearance.Button;
            this.Ckb_Code1.FlatAppearance.BorderSize = 0;
            this.Ckb_Code1.FlatStyle = FlatStyle.Flat;
            this.Ckb_Code1.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.Ckb_Code1.Location = new Point(0x30, 4);
            this.Ckb_Code1.Name = "Ckb_Code1";
            this.Ckb_Code1.Size = new Size(0x15, 0x1a);
            this.Ckb_Code1.TabIndex = 7;
            this.Ckb_Code1.Text = "1";
            this.Ckb_Code1.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Code1.UseVisualStyleBackColor = true;
            this.Lbl_Hint.AutoSize = true;
            this.Lbl_Hint.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.Lbl_Hint.Location = new Point(2, 9);
            this.Lbl_Hint.Name = "Lbl_Hint";
            this.Lbl_Hint.Size = new Size(0x2c, 0x11);
            this.Lbl_Hint.TabIndex = 0;
            this.Lbl_Hint.Text = "万位：";
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.Pnl_Main);
            this.Font = new Font("微软雅黑", 9f);
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "BetCodeLine";
            base.Size = new Size(520, 0x23);
            base.Load += new EventHandler(this.BetCodeLine_Load);
            this.Pnl_Main.ResumeLayout(false);
            this.Pnl_Main.PerformLayout();
            base.ResumeLayout(false);
        }

        public string CodeString
        {
            get
            {
                string str = "";
                foreach (CheckBox box in this.CodeList)
                {
                    if (box.Checked)
                    {
                        str = str + box.Text;
                    }
                }
                return str;
            }
            set
            {
                foreach (CheckBox box in this.CodeList)
                {
                    box.Checked = value.Contains(box.Text);
                }
            }
        }

        public List<string> GetCodeList
        {
            get
            {
                List<string> list = new List<string>();
                foreach (CheckBox box in this.CodeList)
                {
                    if (box.Checked)
                    {
                        list.Add(box.Text);
                    }
                }
                return list;
            }
        }

        public Label GetHintLable =>
            this.Lbl_Hint;

        public string Hint
        {
            get => 
                this.Lbl_Hint.Text;
            set
            {
                this.Lbl_Hint.Text = value;
            }
        }
    }
}

