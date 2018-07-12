namespace IntelligentPlanning
{
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class FrmImageUserLogin : Form
    {
        public ConfigurationStatus.SCAccountData AccountData;
        private Button Btn_BD;
        private PictureBox Btn_Close;
        private Button Btn_EditPW;
        private PictureBox Btn_Min;
        private Button Btn_Ok;
        private Button Btn_QQ;
        private Button Btn_Register;
        private Button Btn_UserCZ;
        private IContainer components = null;
        private List<Control> ControlList = null;
        private const int HTCAPTION = 2;
        private const int HTCLIENT = 1;
        private List<Control> ImageControlList = null;
        private Label Lbl_BDID;
        private Label Lbl_BDPW;
        private Label Lbl_CZID;
        private Label Lbl_CZPW;
        private Label Lbl_EditID;
        private Label Lbl_EditPW;
        private Label Lbl_EditPW1;
        private Label Lbl_EditPW2;
        private Label Lbl_LoginID;
        private Label Lbl_LoginPW;
        private Label Lbl_RegisterID;
        private Label Lbl_RegisterPW;
        private string MachineCode = "";
        private Panel Pnl_BDApp;
        private Panel Pnl_Bottom;
        private Panel Pnl_EditPW;
        private Panel Pnl_Login;
        private Panel Pnl_Register;
        private Panel Pnl_UserCZ;
        private Dictionary<string, ConfigurationStatus.PTLine> PTLineDic = new Dictionary<string, ConfigurationStatus.PTLine>();
        private List<string> QQList = new List<string>();
        private RadioButton Rdb_EditPW;
        private RadioButton Rdb_Login;
        private RadioButton Rdb_Register;
        private RadioButton Rdb_UserCZ;
        private string RegConfigPath = "";
        private List<Control> SpecialControlList = null;
        private ToolTip Tot_Hint;
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
        private TextBox Txt_RegisterPW;
        private const int WM_NCHITTEST = 0x84;
        private const int WM_NCLBUTTONDBLCLK = 0xa3;
        private Dictionary<string, string> YZMDic = new Dictionary<string, string>();

        public FrmImageUserLogin(ConfigurationStatus.SCAccountData pAccountData)
        {
            this.InitializeComponent();
            this.AccountData = pAccountData;
            List<Control> list = new List<Control> {
                this,
                this.Txt_LoginID,
                this.Txt_LoginPW
            };
            this.ControlList = list;
            this.Text = $"{"永信在线挂机软件"}-登录";
            base.Icon = AppInfo.AppIcon32;
            List<Control> list2 = new List<Control> {
                this.Btn_Min,
                this.Btn_Close,
                this.Btn_Ok,
                this.Btn_QQ,
                this.Lbl_LoginID,
                this.Lbl_LoginPW,
                this.Rdb_Login,
                this.Rdb_Register,
                this.Rdb_EditPW,
                this.Rdb_UserCZ,
                this.Lbl_RegisterID,
                this.Lbl_RegisterPW,
                this.Btn_Register,
                this.Lbl_BDID,
                this.Lbl_BDPW,
                this.Btn_BD,
                this.Lbl_EditID,
                this.Lbl_EditPW,
                this.Lbl_EditPW1,
                this.Lbl_EditPW2,
                this.Btn_EditPW,
                this.Lbl_CZID,
                this.Lbl_CZPW,
                this.Btn_UserCZ
            };
            this.ImageControlList = list2;
            AppImage.LoadImage(this.ImageControlList);
            this.LoadImageMain();
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

        private void Btn_Close_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                base.Close();
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

        private void Btn_Min_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                base.WindowState = FormWindowState.Minimized;
            }
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
            if (this.Txt_RegisterID.Text == "")
            {
                CommFunc.PublicMessageAll("输入会员账号不能为空！", true, MessageBoxIcon.Asterisk, "");
                this.Txt_RegisterID.Focus();
            }
            else if (this.Txt_RegisterID.Text.Length > 0x10)
            {
                CommFunc.PublicMessageAll($"输入会员账号的长度不能超过{0x10}位！", true, MessageBoxIcon.Asterisk, "");
                this.Txt_RegisterID.Focus();
            }
            else if (this.Txt_RegisterPW.Text == "")
            {
                CommFunc.PublicMessageAll("输入会员密码不能为空！", true, MessageBoxIcon.Asterisk, "");
                this.Txt_RegisterPW.Focus();
            }
            else if (this.Txt_RegisterPW.Text.Length > 0x10)
            {
                CommFunc.PublicMessageAll($"输入会员密码的长度不能超过{0x10}位！", true, MessageBoxIcon.Asterisk, "");
                this.Txt_RegisterPW.Focus();
            }
            else
            {
                pAccountData.ID = this.Txt_RegisterID.Text;
                pAccountData.PW = this.Txt_RegisterPW.Text;
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
                string str2 = SQLData.AddUserRow(pAccountData);
                if (str2 == "1")
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
                    if (str2.Contains("插入重复键"))
                    {
                        str3 = "该用户名已经存在！";
                        this.Txt_RegisterID.Focus();
                    }
                    CommFunc.PublicMessageAll($"注册失败，{str3}", true, MessageBoxIcon.Asterisk, "");
                }
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

        private void FrmImageUserLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            CommFunc.SaveFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SaveSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
        }

        private void FrmImageUserLogin_Load(object sender, EventArgs e)
        {
            this.RegConfigPath = @"software\TUHAOPLUS\YXZXGJ\DlgConfig\" + base.Name;
            CommFunc.SetFormUseingInfo(this.ControlList, this.RegConfigPath);
            this.MachineCode = CommFunc.GetDiskVolumeSerialNumber();
            if (AppInfo.Account.Configuration.QQ == "")
            {
                this.Btn_QQ.Visible = false;
            }
            this.Rdb_Login.Checked = true;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.Btn_Ok = new Button();
            this.Btn_Register = new Button();
            this.Lbl_RegisterID = new Label();
            this.Lbl_RegisterPW = new Label();
            this.Txt_RegisterID = new TextBox();
            this.Txt_RegisterPW = new TextBox();
            this.Lbl_BDID = new Label();
            this.Btn_BD = new Button();
            this.Lbl_BDPW = new Label();
            this.Txt_BDID = new TextBox();
            this.Txt_BDPW = new TextBox();
            this.Lbl_EditPW2 = new Label();
            this.Txt_EditPW2 = new TextBox();
            this.Btn_EditPW = new Button();
            this.Lbl_EditPW1 = new Label();
            this.Txt_EditPW1 = new TextBox();
            this.Lbl_EditID = new Label();
            this.Lbl_EditPW = new Label();
            this.Txt_EditID = new TextBox();
            this.Txt_EditPW = new TextBox();
            this.Lbl_CZID = new Label();
            this.Btn_UserCZ = new Button();
            this.Lbl_CZPW = new Label();
            this.Txt_CZID = new TextBox();
            this.Txt_CZPW = new TextBox();
            this.Lbl_LoginID = new Label();
            this.Btn_QQ = new Button();
            this.Lbl_LoginPW = new Label();
            this.Txt_LoginID = new TextBox();
            this.Txt_LoginPW = new TextBox();
            this.Tot_Hint = new ToolTip(this.components);
            this.Btn_Min = new PictureBox();
            this.Btn_Close = new PictureBox();
            this.Pnl_Login = new Panel();
            this.Rdb_Login = new RadioButton();
            this.Rdb_Register = new RadioButton();
            this.Rdb_EditPW = new RadioButton();
            this.Rdb_UserCZ = new RadioButton();
            this.Pnl_Bottom = new Panel();
            this.Pnl_Register = new Panel();
            this.Pnl_EditPW = new Panel();
            this.Pnl_BDApp = new Panel();
            this.Pnl_UserCZ = new Panel();
            ((ISupportInitialize) this.Btn_Min).BeginInit();
            ((ISupportInitialize) this.Btn_Close).BeginInit();
            this.Pnl_Login.SuspendLayout();
            this.Pnl_Bottom.SuspendLayout();
            this.Pnl_Register.SuspendLayout();
            this.Pnl_EditPW.SuspendLayout();
            this.Pnl_BDApp.SuspendLayout();
            this.Pnl_UserCZ.SuspendLayout();
            base.SuspendLayout();
            this.Btn_Ok.FlatAppearance.BorderSize = 0;
            this.Btn_Ok.Location = new Point(0x119, 130);
            this.Btn_Ok.Name = "Btn_Ok";
            this.Btn_Ok.Size = new Size(0x4f, 0x1d);
            this.Btn_Ok.TabIndex = 13;
            this.Btn_Ok.Text = "登录";
            this.Btn_Ok.UseVisualStyleBackColor = false;
            this.Btn_Ok.Click += new EventHandler(this.Btn_Ok_Click);
            this.Btn_Register.FlatAppearance.BorderSize = 0;
            this.Btn_Register.Location = new Point(0x119, 0x87);
            this.Btn_Register.Name = "Btn_Register";
            this.Btn_Register.Size = new Size(0x4f, 0x1d);
            this.Btn_Register.TabIndex = 0x11;
            this.Btn_Register.Text = "注册";
            this.Btn_Register.UseVisualStyleBackColor = true;
            this.Btn_Register.Click += new EventHandler(this.Btn_Register_Click);
            this.Lbl_RegisterID.AutoSize = true;
            this.Lbl_RegisterID.Location = new Point(50, 20);
            this.Lbl_RegisterID.Name = "Lbl_RegisterID";
            this.Lbl_RegisterID.Size = new Size(0x44, 0x11);
            this.Lbl_RegisterID.TabIndex = 0x17;
            this.Lbl_RegisterID.Text = "会员账号：";
            this.Lbl_RegisterPW.AutoSize = true;
            this.Lbl_RegisterPW.Location = new Point(50, 60);
            this.Lbl_RegisterPW.Name = "Lbl_RegisterPW";
            this.Lbl_RegisterPW.Size = new Size(0x44, 0x11);
            this.Lbl_RegisterPW.TabIndex = 0x18;
            this.Lbl_RegisterPW.Text = "会员密码：";
            this.Txt_RegisterID.Location = new Point(0x7c, 0x11);
            this.Txt_RegisterID.Name = "Txt_RegisterID";
            this.Txt_RegisterID.Size = new Size(0xec, 0x17);
            this.Txt_RegisterID.TabIndex = 14;
            this.Txt_RegisterPW.Location = new Point(0x7c, 0x39);
            this.Txt_RegisterPW.Name = "Txt_RegisterPW";
            this.Txt_RegisterPW.PasswordChar = '*';
            this.Txt_RegisterPW.Size = new Size(0xec, 0x17);
            this.Txt_RegisterPW.TabIndex = 15;
            this.Lbl_BDID.AutoSize = true;
            this.Lbl_BDID.Location = new Point(50, 40);
            this.Lbl_BDID.Name = "Lbl_BDID";
            this.Lbl_BDID.Size = new Size(0x44, 0x11);
            this.Lbl_BDID.TabIndex = 0x17;
            this.Lbl_BDID.Text = "会员账号：";
            this.Btn_BD.FlatAppearance.BorderSize = 0;
            this.Btn_BD.Location = new Point(0x119, 130);
            this.Btn_BD.Name = "Btn_BD";
            this.Btn_BD.Size = new Size(0x4f, 0x1d);
            this.Btn_BD.TabIndex = 20;
            this.Btn_BD.Text = "绑定";
            this.Btn_BD.UseVisualStyleBackColor = true;
            this.Btn_BD.Click += new EventHandler(this.Btn_BD_Click);
            this.Lbl_BDPW.AutoSize = true;
            this.Lbl_BDPW.Location = new Point(50, 90);
            this.Lbl_BDPW.Name = "Lbl_BDPW";
            this.Lbl_BDPW.Size = new Size(0x44, 0x11);
            this.Lbl_BDPW.TabIndex = 0x18;
            this.Lbl_BDPW.Text = "会员密码：";
            this.Txt_BDID.Location = new Point(0x7c, 0x25);
            this.Txt_BDID.Name = "Txt_BDID";
            this.Txt_BDID.Size = new Size(0xec, 0x17);
            this.Txt_BDID.TabIndex = 0x12;
            this.Txt_BDPW.Location = new Point(0x7c, 0x57);
            this.Txt_BDPW.Name = "Txt_BDPW";
            this.Txt_BDPW.PasswordChar = '*';
            this.Txt_BDPW.Size = new Size(0xec, 0x17);
            this.Txt_BDPW.TabIndex = 0x13;
            this.Lbl_EditPW2.AutoSize = true;
            this.Lbl_EditPW2.Location = new Point(50, 140);
            this.Lbl_EditPW2.Name = "Lbl_EditPW2";
            this.Lbl_EditPW2.Size = new Size(0x44, 0x11);
            this.Lbl_EditPW2.TabIndex = 0x25;
            this.Lbl_EditPW2.Text = "确认密码：";
            this.Txt_EditPW2.Location = new Point(0x7c, 0x89);
            this.Txt_EditPW2.Name = "Txt_EditPW2";
            this.Txt_EditPW2.PasswordChar = '*';
            this.Txt_EditPW2.Size = new Size(0xec, 0x17);
            this.Txt_EditPW2.TabIndex = 0x23;
            this.Btn_EditPW.FlatAppearance.BorderSize = 0;
            this.Btn_EditPW.Location = new Point(0x119, 0xaf);
            this.Btn_EditPW.Name = "Btn_EditPW";
            this.Btn_EditPW.Size = new Size(0x4f, 0x1d);
            this.Btn_EditPW.TabIndex = 0x24;
            this.Btn_EditPW.Text = "修改";
            this.Btn_EditPW.UseVisualStyleBackColor = true;
            this.Btn_EditPW.Click += new EventHandler(this.Btn_EditPW_Click);
            this.Lbl_EditPW1.AutoSize = true;
            this.Lbl_EditPW1.Location = new Point(50, 100);
            this.Lbl_EditPW1.Name = "Lbl_EditPW1";
            this.Lbl_EditPW1.Size = new Size(0x44, 0x11);
            this.Lbl_EditPW1.TabIndex = 0x22;
            this.Lbl_EditPW1.Text = "新 密 码 ：";
            this.Txt_EditPW1.Location = new Point(0x7c, 0x61);
            this.Txt_EditPW1.Name = "Txt_EditPW1";
            this.Txt_EditPW1.PasswordChar = '*';
            this.Txt_EditPW1.Size = new Size(0xec, 0x17);
            this.Txt_EditPW1.TabIndex = 0x22;
            this.Lbl_EditID.AutoSize = true;
            this.Lbl_EditID.Location = new Point(50, 20);
            this.Lbl_EditID.Name = "Lbl_EditID";
            this.Lbl_EditID.Size = new Size(0x44, 0x11);
            this.Lbl_EditID.TabIndex = 30;
            this.Lbl_EditID.Text = "会员账号：";
            this.Lbl_EditPW.AutoSize = true;
            this.Lbl_EditPW.Location = new Point(50, 60);
            this.Lbl_EditPW.Name = "Lbl_EditPW";
            this.Lbl_EditPW.Size = new Size(0x44, 0x11);
            this.Lbl_EditPW.TabIndex = 0x1f;
            this.Lbl_EditPW.Text = "原始密码：";
            this.Txt_EditID.Location = new Point(0x7c, 0x11);
            this.Txt_EditID.Name = "Txt_EditID";
            this.Txt_EditID.Size = new Size(0xec, 0x17);
            this.Txt_EditID.TabIndex = 0x20;
            this.Txt_EditPW.Location = new Point(0x7c, 0x39);
            this.Txt_EditPW.Name = "Txt_EditPW";
            this.Txt_EditPW.PasswordChar = '*';
            this.Txt_EditPW.Size = new Size(0xec, 0x17);
            this.Txt_EditPW.TabIndex = 0x21;
            this.Lbl_CZID.AutoSize = true;
            this.Lbl_CZID.Location = new Point(50, 40);
            this.Lbl_CZID.Name = "Lbl_CZID";
            this.Lbl_CZID.Size = new Size(0x44, 0x11);
            this.Lbl_CZID.TabIndex = 0x17;
            this.Lbl_CZID.Text = "会员账号：";
            this.Btn_UserCZ.FlatAppearance.BorderSize = 0;
            this.Btn_UserCZ.Location = new Point(0x119, 130);
            this.Btn_UserCZ.Name = "Btn_UserCZ";
            this.Btn_UserCZ.Size = new Size(0x4f, 0x1d);
            this.Btn_UserCZ.TabIndex = 0x1c;
            this.Btn_UserCZ.Text = "充值";
            this.Btn_UserCZ.UseVisualStyleBackColor = true;
            this.Btn_UserCZ.Click += new EventHandler(this.Btn_UserCZ_Click);
            this.Lbl_CZPW.AutoSize = true;
            this.Lbl_CZPW.Location = new Point(50, 90);
            this.Lbl_CZPW.Name = "Lbl_CZPW";
            this.Lbl_CZPW.Size = new Size(0x44, 0x11);
            this.Lbl_CZPW.TabIndex = 0x18;
            this.Lbl_CZPW.Text = "充值密码：";
            this.Txt_CZID.Location = new Point(0x7c, 0x25);
            this.Txt_CZID.Name = "Txt_CZID";
            this.Txt_CZID.Size = new Size(0xec, 0x17);
            this.Txt_CZID.TabIndex = 0x1a;
            this.Txt_CZPW.Location = new Point(0x7c, 0x57);
            this.Txt_CZPW.Name = "Txt_CZPW";
            this.Txt_CZPW.PasswordChar = '*';
            this.Txt_CZPW.Size = new Size(0xec, 0x17);
            this.Txt_CZPW.TabIndex = 0x1b;
            this.Lbl_LoginID.AutoSize = true;
            this.Lbl_LoginID.Location = new Point(50, 40);
            this.Lbl_LoginID.Name = "Lbl_LoginID";
            this.Lbl_LoginID.Size = new Size(0x44, 0x11);
            this.Lbl_LoginID.TabIndex = 0;
            this.Lbl_LoginID.Text = "会员账号：";
            this.Btn_QQ.FlatAppearance.BorderSize = 0;
            this.Btn_QQ.Location = new Point(0x35, 130);
            this.Btn_QQ.Name = "Btn_QQ";
            this.Btn_QQ.Size = new Size(0x4f, 0x1d);
            this.Btn_QQ.TabIndex = 14;
            this.Btn_QQ.Text = "联系客服";
            this.Btn_QQ.UseVisualStyleBackColor = true;
            this.Btn_QQ.Click += new EventHandler(this.Btn_QQ_Click);
            this.Lbl_LoginPW.AutoSize = true;
            this.Lbl_LoginPW.Location = new Point(50, 90);
            this.Lbl_LoginPW.Name = "Lbl_LoginPW";
            this.Lbl_LoginPW.Size = new Size(0x44, 0x11);
            this.Lbl_LoginPW.TabIndex = 2;
            this.Lbl_LoginPW.Text = "会员密码：";
            this.Txt_LoginID.Location = new Point(0x7c, 0x25);
            this.Txt_LoginID.Name = "Txt_LoginID";
            this.Txt_LoginID.Size = new Size(0xec, 0x17);
            this.Txt_LoginID.TabIndex = 11;
            this.Txt_LoginPW.Location = new Point(0x7c, 0x57);
            this.Txt_LoginPW.Name = "Txt_LoginPW";
            this.Txt_LoginPW.PasswordChar = '*';
            this.Txt_LoginPW.Size = new Size(0xec, 0x17);
            this.Txt_LoginPW.TabIndex = 12;
            this.Btn_Min.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Btn_Min.BackColor = Color.Transparent;
            this.Btn_Min.BackgroundImage = Resources.BtnMinImage;
            this.Btn_Min.Location = new Point(0x163, 0x15);
            this.Btn_Min.Name = "Btn_Min";
            this.Btn_Min.Size = new Size(0x1a, 0x16);
            this.Btn_Min.TabIndex = 30;
            this.Btn_Min.TabStop = false;
            this.Btn_Min.Tag = "BtnMinImage";
            this.Tot_Hint.SetToolTip(this.Btn_Min, "最小化");
            this.Btn_Min.MouseClick += new MouseEventHandler(this.Btn_Min_MouseClick);
            this.Btn_Close.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Btn_Close.BackColor = Color.Transparent;
            this.Btn_Close.BackgroundImage = Resources.BtnCloseImage;
            this.Btn_Close.Location = new Point(0x17d, 0x15);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new Size(0x1a, 0x16);
            this.Btn_Close.TabIndex = 0x1f;
            this.Btn_Close.TabStop = false;
            this.Btn_Close.Tag = "BtnCloseImage";
            this.Tot_Hint.SetToolTip(this.Btn_Close, "关闭");
            this.Btn_Close.MouseClick += new MouseEventHandler(this.Btn_Close_MouseClick);
            this.Pnl_Login.BackColor = Color.Transparent;
            this.Pnl_Login.Controls.Add(this.Lbl_LoginID);
            this.Pnl_Login.Controls.Add(this.Btn_QQ);
            this.Pnl_Login.Controls.Add(this.Lbl_LoginPW);
            this.Pnl_Login.Controls.Add(this.Txt_LoginPW);
            this.Pnl_Login.Controls.Add(this.Btn_Ok);
            this.Pnl_Login.Controls.Add(this.Txt_LoginID);
            this.Pnl_Login.Dock = DockStyle.Fill;
            this.Pnl_Login.Location = new Point(0, 0);
            this.Pnl_Login.Name = "Pnl_Login";
            this.Pnl_Login.Size = new Size(0x1a7, 230);
            this.Pnl_Login.TabIndex = 0x20;
            this.Rdb_Login.Appearance = Appearance.Button;
            this.Rdb_Login.BackColor = Color.Transparent;
            this.Rdb_Login.FlatAppearance.BorderSize = 0;
            this.Rdb_Login.FlatStyle = FlatStyle.System;
            this.Rdb_Login.Location = new Point(0x23, 0x5e);
            this.Rdb_Login.Name = "Rdb_Login";
            this.Rdb_Login.Size = new Size(0x4f, 0x1d);
            this.Rdb_Login.TabIndex = 0x21;
            this.Rdb_Login.Text = "登录使用";
            this.Rdb_Login.TextAlign = ContentAlignment.MiddleCenter;
            this.Rdb_Login.UseVisualStyleBackColor = false;
            this.Rdb_Login.CheckedChanged += new EventHandler(this.Rdb_Login_CheckedChanged);
            this.Rdb_Register.Appearance = Appearance.Button;
            this.Rdb_Register.BackColor = Color.Transparent;
            this.Rdb_Register.FlatAppearance.BorderSize = 0;
            this.Rdb_Register.FlatStyle = FlatStyle.System;
            this.Rdb_Register.Location = new Point(120, 0x5e);
            this.Rdb_Register.Name = "Rdb_Register";
            this.Rdb_Register.Size = new Size(0x4f, 0x1d);
            this.Rdb_Register.TabIndex = 0x22;
            this.Rdb_Register.Text = "账户注册";
            this.Rdb_Register.TextAlign = ContentAlignment.MiddleCenter;
            this.Rdb_Register.UseVisualStyleBackColor = false;
            this.Rdb_Register.CheckedChanged += new EventHandler(this.Rdb_Login_CheckedChanged);
            this.Rdb_EditPW.Appearance = Appearance.Button;
            this.Rdb_EditPW.BackColor = Color.Transparent;
            this.Rdb_EditPW.FlatAppearance.BorderSize = 0;
            this.Rdb_EditPW.FlatStyle = FlatStyle.System;
            this.Rdb_EditPW.Location = new Point(0xcd, 0x5e);
            this.Rdb_EditPW.Name = "Rdb_EditPW";
            this.Rdb_EditPW.Size = new Size(0x4f, 0x1d);
            this.Rdb_EditPW.TabIndex = 0x23;
            this.Rdb_EditPW.Text = "密码修改";
            this.Rdb_EditPW.TextAlign = ContentAlignment.MiddleCenter;
            this.Rdb_EditPW.UseVisualStyleBackColor = false;
            this.Rdb_EditPW.CheckedChanged += new EventHandler(this.Rdb_Login_CheckedChanged);
            this.Rdb_UserCZ.Appearance = Appearance.Button;
            this.Rdb_UserCZ.BackColor = Color.Transparent;
            this.Rdb_UserCZ.FlatAppearance.BorderSize = 0;
            this.Rdb_UserCZ.FlatStyle = FlatStyle.System;
            this.Rdb_UserCZ.Location = new Point(290, 0x5e);
            this.Rdb_UserCZ.Name = "Rdb_UserCZ";
            this.Rdb_UserCZ.Size = new Size(0x4f, 0x1d);
            this.Rdb_UserCZ.TabIndex = 0x24;
            this.Rdb_UserCZ.Text = "账户充值";
            this.Rdb_UserCZ.TextAlign = ContentAlignment.MiddleCenter;
            this.Rdb_UserCZ.UseVisualStyleBackColor = false;
            this.Rdb_UserCZ.CheckedChanged += new EventHandler(this.Rdb_Login_CheckedChanged);
            this.Pnl_Bottom.BackColor = Color.Transparent;
            this.Pnl_Bottom.Controls.Add(this.Pnl_Register);
            this.Pnl_Bottom.Controls.Add(this.Pnl_Login);
            this.Pnl_Bottom.Controls.Add(this.Pnl_EditPW);
            this.Pnl_Bottom.Controls.Add(this.Pnl_BDApp);
            this.Pnl_Bottom.Controls.Add(this.Pnl_UserCZ);
            this.Pnl_Bottom.Dock = DockStyle.Bottom;
            this.Pnl_Bottom.Location = new Point(0, 130);
            this.Pnl_Bottom.Name = "Pnl_Bottom";
            this.Pnl_Bottom.Size = new Size(0x1a7, 230);
            this.Pnl_Bottom.TabIndex = 0x25;
            this.Pnl_Register.BackColor = Color.Transparent;
            this.Pnl_Register.Controls.Add(this.Btn_Register);
            this.Pnl_Register.Controls.Add(this.Lbl_RegisterID);
            this.Pnl_Register.Controls.Add(this.Txt_RegisterPW);
            this.Pnl_Register.Controls.Add(this.Txt_RegisterID);
            this.Pnl_Register.Controls.Add(this.Lbl_RegisterPW);
            this.Pnl_Register.Dock = DockStyle.Fill;
            this.Pnl_Register.Location = new Point(0, 0);
            this.Pnl_Register.Name = "Pnl_Register";
            this.Pnl_Register.Size = new Size(0x1a7, 230);
            this.Pnl_Register.TabIndex = 0x21;
            this.Pnl_EditPW.BackColor = Color.Transparent;
            this.Pnl_EditPW.Controls.Add(this.Lbl_EditPW2);
            this.Pnl_EditPW.Controls.Add(this.Lbl_EditID);
            this.Pnl_EditPW.Controls.Add(this.Txt_EditPW2);
            this.Pnl_EditPW.Controls.Add(this.Txt_EditPW);
            this.Pnl_EditPW.Controls.Add(this.Btn_EditPW);
            this.Pnl_EditPW.Controls.Add(this.Txt_EditID);
            this.Pnl_EditPW.Controls.Add(this.Lbl_EditPW1);
            this.Pnl_EditPW.Controls.Add(this.Lbl_EditPW);
            this.Pnl_EditPW.Controls.Add(this.Txt_EditPW1);
            this.Pnl_EditPW.Dock = DockStyle.Fill;
            this.Pnl_EditPW.Location = new Point(0, 0);
            this.Pnl_EditPW.Name = "Pnl_EditPW";
            this.Pnl_EditPW.Size = new Size(0x1a7, 230);
            this.Pnl_EditPW.TabIndex = 0x22;
            this.Pnl_BDApp.BackColor = Color.Transparent;
            this.Pnl_BDApp.Controls.Add(this.Lbl_BDID);
            this.Pnl_BDApp.Controls.Add(this.Btn_BD);
            this.Pnl_BDApp.Controls.Add(this.Txt_BDPW);
            this.Pnl_BDApp.Controls.Add(this.Lbl_BDPW);
            this.Pnl_BDApp.Controls.Add(this.Txt_BDID);
            this.Pnl_BDApp.Dock = DockStyle.Fill;
            this.Pnl_BDApp.Location = new Point(0, 0);
            this.Pnl_BDApp.Name = "Pnl_BDApp";
            this.Pnl_BDApp.Size = new Size(0x1a7, 230);
            this.Pnl_BDApp.TabIndex = 0x24;
            this.Pnl_UserCZ.BackColor = Color.Transparent;
            this.Pnl_UserCZ.Controls.Add(this.Lbl_CZID);
            this.Pnl_UserCZ.Controls.Add(this.Btn_UserCZ);
            this.Pnl_UserCZ.Controls.Add(this.Txt_CZPW);
            this.Pnl_UserCZ.Controls.Add(this.Lbl_CZPW);
            this.Pnl_UserCZ.Controls.Add(this.Txt_CZID);
            this.Pnl_UserCZ.Dock = DockStyle.Fill;
            this.Pnl_UserCZ.Location = new Point(0, 0);
            this.Pnl_UserCZ.Name = "Pnl_UserCZ";
            this.Pnl_UserCZ.Size = new Size(0x1a7, 230);
            this.Pnl_UserCZ.TabIndex = 0x23;
            base.AutoScaleDimensions = new SizeF(96f, 96f);
            base.AutoScaleMode = AutoScaleMode.Dpi;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            base.ClientSize = new Size(0x1a7, 360);
            base.ControlBox = false;
            base.Controls.Add(this.Rdb_Login);
            base.Controls.Add(this.Rdb_UserCZ);
            base.Controls.Add(this.Pnl_Bottom);
            base.Controls.Add(this.Btn_Close);
            base.Controls.Add(this.Rdb_Register);
            base.Controls.Add(this.Btn_Min);
            base.Controls.Add(this.Rdb_EditPW);
            this.DoubleBuffered = true;
            this.Font = new Font("微软雅黑", 9f);
            base.FormBorderStyle = FormBorderStyle.None;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FrmImageUserLogin";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "用户登录";
            base.TransparencyKey = SystemColors.Control;
            base.FormClosing += new FormClosingEventHandler(this.FrmImageUserLogin_FormClosing);
            base.Load += new EventHandler(this.FrmImageUserLogin_Load);
            ((ISupportInitialize) this.Btn_Min).EndInit();
            ((ISupportInitialize) this.Btn_Close).EndInit();
            this.Pnl_Login.ResumeLayout(false);
            this.Pnl_Login.PerformLayout();
            this.Pnl_Bottom.ResumeLayout(false);
            this.Pnl_Register.ResumeLayout(false);
            this.Pnl_Register.PerformLayout();
            this.Pnl_EditPW.ResumeLayout(false);
            this.Pnl_EditPW.PerformLayout();
            this.Pnl_BDApp.ResumeLayout(false);
            this.Pnl_BDApp.PerformLayout();
            this.Pnl_UserCZ.ResumeLayout(false);
            this.Pnl_UserCZ.PerformLayout();
            base.ResumeLayout(false);
        }

        private void LoadImageMain()
        {
        }

        private void Rdb_Login_CheckedChanged(object sender, EventArgs e)
        {
            this.Pnl_Login.Visible = this.Rdb_Login.Checked;
            this.Pnl_Register.Visible = this.Rdb_Register.Checked;
            this.Pnl_BDApp.Visible = false;
            this.Pnl_EditPW.Visible = this.Rdb_EditPW.Checked;
            this.Pnl_UserCZ.Visible = this.Rdb_UserCZ.Checked;
        }

        private void Txt_Input_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar == '\b') || char.IsDigit(e.KeyChar)))
            {
                e.Handled = true;
            }
            else
            {
                TextBox box = sender as TextBox;
                if ((box.Text.Trim().Length >= 20) && (e.KeyChar != '\b'))
                {
                    e.Handled = true;
                }
            }
        }

        private void Txt_Input1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar == '\b') || char.IsDigit(e.KeyChar)))
            {
                e.Handled = true;
            }
            else
            {
                TextBox box = sender as TextBox;
                if ((box.Text.Trim().Length >= 11) && (e.KeyChar != '\b'))
                {
                    e.Handled = true;
                }
            }
        }

        private void Txt_Input2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar == '\b') || char.IsDigit(e.KeyChar)))
            {
                e.Handled = true;
            }
            else
            {
                TextBox box = sender as TextBox;
                if ((box.Text.Trim().Length >= 4) && (e.KeyChar != '\b'))
                {
                    e.Handled = true;
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if (((int) m.Result) == 1)
                    {
                        m.Result = (IntPtr) 2;
                    }
                    break;

                case 0xa3:
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }
    }
}

