namespace IntelligentPlanning.ExDataGridView
{
    using System;
    using System.ComponentModel;

    public class CollapsingEventArgs : CancelEventArgs
    {
        private TreeGridNode _node;

        private CollapsingEventArgs()
        {
        }

        public CollapsingEventArgs(TreeGridNode node)
        {
            this._node = node;
        }

        public TreeGridNode Node =>
            this._node;
    }
}

