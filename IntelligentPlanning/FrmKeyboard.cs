namespace IntelligentPlanning
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FrmKeyboard : Form
    {
        private List<CheckBox> AlphaList = new List<CheckBox>();
        private List<CheckBox> CheckBoxList = new List<CheckBox>();
        private CheckBox Ckb_A;
        private CheckBox Ckb_B;
        private CheckBox Ckb_Back;
        private CheckBox Ckb_C;
        private CheckBox Ckb_Cancel;
        private CheckBox Ckb_D;
        private CheckBox Ckb_DX;
        private CheckBox Ckb_E;
        private CheckBox Ckb_F;
        private CheckBox Ckb_G;
        private CheckBox Ckb_H;
        private CheckBox Ckb_I;
        private CheckBox Ckb_J;
        private CheckBox Ckb_K;
        private CheckBox Ckb_L;
        private CheckBox Ckb_M;
        private CheckBox Ckb_N;
        private CheckBox Ckb_N0;
        private CheckBox Ckb_N1;
        private CheckBox Ckb_N2;
        private CheckBox Ckb_N3;
        private CheckBox Ckb_N4;
        private CheckBox Ckb_N5;
        private CheckBox Ckb_N6;
        private CheckBox Ckb_N7;
        private CheckBox Ckb_N8;
        private CheckBox Ckb_N9;
        private CheckBox Ckb_O;
        private CheckBox Ckb_Ok;
        private CheckBox Ckb_P;
        private CheckBox Ckb_Q;
        private CheckBox Ckb_R;
        private CheckBox Ckb_S;
        private CheckBox Ckb_T;
        private CheckBox Ckb_U;
        private CheckBox Ckb_V;
        private CheckBox Ckb_W;
        private CheckBox Ckb_X;
        private CheckBox Ckb_Y;
        private CheckBox Ckb_Z;
        private IContainer components = null;
        private Color hotActionColor = Color.DarkGray;
        private TextBox InputTextBox = null;
        private UpperLower KeyUpperLower = UpperLower.Lower;
        private Point LocationPoint;
        private Color pressedActionColor = Color.DimGray;

        public FrmKeyboard(int pX, int pY, TextBox pTextBox)
        {
            this.InitializeComponent();
            this.LocationPoint = new Point(pX, pY);
            this.InputTextBox = pTextBox;
        }

        private void CheckBoxList_Click(object sender, EventArgs e)
        {
            string text = (sender as CheckBox).Text;
            this.InputTextBox.Text = this.InputTextBox.Text + text;
        }

        private void Ckb_Back_Click(object sender, EventArgs e)
        {
            string text = this.InputTextBox.Text;
            if (text != "")
            {
                this.InputTextBox.Text = text.Substring(0, text.Length - 1);
            }
        }

        private void Ckb_Cancel_Click(object sender, EventArgs e)
        {
            this.InputTextBox.Text = "";
            base.Close();
        }

        private void Ckb_DX_Click(object sender, EventArgs e)
        {
            foreach (CheckBox box in this.CheckBoxList)
            {
                if (box.Tag.ToString() == "1")
                {
                    string text = box.Text;
                    box.Text = (this.KeyUpperLower == UpperLower.Lower) ? text.ToUpper() : text.ToLower();
                }
            }
            this.KeyUpperLower = (this.KeyUpperLower == UpperLower.Lower) ? UpperLower.Upper : UpperLower.Lower;
        }

        private void Ckb_Ok_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FrmKeyboard_Load(object sender, EventArgs e)
        {
            base.Location = this.LocationPoint;
            this.BackColor = this.pressedActionColor;
            this.InputTextBox.Text = "";
            foreach (Control control in base.Controls)
            {
                if (control is CheckBox)
                {
                    this.CheckBoxList.Add(control as CheckBox);
                }
            }
            foreach (CheckBox box in this.CheckBoxList)
            {
                if (box.Tag.ToString() != "2")
                {
                    box.Click += new EventHandler(this.CheckBoxList_Click);
                }
                box.FlatAppearance.BorderSize = 1;
                box.FlatAppearance.BorderColor = Color.Black;
                box.ForeColor = Color.White;
                box.BackColor = this.pressedActionColor;
                box.FlatAppearance.MouseDownBackColor = box.FlatAppearance.CheckedBackColor = this.pressedActionColor;
                box.FlatAppearance.MouseOverBackColor = this.hotActionColor;
            }
        }

        private void InitializeComponent()
        {
            this.Ckb_Back = new CheckBox();
            this.Ckb_Cancel = new CheckBox();
            this.Ckb_Ok = new CheckBox();
            this.Ckb_DX = new CheckBox();
            this.Ckb_Y = new CheckBox();
            this.Ckb_X = new CheckBox();
            this.Ckb_Z = new CheckBox();
            this.Ckb_V = new CheckBox();
            this.Ckb_U = new CheckBox();
            this.Ckb_T = new CheckBox();
            this.Ckb_S = new CheckBox();
            this.Ckb_R = new CheckBox();
            this.Ckb_Q = new CheckBox();
            this.Ckb_P = new CheckBox();
            this.Ckb_O = new CheckBox();
            this.Ckb_N = new CheckBox();
            this.Ckb_W = new CheckBox();
            this.Ckb_L = new CheckBox();
            this.Ckb_K = new CheckBox();
            this.Ckb_M = new CheckBox();
            this.Ckb_I = new CheckBox();
            this.Ckb_H = new CheckBox();
            this.Ckb_G = new CheckBox();
            this.Ckb_F = new CheckBox();
            this.Ckb_E = new CheckBox();
            this.Ckb_D = new CheckBox();
            this.Ckb_C = new CheckBox();
            this.Ckb_B = new CheckBox();
            this.Ckb_A = new CheckBox();
            this.Ckb_J = new CheckBox();
            this.Ckb_N9 = new CheckBox();
            this.Ckb_N8 = new CheckBox();
            this.Ckb_N7 = new CheckBox();
            this.Ckb_N6 = new CheckBox();
            this.Ckb_N5 = new CheckBox();
            this.Ckb_N4 = new CheckBox();
            this.Ckb_N3 = new CheckBox();
            this.Ckb_N2 = new CheckBox();
            this.Ckb_N1 = new CheckBox();
            this.Ckb_N0 = new CheckBox();
            base.SuspendLayout();
            this.Ckb_Back.Appearance = Appearance.Button;
            this.Ckb_Back.FlatStyle = FlatStyle.Flat;
            this.Ckb_Back.Font = new Font("微软雅黑", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_Back.Location = new Point(0x159, 7);
            this.Ckb_Back.Name = "Ckb_Back";
            this.Ckb_Back.Size = new Size(0x2d, 0x1b);
            this.Ckb_Back.TabIndex = 0x61;
            this.Ckb_Back.Tag = "2";
            this.Ckb_Back.Text = "←";
            this.Ckb_Back.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Back.UseVisualStyleBackColor = true;
            this.Ckb_Back.Click += new EventHandler(this.Ckb_Back_Click);
            this.Ckb_Cancel.Appearance = Appearance.Button;
            this.Ckb_Cancel.FlatStyle = FlatStyle.Flat;
            this.Ckb_Cancel.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_Cancel.Location = new Point(0x159, 0x6a);
            this.Ckb_Cancel.Name = "Ckb_Cancel";
            this.Ckb_Cancel.Size = new Size(0x2d, 0x1b);
            this.Ckb_Cancel.TabIndex = 0x60;
            this.Ckb_Cancel.Tag = "2";
            this.Ckb_Cancel.Text = "取消";
            this.Ckb_Cancel.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Cancel.UseVisualStyleBackColor = true;
            this.Ckb_Cancel.Click += new EventHandler(this.Ckb_Cancel_Click);
            this.Ckb_Ok.Appearance = Appearance.Button;
            this.Ckb_Ok.FlatStyle = FlatStyle.Flat;
            this.Ckb_Ok.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_Ok.Location = new Point(0x126, 0x6a);
            this.Ckb_Ok.Name = "Ckb_Ok";
            this.Ckb_Ok.Size = new Size(0x2d, 0x1b);
            this.Ckb_Ok.TabIndex = 0x5f;
            this.Ckb_Ok.Tag = "2";
            this.Ckb_Ok.Text = "确定";
            this.Ckb_Ok.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Ok.UseVisualStyleBackColor = true;
            this.Ckb_Ok.Click += new EventHandler(this.Ckb_Ok_Click);
            this.Ckb_DX.Appearance = Appearance.Button;
            this.Ckb_DX.FlatStyle = FlatStyle.Flat;
            this.Ckb_DX.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_DX.Location = new Point(5, 0x6a);
            this.Ckb_DX.Name = "Ckb_DX";
            this.Ckb_DX.Size = new Size(0x37, 0x1b);
            this.Ckb_DX.TabIndex = 0x5e;
            this.Ckb_DX.Tag = "2";
            this.Ckb_DX.Text = "大小写";
            this.Ckb_DX.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_DX.UseVisualStyleBackColor = true;
            this.Ckb_DX.Click += new EventHandler(this.Ckb_DX_Click);
            this.Ckb_Y.Appearance = Appearance.Button;
            this.Ckb_Y.FlatStyle = FlatStyle.Flat;
            this.Ckb_Y.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_Y.Location = new Point(0x14f, 0x49);
            this.Ckb_Y.Name = "Ckb_Y";
            this.Ckb_Y.Size = new Size(0x19, 0x1b);
            this.Ckb_Y.TabIndex = 0x5d;
            this.Ckb_Y.Tag = "1";
            this.Ckb_Y.Text = "y";
            this.Ckb_Y.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Y.UseVisualStyleBackColor = true;
            this.Ckb_X.Appearance = Appearance.Button;
            this.Ckb_X.FlatStyle = FlatStyle.Flat;
            this.Ckb_X.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_X.Location = new Point(0x131, 0x49);
            this.Ckb_X.Name = "Ckb_X";
            this.Ckb_X.Size = new Size(0x19, 0x1b);
            this.Ckb_X.TabIndex = 0x5c;
            this.Ckb_X.Tag = "1";
            this.Ckb_X.Text = "x";
            this.Ckb_X.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_X.UseVisualStyleBackColor = true;
            this.Ckb_Z.Appearance = Appearance.Button;
            this.Ckb_Z.FlatStyle = FlatStyle.Flat;
            this.Ckb_Z.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_Z.Location = new Point(0x16d, 0x49);
            this.Ckb_Z.Name = "Ckb_Z";
            this.Ckb_Z.Size = new Size(0x19, 0x1b);
            this.Ckb_Z.TabIndex = 0x5b;
            this.Ckb_Z.Tag = "1";
            this.Ckb_Z.Text = "z";
            this.Ckb_Z.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Z.UseVisualStyleBackColor = true;
            this.Ckb_V.Appearance = Appearance.Button;
            this.Ckb_V.FlatStyle = FlatStyle.Flat;
            this.Ckb_V.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_V.Location = new Point(0xf5, 0x49);
            this.Ckb_V.Name = "Ckb_V";
            this.Ckb_V.Size = new Size(0x19, 0x1b);
            this.Ckb_V.TabIndex = 90;
            this.Ckb_V.Tag = "1";
            this.Ckb_V.Text = "v";
            this.Ckb_V.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_V.UseVisualStyleBackColor = true;
            this.Ckb_U.Appearance = Appearance.Button;
            this.Ckb_U.FlatStyle = FlatStyle.Flat;
            this.Ckb_U.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_U.Location = new Point(0xd7, 0x49);
            this.Ckb_U.Name = "Ckb_U";
            this.Ckb_U.Size = new Size(0x19, 0x1b);
            this.Ckb_U.TabIndex = 0x59;
            this.Ckb_U.Tag = "1";
            this.Ckb_U.Text = "u";
            this.Ckb_U.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_U.UseVisualStyleBackColor = true;
            this.Ckb_T.Appearance = Appearance.Button;
            this.Ckb_T.FlatStyle = FlatStyle.Flat;
            this.Ckb_T.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_T.Location = new Point(0xb9, 0x49);
            this.Ckb_T.Name = "Ckb_T";
            this.Ckb_T.Size = new Size(0x19, 0x1b);
            this.Ckb_T.TabIndex = 0x58;
            this.Ckb_T.Tag = "1";
            this.Ckb_T.Text = "t";
            this.Ckb_T.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_T.UseVisualStyleBackColor = true;
            this.Ckb_S.Appearance = Appearance.Button;
            this.Ckb_S.FlatStyle = FlatStyle.Flat;
            this.Ckb_S.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_S.Location = new Point(0x9b, 0x49);
            this.Ckb_S.Name = "Ckb_S";
            this.Ckb_S.Size = new Size(0x19, 0x1b);
            this.Ckb_S.TabIndex = 0x57;
            this.Ckb_S.Tag = "1";
            this.Ckb_S.Text = "s";
            this.Ckb_S.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_S.UseVisualStyleBackColor = true;
            this.Ckb_R.Appearance = Appearance.Button;
            this.Ckb_R.FlatStyle = FlatStyle.Flat;
            this.Ckb_R.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_R.Location = new Point(0x7d, 0x49);
            this.Ckb_R.Name = "Ckb_R";
            this.Ckb_R.Size = new Size(0x19, 0x1b);
            this.Ckb_R.TabIndex = 0x56;
            this.Ckb_R.Tag = "1";
            this.Ckb_R.Text = "r";
            this.Ckb_R.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_R.UseVisualStyleBackColor = true;
            this.Ckb_Q.Appearance = Appearance.Button;
            this.Ckb_Q.FlatStyle = FlatStyle.Flat;
            this.Ckb_Q.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_Q.Location = new Point(0x5f, 0x49);
            this.Ckb_Q.Name = "Ckb_Q";
            this.Ckb_Q.Size = new Size(0x19, 0x1b);
            this.Ckb_Q.TabIndex = 0x55;
            this.Ckb_Q.Tag = "1";
            this.Ckb_Q.Text = "q";
            this.Ckb_Q.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_Q.UseVisualStyleBackColor = true;
            this.Ckb_P.Appearance = Appearance.Button;
            this.Ckb_P.FlatStyle = FlatStyle.Flat;
            this.Ckb_P.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_P.Location = new Point(0x41, 0x49);
            this.Ckb_P.Name = "Ckb_P";
            this.Ckb_P.Size = new Size(0x19, 0x1b);
            this.Ckb_P.TabIndex = 0x54;
            this.Ckb_P.Tag = "1";
            this.Ckb_P.Text = "p";
            this.Ckb_P.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_P.UseVisualStyleBackColor = true;
            this.Ckb_O.Appearance = Appearance.Button;
            this.Ckb_O.FlatStyle = FlatStyle.Flat;
            this.Ckb_O.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_O.Location = new Point(0x23, 0x49);
            this.Ckb_O.Name = "Ckb_O";
            this.Ckb_O.Size = new Size(0x19, 0x1b);
            this.Ckb_O.TabIndex = 0x53;
            this.Ckb_O.Tag = "1";
            this.Ckb_O.Text = "o";
            this.Ckb_O.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_O.UseVisualStyleBackColor = true;
            this.Ckb_N.Appearance = Appearance.Button;
            this.Ckb_N.FlatStyle = FlatStyle.Flat;
            this.Ckb_N.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_N.Location = new Point(5, 0x49);
            this.Ckb_N.Name = "Ckb_N";
            this.Ckb_N.Size = new Size(0x19, 0x1b);
            this.Ckb_N.TabIndex = 0x52;
            this.Ckb_N.Tag = "1";
            this.Ckb_N.Text = "n";
            this.Ckb_N.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_N.UseVisualStyleBackColor = true;
            this.Ckb_W.Appearance = Appearance.Button;
            this.Ckb_W.FlatStyle = FlatStyle.Flat;
            this.Ckb_W.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_W.Location = new Point(0x113, 0x49);
            this.Ckb_W.Name = "Ckb_W";
            this.Ckb_W.Size = new Size(0x19, 0x1b);
            this.Ckb_W.TabIndex = 0x51;
            this.Ckb_W.Tag = "1";
            this.Ckb_W.Text = "w";
            this.Ckb_W.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_W.UseVisualStyleBackColor = true;
            this.Ckb_L.Appearance = Appearance.Button;
            this.Ckb_L.FlatStyle = FlatStyle.Flat;
            this.Ckb_L.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_L.Location = new Point(0x14f, 40);
            this.Ckb_L.Name = "Ckb_L";
            this.Ckb_L.Size = new Size(0x19, 0x1b);
            this.Ckb_L.TabIndex = 80;
            this.Ckb_L.Tag = "1";
            this.Ckb_L.Text = "l";
            this.Ckb_L.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_L.UseVisualStyleBackColor = true;
            this.Ckb_K.Appearance = Appearance.Button;
            this.Ckb_K.FlatStyle = FlatStyle.Flat;
            this.Ckb_K.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_K.Location = new Point(0x131, 40);
            this.Ckb_K.Name = "Ckb_K";
            this.Ckb_K.Size = new Size(0x19, 0x1b);
            this.Ckb_K.TabIndex = 0x4f;
            this.Ckb_K.Tag = "1";
            this.Ckb_K.Text = "k";
            this.Ckb_K.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_K.UseVisualStyleBackColor = true;
            this.Ckb_M.Appearance = Appearance.Button;
            this.Ckb_M.FlatStyle = FlatStyle.Flat;
            this.Ckb_M.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_M.Location = new Point(0x16d, 40);
            this.Ckb_M.Name = "Ckb_M";
            this.Ckb_M.Size = new Size(0x19, 0x1b);
            this.Ckb_M.TabIndex = 0x4e;
            this.Ckb_M.Tag = "1";
            this.Ckb_M.Text = "m";
            this.Ckb_M.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_M.UseVisualStyleBackColor = true;
            this.Ckb_I.Appearance = Appearance.Button;
            this.Ckb_I.FlatStyle = FlatStyle.Flat;
            this.Ckb_I.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_I.Location = new Point(0xf5, 40);
            this.Ckb_I.Name = "Ckb_I";
            this.Ckb_I.Size = new Size(0x19, 0x1b);
            this.Ckb_I.TabIndex = 0x4d;
            this.Ckb_I.Tag = "1";
            this.Ckb_I.Text = "i";
            this.Ckb_I.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_I.UseVisualStyleBackColor = true;
            this.Ckb_H.Appearance = Appearance.Button;
            this.Ckb_H.FlatStyle = FlatStyle.Flat;
            this.Ckb_H.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_H.Location = new Point(0xd7, 40);
            this.Ckb_H.Name = "Ckb_H";
            this.Ckb_H.Size = new Size(0x19, 0x1b);
            this.Ckb_H.TabIndex = 0x4c;
            this.Ckb_H.Tag = "1";
            this.Ckb_H.Text = "h";
            this.Ckb_H.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_H.UseVisualStyleBackColor = true;
            this.Ckb_G.Appearance = Appearance.Button;
            this.Ckb_G.FlatStyle = FlatStyle.Flat;
            this.Ckb_G.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_G.Location = new Point(0xb9, 40);
            this.Ckb_G.Name = "Ckb_G";
            this.Ckb_G.Size = new Size(0x19, 0x1b);
            this.Ckb_G.TabIndex = 0x4b;
            this.Ckb_G.Tag = "1";
            this.Ckb_G.Text = "g";
            this.Ckb_G.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_G.UseVisualStyleBackColor = true;
            this.Ckb_F.Appearance = Appearance.Button;
            this.Ckb_F.FlatStyle = FlatStyle.Flat;
            this.Ckb_F.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_F.Location = new Point(0x9b, 40);
            this.Ckb_F.Name = "Ckb_F";
            this.Ckb_F.Size = new Size(0x19, 0x1b);
            this.Ckb_F.TabIndex = 0x4a;
            this.Ckb_F.Tag = "1";
            this.Ckb_F.Text = "f";
            this.Ckb_F.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_F.UseVisualStyleBackColor = true;
            this.Ckb_E.Appearance = Appearance.Button;
            this.Ckb_E.FlatStyle = FlatStyle.Flat;
            this.Ckb_E.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_E.Location = new Point(0x7d, 40);
            this.Ckb_E.Name = "Ckb_E";
            this.Ckb_E.Size = new Size(0x19, 0x1b);
            this.Ckb_E.TabIndex = 0x49;
            this.Ckb_E.Tag = "1";
            this.Ckb_E.Text = "e";
            this.Ckb_E.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_E.UseVisualStyleBackColor = true;
            this.Ckb_D.Appearance = Appearance.Button;
            this.Ckb_D.FlatStyle = FlatStyle.Flat;
            this.Ckb_D.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_D.Location = new Point(0x5f, 40);
            this.Ckb_D.Name = "Ckb_D";
            this.Ckb_D.Size = new Size(0x19, 0x1b);
            this.Ckb_D.TabIndex = 0x48;
            this.Ckb_D.Tag = "1";
            this.Ckb_D.Text = "d";
            this.Ckb_D.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_D.UseVisualStyleBackColor = true;
            this.Ckb_C.Appearance = Appearance.Button;
            this.Ckb_C.FlatStyle = FlatStyle.Flat;
            this.Ckb_C.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_C.Location = new Point(0x41, 40);
            this.Ckb_C.Name = "Ckb_C";
            this.Ckb_C.Size = new Size(0x19, 0x1b);
            this.Ckb_C.TabIndex = 0x47;
            this.Ckb_C.Tag = "1";
            this.Ckb_C.Text = "c";
            this.Ckb_C.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_C.UseVisualStyleBackColor = true;
            this.Ckb_B.Appearance = Appearance.Button;
            this.Ckb_B.FlatStyle = FlatStyle.Flat;
            this.Ckb_B.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_B.Location = new Point(0x23, 40);
            this.Ckb_B.Name = "Ckb_B";
            this.Ckb_B.Size = new Size(0x19, 0x1b);
            this.Ckb_B.TabIndex = 70;
            this.Ckb_B.Tag = "1";
            this.Ckb_B.Text = "b";
            this.Ckb_B.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_B.UseVisualStyleBackColor = true;
            this.Ckb_A.Appearance = Appearance.Button;
            this.Ckb_A.FlatStyle = FlatStyle.Flat;
            this.Ckb_A.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_A.Location = new Point(5, 40);
            this.Ckb_A.Name = "Ckb_A";
            this.Ckb_A.Size = new Size(0x19, 0x1b);
            this.Ckb_A.TabIndex = 0x45;
            this.Ckb_A.Tag = "1";
            this.Ckb_A.Text = "a";
            this.Ckb_A.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_A.UseVisualStyleBackColor = true;
            this.Ckb_J.Appearance = Appearance.Button;
            this.Ckb_J.FlatStyle = FlatStyle.Flat;
            this.Ckb_J.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_J.Location = new Point(0x113, 40);
            this.Ckb_J.Name = "Ckb_J";
            this.Ckb_J.Size = new Size(0x19, 0x1b);
            this.Ckb_J.TabIndex = 0x44;
            this.Ckb_J.Tag = "1";
            this.Ckb_J.Text = "j";
            this.Ckb_J.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_J.UseVisualStyleBackColor = true;
            this.Ckb_N9.Appearance = Appearance.Button;
            this.Ckb_N9.FlatStyle = FlatStyle.Flat;
            this.Ckb_N9.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_N9.Location = new Point(0x11d, 7);
            this.Ckb_N9.Name = "Ckb_N9";
            this.Ckb_N9.Size = new Size(0x19, 0x1b);
            this.Ckb_N9.TabIndex = 0x43;
            this.Ckb_N9.Tag = "0";
            this.Ckb_N9.Text = "9";
            this.Ckb_N9.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_N9.UseVisualStyleBackColor = true;
            this.Ckb_N8.Appearance = Appearance.Button;
            this.Ckb_N8.FlatStyle = FlatStyle.Flat;
            this.Ckb_N8.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_N8.Location = new Point(0xff, 7);
            this.Ckb_N8.Name = "Ckb_N8";
            this.Ckb_N8.Size = new Size(0x19, 0x1b);
            this.Ckb_N8.TabIndex = 0x42;
            this.Ckb_N8.Tag = "0";
            this.Ckb_N8.Text = "8";
            this.Ckb_N8.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_N8.UseVisualStyleBackColor = true;
            this.Ckb_N7.Appearance = Appearance.Button;
            this.Ckb_N7.FlatStyle = FlatStyle.Flat;
            this.Ckb_N7.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_N7.Location = new Point(0xe1, 7);
            this.Ckb_N7.Name = "Ckb_N7";
            this.Ckb_N7.Size = new Size(0x19, 0x1b);
            this.Ckb_N7.TabIndex = 0x41;
            this.Ckb_N7.Tag = "0";
            this.Ckb_N7.Text = "7";
            this.Ckb_N7.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_N7.UseVisualStyleBackColor = true;
            this.Ckb_N6.Appearance = Appearance.Button;
            this.Ckb_N6.FlatStyle = FlatStyle.Flat;
            this.Ckb_N6.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_N6.Location = new Point(0xc3, 7);
            this.Ckb_N6.Name = "Ckb_N6";
            this.Ckb_N6.Size = new Size(0x19, 0x1b);
            this.Ckb_N6.TabIndex = 0x40;
            this.Ckb_N6.Tag = "0";
            this.Ckb_N6.Text = "6";
            this.Ckb_N6.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_N6.UseVisualStyleBackColor = true;
            this.Ckb_N5.Appearance = Appearance.Button;
            this.Ckb_N5.FlatStyle = FlatStyle.Flat;
            this.Ckb_N5.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_N5.Location = new Point(0xa5, 7);
            this.Ckb_N5.Name = "Ckb_N5";
            this.Ckb_N5.Size = new Size(0x19, 0x1b);
            this.Ckb_N5.TabIndex = 0x3f;
            this.Ckb_N5.Tag = "0";
            this.Ckb_N5.Text = "5";
            this.Ckb_N5.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_N5.UseVisualStyleBackColor = true;
            this.Ckb_N4.Appearance = Appearance.Button;
            this.Ckb_N4.FlatStyle = FlatStyle.Flat;
            this.Ckb_N4.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_N4.Location = new Point(0x87, 7);
            this.Ckb_N4.Name = "Ckb_N4";
            this.Ckb_N4.Size = new Size(0x19, 0x1b);
            this.Ckb_N4.TabIndex = 0x3e;
            this.Ckb_N4.Tag = "0";
            this.Ckb_N4.Text = "4";
            this.Ckb_N4.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_N4.UseVisualStyleBackColor = true;
            this.Ckb_N3.Appearance = Appearance.Button;
            this.Ckb_N3.FlatStyle = FlatStyle.Flat;
            this.Ckb_N3.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_N3.Location = new Point(0x69, 7);
            this.Ckb_N3.Name = "Ckb_N3";
            this.Ckb_N3.Size = new Size(0x19, 0x1b);
            this.Ckb_N3.TabIndex = 0x3d;
            this.Ckb_N3.Tag = "0";
            this.Ckb_N3.Text = "3";
            this.Ckb_N3.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_N3.UseVisualStyleBackColor = true;
            this.Ckb_N2.Appearance = Appearance.Button;
            this.Ckb_N2.FlatStyle = FlatStyle.Flat;
            this.Ckb_N2.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_N2.Location = new Point(0x4b, 7);
            this.Ckb_N2.Name = "Ckb_N2";
            this.Ckb_N2.Size = new Size(0x19, 0x1b);
            this.Ckb_N2.TabIndex = 60;
            this.Ckb_N2.Tag = "0";
            this.Ckb_N2.Text = "2";
            this.Ckb_N2.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_N2.UseVisualStyleBackColor = true;
            this.Ckb_N1.Appearance = Appearance.Button;
            this.Ckb_N1.FlatStyle = FlatStyle.Flat;
            this.Ckb_N1.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_N1.Location = new Point(0x2d, 7);
            this.Ckb_N1.Name = "Ckb_N1";
            this.Ckb_N1.Size = new Size(0x19, 0x1b);
            this.Ckb_N1.TabIndex = 0x3b;
            this.Ckb_N1.Tag = "0";
            this.Ckb_N1.Text = "1";
            this.Ckb_N1.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_N1.UseVisualStyleBackColor = true;
            this.Ckb_N0.Appearance = Appearance.Button;
            this.Ckb_N0.FlatStyle = FlatStyle.Flat;
            this.Ckb_N0.Font = new Font("微软雅黑", 9f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.Ckb_N0.Location = new Point(0x13b, 7);
            this.Ckb_N0.Name = "Ckb_N0";
            this.Ckb_N0.Size = new Size(0x19, 0x1b);
            this.Ckb_N0.TabIndex = 0x3a;
            this.Ckb_N0.Tag = "0";
            this.Ckb_N0.Text = "0";
            this.Ckb_N0.TextAlign = ContentAlignment.MiddleCenter;
            this.Ckb_N0.UseVisualStyleBackColor = true;
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(0x18d, 0x8d);
            base.ControlBox = false;
            base.Controls.Add(this.Ckb_Back);
            base.Controls.Add(this.Ckb_Cancel);
            base.Controls.Add(this.Ckb_Ok);
            base.Controls.Add(this.Ckb_DX);
            base.Controls.Add(this.Ckb_Y);
            base.Controls.Add(this.Ckb_X);
            base.Controls.Add(this.Ckb_Z);
            base.Controls.Add(this.Ckb_V);
            base.Controls.Add(this.Ckb_U);
            base.Controls.Add(this.Ckb_T);
            base.Controls.Add(this.Ckb_S);
            base.Controls.Add(this.Ckb_R);
            base.Controls.Add(this.Ckb_Q);
            base.Controls.Add(this.Ckb_P);
            base.Controls.Add(this.Ckb_O);
            base.Controls.Add(this.Ckb_N);
            base.Controls.Add(this.Ckb_W);
            base.Controls.Add(this.Ckb_L);
            base.Controls.Add(this.Ckb_K);
            base.Controls.Add(this.Ckb_M);
            base.Controls.Add(this.Ckb_I);
            base.Controls.Add(this.Ckb_H);
            base.Controls.Add(this.Ckb_G);
            base.Controls.Add(this.Ckb_F);
            base.Controls.Add(this.Ckb_E);
            base.Controls.Add(this.Ckb_D);
            base.Controls.Add(this.Ckb_C);
            base.Controls.Add(this.Ckb_B);
            base.Controls.Add(this.Ckb_A);
            base.Controls.Add(this.Ckb_J);
            base.Controls.Add(this.Ckb_N9);
            base.Controls.Add(this.Ckb_N8);
            base.Controls.Add(this.Ckb_N7);
            base.Controls.Add(this.Ckb_N6);
            base.Controls.Add(this.Ckb_N5);
            base.Controls.Add(this.Ckb_N4);
            base.Controls.Add(this.Ckb_N3);
            base.Controls.Add(this.Ckb_N2);
            base.Controls.Add(this.Ckb_N1);
            base.Controls.Add(this.Ckb_N0);
            this.Font = new Font("微软雅黑", 9f);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "FrmKeyboard";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            base.Load += new EventHandler(this.FrmKeyboard_Load);
            base.ResumeLayout(false);
        }

        public enum UpperLower
        {
            Lower,
            Upper
        }
    }
}

