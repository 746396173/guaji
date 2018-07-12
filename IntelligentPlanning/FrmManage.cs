namespace IntelligentPlanning
{
    using IntelligentPlanning.CustomControls;
    using IntelligentPlanning.ExDataGridView;
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class FrmManage : ExForm
    {
        private Button Btn_AddDLUser;
        private Button Btn_ClearSelectDS;
        private Button Btn_Close;
        private Button Btn_CZTime;
        private Button Btn_DeleteDLUser;
        private Button Btn_DeleteUser;
        private Button Btn_DK;
        private Button Btn_DLGGReset;
        private Button Btn_DLGGYL;
        private Button Btn_FilterFNEncrypt;
        private Button Btn_JBUser;
        private Button Btn_JCStop;
        private Button Btn_JCStopDL;
        private Button Btn_Ok;
        private Button Btn_PTUser;
        private Button Btn_RegisterPW;
        private Button Btn_Remark;
        private Button Btn_ResetFNEncrypt;
        private Button Btn_Stop;
        private Button Btn_StopDL;
        private ComboBox Cbb_AppName;
        private ComboBox Cbb_DLName;
        private ComboBox Cbb_PTAudit;
        private ComboBox Cbb_Sort;
        private ComboBox Cbb_Time;
        private CheckBox Ckb_UserID;
        private IContainer components = null;
        private List<ConfigurationStatus.SCAccountData> DLUserList = new List<ConfigurationStatus.SCAccountData>();
        private List<ConfigurationStatus.SCAccountData> DLUserViewList = new List<ConfigurationStatus.SCAccountData>();
        private ExpandGirdView Egv_DLUserList;
        private ExpandGirdView Egv_DLUserListEgv_UserList;
        private ExpandGirdView Egv_UserList;
        private string GGImageString = "";
        private Label Lbl_AppName;
        private Label Lbl_CountKey;
        private Label Lbl_CountValue;
        private Label Lbl_DKHint;
        private Label Lbl_DLGGSizeKey;
        private Label Lbl_DLGGSizeValue;
        private Label Lbl_DLName;
        private Label Lbl_FilterFNEncrypt;
        private Label Lbl_PTAudit;
        private Button Lbl_PTEdit;
        private Label Lbl_PTHint;
        private Label Lbl_PTUser;
        private Label Lbl_RegisterID;
        private Label Lbl_RegisterPW;
        private Label Lbl_Remark;
        private Label Lbl_Sort;
        private Label Lbl_Time;
        private Label Lbl_UserID;
        private NumericUpDown Nm_DK;
        private PictureBox Pic_DLGG;
        private Panel Pnl_AppName;
        private Panel Pnl_AppName1;
        private Panel Pnl_AppName2;
        private Panel Pnl_AppName3;
        private Panel Pnl_DL;
        private Panel Pnl_DLImage;
        private Panel Pnl_DLMain;
        private Panel Pnl_DLTop;
        private Panel Pnl_DLUserBottom;
        private Panel Pnl_DLUserLeft;
        private Panel Pnl_FNEncrypt;
        private Panel Pnl_FNEncryptMain;
        private Panel Pnl_FNEncryptTop;
        private Panel Pnl_PTUserBottom;
        private Panel Pnl_PTUserMain;
        private Panel Pnl_User;
        private Panel Pnl_UserBottom;
        private Panel Pnl_UserLeft1;
        private Panel Pnl_UserLeft2;
        private Panel Pnl_UserLeft3;
        private Panel Pnl_UserTop;
        private Dictionary<string, ConfigurationStatus.PTLine> PTLineDic = new Dictionary<string, ConfigurationStatus.PTLine>();
        private TabControl Tab_Main;
        private TabPage Tap_DL;
        private TabPage Tap_FNEncrypt;
        private TabPage Tap_User;
        private TextboxLable Tbl_DLFNEdit;
        private TextboxLable Tbl_DLID;
        private TextboxLable Tbl_DLImageLink;
        private TextboxLable Tbl_DLLoginPT;
        private TextboxLable Tbl_DLPW;
        private TextboxLable Tbl_DLQQ;
        private TextboxLable Tbl_DLQQGroup;
        private TextboxLable Tbl_DLWebUrl;
        private TextBox Txt_FNEncrypt;
        private TextBox Txt_PTHint;
        private TextBox Txt_PTUser;
        private TextBox Txt_RegisterID;
        private TextBox Txt_RegisterPW;
        private TextBox Txt_Remark;
        private TextBox Txt_UserID;
        private List<ConfigurationStatus.SCAccountData> UserList = new List<ConfigurationStatus.SCAccountData>();
        private List<ConfigurationStatus.SCAccountData> UserViewList = new List<ConfigurationStatus.SCAccountData>();

        public FrmManage()
        {
            this.InitializeComponent();
            List<Control> list = new List<Control> {
                this,
                this.Txt_UserID,
                this.Nm_DK,
                this.Txt_PTHint
            };
            base.ControlList = list;
            List<Control> list2 = new List<Control> {
                this.Cbb_PTAudit
            };
            base.SpecialControlList = list2;
            List<CheckBox> list3 = new List<CheckBox> {
                this.Ckb_UserID
            };
            base.CheckBoxList = list3;
            List<Label> list4 = new List<Label> {
                this.Lbl_DLGGSizeValue
            };
            base.LabelList = list4;
            this.Cbb_Time.SelectedIndex = 3;
            this.Lbl_CountValue.ForeColor = AppInfo.appForeColor;
        }

        private void AddDLMain()
        {
            ConfigurationStatus.SCAccountData pAccountData = new ConfigurationStatus.SCAccountData {
                AppName = AppInfo.Account.AppName,
                ActiveTime = DateTime.Now.ToString("yyyy-MM-dd")
            };
            FrmDLDataInput input = new FrmDLDataInput(pAccountData, this.PTLineDic, true);
            if (input.ShowDialog() == DialogResult.OK)
            {
                this.Btn_Ok_Click(null, null);
            }
        }

        private void Btn_AddDLUser_Click(object sender, EventArgs e)
        {
            this.AddDLMain();
        }

        private void Btn_ClearSelectDS_Click(object sender, EventArgs e)
        {
            if (CommFunc.AgreeMessage("是否清除选择待收款？", false, MessageBoxIcon.Asterisk, ""))
            {
                string pUsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                foreach (DataGridViewRow row in this.CurrentGirdView.SelectedRows)
                {
                    int index = row.Index;
                    ConfigurationStatus.SCAccountData data = this.CurrentViewList[index];
                    if (!data.IsCZYS)
                    {
                        SQLData.DeleteCZState(data.ID, data.AppName, pUsedTime);
                    }
                }
                this.Btn_Ok_Click(null, null);
            }
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void Btn_CZTime_Click(object sender, EventArgs e)
        {
            ConfigurationStatus.SCAccountData user = this.GetUser("");
            this.CZMain(user, true);
        }

        private void Btn_DeleteUser_Click(object sender, EventArgs e)
        {
            ConfigurationStatus.SCAccountData user = this.GetUser("");
            if ((user != null) && CommFunc.AgreeMessage($"是否删除【{user.ID}】？", false, MessageBoxIcon.Asterisk, ""))
            {
                bool flag = false;
                if (this.CheckIsUser())
                {
                    flag = SQLData.DeleteUserRow(user, AppInfo.Account);
                }
                else
                {
                    flag = SQLData.DeleteDLUserRow(user, AppInfo.Account);
                }
                if (flag)
                {
                    CommFunc.PublicMessageAll("删除成功！", true, MessageBoxIcon.Asterisk, "");
                    this.Btn_Ok_Click(null, null);
                }
                else
                {
                    CommFunc.PublicMessageAll("删除失败！", true, MessageBoxIcon.Asterisk, "");
                }
            }
        }

        private void Btn_DK_Click(object sender, EventArgs e)
        {
            this.Btn_DK.Enabled = false;
            int num = Convert.ToInt32(this.Nm_DK.Value);
            List<string> pList = new List<string>();
            for (int i = 0; i < num; i++)
            {
                string item = this.CreatDKMain();
                if (item != "")
                {
                    pList.Add(item);
                }
                Thread.Sleep(0x3e8);
                Application.DoEvents();
            }
            string pInput = CommFunc.Join(pList, "\r\n");
            string pTitleId = $"成功生成【{pList.Count}】张点卡";
            new FrmInput(pInput, pTitleId).ShowDialog();
            this.Btn_DK.Enabled = true;
        }

        private void Btn_DLGGReset_Click(object sender, EventArgs e)
        {
            this.Pic_DLGG.Image = AppInfo.Account.GGImage;
            this.GGImageString = AppInfo.Account.GGImageString;
        }

        private void Btn_DLGGYL_Click(object sender, EventArgs e)
        {
            string pFileName = "";
            if (CommFunc.GetFileNameFromOpen(ref pFileName, "图片(*.jpg;*.bmp;*png;*gif)|*.jpeg;*.jpg;*.bmp;*.png;*.gif"))
            {
                byte[] buffer = File.ReadAllBytes(pFileName);
                this.Pic_DLGG.Image = Image.FromStream(new MemoryStream(buffer));
                this.GGImageString = Convert.ToBase64String(buffer);
            }
        }

        private void Btn_FilterFNEncrypt_Click(object sender, EventArgs e)
        {
            this.FilterFNEncrypt(false);
        }

        private void Btn_JBUser_Click(object sender, EventArgs e)
        {
            ConfigurationStatus.SCAccountData user = this.GetUser("");
            if ((user != null) && CommFunc.AgreeMessage($"是否解绑用户【{user.ID}】？", false, MessageBoxIcon.Asterisk, ""))
            {
                if (SQLData.UpdateMachineCode(user.ID, user.PW, user.AppName, ""))
                {
                    CommFunc.PublicMessageAll("解绑成功！", true, MessageBoxIcon.Asterisk, "");
                    this.Btn_Ok_Click(null, null);
                }
                else
                {
                    CommFunc.PublicMessageAll("解绑失败！", true, MessageBoxIcon.Asterisk, "");
                }
            }
        }

        private void Btn_JCStop_Click(object sender, EventArgs e)
        {
            ConfigurationStatus.SCAccountData user = this.GetUser("");
            if ((user != null) && CommFunc.AgreeMessage($"是否将用户【{user.ID}】解除停用？", false, MessageBoxIcon.Asterisk, ""))
            {
                user.IsStop = false;
                bool flag = false;
                if (this.CheckIsUser())
                {
                    flag = SQLData.UpdateUserStop(user);
                }
                else
                {
                    flag = SQLData.UpdateDLUserStop(user);
                }
                if (flag)
                {
                    CommFunc.PublicMessageAll("解除停用成功！", true, MessageBoxIcon.Asterisk, "");
                    this.Btn_Ok_Click(null, null);
                }
                else
                {
                    CommFunc.PublicMessageAll("解除停用失败！", true, MessageBoxIcon.Asterisk, "");
                }
            }
        }

        private void Btn_Ok_Click(object sender, EventArgs e)
        {
            this.Btn_Ok.Enabled = false;
            if (this.Btn_Ok.Text == "查询")
            {
                this.SearchClick();
            }
            else if (this.CheckIsDLList())
            {
                this.DLSaveClick();
            }
            else
            {
                this.SaveFNEncryptData();
            }
            this.Btn_Ok.Enabled = true;
        }

        private void Btn_PTUser_Click(object sender, EventArgs e)
        {
            ConfigurationStatus.SCAccountData user = this.GetUser("");
            if (user != null)
            {
                user.PTUser = this.Txt_PTUser.Text;
                if (SQLData.UpdatePTuser(user))
                {
                    CommFunc.PublicMessageAll("绑定成功！", true, MessageBoxIcon.Asterisk, "");
                    this.Btn_Ok_Click(null, null);
                }
                else
                {
                    CommFunc.PublicMessageAll("绑定失败！", true, MessageBoxIcon.Asterisk, "");
                }
            }
        }

        private void Btn_RegisterPW_Click(object sender, EventArgs e)
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
                pAccountData.MachineCode = "";
                pAccountData.Type = ConfigurationStatus.SCAccountType.FULL;
                int cZTime = this.GetCZTime(this.Cbb_Time.SelectedIndex);
                pAccountData.ActiveTime = DateTime.Now.AddDays((double) cZTime).ToString("yyyy-MM-dd");
                pAccountData.AppName = AppInfo.Account.AppName;
                if (AppInfo.Account.Configuration.BTPTUser)
                {
                    pAccountData.PTUser = CommFunc.CreatPTUserString(pAccountData.ID, this.PTLineDic);
                }
                string pResponseText = SQLData.AddUserRow(pAccountData);
                if (CommFunc.CheckResponseText(pResponseText))
                {
                    CommFunc.PublicMessageAll("注册成功！", true, MessageBoxIcon.Asterisk, "");
                    this.CZMain(pAccountData, false);
                }
                else
                {
                    string str2 = "请联系客服人员！";
                    if (pResponseText.Contains("插入重复键"))
                    {
                        str2 = "该用户名已经存在！";
                        this.Txt_RegisterID.Focus();
                    }
                    CommFunc.PublicMessageAll($"注册失败，{str2}", true, MessageBoxIcon.Asterisk, "");
                }
            }
        }

        private void Btn_Remark_Click(object sender, EventArgs e)
        {
            ConfigurationStatus.SCAccountData user = this.GetUser("");
            if ((user != null) && CommFunc.AgreeMessage($"是否修改用户【{user.ID}】的备注？", true, MessageBoxIcon.Asterisk, ""))
            {
                user.Remark = this.Txt_Remark.Text;
                if (SQLData.UpdateUserRemark(user))
                {
                    CommFunc.PublicMessageAll("修改成功！", true, MessageBoxIcon.Asterisk, "");
                    this.Btn_Ok_Click(null, null);
                }
                else
                {
                    CommFunc.PublicMessageAll("修改失败！", true, MessageBoxIcon.Asterisk, "");
                }
            }
        }

        private void Btn_ResetFNEncrypt_Click(object sender, EventArgs e)
        {
            this.LoadFNEncryptData();
        }

        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            ConfigurationStatus.SCAccountData user = this.GetUser("");
            if ((user != null) && CommFunc.AgreeMessage($"是否将用户【{user.ID}】停用？", false, MessageBoxIcon.Asterisk, ""))
            {
                user.IsStop = true;
                bool flag = false;
                if (this.CheckIsUser())
                {
                    flag = SQLData.UpdateUserStop(user);
                }
                else
                {
                    flag = SQLData.UpdateDLUserStop(user);
                }
                if (flag)
                {
                    CommFunc.PublicMessageAll("停用成功！", true, MessageBoxIcon.Asterisk, "");
                    this.Btn_Ok_Click(null, null);
                }
                else
                {
                    CommFunc.PublicMessageAll("停用失败！", true, MessageBoxIcon.Asterisk, "");
                }
            }
        }

        private void Cbb_AppName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (base._RunEvent)
            {
                this.RefreshControl();
                this.SearchMain(false, true);
            }
        }

        private void Cbb_CZState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (base._RunEvent)
            {
                this.SearchMain(false, true);
            }
        }

        private void Cbb_Sort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (base._RunEvent)
            {
                this.SearchMain(true, true);
            }
        }

        private bool CheckIsDLList() => 
            (this.Tab_Main.SelectedTab.Text == this.Tap_DL.Text);

        private bool CheckIsUser() => 
            ((this.SelectType == ConfigurationStatus.ManageType.User) || (this.SelectType == ConfigurationStatus.ManageType.PT));

        private bool CheckIsUserList() => 
            (this.Tab_Main.SelectedTab.Text == this.Tap_User.Text);

        private void Ckb_UserID_CheckedChanged(object sender, EventArgs e)
        {
            if (base._RunEvent)
            {
                this.SearchMain(false, true);
            }
        }

        private string CreatDKMain()
        {
            ConfigurationStatus.SCAccountData user = this.GetUser("");
            List<string> pList = new List<string>();
            string item = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int cZTime = this.GetCZTime(this.Cbb_Time.SelectedIndex);
            pList.Add(item);
            pList.Add(cZTime.ToString());
            string password = CommFunc.Encode(CommFunc.Join(pList, "|"), AppInfo.Account.AppName);
            if (CommFunc.CheckResponseText(SQLData.AddCZListRow(password, item, cZTime.ToString(), AppInfo.Account.AppName, AppInfo.Account.AppName)))
            {
                return password;
            }
            return "";
        }

        private void CZMain(ConfigurationStatus.SCAccountData pInfo, bool pHint = true)
        {
            if ((pInfo != null) && (!pHint || CommFunc.AgreeMessage($"是否给用户【{pInfo.ID}】充值？", false, MessageBoxIcon.Asterisk, "")))
            {
                string password = this.CreatDKMain();
                if (password != "")
                {
                    string pUsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string localIP = CommFunc.GetLocalIP();
                    string xHintStr = SQLData.CZUser(pInfo.ID, password, pInfo.AppName, pUsedTime, localIP);
                    if (!xHintStr.Contains("【错误】-"))
                    {
                        if (pHint)
                        {
                            CommFunc.PublicMessageAll(xHintStr, true, MessageBoxIcon.Asterisk, "");
                        }
                        this.Btn_Ok_Click(null, null);
                    }
                    else
                    {
                        xHintStr = xHintStr.Replace("【错误】-", "");
                        if (pHint)
                        {
                            CommFunc.PublicMessageAll(xHintStr, true, MessageBoxIcon.Asterisk, "");
                        }
                    }
                }
                else if (pHint)
                {
                    CommFunc.PublicMessageAll("生成密码失败！", true, MessageBoxIcon.Asterisk, "");
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

        private void DLSaveClick()
        {
            AppInfo.Account.PW = this.Tbl_DLPW.Value;
            AppInfo.Account.Configuration.QQ = this.Tbl_DLQQ.Value;
            AppInfo.Account.Configuration.QQGroup = this.Tbl_DLQQGroup.Value;
            AppInfo.Account.Configuration.FNEdit = this.Tbl_DLFNEdit.Value;
            AppInfo.Account.Configuration.ImageLink = this.Tbl_DLImageLink.Value;
            AppInfo.Account.Configuration.WebUrl = this.Tbl_DLWebUrl.Value;
            AppInfo.Account.Configuration.LoginPTListViewString = this.Tbl_DLLoginPT.Value;
            AppInfo.Account.ConfigurationString = AppInfo.Account.Configuration.DLConfiguration;
            if (this.GGImageString != "")
            {
                AppInfo.Account.GGImageString = this.GGImageString;
            }
            if (SQLData.UpdataDLUserRow(AppInfo.Account))
            {
                CommFunc.PublicMessageAll("保存数据成功！", true, MessageBoxIcon.Asterisk, "");
            }
            else
            {
                CommFunc.PublicMessageAll("保存数据超时，请重新保存！", true, MessageBoxIcon.Asterisk, "");
            }
        }

        private void Egv_DLUserList_ButtonClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ConfigurationStatus.SCAccountData user = this.GetUser("");
            string pHint = "";
            if (!SQLData.GetDLUser(user.AppName, user, ref pHint))
            {
                CommFunc.PublicMessageAll("加载配置失败，请重新点击！", true, MessageBoxIcon.Asterisk, "");
            }
            else
            {
                FrmDLDataInput input = new FrmDLDataInput(user, this.PTLineDic, false);
                if (input.ShowDialog() == DialogResult.OK)
                {
                    this.Btn_Ok_Click(null, null);
                }
            }
        }

        private void Egv_DLUserList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                if ((this.Egv_DLUserList.RowCount != 0) && (this.DLUserViewList.Count != 0))
                {
                    ConfigurationStatus.SCAccountData data = this.DLUserViewList[e.RowIndex];
                    DataGridViewCell cell = this.Egv_DLUserList.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    if (e.ColumnIndex == 0)
                    {
                        e.Value = data.ID;
                    }
                    else if (e.ColumnIndex == 1)
                    {
                        e.Value = data.PW;
                    }
                    else if (e.ColumnIndex == 2)
                    {
                        e.Value = AppInfo.Account.AppViewName;
                    }
                    else if (e.ColumnIndex == 3)
                    {
                        e.Value = data.ActiveTimeString;
                    }
                    else if (e.ColumnIndex == 4)
                    {
                        e.Value = data.IsStop ? "停用" : "";
                    }
                    else if (e.ColumnIndex == 5)
                    {
                        e.Value = data.ConfigurationString;
                    }
                    Color blackColor = AppInfo.blackColor;
                    if ((this.Cbb_Sort.SelectedIndex == 4) && data.IsStop)
                    {
                        blackColor = AppInfo.redForeColor;
                    }
                    cell.Style.SelectionForeColor = cell.Style.ForeColor = blackColor;
                }
            }
            catch
            {
            }
        }

        private void Egv_DLUserList_SelectionChanged(object sender, EventArgs e)
        {
            this.RefreshCount();
        }

        private void Egv_UserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.RefreshCount();
            try
            {
                if (this.Egv_UserList.SelectedRows.Count > 0)
                {
                    ConfigurationStatus.SCAccountData user = this.GetUser("");
                    string pTUser = user.PTUser;
                    if (pTUser == "")
                    {
                        pTUser = CommFunc.CreatPTUserString(user.ID, this.PTLineDic);
                    }
                    this.Txt_PTUser.Text = pTUser;
                    this.Txt_Remark.Text = user.Remark;
                }
            }
            catch
            {
            }
        }

        private void Egv_UserList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                if ((this.Egv_UserList.RowCount != 0) && (this.UserViewList.Count != 0))
                {
                    Color blackColor;
                    int selectedIndex;
                    ConfigurationStatus.SCAccountData data = this.UserViewList[e.RowIndex];
                    DataGridViewCell cell = this.Egv_UserList.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    if (this.SelectType == ConfigurationStatus.ManageType.User)
                    {
                        if (e.ColumnIndex == 0)
                        {
                            e.Value = data.ID;
                        }
                        else if (e.ColumnIndex == 1)
                        {
                            e.Value = data.PW;
                        }
                        else if (e.ColumnIndex == 2)
                        {
                            e.Value = data.MachineCode;
                        }
                        else if (e.ColumnIndex == 3)
                        {
                            e.Value = AppInfo.Account.AppViewName;
                        }
                        else if (e.ColumnIndex == 4)
                        {
                            e.Value = data.QQ;
                        }
                        else if (e.ColumnIndex == 5)
                        {
                            e.Value = data.Phone;
                        }
                        else if (e.ColumnIndex == 6)
                        {
                            e.Value = data.Remark;
                        }
                        else if (e.ColumnIndex == 7)
                        {
                            e.Value = data.ActiveTimeString;
                        }
                        else if (e.ColumnIndex == 8)
                        {
                            e.Value = data.OnLineTime;
                        }
                        else if (e.ColumnIndex == 9)
                        {
                            e.Value = data.IsOnLineTime ? "在线" : "";
                        }
                        else if (e.ColumnIndex == 10)
                        {
                            e.Value = data.IsStop ? "停用" : "";
                        }
                        else if (e.ColumnIndex == 11)
                        {
                            e.Value = data.PTUser;
                        }
                        else if (e.ColumnIndex == 12)
                        {
                            if (data.IsCZYS)
                            {
                                e.Value = "";
                            }
                            else
                            {
                                e.Value = data.StateString;
                            }
                        }
                        blackColor = AppInfo.blackColor;
                        selectedIndex = this.Cbb_Sort.SelectedIndex;
                        if ((selectedIndex == 9) && data.IsOnLineTime)
                        {
                            blackColor = AppInfo.redForeColor;
                        }
                        if ((selectedIndex == 10) && data.IsStop)
                        {
                            blackColor = AppInfo.redForeColor;
                        }
                        if ((selectedIndex == 12) && data.IsCZDS)
                        {
                            blackColor = AppInfo.redForeColor;
                        }
                        cell.Style.SelectionForeColor = cell.Style.ForeColor = blackColor;
                    }
                    else if (this.SelectType == ConfigurationStatus.ManageType.PT)
                    {
                        if (e.ColumnIndex == 0)
                        {
                            e.Value = data.ID;
                        }
                        else if (e.ColumnIndex == 1)
                        {
                            e.Value = data.MachineCode;
                        }
                        else if (e.ColumnIndex == 2)
                        {
                            e.Value = AppInfo.Account.AppViewName;
                        }
                        else if (e.ColumnIndex == 3)
                        {
                            e.Value = data.PTName;
                        }
                        else if (e.ColumnIndex == 4)
                        {
                            e.Value = data.OnLineTime;
                        }
                        else if (e.ColumnIndex == 5)
                        {
                            e.Value = data.IsOnLineTime ? "在线" : "";
                        }
                        else if (e.ColumnIndex == 6)
                        {
                            e.Value = data.PTLoginAudit;
                        }
                        else if (e.ColumnIndex == 7)
                        {
                            e.Value = data.PTLoginHint;
                        }
                        blackColor = AppInfo.blackColor;
                        selectedIndex = this.Cbb_Sort.SelectedIndex;
                        if ((selectedIndex == 5) && data.IsOnLineTime)
                        {
                            blackColor = AppInfo.redForeColor;
                        }
                        if ((selectedIndex == 6) && (data.PTLoginAudit == "审核中..."))
                        {
                            blackColor = AppInfo.redForeColor;
                        }
                        cell.Style.SelectionForeColor = cell.Style.ForeColor = blackColor;
                    }
                }
            }
            catch
            {
            }
        }

        private void Egv_UserList_SelectionChanged(object sender, EventArgs e)
        {
            this.Egv_UserList_CellClick(null, null);
        }

        private void FilterFNEncrypt(bool pSaveFNEncrypt)
        {
            List<string> pList = CommFunc.SplitStringSkipNull(this.Txt_FNEncrypt.Text.Replace(" ", ",").Replace("，", ",").Replace("\r", ",").Replace("\n", ",").Replace("\r\n", ",").Replace(".", ",").Replace("。", ",").Replace(":", ",").Replace(";", ",").Trim(), ",");
            this.Txt_FNEncrypt.Text = CommFunc.Join(pList, ",");
            if (pSaveFNEncrypt)
            {
                AppInfo.Account.Configuration.FNEncrypIDList = CommFunc.CopyList(pList);
            }
        }

        private List<ConfigurationStatus.SCAccountData> FilterUserList(List<ConfigurationStatus.SCAccountData> pList)
        {
            List<ConfigurationStatus.SCAccountData> list = new List<ConfigurationStatus.SCAccountData>();
            string str = $"*{this.Txt_UserID.Text}*";
            bool flag = this.Ckb_UserID.Checked;
            foreach (ConfigurationStatus.SCAccountData data in pList)
            {
                if ((!flag || (str == "")) || CommFunc.VBLike(data.ID, str))
                {
                    list.Add(data);
                }
            }
            return list;
        }

        private void FrmManage_Load(object sender, EventArgs e)
        {
            if (CommFunc.CheckUpdateAppl())
            {
                CommFunc.PublicMessageAll("检测到有新版本，请下载使用！", true, MessageBoxIcon.Asterisk, "");
                CommFunc.UpdateApp(AppInfo.Account.AppPerName);
                base.Close();
            }
            else if (!this.WebLoginMain())
            {
                base.Close();
            }
            else if (!CommFunc.LoadConfiguration())
            {
                base.Close();
            }
            else
            {
                if (!AppInfo.Account.IsDL)
                {
                    this.Tap_DL.Parent = null;
                }
                else
                {
                    this.LoadDLData();
                }
                if (!AppInfo.Account.IsAdmin)
                {
                    this.Cbb_AppName.Items.RemoveAt(1);
                }
                if (AppInfo.Account.AppPerName == "XZTD")
                {
                    this.Pnl_DLImage.Visible = this.Tbl_DLImageLink.Visible = this.Tbl_DLWebUrl.Visible = false;
                }
                this.Cbb_AppName.SelectedIndex = 0;
                string webData = HttpHelper.GetWebData(AppInfo.cPTLineFile(AppInfo.Account.AppPerName), "");
                CommFunc.AnalysisPTLine(ref this.PTLineDic, webData);
                this.LoadAppName();
                this.LoadUserList();
                this.LoadDLUserList();
                this.Text = $"用户管理【{AppInfo.Account.AppViewName}】";
                this.LoadFNEncryptData();
                this.Lbl_FilterFNEncrypt.ForeColor = AppInfo.redForeColor;
                this.RefreshControl();
                CommFunc.SetForegroundWindow(base.Handle);
                base._RunEvent = true;
            }
        }

        private int GetCZTime(int pIndex)
        {
            int num = 0;
            if (pIndex == 0)
            {
                return 1;
            }
            if (pIndex == 1)
            {
                return 3;
            }
            if (pIndex == 2)
            {
                return 7;
            }
            if (pIndex == 3)
            {
                return 30;
            }
            if (pIndex == 4)
            {
                return 90;
            }
            if (pIndex == 5)
            {
                return 180;
            }
            if (pIndex == 6)
            {
                return 0x16d;
            }
            if (pIndex == 7)
            {
                num = -1;
            }
            return num;
        }

        private ConfigurationStatus.SCAccountData GetUser(string pName = "")
        {
            ConfigurationStatus.SCAccountData data = null;
            if ((this.CurrentGirdView.Rows.Count != 0) && (this.CurrentGirdView.SelectedRows.Count != 0))
            {
                int index = this.CurrentGirdView.SelectedRows[0].Index;
                data = this.CurrentViewList[index];
            }
            return data;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            DataGridViewCellStyle style3 = new DataGridViewCellStyle();
            DataGridViewCellStyle style4 = new DataGridViewCellStyle();
            DataGridViewCellStyle style5 = new DataGridViewCellStyle();
            DataGridViewCellStyle style6 = new DataGridViewCellStyle();
            this.Tab_Main = new TabControl();
            this.Tap_User = new TabPage();
            this.Pnl_User = new Panel();
            this.Egv_UserList = new ExpandGirdView(this.components);
            this.Egv_DLUserList = new ExpandGirdView(this.components);
            this.Pnl_UserBottom = new Panel();
            this.Pnl_UserLeft3 = new Panel();
            this.Btn_JCStop = new Button();
            this.Btn_JBUser = new Button();
            this.Btn_DeleteUser = new Button();
            this.Btn_Stop = new Button();
            this.Pnl_UserLeft2 = new Panel();
            this.Btn_RegisterPW = new Button();
            this.Txt_RegisterPW = new TextBox();
            this.Lbl_RegisterPW = new Label();
            this.Txt_RegisterID = new TextBox();
            this.Lbl_RegisterID = new Label();
            this.Pnl_UserLeft1 = new Panel();
            this.Btn_DK = new Button();
            this.Nm_DK = new NumericUpDown();
            this.Lbl_DKHint = new Label();
            this.Lbl_Time = new Label();
            this.Btn_CZTime = new Button();
            this.Cbb_Time = new ComboBox();
            this.Pnl_DLUserBottom = new Panel();
            this.Pnl_DLUserLeft = new Panel();
            this.Btn_JCStopDL = new Button();
            this.Btn_AddDLUser = new Button();
            this.Btn_DeleteDLUser = new Button();
            this.Btn_StopDL = new Button();
            this.Pnl_PTUserBottom = new Panel();
            this.Pnl_PTUserMain = new Panel();
            this.Lbl_PTEdit = new Button();
            this.Lbl_PTHint = new Label();
            this.Txt_PTHint = new TextBox();
            this.Lbl_PTAudit = new Label();
            this.Cbb_PTAudit = new ComboBox();
            this.Pnl_UserTop = new Panel();
            this.Lbl_DLName = new Label();
            this.Cbb_DLName = new ComboBox();
            this.Lbl_CountValue = new Label();
            this.Lbl_CountKey = new Label();
            this.Cbb_Sort = new ComboBox();
            this.Ckb_UserID = new CheckBox();
            this.Txt_UserID = new TextBox();
            this.Lbl_Sort = new Label();
            this.Lbl_UserID = new Label();
            this.Cbb_AppName = new ComboBox();
            this.Lbl_AppName = new Label();
            this.Tap_DL = new TabPage();
            this.Pnl_DL = new Panel();
            this.Pnl_DLMain = new Panel();
            this.Pnl_DLImage = new Panel();
            this.Pnl_DLTop = new Panel();
            this.Lbl_DLGGSizeValue = new Label();
            this.Lbl_DLGGSizeKey = new Label();
            this.Btn_DLGGReset = new Button();
            this.Btn_DLGGYL = new Button();
            this.Pic_DLGG = new PictureBox();
            this.Tbl_DLImageLink = new TextboxLable();
            this.Tbl_DLWebUrl = new TextboxLable();
            this.Tbl_DLFNEdit = new TextboxLable();
            this.Tbl_DLQQGroup = new TextboxLable();
            this.Tbl_DLQQ = new TextboxLable();
            this.Tbl_DLPW = new TextboxLable();
            this.Tbl_DLID = new TextboxLable();
            this.Tbl_DLLoginPT = new TextboxLable();
            this.Tap_FNEncrypt = new TabPage();
            this.Pnl_FNEncrypt = new Panel();
            this.Pnl_FNEncryptMain = new Panel();
            this.Txt_FNEncrypt = new TextBox();
            this.Pnl_FNEncryptTop = new Panel();
            this.Lbl_FilterFNEncrypt = new Label();
            this.Btn_FilterFNEncrypt = new Button();
            this.Btn_ResetFNEncrypt = new Button();
            this.Pnl_AppName = new Panel();
            this.Pnl_AppName3 = new Panel();
            this.Btn_PTUser = new Button();
            this.Lbl_PTUser = new Label();
            this.Txt_PTUser = new TextBox();
            this.Pnl_AppName2 = new Panel();
            this.Lbl_Remark = new Label();
            this.Txt_Remark = new TextBox();
            this.Btn_Remark = new Button();
            this.Pnl_AppName1 = new Panel();
            this.Btn_ClearSelectDS = new Button();
            this.Btn_Ok = new Button();
            this.Btn_Close = new Button();
            this.Tab_Main.SuspendLayout();
            this.Tap_User.SuspendLayout();
            this.Pnl_User.SuspendLayout();
            ((ISupportInitialize) this.Egv_UserList).BeginInit();
            ((ISupportInitialize) this.Egv_DLUserList).BeginInit();
            this.Pnl_UserBottom.SuspendLayout();
            this.Pnl_UserLeft3.SuspendLayout();
            this.Pnl_UserLeft2.SuspendLayout();
            this.Pnl_UserLeft1.SuspendLayout();
            this.Nm_DK.BeginInit();
            this.Pnl_DLUserBottom.SuspendLayout();
            this.Pnl_DLUserLeft.SuspendLayout();
            this.Pnl_PTUserBottom.SuspendLayout();
            this.Pnl_PTUserMain.SuspendLayout();
            this.Pnl_UserTop.SuspendLayout();
            this.Tap_DL.SuspendLayout();
            this.Pnl_DL.SuspendLayout();
            this.Pnl_DLMain.SuspendLayout();
            this.Pnl_DLImage.SuspendLayout();
            this.Pnl_DLTop.SuspendLayout();
            ((ISupportInitialize) this.Pic_DLGG).BeginInit();
            this.Tap_FNEncrypt.SuspendLayout();
            this.Pnl_FNEncrypt.SuspendLayout();
            this.Pnl_FNEncryptMain.SuspendLayout();
            this.Pnl_FNEncryptTop.SuspendLayout();
            this.Pnl_AppName.SuspendLayout();
            this.Pnl_AppName3.SuspendLayout();
            this.Pnl_AppName2.SuspendLayout();
            this.Pnl_AppName1.SuspendLayout();
            base.SuspendLayout();
            this.Tab_Main.Controls.Add(this.Tap_User);
            this.Tab_Main.Controls.Add(this.Tap_DL);
            this.Tab_Main.Controls.Add(this.Tap_FNEncrypt);
            this.Tab_Main.Dock = DockStyle.Fill;
            this.Tab_Main.ItemSize = new Size(80, 30);
            this.Tab_Main.Location = new Point(0, 0);
            this.Tab_Main.Name = "Tab_Main";
            this.Tab_Main.SelectedIndex = 0;
            this.Tab_Main.Size = new Size(0x4a0, 0x2c2);
            this.Tab_Main.SizeMode = TabSizeMode.Fixed;
            this.Tab_Main.TabIndex = 0x4e;
            this.Tab_Main.SelectedIndexChanged += new EventHandler(this.Tab_Main_SelectedIndexChanged);
            this.Tap_User.BackColor = SystemColors.Control;
            this.Tap_User.Controls.Add(this.Pnl_User);
            this.Tap_User.Location = new Point(4, 0x22);
            this.Tap_User.Name = "Tap_User";
            this.Tap_User.Padding = new Padding(3);
            this.Tap_User.Size = new Size(0x498, 0x29c);
            this.Tap_User.TabIndex = 0;
            this.Tap_User.Text = "用户管理";
            this.Pnl_User.Controls.Add(this.Egv_UserList);
            this.Pnl_User.Controls.Add(this.Egv_DLUserList);
            this.Pnl_User.Controls.Add(this.Pnl_UserBottom);
            this.Pnl_User.Controls.Add(this.Pnl_DLUserBottom);
            this.Pnl_User.Controls.Add(this.Pnl_PTUserBottom);
            this.Pnl_User.Controls.Add(this.Pnl_UserTop);
            this.Pnl_User.Dock = DockStyle.Fill;
            this.Pnl_User.Location = new Point(3, 3);
            this.Pnl_User.Name = "Pnl_User";
            this.Pnl_User.Size = new Size(0x492, 0x296);
            this.Pnl_User.TabIndex = 0x4d;
            this.Egv_UserList.AllowUserToAddRows = false;
            this.Egv_UserList.AllowUserToDeleteRows = false;
            this.Egv_UserList.AllowUserToResizeColumns = false;
            this.Egv_UserList.AllowUserToResizeRows = false;
            this.Egv_UserList.BackgroundColor = Color.White;
            this.Egv_UserList.BorderStyle = BorderStyle.Fixed3D;
            this.Egv_UserList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style.BackColor = SystemColors.Window;
            style.Font = new Font("微软雅黑", 9f);
            style.ForeColor = SystemColors.ControlText;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.False;
            this.Egv_UserList.ColumnHeadersDefaultCellStyle = style;
            this.Egv_UserList.ColumnHeadersHeight = 30;
            this.Egv_UserList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            style2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style2.BackColor = SystemColors.Control;
            style2.Font = new Font("微软雅黑", 9f);
            style2.ForeColor = SystemColors.ControlText;
            style2.SelectionBackColor = Color.SteelBlue;
            style2.SelectionForeColor = Color.White;
            style2.WrapMode = DataGridViewTriState.False;
            this.Egv_UserList.DefaultCellStyle = style2;
            this.Egv_UserList.Dock = DockStyle.Fill;
            this.Egv_UserList.DragLineColor = Color.Silver;
            this.Egv_UserList.ExternalVirtualMode = true;
            this.Egv_UserList.GridColor = Color.Silver;
            this.Egv_UserList.HeadersCheckDefult = CheckState.Checked;
            this.Egv_UserList.Location = new Point(0, 0x23);
            this.Egv_UserList.MergeColumnHeaderBackColor = SystemColors.Control;
            this.Egv_UserList.MergeColumnHeaderForeColor = Color.Black;
            this.Egv_UserList.MultiSelect = false;
            this.Egv_UserList.Name = "Egv_UserList";
            this.Egv_UserList.RowHeadersVisible = false;
            this.Egv_UserList.RowNum = 0x11;
            style3.BackColor = Color.White;
            style3.SelectionBackColor = Color.FromArgb(0xde, 0xe8, 0xfc);
            style3.SelectionForeColor = Color.Black;
            this.Egv_UserList.RowsDefaultCellStyle = style3;
            this.Egv_UserList.RowTemplate.Height = 0x17;
            this.Egv_UserList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Egv_UserList.Size = new Size(0x492, 0x20a);
            this.Egv_UserList.TabIndex = 0x4b;
            this.Egv_UserList.VirtualMode = true;
            this.Egv_UserList.CellClick += new DataGridViewCellEventHandler(this.Egv_UserList_CellClick);
            this.Egv_UserList.SelectionChanged += new EventHandler(this.Egv_UserList_SelectionChanged);
            this.Egv_DLUserList.AllowUserToAddRows = false;
            this.Egv_DLUserList.AllowUserToDeleteRows = false;
            this.Egv_DLUserList.AllowUserToResizeColumns = false;
            this.Egv_DLUserList.AllowUserToResizeRows = false;
            this.Egv_DLUserList.BackgroundColor = Color.White;
            this.Egv_DLUserList.BorderStyle = BorderStyle.Fixed3D;
            this.Egv_DLUserList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            style4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style4.BackColor = SystemColors.Window;
            style4.Font = new Font("微软雅黑", 9f);
            style4.ForeColor = SystemColors.ControlText;
            style4.SelectionBackColor = SystemColors.Highlight;
            style4.SelectionForeColor = SystemColors.HighlightText;
            style4.WrapMode = DataGridViewTriState.False;
            this.Egv_DLUserList.ColumnHeadersDefaultCellStyle = style4;
            this.Egv_DLUserList.ColumnHeadersHeight = 30;
            this.Egv_DLUserList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            style5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style5.BackColor = SystemColors.Control;
            style5.Font = new Font("微软雅黑", 9f);
            style5.ForeColor = SystemColors.ControlText;
            style5.SelectionBackColor = Color.SteelBlue;
            style5.SelectionForeColor = Color.White;
            style5.WrapMode = DataGridViewTriState.False;
            this.Egv_DLUserList.DefaultCellStyle = style5;
            this.Egv_DLUserList.Dock = DockStyle.Fill;
            this.Egv_DLUserList.DragLineColor = Color.Silver;
            this.Egv_DLUserList.ExternalVirtualMode = true;
            this.Egv_DLUserList.GridColor = Color.Silver;
            this.Egv_DLUserList.HeadersCheckDefult = CheckState.Checked;
            this.Egv_DLUserList.Location = new Point(0, 0x23);
            this.Egv_DLUserList.MergeColumnHeaderBackColor = SystemColors.Control;
            this.Egv_DLUserList.MergeColumnHeaderForeColor = Color.Black;
            this.Egv_DLUserList.MultiSelect = false;
            this.Egv_DLUserList.Name = "Egv_DLUserList";
            this.Egv_DLUserList.RowHeadersVisible = false;
            this.Egv_DLUserList.RowNum = 0x11;
            style6.BackColor = Color.White;
            style6.SelectionBackColor = Color.FromArgb(0xde, 0xe8, 0xfc);
            style6.SelectionForeColor = Color.Black;
            this.Egv_DLUserList.RowsDefaultCellStyle = style6;
            this.Egv_DLUserList.RowTemplate.Height = 0x17;
            this.Egv_DLUserList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Egv_DLUserList.Size = new Size(0x492, 0x20a);
            this.Egv_DLUserList.TabIndex = 0x4d;
            this.Egv_DLUserList.VirtualMode = true;
            this.Egv_DLUserList.SelectionChanged += new EventHandler(this.Egv_DLUserList_SelectionChanged);
            this.Pnl_UserBottom.Controls.Add(this.Pnl_UserLeft3);
            this.Pnl_UserBottom.Controls.Add(this.Pnl_UserLeft2);
            this.Pnl_UserBottom.Controls.Add(this.Pnl_UserLeft1);
            this.Pnl_UserBottom.Dock = DockStyle.Bottom;
            this.Pnl_UserBottom.Location = new Point(0, 0x22d);
            this.Pnl_UserBottom.Name = "Pnl_UserBottom";
            this.Pnl_UserBottom.Size = new Size(0x492, 0x23);
            this.Pnl_UserBottom.TabIndex = 1;
            this.Pnl_UserLeft3.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_UserLeft3.Controls.Add(this.Btn_JCStop);
            this.Pnl_UserLeft3.Controls.Add(this.Btn_JBUser);
            this.Pnl_UserLeft3.Controls.Add(this.Btn_DeleteUser);
            this.Pnl_UserLeft3.Controls.Add(this.Btn_Stop);
            this.Pnl_UserLeft3.Dock = DockStyle.Left;
            this.Pnl_UserLeft3.Location = new Point(900, 0);
            this.Pnl_UserLeft3.Name = "Pnl_UserLeft3";
            this.Pnl_UserLeft3.Size = new Size(270, 0x23);
            this.Pnl_UserLeft3.TabIndex = 0xc0;
            this.Btn_JCStop.Location = new Point(0xcc, 5);
            this.Btn_JCStop.Name = "Btn_JCStop";
            this.Btn_JCStop.Size = new Size(60, 0x19);
            this.Btn_JCStop.TabIndex = 0xc0;
            this.Btn_JCStop.Text = "解停用";
            this.Btn_JCStop.UseVisualStyleBackColor = true;
            this.Btn_JCStop.Click += new EventHandler(this.Btn_JCStop_Click);
            this.Btn_JBUser.Location = new Point(6, 5);
            this.Btn_JBUser.Name = "Btn_JBUser";
            this.Btn_JBUser.Size = new Size(60, 0x19);
            this.Btn_JBUser.TabIndex = 0xbb;
            this.Btn_JBUser.Text = "解绑";
            this.Btn_JBUser.UseVisualStyleBackColor = true;
            this.Btn_JBUser.Click += new EventHandler(this.Btn_JBUser_Click);
            this.Btn_DeleteUser.Location = new Point(0x48, 5);
            this.Btn_DeleteUser.Name = "Btn_DeleteUser";
            this.Btn_DeleteUser.Size = new Size(60, 0x19);
            this.Btn_DeleteUser.TabIndex = 0xba;
            this.Btn_DeleteUser.Text = "删除";
            this.Btn_DeleteUser.UseVisualStyleBackColor = true;
            this.Btn_DeleteUser.Click += new EventHandler(this.Btn_DeleteUser_Click);
            this.Btn_Stop.Location = new Point(0x8a, 5);
            this.Btn_Stop.Name = "Btn_Stop";
            this.Btn_Stop.Size = new Size(60, 0x19);
            this.Btn_Stop.TabIndex = 0xbf;
            this.Btn_Stop.Text = "停用";
            this.Btn_Stop.UseVisualStyleBackColor = true;
            this.Btn_Stop.Click += new EventHandler(this.Btn_Stop_Click);
            this.Pnl_UserLeft2.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_UserLeft2.Controls.Add(this.Btn_RegisterPW);
            this.Pnl_UserLeft2.Controls.Add(this.Txt_RegisterPW);
            this.Pnl_UserLeft2.Controls.Add(this.Lbl_RegisterPW);
            this.Pnl_UserLeft2.Controls.Add(this.Txt_RegisterID);
            this.Pnl_UserLeft2.Controls.Add(this.Lbl_RegisterID);
            this.Pnl_UserLeft2.Dock = DockStyle.Left;
            this.Pnl_UserLeft2.Location = new Point(0x1b3, 0);
            this.Pnl_UserLeft2.Name = "Pnl_UserLeft2";
            this.Pnl_UserLeft2.Size = new Size(0x1d1, 0x23);
            this.Pnl_UserLeft2.TabIndex = 0xc1;
            this.Btn_RegisterPW.Location = new Point(0x18e, 5);
            this.Btn_RegisterPW.Name = "Btn_RegisterPW";
            this.Btn_RegisterPW.Size = new Size(60, 0x19);
            this.Btn_RegisterPW.TabIndex = 0xc7;
            this.Btn_RegisterPW.Text = "注册";
            this.Btn_RegisterPW.UseVisualStyleBackColor = true;
            this.Btn_RegisterPW.Click += new EventHandler(this.Btn_RegisterPW_Click);
            this.Txt_RegisterPW.Location = new Point(0xfc, 6);
            this.Txt_RegisterPW.Name = "Txt_RegisterPW";
            this.Txt_RegisterPW.Size = new Size(140, 0x17);
            this.Txt_RegisterPW.TabIndex = 0xc6;
            this.Lbl_RegisterPW.AutoSize = true;
            this.Lbl_RegisterPW.Location = new Point(0xca, 9);
            this.Lbl_RegisterPW.Name = "Lbl_RegisterPW";
            this.Lbl_RegisterPW.Size = new Size(0x2c, 0x11);
            this.Lbl_RegisterPW.TabIndex = 0xc5;
            this.Lbl_RegisterPW.Text = "密码：";
            this.Txt_RegisterID.Location = new Point(0x38, 6);
            this.Txt_RegisterID.Name = "Txt_RegisterID";
            this.Txt_RegisterID.Size = new Size(140, 0x17);
            this.Txt_RegisterID.TabIndex = 0xc4;
            this.Lbl_RegisterID.AutoSize = true;
            this.Lbl_RegisterID.Location = new Point(6, 9);
            this.Lbl_RegisterID.Name = "Lbl_RegisterID";
            this.Lbl_RegisterID.Size = new Size(0x2c, 0x11);
            this.Lbl_RegisterID.TabIndex = 0xc3;
            this.Lbl_RegisterID.Text = "账号：";
            this.Pnl_UserLeft1.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_UserLeft1.Controls.Add(this.Btn_DK);
            this.Pnl_UserLeft1.Controls.Add(this.Nm_DK);
            this.Pnl_UserLeft1.Controls.Add(this.Lbl_DKHint);
            this.Pnl_UserLeft1.Controls.Add(this.Lbl_Time);
            this.Pnl_UserLeft1.Controls.Add(this.Btn_CZTime);
            this.Pnl_UserLeft1.Controls.Add(this.Cbb_Time);
            this.Pnl_UserLeft1.Dock = DockStyle.Left;
            this.Pnl_UserLeft1.Location = new Point(0, 0);
            this.Pnl_UserLeft1.Name = "Pnl_UserLeft1";
            this.Pnl_UserLeft1.Size = new Size(0x1b3, 0x23);
            this.Pnl_UserLeft1.TabIndex = 0xbf;
            this.Btn_DK.Location = new Point(0x170, 5);
            this.Btn_DK.Name = "Btn_DK";
            this.Btn_DK.Size = new Size(60, 0x19);
            this.Btn_DK.TabIndex = 0xc1;
            this.Btn_DK.Text = "创建";
            this.Btn_DK.UseVisualStyleBackColor = true;
            this.Btn_DK.Click += new EventHandler(this.Btn_DK_Click);
            this.Nm_DK.Location = new Point(0x138, 6);
            int[] bits = new int[4];
            bits[0] = 0x3e8;
            this.Nm_DK.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.Nm_DK.Minimum = new decimal(bits);
            this.Nm_DK.Name = "Nm_DK";
            this.Nm_DK.Size = new Size(50, 0x17);
            this.Nm_DK.TabIndex = 0xc3;
            bits = new int[4];
            bits[0] = 10;
            this.Nm_DK.Value = new decimal(bits);
            this.Lbl_DKHint.AutoSize = true;
            this.Lbl_DKHint.Location = new Point(0x108, 9);
            this.Lbl_DKHint.Name = "Lbl_DKHint";
            this.Lbl_DKHint.Size = new Size(0x2c, 0x11);
            this.Lbl_DKHint.TabIndex = 0xc2;
            this.Lbl_DKHint.Text = "点卡：";
            this.Lbl_Time.AutoSize = true;
            this.Lbl_Time.Location = new Point(6, 8);
            this.Lbl_Time.Name = "Lbl_Time";
            this.Lbl_Time.Size = new Size(0x44, 0x11);
            this.Lbl_Time.TabIndex = 0xbc;
            this.Lbl_Time.Text = "充值时间：";
            this.Btn_CZTime.Location = new Point(0xcb, 5);
            this.Btn_CZTime.Name = "Btn_CZTime";
            this.Btn_CZTime.Size = new Size(60, 0x19);
            this.Btn_CZTime.TabIndex = 190;
            this.Btn_CZTime.Text = "充值";
            this.Btn_CZTime.UseVisualStyleBackColor = true;
            this.Btn_CZTime.Click += new EventHandler(this.Btn_CZTime_Click);
            this.Cbb_Time.BackColor = SystemColors.Window;
            this.Cbb_Time.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_Time.FormattingEnabled = true;
            this.Cbb_Time.Items.AddRange(new object[] { "一天", "三天", "一周", "一月", "三月", "半年", "一年", "终身" });
            this.Cbb_Time.Location = new Point(0x4d, 5);
            this.Cbb_Time.Name = "Cbb_Time";
            this.Cbb_Time.Size = new Size(120, 0x19);
            this.Cbb_Time.TabIndex = 0xbd;
            this.Pnl_DLUserBottom.Controls.Add(this.Pnl_DLUserLeft);
            this.Pnl_DLUserBottom.Dock = DockStyle.Bottom;
            this.Pnl_DLUserBottom.Location = new Point(0, 0x250);
            this.Pnl_DLUserBottom.Name = "Pnl_DLUserBottom";
            this.Pnl_DLUserBottom.Size = new Size(0x492, 0x23);
            this.Pnl_DLUserBottom.TabIndex = 0x4e;
            this.Pnl_DLUserLeft.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_DLUserLeft.Controls.Add(this.Btn_JCStopDL);
            this.Pnl_DLUserLeft.Controls.Add(this.Btn_AddDLUser);
            this.Pnl_DLUserLeft.Controls.Add(this.Btn_DeleteDLUser);
            this.Pnl_DLUserLeft.Controls.Add(this.Btn_StopDL);
            this.Pnl_DLUserLeft.Dock = DockStyle.Left;
            this.Pnl_DLUserLeft.Location = new Point(0, 0);
            this.Pnl_DLUserLeft.Name = "Pnl_DLUserLeft";
            this.Pnl_DLUserLeft.Size = new Size(270, 0x23);
            this.Pnl_DLUserLeft.TabIndex = 0xc0;
            this.Btn_JCStopDL.Location = new Point(0xcc, 5);
            this.Btn_JCStopDL.Name = "Btn_JCStopDL";
            this.Btn_JCStopDL.Size = new Size(60, 0x19);
            this.Btn_JCStopDL.TabIndex = 0xc0;
            this.Btn_JCStopDL.Text = "解停用";
            this.Btn_JCStopDL.UseVisualStyleBackColor = true;
            this.Btn_JCStopDL.Click += new EventHandler(this.Btn_JCStop_Click);
            this.Btn_AddDLUser.Location = new Point(6, 5);
            this.Btn_AddDLUser.Name = "Btn_AddDLUser";
            this.Btn_AddDLUser.Size = new Size(60, 0x19);
            this.Btn_AddDLUser.TabIndex = 0xbb;
            this.Btn_AddDLUser.Text = "添加";
            this.Btn_AddDLUser.UseVisualStyleBackColor = true;
            this.Btn_AddDLUser.Click += new EventHandler(this.Btn_AddDLUser_Click);
            this.Btn_DeleteDLUser.Location = new Point(0x48, 5);
            this.Btn_DeleteDLUser.Name = "Btn_DeleteDLUser";
            this.Btn_DeleteDLUser.Size = new Size(60, 0x19);
            this.Btn_DeleteDLUser.TabIndex = 0xba;
            this.Btn_DeleteDLUser.Text = "删除";
            this.Btn_DeleteDLUser.UseVisualStyleBackColor = true;
            this.Btn_DeleteDLUser.Click += new EventHandler(this.Btn_DeleteUser_Click);
            this.Btn_StopDL.Location = new Point(0x8a, 5);
            this.Btn_StopDL.Name = "Btn_StopDL";
            this.Btn_StopDL.Size = new Size(60, 0x19);
            this.Btn_StopDL.TabIndex = 0xbf;
            this.Btn_StopDL.Text = "停用";
            this.Btn_StopDL.UseVisualStyleBackColor = true;
            this.Btn_StopDL.Click += new EventHandler(this.Btn_Stop_Click);
            this.Pnl_PTUserBottom.Controls.Add(this.Pnl_PTUserMain);
            this.Pnl_PTUserBottom.Dock = DockStyle.Bottom;
            this.Pnl_PTUserBottom.Location = new Point(0, 0x273);
            this.Pnl_PTUserBottom.Name = "Pnl_PTUserBottom";
            this.Pnl_PTUserBottom.Size = new Size(0x492, 0x23);
            this.Pnl_PTUserBottom.TabIndex = 0x4f;
            this.Pnl_PTUserMain.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_PTUserMain.Controls.Add(this.Lbl_PTEdit);
            this.Pnl_PTUserMain.Controls.Add(this.Lbl_PTHint);
            this.Pnl_PTUserMain.Controls.Add(this.Txt_PTHint);
            this.Pnl_PTUserMain.Controls.Add(this.Lbl_PTAudit);
            this.Pnl_PTUserMain.Controls.Add(this.Cbb_PTAudit);
            this.Pnl_PTUserMain.Dock = DockStyle.Fill;
            this.Pnl_PTUserMain.Location = new Point(0, 0);
            this.Pnl_PTUserMain.Name = "Pnl_PTUserMain";
            this.Pnl_PTUserMain.Size = new Size(0x492, 0x23);
            this.Pnl_PTUserMain.TabIndex = 0xc0;
            this.Lbl_PTEdit.Location = new Point(0x452, 5);
            this.Lbl_PTEdit.Name = "Lbl_PTEdit";
            this.Lbl_PTEdit.Size = new Size(60, 0x19);
            this.Lbl_PTEdit.TabIndex = 0xc2;
            this.Lbl_PTEdit.Text = "修改";
            this.Lbl_PTEdit.UseVisualStyleBackColor = true;
            this.Lbl_PTEdit.Click += new EventHandler(this.Lbl_PTEdit_Click);
            this.Lbl_PTHint.AutoSize = true;
            this.Lbl_PTHint.Location = new Point(0xcb, 8);
            this.Lbl_PTHint.Name = "Lbl_PTHint";
            this.Lbl_PTHint.Size = new Size(0x44, 0x11);
            this.Lbl_PTHint.TabIndex = 0xc1;
            this.Lbl_PTHint.Text = "提示信息：";
            this.Txt_PTHint.Location = new Point(0x115, 6);
            this.Txt_PTHint.Name = "Txt_PTHint";
            this.Txt_PTHint.Size = new Size(0x336, 0x17);
            this.Txt_PTHint.TabIndex = 0xc0;
            this.Lbl_PTAudit.AutoSize = true;
            this.Lbl_PTAudit.Location = new Point(6, 8);
            this.Lbl_PTAudit.Name = "Lbl_PTAudit";
            this.Lbl_PTAudit.Size = new Size(0x44, 0x11);
            this.Lbl_PTAudit.TabIndex = 190;
            this.Lbl_PTAudit.Text = "审核状态：";
            this.Cbb_PTAudit.BackColor = SystemColors.Window;
            this.Cbb_PTAudit.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_PTAudit.FormattingEnabled = true;
            this.Cbb_PTAudit.Items.AddRange(new object[] { "审核通过", "审核未过" });
            this.Cbb_PTAudit.Location = new Point(0x4d, 5);
            this.Cbb_PTAudit.Name = "Cbb_PTAudit";
            this.Cbb_PTAudit.Size = new Size(120, 0x19);
            this.Cbb_PTAudit.TabIndex = 0xbf;
            this.Pnl_UserTop.Controls.Add(this.Lbl_DLName);
            this.Pnl_UserTop.Controls.Add(this.Cbb_DLName);
            this.Pnl_UserTop.Controls.Add(this.Lbl_CountValue);
            this.Pnl_UserTop.Controls.Add(this.Lbl_CountKey);
            this.Pnl_UserTop.Controls.Add(this.Cbb_Sort);
            this.Pnl_UserTop.Controls.Add(this.Ckb_UserID);
            this.Pnl_UserTop.Controls.Add(this.Txt_UserID);
            this.Pnl_UserTop.Controls.Add(this.Lbl_Sort);
            this.Pnl_UserTop.Controls.Add(this.Lbl_UserID);
            this.Pnl_UserTop.Controls.Add(this.Cbb_AppName);
            this.Pnl_UserTop.Controls.Add(this.Lbl_AppName);
            this.Pnl_UserTop.Dock = DockStyle.Top;
            this.Pnl_UserTop.Location = new Point(0, 0);
            this.Pnl_UserTop.Name = "Pnl_UserTop";
            this.Pnl_UserTop.Size = new Size(0x492, 0x23);
            this.Pnl_UserTop.TabIndex = 0;
            this.Lbl_DLName.AutoSize = true;
            this.Lbl_DLName.Location = new Point(0x3cd, 8);
            this.Lbl_DLName.Name = "Lbl_DLName";
            this.Lbl_DLName.Size = new Size(0x44, 0x11);
            this.Lbl_DLName.TabIndex = 0xcd;
            this.Lbl_DLName.Text = "代理名称：";
            this.Cbb_DLName.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_DLName.FormattingEnabled = true;
            this.Cbb_DLName.Location = new Point(0x414, 5);
            this.Cbb_DLName.Name = "Cbb_DLName";
            this.Cbb_DLName.Size = new Size(120, 0x19);
            this.Cbb_DLName.TabIndex = 0xce;
            this.Lbl_CountValue.AutoSize = true;
            this.Lbl_CountValue.Location = new Point(0x2a5, 9);
            this.Lbl_CountValue.Name = "Lbl_CountValue";
            this.Lbl_CountValue.Size = new Size(0x1b, 0x11);
            this.Lbl_CountValue.TabIndex = 200;
            this.Lbl_CountValue.Text = "0/0";
            this.Lbl_CountKey.AutoSize = true;
            this.Lbl_CountKey.Location = new Point(0x277, 9);
            this.Lbl_CountKey.Name = "Lbl_CountKey";
            this.Lbl_CountKey.Size = new Size(0x2c, 0x11);
            this.Lbl_CountKey.TabIndex = 0xc7;
            this.Lbl_CountKey.Text = "个数：";
            this.Cbb_Sort.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_Sort.FormattingEnabled = true;
            this.Cbb_Sort.Location = new Point(0x1f9, 6);
            this.Cbb_Sort.Name = "Cbb_Sort";
            this.Cbb_Sort.Size = new Size(120, 0x19);
            this.Cbb_Sort.TabIndex = 80;
            this.Cbb_Sort.SelectedIndexChanged += new EventHandler(this.Cbb_Sort_SelectedIndexChanged);
            this.Ckb_UserID.Appearance = Appearance.Button;
            this.Ckb_UserID.FlatAppearance.BorderSize = 0;
            this.Ckb_UserID.FlatStyle = FlatStyle.Flat;
            this.Ckb_UserID.Image = Resources.Shrink;
            this.Ckb_UserID.Location = new Point(0x18e, 5);
            this.Ckb_UserID.Name = "Ckb_UserID";
            this.Ckb_UserID.Size = new Size(30, 0x19);
            this.Ckb_UserID.TabIndex = 0xc3;
            this.Ckb_UserID.UseVisualStyleBackColor = true;
            this.Ckb_UserID.CheckedChanged += new EventHandler(this.Ckb_UserID_CheckedChanged);
            this.Txt_UserID.Location = new Point(0x112, 6);
            this.Txt_UserID.Name = "Txt_UserID";
            this.Txt_UserID.Size = new Size(120, 0x17);
            this.Txt_UserID.TabIndex = 80;
            this.Txt_UserID.TextChanged += new EventHandler(this.Txt_UserID_TextChanged);
            this.Lbl_Sort.AutoSize = true;
            this.Lbl_Sort.Location = new Point(0x1b2, 9);
            this.Lbl_Sort.Name = "Lbl_Sort";
            this.Lbl_Sort.Size = new Size(0x44, 0x11);
            this.Lbl_Sort.TabIndex = 0x4f;
            this.Lbl_Sort.Text = "排序依据：";
            this.Lbl_UserID.AutoSize = true;
            this.Lbl_UserID.Location = new Point(0xcb, 8);
            this.Lbl_UserID.Name = "Lbl_UserID";
            this.Lbl_UserID.Size = new Size(0x44, 0x11);
            this.Lbl_UserID.TabIndex = 0x4f;
            this.Lbl_UserID.Text = "用户名称：";
            this.Cbb_AppName.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Cbb_AppName.FormattingEnabled = true;
            this.Cbb_AppName.Items.AddRange(new object[] { "用户", "代理" });
            this.Cbb_AppName.Location = new Point(0x4d, 5);
            this.Cbb_AppName.Name = "Cbb_AppName";
            this.Cbb_AppName.Size = new Size(120, 0x19);
            this.Cbb_AppName.TabIndex = 0x4e;
            this.Cbb_AppName.SelectedIndexChanged += new EventHandler(this.Cbb_AppName_SelectedIndexChanged);
            this.Lbl_AppName.AutoSize = true;
            this.Lbl_AppName.Location = new Point(6, 8);
            this.Lbl_AppName.Name = "Lbl_AppName";
            this.Lbl_AppName.Size = new Size(0x44, 0x11);
            this.Lbl_AppName.TabIndex = 0;
            this.Lbl_AppName.Text = "管理类型：";
            this.Tap_DL.BackColor = SystemColors.Control;
            this.Tap_DL.Controls.Add(this.Pnl_DL);
            this.Tap_DL.Location = new Point(4, 0x22);
            this.Tap_DL.Name = "Tap_DL";
            this.Tap_DL.Padding = new Padding(3);
            this.Tap_DL.Size = new Size(0x498, 0x29c);
            this.Tap_DL.TabIndex = 1;
            this.Tap_DL.Text = "代理信息";
            this.Pnl_DL.Controls.Add(this.Pnl_DLMain);
            this.Pnl_DL.Dock = DockStyle.Fill;
            this.Pnl_DL.Location = new Point(3, 3);
            this.Pnl_DL.Name = "Pnl_DL";
            this.Pnl_DL.Size = new Size(0x492, 0x296);
            this.Pnl_DL.TabIndex = 0;
            this.Pnl_DLMain.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_DLMain.Controls.Add(this.Pnl_DLImage);
            this.Pnl_DLMain.Controls.Add(this.Tbl_DLImageLink);
            this.Pnl_DLMain.Controls.Add(this.Tbl_DLWebUrl);
            this.Pnl_DLMain.Controls.Add(this.Tbl_DLFNEdit);
            this.Pnl_DLMain.Controls.Add(this.Tbl_DLQQGroup);
            this.Pnl_DLMain.Controls.Add(this.Tbl_DLQQ);
            this.Pnl_DLMain.Controls.Add(this.Tbl_DLPW);
            this.Pnl_DLMain.Controls.Add(this.Tbl_DLID);
            this.Pnl_DLMain.Controls.Add(this.Tbl_DLLoginPT);
            this.Pnl_DLMain.Dock = DockStyle.Fill;
            this.Pnl_DLMain.Location = new Point(0, 0);
            this.Pnl_DLMain.Name = "Pnl_DLMain";
            this.Pnl_DLMain.Size = new Size(0x492, 0x296);
            this.Pnl_DLMain.TabIndex = 0;
            this.Pnl_DLImage.Controls.Add(this.Pnl_DLTop);
            this.Pnl_DLImage.Controls.Add(this.Pic_DLGG);
            this.Pnl_DLImage.Dock = DockStyle.Left;
            this.Pnl_DLImage.Location = new Point(0, 0xd8);
            this.Pnl_DLImage.Name = "Pnl_DLImage";
            this.Pnl_DLImage.Size = new Size(0x18e, 0x1bc);
            this.Pnl_DLImage.TabIndex = 11;
            this.Pnl_DLTop.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_DLTop.Controls.Add(this.Lbl_DLGGSizeValue);
            this.Pnl_DLTop.Controls.Add(this.Lbl_DLGGSizeKey);
            this.Pnl_DLTop.Controls.Add(this.Btn_DLGGReset);
            this.Pnl_DLTop.Controls.Add(this.Btn_DLGGYL);
            this.Pnl_DLTop.Dock = DockStyle.Top;
            this.Pnl_DLTop.Location = new Point(0, 0x6c);
            this.Pnl_DLTop.Name = "Pnl_DLTop";
            this.Pnl_DLTop.Size = new Size(0x18e, 0x23);
            this.Pnl_DLTop.TabIndex = 11;
            this.Lbl_DLGGSizeValue.AutoSize = true;
            this.Lbl_DLGGSizeValue.Location = new Point(0xcd, 8);
            this.Lbl_DLGGSizeValue.Name = "Lbl_DLGGSizeValue";
            this.Lbl_DLGGSizeValue.Size = new Size(0x37, 0x11);
            this.Lbl_DLGGSizeValue.TabIndex = 0xbd;
            this.Lbl_DLGGSizeValue.Text = "398*108";
            this.Lbl_DLGGSizeKey.AutoSize = true;
            this.Lbl_DLGGSizeKey.Location = new Point(0x88, 8);
            this.Lbl_DLGGSizeKey.Name = "Lbl_DLGGSizeKey";
            this.Lbl_DLGGSizeKey.Size = new Size(0x44, 0x11);
            this.Lbl_DLGGSizeKey.TabIndex = 0xbc;
            this.Lbl_DLGGSizeKey.Text = "建议大小：";
            this.Btn_DLGGReset.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Btn_DLGGReset.Location = new Point(2, 3);
            this.Btn_DLGGReset.Name = "Btn_DLGGReset";
            this.Btn_DLGGReset.Size = new Size(60, 0x19);
            this.Btn_DLGGReset.TabIndex = 0xbb;
            this.Btn_DLGGReset.Text = "重置";
            this.Btn_DLGGReset.UseVisualStyleBackColor = true;
            this.Btn_DLGGReset.Click += new EventHandler(this.Btn_DLGGReset_Click);
            this.Btn_DLGGYL.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Btn_DLGGYL.Location = new Point(0x44, 3);
            this.Btn_DLGGYL.Name = "Btn_DLGGYL";
            this.Btn_DLGGYL.Size = new Size(60, 0x19);
            this.Btn_DLGGYL.TabIndex = 0xba;
            this.Btn_DLGGYL.Text = "浏览";
            this.Btn_DLGGYL.UseVisualStyleBackColor = true;
            this.Btn_DLGGYL.Click += new EventHandler(this.Btn_DLGGYL_Click);
            this.Pic_DLGG.BorderStyle = BorderStyle.FixedSingle;
            this.Pic_DLGG.Dock = DockStyle.Top;
            this.Pic_DLGG.Location = new Point(0, 0);
            this.Pic_DLGG.Name = "Pic_DLGG";
            this.Pic_DLGG.Size = new Size(0x18e, 0x6c);
            this.Pic_DLGG.SizeMode = PictureBoxSizeMode.Zoom;
            this.Pic_DLGG.TabIndex = 10;
            this.Pic_DLGG.TabStop = false;
            this.Tbl_DLImageLink.Dock = DockStyle.Top;
            this.Tbl_DLImageLink.Font = new Font("微软雅黑", 11f);
            this.Tbl_DLImageLink.Hint = "广告网址";
            this.Tbl_DLImageLink.Location = new Point(0, 0xbd);
            this.Tbl_DLImageLink.Margin = new Padding(4, 5, 4, 5);
            this.Tbl_DLImageLink.Name = "Tbl_DLImageLink";
            this.Tbl_DLImageLink.Size = new Size(0x490, 0x1b);
            this.Tbl_DLImageLink.TabIndex = 9;
            this.Tbl_DLImageLink.Tag = "";
            this.Tbl_DLImageLink.Value = "";
            this.Tbl_DLImageLink.ValueReadOnly = false;
            this.Tbl_DLWebUrl.Dock = DockStyle.Top;
            this.Tbl_DLWebUrl.Font = new Font("微软雅黑", 11f);
            this.Tbl_DLWebUrl.Hint = "右下角网址";
            this.Tbl_DLWebUrl.Location = new Point(0, 0xa2);
            this.Tbl_DLWebUrl.Margin = new Padding(4, 5, 4, 5);
            this.Tbl_DLWebUrl.Name = "Tbl_DLWebUrl";
            this.Tbl_DLWebUrl.Size = new Size(0x490, 0x1b);
            this.Tbl_DLWebUrl.TabIndex = 12;
            this.Tbl_DLWebUrl.Tag = "";
            this.Tbl_DLWebUrl.Value = "";
            this.Tbl_DLWebUrl.ValueReadOnly = false;
            this.Tbl_DLFNEdit.Dock = DockStyle.Top;
            this.Tbl_DLFNEdit.Font = new Font("微软雅黑", 11f);
            this.Tbl_DLFNEdit.Hint = "方案密码";
            this.Tbl_DLFNEdit.Location = new Point(0, 0x87);
            this.Tbl_DLFNEdit.Margin = new Padding(4, 5, 4, 5);
            this.Tbl_DLFNEdit.Name = "Tbl_DLFNEdit";
            this.Tbl_DLFNEdit.Size = new Size(0x490, 0x1b);
            this.Tbl_DLFNEdit.TabIndex = 8;
            this.Tbl_DLFNEdit.Tag = "";
            this.Tbl_DLFNEdit.Value = "";
            this.Tbl_DLFNEdit.ValueReadOnly = false;
            this.Tbl_DLQQGroup.Dock = DockStyle.Top;
            this.Tbl_DLQQGroup.Font = new Font("微软雅黑", 11f);
            this.Tbl_DLQQGroup.Hint = "QQ群";
            this.Tbl_DLQQGroup.Location = new Point(0, 0x6c);
            this.Tbl_DLQQGroup.Margin = new Padding(4, 5, 4, 5);
            this.Tbl_DLQQGroup.Name = "Tbl_DLQQGroup";
            this.Tbl_DLQQGroup.Size = new Size(0x490, 0x1b);
            this.Tbl_DLQQGroup.TabIndex = 8;
            this.Tbl_DLQQGroup.Tag = "";
            this.Tbl_DLQQGroup.Value = "";
            this.Tbl_DLQQGroup.ValueReadOnly = false;
            this.Tbl_DLQQ.Dock = DockStyle.Top;
            this.Tbl_DLQQ.Font = new Font("微软雅黑", 11f);
            this.Tbl_DLQQ.Hint = "QQ";
            this.Tbl_DLQQ.Location = new Point(0, 0x51);
            this.Tbl_DLQQ.Margin = new Padding(4, 5, 4, 5);
            this.Tbl_DLQQ.Name = "Tbl_DLQQ";
            this.Tbl_DLQQ.Size = new Size(0x490, 0x1b);
            this.Tbl_DLQQ.TabIndex = 7;
            this.Tbl_DLQQ.Tag = "";
            this.Tbl_DLQQ.Value = "";
            this.Tbl_DLQQ.ValueReadOnly = false;
            this.Tbl_DLPW.Dock = DockStyle.Top;
            this.Tbl_DLPW.Font = new Font("微软雅黑", 11f);
            this.Tbl_DLPW.Hint = "密码";
            this.Tbl_DLPW.Location = new Point(0, 0x36);
            this.Tbl_DLPW.Margin = new Padding(4, 5, 4, 5);
            this.Tbl_DLPW.Name = "Tbl_DLPW";
            this.Tbl_DLPW.Size = new Size(0x490, 0x1b);
            this.Tbl_DLPW.TabIndex = 6;
            this.Tbl_DLPW.Value = "";
            this.Tbl_DLPW.ValueReadOnly = false;
            this.Tbl_DLID.Dock = DockStyle.Top;
            this.Tbl_DLID.Font = new Font("微软雅黑", 11f);
            this.Tbl_DLID.Hint = "账号";
            this.Tbl_DLID.Location = new Point(0, 0x1b);
            this.Tbl_DLID.Margin = new Padding(4, 5, 4, 5);
            this.Tbl_DLID.Name = "Tbl_DLID";
            this.Tbl_DLID.Size = new Size(0x490, 0x1b);
            this.Tbl_DLID.TabIndex = 5;
            this.Tbl_DLID.Value = "";
            this.Tbl_DLID.ValueReadOnly = true;
            this.Tbl_DLLoginPT.Dock = DockStyle.Top;
            this.Tbl_DLLoginPT.Font = new Font("微软雅黑", 11f);
            this.Tbl_DLLoginPT.Hint = "登录平台";
            this.Tbl_DLLoginPT.Location = new Point(0, 0);
            this.Tbl_DLLoginPT.Margin = new Padding(4, 5, 4, 5);
            this.Tbl_DLLoginPT.Name = "Tbl_DLLoginPT";
            this.Tbl_DLLoginPT.Size = new Size(0x490, 0x1b);
            this.Tbl_DLLoginPT.TabIndex = 4;
            this.Tbl_DLLoginPT.Value = "";
            this.Tbl_DLLoginPT.ValueReadOnly = true;
            this.Tap_FNEncrypt.BackColor = SystemColors.Control;
            this.Tap_FNEncrypt.Controls.Add(this.Pnl_FNEncrypt);
            this.Tap_FNEncrypt.Location = new Point(4, 0x22);
            this.Tap_FNEncrypt.Name = "Tap_FNEncrypt";
            this.Tap_FNEncrypt.Padding = new Padding(3);
            this.Tap_FNEncrypt.Size = new Size(0x498, 0x29c);
            this.Tap_FNEncrypt.TabIndex = 2;
            this.Tap_FNEncrypt.Text = "方案加密";
            this.Pnl_FNEncrypt.Controls.Add(this.Pnl_FNEncryptMain);
            this.Pnl_FNEncrypt.Controls.Add(this.Pnl_FNEncryptTop);
            this.Pnl_FNEncrypt.Dock = DockStyle.Fill;
            this.Pnl_FNEncrypt.Location = new Point(3, 3);
            this.Pnl_FNEncrypt.Name = "Pnl_FNEncrypt";
            this.Pnl_FNEncrypt.Size = new Size(0x492, 0x296);
            this.Pnl_FNEncrypt.TabIndex = 0;
            this.Pnl_FNEncryptMain.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_FNEncryptMain.Controls.Add(this.Txt_FNEncrypt);
            this.Pnl_FNEncryptMain.Dock = DockStyle.Fill;
            this.Pnl_FNEncryptMain.Location = new Point(0, 0x21);
            this.Pnl_FNEncryptMain.Name = "Pnl_FNEncryptMain";
            this.Pnl_FNEncryptMain.Size = new Size(0x492, 0x275);
            this.Pnl_FNEncryptMain.TabIndex = 0xc5;
            this.Txt_FNEncrypt.Dock = DockStyle.Fill;
            this.Txt_FNEncrypt.Font = new Font("微软雅黑", 11f, FontStyle.Bold);
            this.Txt_FNEncrypt.Location = new Point(0, 0);
            this.Txt_FNEncrypt.Multiline = true;
            this.Txt_FNEncrypt.Name = "Txt_FNEncrypt";
            this.Txt_FNEncrypt.Size = new Size(0x490, 0x273);
            this.Txt_FNEncrypt.TabIndex = 0x51;
            this.Pnl_FNEncryptTop.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_FNEncryptTop.Controls.Add(this.Lbl_FilterFNEncrypt);
            this.Pnl_FNEncryptTop.Controls.Add(this.Btn_FilterFNEncrypt);
            this.Pnl_FNEncryptTop.Controls.Add(this.Btn_ResetFNEncrypt);
            this.Pnl_FNEncryptTop.Dock = DockStyle.Top;
            this.Pnl_FNEncryptTop.Location = new Point(0, 0);
            this.Pnl_FNEncryptTop.Name = "Pnl_FNEncryptTop";
            this.Pnl_FNEncryptTop.Size = new Size(0x492, 0x21);
            this.Pnl_FNEncryptTop.TabIndex = 0xc4;
            this.Lbl_FilterFNEncrypt.AutoSize = true;
            this.Lbl_FilterFNEncrypt.Location = new Point(70, 7);
            this.Lbl_FilterFNEncrypt.Name = "Lbl_FilterFNEncrypt";
            this.Lbl_FilterFNEncrypt.Size = new Size(0x248, 0x11);
            this.Lbl_FilterFNEncrypt.TabIndex = 200;
            this.Lbl_FilterFNEncrypt.Text = "提示：输入需要加密的账号后点击【转换】按钮可以看到标准的加密账号格式，确认无误后点击【保存】按钮";
            this.Btn_FilterFNEncrypt.Location = new Point(4, 3);
            this.Btn_FilterFNEncrypt.Name = "Btn_FilterFNEncrypt";
            this.Btn_FilterFNEncrypt.Size = new Size(60, 0x19);
            this.Btn_FilterFNEncrypt.TabIndex = 0xbb;
            this.Btn_FilterFNEncrypt.Text = "转换";
            this.Btn_FilterFNEncrypt.UseVisualStyleBackColor = true;
            this.Btn_FilterFNEncrypt.Click += new EventHandler(this.Btn_FilterFNEncrypt_Click);
            this.Btn_ResetFNEncrypt.Location = new Point(0x450, 3);
            this.Btn_ResetFNEncrypt.Name = "Btn_ResetFNEncrypt";
            this.Btn_ResetFNEncrypt.Size = new Size(60, 0x19);
            this.Btn_ResetFNEncrypt.TabIndex = 0xba;
            this.Btn_ResetFNEncrypt.Text = "重置";
            this.Btn_ResetFNEncrypt.UseVisualStyleBackColor = true;
            this.Btn_ResetFNEncrypt.Click += new EventHandler(this.Btn_ResetFNEncrypt_Click);
            this.Pnl_AppName.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_AppName.Controls.Add(this.Pnl_AppName3);
            this.Pnl_AppName.Controls.Add(this.Pnl_AppName2);
            this.Pnl_AppName.Controls.Add(this.Pnl_AppName1);
            this.Pnl_AppName.Controls.Add(this.Btn_Ok);
            this.Pnl_AppName.Controls.Add(this.Btn_Close);
            this.Pnl_AppName.Dock = DockStyle.Bottom;
            this.Pnl_AppName.Location = new Point(0, 0x2c2);
            this.Pnl_AppName.Name = "Pnl_AppName";
            this.Pnl_AppName.Size = new Size(0x4a0, 0x23);
            this.Pnl_AppName.TabIndex = 0x4c;
            this.Pnl_AppName3.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_AppName3.Controls.Add(this.Btn_PTUser);
            this.Pnl_AppName3.Controls.Add(this.Lbl_PTUser);
            this.Pnl_AppName3.Controls.Add(this.Txt_PTUser);
            this.Pnl_AppName3.Dock = DockStyle.Left;
            this.Pnl_AppName3.Location = new Point(400, 0);
            this.Pnl_AppName3.Name = "Pnl_AppName3";
            this.Pnl_AppName3.Size = new Size(0x284, 0x21);
            this.Pnl_AppName3.TabIndex = 0xc3;
            this.Btn_PTUser.Location = new Point(0x243, 5);
            this.Btn_PTUser.Name = "Btn_PTUser";
            this.Btn_PTUser.Size = new Size(60, 0x19);
            this.Btn_PTUser.TabIndex = 0xba;
            this.Btn_PTUser.Text = "绑定";
            this.Btn_PTUser.UseVisualStyleBackColor = true;
            this.Btn_PTUser.Click += new EventHandler(this.Btn_PTUser_Click);
            this.Lbl_PTUser.AutoSize = true;
            this.Lbl_PTUser.Location = new Point(5, 8);
            this.Lbl_PTUser.Name = "Lbl_PTUser";
            this.Lbl_PTUser.Size = new Size(0x44, 0x11);
            this.Lbl_PTUser.TabIndex = 0x52;
            this.Lbl_PTUser.Text = "平台账号：";
            this.Txt_PTUser.Location = new Point(0x4f, 6);
            this.Txt_PTUser.Name = "Txt_PTUser";
            this.Txt_PTUser.Size = new Size(0x1ee, 0x17);
            this.Txt_PTUser.TabIndex = 0x51;
            this.Pnl_AppName2.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_AppName2.Controls.Add(this.Lbl_Remark);
            this.Pnl_AppName2.Controls.Add(this.Txt_Remark);
            this.Pnl_AppName2.Controls.Add(this.Btn_Remark);
            this.Pnl_AppName2.Dock = DockStyle.Left;
            this.Pnl_AppName2.Location = new Point(0x5f, 0);
            this.Pnl_AppName2.Name = "Pnl_AppName2";
            this.Pnl_AppName2.Size = new Size(0x131, 0x21);
            this.Pnl_AppName2.TabIndex = 0xc2;
            this.Lbl_Remark.AutoSize = true;
            this.Lbl_Remark.Location = new Point(6, 8);
            this.Lbl_Remark.Name = "Lbl_Remark";
            this.Lbl_Remark.Size = new Size(0x44, 0x11);
            this.Lbl_Remark.TabIndex = 0xbf;
            this.Lbl_Remark.Text = "用户备注：";
            this.Txt_Remark.Location = new Point(0x4d, 6);
            this.Txt_Remark.Name = "Txt_Remark";
            this.Txt_Remark.Size = new Size(150, 0x17);
            this.Txt_Remark.TabIndex = 190;
            this.Btn_Remark.Location = new Point(0xe9, 5);
            this.Btn_Remark.Name = "Btn_Remark";
            this.Btn_Remark.Size = new Size(60, 0x19);
            this.Btn_Remark.TabIndex = 0xc0;
            this.Btn_Remark.Text = "修改";
            this.Btn_Remark.UseVisualStyleBackColor = true;
            this.Btn_Remark.Click += new EventHandler(this.Btn_Remark_Click);
            this.Pnl_AppName1.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_AppName1.Controls.Add(this.Btn_ClearSelectDS);
            this.Pnl_AppName1.Dock = DockStyle.Left;
            this.Pnl_AppName1.Location = new Point(0, 0);
            this.Pnl_AppName1.Name = "Pnl_AppName1";
            this.Pnl_AppName1.Size = new Size(0x5f, 0x21);
            this.Pnl_AppName1.TabIndex = 0xc1;
            this.Btn_ClearSelectDS.Location = new Point(6, 5);
            this.Btn_ClearSelectDS.Name = "Btn_ClearSelectDS";
            this.Btn_ClearSelectDS.Size = new Size(80, 0x19);
            this.Btn_ClearSelectDS.TabIndex = 0xba;
            this.Btn_ClearSelectDS.Text = "清空所选";
            this.Btn_ClearSelectDS.UseVisualStyleBackColor = true;
            this.Btn_ClearSelectDS.Click += new EventHandler(this.Btn_ClearSelectDS_Click);
            this.Btn_Ok.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Btn_Ok.Location = new Point(0x41a, 5);
            this.Btn_Ok.Name = "Btn_Ok";
            this.Btn_Ok.Size = new Size(60, 0x19);
            this.Btn_Ok.TabIndex = 0xb9;
            this.Btn_Ok.Text = "查询";
            this.Btn_Ok.UseVisualStyleBackColor = true;
            this.Btn_Ok.Click += new EventHandler(this.Btn_Ok_Click);
            this.Btn_Close.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Btn_Close.Location = new Point(0x45c, 5);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new Size(60, 0x19);
            this.Btn_Close.TabIndex = 0xb8;
            this.Btn_Close.Text = "关闭";
            this.Btn_Close.UseVisualStyleBackColor = true;
            this.Btn_Close.Click += new EventHandler(this.Btn_Close_Click);
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x4a0, 0x2e5);
            base.Controls.Add(this.Tab_Main);
            base.Controls.Add(this.Pnl_AppName);
            base.FormBorderStyle = FormBorderStyle.Sizable;
            base.MaximizeBox = true;
            base.MinimizeBox = true;
            base.Name = "FrmManage";
            base.Load += new EventHandler(this.FrmManage_Load);
            this.Tab_Main.ResumeLayout(false);
            this.Tap_User.ResumeLayout(false);
            this.Pnl_User.ResumeLayout(false);
            ((ISupportInitialize) this.Egv_UserList).EndInit();
            ((ISupportInitialize) this.Egv_DLUserList).EndInit();
            this.Pnl_UserBottom.ResumeLayout(false);
            this.Pnl_UserLeft3.ResumeLayout(false);
            this.Pnl_UserLeft2.ResumeLayout(false);
            this.Pnl_UserLeft2.PerformLayout();
            this.Pnl_UserLeft1.ResumeLayout(false);
            this.Pnl_UserLeft1.PerformLayout();
            this.Nm_DK.EndInit();
            this.Pnl_DLUserBottom.ResumeLayout(false);
            this.Pnl_DLUserLeft.ResumeLayout(false);
            this.Pnl_PTUserBottom.ResumeLayout(false);
            this.Pnl_PTUserMain.ResumeLayout(false);
            this.Pnl_PTUserMain.PerformLayout();
            this.Pnl_UserTop.ResumeLayout(false);
            this.Pnl_UserTop.PerformLayout();
            this.Tap_DL.ResumeLayout(false);
            this.Pnl_DL.ResumeLayout(false);
            this.Pnl_DLMain.ResumeLayout(false);
            this.Pnl_DLImage.ResumeLayout(false);
            this.Pnl_DLTop.ResumeLayout(false);
            this.Pnl_DLTop.PerformLayout();
            ((ISupportInitialize) this.Pic_DLGG).EndInit();
            this.Tap_FNEncrypt.ResumeLayout(false);
            this.Pnl_FNEncrypt.ResumeLayout(false);
            this.Pnl_FNEncryptMain.ResumeLayout(false);
            this.Pnl_FNEncryptMain.PerformLayout();
            this.Pnl_FNEncryptTop.ResumeLayout(false);
            this.Pnl_FNEncryptTop.PerformLayout();
            this.Pnl_AppName.ResumeLayout(false);
            this.Pnl_AppName3.ResumeLayout(false);
            this.Pnl_AppName3.PerformLayout();
            this.Pnl_AppName2.ResumeLayout(false);
            this.Pnl_AppName2.PerformLayout();
            this.Pnl_AppName1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void Lbl_PTEdit_Click(object sender, EventArgs e)
        {
            if (CommFunc.AgreeMessage("是否修改选择的账号？", false, MessageBoxIcon.Asterisk, ""))
            {
                foreach (DataGridViewRow row in this.CurrentGirdView.SelectedRows)
                {
                    int index = row.Index;
                    ConfigurationStatus.SCAccountData pAccountData = this.CurrentViewList[index];
                    pAccountData.PTLoginAudit = this.Cbb_PTAudit.Text;
                    pAccountData.PTLoginHint = this.Txt_PTHint.Text;
                    SQLData.UpdataPTUserRow(pAccountData);
                }
                this.Btn_Ok_Click(null, null);
            }
        }

        private void LoadAppName()
        {
            List<string> list = CommFunc.SplitString(HttpHelper.GetWebData("TypeName", ""), "\r\n", -1);
            List<string> list2 = new List<string>();
            foreach (string str2 in list)
            {
                List<string> list3 = CommFunc.SplitString(str2.Split(new char[] { ':' })[1], ",", -1);
                foreach (string str3 in list3)
                {
                    string[] strArray = str3.Split(new char[] { '-' });
                    if (strArray[0] == AppInfo.Account.AppPerName)
                    {
                        AppInfo.Account.AppViewName = strArray[1];
                        break;
                    }
                }
            }
        }

        private void LoadDLData()
        {
            this.Tbl_DLLoginPT.Value = AppInfo.Account.Configuration.LoginPTListViewString;
            this.Tbl_DLID.Value = AppInfo.Account.ID;
            this.Tbl_DLPW.Value = AppInfo.Account.PW;
            this.Tbl_DLQQ.Value = AppInfo.Account.Configuration.QQ;
            this.Tbl_DLQQGroup.Value = AppInfo.Account.Configuration.QQGroup;
            this.Tbl_DLFNEdit.Value = AppInfo.Account.Configuration.FNEdit;
            this.Tbl_DLImageLink.Value = AppInfo.Account.Configuration.ImageLink;
            this.Tbl_DLWebUrl.Value = AppInfo.Account.Configuration.WebUrl;
            this.Pic_DLGG.Image = AppInfo.Account.GGImage;
            this.Tbl_DLFNEdit.Visible = AppInfo.Account.Configuration.FNEdit != "";
        }

        private void LoadDLUserList()
        {
            List<int> pType = new List<int> { 
                1,
                1,
                1,
                1,
                1,
                2
            };
            List<string> pText = new List<string> { 
                "账号",
                "密码",
                "软件名称",
                "到期时间",
                "停用",
                "配置"
            };
            List<int> pWidth = new List<int> { 
                120,
                120,
                100,
                100,
                60,
                10
            };
            List<bool> pRead = new List<bool> { 
                true,
                true,
                true,
                true,
                true,
                true
            };
            List<bool> pVis = new List<bool> { 
                true,
                true,
                true,
                false,
                true,
                true
            };
            this.Egv_DLUserList.LoadInitialization(pType, pText, pWidth, pRead, pVis, null);
            this.Egv_DLUserList.MultiSelect = true;
            this.Egv_DLUserList.VirtualMode = true;
            CommFunc.SetExpandGirdViewFormat(this.Egv_DLUserList, 9);
            this.Egv_DLUserList.Columns[this.Egv_DLUserList.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Egv_DLUserList.CellValueNeeded += new DataGridViewCellValueEventHandler(this.Egv_DLUserList_CellValueNeeded);
            for (int i = 0; i < this.Egv_DLUserList.ColumnCount; i++)
            {
                this.Egv_DLUserList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            ((ImageTextColumn) this.Egv_DLUserList.Columns[5]).ButtonClick += new DataGridViewCellMouseEventHandler(this.Egv_DLUserList_ButtonClick);
        }

        private void LoadFNEncryptData()
        {
            string str = CommFunc.Join(AppInfo.Account.Configuration.FNEncrypIDList, ",");
            if (str != "")
            {
                str = str + ",";
            }
            this.Txt_FNEncrypt.Text = str;
        }

        private void LoadUserList()
        {
            List<int> list;
            List<string> list2;
            List<int> list3;
            List<bool> list4;
            List<bool> list5;
            if (this.SelectType == ConfigurationStatus.ManageType.User)
            {
                list = new List<int> { 
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1
                };
                list2 = new List<string> { 
                    "账号",
                    "密码",
                    "机器码",
                    "软件名称",
                    "QQ",
                    "手机",
                    "备注",
                    "到期时间",
                    "最后登录时间",
                    "在线",
                    "停用",
                    "登录平台账号",
                    "状态"
                };
                list3 = new List<int> { 
                    120,
                    120,
                    90,
                    90,
                    90,
                    90,
                    90,
                    90,
                    140,
                    60,
                    60,
                    120,
                    10
                };
                list4 = new List<bool> { 
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true
                };
                list5 = new List<bool> { 
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true
                };
                this.Egv_UserList.LoadInitialization(list, list2, list3, list4, list5, null);
                if (!AppInfo.Account.Configuration.ViewQQPhone)
                {
                    this.Egv_UserList.Columns[4].Visible = this.Egv_UserList.Columns[5].Visible = false;
                }
                if ((AppInfo.Account.AppPerName == "LKGJ") || (AppInfo.Account.AppPerName == "JXGJ"))
                {
                    this.Egv_UserList.Columns[1].Visible = false;
                }
            }
            else if (this.SelectType == ConfigurationStatus.ManageType.PT)
            {
                list = new List<int> { 
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1,
                    1
                };
                list2 = new List<string> { 
                    "账号",
                    "机器码",
                    "软件名称",
                    "平台名称",
                    "最后登录时间",
                    "在线",
                    "审核状态",
                    "提示"
                };
                list3 = new List<int> { 
                    120,
                    100,
                    100,
                    100,
                    140,
                    60,
                    100,
                    10
                };
                list4 = new List<bool> { 
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true
                };
                list5 = new List<bool> { 
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    true
                };
                this.Egv_UserList.LoadInitialization(list, list2, list3, list4, list5, null);
            }
            this.Egv_UserList.MultiSelect = true;
            this.Egv_UserList.VirtualMode = true;
            CommFunc.SetExpandGirdViewFormat(this.Egv_UserList, 9);
            this.Egv_UserList.Columns[this.Egv_UserList.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Egv_UserList.CellValueNeeded += new DataGridViewCellValueEventHandler(this.Egv_UserList_CellValueNeeded);
            for (int i = 0; i < this.Egv_UserList.ColumnCount; i++)
            {
                this.Egv_UserList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void RefreshControl()
        {
            this.Pnl_UserBottom.Visible = this.SelectType == ConfigurationStatus.ManageType.User;
            this.Pnl_DLUserBottom.Visible = this.SelectType == ConfigurationStatus.ManageType.DL;
            this.Pnl_PTUserBottom.Visible = this.SelectType == ConfigurationStatus.ManageType.PT;
            this.Egv_UserList.Visible = this.CheckIsUser();
            this.Egv_DLUserList.Visible = !this.CheckIsUser();
            List<string> pList = new List<string>();
            foreach (DataGridViewColumn column in this.CurrentGirdView.Columns)
            {
                pList.Add(column.HeaderText);
            }
            CommFunc.SetComboBoxList(this.Cbb_Sort, pList);
        }

        private void RefreshCount()
        {
            this.Lbl_CountValue.Text = $"{this.CurrentGirdView.SelectedRows.Count}/{this.CurrentGirdView.RowCount}";
        }

        private void RefreshUserList(List<ConfigurationStatus.SCAccountData> pViewList)
        {
            CommFunc.RefreshDataGridView(this.CurrentGirdView, pViewList.Count);
        }

        private void SaveFNEncryptData()
        {
            this.FilterFNEncrypt(true);
            if (SQLData.SaveFNEncrypt(AppInfo.Account))
            {
                CommFunc.PublicMessageAll("方案加密账号保存成功！", true, MessageBoxIcon.Asterisk, "");
            }
            else
            {
                CommFunc.PublicMessageAll("方案加密账号保存失败，请重试！", true, MessageBoxIcon.Asterisk, "");
            }
        }

        private void SearchClick()
        {
            try
            {
                if ((this.Cbb_DLName.Items.Count == 0) || !this.CheckIsUser())
                {
                    this.DLUserList = SQLData.GetAllDLUserList(AppInfo.Account.AppName, AppInfo.Account);
                    List<string> pList = new List<string> {
                        AppInfo.Account.AppName
                    };
                    foreach (ConfigurationStatus.SCAccountData data in this.DLUserList)
                    {
                        pList.Add(data.AppName);
                    }
                    CommFunc.SetComboBoxList(this.Cbb_DLName, pList);
                }
                if (this.CheckIsUser())
                {
                    string text = this.Cbb_DLName.Text;
                    ConfigurationStatus.SCAccountData account = AppInfo.Account;
                    if (text.Contains("-"))
                    {
                        foreach (ConfigurationStatus.SCAccountData data in this.DLUserList)
                        {
                            if (data.AppName == text)
                            {
                                account = data;
                                break;
                            }
                        }
                    }
                    if (this.SelectType == ConfigurationStatus.ManageType.User)
                    {
                        this.UserList = SQLData.GetAllUserList(text, account.ID, account.PW);
                    }
                    else
                    {
                        this.UserList = SQLData.GetAllPTUserList(text);
                    }
                }
                this.SearchMain(true, true);
            }
            catch (Exception exception)
            {
                CommFunc.PublicMessageAll(exception.ToString(), false, MessageBoxIcon.Asterisk, "");
            }
        }

        private void SearchMain(bool pIsSort, bool pIsFilte)
        {
            if (pIsSort)
            {
                this.SortUserList(this.CurrentList);
            }
            if (pIsFilte)
            {
                this.CurrentViewList = this.FilterUserList(this.CurrentList);
            }
            this.RefreshUserList(this.CurrentViewList);
            this.RefreshCount();
        }

        private void SortUserList(List<ConfigurationStatus.SCAccountData> pList)
        {
            switch (this.Cbb_Sort.Text)
            {
                case "账号":
                    pList.Sort(delegate (ConfigurationStatus.SCAccountData info1, ConfigurationStatus.SCAccountData info2) {
                        string iD = info1.ID;
                        string strB = info2.ID;
                        return string.Compare(iD, strB);
                    });
                    break;

                case "密码":
                    pList.Sort(delegate (ConfigurationStatus.SCAccountData info1, ConfigurationStatus.SCAccountData info2) {
                        string pW = info1.PW;
                        string strB = info2.PW;
                        return string.Compare(pW, strB);
                    });
                    break;

                case "机器码":
                    pList.Sort(delegate (ConfigurationStatus.SCAccountData info1, ConfigurationStatus.SCAccountData info2) {
                        string machineCode = info1.MachineCode;
                        string strB = info2.MachineCode;
                        return string.Compare(machineCode, strB);
                    });
                    break;

                case "QQ":
                    pList.Sort((info1, info2) => string.Compare(info2.QQ, info1.QQ));
                    break;

                case "手机":
                    pList.Sort((info1, info2) => string.Compare(info2.Phone, info1.Phone));
                    break;

                case "备注":
                    pList.Sort((info1, info2) => string.Compare(info2.Remark, info1.Remark));
                    break;

                case "到期时间":
                    pList.Sort(delegate (ConfigurationStatus.SCAccountData info1, ConfigurationStatus.SCAccountData info2) {
                        string activeTime = info1.ActiveTime;
                        string strA = info2.ActiveTime;
                        if (activeTime == "9999-12-31")
                        {
                            return 1;
                        }
                        if (strA == "9999-12-31")
                        {
                            return -1;
                        }
                        return string.Compare(strA, activeTime);
                    });
                    break;

                case "最后登录时间":
                    pList.Sort((info1, info2) => string.Compare(info2.OnLineTime, info1.OnLineTime));
                    break;

                case "在线":
                    pList.Sort((info1, info2) => string.Compare(info2.IsOnLineTime.ToString(), info1.IsOnLineTime.ToString()));
                    break;

                case "停用":
                    pList.Sort((info1, info2) => string.Compare(info2.IsStop.ToString(), info1.IsStop.ToString()));
                    break;

                case "登录平台账号":
                    pList.Sort(delegate (ConfigurationStatus.SCAccountData info1, ConfigurationStatus.SCAccountData info2) {
                        string pTUser = info1.PTUser;
                        string strB = info2.PTUser;
                        return string.Compare(pTUser, strB);
                    });
                    break;

                case "状态":
                    pList.Sort(delegate (ConfigurationStatus.SCAccountData info1, ConfigurationStatus.SCAccountData info2) {
                        string stateString = info1.StateString;
                        string strB = info2.StateString;
                        if ((stateString == "") && (strB != ""))
                        {
                            return 1;
                        }
                        if ((strB == "") && (stateString != ""))
                        {
                            return -1;
                        }
                        return string.Compare(stateString, strB);
                    });
                    break;

                case "配置":
                    pList.Sort(delegate (ConfigurationStatus.SCAccountData info1, ConfigurationStatus.SCAccountData info2) {
                        string configurationString = info1.ConfigurationString;
                        string strB = info2.ConfigurationString;
                        return string.Compare(configurationString, strB);
                    });
                    break;
            }
        }

        private void Tab_Main_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Pnl_AppName1.Visible = this.Pnl_AppName2.Visible = this.Pnl_AppName3.Visible = this.CheckIsUserList();
            this.Btn_Ok.Text = this.CheckIsUserList() ? "查询" : "保存";
        }

        private void Txt_UserID_TextChanged(object sender, EventArgs e)
        {
            if (base._RunEvent && this.Ckb_UserID.Checked)
            {
                this.SearchMain(false, true);
            }
        }

        public bool WebLoginMain()
        {
            if (AppInfo.Account.LoginStatus)
            {
                AppInfo.Account.LoginStatus = false;
            }
            else
            {
                FrmDLUserLogin login = new FrmDLUserLogin(AppInfo.Account);
                if (login.ShowDialog() == DialogResult.OK)
                {
                    AppInfo.Account.LoginStatus = true;
                    this.Lbl_Time.Enabled = this.Cbb_Time.Enabled = this.Btn_CZTime.Enabled = AppInfo.Account.AllowCZ;
                    this.Btn_Stop.Enabled = this.Btn_JCStop.Enabled = this.Btn_DeleteUser.Enabled = AppInfo.Account.AllowDelete;
                    this.Btn_ClearSelectDS.Enabled = AppInfo.Account.AllowClear;
                    this.Lbl_DKHint.Enabled = this.Nm_DK.Enabled = this.Btn_DK.Enabled = AppInfo.Account.AllowDK;
                }
            }
            return AppInfo.Account.LoginStatus;
        }

        private ExpandGirdView CurrentGirdView =>
            (this.CheckIsUser() ? this.Egv_UserList : this.Egv_DLUserList);

        private List<ConfigurationStatus.SCAccountData> CurrentList =>
            (this.CheckIsUser() ? this.UserList : this.DLUserList);

        private List<ConfigurationStatus.SCAccountData> CurrentViewList
        {
            get => 
                (this.CheckIsUser() ? this.UserViewList : this.DLUserViewList);
            set
            {
                if (this.CheckIsUser())
                {
                    this.UserViewList = value;
                }
                else
                {
                    this.DLUserViewList = value;
                }
            }
        }

        private ConfigurationStatus.ManageType SelectType
        {
            get
            {
                if (this.Cbb_AppName.SelectedIndex == 0)
                {
                    if (AppInfo.Account.Configuration.IsPTLogin)
                    {
                        return ConfigurationStatus.ManageType.PT;
                    }
                    return ConfigurationStatus.ManageType.User;
                }
                return ConfigurationStatus.ManageType.DL;
            }
        }
    }
}

