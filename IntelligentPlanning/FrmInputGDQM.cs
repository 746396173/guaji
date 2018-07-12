namespace IntelligentPlanning
{
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FrmInputGDQM : ExForm
    {
        private CheckBox Ckb_Cancel;
        private CheckBox Ckb_Ok;
        private CheckBox Ckb_Type1;
        private CheckBox Ckb_Type2;
        private CheckBox Ckb_Type3;
        private CheckBox Ckb_Type4;
        private CheckBox Ckb_Type5;
        private IContainer components = null;
        private Label Lbl_KCCode;
        private Label Lbl_NumberCount;
        private Label Lbl_Random;
        private Label Lbl_Type;
        public static ConfigurationStatus.KMTM OutValue;
        public ConfigurationStatus.PlayBase PlayInfo;
        private Panel Pnl_Bottom;
        private Panel Pnl_Top;
        private RichTextBox Txt_Input;
        private TextBox Txt_KCCode;
        private List<CheckBox> TypeList = null;

        public FrmInputGDQM(ConfigurationStatus.KMTM pInput, ConfigurationStatus.PlayBase playInfo, bool pIsAdd)
        {
            this.InitializeComponent();
            List<CheckBox> list = new List<CheckBox> {
                this.Ckb_Type1,
                this.Ckb_Type2,
                this.Ckb_Type3,
                this.Ckb_Type4,
                this.Ckb_Type5
            };
            this.TypeList = list;
            List<Control> list2 = new List<Control> {
                this
            };
            base.ControlList = list2;
            List<Label> list3 = new List<Label> {
                this.Lbl_NumberCount
            };
            base.LabelList = list3;
            List<Control> list4 = new List<Control> {
                this
            };
            base.ControlList = list4;
            if (!pIsAdd)
            {
                this.SetCHTypeList(pInput.KMTMTypeList);
                this.Txt_KCCode.Text = pInput.KMTMCode;
                this.Txt_Input.Text = pInput.KMTMValue.Replace("|", "\r\n");
                CommFunc.ConvertInputText(this.Txt_Input, this.PlayInfo);
            }
            this.Text = pIsAdd ? "添加条件" : "修改条件";
            this.PlayInfo = playInfo;
            List<CheckBox> list5 = new List<CheckBox> {
                this.Ckb_Ok,
                this.Ckb_Cancel
            };
            base.CheckBoxList = list5;
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
                    this.Pnl_Top
                };
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Lbl_Type,
                    this.Lbl_KCCode,
                    this.Lbl_Random,
                    this.Ckb_Type1,
                    this.Ckb_Type2,
                    this.Ckb_Type3,
                    this.Ckb_Type4,
                    this.Ckb_Type5,
                    this.Ckb_Ok,
                    this.Ckb_Cancel
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
            }
        }

        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            this.Txt_Input.Text = "";
        }

        private void Ckb_Cancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void Ckb_Ok_Click(object sender, EventArgs e)
        {
            string pErrorHint = "";
            List<string> list = CommFunc.FilterNumber(CommFunc.ReplaceText(this.Txt_Input.Text, this.PlayInfo, true), this.PlayInfo.CodeCount, this.PlayInfo.Play, ref pErrorHint);
            if (pErrorHint != "")
            {
                CommFunc.PublicMessageAll(pErrorHint, true, MessageBoxIcon.Asterisk, "");
            }
            else
            {
                OutValue = new ConfigurationStatus.KMTM();
                OutValue.KMTMTypeList = this.GetCHTypeList();
                OutValue.KMTMCode = this.Txt_KCCode.Text;
                OutValue.KMTMValue = this.Txt_Input.Text.Replace("\r\n", "|");
                if (OutValue.KMTMValue == "")
                {
                    CommFunc.PublicMessageAll("请输入号码！", true, MessageBoxIcon.Asterisk, "");
                    this.Txt_Input.Focus();
                }
                else
                {
                    base.DialogResult = DialogResult.OK;
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

        private void FrmInputGDQM_Load(object sender, EventArgs e)
        {
            this.BeautifyInterface();
            this.Txt_Input_TextChanged(null, null);
            this.Txt_Input.Select(this.Txt_Input.TextLength, 1);
            this.Txt_Input.Focus();
        }

        private List<int> GetCHTypeList()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < this.TypeList.Count; i++)
            {
                if (this.TypeList[i].Checked)
                {
                    list.Add(AppInfo.FiveDic[this.TypeList[i].Text]);
                }
            }
            return list;
        }

        private void InitializeComponent()
        {
            this.Pnl_Bottom = new Panel();
            this.Ckb_Cancel = new CheckBox();
            this.Ckb_Ok = new CheckBox();
            this.Txt_KCCode = new TextBox();
            this.Txt_Input = new RichTextBox();
            this.Lbl_NumberCount = new Label();
            this.Ckb_Type5 = new CheckBox();
            this.Lbl_KCCode = new Label();
            this.Ckb_Type4 = new CheckBox();
            this.Lbl_Type = new Label();
            this.Ckb_Type3 = new CheckBox();
            this.Ckb_Type1 = new CheckBox();
            this.Ckb_Type2 = new CheckBox();
            this.Lbl_Random = new Label();
            this.Pnl_Top = new Panel();
            this.Pnl_Bottom.SuspendLayout();
            this.Pnl_Top.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_Bottom.Controls.Add(this.Ckb_Cancel);
            this.Pnl_Bottom.Controls.Add(this.Ckb_Ok);
            this.Pnl_Bottom.Dock = DockStyle.Bottom;
            this.Pnl_Bottom.Location = new Point(0, 0x100);
            this.Pnl_Bottom.Name = "Pnl_Bottom";
            this.Pnl_Bottom.Size = new Size(0x13d, 0x23);
            this.Pnl_Bottom.TabIndex = 10;
            this.Ckb_Cancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.Ckb_Cancel.Appearance = Appearance.Button;
            this.Ckb_Cancel.AutoCheck = false;
            this.Ckb_Cancel.FlatAppearance.BorderSize = 0;
            this.Ckb_Cancel.FlatStyle = FlatStyle.Flat;
            this.Ckb_Cancel.Image = Resources.CancelRound;
            this.Ckb_Cancel.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Cancel.Location = new Point(0xf9, 5);
            this.Ckb_Cancel.Name = "Ckb_Cancel";
            this.Ckb_Cancel.Size = new Size(60, 0x19);
            this.Ckb_Cancel.TabIndex = 0xa1;
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
            this.Ckb_Ok.Location = new Point(0xb7, 5);
            this.Ckb_Ok.Name = "Ckb_Ok";
            this.Ckb_Ok.Size = new Size(60, 0x19);
            this.Ckb_Ok.TabIndex = 160;
            this.Ckb_Ok.Text = "确定";
            this.Ckb_Ok.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Ok.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Ok.UseVisualStyleBackColor = true;
            this.Ckb_Ok.Click += new EventHandler(this.Ckb_Ok_Click);
            this.Txt_KCCode.Location = new Point(80, 40);
            this.Txt_KCCode.Name = "Txt_KCCode";
            this.Txt_KCCode.Size = new Size(0xe5, 0x17);
            this.Txt_KCCode.TabIndex = 330;
            this.Txt_KCCode.Text = "1-2";
            this.Txt_Input.Location = new Point(8, 0x67);
            this.Txt_Input.Name = "Txt_Input";
            this.Txt_Input.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.Txt_Input.Size = new Size(0x12d, 0x91);
            this.Txt_Input.TabIndex = 0x14c;
            this.Txt_Input.Text = "";
            this.Txt_Input.TextChanged += new EventHandler(this.Txt_Input_TextChanged);
            this.Lbl_NumberCount.AutoSize = true;
            this.Lbl_NumberCount.Location = new Point(0x4a, 0x4e);
            this.Lbl_NumberCount.Name = "Lbl_NumberCount";
            this.Lbl_NumberCount.Size = new Size(0x16, 0x11);
            this.Lbl_NumberCount.TabIndex = 0x14d;
            this.Lbl_NumberCount.Text = "00";
            this.Ckb_Type5.AutoSize = true;
            this.Ckb_Type5.Checked = true;
            this.Ckb_Type5.CheckState = CheckState.Checked;
            this.Ckb_Type5.Location = new Point(0x102, 7);
            this.Ckb_Type5.Name = "Ckb_Type5";
            this.Ckb_Type5.Size = new Size(0x27, 0x15);
            this.Ckb_Type5.TabIndex = 0x153;
            this.Ckb_Type5.Text = "个";
            this.Ckb_Type5.UseVisualStyleBackColor = true;
            this.Lbl_KCCode.AutoSize = true;
            this.Lbl_KCCode.Location = new Point(5, 0x2b);
            this.Lbl_KCCode.Name = "Lbl_KCCode";
            this.Lbl_KCCode.Size = new Size(0x44, 0x11);
            this.Lbl_KCCode.TabIndex = 0x149;
            this.Lbl_KCCode.Text = "开出号码：";
            this.Ckb_Type4.AutoSize = true;
            this.Ckb_Type4.Checked = true;
            this.Ckb_Type4.CheckState = CheckState.Checked;
            this.Ckb_Type4.Location = new Point(0xd5, 7);
            this.Ckb_Type4.Name = "Ckb_Type4";
            this.Ckb_Type4.Size = new Size(0x27, 0x15);
            this.Ckb_Type4.TabIndex = 0x152;
            this.Ckb_Type4.Text = "十";
            this.Ckb_Type4.UseVisualStyleBackColor = true;
            this.Lbl_Type.AutoSize = true;
            this.Lbl_Type.Location = new Point(5, 8);
            this.Lbl_Type.Name = "Lbl_Type";
            this.Lbl_Type.Size = new Size(0x44, 0x11);
            this.Lbl_Type.TabIndex = 0x14e;
            this.Lbl_Type.Text = "开出位置：";
            this.Ckb_Type3.AutoSize = true;
            this.Ckb_Type3.Checked = true;
            this.Ckb_Type3.CheckState = CheckState.Checked;
            this.Ckb_Type3.Location = new Point(0xa8, 7);
            this.Ckb_Type3.Name = "Ckb_Type3";
            this.Ckb_Type3.Size = new Size(0x27, 0x15);
            this.Ckb_Type3.TabIndex = 0x151;
            this.Ckb_Type3.Text = "百";
            this.Ckb_Type3.UseVisualStyleBackColor = true;
            this.Ckb_Type1.AutoSize = true;
            this.Ckb_Type1.Checked = true;
            this.Ckb_Type1.CheckState = CheckState.Checked;
            this.Ckb_Type1.Location = new Point(0x4e, 7);
            this.Ckb_Type1.Name = "Ckb_Type1";
            this.Ckb_Type1.Size = new Size(0x27, 0x15);
            this.Ckb_Type1.TabIndex = 0x14f;
            this.Ckb_Type1.Text = "万";
            this.Ckb_Type1.UseVisualStyleBackColor = true;
            this.Ckb_Type2.AutoSize = true;
            this.Ckb_Type2.Checked = true;
            this.Ckb_Type2.CheckState = CheckState.Checked;
            this.Ckb_Type2.Location = new Point(0x7b, 7);
            this.Ckb_Type2.Name = "Ckb_Type2";
            this.Ckb_Type2.Size = new Size(0x27, 0x15);
            this.Ckb_Type2.TabIndex = 0x150;
            this.Ckb_Type2.Text = "千";
            this.Ckb_Type2.UseVisualStyleBackColor = true;
            this.Lbl_Random.AutoSize = true;
            this.Lbl_Random.Location = new Point(5, 0x4e);
            this.Lbl_Random.Name = "Lbl_Random";
            this.Lbl_Random.Size = new Size(0x44, 0x11);
            this.Lbl_Random.TabIndex = 0x14b;
            this.Lbl_Random.Text = "输入号码：";
            this.Pnl_Top.Controls.Add(this.Lbl_Type);
            this.Pnl_Top.Controls.Add(this.Txt_KCCode);
            this.Pnl_Top.Controls.Add(this.Lbl_Random);
            this.Pnl_Top.Controls.Add(this.Txt_Input);
            this.Pnl_Top.Controls.Add(this.Ckb_Type2);
            this.Pnl_Top.Controls.Add(this.Lbl_NumberCount);
            this.Pnl_Top.Controls.Add(this.Ckb_Type1);
            this.Pnl_Top.Controls.Add(this.Ckb_Type5);
            this.Pnl_Top.Controls.Add(this.Ckb_Type3);
            this.Pnl_Top.Controls.Add(this.Lbl_KCCode);
            this.Pnl_Top.Controls.Add(this.Ckb_Type4);
            this.Pnl_Top.Dock = DockStyle.Fill;
            this.Pnl_Top.Location = new Point(0, 0);
            this.Pnl_Top.Name = "Pnl_Top";
            this.Pnl_Top.Size = new Size(0x13d, 0x100);
            this.Pnl_Top.TabIndex = 0x98;
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x13d, 0x123);
            base.Controls.Add(this.Pnl_Top);
            base.Controls.Add(this.Pnl_Bottom);
            base.MaximizeBox = true;
            base.MinimizeBox = true;
            base.Name = "FrmInputGDQM";
            base.Load += new EventHandler(this.FrmInputGDQM_Load);
            this.Pnl_Bottom.ResumeLayout(false);
            this.Pnl_Top.ResumeLayout(false);
            this.Pnl_Top.PerformLayout();
            base.ResumeLayout(false);
        }

        private void RefreshNumCount()
        {
            string pErrorHint = "";
            int num = CommFunc.GetBetsCodeCount(CommFunc.FilterNumber(CommFunc.ReplaceText(this.Txt_Input.Text, this.PlayInfo, true), this.PlayInfo.CodeCount, this.PlayInfo.Play, ref pErrorHint), this.PlayInfo.Play, null);
            this.Lbl_NumberCount.Text = $"{num} 注";
            this.Ckb_Ok.Enabled = num > 0;
        }

        private void SetCHTypeList(List<int> pValueList)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < this.TypeList.Count; i++)
            {
                this.TypeList[i].Checked = pValueList.Contains(AppInfo.FiveDic[this.TypeList[i].Text]);
            }
        }

        private void Txt_Input_TextChanged(object sender, EventArgs e)
        {
            this.RefreshNumCount();
        }
    }
}

