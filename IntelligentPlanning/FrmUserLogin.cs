namespace IntelligentPlanning
{
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class FrmUserLogin : ExForm
    {
        public ConfigurationStatus.SCAccountData AccountData;
        private Button Btn_Register1;
        private CheckBox Ckb_BD;
        private CheckBox Ckb_EditPW;
        private CheckBox Ckb_LoginWebHint;
        private CheckBox Ckb_Ok;
        private CheckBox Ckb_QQ;
        private CheckBox Ckb_Register;
        private CheckBox Ckb_UserCZ;
        private IContainer components = null;
        private Label Lbl_BDID;
        private Label Lbl_BDPW;
        private Label Lbl_CZID;
        private Label Lbl_CZPW;
        private Label Lbl_EditID;
        private Label Lbl_EditPW;
        private Label Lbl_EditPW1;
        private Label Lbl_EditPW2;
        private Label Lbl_LoginHint;
        private Label Lbl_LoginID;
        private Label Lbl_LoginPW;
        private Label Lbl_RegisterHint;
        private Label Lbl_RegisterID;
        private Label Lbl_RegisterID1;
        private Label Lbl_RegisterPhone;
        private Label Lbl_RegisterPW;
        private Label Lbl_RegisterPW1;
        private Label Lbl_RegisterPW2;
        private Label Lbl_RegisterPW3;
        private Label Lbl_RegisterQQ;
        private string MachineCode = "";
        private Panel Pnl_Main;
        private Panel Pnl_Register;
        private Panel Pnl_Register1;
        private Dictionary<string, ConfigurationStatus.PTLine> PTLineDic = new Dictionary<string, ConfigurationStatus.PTLine>();
        private List<string> QQList = new List<string>();
        private TabControl Tab_Main;
        private TabPage Tap_BDApp;
        private TabPage Tap_EditPW;
        private TabPage Tap_Login;
        private TabPage Tap_Register;
        private TabPage Tap_UserCZ;
        private TextBox Txt_BDID;
        private TextBox Txt_BDPW;
        private TextBox Txt_CZID;
        private TextBox Txt_CZPW;
        private TextBox Txt_EditID;
        private TextBox Txt_EditPW;
        private TextBox Txt_EditPW1;
        private TextBox Txt_EditPW2;
        private TextBox Txt_LoginID;
        private TextBox Txt_LoginPW;
        private TextBox Txt_RegisterID;
        private TextBox Txt_RegisterID1;
        private TextBox Txt_RegisterPhone;
        private TextBox Txt_RegisterPW;
        private TextBox Txt_RegisterPW1;
        private TextBox Txt_RegisterPW2;
        private TextBox Txt_RegisterPW3;
        private TextBox Txt_RegisterQQ;
        private WebBrowser Web_GetQQList = new WebBrowser();

        public FrmUserLogin(ConfigurationStatus.SCAccountData pAccountData)
        {
            this.InitializeComponent();
            this.AccountData = pAccountData;
            List<Control> list = new List<Control> {
                this,
                this.Txt_LoginID,
                this.Txt_LoginPW
            };
            base.ControlList = list;
            string appText = "永信在线挂机软件";
            if (AppInfo.Account.Configuration.AppText != "")
            {
                appText = AppInfo.Account.Configuration.AppText;
            }
            this.Text = $"{appText}-登录";
        }

        private void BeautifyInterface()
        {
            if (AppInfo.Account.Configuration.Beautify)
            {
                List<Control> pControlList = new List<Control> {
                    this.Tap_Login,
                    this.Tap_Register,
                    this.Tap_BDApp,
                    this.Tap_EditPW,
                    this.Tap_UserCZ
                };
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list2 = new List<Control>();
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Ckb_QQ,
                    this.Ckb_LoginWebHint,
                    this.Ckb_Ok,
                    this.Ckb_Register,
                    this.Ckb_BD,
                    this.Ckb_EditPW,
                    this.Ckb_UserCZ,
                    this.Lbl_LoginID,
                    this.Lbl_LoginPW,
                    this.Lbl_LoginHint,
                    this.Lbl_RegisterID,
                    this.Lbl_RegisterPW,
                    this.Lbl_RegisterPW1,
                    this.Lbl_RegisterHint,
                    this.Lbl_BDID,
                    this.Lbl_BDPW,
                    this.Lbl_EditID,
                    this.Lbl_EditPW,
                    this.Lbl_EditPW1,
                    this.Lbl_EditPW2,
                    this.Lbl_CZID,
                    this.Lbl_CZPW
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
                List<TabControl> pTabControlList = new List<TabControl> {
                    this.Tab_Main
                };
                CommFunc.BeautifyTabControl(pTabControlList);
            }
        }

        private void Btn_BD_Click(object sender, EventArgs e)
        {
            if (this.Txt_BDID.Text == "")
            {
                CommFunc.PublicMessageAll("输入会员账号不能为空！", true, MessageBoxIcon.Asterisk, "");
                this.Txt_BDID.Focus();
            }
            else if (this.Txt_BDPW.Text == "")
            {
                CommFunc.PublicMessageAll("输入会员密码不能为空！", true, MessageBoxIcon.Asterisk, "");
                this.Txt_BDPW.Focus();
            }
            else if (SQLData.UpdateMachineCode(this.Txt_BDID.Text, this.Txt_BDPW.Text, AppInfo.Account.AppName, this.MachineCode))
            {
                CommFunc.PublicMessageAll("绑定成功！", true, MessageBoxIcon.Asterisk, "");
            }
            else
            {
                CommFunc.PublicMessageAll("绑定失败！", true, MessageBoxIcon.Asterisk, "");
                this.Txt_LoginID.SelectAll();
                this.Txt_LoginID.Focus();
            }
        }

        private void Btn_EditPW_Click(object sender, EventArgs e)
        {
            if (this.Txt_EditID.Text == "")
            {
                CommFunc.PublicMessageAll("输入会员账号不能为空！", true, MessageBoxIcon.Asterisk, "");
                this.Txt_EditID.Focus();
            }
            else if (this.Txt_EditPW.Text == "")
            {
                CommFunc.PublicMessageAll("输入原始密码不能为空！", true, MessageBoxIcon.Asterisk, "");
                this.Txt_EditPW.Focus();
            }
            else if (this.Txt_EditPW1.Text == "")
            {
                CommFunc.PublicMessageAll("输入新密码不能为空！", true, MessageBoxIcon.Asterisk, "");
                this.Txt_EditPW1.Focus();
            }
            else if (!this.Txt_EditPW2.Text.Equals(this.Txt_EditPW1.Text))
            {
                CommFunc.PublicMessageAll("两次输入的密码不一致！", true, MessageBoxIcon.Asterisk, "");
                this.Txt_EditPW2.Focus();
            }
            else
            {
                string pHint = "";
                ConfigurationStatus.SCAccountData pAccountData = new ConfigurationStatus.SCAccountData();
                if (SQLData.UserLogin(this.Txt_EditID.Text, this.Txt_EditPW.Text, this.MachineCode, pAccountData, ref pHint))
                {
                    pAccountData.PW = this.Txt_EditPW1.Text;
                    if (SQLData.EditUserRow(pAccountData))
                    {
                        CommFunc.PublicMessageAll("修改密码成功！", true, MessageBoxIcon.Asterisk, "");
                    }
                    else
                    {
                        CommFunc.PublicMessageAll("修改密码失败，请联系客服！", true, MessageBoxIcon.Asterisk, "");
                    }
                }
                else
                {
                    CommFunc.PublicMessageAll("修改密码失败，请联系客服！", true, MessageBoxIcon.Asterisk, "");
                    this.Txt_EditID.SelectAll();
                    this.Txt_EditID.Focus();
                }
            }
        }

        private void Btn_LoginWebHint_Click(object sender, EventArgs e)
        {
            CommFunc.OpenWeb(AppInfo.Account.Configuration.LoginWebValue);
        }

        private void Btn_Ok_Click(object sender, EventArgs e)
        {
            if (this.Txt_LoginID.Text == "")
            {
                CommFunc.PublicMessageAll("输入会员账号不能为空！", true, MessageBoxIcon.Asterisk, "");
                this.Txt_LoginID.Focus();
            }
            else if (this.Txt_LoginPW.Text == "")
            {
                CommFunc.PublicMessageAll("输入会员密码不能为空！", true, MessageBoxIcon.Asterisk, "");
                this.Txt_LoginPW.Focus();
            }
            else
            {
                string pHint = "";
                if (SQLData.UserLogin(this.Txt_LoginID.Text, this.Txt_LoginPW.Text, this.MachineCode, this.AccountData, ref pHint))
                {
                    base.DialogResult = DialogResult.OK;
                }
                else
                {
                    CommFunc.PublicMessageAll(pHint, true, MessageBoxIcon.Asterisk, "");
                    this.Txt_LoginID.SelectAll();
                    this.Txt_LoginID.Focus();
                }
            }
        }

        private void Btn_QQ_Click(object sender, EventArgs e)
        {
            CommFunc.OpenQQWeb(AppInfo.Account.Configuration.QQ);
        }

        private void Btn_Register_Click(object sender, EventArgs e)
        {
            ConfigurationStatus.SCAccountData pAccountData = new ConfigurationStatus.SCAccountData();
            if (AppInfo.Account.Configuration.ViewQQPhone)
            {
                if (this.Txt_RegisterID1.Text == "")
                {
                    CommFunc.PublicMessageAll("输入会员账号不能为空！", true, MessageBoxIcon.Asterisk, "");
                    this.Txt_RegisterID1.Focus();
                    return;
                }
                if (this.Txt_RegisterID1.Text.Length > 0x10)
                {
                    CommFunc.PublicMessageAll($"输入会员账号的长度不能超过{0x10}位！", true, MessageBoxIcon.Asterisk, "");
                    this.Txt_RegisterID1.Focus();
                    return;
                }
                if (this.Txt_RegisterPW2.Text == "")
                {
                    CommFunc.PublicMessageAll("输入会员密码不能为空！", true, MessageBoxIcon.Asterisk, "");
                    this.Txt_RegisterPW2.Focus();
                    return;
                }
                if (this.Txt_RegisterPW2.Text.Length > 0x10)
                {
                    CommFunc.PublicMessageAll($"输入会员密码的长度不能超过{0x10}位！", true, MessageBoxIcon.Asterisk, "");
                    this.Txt_RegisterPW2.Focus();
                    return;
                }
                if (!this.Txt_RegisterPW3.Text.Equals(this.Txt_RegisterPW2.Text))
                {
                    CommFunc.PublicMessageAll("两次输入的密码不一致！", true, MessageBoxIcon.Asterisk, "");
                    this.Txt_RegisterPW3.Focus();
                    return;
                }
                if (this.Txt_RegisterQQ.Text == "")
                {
                    CommFunc.PublicMessageAll("输入会员QQ不能为空！", true, MessageBoxIcon.Asterisk, "");
                    this.Txt_RegisterQQ.Focus();
                    return;
                }
                if (!AppInfo.Account.Configuration.HidePhone && (this.Txt_RegisterPhone.Text == ""))
                {
                    CommFunc.PublicMessageAll("输入会员电话不能为空！", true, MessageBoxIcon.Asterisk, "");
                    this.Txt_RegisterPhone.Focus();
                    return;
                }
                pAccountData.ID = this.Txt_RegisterID1.Text;
                pAccountData.PW = this.Txt_RegisterPW2.Text;
                pAccountData.QQ = this.Txt_RegisterQQ.Text;
                pAccountData.Phone = this.Txt_RegisterPhone.Text;
            }
            else
            {
                if (this.Txt_RegisterID.Text == "")
                {
                    CommFunc.PublicMessageAll("输入会员账号不能为空！", true, MessageBoxIcon.Asterisk, "");
                    this.Txt_RegisterID.Focus();
                    return;
                }
                if (this.Txt_RegisterID.Text.Length > 0x10)
                {
                    CommFunc.PublicMessageAll($"输入会员账号的长度不能超过{0x10}位！", true, MessageBoxIcon.Asterisk, "");
                    this.Txt_RegisterID.Focus();
                    return;
                }
                if (this.Txt_RegisterPW.Text == "")
                {
                    CommFunc.PublicMessageAll("输入会员密码不能为空！", true, MessageBoxIcon.Asterisk, "");
                    this.Txt_RegisterPW.Focus();
                    return;
                }
                if (this.Txt_RegisterPW.Text.Length > 0x10)
                {
                    CommFunc.PublicMessageAll($"输入会员密码的长度不能超过{0x10}位！", true, MessageBoxIcon.Asterisk, "");
                    this.Txt_RegisterPW.Focus();
                    return;
                }
                if (!this.Txt_RegisterPW.Text.Equals(this.Txt_RegisterPW1.Text))
                {
                    CommFunc.PublicMessageAll("两次输入的密码不一致！", true, MessageBoxIcon.Asterisk, "");
                    this.Txt_RegisterPW1.Focus();
                    return;
                }
                pAccountData.ID = this.Txt_RegisterID.Text;
                pAccountData.PW = this.Txt_RegisterPW.Text;
            }
            pAccountData.MachineCode = this.MachineCode;
            pAccountData.Type = ConfigurationStatus.SCAccountType.FULL;
            pAccountData.ActiveTime = DateTime.Now.ToString("yyyy-MM-dd");
            pAccountData.AppName = AppInfo.Account.AppName;
            pAccountData.Configuration.FreeDay = AppInfo.Account.Configuration.FreeDay;
            if (AppInfo.Account.Configuration.BTPTUser)
            {
                if (this.PTLineDic.Count == 0)
                {
                    string webData = HttpHelper.GetWebData(AppInfo.cPTLineFile(""), "");
                    CommFunc.AnalysisPTLine(ref this.PTLineDic, webData);
                }
                pAccountData.PTUser = CommFunc.CreatPTUserString(pAccountData.ID, this.PTLineDic);
            }
            string pResponseText = SQLData.AddUserRow(pAccountData);
            if (CommFunc.CheckResponseText(pResponseText))
            {
                if (AppInfo.Account.Configuration.FreeHint != "")
                {
                    CommFunc.PublicMessageAll(AppInfo.Account.Configuration.FreeHint, true, MessageBoxIcon.Asterisk, "");
                }
                else
                {
                    CommFunc.PublicMessageAll("注册成功！", true, MessageBoxIcon.Asterisk, "");
                }
            }
            else
            {
                string str3 = "请联系客服人员！";
                if (pResponseText.Contains("插入重复键"))
                {
                    str3 = "该用户名已经存在！";
                    this.Txt_RegisterID.Focus();
                }
                CommFunc.PublicMessageAll($"注册失败，{str3}", true, MessageBoxIcon.Asterisk, "");
            }
        }

        private void Btn_UserCZ_Click(object sender, EventArgs e)
        {
            if (this.Txt_CZID.Text == "")
            {
                CommFunc.PublicMessageAll("输入会员账号不能为空！", true, MessageBoxIcon.Asterisk, "");
                this.Txt_CZID.Focus();
            }
            else if (this.Txt_CZPW.Text == "")
            {
                CommFunc.PublicMessageAll("输入充值密码不能为空！", true, MessageBoxIcon.Asterisk, "");
                this.Txt_CZPW.Focus();
            }
            else
            {
                string text = this.Txt_CZID.Text;
                string password = this.Txt_CZPW.Text;
                string pUsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string localIP = CommFunc.GetLocalIP();
                string xHintStr = SQLData.CZUser(text, password, AppInfo.Account.AppName, pUsedTime, localIP);
                if (!xHintStr.Contains("【错误】-"))
                {
                    CommFunc.PublicMessageAll(xHintStr, false, MessageBoxIcon.Asterisk, "");
                }
                else
                {
                    CommFunc.PublicMessageAll(xHintStr.Replace("【错误】-", ""), true, MessageBoxIcon.Asterisk, "");
                }
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

        private void FrmUserLogin_Load(object sender, EventArgs e)
        {
            this.MachineCode = CommFunc.GetDiskVolumeSerialNumber();
            if (!((AppInfo.Account.Configuration.QQ != "") && AppInfo.Account.Configuration.LoginQQ))
            {
                this.Ckb_QQ.Visible = false;
            }
            this.Pnl_Register.Visible = !AppInfo.Account.Configuration.ViewQQPhone;
            this.Pnl_Register1.Visible = AppInfo.Account.Configuration.ViewQQPhone;
            if (AppInfo.Account.Configuration.ViewQQPhone && AppInfo.Account.Configuration.HidePhone)
            {
                this.Lbl_RegisterPhone.Visible = this.Txt_RegisterPhone.Visible = false;
            }
            if (AppInfo.Account.Configuration.LoginWebKey != "")
            {
                this.Ckb_LoginWebHint.Text = AppInfo.Account.Configuration.LoginWebKey;
                this.Ckb_LoginWebHint.Visible = true;
            }
            this.Lbl_LoginHint.ForeColor = AppInfo.redForeColor;
            this.Lbl_RegisterHint.ForeColor = AppInfo.redForeColor;
            List<CheckBox> pCheckBoxList = new List<CheckBox> {
                this.Ckb_QQ,
                this.Ckb_LoginWebHint,
                this.Ckb_Ok,
                this.Ckb_Register,
                this.Ckb_BD,
                this.Ckb_EditPW,
                this.Ckb_UserCZ
            };
            CommFunc.SetCheckBoxFormatFlat(pCheckBoxList);
            this.BeautifyInterface();
            if (this.AccountData.Configuration.OnlyLogin)
            {
                this.Tap_Register.Parent = this.Tap_EditPW.Parent = this.Tap_BDApp.Parent = (Control) (this.Tap_UserCZ.Parent = null);
                if ((AppInfo.Account.Configuration.QQ != "") && (AppInfo.Account.Configuration.OnlyLoginHint != ""))
                {
                    this.Lbl_LoginHint.Text = string.Format(AppInfo.Account.Configuration.OnlyLoginHint, AppInfo.Account.Configuration.QQ);
                    this.Lbl_LoginHint.Visible = true;
                    if (AppInfo.Account.Configuration.OnlyLoginImage != "")
                    {
                        string path = CommFunc.getApplicationDataPath();
                        string cServerGGUrl = AppInfo.cServerGGUrl;
                        List<Image> pImageList = new List<Image>();
                        CommFunc.GetWebImage(ref pImageList, cServerGGUrl, path, AppInfo.Account.Configuration.OnlyLoginImage, true);
                        this.Tap_Login.BackgroundImage = pImageList[0];
                    }
                }
            }
            else if (AppInfo.Account.Configuration.OnlyLoginHint != "")
            {
                this.Lbl_LoginHint.Text = AppInfo.Account.Configuration.OnlyLoginHint;
                this.Lbl_LoginHint.Visible = true;
            }
            if (AppInfo.Account.Configuration.RegisterHint != "")
            {
                this.Lbl_RegisterHint.Text = AppInfo.Account.Configuration.RegisterHint;
                this.Lbl_RegisterHint.Visible = true;
            }
        }

        public void GetLoginQQ()
        {
            this.Web_GetQQList.Navigate("http://xui.ptlogin2.qq.com/cgi-bin/qlogin?domain=qq.com&amp;lang=2052&amp;qtarget=1&amp;jumpname=&amp;appid=549000912&amp;ptcss=undefined&amp;param=u1%253Dhttp%25253A%25252F%25252Fqun.qzone.qq.com%25252Fgroup&amp;css=&amp;mibao_css=&amp;s_url=http%253A%252F%252Fqun.qzone.qq.com%252Fgroup&amp;low_login=0&amp;style=12&amp;authParamUrl=&amp;needVip=1&amp;ptui_version=10028");
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FrmUserLogin));
            this.Pnl_Main = new Panel();
            this.Tab_Main = new TabControl();
            this.Tap_Login = new TabPage();
            this.Ckb_LoginWebHint = new CheckBox();
            this.Ckb_QQ = new CheckBox();
            this.Ckb_Ok = new CheckBox();
            this.Lbl_LoginHint = new Label();
            this.Lbl_LoginID = new Label();
            this.Lbl_LoginPW = new Label();
            this.Txt_LoginID = new TextBox();
            this.Txt_LoginPW = new TextBox();
            this.Tap_Register = new TabPage();
            this.Pnl_Register = new Panel();
            this.Ckb_Register = new CheckBox();
            this.Lbl_RegisterHint = new Label();
            this.Lbl_RegisterID = new Label();
            this.Txt_RegisterPW = new TextBox();
            this.Lbl_RegisterPW1 = new Label();
            this.Txt_RegisterID = new TextBox();
            this.Txt_RegisterPW1 = new TextBox();
            this.Lbl_RegisterPW = new Label();
            this.Pnl_Register1 = new Panel();
            this.Txt_RegisterQQ = new TextBox();
            this.Lbl_RegisterPhone = new Label();
            this.Txt_RegisterPhone = new TextBox();
            this.Lbl_RegisterQQ = new Label();
            this.Lbl_RegisterID1 = new Label();
            this.Btn_Register1 = new Button();
            this.Txt_RegisterPW2 = new TextBox();
            this.Lbl_RegisterPW3 = new Label();
            this.Txt_RegisterID1 = new TextBox();
            this.Txt_RegisterPW3 = new TextBox();
            this.Lbl_RegisterPW2 = new Label();
            this.Tap_BDApp = new TabPage();
            this.Ckb_BD = new CheckBox();
            this.Lbl_BDID = new Label();
            this.Lbl_BDPW = new Label();
            this.Txt_BDID = new TextBox();
            this.Txt_BDPW = new TextBox();
            this.Tap_EditPW = new TabPage();
            this.Ckb_EditPW = new CheckBox();
            this.Lbl_EditPW2 = new Label();
            this.Txt_EditPW2 = new TextBox();
            this.Lbl_EditPW1 = new Label();
            this.Txt_EditPW1 = new TextBox();
            this.Lbl_EditID = new Label();
            this.Lbl_EditPW = new Label();
            this.Txt_EditID = new TextBox();
            this.Txt_EditPW = new TextBox();
            this.Tap_UserCZ = new TabPage();
            this.Ckb_UserCZ = new CheckBox();
            this.Lbl_CZID = new Label();
            this.Lbl_CZPW = new Label();
            this.Txt_CZID = new TextBox();
            this.Txt_CZPW = new TextBox();
            this.Pnl_Main.SuspendLayout();
            this.Tab_Main.SuspendLayout();
            this.Tap_Login.SuspendLayout();
            this.Tap_Register.SuspendLayout();
            this.Pnl_Register.SuspendLayout();
            this.Pnl_Register1.SuspendLayout();
            this.Tap_BDApp.SuspendLayout();
            this.Tap_EditPW.SuspendLayout();
            this.Tap_UserCZ.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_Main.BorderStyle = BorderStyle.Fixed3D;
            this.Pnl_Main.Controls.Add(this.Tab_Main);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(0x15a, 0xdd);
            this.Pnl_Main.TabIndex = 0;
            this.Tab_Main.Controls.Add(this.Tap_Login);
            this.Tab_Main.Controls.Add(this.Tap_Register);
            this.Tab_Main.Controls.Add(this.Tap_BDApp);
            this.Tab_Main.Controls.Add(this.Tap_EditPW);
            this.Tab_Main.Controls.Add(this.Tap_UserCZ);
            this.Tab_Main.Dock = DockStyle.Fill;
            this.Tab_Main.ItemSize = new Size(0x41, 30);
            this.Tab_Main.Location = new Point(0, 0);
            this.Tab_Main.Name = "Tab_Main";
            this.Tab_Main.SelectedIndex = 0;
            this.Tab_Main.Size = new Size(0x156, 0xd9);
            this.Tab_Main.SizeMode = TabSizeMode.Fixed;
            this.Tab_Main.TabIndex = 0x1d;
            this.Tap_Login.BackColor = SystemColors.Control;
            this.Tap_Login.Controls.Add(this.Ckb_LoginWebHint);
            this.Tap_Login.Controls.Add(this.Ckb_QQ);
            this.Tap_Login.Controls.Add(this.Ckb_Ok);
            this.Tap_Login.Controls.Add(this.Lbl_LoginHint);
            this.Tap_Login.Controls.Add(this.Lbl_LoginID);
            this.Tap_Login.Controls.Add(this.Lbl_LoginPW);
            this.Tap_Login.Controls.Add(this.Txt_LoginID);
            this.Tap_Login.Controls.Add(this.Txt_LoginPW);
            this.Tap_Login.Location = new Point(4, 0x22);
            this.Tap_Login.Name = "Tap_Login";
            this.Tap_Login.Padding = new Padding(3);
            this.Tap_Login.Size = new Size(0x14e, 0xb3);
            this.Tap_Login.TabIndex = 0;
            this.Tap_Login.Text = "登录使用";
            this.Ckb_LoginWebHint.Appearance = Appearance.Button;
            this.Ckb_LoginWebHint.AutoCheck = false;
            this.Ckb_LoginWebHint.FlatAppearance.BorderSize = 0;
            this.Ckb_LoginWebHint.FlatStyle = FlatStyle.Flat;
            this.Ckb_LoginWebHint.Location = new Point(0x61, 0x71);
            this.Ckb_LoginWebHint.Name = "Ckb_LoginWebHint";
            this.Ckb_LoginWebHint.Size = new Size(80, 0x19);
            this.Ckb_LoginWebHint.TabIndex = 0x120;
            this.Ckb_LoginWebHint.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_LoginWebHint.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_LoginWebHint.UseVisualStyleBackColor = true;
            this.Ckb_LoginWebHint.Visible = false;
            this.Ckb_LoginWebHint.Click += new EventHandler(this.Btn_LoginWebHint_Click);
            this.Ckb_QQ.Appearance = Appearance.Button;
            this.Ckb_QQ.AutoCheck = false;
            this.Ckb_QQ.FlatAppearance.BorderSize = 0;
            this.Ckb_QQ.FlatStyle = FlatStyle.Flat;
            this.Ckb_QQ.Image = Resources.QQ;
            this.Ckb_QQ.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_QQ.Location = new Point(11, 0x71);
            this.Ckb_QQ.Name = "Ckb_QQ";
            this.Ckb_QQ.Size = new Size(80, 0x19);
            this.Ckb_QQ.TabIndex = 0xd7;
            this.Ckb_QQ.Text = "联系客服";
            this.Ckb_QQ.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_QQ.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_QQ.UseVisualStyleBackColor = true;
            this.Ckb_QQ.Click += new EventHandler(this.Btn_QQ_Click);
            this.Ckb_Ok.Appearance = Appearance.Button;
            this.Ckb_Ok.AutoCheck = false;
            this.Ckb_Ok.FlatAppearance.BorderSize = 0;
            this.Ckb_Ok.FlatStyle = FlatStyle.Flat;
            this.Ckb_Ok.Image = Resources.User;
            this.Ckb_Ok.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Ok.Location = new Point(0x102, 0x71);
            this.Ckb_Ok.Name = "Ckb_Ok";
            this.Ckb_Ok.Size = new Size(60, 0x19);
            this.Ckb_Ok.TabIndex = 0xd6;
            this.Ckb_Ok.Text = "登录";
            this.Ckb_Ok.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Ok.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Ok.UseVisualStyleBackColor = true;
            this.Ckb_Ok.Click += new EventHandler(this.Btn_Ok_Click);
            this.Lbl_LoginHint.AutoSize = true;
            this.Lbl_LoginHint.Location = new Point(8, 150);
            this.Lbl_LoginHint.Name = "Lbl_LoginHint";
            this.Lbl_LoginHint.Size = new Size(0x2c, 0x11);
            this.Lbl_LoginHint.TabIndex = 15;
            this.Lbl_LoginHint.Text = "提示：";
            this.Lbl_LoginHint.Visible = false;
            this.Lbl_LoginID.AutoSize = true;
            this.Lbl_LoginID.Location = new Point(8, 0x2a);
            this.Lbl_LoginID.Name = "Lbl_LoginID";
            this.Lbl_LoginID.Size = new Size(0x44, 0x11);
            this.Lbl_LoginID.TabIndex = 0;
            this.Lbl_LoginID.Text = "会员账号：";
            this.Lbl_LoginPW.AutoSize = true;
            this.Lbl_LoginPW.Location = new Point(8, 0x54);
            this.Lbl_LoginPW.Name = "Lbl_LoginPW";
            this.Lbl_LoginPW.Size = new Size(0x44, 0x11);
            this.Lbl_LoginPW.TabIndex = 2;
            this.Lbl_LoginPW.Text = "会员密码：";
            this.Txt_LoginID.Location = new Point(0x52, 0x27);
            this.Txt_LoginID.Name = "Txt_LoginID";
            this.Txt_LoginID.Size = new Size(0xec, 0x17);
            this.Txt_LoginID.TabIndex = 11;
            this.Txt_LoginPW.Location = new Point(0x52, 0x51);
            this.Txt_LoginPW.Name = "Txt_LoginPW";
            this.Txt_LoginPW.PasswordChar = '*';
            this.Txt_LoginPW.Size = new Size(0xec, 0x17);
            this.Txt_LoginPW.TabIndex = 12;
            this.Tap_Register.BackColor = SystemColors.Control;
            this.Tap_Register.Controls.Add(this.Pnl_Register);
            this.Tap_Register.Controls.Add(this.Pnl_Register1);
            this.Tap_Register.Location = new Point(4, 0x22);
            this.Tap_Register.Name = "Tap_Register";
            this.Tap_Register.Padding = new Padding(3);
            this.Tap_Register.Size = new Size(0x14e, 0xb3);
            this.Tap_Register.TabIndex = 1;
            this.Tap_Register.Text = "用户注册";
            this.Pnl_Register.BackColor = Color.Transparent;
            this.Pnl_Register.Controls.Add(this.Ckb_Register);
            this.Pnl_Register.Controls.Add(this.Lbl_RegisterHint);
            this.Pnl_Register.Controls.Add(this.Lbl_RegisterID);
            this.Pnl_Register.Controls.Add(this.Txt_RegisterPW);
            this.Pnl_Register.Controls.Add(this.Lbl_RegisterPW1);
            this.Pnl_Register.Controls.Add(this.Txt_RegisterID);
            this.Pnl_Register.Controls.Add(this.Txt_RegisterPW1);
            this.Pnl_Register.Controls.Add(this.Lbl_RegisterPW);
            this.Pnl_Register.Dock = DockStyle.Fill;
            this.Pnl_Register.Location = new Point(3, 3);
            this.Pnl_Register.Name = "Pnl_Register";
            this.Pnl_Register.Size = new Size(0x148, 0xad);
            this.Pnl_Register.TabIndex = 30;
            this.Ckb_Register.Appearance = Appearance.Button;
            this.Ckb_Register.AutoCheck = false;
            this.Ckb_Register.FlatAppearance.BorderSize = 0;
            this.Ckb_Register.FlatStyle = FlatStyle.Flat;
            this.Ckb_Register.Image = (Image) manager.GetObject("Ckb_Register.Image");
            this.Ckb_Register.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Register.Location = new Point(0x102, 0x77);
            this.Ckb_Register.Name = "Ckb_Register";
            this.Ckb_Register.Size = new Size(60, 0x19);
            this.Ckb_Register.TabIndex = 0xd7;
            this.Ckb_Register.Text = "注册";
            this.Ckb_Register.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Register.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Register.UseVisualStyleBackColor = true;
            this.Ckb_Register.Click += new EventHandler(this.Btn_Register_Click);
            this.Lbl_RegisterHint.AutoSize = true;
            this.Lbl_RegisterHint.Location = new Point(8, 150);
            this.Lbl_RegisterHint.Name = "Lbl_RegisterHint";
            this.Lbl_RegisterHint.Size = new Size(0x2c, 0x11);
            this.Lbl_RegisterHint.TabIndex = 0x1c;
            this.Lbl_RegisterHint.Text = "提示：";
            this.Lbl_RegisterHint.Visible = false;
            this.Lbl_RegisterID.AutoSize = true;
            this.Lbl_RegisterID.Location = new Point(8, 30);
            this.Lbl_RegisterID.Name = "Lbl_RegisterID";
            this.Lbl_RegisterID.Size = new Size(0x44, 0x11);
            this.Lbl_RegisterID.TabIndex = 0x17;
            this.Lbl_RegisterID.Text = "会员账号：";
            this.Txt_RegisterPW.Location = new Point(0x52, 0x39);
            this.Txt_RegisterPW.Name = "Txt_RegisterPW";
            this.Txt_RegisterPW.PasswordChar = '*';
            this.Txt_RegisterPW.Size = new Size(0xec, 0x17);
            this.Txt_RegisterPW.TabIndex = 15;
            this.Lbl_RegisterPW1.AutoSize = true;
            this.Lbl_RegisterPW1.Location = new Point(8, 90);
            this.Lbl_RegisterPW1.Name = "Lbl_RegisterPW1";
            this.Lbl_RegisterPW1.Size = new Size(0x44, 0x11);
            this.Lbl_RegisterPW1.TabIndex = 0x1b;
            this.Lbl_RegisterPW1.Text = "确认密码：";
            this.Txt_RegisterID.Location = new Point(0x52, 0x1b);
            this.Txt_RegisterID.Name = "Txt_RegisterID";
            this.Txt_RegisterID.Size = new Size(0xec, 0x17);
            this.Txt_RegisterID.TabIndex = 14;
            this.Txt_RegisterPW1.Location = new Point(0x52, 0x57);
            this.Txt_RegisterPW1.Name = "Txt_RegisterPW1";
            this.Txt_RegisterPW1.PasswordChar = '*';
            this.Txt_RegisterPW1.Size = new Size(0xec, 0x17);
            this.Txt_RegisterPW1.TabIndex = 0x10;
            this.Lbl_RegisterPW.AutoSize = true;
            this.Lbl_RegisterPW.Location = new Point(8, 60);
            this.Lbl_RegisterPW.Name = "Lbl_RegisterPW";
            this.Lbl_RegisterPW.Size = new Size(0x44, 0x11);
            this.Lbl_RegisterPW.TabIndex = 0x18;
            this.Lbl_RegisterPW.Text = "会员密码：";
            this.Pnl_Register1.BackColor = Color.Transparent;
            this.Pnl_Register1.Controls.Add(this.Txt_RegisterQQ);
            this.Pnl_Register1.Controls.Add(this.Lbl_RegisterPhone);
            this.Pnl_Register1.Controls.Add(this.Txt_RegisterPhone);
            this.Pnl_Register1.Controls.Add(this.Lbl_RegisterQQ);
            this.Pnl_Register1.Controls.Add(this.Lbl_RegisterID1);
            this.Pnl_Register1.Controls.Add(this.Btn_Register1);
            this.Pnl_Register1.Controls.Add(this.Txt_RegisterPW2);
            this.Pnl_Register1.Controls.Add(this.Lbl_RegisterPW3);
            this.Pnl_Register1.Controls.Add(this.Txt_RegisterID1);
            this.Pnl_Register1.Controls.Add(this.Txt_RegisterPW3);
            this.Pnl_Register1.Controls.Add(this.Lbl_RegisterPW2);
            this.Pnl_Register1.Dock = DockStyle.Fill;
            this.Pnl_Register1.Location = new Point(3, 3);
            this.Pnl_Register1.Name = "Pnl_Register1";
            this.Pnl_Register1.Size = new Size(0x148, 0xad);
            this.Pnl_Register1.TabIndex = 0x1d;
            this.Txt_RegisterQQ.Location = new Point(0x52, 0x58);
            this.Txt_RegisterQQ.Name = "Txt_RegisterQQ";
            this.Txt_RegisterQQ.Size = new Size(0xec, 0x17);
            this.Txt_RegisterQQ.TabIndex = 0x35;
            this.Lbl_RegisterPhone.AutoSize = true;
            this.Lbl_RegisterPhone.Location = new Point(8, 0x76);
            this.Lbl_RegisterPhone.Name = "Lbl_RegisterPhone";
            this.Lbl_RegisterPhone.Size = new Size(0x44, 0x11);
            this.Lbl_RegisterPhone.TabIndex = 0x1f;
            this.Lbl_RegisterPhone.Text = "会员电话：";
            this.Txt_RegisterPhone.Location = new Point(0x52, 0x73);
            this.Txt_RegisterPhone.Name = "Txt_RegisterPhone";
            this.Txt_RegisterPhone.Size = new Size(0xec, 0x17);
            this.Txt_RegisterPhone.TabIndex = 0x36;
            this.Lbl_RegisterQQ.AutoSize = true;
            this.Lbl_RegisterQQ.Location = new Point(8, 0x5b);
            this.Lbl_RegisterQQ.Name = "Lbl_RegisterQQ";
            this.Lbl_RegisterQQ.Size = new Size(0x40, 0x11);
            this.Lbl_RegisterQQ.TabIndex = 30;
            this.Lbl_RegisterQQ.Text = "会员QQ：";
            this.Lbl_RegisterID1.AutoSize = true;
            this.Lbl_RegisterID1.Location = new Point(8, 10);
            this.Lbl_RegisterID1.Name = "Lbl_RegisterID1";
            this.Lbl_RegisterID1.Size = new Size(0x44, 0x11);
            this.Lbl_RegisterID1.TabIndex = 0x17;
            this.Lbl_RegisterID1.Text = "会员账号：";
            this.Btn_Register1.FlatAppearance.BorderSize = 0;
            this.Btn_Register1.Location = new Point(0xee, 0x90);
            this.Btn_Register1.Name = "Btn_Register1";
            this.Btn_Register1.Size = new Size(80, 0x19);
            this.Btn_Register1.TabIndex = 0x37;
            this.Btn_Register1.Text = "注册";
            this.Btn_Register1.UseVisualStyleBackColor = true;
            this.Btn_Register1.Click += new EventHandler(this.Btn_Register_Click);
            this.Txt_RegisterPW2.Location = new Point(0x52, 0x22);
            this.Txt_RegisterPW2.Name = "Txt_RegisterPW2";
            this.Txt_RegisterPW2.PasswordChar = '*';
            this.Txt_RegisterPW2.Size = new Size(0xec, 0x17);
            this.Txt_RegisterPW2.TabIndex = 0x33;
            this.Lbl_RegisterPW3.AutoSize = true;
            this.Lbl_RegisterPW3.Location = new Point(8, 0x40);
            this.Lbl_RegisterPW3.Name = "Lbl_RegisterPW3";
            this.Lbl_RegisterPW3.Size = new Size(0x44, 0x11);
            this.Lbl_RegisterPW3.TabIndex = 0x1b;
            this.Lbl_RegisterPW3.Text = "确认密码：";
            this.Txt_RegisterID1.Location = new Point(0x52, 7);
            this.Txt_RegisterID1.Name = "Txt_RegisterID1";
            this.Txt_RegisterID1.Size = new Size(0xec, 0x17);
            this.Txt_RegisterID1.TabIndex = 50;
            this.Txt_RegisterPW3.Location = new Point(0x52, 0x3d);
            this.Txt_RegisterPW3.Name = "Txt_RegisterPW3";
            this.Txt_RegisterPW3.PasswordChar = '*';
            this.Txt_RegisterPW3.Size = new Size(0xec, 0x17);
            this.Txt_RegisterPW3.TabIndex = 0x34;
            this.Lbl_RegisterPW2.AutoSize = true;
            this.Lbl_RegisterPW2.Location = new Point(8, 0x25);
            this.Lbl_RegisterPW2.Name = "Lbl_RegisterPW2";
            this.Lbl_RegisterPW2.Size = new Size(0x44, 0x11);
            this.Lbl_RegisterPW2.TabIndex = 0x18;
            this.Lbl_RegisterPW2.Text = "会员密码：";
            this.Tap_BDApp.BackColor = SystemColors.Control;
            this.Tap_BDApp.Controls.Add(this.Ckb_BD);
            this.Tap_BDApp.Controls.Add(this.Lbl_BDID);
            this.Tap_BDApp.Controls.Add(this.Lbl_BDPW);
            this.Tap_BDApp.Controls.Add(this.Txt_BDID);
            this.Tap_BDApp.Controls.Add(this.Txt_BDPW);
            this.Tap_BDApp.Location = new Point(4, 0x22);
            this.Tap_BDApp.Name = "Tap_BDApp";
            this.Tap_BDApp.Size = new Size(0x14e, 0xb3);
            this.Tap_BDApp.TabIndex = 0;
            this.Tap_BDApp.Text = "账户解绑";
            this.Ckb_BD.Appearance = Appearance.Button;
            this.Ckb_BD.AutoCheck = false;
            this.Ckb_BD.FlatAppearance.BorderSize = 0;
            this.Ckb_BD.FlatStyle = FlatStyle.Flat;
            this.Ckb_BD.Image = (Image) manager.GetObject("Ckb_BD.Image");
            this.Ckb_BD.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_BD.Location = new Point(0x102, 0x71);
            this.Ckb_BD.Name = "Ckb_BD";
            this.Ckb_BD.Size = new Size(60, 0x19);
            this.Ckb_BD.TabIndex = 0xd8;
            this.Ckb_BD.Text = "绑定";
            this.Ckb_BD.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_BD.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_BD.UseVisualStyleBackColor = true;
            this.Ckb_BD.Click += new EventHandler(this.Btn_BD_Click);
            this.Lbl_BDID.AutoSize = true;
            this.Lbl_BDID.Location = new Point(8, 0x2a);
            this.Lbl_BDID.Name = "Lbl_BDID";
            this.Lbl_BDID.Size = new Size(0x44, 0x11);
            this.Lbl_BDID.TabIndex = 0x17;
            this.Lbl_BDID.Text = "会员账号：";
            this.Lbl_BDPW.AutoSize = true;
            this.Lbl_BDPW.Location = new Point(8, 0x54);
            this.Lbl_BDPW.Name = "Lbl_BDPW";
            this.Lbl_BDPW.Size = new Size(0x44, 0x11);
            this.Lbl_BDPW.TabIndex = 0x18;
            this.Lbl_BDPW.Text = "会员密码：";
            this.Txt_BDID.Location = new Point(0x52, 0x27);
            this.Txt_BDID.Name = "Txt_BDID";
            this.Txt_BDID.Size = new Size(0xec, 0x17);
            this.Txt_BDID.TabIndex = 0x12;
            this.Txt_BDPW.Location = new Point(0x52, 0x51);
            this.Txt_BDPW.Name = "Txt_BDPW";
            this.Txt_BDPW.PasswordChar = '*';
            this.Txt_BDPW.Size = new Size(0xec, 0x17);
            this.Txt_BDPW.TabIndex = 0x13;
            this.Tap_EditPW.BackColor = SystemColors.Control;
            this.Tap_EditPW.Controls.Add(this.Ckb_EditPW);
            this.Tap_EditPW.Controls.Add(this.Lbl_EditPW2);
            this.Tap_EditPW.Controls.Add(this.Txt_EditPW2);
            this.Tap_EditPW.Controls.Add(this.Lbl_EditPW1);
            this.Tap_EditPW.Controls.Add(this.Txt_EditPW1);
            this.Tap_EditPW.Controls.Add(this.Lbl_EditID);
            this.Tap_EditPW.Controls.Add(this.Lbl_EditPW);
            this.Tap_EditPW.Controls.Add(this.Txt_EditID);
            this.Tap_EditPW.Controls.Add(this.Txt_EditPW);
            this.Tap_EditPW.Location = new Point(4, 0x22);
            this.Tap_EditPW.Name = "Tap_EditPW";
            this.Tap_EditPW.Padding = new Padding(3);
            this.Tap_EditPW.Size = new Size(0x14e, 0xb3);
            this.Tap_EditPW.TabIndex = 2;
            this.Tap_EditPW.Text = "密码修改";
            this.Ckb_EditPW.Appearance = Appearance.Button;
            this.Ckb_EditPW.AutoCheck = false;
            this.Ckb_EditPW.FlatAppearance.BorderSize = 0;
            this.Ckb_EditPW.FlatStyle = FlatStyle.Flat;
            this.Ckb_EditPW.Image = Resources.EditHot;
            this.Ckb_EditPW.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_EditPW.Location = new Point(0x102, 0x86);
            this.Ckb_EditPW.Name = "Ckb_EditPW";
            this.Ckb_EditPW.Size = new Size(60, 0x19);
            this.Ckb_EditPW.TabIndex = 0xd9;
            this.Ckb_EditPW.Text = "修改";
            this.Ckb_EditPW.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_EditPW.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_EditPW.UseVisualStyleBackColor = true;
            this.Ckb_EditPW.Click += new EventHandler(this.Btn_EditPW_Click);
            this.Lbl_EditPW2.AutoSize = true;
            this.Lbl_EditPW2.Location = new Point(8, 0x69);
            this.Lbl_EditPW2.Name = "Lbl_EditPW2";
            this.Lbl_EditPW2.Size = new Size(0x44, 0x11);
            this.Lbl_EditPW2.TabIndex = 0x25;
            this.Lbl_EditPW2.Text = "确认密码：";
            this.Txt_EditPW2.Location = new Point(0x52, 0x66);
            this.Txt_EditPW2.Name = "Txt_EditPW2";
            this.Txt_EditPW2.PasswordChar = '*';
            this.Txt_EditPW2.Size = new Size(0xec, 0x17);
            this.Txt_EditPW2.TabIndex = 0x23;
            this.Lbl_EditPW1.AutoSize = true;
            this.Lbl_EditPW1.Location = new Point(8, 0x4b);
            this.Lbl_EditPW1.Name = "Lbl_EditPW1";
            this.Lbl_EditPW1.Size = new Size(0x44, 0x11);
            this.Lbl_EditPW1.TabIndex = 0x22;
            this.Lbl_EditPW1.Text = "新 密 码 ：";
            this.Txt_EditPW1.Location = new Point(0x52, 0x48);
            this.Txt_EditPW1.Name = "Txt_EditPW1";
            this.Txt_EditPW1.PasswordChar = '*';
            this.Txt_EditPW1.Size = new Size(0xec, 0x17);
            this.Txt_EditPW1.TabIndex = 0x22;
            this.Lbl_EditID.AutoSize = true;
            this.Lbl_EditID.Location = new Point(8, 15);
            this.Lbl_EditID.Name = "Lbl_EditID";
            this.Lbl_EditID.Size = new Size(0x44, 0x11);
            this.Lbl_EditID.TabIndex = 30;
            this.Lbl_EditID.Text = "会员账号：";
            this.Lbl_EditPW.AutoSize = true;
            this.Lbl_EditPW.Location = new Point(8, 0x2d);
            this.Lbl_EditPW.Name = "Lbl_EditPW";
            this.Lbl_EditPW.Size = new Size(0x44, 0x11);
            this.Lbl_EditPW.TabIndex = 0x1f;
            this.Lbl_EditPW.Text = "原始密码：";
            this.Txt_EditID.Location = new Point(0x52, 12);
            this.Txt_EditID.Name = "Txt_EditID";
            this.Txt_EditID.Size = new Size(0xec, 0x17);
            this.Txt_EditID.TabIndex = 0x20;
            this.Txt_EditPW.Location = new Point(0x52, 0x2a);
            this.Txt_EditPW.Name = "Txt_EditPW";
            this.Txt_EditPW.PasswordChar = '*';
            this.Txt_EditPW.Size = new Size(0xec, 0x17);
            this.Txt_EditPW.TabIndex = 0x21;
            this.Tap_UserCZ.BackColor = SystemColors.Control;
            this.Tap_UserCZ.Controls.Add(this.Ckb_UserCZ);
            this.Tap_UserCZ.Controls.Add(this.Lbl_CZID);
            this.Tap_UserCZ.Controls.Add(this.Lbl_CZPW);
            this.Tap_UserCZ.Controls.Add(this.Txt_CZID);
            this.Tap_UserCZ.Controls.Add(this.Txt_CZPW);
            this.Tap_UserCZ.Location = new Point(4, 0x22);
            this.Tap_UserCZ.Name = "Tap_UserCZ";
            this.Tap_UserCZ.Padding = new Padding(3);
            this.Tap_UserCZ.Size = new Size(0x14e, 0xb3);
            this.Tap_UserCZ.TabIndex = 3;
            this.Tap_UserCZ.Text = "账户充值";
            this.Ckb_UserCZ.Appearance = Appearance.Button;
            this.Ckb_UserCZ.AutoCheck = false;
            this.Ckb_UserCZ.FlatAppearance.BorderSize = 0;
            this.Ckb_UserCZ.FlatStyle = FlatStyle.Flat;
            this.Ckb_UserCZ.Image = Resources.UserTime;
            this.Ckb_UserCZ.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_UserCZ.Location = new Point(0x102, 0x71);
            this.Ckb_UserCZ.Name = "Ckb_UserCZ";
            this.Ckb_UserCZ.Size = new Size(60, 0x19);
            this.Ckb_UserCZ.TabIndex = 0xda;
            this.Ckb_UserCZ.Text = "充值";
            this.Ckb_UserCZ.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_UserCZ.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_UserCZ.UseVisualStyleBackColor = true;
            this.Ckb_UserCZ.Click += new EventHandler(this.Btn_UserCZ_Click);
            this.Lbl_CZID.AutoSize = true;
            this.Lbl_CZID.Location = new Point(8, 0x2a);
            this.Lbl_CZID.Name = "Lbl_CZID";
            this.Lbl_CZID.Size = new Size(0x44, 0x11);
            this.Lbl_CZID.TabIndex = 0x17;
            this.Lbl_CZID.Text = "会员账号：";
            this.Lbl_CZPW.AutoSize = true;
            this.Lbl_CZPW.Location = new Point(8, 0x54);
            this.Lbl_CZPW.Name = "Lbl_CZPW";
            this.Lbl_CZPW.Size = new Size(0x44, 0x11);
            this.Lbl_CZPW.TabIndex = 0x18;
            this.Lbl_CZPW.Text = "充值密码：";
            this.Txt_CZID.Location = new Point(0x52, 0x27);
            this.Txt_CZID.Name = "Txt_CZID";
            this.Txt_CZID.Size = new Size(0xec, 0x17);
            this.Txt_CZID.TabIndex = 0x1a;
            this.Txt_CZPW.Location = new Point(0x52, 0x51);
            this.Txt_CZPW.Name = "Txt_CZPW";
            this.Txt_CZPW.PasswordChar = '*';
            this.Txt_CZPW.Size = new Size(0xec, 0x17);
            this.Txt_CZPW.TabIndex = 0x1b;
            base.AutoScaleDimensions = new SizeF(96f, 96f);
            base.AutoScaleMode = AutoScaleMode.Dpi;
            base.ClientSize = new Size(0x15a, 0xdd);
            base.Controls.Add(this.Pnl_Main);
            base.Name = "FrmUserLogin";
            this.Text = "用户登录";
            base.Load += new EventHandler(this.FrmUserLogin_Load);
            this.Pnl_Main.ResumeLayout(false);
            this.Tab_Main.ResumeLayout(false);
            this.Tap_Login.ResumeLayout(false);
            this.Tap_Login.PerformLayout();
            this.Tap_Register.ResumeLayout(false);
            this.Pnl_Register.ResumeLayout(false);
            this.Pnl_Register.PerformLayout();
            this.Pnl_Register1.ResumeLayout(false);
            this.Pnl_Register1.PerformLayout();
            this.Tap_BDApp.ResumeLayout(false);
            this.Tap_BDApp.PerformLayout();
            this.Tap_EditPW.ResumeLayout(false);
            this.Tap_EditPW.PerformLayout();
            this.Tap_UserCZ.ResumeLayout(false);
            this.Tap_UserCZ.PerformLayout();
            base.ResumeLayout(false);
        }

        public void Web_GetQQList_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                this.QQList.Clear();
                HtmlDocument document = this.Web_GetQQList.Document;
                HtmlElement elementById = document.GetElementById("list_uin");
                if ((document != null) && (elementById != null))
                {
                    for (int i = 0; i < elementById.Children.Count; i++)
                    {
                        string[] strArray = elementById.Children[i].InnerText.Trim().Split(new char[] { '(' });
                        this.QQList.Add(strArray[1].Replace(")", ""));
                    }
                }
            }
            catch
            {
            }
        }
    }
}

