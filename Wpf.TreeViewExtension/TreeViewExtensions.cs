using System.Windows.Controls;

namespace Lib.Wpf
{
    public static class TreeViewExtensions
    {
        /// <summary>
        /// Selects the System.Windows.Controls.TreeViewItem item in System.Windows.Controls.TreeView
        /// with the specified header.
        /// </summary>
        /// <param name="tree">TreeView control.</param>
        /// <param name="header">The content to select.</param>
        public static void SelectItemFromHeader(this TreeView tree, object header)
        {
            var selector = new TreeViewItemSelector(header);
            tree.Accept(selector);
        }

        private class TreeViewItemSelector : TreeViewItemVisitor<object>
        {
            public TreeViewItemSelector(object header) : base(header) { }

            public override void Visit(TreeViewItem item)
            {
                if (item.Header == this.Context)
                {
                    this.Continue = false;
                    item.IsSelected = true;
                }
            }
        }

        /// <summary>
        /// Accepts the Lib.Wpf.TreeViewExtension.ITreeViewItemVisitor object.
        /// The visitor scans TreeViewItem controls until Visit method of the visitor returns true.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="visitor"></param>
        public static void Accept(this TreeView tree, ITreeViewItemVisitor visitor)
        {
            var generator = tree.ItemContainerGenerator;
            foreach (var item in tree.Items)
            {
                var container = generator.ContainerFromItem(item) as TreeViewItem;
                if (container != null)
                {
                    container.Accept(visitor);
                    if (!visitor.Continue) return;
                }
            }
        }

        /// <summary>
        /// Accepts the Lib.Wpf.TreeViewExtension.ITreeViewItemVisitor object.
        /// The visitor scans this and all TreeViewItem controls under this until Visit method of the visitor returns true.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="visitor"></param>
        /// <returns></returns>
        public static void Accept(this TreeViewItem source, ITreeViewItemVisitor visitor)
        {
            visitor.Visit(source);
            if (!visitor.Continue)
                return;
            else
            {
                var generator = source.ItemContainerGenerator;
                foreach (var item in source.Items)
                {
                    var container = generator.ContainerFromItem(item) as TreeViewItem;
                    if (container != null)
                    {
                        container.Accept(visitor);
                        if (!visitor.Continue) return;
                    }
                }
            }
        }
    }
}
