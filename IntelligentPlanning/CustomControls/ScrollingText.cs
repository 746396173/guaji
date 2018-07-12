namespace IntelligentPlanning.CustomControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class ScrollingText : Control
    {
        private Brush backgroundBrush = null;
        private float BeginX = 0f;
        private float BeginY = 0f;
        private Color borderColor = Color.Black;
        private IContainer components = null;
        private IntelligentPlanning.CustomControls.ScrollDirection currentDirection = IntelligentPlanning.CustomControls.ScrollDirection.LeftToRight;
        private Brush foregroundBrush = null;
        private RectangleF lastKnownRect;
        private IntelligentPlanning.CustomControls.ScrollDirection scrollDirection = IntelligentPlanning.CustomControls.ScrollDirection.RightToLeft;
        private System.Windows.Forms.Timer ScrollingTimer;
        private bool scrollOn = true;
        private int scrollPixelDistance = 2;
        private bool showBorder = true;
        private bool stopScrollOnMouseOver = false;
        private IntelligentPlanning.CustomControls.VerticleTextPosition verticleTextPosition = IntelligentPlanning.CustomControls.VerticleTextPosition.Center;
        private string ViewText = "Text";

        public event TextClickEventHandler TextClicked;

        public ScrollingText()
        {
            this.InitializeComponent();
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.ScrollingTimer = new System.Windows.Forms.Timer();
            this.ScrollingTimer.Interval = 0x19;
            this.ScrollingTimer.Enabled = true;
            this.ScrollingTimer.Tick += new EventHandler(this.ScrollingTimer_Tick);
        }

        private void CalcTextPosition(SizeF pStringSize)
        {
            switch (this.scrollDirection)
            {
                case IntelligentPlanning.CustomControls.ScrollDirection.RightToLeft:
                    if (this.BeginX >= (-1f * pStringSize.Width))
                    {
                        this.BeginX -= this.scrollPixelDistance;
                        break;
                    }
                    this.BeginX = base.ClientSize.Width - 1;
                    break;

                case IntelligentPlanning.CustomControls.ScrollDirection.LeftToRight:
                    if (this.BeginX <= base.ClientSize.Width)
                    {
                        this.BeginX += this.scrollPixelDistance;
                        break;
                    }
                    this.BeginX = -1f * pStringSize.Width;
                    break;

                case IntelligentPlanning.CustomControls.ScrollDirection.Bouncing:
                    if (this.currentDirection != IntelligentPlanning.CustomControls.ScrollDirection.RightToLeft)
                    {
                        if (this.currentDirection == IntelligentPlanning.CustomControls.ScrollDirection.LeftToRight)
                        {
                            if (this.BeginX > (base.ClientSize.Width - pStringSize.Width))
                            {
                                this.currentDirection = IntelligentPlanning.CustomControls.ScrollDirection.RightToLeft;
                            }
                            else
                            {
                                this.BeginX += this.scrollPixelDistance;
                            }
                        }
                        break;
                    }
                    if (this.BeginX >= 0f)
                    {
                        this.BeginX -= this.scrollPixelDistance;
                        break;
                    }
                    this.currentDirection = IntelligentPlanning.CustomControls.ScrollDirection.LeftToRight;
                    break;
            }
            switch (this.verticleTextPosition)
            {
                case IntelligentPlanning.CustomControls.VerticleTextPosition.Top:
                    this.BeginY = 2f;
                    break;

                case IntelligentPlanning.CustomControls.VerticleTextPosition.Center:
                    this.BeginY = (base.ClientSize.Height / 2) - (pStringSize.Height / 2f);
                    break;

                case IntelligentPlanning.CustomControls.VerticleTextPosition.Botom:
                    this.BeginY = base.ClientSize.Height - pStringSize.Height;
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.foregroundBrush != null)
                {
                    this.foregroundBrush.Dispose();
                }
                if (this.backgroundBrush != null)
                {
                    this.backgroundBrush.Dispose();
                }
                if (this.ScrollingTimer != null)
                {
                    this.ScrollingTimer.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public void DrawScrollingText(Graphics pGraphics)
        {
            pGraphics.SmoothingMode = SmoothingMode.HighQuality;
            pGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            SizeF pStringSize = pGraphics.MeasureString(this.ViewText, this.Font);
            if (this.scrollOn)
            {
                this.CalcTextPosition(pStringSize);
            }
            if (this.backgroundBrush != null)
            {
                pGraphics.FillRectangle(this.backgroundBrush, 0, 0, base.ClientSize.Width, base.ClientSize.Height);
            }
            else
            {
                pGraphics.Clear(this.BackColor);
            }
            if (this.showBorder)
            {
                using (Pen pen = new Pen(this.borderColor))
                {
                    pGraphics.DrawRectangle(pen, 0, 0, base.ClientSize.Width - 1, base.ClientSize.Height - 1);
                }
            }
            if (this.foregroundBrush == null)
            {
                using (Brush brush = new SolidBrush(this.ForeColor))
                {
                    pGraphics.DrawString(this.ViewText, this.Font, brush, this.BeginX, this.BeginY);
                }
            }
            else
            {
                pGraphics.DrawString(this.ViewText, this.Font, this.foregroundBrush, this.BeginX, this.BeginY);
            }
            this.lastKnownRect = new RectangleF(this.BeginX, this.BeginY, pStringSize.Width, pStringSize.Height);
            this.EnableTextLink(this.lastKnownRect);
        }

        private void EnableTextLink(RectangleF pTextRect)
        {
            Point point = base.PointToClient(Cursor.Position);
            if (pTextRect.Contains((PointF) point))
            {
                if (this.stopScrollOnMouseOver)
                {
                    this.scrollOn = false;
                }
                this.Cursor = Cursors.Hand;
            }
            else
            {
                this.scrollOn = true;
                this.Cursor = Cursors.Default;
            }
        }

        private void InitializeComponent()
        {
            base.Name = "ScrollingText";
            base.Size = new Size(0xd8, 40);
            base.Click += new EventHandler(this.ScrollingText_Click);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.DrawScrollingText(e.Graphics);
            base.OnPaint(e);
        }

        private void OnTextClicked(object sender, EventArgs args)
        {
            if (this.TextClicked != null)
            {
                this.TextClicked(sender, args);
            }
        }

        private void ScrollingText_Click(object sender, EventArgs e)
        {
            if (this.Cursor == Cursors.Hand)
            {
                this.OnTextClicked(this, new EventArgs());
            }
        }

        private void ScrollingTimer_Tick(object sender, EventArgs e)
        {
            this.lastKnownRect.Inflate(10f, 5f);
            RectangleF lastKnownRect = this.lastKnownRect;
            lastKnownRect.X = Math.Max(0f, this.lastKnownRect.X);
            lastKnownRect.Width = Math.Min(this.lastKnownRect.Width + this.lastKnownRect.X, (float) base.Width);
            lastKnownRect.Width = Math.Min(base.Width - this.lastKnownRect.X, lastKnownRect.Width);
            base.Invalidate(new Region(lastKnownRect));
            base.Update();
        }

        [ReadOnly(false)]
        public Brush BackgroundBrush
        {
            get => 
                this.backgroundBrush;
            set
            {
                this.backgroundBrush = value;
            }
        }

        [Description("滚动窗体的边框颜色"), Category("滚动文本"), Browsable(true)]
        public Color BorderColor
        {
            get => 
                this.borderColor;
            set
            {
                this.borderColor = value;
            }
        }

        [Browsable(true), Description("指示是否启动该控件"), Category("行为")]
        public bool Enabled
        {
            get => 
                base.Enabled;
            set
            {
                this.ScrollingTimer.Enabled = value;
                base.Enabled = value;
            }
        }

        [Browsable(false)]
        public Brush ForegroundBrush
        {
            get => 
                this.foregroundBrush;
            set
            {
                this.foregroundBrush = value;
            }
        }

        [Browsable(true), Category("滚动文本"), Description("文本的滚动方向")]
        public IntelligentPlanning.CustomControls.ScrollDirection ScrollDirection
        {
            get => 
                this.scrollDirection;
            set
            {
                this.scrollDirection = value;
            }
        }

        [Category("滚动文本"), Description("显示的滚动文本"), Browsable(true)]
        public string ScrollText
        {
            get => 
                this.ViewText;
            set
            {
                this.ViewText = value;
                base.Invalidate();
                base.Update();
            }
        }

        [Description("是否显示边框"), Browsable(true), Category("滚动文本")]
        public bool ShowBorder
        {
            get => 
                this.showBorder;
            set
            {
                this.showBorder = value;
            }
        }

        [Description("鼠标移动到滚动文本上时是否停止滚动"), Category("滚动文本"), Browsable(true)]
        public bool StopScrollOnMouseOver
        {
            get => 
                this.stopScrollOnMouseOver;
            set
            {
                this.stopScrollOnMouseOver = value;
            }
        }

        [Description("滚动的像素"), Category("滚动文本"), Browsable(true)]
        public int TextScrollDistance
        {
            get => 
                this.scrollPixelDistance;
            set
            {
                this.scrollPixelDistance = value;
            }
        }

        [Browsable(true), Description("滚动的速度"), Category("滚动文本")]
        public int TextScrollSpeed
        {
            get => 
                this.ScrollingTimer.Interval;
            set
            {
                this.ScrollingTimer.Interval = value;
            }
        }

        [Description("文本的垂直位置"), Browsable(true), Category("滚动文本")]
        public IntelligentPlanning.CustomControls.VerticleTextPosition VerticleTextPosition
        {
            get => 
                this.verticleTextPosition;
            set
            {
                this.verticleTextPosition = value;
            }
        }

        public delegate void TextClickEventHandler(object sender, EventArgs args);
    }
}

