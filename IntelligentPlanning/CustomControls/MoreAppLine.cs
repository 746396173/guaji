namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class MoreAppLine : UserControl
    {
        private CheckBox Ckb_AppName;
        private IContainer components = null;
        private Label Lbl_AppRemark;
        private Panel Pnl_Main;

        public MoreAppLine()
        {
            this.InitializeComponent();
        }

        private void BeautifyInterface()
        {
            if (AppInfo.Account.Configuration.Beautify)
            {
                List<Control> pControlList = new List<Control> {
                    this.Pnl_Main
                };
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list2 = new List<Control>();
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Lbl_AppRemark,
                    this.Ckb_AppName
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
            }
        }

        private void Btn_AppName_Click(object sender, EventArgs e)
        {
            string iD = AppInfo.Account.ID;
            CommFunc.StartProcess(this.Ckb_AppName.Tag.ToString(), iD);
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
            this.Ckb_AppName = new CheckBox();
            this.Lbl_AppRemark = new Label();
            this.Pnl_Main.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_Main.Controls.Add(this.Ckb_AppName);
            this.Pnl_Main.Controls.Add(this.Lbl_AppRemark);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(600, 0x21);
            this.Pnl_Main.TabIndex = 0;
            this.Ckb_AppName.Appearance = Appearance.Button;
            this.Ckb_AppName.AutoCheck = false;
            this.Ckb_AppName.FlatAppearance.BorderSize = 0;
            this.Ckb_AppName.FlatStyle = FlatStyle.Flat;
            this.Ckb_AppName.Image = Resources.Fold;
            this.Ckb_AppName.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_AppName.Location = new Point(5, 3);
            this.Ckb_AppName.Name = "Ckb_AppName";
            this.Ckb_AppName.Size = new Size(200, 0x19);
            this.Ckb_AppName.TabIndex = 0x157;
            this.Ckb_AppName.Text = "软件名称";
            this.Ckb_AppName.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_AppName.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_AppName.UseVisualStyleBackColor = true;
            this.Ckb_AppName.Click += new EventHandler(this.Btn_AppName_Click);
            this.Lbl_AppRemark.AutoSize = true;
            this.Lbl_AppRemark.Location = new Point(0xd3, 7);
            this.Lbl_AppRemark.Name = "Lbl_AppRemark";
            this.Lbl_AppRemark.Size = new Size(0x29, 0x11);
            this.Lbl_AppRemark.TabIndex = 0xba;
            this.Lbl_AppRemark.Text = "介绍...";
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.BorderStyle = BorderStyle.FixedSingle;
            base.Controls.Add(this.Pnl_Main);
            this.Font = new Font("微软雅黑", 9f);
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "MoreAppLine";
            base.Size = new Size(600, 0x21);
            base.Load += new EventHandler(this.MoreAppLine_Load);
            this.Pnl_Main.ResumeLayout(false);
            this.Pnl_Main.PerformLayout();
            base.ResumeLayout(false);
        }

        public void LoadData(ConfigurationStatus.MoreAppData pInfo)
        {
            this.Ckb_AppName.Text = Path.GetFileNameWithoutExtension(pInfo.File);
            this.Ckb_AppName.Tag = pInfo.File;
            this.Lbl_AppRemark.Text = pInfo.Remark;
        }

        private void MoreAppLine_Load(object sender, EventArgs e)
        {
            List<CheckBox> pCheckBoxList = new List<CheckBox> {
                this.Ckb_AppName
            };
            CommFunc.SetCheckBoxFormatFlat(pCheckBoxList);
            this.BeautifyInterface();
        }
    }
}

