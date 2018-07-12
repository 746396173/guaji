namespace IntelligentPlanning
{
    using IntelligentPlanning.Properties;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    internal class AppImage
    {
        private static void Btn_MouseDown(object sender, MouseEventArgs e)
        {
            Control control = (Control) sender;
            Bitmap resWithState = GetResWithState(control.Tag.ToString(), ControlState.MouseDown);
            control.BackgroundImage = resWithState;
            control.Invalidate();
        }

        private static void Btn_MouseEnter(object sender, EventArgs e)
        {
            Control control = (Control) sender;
            string name = control.Tag.ToString();
            Bitmap resWithState = GetResWithState(name, ControlState.MouseOver);
            if (control is RadioButton)
            {
                RadioButton button = sender as RadioButton;
                if (button.Checked)
                {
                    resWithState = GetResWithState(name, ControlState.MouseDown);
                }
            }
            control.BackgroundImage = resWithState;
            control.Invalidate();
        }

        private static void Btn_MouseLeave(object sender, EventArgs e)
        {
            Control control = (Control) sender;
            string name = control.Tag.ToString();
            Bitmap resWithState = GetResWithState(name, ControlState.Normal);
            if (control is RadioButton)
            {
                RadioButton button = sender as RadioButton;
                if (button.Checked)
                {
                    resWithState = GetResWithState(name, ControlState.MouseDown);
                }
            }
            control.BackgroundImage = resWithState;
            control.Invalidate();
        }

        private static void Btn_MouseUp(object sender, MouseEventArgs e)
        {
            Control control = (Control) sender;
            string name = control.Tag.ToString();
            Bitmap resWithState = GetResWithState(name, ControlState.Normal);
            if (control is RadioButton)
            {
                RadioButton button = sender as RadioButton;
                if (button.Checked)
                {
                    resWithState = GetResWithState(name, ControlState.MouseDown);
                }
            }
            control.BackgroundImage = resWithState;
            control.Invalidate();
        }

        public static Bitmap GetResAsImage(string pName)
        {
            if ((pName == null) || (pName == ""))
            {
                return null;
            }
            return (Bitmap) Resources.ResourceManager.GetObject(pName);
        }

        public static Bitmap GetResWithState(string name, ControlState pState)
        {
            Bitmap resAsImage = GetResAsImage(name);
            if (resAsImage == null)
            {
                return null;
            }
            int num = 0;
            switch (pState)
            {
                case ControlState.Normal:
                    num = 0;
                    break;

                case ControlState.MouseOver:
                    num = 1;
                    break;

                case ControlState.MouseDown:
                    num = 2;
                    break;

                case ControlState.Disable:
                    num = 3;
                    break;
            }
            int width = resAsImage.Width / 4;
            Rectangle rect = new Rectangle(num * width, 0, width, resAsImage.Height);
            return resAsImage.Clone(rect, resAsImage.PixelFormat);
        }

        public static void LoadImage(List<Control> pImageControlList)
        {
            foreach (Control control in pImageControlList)
            {
                if (control is Button)
                {
                    SetButtonFormatFlat(control);
                }
                if (control is Label)
                {
                    SetLabelFormat(control);
                }
                if (control is RadioButton)
                {
                    SetRadioButtonFormat(control);
                }
                if (!(control is Label))
                {
                    if (control.Tag == null)
                    {
                        break;
                    }
                    control.MouseDown += new MouseEventHandler(AppImage.Btn_MouseDown);
                    control.MouseEnter += new EventHandler(AppImage.Btn_MouseEnter);
                    control.MouseLeave += new EventHandler(AppImage.Btn_MouseLeave);
                    control.MouseUp += new MouseEventHandler(AppImage.Btn_MouseUp);
                    string pName = control.Tag.ToString();
                    control.BackgroundImage = GetResAsImage(pName);
                    control.Invalidate();
                }
            }
        }

        public static void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;
            string name = button.Tag.ToString();
            button.BackgroundImage = button.Checked ? GetResWithState(button.Tag.ToString(), ControlState.MouseDown) : GetResWithState(name, ControlState.Normal);
            button.ForeColor = button.Checked ? AppInfo.yellowBackColor : AppInfo.whiteColor;
        }

        public static void SetButtonFormatFlat(Control pButton)
        {
            Button button = pButton as Button;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.MouseOverBackColor = button.FlatAppearance.MouseDownBackColor = button.FlatAppearance.CheckedBackColor = Color.Transparent;
            button.Font = new Font("微软雅黑", 11f);
            button.ForeColor = AppInfo.whiteColor;
            button.Tag = cBtnImage;
        }

        public static void SetLabelFormat(Control pLabel)
        {
            Label label = pLabel as Label;
            label.ForeColor = AppInfo.whiteColor;
        }

        public static void SetRadioButtonFormat(Control pRadioButton)
        {
            RadioButton button = pRadioButton as RadioButton;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.MouseOverBackColor = button.FlatAppearance.MouseDownBackColor = button.FlatAppearance.CheckedBackColor = Color.Transparent;
            button.Font = new Font("微软雅黑", 11f);
            button.ForeColor = AppInfo.whiteColor;
            button.Tag = cBtnImage;
            button.FlatAppearance.BorderSize = 0;
            button.CheckedChanged += new EventHandler(AppImage.RadioButton_CheckedChanged);
        }

        public static string cBtnImage =>
            "";

        public enum ControlState
        {
            Disable = 4,
            MouseDown = 3,
            MouseOver = 2,
            Normal = 1
        }
    }
}

