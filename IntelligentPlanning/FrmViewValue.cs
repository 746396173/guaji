namespace IntelligentPlanning
{
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FrmViewValue : ExForm
    {
        private CheckBox Ckb_Close;
        private CheckBox Ckb_CopyPlan;
        private IContainer components = null;
        private Panel Pnl_AppName;
        private Panel Pnl_Main;
        private RichTextBox Txt_Value;

        public FrmViewValue(string pValue, ConfigurationStatus.PlayBase playInfo)
        {
            this.InitializeComponent();
            List<Control> list = new List<Control> {
                this
            };
            base.ControlList = list;
            this.Txt_Value.Text = pValue;
            CommFunc.ConvertInputText(this.Txt_Value, playInfo);
            List<CheckBox> list2 = new List<CheckBox> {
                this.Ckb_CopyPlan,
                this.Ckb_Close
            };
            base.CheckBoxList = list2;
        }

        private void BeautifyInterface()
        {
            if (AppInfo.Account.Configuration.Beautify)
            {
                List<Control> pControlList = new List<Control> {
                    this.Pnl_AppName
                };
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list2 = new List<Control> {
                    this.Txt_Value
                };
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Txt_Value,
                    this.Ckb_CopyPlan,
                    this.Ckb_Close
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
            }
        }

        private void Ckb_Close_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void Ckb_CopyPlan_Click(object sender, EventArgs e)
        {
            CommFunc.CopyText(this.Txt_Value.Text);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FrmViewValue_Load(object sender, EventArgs e)
        {
            this.BeautifyInterface();
        }

        private void InitializeComponent()
        {
            this.Pnl_Main = new Panel();
            this.Txt_Value = new RichTextBox();
            this.Pnl_AppName = new Panel();
            this.Ckb_Close = new CheckBox();
            this.Ckb_CopyPlan = new CheckBox();
            this.Pnl_Main.SuspendLayout();
            this.Pnl_AppName.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_Main.Controls.Add(this.Txt_Value);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(0x201, 0x155);
            this.Pnl_Main.TabIndex = 0x4d;
            this.Txt_Value.Dock = DockStyle.Fill;
            this.Txt_Value.Location = new Point(0, 0);
            this.Txt_Value.Name = "Txt_Value";
            this.Txt_Value.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.Txt_Value.Size = new Size(0x201, 0x155);
            this.Txt_Value.TabIndex = 11;
            this.Txt_Value.Text = "";
            this.Pnl_AppName.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_AppName.Controls.Add(this.Ckb_Close);
            this.Pnl_AppName.Controls.Add(this.Ckb_CopyPlan);
            this.Pnl_AppName.Dock = DockStyle.Bottom;
            this.Pnl_AppName.Location = new Point(0, 0x155);
            this.Pnl_AppName.Name = "Pnl_AppName";
            this.Pnl_AppName.Size = new Size(0x201, 0x23);
            this.Pnl_AppName.TabIndex = 0x4c;
            this.Ckb_Close.Appearance = Appearance.Button;
            this.Ckb_Close.AutoCheck = false;
            this.Ckb_Close.FlatAppearance.BorderSize = 0;
            this.Ckb_Close.FlatStyle = FlatStyle.Flat;
            this.Ckb_Close.Image = Resources.Close;
            this.Ckb_Close.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Close.Location = new Point(0x1c0, 4);
            this.Ckb_Close.Name = "Ckb_Close";
            this.Ckb_Close.Size = new Size(60, 0x19);
            this.Ckb_Close.TabIndex = 0xbb;
            this.Ckb_Close.Text = "关闭";
            this.Ckb_Close.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Close.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_Close.UseVisualStyleBackColor = true;
            this.Ckb_Close.Click += new EventHandler(this.Ckb_Close_Click);
            this.Ckb_CopyPlan.Appearance = Appearance.Button;
            this.Ckb_CopyPlan.AutoCheck = false;
            this.Ckb_CopyPlan.FlatAppearance.BorderSize = 0;
            this.Ckb_CopyPlan.FlatStyle = FlatStyle.Flat;
            this.Ckb_CopyPlan.Image = Resources.Copy;
            this.Ckb_CopyPlan.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_CopyPlan.Location = new Point(3, 4);
            this.Ckb_CopyPlan.Name = "Ckb_CopyPlan";
            this.Ckb_CopyPlan.Size = new Size(60, 0x19);
            this.Ckb_CopyPlan.TabIndex = 0xba;
            this.Ckb_CopyPlan.Text = "复制";
            this.Ckb_CopyPlan.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_CopyPlan.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_CopyPlan.UseVisualStyleBackColor = true;
            this.Ckb_CopyPlan.Click += new EventHandler(this.Ckb_CopyPlan_Click);
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x201, 0x178);
            base.Controls.Add(this.Pnl_Main);
            base.Controls.Add(this.Pnl_AppName);
            base.Name = "FrmViewValue";
            this.Text = "投注内容";
            base.Load += new EventHandler(this.FrmViewValue_Load);
            this.Pnl_Main.ResumeLayout(false);
            this.Pnl_AppName.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}

