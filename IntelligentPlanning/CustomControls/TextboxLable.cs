namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TextboxLable : UserControl
    {
        private IContainer components = null;
        private Label Lbl_Hint;
        private TextBox Txt_Input;

        public TextboxLable()
        {
            this.InitializeComponent();
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
            this.Lbl_Hint = new Label();
            this.Txt_Input = new TextBox();
            base.SuspendLayout();
            this.Lbl_Hint.BorderStyle = BorderStyle.Fixed3D;
            this.Lbl_Hint.Dock = DockStyle.Left;
            this.Lbl_Hint.Location = new Point(0, 0);
            this.Lbl_Hint.Name = "Lbl_Hint";
            this.Lbl_Hint.Size = new Size(90, 0x1b);
            this.Lbl_Hint.TabIndex = 1;
            this.Lbl_Hint.Text = "标签";
            this.Lbl_Hint.TextAlign = ContentAlignment.MiddleCenter;
            this.Txt_Input.Dock = DockStyle.Fill;
            this.Txt_Input.Location = new Point(90, 0);
            this.Txt_Input.Multiline = true;
            this.Txt_Input.Name = "Txt_Input";
            this.Txt_Input.Size = new Size(0x11f, 0x1b);
            this.Txt_Input.TabIndex = 2;
            base.AutoScaleDimensions = new SizeF(9f, 20f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.Txt_Input);
            base.Controls.Add(this.Lbl_Hint);
            this.Font = new Font("微软雅黑", 11f);
            base.Margin = new Padding(4, 5, 4, 5);
            base.Name = "TextboxLable";
            base.Size = new Size(0x179, 0x1b);
            base.Load += new EventHandler(this.TextboxLable_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void TextboxLable_Load(object sender, EventArgs e)
        {
            List<Label> pLabelList = new List<Label> {
                this.Lbl_Hint
            };
            CommFunc.SetLabelFormat(pLabelList);
        }

        public void ValueFocus()
        {
            this.Txt_Input.SelectAll();
            this.Txt_Input.Focus();
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

        public bool ValueReadOnly
        {
            get => 
                this.Txt_Input.ReadOnly;
            set
            {
                this.Txt_Input.ReadOnly = value;
            }
        }
    }
}

