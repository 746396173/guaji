namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class PTOpenDataLine : UserControl
    {
        private ComboBox Cbb_LoginLine;
        private CheckBox Ckb_OpenCJ;
        private IContainer components = null;
        private List<Control> ControlList = null;
        private Label Lbl_LoginHint;
        private Label Lbl_LoginLine;
        private Label Lbl_PTPassWord;
        private Label Lbl_PTUserID;
        private Panel Pnl_Bottom;
        private Panel Pnl_Main;
        private Panel Pnl_Top;
        public ConfigurationStatus.PTOpenData PTDataInfo;
        public PTBase PTInfo = null;
        private string RegConfigPath = "";
        private List<Control> SpecialControlList = null;
        private TextBox Txt_PTUserID;
        private TextBox Txt_PTUserPW;
        private WebBrowser Web_Login;

        public PTOpenDataLine()
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

        private void InitializeComponent()
        {
            this.Pnl_Main = new Panel();
            this.Web_Login = new WebBrowser();
            this.Pnl_Bottom = new Panel();
            this.Pnl_Top = new Panel();
            this.Ckb_OpenCJ = new CheckBox();
            this.Txt_PTUserPW = new TextBox();
            this.Lbl_PTPassWord = new Label();
            this.Txt_PTUserID = new TextBox();
            this.Lbl_PTUserID = new Label();
            this.Cbb_LoginLine = new ComboBox();
            this.Lbl_LoginLine = new Label();
            this.Lbl_LoginHint = new Label();
            this.Pnl_Main.SuspendLayout();
            this.Pnl_Top.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_Main.Controls.Add(this.Web_Login);
            this.Pnl_Main.Controls.Add(this.Pnl_Bottom);
            this.Pnl_Main.Controls.Add(this.Pnl_Top);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(0x3e8, 0x26d);
            this.Pnl_Main.TabIndex = 0;
            this.Web_Login.Dock = DockStyle.Fill;
            this.Web_Login.Location = new Point(0, 0x23);
            this.Web_Login.MinimumSize = new Size(20, 20);
            this.Web_Login.Name = "Web_Login";
            this.Web_Login.Size = new Size(0x3e8, 0x1e6);
            this.Web_Login.TabIndex = 9;
            this.Pnl_Bottom.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Bottom.Dock = DockStyle.Bottom;
            this.Pnl_Bottom.Location = new Point(0, 0x209);
            this.Pnl_Bottom.Name = "Pnl_Bottom";
            this.Pnl_Bottom.Size = new Size(0x3e8, 100);
            this.Pnl_Bottom.TabIndex = 8;
            this.Pnl_Top.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Top.Controls.Add(this.Lbl_LoginHint);
            this.Pnl_Top.Controls.Add(this.Ckb_OpenCJ);
            this.Pnl_Top.Controls.Add(this.Txt_PTUserPW);
            this.Pnl_Top.Controls.Add(this.Lbl_PTPassWord);
            this.Pnl_Top.Controls.Add(this.Txt_PTUserID);
            this.Pnl_Top.Controls.Add(this.Lbl_PTUserID);
            this.Pnl_Top.Controls.Add(this.Cbb_LoginLine);
            this.Pnl_Top.Controls.Add(this.Lbl_LoginLine);
            this.Pnl_Top.Dock = DockStyle.Top;
            this.Pnl_Top.Location = new Point(0, 0);
            this.Pnl_Top.Name = "Pnl_Top";
            this.Pnl_Top.Size = new Size(0x3e8, 0x23);
            this.Pnl_Top.TabIndex = 4;
            this.Ckb_OpenCJ.AutoSize = true;
            this.Ckb_OpenCJ.Location = new Point(6, 6);
            this.Ckb_OpenCJ.Name = "Ckb_OpenCJ";
            this.Ckb_OpenCJ.Size = new Size(0x4b, 0x15);
            this.Ckb_OpenCJ.TabIndex = 0x9f;
            this.Ckb_OpenCJ.Text = "开启采集";
            this.Ckb_OpenCJ.UseVisualStyleBackColor = true;
            this.Txt_PTUserPW.Location = new Point(0x289, 5);
            this.Txt_PTUserPW.Name = "Txt_PTUserPW";
            this.Txt_PTUserPW.Size = new Size(130, 0x17);
            this.Txt_PTUserPW.TabIndex = 0x9e;
            this.Lbl_PTPassWord.AutoSize = true;
            this.Lbl_PTPassWord.Location = new Point(0x257, 8);
            this.Lbl_PTPassWord.Name = "Lbl_PTPassWord";
            this.Lbl_PTPassWord.Size = new Size(0x2c, 0x11);
            this.Lbl_PTPassWord.TabIndex = 0x9d;
            this.Lbl_PTPassWord.Text = "密码：";
            this.Txt_PTUserID.Location = new Point(0x1cf, 5);
            this.Txt_PTUserID.Name = "Txt_PTUserID";
            this.Txt_PTUserID.Size = new Size(130, 0x17);
            this.Txt_PTUserID.TabIndex = 0x9c;
            this.Lbl_PTUserID.AutoSize = true;
            this.Lbl_PTUserID.Location = new Point(0x19d, 8);
            this.Lbl_PTUserID.Name = "Lbl_PTUserID";
            this.Lbl_PTUserID.Size = new Size(0x2c, 0x11);
            this.Lbl_PTUserID.TabIndex = 0x9b;
            this.Lbl_PTUserID.Text = "账号：";
            this.Cbb_LoginLine.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_LoginLine.FormattingEnabled = true;
            this.Cbb_LoginLine.Location = new Point(0x9d, 5);
            this.Cbb_LoginLine.Name = "Cbb_LoginLine";
            this.Cbb_LoginLine.Size = new Size(250, 0x19);
            this.Cbb_LoginLine.TabIndex = 0x98;
            this.Lbl_LoginLine.AutoSize = true;
            this.Lbl_LoginLine.Location = new Point(0x53, 8);
            this.Lbl_LoginLine.Name = "Lbl_LoginLine";
            this.Lbl_LoginLine.Size = new Size(0x44, 0x11);
            this.Lbl_LoginLine.TabIndex = 0;
            this.Lbl_LoginLine.Text = "登录线路：";
            this.Lbl_LoginHint.AutoSize = true;
            this.Lbl_LoginHint.Location = new Point(0x311, 7);
            this.Lbl_LoginHint.Name = "Lbl_LoginHint";
            this.Lbl_LoginHint.Size = new Size(0, 0x11);
            this.Lbl_LoginHint.TabIndex = 160;
            base.AutoScaleDimensions = new SizeF(96f, 96f);
            base.AutoScaleMode = AutoScaleMode.Dpi;
            base.BorderStyle = BorderStyle.FixedSingle;
            base.Controls.Add(this.Pnl_Main);
            this.Font = new Font("微软雅黑", 9f);
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "PTOpenDataLine";
            base.Size = new Size(0x3e8, 0x26d);
            this.Pnl_Main.ResumeLayout(false);
            this.Pnl_Top.ResumeLayout(false);
            this.Pnl_Top.PerformLayout();
            base.ResumeLayout(false);
        }

        public void LoadPTOpenDataLine(ConfigurationStatus.PTOpenData pInfo, string pRegConfigPath)
        {
            this.PTDataInfo = pInfo;
            this.RegConfigPath = pRegConfigPath;
            if ((this.PTDataInfo != null) && (this.PTDataInfo.LoginLineList.Count != 0))
            {
                CommFunc.SetComboBoxList(this.Cbb_LoginLine, this.PTDataInfo.LoginLineList);
                for (int i = this.PTDataInfo.LotteryIDList.Count - 1; i >= 0; i--)
                {
                    string pID = this.PTDataInfo.LotteryIDList[i];
                    AppInfo.Current.LotteryDic[pID] = AppInfo.Current.LoadLotteryConfig(pID);
                    ConfigurationStatus.LotteryConfig config = AppInfo.Current.LotteryDic[pID];
                    OpenDataLine line = new OpenDataLine {
                        LotteryID = pID,
                        GetLotterySelect = { Tag = pID },
                        Hint = config.Name,
                        Type = config.Type,
                        Expect = config.RefreshExpect,
                        Dock = DockStyle.Left
                    };
                    this.Pnl_Bottom.Controls.Add(line);
                }
                this.SetControlInfoByReg();
            }
        }

        public void RefreshLogin(bool pState, string pHint = "")
        {
            this.PTInfo.IsLoginRun = pState;
            this.Lbl_LoginHint.Text = pState ? "正在登录中..." : pHint;
            if (pState)
            {
                this.Lbl_LoginLine.Enabled = this.Cbb_LoginLine.Enabled = false;
            }
            else if (!this.PTInfo.PTLoginStatus)
            {
                this.Lbl_LoginLine.Enabled = this.Cbb_LoginLine.Enabled = true;
            }
        }

        public void RefreshPTLineList()
        {
            CommFunc.SetComboBoxSelectedIndex(this.Cbb_LoginLine, this.PTInfo.LoginUrl);
        }

        public void SaveControlInfoByReg()
        {
            CommFunc.SaveFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SaveSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
        }

        public void SetControlInfoByReg()
        {
            this.RegConfigPath = this.RegConfigPath + @"\" + this.PTDataInfo.Name;
            List<Control> list = new List<Control> {
                this.Txt_PTUserID,
                this.Txt_PTUserPW
            };
            this.ControlList = list;
            List<Control> list2 = new List<Control> {
                this.Cbb_LoginLine
            };
            this.SpecialControlList = list2;
            CommFunc.SetFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SetSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
            foreach (OpenDataLine line in this.GetOpenDataLine)
            {
                line.SetControlInfoByReg(this.RegConfigPath);
            }
            if (this.Cbb_LoginLine.SelectedIndex == -1)
            {
                this.Cbb_LoginLine.SelectedIndex = 0;
            }
        }

        public void WebLoginOut()
        {
            this.PTInfo.QuitPT();
        }

        public bool CJState
        {
            get => 
                this.Ckb_OpenCJ.Checked;
            set
            {
                this.Ckb_OpenCJ.Checked = value;
            }
        }

        public CheckBox GetOpenCJ =>
            this.Ckb_OpenCJ;

        public List<OpenDataLine> GetOpenDataLine
        {
            get
            {
                List<OpenDataLine> list = new List<OpenDataLine>();
                foreach (Control control in this.Pnl_Bottom.Controls)
                {
                    OpenDataLine item = (OpenDataLine) control;
                    list.Add(item);
                }
                return list;
            }
        }

        public string PTLoginLine =>
            this.Cbb_LoginLine.Text;

        public int PTLoginLineIndex =>
            this.Cbb_LoginLine.SelectedIndex;

        public string PTUserID =>
            this.Txt_PTUserID.Text;

        public string PTUserPW =>
            this.Txt_PTUserPW.Text;
    }
}

