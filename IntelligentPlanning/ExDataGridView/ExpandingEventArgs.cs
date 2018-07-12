namespace IntelligentPlanning.ExDataGridView
{
    using System;
    using System.ComponentModel;

    public class ExpandingEventArgs : CancelEventArgs
    {
        private TreeGridNode _node;

        private ExpandingEventArgs()
        {
        }

        public ExpandingEventArgs(TreeGridNode node)
        {
            this._node = node;
        }

        public TreeGridNode Node =>
            this._node;
    }
}

