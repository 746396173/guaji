namespace IntelligentPlanning
{
    using IntelligentPlanning.CustomControls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class FrmDLDataInput : ExForm
    {
        private ConfigurationStatus.SCAccountData AccountData;
        private Button Btn_Close;
        private Button Btn_DLGGReset;
        private Button Btn_DLGGYL;
        private Button Btn_Ok;
        private CheckBox Ckb_AllowClear;
        private CheckBox Ckb_AllowCZ;
        private CheckBox Ckb_AllowDelete;
        private CheckBox Ckb_AllowDK;
        private IContainer components = null;
        private string GGImageString = "";
        private bool IsAdd;
        private Label Lbl_DLGGSizeKey;
        private Label Lbl_DLGGSizeValue;
        private PictureBox Pic_DLGG;
        private Panel Pnl_AppName;
        private Panel Pnl_DL;
        private Panel Pnl_DLImage;
        private Panel Pnl_DLMain;
        private Panel Pnl_DLTop;
        private Panel Pnl_Top;
        private List<string> PTNameList;
        private TextboxLable Tbl_DLFNEdit;
        private TextboxLable Tbl_DLID;
        private TextboxLable Tbl_DLImageLink;
        private TextboxLable Tbl_DLLoginCKPT;
        private TextboxLable Tbl_DLLoginPT;
        private TextboxLable Tbl_DLPW;
        private TextboxLable Tbl_DLQQ;
        private TextboxLable Tbl_DLQQGroup;
        private TextboxLable Tbl_DLWebUrl;

        public FrmDLDataInput(ConfigurationStatus.SCAccountData pAccountData, Dictionary<string, ConfigurationStatus.PTLine> pLineDic, bool pIsAdd)
        {
            this.InitializeComponent();
            this.AccountData = pAccountData;
            this.IsAdd = pIsAdd;
            this.Btn_Ok.Text = pIsAdd ? "添加" : "保存";
            this.Text = pIsAdd ? "添加代理信息" : "编辑代理信息";
            this.PTNameList = CommFunc.GetDicKeyList<ConfigurationStatus.PTLine>(pLineDic);
            if (pIsAdd)
            {
                this.AccountData.Configuration.LoginPTList = this.PTNameList;
            }
            this.Tbl_DLLoginPT.Value = this.AccountData.Configuration.LoginPTListViewString;
            this.Tbl_DLLoginCKPT.Value = this.AccountData.Configuration.GetLoginPTListViewString(this.PTNameList);
            this.Tbl_DLFNEdit.Visible = this.AccountData.Configuration.FNEdit != "";
            if (!pIsAdd)
            {
                this.LoadDLData();
            }
            else
            {
                this.Tbl_DLID.Value = this.AccountData.AppCharName;
                this.Tbl_DLID.ValueFocus();
            }
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void Btn_DLGGReset_Click(object sender, EventArgs e)
        {
            this.Pic_DLGG.Image = this.AccountData.GGImage;
            this.GGImageString = this.AccountData.GGImageString;
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

        private void Btn_Ok_Click(object sender, EventArgs e)
        {
            if (this.SetConfiguration())
            {
                bool flag = false;
                if (this.IsAdd)
                {
                    flag = this.DLAddMain();
                }
                else
                {
                    flag = this.DLSaveMain();
                }
                base.DialogResult = flag ? DialogResult.OK : DialogResult.No;
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

        private bool DLAddMain()
        {
            this.AccountData.AppName = this.AccountData.ID;
            string pResponseText = SQLData.AddDLUserRow(this.AccountData, AppInfo.Account);
            if (CommFunc.CheckResponseText(pResponseText))
            {
                CommFunc.PublicMessageAll("添加代理成功！", true, MessageBoxIcon.Asterisk, "");
                return true;
            }
            string str2 = "请联系客服人员！";
            if (pResponseText.Contains("插入重复键"))
            {
                str2 = "该代理已经存在！";
                this.Tbl_DLID.ValueFocus();
            }
            CommFunc.PublicMessageAll($"添加代理失败，{str2}", true, MessageBoxIcon.Asterisk, "");
            return false;
        }

        private bool DLSaveMain()
        {
            if (SQLData.UpdataDLUserRow(this.AccountData))
            {
                CommFunc.PublicMessageAll("保存数据成功！", true, MessageBoxIcon.Asterisk, "");
                return true;
            }
            CommFunc.PublicMessageAll("保存数据超时，请重新保存！", true, MessageBoxIcon.Asterisk, "");
            return false;
        }

        private void FrmDLDataInput_Load(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            this.Pnl_DL = new Panel();
            this.Pnl_DLMain = new Panel();
            this.Pnl_Top = new Panel();
            this.Ckb_AllowDK = new CheckBox();
            this.Ckb_AllowClear = new CheckBox();
            this.Ckb_AllowDelete = new CheckBox();
            this.Ckb_AllowCZ = new CheckBox();
            this.Pnl_DLImage = new Panel();
            this.Pnl_DLTop = new Panel();
            this.Lbl_DLGGSizeValue = new Label();
            this.Lbl_DLGGSizeKey = new Label();
            this.Btn_DLGGReset = new Button();
            this.Btn_DLGGYL = new Button();
            this.Pic_DLGG = new PictureBox();
            this.Tbl_DLImageLink = new TextboxLable();
            this.Tbl_DLFNEdit = new TextboxLable();
            this.Tbl_DLQQGroup = new TextboxLable();
            this.Tbl_DLQQ = new TextboxLable();
            this.Tbl_DLPW = new TextboxLable();
            this.Tbl_DLID = new TextboxLable();
            this.Tbl_DLLoginCKPT = new TextboxLable();
            this.Tbl_DLLoginPT = new TextboxLable();
            this.Pnl_AppName = new Panel();
            this.Btn_Ok = new Button();
            this.Btn_Close = new Button();
            this.Tbl_DLWebUrl = new TextboxLable();
            this.Pnl_DL.SuspendLayout();
            this.Pnl_DLMain.SuspendLayout();
            this.Pnl_Top.SuspendLayout();
            this.Pnl_DLImage.SuspendLayout();
            this.Pnl_DLTop.SuspendLayout();
            ((ISupportInitialize) this.Pic_DLGG).BeginInit();
            this.Pnl_AppName.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_DL.Controls.Add(this.Pnl_DLMain);
            this.Pnl_DL.Dock = DockStyle.Fill;
            this.Pnl_DL.Location = new Point(0, 0);
            this.Pnl_DL.Name = "Pnl_DL";
            this.Pnl_DL.Size = new Size(0x327, 0x229);
            this.Pnl_DL.TabIndex = 0x4e;
            this.Pnl_DLMain.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_DLMain.Controls.Add(this.Pnl_Top);
            this.Pnl_DLMain.Controls.Add(this.Pnl_DLImage);
            this.Pnl_DLMain.Controls.Add(this.Tbl_DLImageLink);
            this.Pnl_DLMain.Controls.Add(this.Tbl_DLWebUrl);
            this.Pnl_DLMain.Controls.Add(this.Tbl_DLFNEdit);
            this.Pnl_DLMain.Controls.Add(this.Tbl_DLQQGroup);
            this.Pnl_DLMain.Controls.Add(this.Tbl_DLQQ);
            this.Pnl_DLMain.Controls.Add(this.Tbl_DLPW);
            this.Pnl_DLMain.Controls.Add(this.Tbl_DLID);
            this.Pnl_DLMain.Controls.Add(this.Tbl_DLLoginCKPT);
            this.Pnl_DLMain.Controls.Add(this.Tbl_DLLoginPT);
            this.Pnl_DLMain.Dock = DockStyle.Fill;
            this.Pnl_DLMain.Location = new Point(0, 0);
            this.Pnl_DLMain.Name = "Pnl_DLMain";
            this.Pnl_DLMain.Size = new Size(0x327, 0x229);
            this.Pnl_DLMain.TabIndex = 0;
            this.Pnl_Top.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Top.Controls.Add(this.Ckb_AllowDK);
            this.Pnl_Top.Controls.Add(this.Ckb_AllowClear);
            this.Pnl_Top.Controls.Add(this.Ckb_AllowDelete);
            this.Pnl_Top.Controls.Add(this.Ckb_AllowCZ);
            this.Pnl_Top.Dock = DockStyle.Top;
            this.Pnl_Top.Location = new Point(0x18e, 0xd8);
            this.Pnl_Top.Name = "Pnl_Top";
            this.Pnl_Top.Size = new Size(0x197, 0x23);
            this.Pnl_Top.TabIndex = 13;
            this.Ckb_AllowDK.AutoSize = true;
            this.Ckb_AllowDK.Location = new Point(0x129, 7);
            this.Ckb_AllowDK.Name = "Ckb_AllowDK";
            this.Ckb_AllowDK.Size = new Size(0x63, 0x15);
            this.Ckb_AllowDK.TabIndex = 0x11;
            this.Ckb_AllowDK.Text = "允许生成点卡";
            this.Ckb_AllowDK.UseVisualStyleBackColor = true;
            this.Ckb_AllowClear.AutoSize = true;
            this.Ckb_AllowClear.Location = new Point(0xc0, 7);
            this.Ckb_AllowClear.Name = "Ckb_AllowClear";
            this.Ckb_AllowClear.Size = new Size(0x63, 0x15);
            this.Ckb_AllowClear.TabIndex = 0x10;
            this.Ckb_AllowClear.Text = "允许清除状态";
            this.Ckb_AllowClear.UseVisualStyleBackColor = true;
            this.Ckb_AllowDelete.AutoSize = true;
            this.Ckb_AllowDelete.Location = new Point(0x57, 7);
            this.Ckb_AllowDelete.Name = "Ckb_AllowDelete";
            this.Ckb_AllowDelete.Size = new Size(0x63, 0x15);
            this.Ckb_AllowDelete.TabIndex = 15;
            this.Ckb_AllowDelete.Text = "允许删除用户";
            this.Ckb_AllowDelete.UseVisualStyleBackColor = true;
            this.Ckb_AllowCZ.AutoSize = true;
            this.Ckb_AllowCZ.Location = new Point(6, 7);
            this.Ckb_AllowCZ.Name = "Ckb_AllowCZ";
            this.Ckb_AllowCZ.Size = new Size(0x4b, 0x15);
            this.Ckb_AllowCZ.TabIndex = 14;
            this.Ckb_AllowCZ.Text = "允许充值";
            this.Ckb_AllowCZ.UseVisualStyleBackColor = true;
            this.Pnl_DLImage.Controls.Add(this.Pnl_DLTop);
            this.Pnl_DLImage.Controls.Add(this.Pic_DLGG);
            this.Pnl_DLImage.Dock = DockStyle.Left;
            this.Pnl_DLImage.Location = new Point(0, 0xd8);
            this.Pnl_DLImage.Name = "Pnl_DLImage";
            this.Pnl_DLImage.Size = new Size(0x18e, 0x14f);
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
            this.Btn_DLGGReset.Tag = "7";
            this.Btn_DLGGReset.Text = "重置";
            this.Btn_DLGGReset.UseVisualStyleBackColor = true;
            this.Btn_DLGGReset.Click += new EventHandler(this.Btn_DLGGReset_Click);
            this.Btn_DLGGYL.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Btn_DLGGYL.Location = new Point(0x44, 3);
            this.Btn_DLGGYL.Name = "Btn_DLGGYL";
            this.Btn_DLGGYL.Size = new Size(60, 0x19);
            this.Btn_DLGGYL.TabIndex = 0xba;
            this.Btn_DLGGYL.Tag = "8";
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
            this.Tbl_DLImageLink.Size = new Size(0x325, 0x1b);
            this.Tbl_DLImageLink.TabIndex = 6;
            this.Tbl_DLImageLink.Tag = "";
            this.Tbl_DLImageLink.Value = "";
            this.Tbl_DLImageLink.ValueReadOnly = false;
            this.Tbl_DLFNEdit.Dock = DockStyle.Top;
            this.Tbl_DLFNEdit.Font = new Font("微软雅黑", 11f);
            this.Tbl_DLFNEdit.Hint = "方案密码";
            this.Tbl_DLFNEdit.Location = new Point(0, 0xa2);
            this.Tbl_DLFNEdit.Margin = new Padding(4, 5, 4, 5);
            this.Tbl_DLFNEdit.Name = "Tbl_DLFNEdit";
            this.Tbl_DLFNEdit.Size = new Size(0x325, 0x1b);
            this.Tbl_DLFNEdit.TabIndex = 7;
            this.Tbl_DLFNEdit.Tag = "";
            this.Tbl_DLFNEdit.Value = "";
            this.Tbl_DLFNEdit.ValueReadOnly = false;
            this.Tbl_DLQQGroup.Dock = DockStyle.Top;
            this.Tbl_DLQQGroup.Font = new Font("微软雅黑", 11f);
            this.Tbl_DLQQGroup.Hint = "QQ群";
            this.Tbl_DLQQGroup.Location = new Point(0, 0x87);
            this.Tbl_DLQQGroup.Margin = new Padding(4, 5, 4, 5);
            this.Tbl_DLQQGroup.Name = "Tbl_DLQQGroup";
            this.Tbl_DLQQGroup.Size = new Size(0x325, 0x1b);
            this.Tbl_DLQQGroup.TabIndex = 5;
            this.Tbl_DLQQGroup.Tag = "";
            this.Tbl_DLQQGroup.Value = "";
            this.Tbl_DLQQGroup.ValueReadOnly = false;
            this.Tbl_DLQQ.Dock = DockStyle.Top;
            this.Tbl_DLQQ.Font = new Font("微软雅黑", 11f);
            this.Tbl_DLQQ.Hint = "QQ";
            this.Tbl_DLQQ.Location = new Point(0, 0x6c);
            this.Tbl_DLQQ.Margin = new Padding(4, 5, 4, 5);
            this.Tbl_DLQQ.Name = "Tbl_DLQQ";
            this.Tbl_DLQQ.Size = new Size(0x325, 0x1b);
            this.Tbl_DLQQ.TabIndex = 4;
            this.Tbl_DLQQ.Tag = "";
            this.Tbl_DLQQ.Value = "";
            this.Tbl_DLQQ.ValueReadOnly = false;
            this.Tbl_DLPW.Dock = DockStyle.Top;
            this.Tbl_DLPW.Font = new Font("微软雅黑", 11f);
            this.Tbl_DLPW.Hint = "密码";
            this.Tbl_DLPW.Location = new Point(0, 0x51);
            this.Tbl_DLPW.Margin = new Padding(4, 5, 4, 5);
            this.Tbl_DLPW.Name = "Tbl_DLPW";
            this.Tbl_DLPW.Size = new Size(0x325, 0x1b);
            this.Tbl_DLPW.TabIndex = 3;
            this.Tbl_DLPW.Value = "";
            this.Tbl_DLPW.ValueReadOnly = false;
            this.Tbl_DLID.Dock = DockStyle.Top;
            this.Tbl_DLID.Font = new Font("微软雅黑", 11f);
            this.Tbl_DLID.Hint = "账号";
            this.Tbl_DLID.Location = new Point(0, 0x36);
            this.Tbl_DLID.Margin = new Padding(4, 5, 4, 5);
            this.Tbl_DLID.Name = "Tbl_DLID";
            this.Tbl_DLID.Size = new Size(0x325, 0x1b);
            this.Tbl_DLID.TabIndex = 2;
            this.Tbl_DLID.Value = "";
            this.Tbl_DLID.ValueReadOnly = false;
            this.Tbl_DLLoginCKPT.Dock = DockStyle.Top;
            this.Tbl_DLLoginCKPT.Font = new Font("微软雅黑", 11f);
            this.Tbl_DLLoginCKPT.Hint = "可选平台";
            this.Tbl_DLLoginCKPT.Location = new Point(0, 0x1b);
            this.Tbl_DLLoginCKPT.Margin = new Padding(4, 5, 4, 5);
            this.Tbl_DLLoginCKPT.Name = "Tbl_DLLoginCKPT";
            this.Tbl_DLLoginCKPT.Size = new Size(0x325, 0x1b);
            this.Tbl_DLLoginCKPT.TabIndex = 12;
            this.Tbl_DLLoginCKPT.Value = "";
            this.Tbl_DLLoginCKPT.ValueReadOnly = true;
            this.Tbl_DLLoginPT.Dock = DockStyle.Top;
            this.Tbl_DLLoginPT.Font = new Font("微软雅黑", 11f);
            this.Tbl_DLLoginPT.Hint = "登录平台";
            this.Tbl_DLLoginPT.Location = new Point(0, 0);
            this.Tbl_DLLoginPT.Margin = new Padding(4, 5, 4, 5);
            this.Tbl_DLLoginPT.Name = "Tbl_DLLoginPT";
            this.Tbl_DLLoginPT.Size = new Size(0x325, 0x1b);
            this.Tbl_DLLoginPT.TabIndex = 1;
            this.Tbl_DLLoginPT.Value = "";
            this.Tbl_DLLoginPT.ValueReadOnly = false;
            this.Pnl_AppName.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_AppName.Controls.Add(this.Btn_Ok);
            this.Pnl_AppName.Controls.Add(this.Btn_Close);
            this.Pnl_AppName.Dock = DockStyle.Bottom;
            this.Pnl_AppName.Location = new Point(0, 0x229);
            this.Pnl_AppName.Name = "Pnl_AppName";
            this.Pnl_AppName.Size = new Size(0x327, 0x23);
            this.Pnl_AppName.TabIndex = 0x4d;
            this.Btn_Ok.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Btn_Ok.Location = new Point(0x2a1, 5);
            this.Btn_Ok.Name = "Btn_Ok";
            this.Btn_Ok.Size = new Size(60, 0x19);
            this.Btn_Ok.TabIndex = 0xb9;
            this.Btn_Ok.Text = "新建";
            this.Btn_Ok.UseVisualStyleBackColor = true;
            this.Btn_Ok.Click += new EventHandler(this.Btn_Ok_Click);
            this.Btn_Close.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Btn_Close.Location = new Point(0x2e3, 5);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new Size(60, 0x19);
            this.Btn_Close.TabIndex = 0xb8;
            this.Btn_Close.Text = "关闭";
            this.Btn_Close.UseVisualStyleBackColor = true;
            this.Btn_Close.Click += new EventHandler(this.Btn_Close_Click);
            this.Tbl_DLWebUrl.Dock = DockStyle.Top;
            this.Tbl_DLWebUrl.Font = new Font("微软雅黑", 11f);
            this.Tbl_DLWebUrl.Hint = "右下角网址";
            this.Tbl_DLWebUrl.Location = new Point(0x18e, 0xfb);
            this.Tbl_DLWebUrl.Margin = new Padding(4, 5, 4, 5);
            this.Tbl_DLWebUrl.Name = "Tbl_DLWebUrl";
            this.Tbl_DLWebUrl.Size = new Size(0x197, 0x1b);
            this.Tbl_DLWebUrl.TabIndex = 14;
            this.Tbl_DLWebUrl.Tag = "";
            this.Tbl_DLWebUrl.Value = "";
            this.Tbl_DLWebUrl.ValueReadOnly = false;
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x327, 0x24c);
            base.Controls.Add(this.Pnl_DL);
            base.Controls.Add(this.Pnl_AppName);
            base.Name = "FrmDLDataInput";
            base.Load += new EventHandler(this.FrmDLDataInput_Load);
            this.Pnl_DL.ResumeLayout(false);
            this.Pnl_DLMain.ResumeLayout(false);
            this.Pnl_Top.ResumeLayout(false);
            this.Pnl_Top.PerformLayout();
            this.Pnl_DLImage.ResumeLayout(false);
            this.Pnl_DLTop.ResumeLayout(false);
            this.Pnl_DLTop.PerformLayout();
            ((ISupportInitialize) this.Pic_DLGG).EndInit();
            this.Pnl_AppName.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void LoadDLData()
        {
            this.Tbl_DLID.Value = this.AccountData.ID;
            this.Tbl_DLPW.Value = this.AccountData.PW;
            this.Tbl_DLQQ.Value = this.AccountData.Configuration.QQ;
            this.Ckb_AllowCZ.Checked = this.AccountData.AllowCZ;
            this.Ckb_AllowDelete.Checked = this.AccountData.AllowDelete;
            this.Ckb_AllowClear.Checked = this.AccountData.AllowClear;
            this.Ckb_AllowDK.Checked = this.AccountData.AllowDK;
            this.Tbl_DLQQGroup.Value = this.AccountData.Configuration.QQGroup;
            this.Tbl_DLFNEdit.Value = this.AccountData.Configuration.FNEdit;
            this.Tbl_DLImageLink.Value = this.AccountData.Configuration.ImageLink;
            this.Tbl_DLWebUrl.Value = this.AccountData.Configuration.WebUrl;
            this.Pic_DLGG.Image = this.AccountData.GGImage;
            this.Tbl_DLID.ValueReadOnly = true;
        }

        private bool SetConfiguration()
        {
            this.AccountData.ID = this.Tbl_DLID.Value;
            this.AccountData.PW = this.Tbl_DLPW.Value;
            this.AccountData.AllowCZ = this.Ckb_AllowCZ.Checked;
            this.AccountData.AllowDelete = this.Ckb_AllowDelete.Checked;
            this.AccountData.AllowClear = this.Ckb_AllowClear.Checked;
            this.AccountData.AllowDK = this.Ckb_AllowDK.Checked;
            this.AccountData.Configuration.QQ = this.Tbl_DLQQ.Value;
            this.AccountData.Configuration.QQGroup = this.Tbl_DLQQGroup.Value;
            this.AccountData.Configuration.FNEdit = this.Tbl_DLFNEdit.Value;
            this.AccountData.Configuration.ImageLink = this.Tbl_DLImageLink.Value;
            this.AccountData.Configuration.WebUrl = this.Tbl_DLWebUrl.Value;
            this.AccountData.Configuration.LoginPTListViewString = this.Tbl_DLLoginPT.Value;
            this.AccountData.ConfigurationString = this.AccountData.Configuration.DLConfiguration;
            if (this.GGImageString != "")
            {
                this.AccountData.GGImageString = this.GGImageString;
            }
            if (this.IsAdd)
            {
                if (this.AccountData.ID == "")
                {
                    CommFunc.PublicMessageAll("输入账号不能为空！", true, MessageBoxIcon.Asterisk, "");
                    return false;
                }
                if (this.AccountData.ID.IndexOf(this.AccountData.AppCharName) != 0)
                {
                    CommFunc.PublicMessageAll($"输入账号必须是【{this.AccountData.AppCharName}】开头！", true, MessageBoxIcon.Asterisk, "");
                    return false;
                }
                if (this.AccountData.ID.Replace(this.AccountData.AppCharName, "").Length != 3)
                {
                    CommFunc.PublicMessageAll($"输入账号后缀必须是三位！例如：【{this.AccountData.AppCharName}001】", true, MessageBoxIcon.Asterisk, "");
                    return false;
                }
            }
            if (this.AccountData.PW == "")
            {
                CommFunc.PublicMessageAll("输入密码不能为空！", true, MessageBoxIcon.Asterisk, "");
                return false;
            }
            if (this.Tbl_DLLoginPT.Value == "")
            {
                CommFunc.PublicMessageAll("输入登录平台不能为空！", true, MessageBoxIcon.Asterisk, "");
                return false;
            }
            foreach (string str in this.AccountData.Configuration.LoginPTList)
            {
                if (!this.PTNameList.Contains(str))
                {
                    CommFunc.PublicMessageAll("输入的平台名称必须在可选平台范围内！", true, MessageBoxIcon.Asterisk, "");
                    return false;
                }
            }
            return true;
        }
    }
}

