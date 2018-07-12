namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ZBJView : UserControl
    {
        private List<CheckBox> CheckBoxList = null;
        private CheckBox Ckb_Refresh;
        private CheckBox Ckb_Start;
        private CheckBox Ckb_Stop;
        private IContainer components = null;
        private List<Control> ControlList = null;
        private bool isZB;
        private Label Lbl_Hint;
        private Panel Pnl_Main;
        private Panel Pnl_Top;
        private string RegConfigPath = "";
        private List<Control> SpecialControlList = null;
        private WebBrowser Web_ZBJ;
        private List<WebBrowser> WebBrowserList = null;

        public ZBJView()
        {
            this.InitializeComponent();
        }

        private void BeautifyInterface()
        {
            if (AppInfo.Account.Configuration.Beautify)
            {
                List<Control> pControlList = new List<Control>();
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list2 = new List<Control> {
                    this.Pnl_Main,
                    this.Pnl_Top
                };
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Ckb_Start,
                    this.Ckb_Stop,
                    this.Ckb_Refresh,
                    this.Lbl_Hint
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
            }
        }

        private void Ckb_Refresh_Click(object sender, EventArgs e)
        {
            this.LoadWebZBJIndex();
        }

        private void Ckb_Start_Click(object sender, EventArgs e)
        {
            this.LoadWebZBJIndex();
        }

        private void Ckb_Stop_Click(object sender, EventArgs e)
        {
            this.Web_ZBJ.Url = null;
            this.isZB = false;
            this.RefreshControl();
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
            this.Web_ZBJ = new WebBrowser();
            this.Pnl_Top = new Panel();
            this.Lbl_Hint = new Label();
            this.Ckb_Stop = new CheckBox();
            this.Ckb_Refresh = new CheckBox();
            this.Ckb_Start = new CheckBox();
            this.Pnl_Main.SuspendLayout();
            this.Pnl_Top.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_Main.Controls.Add(this.Web_ZBJ);
            this.Pnl_Main.Controls.Add(this.Pnl_Top);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(0x42d, 730);
            this.Pnl_Main.TabIndex = 0;
            this.Web_ZBJ.Dock = DockStyle.Fill;
            this.Web_ZBJ.Location = new Point(0, 0x23);
            this.Web_ZBJ.MinimumSize = new Size(20, 20);
            this.Web_ZBJ.Name = "Web_ZBJ";
            this.Web_ZBJ.Size = new Size(0x42d, 0x2b7);
            this.Web_ZBJ.TabIndex = 4;
            this.Pnl_Top.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Top.Controls.Add(this.Lbl_Hint);
            this.Pnl_Top.Controls.Add(this.Ckb_Stop);
            this.Pnl_Top.Controls.Add(this.Ckb_Refresh);
            this.Pnl_Top.Controls.Add(this.Ckb_Start);
            this.Pnl_Top.Dock = DockStyle.Top;
            this.Pnl_Top.Location = new Point(0, 0);
            this.Pnl_Top.Name = "Pnl_Top";
            this.Pnl_Top.Size = new Size(0x42d, 0x23);
            this.Pnl_Top.TabIndex = 0;
            this.Lbl_Hint.AutoSize = true;
            this.Lbl_Hint.Font = new Font("微软雅黑", 9f, FontStyle.Bold);
            this.Lbl_Hint.Location = new Point(0xf2, 8);
            this.Lbl_Hint.Name = "Lbl_Hint";
            this.Lbl_Hint.Size = new Size(0x2c, 0x11);
            this.Lbl_Hint.TabIndex = 140;
            this.Lbl_Hint.Text = "提示：";
            this.Ckb_Stop.Appearance = Appearance.Button;
            this.Ckb_Stop.AutoCheck = false;
            this.Ckb_Stop.FlatAppearance.BorderSize = 0;
            this.Ckb_Stop.FlatStyle = FlatStyle.Flat;
            this.Ckb_Stop.Image = Resources.CancelRound;
            this.Ckb_Stop.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Stop.Location = new Point(0x5b, 4);
            this.Ckb_Stop.Name = "Ckb_Stop";
            this.Ckb_Stop.Size = new Size(80, 0x19);
            this.Ckb_Stop.TabIndex = 0x8b;
            this.Ckb_Stop.Text = "关闭直播";
            this.Ckb_Stop.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Stop.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Stop.UseVisualStyleBackColor = true;
            this.Ckb_Stop.Click += new EventHandler(this.Ckb_Stop_Click);
            this.Ckb_Refresh.Appearance = Appearance.Button;
            this.Ckb_Refresh.AutoCheck = false;
            this.Ckb_Refresh.FlatAppearance.BorderSize = 0;
            this.Ckb_Refresh.FlatStyle = FlatStyle.Flat;
            this.Ckb_Refresh.Image = Resources.Refresh;
            this.Ckb_Refresh.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Refresh.Location = new Point(0xb0, 4);
            this.Ckb_Refresh.Name = "Ckb_Refresh";
            this.Ckb_Refresh.Size = new Size(60, 0x19);
            this.Ckb_Refresh.TabIndex = 0x89;
            this.Ckb_Refresh.Text = "刷新";
            this.Ckb_Refresh.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Refresh.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Refresh.UseVisualStyleBackColor = true;
            this.Ckb_Refresh.Click += new EventHandler(this.Ckb_Refresh_Click);
            this.Ckb_Start.Appearance = Appearance.Button;
            this.Ckb_Start.AutoCheck = false;
            this.Ckb_Start.FlatAppearance.BorderSize = 0;
            this.Ckb_Start.FlatStyle = FlatStyle.Flat;
            this.Ckb_Start.Image = Resources.OkRound;
            this.Ckb_Start.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Start.Location = new Point(6, 4);
            this.Ckb_Start.Name = "Ckb_Start";
            this.Ckb_Start.Size = new Size(80, 0x19);
            this.Ckb_Start.TabIndex = 0x8a;
            this.Ckb_Start.Text = "开启直播";
            this.Ckb_Start.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Start.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Start.UseVisualStyleBackColor = true;
            this.Ckb_Start.Click += new EventHandler(this.Ckb_Start_Click);
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.Pnl_Main);
            this.Font = new Font("微软雅黑", 9f);
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "ZBJView";
            base.Size = new Size(0x42d, 730);
            base.Load += new EventHandler(this.ZBJView_Load);
            this.Pnl_Main.ResumeLayout(false);
            this.Pnl_Top.ResumeLayout(false);
            this.Pnl_Top.PerformLayout();
            base.ResumeLayout(false);
        }

        private void LoadWebZBJIndex()
        {
            string zBJUrl = AppInfo.Account.Configuration.ZBJUrl;
            if (zBJUrl != "")
            {
                if (zBJUrl.Contains("http://") || zBJUrl.Contains("https://"))
                {
                    this.Web_ZBJ.Url = null;
                    this.Web_ZBJ.Navigate(zBJUrl);
                    this.isZB = true;
                }
            }
            else
            {
                this.Web_ZBJ.Url = null;
                this.isZB = false;
            }
            this.RefreshControl();
        }

        private void RefreshControl()
        {
            this.Ckb_Start.Enabled = !this.isZB;
            this.Ckb_Stop.Enabled = this.isZB;
            this.Ckb_Refresh.Enabled = this.isZB;
            this.Lbl_Hint.Visible = this.isZB;
        }

        public void SaveControlInfoByReg()
        {
            CommFunc.SaveFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SaveSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
            CommFunc.WriteRegValue(this.RegConfigPath, "IsZB ", this.isZB.ToString());
        }

        public void SetControlInfoByReg()
        {
            this.RegConfigPath = @"software\TUHAOPLUS\YXZXGJ\DlgConfig\永信在线挂机软件\" + base.Name;
            this.ControlList = new List<Control>();
            this.SpecialControlList = new List<Control>();
            CommFunc.SetFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SetSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
            this.isZB = CommFunc.ReadRegBoolean(this.RegConfigPath, "IsZB ", "True");
        }

        private void ZBJView_Load(object sender, EventArgs e)
        {
            this.BeautifyInterface();
            this.SetControlInfoByReg();
            List<CheckBox> list = new List<CheckBox> {
                this.Ckb_Start,
                this.Ckb_Stop,
                this.Ckb_Refresh
            };
            this.CheckBoxList = list;
            CommFunc.SetCheckBoxFormatFlat(this.CheckBoxList);
            List<WebBrowser> list2 = new List<WebBrowser> {
                this.Web_ZBJ
            };
            this.WebBrowserList = list2;
            CommFunc.SetWebBrowserFormat(this.WebBrowserList);
            if (AppInfo.Account.Configuration.ZBJHint != "")
            {
                this.Lbl_Hint.Text = AppInfo.Account.Configuration.ZBJHint;
                this.Lbl_Hint.ForeColor = AppInfo.redForeColor;
            }
            if (this.isZB)
            {
                this.LoadWebZBJIndex();
            }
            else
            {
                this.RefreshControl();
            }
        }
    }
}

