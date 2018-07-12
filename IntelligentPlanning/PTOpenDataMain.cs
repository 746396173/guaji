namespace IntelligentPlanning
{
    using IntelligentPlanning.CustomControls;
    using IntelligentPlanning.WorkThread;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class PTOpenDataMain : ExForm
    {
        private Button Btn_Select;
        private Button Btn_UnSelect;
        private Button Ckb_Close;
        private Button Ckb_Ok;
        private IContainer components = null;
        private Label Lbl_InitialCatalog;
        private Label Lbl_PassWord;
        private Label Lbl_Source;
        private Label Lbl_UserID;
        private Dictionary<string, MainThread> MainThreadDic = new Dictionary<string, MainThread>();
        private List<OpenDataLine> OpenDataLineList = new List<OpenDataLine>();
        private Panel Pnl_Bottom;
        private Panel Pnl_Main;
        private Panel Pnl_SQLData;
        private Dictionary<string, PTOpenDataLine> PTControlDic = new Dictionary<string, PTOpenDataLine>();
        private Dictionary<string, ConfigurationStatus.PTLine> PTLineDic = new Dictionary<string, ConfigurationStatus.PTLine>();
        private TabControl Tab_Main;
        private TextBox Txt_InitialCatalog;
        private TextBox Txt_PassWord;
        private TextBox Txt_Source;
        private TextBox Txt_UserID;

        public PTOpenDataMain()
        {
            this.InitializeComponent();
            List<Control> list = new List<Control> {
                this,
                this.Txt_Source,
                this.Txt_InitialCatalog,
                this.Txt_UserID,
                this.Txt_PassWord
            };
            base.ControlList = list;
            this.LoadMainApp();
        }

        private bool AnalysisVerifyCode(string pName, ref int pCount)
        {
            bool flag = false;
            try
            {
                PTOpenDataLine line = this.PTControlDic[pName];
                PTBase pTInfo = line.PTInfo;
                List<string> pTextList = new List<string>();
                string pHint = CommFunc.ClickEnter(this.Text, "来自网页的消息", pTextList, true);
                if (!pTInfo.IsLoginRun && !pTInfo.AnalysisVerifyCode)
                {
                    return flag;
                }
                if ((pHint == "") && (pTInfo.LoginMain && pTInfo.AnalysisVerifyCode))
                {
                    if (pTInfo.IsHTLoginMain)
                    {
                        if (pTInfo.WebLoginMain(line.PTUserID, line.PTUserPW, ref pHint))
                        {
                            base.Invoke(AppInfo.PTIndexMain, new object[] { pName });
                            pTInfo.LoginMain = false;
                        }
                        else if (pHint == "")
                        {
                            pHint = "登录超时";
                        }
                    }
                    else
                    {
                        base.Invoke(AppInfo.LoginVerify);
                        pTInfo.LoginMain = false;
                    }
                }
                if (pHint == "")
                {
                    pCount++;
                    if (pCount >= 30)
                    {
                        pHint = "登录超时";
                        pCount = 0;
                    }
                }
                if (pTInfo.PTLoginStatus)
                {
                    pTInfo.AnalysisVerifyCode = false;
                    string cookieInternal = HttpHelper.GetCookieInternal(pTInfo.GetUrlLine());
                    if (cookieInternal != "")
                    {
                        pTInfo.WebCookie = cookieInternal;
                        HttpHelper.SaveCookies(cookieInternal, "");
                    }
                    flag = true;
                }
                switch (pHint)
                {
                    case "":
                        return flag;

                    case "登录超时":
                        base.Invoke(AppInfo.RemoveLoginLock, new object[] { pHint, pName });
                        pTInfo.AnalysisVerifyCode = false;
                        pTInfo.LoginMain = false;
                        pTInfo.PTLoginStatus = false;
                        CommFunc.PlaySound("登录失败");
                        base.Invoke(AppInfo.LoginMain, new object[] { true, pName });
                        pCount = 0;
                        return flag;
                }
                if (pHint.Contains("用户名"))
                {
                    pHint = "用户名或者密码错误";
                }
                pTInfo.DefaultOption();
                foreach (string str3 in line.PTDataInfo.LotteryIDList)
                {
                    if (this.MainThreadDic.ContainsKey(str3))
                    {
                        this.MainThreadDic[str3].downCode.DefaultOption();
                    }
                }
                base.Invoke(AppInfo.RemoveLoginLock, new object[] { pHint, pName });
                pTInfo.AnalysisVerifyCode = false;
                pTInfo.LoginMain = false;
                CommFunc.PlaySound("登录失败");
            }
            catch (Exception exception)
            {
                DebugLog.SaveDebug(exception, "");
            }
            return flag;
        }

        private void Btn_Select_Click(object sender, EventArgs e)
        {
            foreach (OpenDataLine line in this.OpenDataLineList)
            {
                line.SelectState = true;
            }
        }

        private void Btn_UnSelect_Click(object sender, EventArgs e)
        {
            foreach (OpenDataLine line in this.OpenDataLineList)
            {
                line.SelectState = false;
            }
        }

        private void Ckb_Close_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void Ckb_Ok_Click(object sender, EventArgs e)
        {
            if (SQLServer.SQLConnection(SQLServer.GetConnectionString(this.Txt_Source.Text, this.Txt_InitialCatalog.Text, this.Txt_UserID.Text, this.Txt_PassWord.Text)))
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                foreach (OpenDataLine line in this.OpenDataLineList)
                {
                    if (line.SelectState)
                    {
                        this.MainThreadDic[line.LotteryID] = new MainThread(line.LotteryID, line.PTInfo);
                        dictionary[line.PTInfo.PTName] = "";
                    }
                }
                this.Ckb_Ok.Enabled = false;
                base._RunEvent = true;
                foreach (string str2 in dictionary.Keys)
                {
                    PTOpenDataLine line2 = this.PTControlDic[str2];
                    line2.GetOpenCJ.Checked = true;
                }
            }
        }

        private void ColsingCheck()
        {
            foreach (string str in this.MainThreadDic.Keys)
            {
                this.MainThreadDic[str].CloseThreadList();
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

        private void FillPTLineList(string pName)
        {
            ConfigurationStatus.PTLine line = this.PTLineDic[pName];
            PTOpenDataLine line2 = this.PTControlDic[pName];
            PTBase pTInfo = line2.PTInfo;
            pTInfo.PTID = line.ID;
            pTInfo.PTName = line.Name;
            pTInfo.LineList.Clear();
            List<string> lineList = line.LineList;
            pTInfo.LineList = PTBase.GetSkipUrlList(lineList, pTInfo);
        }

        public ConfigurationStatus.OpenData GetOpenDataFirstExpect(List<string> pList)
        {
            if (pList.Count == 0)
            {
                return null;
            }
            string[] strArray = pList[0].Split(new char[] { '\t' });
            return new ConfigurationStatus.OpenData { 
                Expect = strArray[0],
                Code = strArray[1]
            };
        }

        private void InitializeComponent()
        {
            this.Ckb_Close = new Button();
            this.Pnl_Main = new Panel();
            this.Tab_Main = new TabControl();
            this.Pnl_SQLData = new Panel();
            this.Txt_PassWord = new TextBox();
            this.Lbl_PassWord = new Label();
            this.Txt_UserID = new TextBox();
            this.Lbl_UserID = new Label();
            this.Txt_InitialCatalog = new TextBox();
            this.Lbl_InitialCatalog = new Label();
            this.Txt_Source = new TextBox();
            this.Lbl_Source = new Label();
            this.Pnl_Bottom = new Panel();
            this.Btn_UnSelect = new Button();
            this.Btn_Select = new Button();
            this.Ckb_Ok = new Button();
            this.Pnl_Main.SuspendLayout();
            this.Pnl_SQLData.SuspendLayout();
            this.Pnl_Bottom.SuspendLayout();
            base.SuspendLayout();
            this.Ckb_Close.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Ckb_Close.DialogResult = DialogResult.Cancel;
            this.Ckb_Close.FlatAppearance.BorderSize = 0;
            this.Ckb_Close.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Close.Location = new Point(0x416, 3);
            this.Ckb_Close.Name = "Ckb_Close";
            this.Ckb_Close.Size = new Size(60, 0x19);
            this.Ckb_Close.TabIndex = 20;
            this.Ckb_Close.Text = "关闭";
            this.Ckb_Close.UseVisualStyleBackColor = true;
            this.Ckb_Close.Click += new EventHandler(this.Ckb_Close_Click);
            this.Pnl_Main.Controls.Add(this.Tab_Main);
            this.Pnl_Main.Controls.Add(this.Pnl_SQLData);
            this.Pnl_Main.Controls.Add(this.Pnl_Bottom);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(0x458, 0x286);
            this.Pnl_Main.TabIndex = 0;
            this.Tab_Main.Dock = DockStyle.Fill;
            this.Tab_Main.ItemSize = new Size(60, 30);
            this.Tab_Main.Location = new Point(0, 0x23);
            this.Tab_Main.Name = "Tab_Main";
            this.Tab_Main.SelectedIndex = 0;
            this.Tab_Main.Size = new Size(0x458, 0x240);
            this.Tab_Main.SizeMode = TabSizeMode.Fixed;
            this.Tab_Main.TabIndex = 13;
            this.Pnl_SQLData.Controls.Add(this.Txt_PassWord);
            this.Pnl_SQLData.Controls.Add(this.Lbl_PassWord);
            this.Pnl_SQLData.Controls.Add(this.Txt_UserID);
            this.Pnl_SQLData.Controls.Add(this.Lbl_UserID);
            this.Pnl_SQLData.Controls.Add(this.Txt_InitialCatalog);
            this.Pnl_SQLData.Controls.Add(this.Lbl_InitialCatalog);
            this.Pnl_SQLData.Controls.Add(this.Txt_Source);
            this.Pnl_SQLData.Controls.Add(this.Lbl_Source);
            this.Pnl_SQLData.Dock = DockStyle.Top;
            this.Pnl_SQLData.Location = new Point(0, 0);
            this.Pnl_SQLData.Name = "Pnl_SQLData";
            this.Pnl_SQLData.Size = new Size(0x458, 0x23);
            this.Pnl_SQLData.TabIndex = 0x1f;
            this.Txt_PassWord.Location = new Point(0x27b, 6);
            this.Txt_PassWord.Name = "Txt_PassWord";
            this.Txt_PassWord.Size = new Size(130, 0x17);
            this.Txt_PassWord.TabIndex = 7;
            this.Lbl_PassWord.AutoSize = true;
            this.Lbl_PassWord.Location = new Point(0x249, 9);
            this.Lbl_PassWord.Name = "Lbl_PassWord";
            this.Lbl_PassWord.Size = new Size(0x2c, 0x11);
            this.Lbl_PassWord.TabIndex = 6;
            this.Lbl_PassWord.Text = "密码：";
            this.Txt_UserID.Location = new Point(0x1c1, 6);
            this.Txt_UserID.Name = "Txt_UserID";
            this.Txt_UserID.Size = new Size(130, 0x17);
            this.Txt_UserID.TabIndex = 5;
            this.Lbl_UserID.AutoSize = true;
            this.Lbl_UserID.Location = new Point(0x18f, 9);
            this.Lbl_UserID.Name = "Lbl_UserID";
            this.Lbl_UserID.Size = new Size(0x2c, 0x11);
            this.Lbl_UserID.TabIndex = 4;
            this.Lbl_UserID.Text = "账号：";
            this.Txt_InitialCatalog.Location = new Point(0x107, 6);
            this.Txt_InitialCatalog.Name = "Txt_InitialCatalog";
            this.Txt_InitialCatalog.Size = new Size(130, 0x17);
            this.Txt_InitialCatalog.TabIndex = 3;
            this.Lbl_InitialCatalog.AutoSize = true;
            this.Lbl_InitialCatalog.Location = new Point(0xca, 9);
            this.Lbl_InitialCatalog.Name = "Lbl_InitialCatalog";
            this.Lbl_InitialCatalog.Size = new Size(0x38, 0x11);
            this.Lbl_InitialCatalog.TabIndex = 2;
            this.Lbl_InitialCatalog.Text = "数据库：";
            this.Txt_Source.Location = new Point(0x42, 6);
            this.Txt_Source.Name = "Txt_Source";
            this.Txt_Source.Size = new Size(130, 0x17);
            this.Txt_Source.TabIndex = 1;
            this.Lbl_Source.AutoSize = true;
            this.Lbl_Source.Location = new Point(5, 9);
            this.Lbl_Source.Name = "Lbl_Source";
            this.Lbl_Source.Size = new Size(0x38, 0x11);
            this.Lbl_Source.TabIndex = 0;
            this.Lbl_Source.Text = "服务器：";
            this.Pnl_Bottom.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Bottom.Controls.Add(this.Btn_UnSelect);
            this.Pnl_Bottom.Controls.Add(this.Btn_Select);
            this.Pnl_Bottom.Controls.Add(this.Ckb_Close);
            this.Pnl_Bottom.Controls.Add(this.Ckb_Ok);
            this.Pnl_Bottom.Dock = DockStyle.Bottom;
            this.Pnl_Bottom.Location = new Point(0, 0x263);
            this.Pnl_Bottom.Name = "Pnl_Bottom";
            this.Pnl_Bottom.Size = new Size(0x458, 0x23);
            this.Pnl_Bottom.TabIndex = 12;
            this.Btn_UnSelect.FlatAppearance.BorderSize = 0;
            this.Btn_UnSelect.ImageAlign = ContentAlignment.MiddleLeft;
            this.Btn_UnSelect.Location = new Point(0x49, 3);
            this.Btn_UnSelect.Name = "Btn_UnSelect";
            this.Btn_UnSelect.Size = new Size(60, 0x19);
            this.Btn_UnSelect.TabIndex = 0x18;
            this.Btn_UnSelect.Text = "不选";
            this.Btn_UnSelect.UseVisualStyleBackColor = true;
            this.Btn_UnSelect.Click += new EventHandler(this.Btn_UnSelect_Click);
            this.Btn_Select.FlatAppearance.BorderSize = 0;
            this.Btn_Select.ImageAlign = ContentAlignment.MiddleLeft;
            this.Btn_Select.Location = new Point(7, 3);
            this.Btn_Select.Name = "Btn_Select";
            this.Btn_Select.Size = new Size(60, 0x19);
            this.Btn_Select.TabIndex = 0x17;
            this.Btn_Select.Text = "全选";
            this.Btn_Select.UseVisualStyleBackColor = true;
            this.Btn_Select.Click += new EventHandler(this.Btn_Select_Click);
            this.Ckb_Ok.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Ckb_Ok.FlatAppearance.BorderSize = 0;
            this.Ckb_Ok.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Ok.Location = new Point(980, 3);
            this.Ckb_Ok.Name = "Ckb_Ok";
            this.Ckb_Ok.Size = new Size(60, 0x19);
            this.Ckb_Ok.TabIndex = 0x13;
            this.Ckb_Ok.Text = "开始";
            this.Ckb_Ok.UseVisualStyleBackColor = true;
            this.Ckb_Ok.Click += new EventHandler(this.Ckb_Ok_Click);
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.Ckb_Close;
            base.ClientSize = new Size(0x458, 0x286);
            base.Controls.Add(this.Pnl_Main);
            base.FormBorderStyle = FormBorderStyle.Sizable;
            base.MaximizeBox = true;
            base.MinimizeBox = true;
            base.Name = "PTOpenDataMain";
            this.Text = "开奖号码采集器";
            base.FormClosing += new FormClosingEventHandler(this.PTOpenDataMain_FormClosing);
            base.Load += new EventHandler(this.PTOpenDataMain_Load);
            this.Pnl_Main.ResumeLayout(false);
            this.Pnl_SQLData.ResumeLayout(false);
            this.Pnl_SQLData.PerformLayout();
            this.Pnl_Bottom.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void LoadMainApp()
        {
            AppInfo.RefreshList = new ConfigurationStatus.RefreshListDelegate(this.RefreshData);
            AppInfo.AnalysisVerifyCode = new ConfigurationStatus.AnalysisVerifyCodeDelegate(this.AnalysisVerifyCode);
            AppInfo.LoginMain = new ConfigurationStatus.LoginMainDelegate(this.SwitchNextLine);
            AppInfo.PTIndexMain = new ConfigurationStatus.PTIndexMainDelegate(this.PTIndexMain);
            AppInfo.RemoveLoginLock = new ConfigurationStatus.RemoveLoginLockDelegate(this.RemoveLoginLock);
        }

        private void LoginMain(string pName)
        {
            PTOpenDataLine line = this.PTControlDic[pName];
            PTBase pTInfo = line.PTInfo;
            if (!pTInfo.IsLoginRun)
            {
                if (!line.PTInfo.PTLoginStatus)
                {
                    if (line.PTUserID == "")
                    {
                        CommFunc.PublicMessageAll("输入账号不能为空！", true, MessageBoxIcon.Asterisk, "");
                    }
                    else if (line.PTUserPW == "")
                    {
                        CommFunc.PublicMessageAll("输入密码不能为空！", true, MessageBoxIcon.Asterisk, "");
                    }
                    else
                    {
                        line.RefreshLogin(true, "");
                        pTInfo.UserID = line.PTUserID;
                        if (pTInfo.LineIndex == -1)
                        {
                            pTInfo.LineIndex = line.PTLoginLineIndex;
                        }
                        pTInfo.LoginUrl = pTInfo.GetLine();
                        if (pTInfo.IsHTLoginMain)
                        {
                            pTInfo.AnalysisVerifyCode = true;
                            pTInfo.LoginMain = true;
                        }
                        else
                        {
                            string cookieInternal = HttpHelper.GetCookieInternal(pTInfo.GetUrlLine());
                            pTInfo.WebCookie = cookieInternal;
                            string loginLine = pTInfo.GetLoginLine();
                        }
                    }
                }
                else
                {
                    pTInfo.PTLoginStatus = false;
                    line.WebLoginOut();
                    pTInfo.DefaultOption();
                    foreach (string str3 in line.PTDataInfo.LotteryIDList)
                    {
                        if (this.MainThreadDic.ContainsKey(str3))
                        {
                            this.MainThreadDic[str3].downCode.DefaultOption();
                        }
                    }
                    line.RefreshLogin(false, "");
                }
            }
        }

        public void LotterySelect_CheckedChanged(object sender, EventArgs e)
        {
            if (base._RunEvent)
            {
                CheckBox box = sender as CheckBox;
                string pID = box.Tag.ToString();
                foreach (OpenDataLine line in this.OpenDataLineList)
                {
                    if (pID == line.LotteryID)
                    {
                        if (box.Checked)
                        {
                            this.MainThreadDic[pID] = new MainThread(pID, line.PTInfo);
                        }
                        else
                        {
                            this.MainThreadDic[pID].CloseThreadList();
                            this.MainThreadDic.Remove(pID);
                            line.ClearForm();
                        }
                    }
                }
            }
        }

        public void OpenCJ_CheckedChanged(object sender, EventArgs e)
        {
            if (base._RunEvent)
            {
                CheckBox box = sender as CheckBox;
                string pName = box.Tag.ToString();
                this.LoginMain(pName);
            }
        }

        private void PTIndexMain(string pName = "")
        {
            PTOpenDataLine line = this.PTControlDic[pName];
            PTBase pTInfo = line.PTInfo;
            if (!pTInfo.PTLoginStatus)
            {
                pTInfo.PTLoginStatus = true;
                line.RefreshLogin(false, "");
                line.RefreshPTLineList();
                CommFunc.PlaySound("登录成功");
                pTInfo.AnalysisVerifyCode = true;
            }
        }

        private void PTOpenDataMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (string str in this.PTControlDic.Keys)
            {
                PTOpenDataLine line = this.PTControlDic[str];
                line.SaveControlInfoByReg();
                foreach (OpenDataLine line2 in line.GetOpenDataLine)
                {
                    line2.SaveControlInfoByReg();
                }
            }
            this.ColsingCheck();
            this.Dispose(true);
            Application.ExitThread();
            Application.Exit();
            CommFunc.ClearObejct();
        }

        private void PTOpenDataMain_Load(object sender, EventArgs e)
        {
            AppInfo.Current = new ConfigurationStatus(new List<string>());
            string pTLineFile = AppInfo.PTLineFile;
            string pLineString = "";
            if (File.Exists(pTLineFile))
            {
                pLineString = CommFunc.ReadTextFileToStr(pTLineFile);
                List<string> list = CommFunc.AnalysisPTLine(ref this.PTLineDic, pLineString);
                foreach (string str3 in this.PTLineDic.Keys)
                {
                    try
                    {
                        ConfigurationStatus.PTOpenData pInfo = new ConfigurationStatus.PTOpenData {
                            Name = str3,
                            LoginLineList = CommFunc.CopyList(this.PTLineDic[str3].LineList)
                        };
                        List<string> list2 = new List<string>();
                        PTBase mRYLInfo = null;
                        if (str3 == "361彩票")
                        {
                            mRYLInfo = AppInfo.CP361Info;
                            list2 = new List<string> { 
                                "BDFFC",
                                "BD2FC"
                            };
                        }
                        else if (str3 == "名人娱乐")
                        {
                            mRYLInfo = AppInfo.MRYLInfo;
                            list2 = new List<string> { 
                                "MRFFC",
                                "MR2FC",
                                "MR45C",
                                "MR11X5",
                                "MRPK10"
                            };
                        }
                        else if (str3 == "华人彩票")
                        {
                            mRYLInfo = AppInfo.HRCPInfo;
                            list2 = new List<string> { 
                                "HRFFC",
                                "HR2FC",
                                "HR45C"
                            };
                        }
                        else if (str3 == "大唐彩票")
                        {
                            mRYLInfo = AppInfo.DTCPInfo;
                            list2 = new List<string> { 
                                "DTFFC",
                                "DT2FC"
                            };
                        }
                        else if (str3 == "亿宝娱乐")
                        {
                            mRYLInfo = AppInfo.YBAOInfo;
                            list2 = new List<string> { 
                                "YBAO45C",
                                "XJSSC",
                                "YBAOFFC",
                                "YBAO2FC"
                            };
                        }
                        else if (str3 == "千金城")
                        {
                            mRYLInfo = AppInfo.QJCInfo;
                            list2 = new List<string> { 
                                "FLBSSC",
                                "QJCTXFFC",
                                "QJCFFC",
                                "QJC2FC",
                                "QJC45C",
                                "QJCHL15C",
                                "QJCAM15C",
                                "QJCXNFFC",
                                "QJCPDFFC",
                                "QJC11X5",
                                "QJCPK10"
                            };
                        }
                        else if (str3 == "WE娱乐")
                        {
                            mRYLInfo = AppInfo.WEYLInfo;
                            list2 = new List<string> { 
                                "WEFFC",
                                "WE2FC",
                                "WE45C",
                                "WETXFFC",
                                "WEAM15C",
                                "WE11X5",
                                "WEPK10"
                            };
                        }
                        else if (str3 == "唐人娱乐")
                        {
                            mRYLInfo = AppInfo.TRYLInfo;
                            list2 = new List<string> { 
                                "TRFFC",
                                "TR2FC",
                                "TR45C",
                                "TR11X5",
                                "TRPK10"
                            };
                        }
                        else if (str3 == "博猫平台")
                        {
                            mRYLInfo = AppInfo.BMYXInfo;
                            list2 = new List<string> { 
                                "BM1FC",
                                "BM2FC",
                                "BM5FC"
                            };
                        }
                        else if (str3 == "梦之城")
                        {
                            mRYLInfo = AppInfo.MZCInfo;
                            list2 = new List<string> { 
                                "MZCFFC",
                                "MZC3FC"
                            };
                        }
                        else if (str3 == "大唐彩票")
                        {
                            mRYLInfo = AppInfo.DTCPInfo;
                            list2 = new List<string> { 
                                "DTFFC",
                                "DT2FC"
                            };
                        }
                        else if (str3 == "M5彩票")
                        {
                            mRYLInfo = AppInfo.M5CPInfo;
                            list2 = new List<string> { 
                                "M5FFC",
                                "M53FC"
                            };
                        }
                        else if (str3 == "大发彩票")
                        {
                            mRYLInfo = AppInfo.DACPInfo;
                            list2 = new List<string> { 
                                "DAFFC",
                                "DA3FC"
                            };
                        }
                        else if (str3 == "玩家世界")
                        {
                            mRYLInfo = AppInfo.WJSJInfo;
                            list2 = new List<string> { 
                                "DJSSC",
                                "KLFFC",
                                "KL2FC"
                            };
                        }
                        else if (str3 == "众赢娱乐")
                        {
                            mRYLInfo = AppInfo.UCYLInfo;
                            list2 = new List<string> { 
                                "UCFFC",
                                "UC5FC",
                                "UCHGSSC",
                                "UCTWSSC",
                                "UCHL2FC"
                            };
                        }
                        else if (str3 == "拉菲娱乐")
                        {
                            mRYLInfo = AppInfo.LFYLInfo;
                            list2 = new List<string> { 
                                "TWSSC",
                                "BJSSC",
                                "JNDSSC",
                                "MD2FC",
                                "LFFFC",
                                "LF2FC",
                                "LF5FC"
                            };
                        }
                        else if (str3 == "乐丰国际")
                        {
                            mRYLInfo = AppInfo.LFGJInfo;
                            list2 = new List<string> { 
                                "LEFFFC",
                                "LEF2FC",
                                "LEF5FC"
                            };
                        }
                        else if (str3 == "利信娱乐")
                        {
                            mRYLInfo = AppInfo.LXYLInfo;
                            list2 = new List<string> { 
                                "SESSC",
                                "LXFFC",
                                "LX3FC",
                                "LX5FC",
                                "SH11X5",
                                "AH11X5"
                            };
                        }
                        else if (str3 == "K5娱乐")
                        {
                            mRYLInfo = AppInfo.K5YLInfo;
                            list2 = new List<string> { 
                                "K5FFC",
                                "K55FC"
                            };
                        }
                        else if (str3 == "必火直营")
                        {
                            mRYLInfo = AppInfo.BHZYInfo;
                            list2 = new List<string> { 
                                "BHZYFFC",
                                "BHZY5FC"
                            };
                        }
                        else if (str3 == "A6娱乐")
                        {
                            mRYLInfo = AppInfo.A6YLInfo;
                            list2 = new List<string> { 
                                "A6FFC",
                                "A65FC"
                            };
                        }
                        else if (str3 == "欧亿娱乐")
                        {
                            mRYLInfo = AppInfo.OEYLInfo;
                            list2 = new List<string> { 
                                "OEFFC",
                                "OE3FC"
                            };
                        }
                        else if (str3 == "易发彩票")
                        {
                            mRYLInfo = AppInfo.YIFAInfo;
                            list2 = new List<string> { 
                                "YIFAFFC",
                                "YIFA5FC",
                                "DJSSC"
                            };
                        }
                        else if (str3 == "新宝3")
                        {
                            mRYLInfo = AppInfo.XB3Info;
                            list2 = new List<string> { 
                                "XJPSSC",
                                "QQFFC"
                            };
                        }
                        else if (str3 == "金狐娱乐")
                        {
                            mRYLInfo = AppInfo.JHYLInfo;
                            list2 = new List<string> { "JHJPZ15C" };
                        }
                        else if (str3 == "拉菲II")
                        {
                            mRYLInfo = AppInfo.LF2Info;
                            list2 = new List<string> { 
                                "LFHGSSC",
                                "LFDJSSC",
                                "LF2FFC",
                                "LF22FC",
                                "LF25FC"
                            };
                        }
                        else if (str3 == "彩天堂")
                        {
                            mRYLInfo = AppInfo.CTTInfo;
                            list2 = new List<string> { "HLCFLB15C" };
                        }
                        else if (str3 == "SKY娱乐")
                        {
                            mRYLInfo = AppInfo.SKYYLInfo;
                            list2 = new List<string> { 
                                "SKYFFC",
                                "SKY2FC",
                                "DB15C",
                                "VRSSC",
                                "VRHXSSC",
                                "VR3FC",
                                "VRPK10"
                            };
                        }
                        else if (str3 == "鹿鼎娱乐")
                        {
                            mRYLInfo = AppInfo.LUDIInfo;
                            list2 = new List<string> { 
                                "TXFFC",
                                "MD2FC",
                                "LUDIFFC",
                                "LUDI3FC"
                            };
                        }
                        else if (str3 == "亿人娱乐")
                        {
                            mRYLInfo = AppInfo.YRYLInfo;
                            list2 = new List<string> { 
                                "YRALFFC",
                                "YRHG15C",
                                "YRTXFFC",
                                "YRFFC",
                                "YR2FC",
                                "YR5FC"
                            };
                        }
                        else if (str3 == "纵达娱乐")
                        {
                            mRYLInfo = AppInfo.ZDYLInfo;
                            list2 = new List<string> { 
                                "ZDTGSSC",
                                "ZDDJSSC",
                                "ZDHS15F",
                                "ZDRBSSC",
                                "ZDFFC",
                                "ZD5FC"
                            };
                        }
                        else if (str3 == "Z娱乐")
                        {
                            mRYLInfo = AppInfo.ZYLInfo;
                            list2 = new List<string> { 
                                "ZYLTGSSC",
                                "ZYLRBSSC",
                                "ZYLFFC",
                                "ZYL5FC",
                                "ZYLDJSSC",
                                "ZYLHS15F"
                            };
                        }
                        else if (str3 == "BA娱乐")
                        {
                            mRYLInfo = AppInfo.BAYLInfo;
                            list2 = new List<string> { 
                                "NYFFC",
                                "NY3FC",
                                "NY5FC"
                            };
                        }
                        else if (str3 == "四季娱乐")
                        {
                            mRYLInfo = AppInfo.SIJIInfo;
                            list2 = new List<string> { 
                                "SIJIFFC",
                                "SIJI3FC",
                                "SIJI5FC",
                                "SIJITXYFC",
                                "SIJIDJSSC",
                                "SIJIFLBSSC",
                                "SIJIELSSSC",
                                "SIJIHGSSC",
                                "SIJIFF11X5",
                                "SIJI3F11X5",
                                "SIJI5F11X5"
                            };
                        }
                        else if (str3 == "欢乐城")
                        {
                            mRYLInfo = AppInfo.HLCInfo;
                            list2 = new List<string> { 
                                "HLCHG15F",
                                "HLCDJ15F",
                                "HLCSE15F",
                                "HLCNY15C",
                                "HLCFLB15C",
                                "HLCFLB2FC",
                                "HLCFLB5FC"
                            };
                        }
                        else if (str3 == "仁鼎娱乐")
                        {
                            mRYLInfo = AppInfo.RDYLInfo;
                            list2 = new List<string> { 
                                "HLCHG15F",
                                "HLCDJ15F",
                                "HLCSE15F",
                                "HLCNY15C"
                            };
                        }
                        else if (str3 == "宏達娱乐")
                        {
                            mRYLInfo = AppInfo.HONDInfo;
                            list2 = new List<string> { 
                                "HLCFLB15C",
                                "HLCFLB2FC",
                                "HLCFLB5FC",
                                "HONDSE15F",
                                "HONDNY15C",
                                "HONDHG15F",
                                "HONDDJ15F",
                                "HONDFFC",
                                "HOND2FC",
                                "HOND5FC"
                            };
                        }
                        else if (str3 == "菲洛城")
                        {
                            mRYLInfo = AppInfo.FLCInfo;
                            list2 = new List<string> { 
                                "HLCSE15F",
                                "HLCNY15C",
                                "HLCHG15F",
                                "HLCDJ15F",
                                "HLCFLB15C",
                                "HLCFLB2FC",
                                "HLCFLB5FC",
                                "FLCFFC",
                                "FLC2FC",
                                "FLC5FC"
                            };
                        }
                        else if (str3 == "万森娱乐")
                        {
                            mRYLInfo = AppInfo.WSYLInfo;
                            list2 = new List<string> { 
                                "WS30M",
                                "WSBLS60M",
                                "WSHS15F",
                                "WSFS15F",
                                "WSXDL15F"
                            };
                        }
                        else if (str3 == "万彩娱乐")
                        {
                            mRYLInfo = AppInfo.WCYLInfo;
                            list2 = new List<string> { 
                                "WCFLPFFC",
                                "WCJPZFFC",
                                "WCDBFFC",
                                "WCMDJB15C",
                                "WCMGDZ2FC",
                                "WCMG45M",
                                "WCMLXY3FC",
                                "WCTWFFC",
                                "WCXBYFFC",
                                "WCXDL15C",
                                "WCYGFFC"
                            };
                        }
                        else if (str3 == "万恒娱乐")
                        {
                            mRYLInfo = AppInfo.WHENInfo;
                            list2 = new List<string> { 
                                "WHEN30M",
                                "WHENBLS60M",
                                "WHENHS15F",
                                "WHENFS15F",
                                "WHENXDL15F",
                                "WHENNDJSSC",
                                "WHENELSSSC",
                                "WHENQQFFC",
                                "WHENFLP15C",
                                "WHENHGSSC",
                                "WHENDJSSC",
                                "WHENXJPSSC",
                                "WHENYG60M",
                                "WHEN11X5",
                                "WHEN120MPK10",
                                "WHEN180MPK10"
                            };
                        }
                        else if (str3 == "华纳娱乐")
                        {
                            mRYLInfo = AppInfo.HNYLInfo;
                            list2 = new List<string> { 
                                "HNYL5FC",
                                "HNYLFFC",
                                "HNYLFLP15C",
                                "HNYLXJPSSC",
                                "HNYL11X5"
                            };
                        }
                        else if (str3 == "迪拜城")
                        {
                            mRYLInfo = AppInfo.DPCInfo;
                            list2 = new List<string> { 
                                "DPCOZBWC",
                                "DPCTXFFC",
                                "DPCFFC",
                                "DPC5FC"
                            };
                        }
                        else if (str3 == "红馆迪拜")
                        {
                            mRYLInfo = AppInfo.HGDBInfo;
                            list2 = new List<string> { 
                                "DPCDJSSC",
                                "ELSSSC",
                                "XXLSSC",
                                "HGDBFFC",
                                "HGDB5FC"
                            };
                        }
                        else if (str3 == "博牛国际")
                        {
                            mRYLInfo = AppInfo.BNGJInfo;
                            list2 = new List<string> { 
                                "BNFFC",
                                "BN5FC",
                                "BNTXFFC"
                            };
                        }
                        else if (str3 == "博美娱乐")
                        {
                            mRYLInfo = AppInfo.BMEIInfo;
                            list2 = new List<string> { 
                                "BMTXFFC",
                                "BMEIFFC",
                                "BMEI5FC",
                                "BMEISE15F",
                                "BMHGSSC",
                                "BMDJSSC",
                                "BMFLBSSC",
                                "BMTWFFC",
                                "BMQQFFC"
                            };
                        }
                        else if (str3 == "美娱国际")
                        {
                            mRYLInfo = AppInfo.MYGJInfo;
                            list2 = new List<string> { "MYOZBWC" };
                        }
                        else if (str3 == "桃花岛娱乐")
                        {
                            mRYLInfo = AppInfo.THDYLInfo;
                            list2 = new List<string> { 
                                "BLSFFC",
                                "XDLSSC",
                                "JZD15FC",
                                "LD2FC"
                            };
                        }
                        else if (str3 == "游艇会(UT8)")
                        {
                            mRYLInfo = AppInfo.UT8Info;
                            list2 = new List<string> { 
                                "UT8FFC",
                                "UT83FC",
                                "XDLSSC",
                                "UT8HGSSC",
                                "UT8DJSSC",
                                "BLSFFC",
                                "JZD15FC",
                                "LD2FC",
                                "XDL11X5",
                                "JZD11X5",
                                "LD11X5",
                                "XDLPK10",
                                "JZDPK10",
                                "LDPK10"
                            };
                        }
                        else if (str3 == "航洋国际")
                        {
                            mRYLInfo = AppInfo.HANYInfo;
                            list2 = new List<string> { 
                                "HANYDJSSC",
                                "HANYHGSSC",
                                "HANYTXFFC",
                                "HANYXJP30M",
                                "HANYFLP30M",
                                "HANYFLPFFC",
                                "HANYFLP2FC",
                                "HANYJPZ30M",
                                "HANYJPZFFC",
                                "HANYJPZ5FC",
                                "HANYMD30M",
                                "HANYMDFFC",
                                "HANYMD3FC"
                            };
                        }
                        else if (str3 == "新火巅峰")
                        {
                            mRYLInfo = AppInfo.XHDFInfo;
                            list2 = new List<string> { 
                                "XHDFFFC",
                                "XHDF5FC",
                                "BJSSC",
                                "JNDSSC",
                                "TWSSC",
                                "ELSSSC",
                                "XHDFDJSSC",
                                "XHDFHGSSC",
                                "XHDFTXFFC",
                                "XXLSSC"
                            };
                        }
                        else if (str3 == "万兴国际")
                        {
                            mRYLInfo = AppInfo.YDYLInfo;
                            list2 = new List<string> { 
                                "YDFFPK10",
                                "YDXXLSSC",
                                "YDHGSSC",
                                "YDFFC",
                                "YD2FC",
                                "YDTXFFC"
                            };
                        }
                        else if (str3 == "华众娱乐")
                        {
                            mRYLInfo = AppInfo.HZYLInfo;
                            list2 = new List<string> { 
                                "HZQQFFC",
                                "HZFFC",
                                "HZ3FC",
                                "HZ5FC",
                                "HZXXLSSC",
                                "HZJNDSSC",
                                "HZHG2FC",
                                "HZXJP2FC",
                                "HZHGSSC",
                                "HZDJSSC",
                                "HZXDLSSC",
                                "HZTG15F",
                                "HZML15F",
                                "HZFF11X5",
                                "HZTG11X5"
                            };
                        }
                        else if (str3 == "在线娱乐")
                        {
                            mRYLInfo = AppInfo.ZXYLInfo;
                            list2 = new List<string> { 
                                "ZXGBFFC",
                                "ZXBX3FC",
                                "ZXBX5FC",
                                "ZXXXLSSC",
                                "ZXJNDSSC",
                                "ZXHG2FC",
                                "ZXXJP2FC",
                                "ZXHGSSC",
                                "ZXDJSSC",
                                "ZXXDLSSC",
                                "ZXTG15F",
                                "ZXML15F",
                                "ZXFF11X5"
                            };
                        }
                        else if (str3 == "疯彩娱乐")
                        {
                            mRYLInfo = AppInfo.FCYLInfo;
                            list2 = new List<string> { 
                                "QQFFC",
                                "LFHGSSC",
                                "LFDJSSC",
                                "XYFTPK10",
                                "FCOZ3FC",
                                "FCSLFK5FC"
                            };
                        }
                        else if (str3 == "彩宏娱乐")
                        {
                            mRYLInfo = AppInfo.CAIHInfo;
                            list2 = new List<string> { 
                                "CAIHPK10",
                                "CAIHFLP15C",
                                "CAIHFLP2FC",
                                "CAIHFLP5FC",
                                "CAIHDLD30M",
                                "CAIHSE15F",
                                "CAIHNY15C",
                                "CAIHXWYFFC",
                                "CAIHXDLSSC",
                                "CAIHHGSSC",
                                "CAIHDJSSC",
                                "CAIHXJPSSC",
                                "CAIHLD2FC"
                            };
                        }
                        else if (str3 == "鑫旺娱乐")
                        {
                            mRYLInfo = AppInfo.XWYLInfo;
                            list2 = new List<string> { 
                                "XWYLXDLSSC",
                                "XWYLXWYFFC",
                                "XWYLXJPSSC",
                                "XWYLFL30M",
                                "XWYLTX30M",
                                "XWYLQQFFC",
                                "XWYLLD2FC",
                                "XWYLHGSSC",
                                "XWYLDJSSC",
                                "XWYLSE15F",
                                "XWYLNY15C"
                            };
                        }
                        else if (str3 == "青蜂在线")
                        {
                            mRYLInfo = AppInfo.QFZXInfo;
                            list2 = new List<string> { 
                                "QFZXXDLSSC",
                                "QFZXXWYFFC",
                                "QFZXHGSSC",
                                "QFZXXJPSSC",
                                "QFZXDJSSC",
                                "BJSSC",
                                "TWSSC",
                                "QFZXFL30M",
                                "QFZXTX30M",
                                "QFZXQQFFC",
                                "QFZXLD2FC",
                                "QFZXSE15F",
                                "QFZXNY15C",
                                "QFZXPK10"
                            };
                        }
                        else if (str3 == "彩部落")
                        {
                            mRYLInfo = AppInfo.CBLInfo;
                            list2 = new List<string> { 
                                "CBLDJSSC",
                                "CBLXJPSSC",
                                "CBLLD2FC",
                                "CBLXDLSSC",
                                "CBLHGSSC",
                                "CBLDLD30M",
                                "CBLXWYFFC",
                                "CBLSE15F",
                                "CBLNY15C",
                                "CBLPK10"
                            };
                        }
                        else if (str3 == "马泰娱乐")
                        {
                            mRYLInfo = AppInfo.MTYLInfo;
                            list2 = new List<string> { 
                                "MTDJSSC",
                                "MTXJPSSC",
                                "MTLD2FC",
                                "MTXDLSSC",
                                "MTHGSSC",
                                "MTDLD30M",
                                "MTXWYFFC",
                                "MTSE15F",
                                "MTNY15C",
                                "MTPK10"
                            };
                        }
                        else if (str3 == "彩云娱乐")
                        {
                            mRYLInfo = AppInfo.CYYLInfo;
                            list2 = new List<string> { 
                                "CYYLXJPSSC",
                                "CYYLLD2FC",
                                "CYYLDJSSC",
                                "CYYLSE15F",
                                "CYYLXDLSSC",
                                "CYYLXWYFFC",
                                "CYYLDLD30M",
                                "CYYLHGSSC",
                                "CYYLNY15C"
                            };
                        }
                        else if (str3 == "新泰娱乐")
                        {
                            mRYLInfo = AppInfo.XTYLInfo;
                            list2 = new List<string> { 
                                "XTYLHGSSC",
                                "XTYLDLD30M",
                                "XTYLXWYFFC",
                                "XTYLDJSSC",
                                "XTYLXJPSSC",
                                "XTYLLD2FC",
                                "XTYLPK10"
                            };
                        }
                        else if (str3 == "鲸鱼娱乐")
                        {
                            mRYLInfo = AppInfo.JYYLInfo;
                            list2 = new List<string> { 
                                "JYYLXDLSSC",
                                "JYYLXWYFFC",
                                "JYYLXJPSSC",
                                "JYYLTX30M",
                                "JYYLQQFFC",
                                "JYYLLD2FC",
                                "JYYLHGSSC",
                                "JYYLDJSSC"
                            };
                        }
                        else if (str3 == "梦想娱乐")
                        {
                            mRYLInfo = AppInfo.MXYLInfo;
                            list2 = new List<string> { 
                                "MXYLHGSSC",
                                "MXYLDJSSC",
                                "MXYLXDLSSC",
                                "MXYLXWYFFC",
                                "MXYLXJPSSC",
                                "MXYLLD2FC",
                                "MXYLTX30M",
                                "MXYLQQFFC"
                            };
                        }
                        else if (str3 == "万彩")
                        {
                            mRYLInfo = AppInfo.WCAIInfo;
                            list2 = new List<string> { 
                                "WCAIHGSSC",
                                "WCAIDJSSC",
                                "WCAIXDLSSC",
                                "WCAIXWYFFC",
                                "WCAIXJPSSC",
                                "WCAILD2FC",
                                "WCAIMBE30M",
                                "WCAITWSSC"
                            };
                        }
                        else if (str3 == "彩天下")
                        {
                            mRYLInfo = AppInfo.CTXInfo;
                            list2 = new List<string> { 
                                "CTXDJSSC",
                                "CTXXJPSSC",
                                "CTXLD2FC",
                                "CTXXDLSSC",
                                "CTXHGSSC",
                                "CTXDLD30M",
                                "CTXXWYFFC",
                                "CTXSE15F",
                                "CTXNY15C",
                                "CTXPK10"
                            };
                        }
                        else if (str3 == "天豪娱乐")
                        {
                            mRYLInfo = AppInfo.THYLInfo;
                            list2 = new List<string> { 
                                "THFFC",
                                "THMD2FC",
                                "THDJSSC",
                                "THJDSSC",
                                "THTGSSC",
                                "TH5FC",
                                "THHGSSC",
                                "THPK10",
                                "THOZPK10"
                            };
                        }
                        else if (str3 == "TA娱乐")
                        {
                            mRYLInfo = AppInfo.TAYLInfo;
                            list2 = new List<string> { 
                                "TA11X5FFC",
                                "TA11X53FC",
                                "TATXFFC",
                                "TAWBFFC",
                                "TABL15C",
                                "TAWX15C",
                                "TAHGSSC",
                                "TAFLBSSC",
                                "TAFFC",
                                "TA3FC",
                                "TA5FC"
                            };
                        }
                        else if (str3 == "凯鑫娱乐")
                        {
                            mRYLInfo = AppInfo.KXYLInfo;
                            list2 = new List<string> { 
                                "KXTXFFC",
                                "KXHGSSC",
                                "KXBL15C",
                                "KXWX15C",
                                "KXFLBSSC",
                                "KXWBFFC",
                                "KXYNFFC",
                                "KXYN3FC",
                                "KXYN5FC",
                                "KX11X5FFC",
                                "KX11X53FC"
                            };
                        }
                        else if (str3 == "汇盛国际")
                        {
                            mRYLInfo = AppInfo.HSGJInfo;
                            list2 = new List<string> { 
                                "HSFFC",
                                "HS5FC",
                                "HSDJSSC",
                                "HSAZXYC",
                                "HSQQFFC",
                                "HSSE15F"
                            };
                        }
                        else if (str3 == "澳利国际")
                        {
                            mRYLInfo = AppInfo.ALGJInfo;
                            list2 = new List<string> { 
                                "ALGJFFC",
                                "ALGJ5FC",
                                "ALGJRB15C",
                                "ALGJSE15C",
                                "ALGJTXFFC",
                                "ALGJNTXFFC",
                                "ALGJOZBWC",
                                "ALGJFF11X5"
                            };
                        }
                        else if (str3 == "公爵娱乐")
                        {
                            mRYLInfo = AppInfo.GJYLInfo;
                            list2 = new List<string> { 
                                "GJFFC",
                                "GJ5FC",
                                "GJTX60M",
                                "GJOZBWC",
                                "GJFF11X5"
                            };
                        }
                        else if (str3 == "汇众娱乐")
                        {
                            mRYLInfo = AppInfo.HUIZInfo;
                            list2 = new List<string> { 
                                "HUIZFFC",
                                "HUIZ5FC",
                                "HUIZHGSSC",
                                "HUIZJN15C",
                                "HUIZXJPSSC",
                                "HSAZXYC",
                                "HUIZFF11X5",
                                "HUIZFFPK10",
                                "XYFTPK10"
                            };
                        }
                        else if (str3 == "大众娱乐")
                        {
                            mRYLInfo = AppInfo.DAZYLInfo;
                            list2 = new List<string> { 
                                "DAZRBSSC",
                                "DAZFFC",
                                "DAZ5FC",
                                "DAZTG15C",
                                "DAZDJSSC",
                                "DAZJDSSC",
                                "DAZHGSSC",
                                "DAZ11X5"
                            };
                        }
                        else if (str3 == "十里桃花")
                        {
                            mRYLInfo = AppInfo.SLTHInfo;
                            list2 = new List<string> { 
                                "TXFFC",
                                "SLTHHGSSC",
                                "SLTHNHGSSC",
                                "SLTHDJSSC",
                                "SLTHFFC",
                                "SLTH2FC"
                            };
                        }
                        else if (str3 == "翡翠娱乐")
                        {
                            mRYLInfo = AppInfo.FEICInfo;
                            list2 = new List<string> { 
                                "FEIC3FC",
                                "FEICSLFK5FC",
                                "FEICFFC",
                                "FEICDJSSC",
                                "FEICHGSSC",
                                "FEIC30M",
                                "FEIC45M",
                                "FEICFF11X5",
                                "FEIC2F11X5",
                                "FEIC3F11X5",
                                "FEIC5F11X5",
                                "FEICFFPK10",
                                "FEIC2FPK10",
                                "FEIC3FPK10",
                                "FEIC5FPK10"
                            };
                        }
                        else if (str3 == "中呗娱乐")
                        {
                            mRYLInfo = AppInfo.ZBEIInfo;
                            list2 = new List<string> { 
                                "ZBEIXJP2FC",
                                "ZBEI3FC",
                                "ZBEISLFK5FC",
                                "ZBEIFFC",
                                "ZBEIDJSSC",
                                "ZBEIHGSSC",
                                "ZBEI30M",
                                "ZBEI45M",
                                "ZBEIQQFFC",
                                "ZBEIWXFFC",
                                "ZBEIGGFFC",
                                "ZBEIXJP15C",
                                "ZBEIXDL15C",
                                "ZBEIJNDSSC",
                                "ZBEIFF11X5",
                                "ZBEI2F11X5",
                                "ZBEI3F11X5",
                                "ZBEI5F11X5",
                                "ZBEIFFPK10",
                                "ZBEI2FPK10",
                                "ZBEI3FPK10",
                                "ZBEI5FPK10"
                            };
                        }
                        else if (str3 == "玖富娱乐")
                        {
                            mRYLInfo = AppInfo.JFYLInfo;
                            list2 = new List<string> { 
                                "JFYLBHD15C",
                                "JFYLFFC",
                                "JFYL2FC",
                                "JFYL2F11X5"
                            };
                        }
                        else if (str3 == "K3娱乐")
                        {
                            mRYLInfo = AppInfo.K3YLInfo;
                            list2 = new List<string> { 
                                "K3TXFFC",
                                "K3NTXFFC",
                                "K3XXLSSC",
                                "K3DJFFC",
                                "K3HG2FC",
                                "K3MG5FC",
                                "K3HGSSC",
                                "K3DJSSC",
                                "K311X5"
                            };
                        }
                        else if (str3 == "长隆娱乐")
                        {
                            mRYLInfo = AppInfo.CLYLInfo;
                            list2 = new List<string> { 
                                "CLYLAM15C",
                                "CLYLTB15C",
                                "CLYLDJSSC",
                                "CLYLXDL15F",
                                "CLYLHGSSC",
                                "CLYLFFC",
                                "CLYL2FC",
                                "CLYL3FC",
                                "CLYL5FC",
                                "CLYLFF11X5",
                                "CLYL2F11X5",
                                "CLYL3F11X5",
                                "CLYL5F11X5"
                            };
                        }
                        else if (str3 == "恒瑞")
                        {
                            mRYLInfo = AppInfo.HENRInfo;
                            list2 = new List<string> { 
                                "HENRTXFFC",
                                "HENRDJ2FC",
                                "HENRXJPSSC",
                                "HENROZ15C",
                                "HENRXG15C",
                                "HENRFFC",
                                "HENR2FC",
                                "HENR3FC"
                            };
                        }
                        else if (str3 == "宏达娱乐")
                        {
                            mRYLInfo = AppInfo.HDYLInfo;
                            list2 = new List<string> { 
                                "HDYLFFC",
                                "HDYL2FC",
                                "HDYL5FC",
                                "HDYLASKFFC",
                                "HDYLFF11X5",
                                "HDYL2F11X5",
                                "HDYLFFPK10",
                                "HDYLFFFT"
                            };
                        }
                        else if (str3 == "香格里拉")
                        {
                            mRYLInfo = AppInfo.XGLLInfo;
                            list2 = new List<string> { 
                                "XGLLWYN30M",
                                "XGLLDJSSC",
                                "XGLLLSJ2FC",
                                "XGLLFLP15C",
                                "XGLLWNS15C",
                                "XGLLLFFC",
                                "XGLLL2FC",
                                "XGLLLSY11X5",
                                "XGLLLDPK10",
                                "XGLLSSPK10"
                            };
                        }
                        else if (str3 == "侏罗纪")
                        {
                            mRYLInfo = AppInfo.ZLJInfo;
                            list2 = new List<string> { 
                                "ZLJMGFFC",
                                "ZLJSE15C",
                                "ZLJDJSSC",
                                "ZLJFLP2FC",
                                "ZLJELS5FC",
                                "ZLJFF11X5",
                                "ZLJFFPK10"
                            };
                        }
                        else if (str3 == "博客彩")
                        {
                            mRYLInfo = AppInfo.BKCInfo;
                            list2 = new List<string> { 
                                "JN15F",
                                "BKCFFC",
                                "BKC2FC",
                                "BKC5FC",
                                "BKC11X5FFC"
                            };
                        }
                        else if (str3 == "豪客彩")
                        {
                            mRYLInfo = AppInfo.HKCInfo;
                            list2 = new List<string> { 
                                "HKCFFC",
                                "HKC2FC",
                                "HKC5FC",
                                "HKC11X5FFC"
                            };
                        }
                        else if (str3 == "B6娱乐城")
                        {
                            mRYLInfo = AppInfo.B6YLInfo;
                            list2 = new List<string> { 
                                "B6YLFFC",
                                "B6YL3FC",
                                "B6YL5FC",
                                "B6YL3F11X5"
                            };
                        }
                        else if (str3 == "众博娱乐")
                        {
                            mRYLInfo = AppInfo.ZBYLInfo;
                            list2 = new List<string> { 
                                "ZBYLGGFFC",
                                "ZBYLTX2FC",
                                "ZBYLFFC",
                                "ZBYL2FC",
                                "ZBYL3FC",
                                "ZBYL5FC",
                                "ZBYLFF11X5",
                                "ZBYL2F11X5",
                                "ZBYL3F11X5",
                                "ZBYL5F11X5"
                            };
                        }
                        else if (str3 == "网赚娱乐")
                        {
                            mRYLInfo = AppInfo.WZYLInfo;
                            list2 = new List<string> { 
                                "WZYLJDSSC",
                                "WZYLTXFFC",
                                "WZYLHGSSC",
                                "WZYLHG1FC",
                                "WZYLDJSSC",
                                "WZYLXJPSSC",
                                "WZYLXGSSC",
                                "WZYLLSWJS15C",
                                "WZYLMG35C",
                                "WZYLBL1FC",
                                "WZYLMG15C",
                                "WZYLTG11X5",
                                "WZYLBL11X5",
                                "WZYLJSPK10"
                            };
                        }
                        else if (str3 == "亚洲彩票")
                        {
                            mRYLInfo = AppInfo.YZCPInfo;
                            list2 = new List<string> { 
                                "YZCPYN5FC",
                                "YZCPTG15C",
                                "YZCPJSFFC",
                                "YZCPRBSSC",
                                "YZCPHGSSC",
                                "YZCPXJPSSC",
                                "YZCPJDSSC",
                                "YZCPMG35C",
                                "YZCPLSWJS15C",
                                "YZCPBL1FC",
                                "YZCPMG15C",
                                "YZCPHG1FC",
                                "YZCPTG30M",
                                "YZCPTG11X5",
                                "YZCPXG11X5",
                                "YZCPJSPK10"
                            };
                        }
                        else if (str3 == "天宇娱乐")
                        {
                            mRYLInfo = AppInfo.TIYUInfo;
                            list2 = new List<string> { 
                                "TIYUML2FC",
                                "TIYUFLP15C",
                                "TIYUNHG15C",
                                "TIYUXDL15C",
                                "TIYUXJPSSC"
                            };
                        }
                        else if (str3 == "飞牛游戏")
                        {
                            mRYLInfo = AppInfo.FNYXInfo;
                            list2 = new List<string> { 
                                "FNYXFFC",
                                "FNYXHY11X5"
                            };
                        }
                        else if (str3 == "喜鹊娱乐")
                        {
                            mRYLInfo = AppInfo.XQYLInfo;
                            list2 = new List<string> { 
                                "XQYLFFC",
                                "XQYL3FC",
                                "XQYL5FC",
                                "XQYLHGSSC",
                                "XQYLDJSSC",
                                "XQYLSE15F",
                                "XQYLNY15C",
                                "XQYLQQFFC",
                                "XQYLBDFFC",
                                "XQYLWBFFC",
                                "XQYL11X5",
                                "XQYLJS11X5",
                                "XQYLJSPK10"
                            };
                        }
                        AppInfo.OpenDataLoginLotteryDic[str3] = list2[0];
                        pInfo.LotteryIDList = list2;
                        pInfo.LoginLineList = PTBase.GetSkipUrlList(pInfo.LoginLineList, mRYLInfo);
                        PTOpenDataLine line = new PTOpenDataLine();
                        line.LoadPTOpenDataLine(pInfo, base.RegConfigPath);
                        line.PTInfo = mRYLInfo;
                        line.Dock = DockStyle.Fill;
                        line.GetOpenCJ.Tag = str3;
                        line.GetOpenCJ.CheckedChanged += new EventHandler(this.OpenCJ_CheckedChanged);
                        List<OpenDataLine> getOpenDataLine = line.GetOpenDataLine;
                        foreach (OpenDataLine line2 in getOpenDataLine)
                        {
                            this.OpenDataLineList.Add(line2);
                            line2.PTInfo = mRYLInfo;
                            line2.GetLotterySelect.CheckedChanged += new EventHandler(this.LotterySelect_CheckedChanged);
                        }
                        this.PTControlDic[str3] = line;
                        this.FillPTLineList(str3);
                        TabPage page = new TabPage {
                            Text = str3,
                            Controls = { line }
                        };
                        this.Tab_Main.TabPages.Add(page);
                    }
                    catch
                    {
                    }
                }
            }
        }

        public void RefreshData(List<string> pList, ConfigurationStatus.LotteryType pLottery, int pDataCount)
        {
            try
            {
                ConfigurationStatus.OpenData openDataFirstExpect = this.GetOpenDataFirstExpect(pList);
                string lotteryID = CommFunc.GetLotteryID(pLottery);
                foreach (OpenDataLine line in this.OpenDataLineList)
                {
                    if (line.LotteryID == lotteryID)
                    {
                        line.RefreshNewData(openDataFirstExpect);
                        line.Count = pDataCount;
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                DebugLog.SaveDebug(exception, "");
            }
        }

        private void RemoveLoginLock(string pError, string pName = "")
        {
            this.PTControlDic[pName].RefreshLogin(false, $"登录失败-{pError}");
        }

        public void SwitchNextLine(bool Switch, string pName = "")
        {
            PTOpenDataLine line = this.PTControlDic[pName];
            if (!line.PTInfo.IsLoginRun)
            {
                if (Switch)
                {
                    line.PTInfo.SwitchNextLine();
                }
                this.LoginMain(pName);
            }
        }
    }
}

