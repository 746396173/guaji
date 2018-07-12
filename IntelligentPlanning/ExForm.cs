namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ExForm : Form
    {
        public bool _RunEvent = false;
        public List<CheckBox> CheckBoxList = null;
        public List<ComboBox> ComboBoxList = null;
        private IContainer components = null;
        public List<Control> ControlList = null;
        public List<Control> ItemControlList = null;
        public List<Label> LabelList = null;
        public string RegConfigPath = "";
        public List<Control> SpecialControlList = null;
        public List<CheckBox> StandardList = null;
        public List<Control> TextControlList = null;
        public List<WebBrowser> WebBrowserList = null;

        public ExForm()
        {
            this.InitializeComponent();
            base.Icon = AppInfo.AppIcon16;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ExForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.ControlList != null)
            {
                CommFunc.SaveFormUseingInfo(this.ControlList, this.RegConfigPath);
            }
            if (this.SpecialControlList != null)
            {
                CommFunc.SaveSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
            }
        }

        private void ExForm_Load(object sender, EventArgs e)
        {
            if (this.RegConfigPath == "")
            {
                this.RegConfigPath = @"software\TUHAOPLUS\YXZXGJ\DlgConfig\" + base.Name;
            }
            if (this.SpecialControlList != null)
            {
                CommFunc.SetSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
            }
            if (this.CheckBoxList != null)
            {
                CommFunc.SetCheckBoxFormatFlat(this.CheckBoxList);
            }
            if (this.StandardList != null)
            {
                CommFunc.SetCheckBoxFormatStandard(this.StandardList);
            }
            if (this.ComboBoxList != null)
            {
                CommFunc.SetComboBoxFormat(this.ComboBoxList, 8);
            }
            if (this.LabelList != null)
            {
                CommFunc.SetLabelFormat(this.LabelList);
            }
            if (this.WebBrowserList != null)
            {
                CommFunc.SetWebBrowserFormat(this.WebBrowserList);
            }
            if (this.ControlList != null)
            {
                CommFunc.SetFormUseingInfo(this.ControlList, this.RegConfigPath);
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ExForm));
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(96f, 96f);
            base.AutoScaleMode = AutoScaleMode.Dpi;
            base.ClientSize = new Size(0x160, 0x11b);
            this.Font = new Font("微软雅黑", 9f);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ExForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            base.FormClosing += new FormClosingEventHandler(this.ExForm_FormClosing);
            base.Load += new EventHandler(this.ExForm_Load);
            base.ResumeLayout(false);
        }
    }
}

