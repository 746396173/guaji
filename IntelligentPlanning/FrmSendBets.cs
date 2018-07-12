namespace IntelligentPlanning
{
    using IntelligentPlanning.ExDataGridView;
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FrmSendBets : ExForm
    {
        private CheckBox Ckb_AddFollowUser;
        private CheckBox Ckb_Cancel;
        private CheckBox Ckb_ClearFollowUser;
        private CheckBox Ckb_DeleteFollowUser;
        private CheckBox Ckb_Help;
        private CheckBox Ckb_Ok;
        private CheckBox Ckb_ShareCopy;
        private IContainer components = null;
        private ExpandGirdView Egv_FollowUserList;
        private List<string> FollowUserList = new List<string>();
        public bool IsShareBets;
        private Label Lbl_Value;
        private Panel Pnl_Bottom;
        private Panel Pnl_FollowUserList;
        private Panel Pnl_Main;
        private Panel Pnl_PTRefresh;
        private TextBox Txt_ShareCopy;

        public FrmSendBets(bool pIsShareBets)
        {
            this.InitializeComponent();
            this.IsShareBets = pIsShareBets;
            List<CheckBox> list = new List<CheckBox> {
                this.Ckb_Ok,
                this.Ckb_Cancel,
                this.Ckb_ShareCopy,
                this.Ckb_AddFollowUser,
                this.Ckb_DeleteFollowUser,
                this.Ckb_ClearFollowUser,
                this.Ckb_Help
            };
            base.CheckBoxList = list;
            this.Text = $"共享{this.IsShareBets} ? 投注:方案 管理";
        }

        private void BeautifyInterface()
        {
            if (AppInfo.Account.Configuration.Beautify)
            {
                List<Control> pControlList = new List<Control> {
                    this.Pnl_Bottom
                };
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list2 = new List<Control> {
                    this.Pnl_Main
                };
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Ckb_Ok,
                    this.Ckb_Cancel,
                    this.Ckb_ShareCopy,
                    this.Ckb_AddFollowUser,
                    this.Ckb_DeleteFollowUser,
                    this.Ckb_ClearFollowUser,
                    this.Lbl_Value,
                    this.Ckb_Help
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
            }
        }

        private void Ckb_AddFollowUser_Click(object sender, EventArgs e)
        {
            string item = "";
            while (true)
            {
                item = CommFunc.GetStringFromInputBox($"请输入下级的{this.UserIDString}账号", "", "", false);
                if (item == "")
                {
                    return;
                }
                if (!this.FollowUserList.Contains(item))
                {
                    this.FollowUserList.Add(item);
                    this.RefreshFollowUserList();
                    return;
                }
                CommFunc.PublicMessageAll($"下级【{item}】已存在，请重新输入！", true, MessageBoxIcon.Asterisk, "");
            }
        }

        private void Ckb_Cancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.No;
        }

        private void Ckb_ClearFollowUser_Click(object sender, EventArgs e)
        {
            if (CommFunc.AgreeMessage("是否要清空当前全部下级", false, MessageBoxIcon.Asterisk, ""))
            {
                this.FollowUserList.Clear();
                this.RefreshFollowUserList();
            }
        }

        private void Ckb_DeleteFollowUser_Click(object sender, EventArgs e)
        {
            string userID = this.GetUserID();
            if (userID != "")
            {
                this.FollowUserList.Remove(userID);
                this.RefreshFollowUserList();
            }
        }

        private void Ckb_Help_Click(object sender, EventArgs e)
        {
            if (this.IsShareBets)
            {
                CommFunc.PublicMessageAll($"添加需要共享计划投注的下级{this.UserIDString}账号，再复制共享码发给下级，下级即可跟单投注", false, MessageBoxIcon.Asterisk, "");
            }
            else
            {
                CommFunc.PublicMessageAll($"添加需要共享方案的下级{this.UserIDString}账号，再复制共享码发给下级，下级即可下载方案", false, MessageBoxIcon.Asterisk, "");
            }
        }

        private void Ckb_Ok_Click(object sender, EventArgs e)
        {
            if (!this.UpdateFollowUserList())
            {
                CommFunc.PublicMessageAll("更新下级列表失败，请重试！", true, MessageBoxIcon.Asterisk, "");
            }
            else
            {
                base.DialogResult = DialogResult.Yes;
            }
        }

        private void Ckb_ShareCopy_Click(object sender, EventArgs e)
        {
            CommFunc.CopyText(this.Txt_ShareCopy.Text.Trim());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Egv_FollowUserList_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (((this.Egv_FollowUserList.RowCount != 0) && (this.FollowUserList.Count != 0)) && (e.RowIndex < this.FollowUserList.Count))
            {
                e.Value = this.FollowUserList[e.RowIndex];
            }
        }

        private void FrmSendBets_Load(object sender, EventArgs e)
        {
            this.BeautifyInterface();
            this.LoadFollowUserList();
            this.GetFollowUserList();
            this.Txt_ShareCopy.Text = ConfigurationStatus.ShareBets.GetEncodeShareCode("");
            this.Txt_ShareCopy.SelectAll();
            this.Txt_ShareCopy.Focus();
        }

        private void GetFollowUserList()
        {
            string followUser = SQLData.GetFollowUser();
            if (followUser != "")
            {
                this.FollowUserList = CommFunc.SplitString(followUser, ",", -1);
            }
            this.RefreshFollowUserList();
        }

        private string GetUserID()
        {
            string str = "";
            if (this.Egv_FollowUserList.SelectedRows.Count != 0)
            {
                str = this.Egv_FollowUserList.SelectedRows[0].Cells[0].Value.ToString();
            }
            return str;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            DataGridViewCellStyle style3 = new DataGridViewCellStyle();
            this.Pnl_Main = new Panel();
            this.Pnl_FollowUserList = new Panel();
            this.Egv_FollowUserList = new ExpandGirdView(this.components);
            this.Pnl_PTRefresh = new Panel();
            this.Ckb_Help = new CheckBox();
            this.Ckb_ClearFollowUser = new CheckBox();
            this.Ckb_DeleteFollowUser = new CheckBox();
            this.Ckb_AddFollowUser = new CheckBox();
            this.Ckb_ShareCopy = new CheckBox();
            this.Txt_ShareCopy = new TextBox();
            this.Lbl_Value = new Label();
            this.Pnl_Bottom = new Panel();
            this.Ckb_Cancel = new CheckBox();
            this.Ckb_Ok = new CheckBox();
            this.Pnl_Main.SuspendLayout();
            this.Pnl_FollowUserList.SuspendLayout();
            ((ISupportInitialize) this.Egv_FollowUserList).BeginInit();
            this.Pnl_PTRefresh.SuspendLayout();
            this.Pnl_Bottom.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_Main.BackColor = SystemColors.Control;
            this.Pnl_Main.Controls.Add(this.Pnl_FollowUserList);
            this.Pnl_Main.Controls.Add(this.Ckb_ShareCopy);
            this.Pnl_Main.Controls.Add(this.Txt_ShareCopy);
            this.Pnl_Main.Controls.Add(this.Lbl_Value);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(370, 0x175);
            this.Pnl_Main.TabIndex = 12;
            this.Pnl_FollowUserList.Controls.Add(this.Egv_FollowUserList);
            this.Pnl_FollowUserList.Controls.Add(this.Pnl_PTRefresh);
            this.Pnl_FollowUserList.Dock = DockStyle.Top;
            this.Pnl_FollowUserList.Location = new Point(0, 0);
            this.Pnl_FollowUserList.Name = "Pnl_FollowUserList";
            this.Pnl_FollowUserList.Size = new Size(370, 0x148);
            this.Pnl_FollowUserList.TabIndex = 0x139;
            this.Egv_FollowUserList.AllowUserToAddRows = false;
            this.Egv_FollowUserList.AllowUserToDeleteRows = false;
            this.Egv_FollowUserList.AllowUserToResizeColumns = false;
            this.Egv_FollowUserList.AllowUserToResizeRows = false;
            this.Egv_FollowUserList.BackgroundColor = SystemColors.Control;
            this.Egv_FollowUserList.BorderStyle = BorderStyle.Fixed3D;
            this.Egv_FollowUserList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style.BackColor = SystemColors.Window;
            style.Font = new Font("微软雅黑", 9f);
            style.ForeColor = SystemColors.ControlText;
            style.SelectionBackColor = SystemColors.Highlight;
            style.SelectionForeColor = SystemColors.HighlightText;
            style.WrapMode = DataGridViewTriState.False;
            this.Egv_FollowUserList.ColumnHeadersDefaultCellStyle = style;
            this.Egv_FollowUserList.ColumnHeadersHeight = 30;
            this.Egv_FollowUserList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            style2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style2.BackColor = SystemColors.Control;
            style2.Font = new Font("微软雅黑", 9f);
            style2.ForeColor = SystemColors.ControlText;
            style2.SelectionBackColor = Color.SteelBlue;
            style2.SelectionForeColor = Color.White;
            style2.WrapMode = DataGridViewTriState.False;
            this.Egv_FollowUserList.DefaultCellStyle = style2;
            this.Egv_FollowUserList.Dock = DockStyle.Fill;
            this.Egv_FollowUserList.DragLineColor = Color.Silver;
            this.Egv_FollowUserList.ExternalVirtualMode = true;
            this.Egv_FollowUserList.GridColor = Color.Silver;
            this.Egv_FollowUserList.HeadersCheckDefult = CheckState.Checked;
            this.Egv_FollowUserList.Location = new Point(0, 0x23);
            this.Egv_FollowUserList.MergeColumnHeaderBackColor = SystemColors.Control;
            this.Egv_FollowUserList.MultiSelect = false;
            this.Egv_FollowUserList.Name = "Egv_FollowUserList";
            this.Egv_FollowUserList.RowHeadersVisible = false;
            this.Egv_FollowUserList.RowNum = 0x11;
            style3.BackColor = SystemColors.Control;
            style3.SelectionBackColor = Color.FromArgb(0xde, 0xe8, 0xfc);
            style3.SelectionForeColor = Color.Black;
            this.Egv_FollowUserList.RowsDefaultCellStyle = style3;
            this.Egv_FollowUserList.RowTemplate.Height = 0x17;
            this.Egv_FollowUserList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Egv_FollowUserList.Size = new Size(370, 0x125);
            this.Egv_FollowUserList.TabIndex = 0x49;
            this.Pnl_PTRefresh.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_PTRefresh.Controls.Add(this.Ckb_Help);
            this.Pnl_PTRefresh.Controls.Add(this.Ckb_ClearFollowUser);
            this.Pnl_PTRefresh.Controls.Add(this.Ckb_DeleteFollowUser);
            this.Pnl_PTRefresh.Controls.Add(this.Ckb_AddFollowUser);
            this.Pnl_PTRefresh.Dock = DockStyle.Top;
            this.Pnl_PTRefresh.Location = new Point(0, 0);
            this.Pnl_PTRefresh.Name = "Pnl_PTRefresh";
            this.Pnl_PTRefresh.Size = new Size(370, 0x23);
            this.Pnl_PTRefresh.TabIndex = 0x4a;
            this.Ckb_Help.Appearance = Appearance.Button;
            this.Ckb_Help.AutoCheck = false;
            this.Ckb_Help.FlatAppearance.BorderSize = 0;
            this.Ckb_Help.FlatStyle = FlatStyle.Flat;
            this.Ckb_Help.Image = Resources.Help1;
            this.Ckb_Help.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Help.Location = new Point(0x12d, 4);
            this.Ckb_Help.Name = "Ckb_Help";
            this.Ckb_Help.Size = new Size(60, 0x19);
            this.Ckb_Help.TabIndex = 0x137;
            this.Ckb_Help.Text = "帮助";
            this.Ckb_Help.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Help.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_Help.UseVisualStyleBackColor = true;
            this.Ckb_Help.Click += new EventHandler(this.Ckb_Help_Click);
            this.Ckb_ClearFollowUser.Appearance = Appearance.Button;
            this.Ckb_ClearFollowUser.AutoCheck = false;
            this.Ckb_ClearFollowUser.FlatAppearance.BorderSize = 0;
            this.Ckb_ClearFollowUser.FlatStyle = FlatStyle.Flat;
            this.Ckb_ClearFollowUser.Image = Resources.ClearAll;
            this.Ckb_ClearFollowUser.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_ClearFollowUser.Location = new Point(0x8a, 4);
            this.Ckb_ClearFollowUser.Name = "Ckb_ClearFollowUser";
            this.Ckb_ClearFollowUser.Size = new Size(60, 0x19);
            this.Ckb_ClearFollowUser.TabIndex = 310;
            this.Ckb_ClearFollowUser.Text = "清空";
            this.Ckb_ClearFollowUser.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_ClearFollowUser.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_ClearFollowUser.UseVisualStyleBackColor = true;
            this.Ckb_ClearFollowUser.Click += new EventHandler(this.Ckb_ClearFollowUser_Click);
            this.Ckb_DeleteFollowUser.Appearance = Appearance.Button;
            this.Ckb_DeleteFollowUser.AutoCheck = false;
            this.Ckb_DeleteFollowUser.FlatAppearance.BorderSize = 0;
            this.Ckb_DeleteFollowUser.FlatStyle = FlatStyle.Flat;
            this.Ckb_DeleteFollowUser.Image = Resources.Remove;
            this.Ckb_DeleteFollowUser.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_DeleteFollowUser.Location = new Point(0x47, 4);
            this.Ckb_DeleteFollowUser.Name = "Ckb_DeleteFollowUser";
            this.Ckb_DeleteFollowUser.Size = new Size(60, 0x19);
            this.Ckb_DeleteFollowUser.TabIndex = 0xd3;
            this.Ckb_DeleteFollowUser.Text = "删除";
            this.Ckb_DeleteFollowUser.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_DeleteFollowUser.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_DeleteFollowUser.UseVisualStyleBackColor = true;
            this.Ckb_DeleteFollowUser.Click += new EventHandler(this.Ckb_DeleteFollowUser_Click);
            this.Ckb_AddFollowUser.Appearance = Appearance.Button;
            this.Ckb_AddFollowUser.AutoCheck = false;
            this.Ckb_AddFollowUser.FlatAppearance.BorderSize = 0;
            this.Ckb_AddFollowUser.FlatStyle = FlatStyle.Flat;
            this.Ckb_AddFollowUser.Image = Resources.Add;
            this.Ckb_AddFollowUser.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_AddFollowUser.Location = new Point(4, 4);
            this.Ckb_AddFollowUser.Name = "Ckb_AddFollowUser";
            this.Ckb_AddFollowUser.Size = new Size(60, 0x19);
            this.Ckb_AddFollowUser.TabIndex = 210;
            this.Ckb_AddFollowUser.Text = "添加";
            this.Ckb_AddFollowUser.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_AddFollowUser.TextImageRelation = TextImageRelation.ImageAboveText;
            this.Ckb_AddFollowUser.UseVisualStyleBackColor = true;
            this.Ckb_AddFollowUser.Click += new EventHandler(this.Ckb_AddFollowUser_Click);
            this.Ckb_ShareCopy.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Ckb_ShareCopy.Appearance = Appearance.Button;
            this.Ckb_ShareCopy.AutoCheck = false;
            this.Ckb_ShareCopy.FlatAppearance.BorderSize = 0;
            this.Ckb_ShareCopy.FlatStyle = FlatStyle.Flat;
            this.Ckb_ShareCopy.Image = Resources.Copy;
            this.Ckb_ShareCopy.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_ShareCopy.Location = new Point(0x12f, 0x151);
            this.Ckb_ShareCopy.Name = "Ckb_ShareCopy";
            this.Ckb_ShareCopy.Size = new Size(60, 0x19);
            this.Ckb_ShareCopy.TabIndex = 0x138;
            this.Ckb_ShareCopy.Text = "复制";
            this.Ckb_ShareCopy.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_ShareCopy.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_ShareCopy.UseVisualStyleBackColor = true;
            this.Ckb_ShareCopy.Click += new EventHandler(this.Ckb_ShareCopy_Click);
            this.Txt_ShareCopy.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Txt_ShareCopy.Location = new Point(70, 0x152);
            this.Txt_ShareCopy.Name = "Txt_ShareCopy";
            this.Txt_ShareCopy.ReadOnly = true;
            this.Txt_ShareCopy.Size = new Size(0xe3, 0x17);
            this.Txt_ShareCopy.TabIndex = 1;
            this.Lbl_Value.AutoSize = true;
            this.Lbl_Value.Location = new Point(10, 0x155);
            this.Lbl_Value.Name = "Lbl_Value";
            this.Lbl_Value.Size = new Size(0x38, 0x11);
            this.Lbl_Value.TabIndex = 2;
            this.Lbl_Value.Text = "共享码：";
            this.Pnl_Bottom.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_Bottom.Controls.Add(this.Ckb_Cancel);
            this.Pnl_Bottom.Controls.Add(this.Ckb_Ok);
            this.Pnl_Bottom.Dock = DockStyle.Bottom;
            this.Pnl_Bottom.Location = new Point(0, 0x175);
            this.Pnl_Bottom.Name = "Pnl_Bottom";
            this.Pnl_Bottom.Size = new Size(370, 0x23);
            this.Pnl_Bottom.TabIndex = 11;
            this.Ckb_Cancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Ckb_Cancel.Appearance = Appearance.Button;
            this.Ckb_Cancel.AutoCheck = false;
            this.Ckb_Cancel.FlatAppearance.BorderSize = 0;
            this.Ckb_Cancel.FlatStyle = FlatStyle.Flat;
            this.Ckb_Cancel.Image = Resources.CancelRound;
            this.Ckb_Cancel.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Cancel.Location = new Point(0x12d, 3);
            this.Ckb_Cancel.Name = "Ckb_Cancel";
            this.Ckb_Cancel.Size = new Size(60, 0x19);
            this.Ckb_Cancel.TabIndex = 0x9d;
            this.Ckb_Cancel.Text = "取消";
            this.Ckb_Cancel.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Cancel.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Cancel.UseVisualStyleBackColor = true;
            this.Ckb_Cancel.Click += new EventHandler(this.Ckb_Cancel_Click);
            this.Ckb_Ok.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Ckb_Ok.Appearance = Appearance.Button;
            this.Ckb_Ok.AutoCheck = false;
            this.Ckb_Ok.FlatAppearance.BorderSize = 0;
            this.Ckb_Ok.FlatStyle = FlatStyle.Flat;
            this.Ckb_Ok.Image = Resources.OkRound;
            this.Ckb_Ok.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Ok.Location = new Point(0xeb, 3);
            this.Ckb_Ok.Name = "Ckb_Ok";
            this.Ckb_Ok.Size = new Size(60, 0x19);
            this.Ckb_Ok.TabIndex = 0x9c;
            this.Ckb_Ok.Text = "确定";
            this.Ckb_Ok.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Ok.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Ok.UseVisualStyleBackColor = true;
            this.Ckb_Ok.Click += new EventHandler(this.Ckb_Ok_Click);
            base.AutoScaleDimensions = new SizeF(96f, 96f);
            base.AutoScaleMode = AutoScaleMode.Dpi;
            base.ClientSize = new Size(370, 0x198);
            base.Controls.Add(this.Pnl_Main);
            base.Controls.Add(this.Pnl_Bottom);
            base.Name = "FrmSendBets";
            base.Load += new EventHandler(this.FrmSendBets_Load);
            this.Pnl_Main.ResumeLayout(false);
            this.Pnl_Main.PerformLayout();
            this.Pnl_FollowUserList.ResumeLayout(false);
            ((ISupportInitialize) this.Egv_FollowUserList).EndInit();
            this.Pnl_PTRefresh.ResumeLayout(false);
            this.Pnl_Bottom.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void LoadFollowUserList()
        {
            List<int> pType = new List<int> { 1 };
            List<string> pText = new List<string> { "下级列表" };
            List<int> pWidth = new List<int> { 10 };
            List<bool> pRead = new List<bool> { true };
            List<bool> pVis = new List<bool> { true };
            this.Egv_FollowUserList.LoadInitialization(pType, pText, pWidth, pRead, pVis, null);
            this.Egv_FollowUserList.MultiSelect = false;
            this.Egv_FollowUserList.VirtualMode = true;
            CommFunc.SetExpandGirdViewFormat(this.Egv_FollowUserList, 9);
            this.Egv_FollowUserList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Egv_FollowUserList.CellValueNeeded += new DataGridViewCellValueEventHandler(this.Egv_FollowUserList_CellValueNeeded);
            for (int i = 0; i < this.Egv_FollowUserList.ColumnCount; i++)
            {
                this.Egv_FollowUserList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.Egv_FollowUserList.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void RefreshControl()
        {
            int count = this.FollowUserList.Count;
            this.Ckb_DeleteFollowUser.Enabled = count > 0;
            this.Ckb_ClearFollowUser.Enabled = count > 0;
        }

        private void RefreshFollowUserList()
        {
            CommFunc.RefreshDataGridView(this.Egv_FollowUserList, this.FollowUserList.Count, false);
            this.RefreshControl();
        }

        private bool UpdateFollowUserList() => 
            SQLData.UpdateFollowUser(CommFunc.Join(this.FollowUserList, ","));

        public string UserIDString =>
            (AppInfo.IsViewLogin ? "软件" : "平台");
    }
}

