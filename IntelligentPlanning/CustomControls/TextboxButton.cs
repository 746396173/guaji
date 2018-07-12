namespace IntelligentPlanning.CustomControls
{
    using IntelligentPlanning.Properties;
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    internal class TextboxButton : TextBox
    {
        public const int cBtnWidth = 0x19;
        private Button xButton;

        public event EventHandler<EventArgs> ButtonClick;

        public TextboxButton()
        {
            Padding padding = base.Padding;
            base.Padding = new Padding(padding.Left + 100, padding.Top, padding.Right + 100, padding.Bottom);
            this.OnCreateControl();
        }

        private void CusMoustEnter(object sender, EventArgs e)
        {
            this.xButton.Cursor = Cursors.Default;
        }

        public void OnButtonClick(object sender, EventArgs e)
        {
            if (this.ButtonClick != null)
            {
                this.ButtonClick(sender, e);
            }
        }

        protected override void OnCreateControl()
        {
            if (!base.Controls.Contains(this.xButton))
            {
                this.xButton = new Button();
                this.xButton.Width = 0x19;
                this.xButton.Font = new Font("Tahoma", 9f);
                this.xButton.ImageAlign = ContentAlignment.MiddleCenter;
                this.xButton.FlatStyle = FlatStyle.Flat;
                this.xButton.FlatAppearance.BorderSize = 0;
                this.xButton.FlatAppearance.MouseDownBackColor = base.BackColor;
                this.xButton.FlatAppearance.MouseOverBackColor = base.BackColor;
                base.Controls.Add(this.xButton);
                this.xButton.MouseEnter += new EventHandler(this.CusMoustEnter);
                this.xButton.Click += new EventHandler(this.OnButtonClick);
                this.xButton.Top = -1;
                try
                {
                    this.xButton.Image = Resources.BtnMore2;
                }
                catch
                {
                }
                this.xButton.Height = base.Height;
                this.xButton.Left = (base.Width - this.xButton.Width) - 2;
                this.xButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            }
            base.OnCreateControl();
        }

        public Image ButtonImage
        {
            get => 
                this.xButton.Image;
            set
            {
                if (this.xButton.Image != value)
                {
                    this.xButton.Image = value;
                }
            }
        }
    }
}

