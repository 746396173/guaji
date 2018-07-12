namespace IntelligentPlanning.ExDataGridView
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class ShapeTextColumn : DataGridViewColumn
    {
        private Image _defaultImage;
        public Font _font;
        private Image _hotImage;
        private int _imageSize;
        private Image _pressedImage;

        public ShapeTextColumn(Image pdefault, Image pHot, Image pPressed, int pSize)
        {
            this.DefaultImage = pdefault;
            this.HotImage = pHot;
            this.PressedImage = pPressed;
            this._imageSize = pSize;
            this._font = new Font("微软雅黑", 9f);
            this.CellTemplate = new ShapeTextCell();
        }

        [Description("图形的默认外观"), Category("外观")]
        public Image DefaultImage
        {
            get => 
                this._defaultImage;
            set
            {
                this._defaultImage = value;
            }
        }

        [Description("图形被选中的外观"), Category("外观")]
        public Image HotImage
        {
            get => 
                this._hotImage;
            set
            {
                this._hotImage = value;
            }
        }

        [Category("外观"), Description("图形的大小")]
        public int ImageSize
        {
            get => 
                this._imageSize;
            set
            {
                this._imageSize = value;
            }
        }

        [Description("图形按下后的外观"), Category("外观")]
        public Image PressedImage
        {
            get => 
                this._pressedImage;
            set
            {
                this._pressedImage = value;
            }
        }
    }
}

