namespace IntelligentPlanning
{
    using System;
    using System.Windows.Forms;

    public class WindowWrap : IWin32Window
    {
        private IntPtr m_Handle;

        public WindowWrap(IntPtr handle)
        {
            this.m_Handle = handle;
        }

        public IntPtr Handle =>
            this.m_Handle;
    }
}

