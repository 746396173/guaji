namespace IntelligentPlanning.ExDataGridView
{
    using System;

    public class TreeGridNodeEventBase
    {
        private TreeGridNode _node;

        public TreeGridNodeEventBase(TreeGridNode node)
        {
            this._node = node;
        }

        public TreeGridNode Node =>
            this._node;
    }
}

