namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning;
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class HJFGCount : UserControl
    {
        private List<CheckBox> CheckBoxList = null;
        private CheckBox Ckb_Clear;
        private CheckBox Ckb_CodeYZ;
        private CheckBox Ckb_Copy;
        private CheckBox Ckb_OpenPath;
        private CheckBox Ckb_Paste;
        private CheckBox Ckb_SplitValue;
        private IContainer components = null;
        private List<Control> ControlList = null;
        private GroupBox Grp_DT;
        private GroupBox Grp_Type;
        private GroupBox Grp_Value;
        private Label Lbl_CodeYZ;
        private Panel Pnl_DTTop;
        private Panel Pnl_Left;
        private Panel Pnl_Main;
        private Panel Pnl_Right;
        private RadioButton Rdb_Type1;
        private RadioButton Rdb_Type2;
        private RadioButton Rdb_Type3;
        private RadioButton Rdb_Type4;
        private RadioButton Rdb_Type5;
        private RadioButton Rdb_Type6;
        private RadioButton Rdb_Type7;
        private RadioButton Rdb_Type8;
        private string RegConfigPath = "";
        private List<Control> SpecialControlList = null;
        private ToolTip Tot_Hint;
        private TextBox Txt_CodeYZ;
        private RichTextBox Txt_Number;
        private List<RadioButton> TypeList = null;
        private List<ValueLable> ValueLableList = null;
        private ValueLable VL_Value1;
        private ValueLable VL_Value10;
        private ValueLable VL_Value2;
        private ValueLable VL_Value3;
        private ValueLable VL_Value4;
        private ValueLable VL_Value5;
        private ValueLable VL_Value6;
        private ValueLable VL_Value7;
        private ValueLable VL_Value8;
        private ValueLable VL_Value9;

        public HJFGCount()
        {
            this.InitializeComponent();
            List<RadioButton> list = new List<RadioButton> {
                this.Rdb_Type1,
                this.Rdb_Type2,
                this.Rdb_Type3,
                this.Rdb_Type4,
                this.Rdb_Type5,
                this.Rdb_Type6,
                this.Rdb_Type7,
                this.Rdb_Type8
            };
            this.TypeList = list;
            List<ValueLable> list2 = new List<ValueLable> {
                this.VL_Value1,
                this.VL_Value2,
                this.VL_Value3,
                this.VL_Value4,
                this.VL_Value5,
                this.VL_Value6,
                this.VL_Value7,
                this.VL_Value8,
                this.VL_Value9,
                this.VL_Value10
            };
            this.ValueLableList = list2;
        }

        private void BeautifyInterface()
        {
            if (AppInfo.Account.Configuration.Beautify)
            {
                List<Control> pControlList = new List<Control>();
                CommFunc.SetControlBackColor(pControlList, AppInfo.appBackColor);
                List<Control> list2 = new List<Control> {
                    this.Pnl_Main
                };
                CommFunc.SetControlBackColor(list2, AppInfo.beaBackColor);
                List<Control> list3 = new List<Control> {
                    this.Grp_DT,
                    this.Grp_Type,
                    this.Grp_Value,
                    this.Ckb_Copy,
                    this.Ckb_Paste,
                    this.Ckb_Clear,
                    this.Ckb_CodeYZ,
                    this.Ckb_SplitValue,
                    this.Ckb_OpenPath,
                    this.Lbl_CodeYZ,
                    this.Rdb_Type1,
                    this.Rdb_Type2,
                    this.Rdb_Type3,
                    this.Rdb_Type4,
                    this.Rdb_Type5,
                    this.Rdb_Type6,
                    this.Rdb_Type7,
                    this.Rdb_Type8
                };
                CommFunc.SetControlForeColor(list3, AppInfo.whiteColor);
                List<Control> list4 = new List<Control>();
                CommFunc.SetControlForeColor(list4, AppInfo.appForeColor);
            }
        }

        private void Ckb_Clear_Click(object sender, EventArgs e)
        {
            this.Txt_Number.Text = "";
            this.Txt_Number.Focus();
        }

        private void Ckb_CodeYZ_Click(object sender, EventArgs e)
        {
            if (!CommFunc.CheckTextBoxIsNull(this.Txt_CodeYZ, "验证号码"))
            {
                string item = this.Txt_CodeYZ.Text.Trim();
                List<string> pList = new List<string>();
                int num = 0;
                for (int i = 0; i < this.ValueLableList.Count; i++)
                {
                    ValueLable lable = this.ValueLableList[i];
                    if (lable.Visible)
                    {
                        num++;
                        if (!lable.ValueList.Contains(item))
                        {
                            pList.Add(CommFunc.GetNumber(lable.Hint.Split(new char[] { '组' })[0]));
                        }
                    }
                }
                if (num == 0)
                {
                    CommFunc.PublicMessageAll("没有找到待验证的分割结果！", true, MessageBoxIcon.Asterisk, "");
                }
                else if (pList.Count > 0)
                {
                    CommFunc.PublicMessageAll("以下组数没有找到验证的号码：\r\n" + CommFunc.Join(pList, ","), true, MessageBoxIcon.Asterisk, "");
                }
                else
                {
                    CommFunc.PublicMessageAll("验证的号码都在分割结果里！", true, MessageBoxIcon.Asterisk, "");
                }
            }
        }

        private void Ckb_Copy_Click(object sender, EventArgs e)
        {
            CommFunc.CopyText(this.Txt_Number.Text);
            this.Txt_Number.Focus();
        }

        private void Ckb_OpenPath_Click(object sender, EventArgs e)
        {
            CommFunc.OpenDirectory(this.HJFGPath);
        }

        private void Ckb_Paste_Click(object sender, EventArgs e)
        {
            IDataObject dataObject = Clipboard.GetDataObject();
            if (dataObject.GetDataPresent(DataFormats.Text))
            {
                this.Txt_Number.Text = dataObject.GetData(DataFormats.Text).ToString();
            }
            this.Txt_Number.SelectAll();
            this.Txt_Number.Focus();
        }

        private void Ckb_SplitValue_Click(object sender, EventArgs e)
        {
            int pCount = Convert.ToInt32(this.GetSelectType().Split(new char[] { '中' })[0]);
            List<List<string>> list = this.SplitValueMain(pCount);
            if (list.Count == 0)
            {
                CommFunc.PublicMessageAll("号码太少，无法分割！", true, MessageBoxIcon.Asterisk, "");
            }
            else
            {
                int num2;
                ValueLable lable;
                for (num2 = 1; num2 <= pCount; num2++)
                {
                    List<string> pList = list[num2 - 1];
                    lable = this.ValueLableList[num2 - 1];
                    lable.Hint = $"第{num2}组（{pList.Count}注）";
                    lable.Visible = true;
                    lable.Value = CommFunc.Join(pList, " ");
                    lable.ValueList = CommFunc.CopyList(pList);
                    string pFile = string.Concat(new object[] { this.HJFGPath, @"\", num2, ".txt" });
                    CommFunc.WriteTextFileToStr(pFile, lable.Value);
                    lable.Tag = pFile;
                }
                for (num2 = pCount; num2 < this.ValueLableList.Count; num2++)
                {
                    lable = this.ValueLableList[num2];
                    lable.Visible = false;
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

        private string GetSelectType()
        {
            foreach (RadioButton button in this.TypeList)
            {
                if (button.Checked)
                {
                    return button.Text;
                }
            }
            return "";
        }

        private void HJFGCount_Load(object sender, EventArgs e)
        {
            this.BeautifyInterface();
            this.SetControlInfoByReg();
            List<CheckBox> list = new List<CheckBox> {
                this.Ckb_Copy,
                this.Ckb_Paste,
                this.Ckb_Clear,
                this.Ckb_CodeYZ,
                this.Ckb_SplitValue,
                this.Ckb_OpenPath
            };
            this.CheckBoxList = list;
            CommFunc.SetCheckBoxFormatFlat(this.CheckBoxList);
            CommFunc.CreateDirectory(this.HJFGPath);
            foreach (ValueLable lable in this.ValueLableList)
            {
                lable.Visible = false;
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.Pnl_Main = new Panel();
            this.Pnl_Right = new Panel();
            this.Grp_Value = new GroupBox();
            this.VL_Value10 = new ValueLable();
            this.VL_Value9 = new ValueLable();
            this.VL_Value8 = new ValueLable();
            this.VL_Value7 = new ValueLable();
            this.VL_Value6 = new ValueLable();
            this.VL_Value5 = new ValueLable();
            this.VL_Value4 = new ValueLable();
            this.VL_Value3 = new ValueLable();
            this.VL_Value2 = new ValueLable();
            this.VL_Value1 = new ValueLable();
            this.Pnl_Left = new Panel();
            this.Grp_DT = new GroupBox();
            this.Txt_Number = new RichTextBox();
            this.Pnl_DTTop = new Panel();
            this.Ckb_CodeYZ = new CheckBox();
            this.Txt_CodeYZ = new TextBox();
            this.Lbl_CodeYZ = new Label();
            this.Ckb_Paste = new CheckBox();
            this.Ckb_Clear = new CheckBox();
            this.Ckb_Copy = new CheckBox();
            this.Grp_Type = new GroupBox();
            this.Ckb_OpenPath = new CheckBox();
            this.Ckb_SplitValue = new CheckBox();
            this.Rdb_Type8 = new RadioButton();
            this.Rdb_Type5 = new RadioButton();
            this.Rdb_Type7 = new RadioButton();
            this.Rdb_Type6 = new RadioButton();
            this.Rdb_Type4 = new RadioButton();
            this.Rdb_Type1 = new RadioButton();
            this.Rdb_Type3 = new RadioButton();
            this.Rdb_Type2 = new RadioButton();
            this.Tot_Hint = new ToolTip(this.components);
            this.Pnl_Main.SuspendLayout();
            this.Pnl_Right.SuspendLayout();
            this.Grp_Value.SuspendLayout();
            this.Pnl_Left.SuspendLayout();
            this.Grp_DT.SuspendLayout();
            this.Pnl_DTTop.SuspendLayout();
            this.Grp_Type.SuspendLayout();
            base.SuspendLayout();
            this.Pnl_Main.Controls.Add(this.Pnl_Right);
            this.Pnl_Main.Controls.Add(this.Pnl_Left);
            this.Pnl_Main.Dock = DockStyle.Fill;
            this.Pnl_Main.Location = new Point(0, 0);
            this.Pnl_Main.Name = "Pnl_Main";
            this.Pnl_Main.Size = new Size(0x37f, 0x1f5);
            this.Pnl_Main.TabIndex = 0;
            this.Pnl_Right.Controls.Add(this.Grp_Value);
            this.Pnl_Right.Dock = DockStyle.Fill;
            this.Pnl_Right.Location = new Point(400, 0);
            this.Pnl_Right.Name = "Pnl_Right";
            this.Pnl_Right.Size = new Size(0x1ef, 0x1f5);
            this.Pnl_Right.TabIndex = 1;
            this.Grp_Value.BackColor = Color.Transparent;
            this.Grp_Value.Controls.Add(this.VL_Value10);
            this.Grp_Value.Controls.Add(this.VL_Value9);
            this.Grp_Value.Controls.Add(this.VL_Value8);
            this.Grp_Value.Controls.Add(this.VL_Value7);
            this.Grp_Value.Controls.Add(this.VL_Value6);
            this.Grp_Value.Controls.Add(this.VL_Value5);
            this.Grp_Value.Controls.Add(this.VL_Value4);
            this.Grp_Value.Controls.Add(this.VL_Value3);
            this.Grp_Value.Controls.Add(this.VL_Value2);
            this.Grp_Value.Controls.Add(this.VL_Value1);
            this.Grp_Value.Dock = DockStyle.Fill;
            this.Grp_Value.Location = new Point(0, 0);
            this.Grp_Value.Name = "Grp_Value";
            this.Grp_Value.Size = new Size(0x1ef, 0x1f5);
            this.Grp_Value.TabIndex = 2;
            this.Grp_Value.TabStop = false;
            this.Grp_Value.Text = "分割结果";
            this.VL_Value10.BorderStyle = BorderStyle.FixedSingle;
            this.VL_Value10.Dock = DockStyle.Top;
            this.VL_Value10.Font = new Font("微软雅黑", 11f);
            this.VL_Value10.Hint = "标签";
            this.VL_Value10.Location = new Point(3, 0x10f);
            this.VL_Value10.Margin = new Padding(4, 5, 4, 5);
            this.VL_Value10.Name = "VL_Value10";
            this.VL_Value10.Size = new Size(0x1e9, 0x1c);
            this.VL_Value10.TabIndex = 9;
            this.VL_Value9.BorderStyle = BorderStyle.FixedSingle;
            this.VL_Value9.Dock = DockStyle.Top;
            this.VL_Value9.Font = new Font("微软雅黑", 11f);
            this.VL_Value9.Hint = "标签";
            this.VL_Value9.Location = new Point(3, 0xf3);
            this.VL_Value9.Margin = new Padding(4, 5, 4, 5);
            this.VL_Value9.Name = "VL_Value9";
            this.VL_Value9.Size = new Size(0x1e9, 0x1c);
            this.VL_Value9.TabIndex = 8;
            this.VL_Value8.BorderStyle = BorderStyle.FixedSingle;
            this.VL_Value8.Dock = DockStyle.Top;
            this.VL_Value8.Font = new Font("微软雅黑", 11f);
            this.VL_Value8.Hint = "标签";
            this.VL_Value8.Location = new Point(3, 0xd7);
            this.VL_Value8.Margin = new Padding(4, 5, 4, 5);
            this.VL_Value8.Name = "VL_Value8";
            this.VL_Value8.Size = new Size(0x1e9, 0x1c);
            this.VL_Value8.TabIndex = 7;
            this.VL_Value7.BorderStyle = BorderStyle.FixedSingle;
            this.VL_Value7.Dock = DockStyle.Top;
            this.VL_Value7.Font = new Font("微软雅黑", 11f);
            this.VL_Value7.Hint = "标签";
            this.VL_Value7.Location = new Point(3, 0xbb);
            this.VL_Value7.Margin = new Padding(4, 5, 4, 5);
            this.VL_Value7.Name = "VL_Value7";
            this.VL_Value7.Size = new Size(0x1e9, 0x1c);
            this.VL_Value7.TabIndex = 6;
            this.VL_Value6.BorderStyle = BorderStyle.FixedSingle;
            this.VL_Value6.Dock = DockStyle.Top;
            this.VL_Value6.Font = new Font("微软雅黑", 11f);
            this.VL_Value6.Hint = "标签";
            this.VL_Value6.Location = new Point(3, 0x9f);
            this.VL_Value6.Margin = new Padding(4, 5, 4, 5);
            this.VL_Value6.Name = "VL_Value6";
            this.VL_Value6.Size = new Size(0x1e9, 0x1c);
            this.VL_Value6.TabIndex = 5;
            this.VL_Value5.BorderStyle = BorderStyle.FixedSingle;
            this.VL_Value5.Dock = DockStyle.Top;
            this.VL_Value5.Font = new Font("微软雅黑", 11f);
            this.VL_Value5.Hint = "标签";
            this.VL_Value5.Location = new Point(3, 0x83);
            this.VL_Value5.Margin = new Padding(4, 5, 4, 5);
            this.VL_Value5.Name = "VL_Value5";
            this.VL_Value5.Size = new Size(0x1e9, 0x1c);
            this.VL_Value5.TabIndex = 4;
            this.VL_Value4.BorderStyle = BorderStyle.FixedSingle;
            this.VL_Value4.Dock = DockStyle.Top;
            this.VL_Value4.Font = new Font("微软雅黑", 11f);
            this.VL_Value4.Hint = "标签";
            this.VL_Value4.Location = new Point(3, 0x67);
            this.VL_Value4.Margin = new Padding(4, 5, 4, 5);
            this.VL_Value4.Name = "VL_Value4";
            this.VL_Value4.Size = new Size(0x1e9, 0x1c);
            this.VL_Value4.TabIndex = 3;
            this.VL_Value3.BorderStyle = BorderStyle.FixedSingle;
            this.VL_Value3.Dock = DockStyle.Top;
            this.VL_Value3.Font = new Font("微软雅黑", 11f);
            this.VL_Value3.Hint = "标签";
            this.VL_Value3.Location = new Point(3, 0x4b);
            this.VL_Value3.Margin = new Padding(4, 5, 4, 5);
            this.VL_Value3.Name = "VL_Value3";
            this.VL_Value3.Size = new Size(0x1e9, 0x1c);
            this.VL_Value3.TabIndex = 2;
            this.VL_Value2.BorderStyle = BorderStyle.FixedSingle;
            this.VL_Value2.Dock = DockStyle.Top;
            this.VL_Value2.Font = new Font("微软雅黑", 11f);
            this.VL_Value2.Hint = "标签";
            this.VL_Value2.Location = new Point(3, 0x2f);
            this.VL_Value2.Margin = new Padding(4, 5, 4, 5);
            this.VL_Value2.Name = "VL_Value2";
            this.VL_Value2.Size = new Size(0x1e9, 0x1c);
            this.VL_Value2.TabIndex = 1;
            this.VL_Value1.BorderStyle = BorderStyle.FixedSingle;
            this.VL_Value1.Dock = DockStyle.Top;
            this.VL_Value1.Font = new Font("微软雅黑", 11f);
            this.VL_Value1.Hint = "标签";
            this.VL_Value1.Location = new Point(3, 0x13);
            this.VL_Value1.Margin = new Padding(4, 5, 4, 5);
            this.VL_Value1.Name = "VL_Value1";
            this.VL_Value1.Size = new Size(0x1e9, 0x1c);
            this.VL_Value1.TabIndex = 0;
            this.Pnl_Left.Controls.Add(this.Grp_DT);
            this.Pnl_Left.Controls.Add(this.Grp_Type);
            this.Pnl_Left.Dock = DockStyle.Left;
            this.Pnl_Left.Location = new Point(0, 0);
            this.Pnl_Left.Name = "Pnl_Left";
            this.Pnl_Left.Size = new Size(400, 0x1f5);
            this.Pnl_Left.TabIndex = 0;
            this.Grp_DT.BackColor = Color.Transparent;
            this.Grp_DT.Controls.Add(this.Txt_Number);
            this.Grp_DT.Controls.Add(this.Pnl_DTTop);
            this.Grp_DT.Dock = DockStyle.Fill;
            this.Grp_DT.Location = new Point(0, 0);
            this.Grp_DT.Name = "Grp_DT";
            this.Grp_DT.Size = new Size(400, 0x1a5);
            this.Grp_DT.TabIndex = 0;
            this.Grp_DT.TabStop = false;
            this.Grp_DT.Text = "大底号码";
            this.Txt_Number.BorderStyle = BorderStyle.FixedSingle;
            this.Txt_Number.Dock = DockStyle.Fill;
            this.Txt_Number.Location = new Point(3, 0x36);
            this.Txt_Number.Name = "Txt_Number";
            this.Txt_Number.Size = new Size(0x18a, 0x16c);
            this.Txt_Number.TabIndex = 10;
            this.Pnl_DTTop.BorderStyle = BorderStyle.FixedSingle;
            this.Pnl_DTTop.Controls.Add(this.Ckb_CodeYZ);
            this.Pnl_DTTop.Controls.Add(this.Txt_CodeYZ);
            this.Pnl_DTTop.Controls.Add(this.Lbl_CodeYZ);
            this.Pnl_DTTop.Controls.Add(this.Ckb_Paste);
            this.Pnl_DTTop.Controls.Add(this.Ckb_Clear);
            this.Pnl_DTTop.Controls.Add(this.Ckb_Copy);
            this.Pnl_DTTop.Dock = DockStyle.Top;
            this.Pnl_DTTop.Location = new Point(3, 0x13);
            this.Pnl_DTTop.Name = "Pnl_DTTop";
            this.Pnl_DTTop.Size = new Size(0x18a, 0x23);
            this.Pnl_DTTop.TabIndex = 0;
            this.Ckb_CodeYZ.Appearance = Appearance.Button;
            this.Ckb_CodeYZ.AutoCheck = false;
            this.Ckb_CodeYZ.FlatAppearance.BorderSize = 0;
            this.Ckb_CodeYZ.FlatStyle = FlatStyle.Flat;
            this.Ckb_CodeYZ.Image = Resources.CodeYZ;
            this.Ckb_CodeYZ.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_CodeYZ.Location = new Point(0x133, 5);
            this.Ckb_CodeYZ.Name = "Ckb_CodeYZ";
            this.Ckb_CodeYZ.Size = new Size(80, 0x19);
            this.Ckb_CodeYZ.TabIndex = 0xa1;
            this.Ckb_CodeYZ.Text = "验证中奖";
            this.Ckb_CodeYZ.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_CodeYZ.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_CodeYZ.UseVisualStyleBackColor = true;
            this.Ckb_CodeYZ.Click += new EventHandler(this.Ckb_CodeYZ_Click);
            this.Txt_CodeYZ.Location = new Point(0xf3, 6);
            this.Txt_CodeYZ.Name = "Txt_CodeYZ";
            this.Txt_CodeYZ.Size = new Size(60, 0x17);
            this.Txt_CodeYZ.TabIndex = 160;
            this.Txt_CodeYZ.Text = "123";
            this.Lbl_CodeYZ.AutoSize = true;
            this.Lbl_CodeYZ.Location = new Point(0xc5, 8);
            this.Lbl_CodeYZ.Name = "Lbl_CodeYZ";
            this.Lbl_CodeYZ.Size = new Size(0x2c, 0x11);
            this.Lbl_CodeYZ.TabIndex = 0x89;
            this.Lbl_CodeYZ.Text = "号码：";
            this.Ckb_Paste.Appearance = Appearance.Button;
            this.Ckb_Paste.AutoCheck = false;
            this.Ckb_Paste.FlatAppearance.BorderSize = 0;
            this.Ckb_Paste.FlatStyle = FlatStyle.Flat;
            this.Ckb_Paste.Image = Resources.Paste;
            this.Ckb_Paste.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Paste.Location = new Point(0x47, 4);
            this.Ckb_Paste.Name = "Ckb_Paste";
            this.Ckb_Paste.Size = new Size(60, 0x19);
            this.Ckb_Paste.TabIndex = 0x88;
            this.Ckb_Paste.Text = "粘贴";
            this.Ckb_Paste.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Paste.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Paste.UseVisualStyleBackColor = true;
            this.Ckb_Paste.Click += new EventHandler(this.Ckb_Paste_Click);
            this.Ckb_Clear.Appearance = Appearance.Button;
            this.Ckb_Clear.AutoCheck = false;
            this.Ckb_Clear.FlatAppearance.BorderSize = 0;
            this.Ckb_Clear.FlatStyle = FlatStyle.Flat;
            this.Ckb_Clear.Image = Resources.ClearAll;
            this.Ckb_Clear.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Clear.Location = new Point(0x88, 4);
            this.Ckb_Clear.Name = "Ckb_Clear";
            this.Ckb_Clear.Size = new Size(60, 0x19);
            this.Ckb_Clear.TabIndex = 0x86;
            this.Ckb_Clear.Text = "清空";
            this.Ckb_Clear.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Clear.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Clear.UseVisualStyleBackColor = true;
            this.Ckb_Clear.Click += new EventHandler(this.Ckb_Clear_Click);
            this.Ckb_Copy.Appearance = Appearance.Button;
            this.Ckb_Copy.AutoCheck = false;
            this.Ckb_Copy.FlatAppearance.BorderSize = 0;
            this.Ckb_Copy.FlatStyle = FlatStyle.Flat;
            this.Ckb_Copy.Image = Resources.Copy;
            this.Ckb_Copy.ImageAlign = ContentAlignment.MiddleLeft;
            this.Ckb_Copy.Location = new Point(6, 4);
            this.Ckb_Copy.Name = "Ckb_Copy";
            this.Ckb_Copy.Size = new Size(60, 0x19);
            this.Ckb_Copy.TabIndex = 0x87;
            this.Ckb_Copy.Text = "复制";
            this.Ckb_Copy.TextAlign = ContentAlignment.MiddleRight;
            this.Ckb_Copy.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_Copy.UseVisualStyleBackColor = true;
            this.Ckb_Copy.Click += new EventHandler(this.Ckb_Copy_Click);
            this.Grp_Type.BackColor = Color.Transparent;
            this.Grp_Type.Controls.Add(this.Ckb_OpenPath);
            this.Grp_Type.Controls.Add(this.Ckb_SplitValue);
            this.Grp_Type.Controls.Add(this.Rdb_Type8);
            this.Grp_Type.Controls.Add(this.Rdb_Type5);
            this.Grp_Type.Controls.Add(this.Rdb_Type7);
            this.Grp_Type.Controls.Add(this.Rdb_Type6);
            this.Grp_Type.Controls.Add(this.Rdb_Type4);
            this.Grp_Type.Controls.Add(this.Rdb_Type1);
            this.Grp_Type.Controls.Add(this.Rdb_Type3);
            this.Grp_Type.Controls.Add(this.Rdb_Type2);
            this.Grp_Type.Dock = DockStyle.Bottom;
            this.Grp_Type.Location = new Point(0, 0x1a5);
            this.Grp_Type.Name = "Grp_Type";
            this.Grp_Type.Size = new Size(400, 80);
            this.Grp_Type.TabIndex = 1;
            this.Grp_Type.TabStop = false;
            this.Grp_Type.Text = "分割类型";
            this.Ckb_OpenPath.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Ckb_OpenPath.Appearance = Appearance.Button;
            this.Ckb_OpenPath.AutoCheck = false;
            this.Ckb_OpenPath.FlatAppearance.BorderSize = 0;
            this.Ckb_OpenPath.FlatStyle = FlatStyle.Flat;
            this.Ckb_OpenPath.Image = Resources.Fold;
            this.Ckb_OpenPath.Location = new Point(0x127, 0x2f);
            this.Ckb_OpenPath.Name = "Ckb_OpenPath";
            this.Ckb_OpenPath.Size = new Size(80, 0x19);
            this.Ckb_OpenPath.TabIndex = 0x88;
            this.Ckb_OpenPath.Text = "打开目录";
            this.Ckb_OpenPath.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_OpenPath.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Tot_Hint.SetToolTip(this.Ckb_OpenPath, "打开分割结果所在的目录");
            this.Ckb_OpenPath.UseVisualStyleBackColor = true;
            this.Ckb_OpenPath.Click += new EventHandler(this.Ckb_OpenPath_Click);
            this.Ckb_SplitValue.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Ckb_SplitValue.Appearance = Appearance.Button;
            this.Ckb_SplitValue.AutoCheck = false;
            this.Ckb_SplitValue.FlatAppearance.BorderSize = 0;
            this.Ckb_SplitValue.FlatStyle = FlatStyle.Flat;
            this.Ckb_SplitValue.Image = Resources.Split;
            this.Ckb_SplitValue.Location = new Point(0x127, 0x13);
            this.Ckb_SplitValue.Name = "Ckb_SplitValue";
            this.Ckb_SplitValue.Size = new Size(80, 0x19);
            this.Ckb_SplitValue.TabIndex = 0x87;
            this.Ckb_SplitValue.Text = "开始分割";
            this.Ckb_SplitValue.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_SplitValue.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.Ckb_SplitValue.UseVisualStyleBackColor = true;
            this.Ckb_SplitValue.Click += new EventHandler(this.Ckb_SplitValue_Click);
            this.Rdb_Type8.AutoSize = true;
            this.Rdb_Type8.Location = new Point(220, 0x31);
            this.Rdb_Type8.Name = "Rdb_Type8";
            this.Rdb_Type8.Size = new Size(0x3b, 0x15);
            this.Rdb_Type8.TabIndex = 8;
            this.Rdb_Type8.Text = "10中9";
            this.Rdb_Type8.UseVisualStyleBackColor = true;
            this.Rdb_Type5.AutoSize = true;
            this.Rdb_Type5.Location = new Point(10, 0x31);
            this.Rdb_Type5.Name = "Rdb_Type5";
            this.Rdb_Type5.Size = new Size(0x34, 0x15);
            this.Rdb_Type5.TabIndex = 5;
            this.Rdb_Type5.Text = "7中6";
            this.Rdb_Type5.UseVisualStyleBackColor = true;
            this.Rdb_Type7.AutoSize = true;
            this.Rdb_Type7.Location = new Point(150, 0x31);
            this.Rdb_Type7.Name = "Rdb_Type7";
            this.Rdb_Type7.Size = new Size(0x34, 0x15);
            this.Rdb_Type7.TabIndex = 7;
            this.Rdb_Type7.Text = "9中8";
            this.Rdb_Type7.UseVisualStyleBackColor = true;
            this.Rdb_Type6.AutoSize = true;
            this.Rdb_Type6.Location = new Point(80, 0x31);
            this.Rdb_Type6.Name = "Rdb_Type6";
            this.Rdb_Type6.Size = new Size(0x34, 0x15);
            this.Rdb_Type6.TabIndex = 6;
            this.Rdb_Type6.Text = "8中7";
            this.Rdb_Type6.UseVisualStyleBackColor = true;
            this.Rdb_Type4.AutoSize = true;
            this.Rdb_Type4.Location = new Point(220, 0x16);
            this.Rdb_Type4.Name = "Rdb_Type4";
            this.Rdb_Type4.Size = new Size(0x34, 0x15);
            this.Rdb_Type4.TabIndex = 4;
            this.Rdb_Type4.Text = "6中5";
            this.Rdb_Type4.UseVisualStyleBackColor = true;
            this.Rdb_Type1.AutoSize = true;
            this.Rdb_Type1.Checked = true;
            this.Rdb_Type1.Location = new Point(10, 0x16);
            this.Rdb_Type1.Name = "Rdb_Type1";
            this.Rdb_Type1.Size = new Size(0x34, 0x15);
            this.Rdb_Type1.TabIndex = 1;
            this.Rdb_Type1.TabStop = true;
            this.Rdb_Type1.Text = "3中2";
            this.Rdb_Type1.UseVisualStyleBackColor = true;
            this.Rdb_Type3.AutoSize = true;
            this.Rdb_Type3.Location = new Point(150, 0x16);
            this.Rdb_Type3.Name = "Rdb_Type3";
            this.Rdb_Type3.Size = new Size(0x34, 0x15);
            this.Rdb_Type3.TabIndex = 3;
            this.Rdb_Type3.Text = "5中4";
            this.Rdb_Type3.UseVisualStyleBackColor = true;
            this.Rdb_Type2.AutoSize = true;
            this.Rdb_Type2.Location = new Point(80, 0x16);
            this.Rdb_Type2.Name = "Rdb_Type2";
            this.Rdb_Type2.Size = new Size(0x34, 0x15);
            this.Rdb_Type2.TabIndex = 2;
            this.Rdb_Type2.Text = "4中3";
            this.Rdb_Type2.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(7f, 17f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.Pnl_Main);
            this.Font = new Font("微软雅黑", 9f);
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "HJFGCount";
            base.Size = new Size(0x37f, 0x1f5);
            base.Load += new EventHandler(this.HJFGCount_Load);
            this.Pnl_Main.ResumeLayout(false);
            this.Pnl_Right.ResumeLayout(false);
            this.Grp_Value.ResumeLayout(false);
            this.Pnl_Left.ResumeLayout(false);
            this.Grp_DT.ResumeLayout(false);
            this.Pnl_DTTop.ResumeLayout(false);
            this.Pnl_DTTop.PerformLayout();
            this.Grp_Type.ResumeLayout(false);
            this.Grp_Type.PerformLayout();
            base.ResumeLayout(false);
        }

        public void SaveControlInfoByReg()
        {
            CommFunc.SaveFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SaveSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
        }

        public void SetControlInfoByReg()
        {
            this.RegConfigPath = @"software\TUHAOPLUS\YXZXGJ\DlgConfig\永信在线挂机软件\" + base.Name;
            List<Control> list = new List<Control> {
                this.Txt_CodeYZ,
                this.Txt_Number
            };
            this.ControlList = list;
            this.SpecialControlList = new List<Control>();
            foreach (RadioButton button in this.TypeList)
            {
                this.ControlList.Add(button);
            }
            CommFunc.SetFormUseingInfo(this.ControlList, this.RegConfigPath);
            CommFunc.SetSpecialControlInfo(this.SpecialControlList, this.RegConfigPath);
        }

        private List<List<string>> SplitValueMain(int pCount)
        {
            List<List<string>> list = new List<List<string>>();
            List<string> pList = CommFunc.SplitStringSkipNull(CommFunc.ConvertNumberString(this.Txt_Number.Text, " ", ""), " ");
            this.Txt_Number.Text = CommFunc.Join(pList, " ");
            if (pList.Count >= pCount)
            {
                int num;
                for (num = 0; num < pCount; num++)
                {
                    List<string> item = new List<string>();
                    list.Add(item);
                }
                for (num = 0; num < pList.Count; num++)
                {
                    string str2 = pList[num];
                    list.Sort((pStr1, pStr2) => pStr1.Count - pStr2.Count);
                    for (int i = 0; i < (pCount - 1); i++)
                    {
                        list[i].Add(str2);
                    }
                }
                list.Sort((pStr1, pStr2) => pStr2.Count - pStr1.Count);
            }
            return list;
        }

        public string HJFGPath =>
            (CommFunc.getDllPath() + @"\HJFG\");
    }
}

