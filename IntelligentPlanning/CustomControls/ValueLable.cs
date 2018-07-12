namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ValueLable : UserControl
    {
        private List<CheckBox> CheckBoxList = null;
        private CheckBox Ckb_Copy;
        private CheckBox Ckb_OpenFile;
        private IContainer components = null;
        private Label Lbl_Hint;
        private TextBox Txt_Input;
        public List<string> ValueList = new List<string>();

        public ValueLable()
        {
            this.InitializeComponent();
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
                    this.Ckb_Copy,
                    this.Ckb_OpenFile
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
            }
        }

        private void Ckb_Copy_Click(object sender, EventArgs e)
        {
            CommFunc.CopyText(this.Txt_Input.Text);
        }

        private void Ckb_OpenFile_Click(object sender, EventArgs e)
        {
            if (base.Tag != null)
            {
                CommFunc.OpenFile(base.Tag.ToString());
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

        public TextBox GetTextBox() => 
            this.Txt_Input;

        private void InitializeComponent()
        {
            this.Txt_Input = new TextBox();
            this.Ckb_Copy = new CheckBox();
            this.Ckb_OpenFile = new CheckBox();
            this.Lbl_Hint = new Label();
            base.SuspendLayout();
            this.Txt_Input.BackColor = SystemColors.Window;
            this.Txt_Input.Dock = DockStyle.Fill;
            this.Txt_Input.Location = new Point(150, 0);
            this.Txt_Input.Multiline = true;
            this.Txt_Input.Name = "Txt_Input";
            this.Txt_Input.ReadOnly = true;
            this.Txt_Input.Size = new Size(0xce, 0x1a);
            this.Txt_Input.TabIndex = 0x88;
            this.Ckb_Copy.Appearance = Appearance.Button;
            this.Ckb_Copy.AutoCheck = false;
            this.Ckb_Copy.Dock = DockStyle.Right;
            this.Ckb_Copy.FlatAppearance.BorderSize = 0;
            this.Ckb_Copy.FlatStyle = FlatStyle.Flat;
            this.Ckb_Copy.Image = Resources.Copy;
            this.Ckb_Copy.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Copy.Location = new Point(0x164, 0);
            this.Ckb_Copy.Name = "Ckb_Copy";
            this.Ckb_Copy.Size = new Size(70, 0x1a);
            this.Ckb_Copy.TabIndex = 0x89;
            this.Ckb_Copy.Text = "复制";
            this.Ckb_Copy.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Copy.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Copy.UseVisualStyleBackColor = true;
            this.Ckb_Copy.Click += new EventHandler(this.Ckb_Copy_Click);
            this.Ckb_OpenFile.Appearance = Appearance.Button;
            this.Ckb_OpenFile.AutoCheck = false;
            this.Ckb_OpenFile.Dock = DockStyle.Right;
            this.Ckb_OpenFile.FlatAppearance.BorderSize = 0;
            this.Ckb_OpenFile.FlatStyle = FlatStyle.Flat;
            this.Ckb_OpenFile.Image = Resources.Fold;
            this.Ckb_OpenFile.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_OpenFile.Location = new Point(0x1aa, 0);
            this.Ckb_OpenFile.Name = "Ckb_OpenFile";
            this.Ckb_OpenFile.Size = new Size(70, 0x1a);
            this.Ckb_OpenFile.TabIndex = 0x8a;
            this.Ckb_OpenFile.Text = "打开";
            this.Ckb_OpenFile.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_OpenFile.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_OpenFile.UseVisualStyleBackColor = true;
            this.Ckb_OpenFile.Click += new EventHandler(this.Ckb_OpenFile_Click);
            this.Lbl_Hint.BorderStyle = BorderStyle.Fixed3D;
            this.Lbl_Hint.Dock = DockStyle.Left;
            this.Lbl_Hint.Location = new Point(0, 0);
            this.Lbl_Hint.Name = "Lbl_Hint";
            this.Lbl_Hint.Size = new Size(150, 0x1a);
            this.Lbl_Hint.TabIndex = 0x87;
            this.Lbl_Hint.Text = "标签";
            this.Lbl_Hint.TextAlign = ContentAlignment.MiddleCenter;
            base.AutoScaleDimensions = new SizeF(9f, 20f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.BorderStyle = BorderStyle.FixedSingle;
            base.Controls.Add(this.Txt_Input);
            base.Controls.Add(this.Ckb_Copy);
            base.Controls.Add(this.Ckb_OpenFile);
            base.Controls.Add(this.Lbl_Hint);
            this.Font = new Font("微软雅黑", 11f);
            base.Margin = new Padding(4, 5, 4, 5);
            base.Name = "ValueLable";
            base.Size = new Size(0x1f0, 0x1a);
            base.Load += new EventHandler(this.ValueLable_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void ValueLable_Load(object sender, EventArgs e)
        {
            List<Label> pLabelList = new List<Label> {
                this.Lbl_Hint
            };
            CommFunc.SetLabelFormat(pLabelList);
            List<CheckBox> list3 = new List<CheckBox> {
                this.Ckb_Copy,
                this.Ckb_OpenFile
            };
            this.CheckBoxList = list3;
            CommFunc.SetCheckBoxFormatFlat(this.CheckBoxList);
            this.BeautifyInterface();
        }

        public string Hint
        {
            get => 
                this.Lbl_Hint.Text;
            set
            {
                this.Lbl_Hint.Text = value;
            }
        }

        public string Value
        {
            get => 
                this.Txt_Input.Text;
            set
            {
                this.Txt_Input.Text = value;
            }
        }
    }
}

