namespace IntelligentPlanning.ExDataGridView
{
    using IntelligentPlanning.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    internal class ImageTextColumn : DataGridViewColumn
    {
        private Image _defaultImage;
        private bool _doubleClick;
        private Image _hotImage;

        [Description("点击按钮时发生"), Category("Custom")]
        public event DataGridViewCellMouseEventHandler ButtonClick;

        public ImageTextColumn()
        {
            this.DefaultImage = Resources.BtnMore1;
            this.HotImage = Resources.BtnMore2;
            this._doubleClick = false;
            this.CellTemplate = new ImageTextCell();
        }

        public void onButtonClick(ImageTextCell sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.ButtonClick != null)
            {
                this.ButtonClick(this, e);
            }
        }

        [Category("外观"), Description("按钮的默认外观")]
        public Image DefaultImage
        {
            get => 
                this._defaultImage;
            set
            {
                this._defaultImage = value;
            }
        }

        [Description("启动双击触发事件"), Category("Custom")]
        public bool EnableDoubleClick
        {
            get => 
                this._doubleClick;
            set
            {
                this._doubleClick = value;
            }
        }

        [Description("按钮选中时候的外观"), Category("外观")]
        public Image HotImage
        {
            get => 
                this._hotImage;
            set
            {
                this._hotImage = value;
            }
        }
    }
}

