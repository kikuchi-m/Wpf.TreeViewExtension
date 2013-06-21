using System.Windows.Controls;

namespace Lib.Wpf
{
    public interface ITreeViewItemVisitor
    {
        bool Continue { get; set; }
        void Visit(TreeViewItem item);
    }

    public abstract class TreeViewItemVisitor<T> : ITreeViewItemVisitor
    {
        public bool Continue { get; set; }
        public T Context { get; set; }

        public TreeViewItemVisitor() { this.Continue = true; }

        public TreeViewItemVisitor(T context)
            : this()
        {
            this.Context = context;
        }

        public abstract void Visit(TreeViewItem item);
    }
}
