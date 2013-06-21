using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Lib.Wpf;
using Lib.Misc;
using System;
using System.Windows.Data;
using System.Linq;

namespace Lib.Wpf
{
    [TestFixture, RequiresSTA]
    public class TreeViewExtensionsTest
    {
        IEnumerable<TreeElement> treeSource;
        List<TreeElement> linearize;
        TreeElement elem001;
        TreeElement elem11;
        TreeElement toSelect;
        TreeTestWindow win;

        [SetUp]
        public void Init()
        {
            SetUpTreeSource();
            win = new TreeTestWindow();
            win.Tree.ItemsSource = treeSource;
            win.Show();
        }

        private void SetUpTreeSource()
        {
            #region generate tree source
            treeSource = new List<TreeElement>
            {
                new TreeElement("elem 0",ElementType.Node)
                {
                    Children = new ObservableCollection<TreeElement>
                    {
                        new TreeElement("elem 00",ElementType.Node)
                        {
                            Children = new ObservableCollection<TreeElement>
                            {
                                new TreeElement("elem 000",ElementType.Leaf),
                                (elem001 = 
                                new TreeElement("elem 001",ElementType.Leaf))
                            }
                        }, 
                        new TreeElement("elem 01",ElementType.Node)
                        {
                            Children = new ObservableCollection<TreeElement>
                            {
                                new TreeElement("elem 010",ElementType.Leaf),
                                new TreeElement("elem 011",ElementType.Leaf)
                            }
                        }
                    }
                },
                new TreeElement("elem 1", ElementType.Node)
                {
                    Children = new ObservableCollection<TreeElement>
                    {
                        new TreeElement("elem 10",ElementType.Leaf),
                        (elem11 =
                        new TreeElement("elem 11",ElementType.Node)
                        {
                            Children = new ObservableCollection<TreeElement>
                            {
                                new TreeElement("elem 110",ElementType.Leaf),
                                new TreeElement("elem 111",ElementType.Leaf)
                            }
                        }), 
                        new TreeElement("elem 12",ElementType.Node)
                        {
                            Children = new ObservableCollection<TreeElement>
                            {
                                new TreeElement("elem 120",ElementType.Leaf),
                                new TreeElement("elem 121",ElementType.Leaf)
                            }
                        }
                    }
                }
            };
            #endregion
            linearize = new List<TreeElement>();
            NewMethod(treeSource);
        }

        private void NewMethod(IEnumerable<TreeElement> children)
        {
            foreach (var elem in children)
            {
                linearize.Add(elem);
                NewMethod(elem.Children);
            }
        }

        [Test]
        public void ShouldSelectSpecifiedHeader()
        {
            Console.WriteLine("Started tree accept test.");
            win.Tree.SelectedItemChanged += SelectedItemChanged;
            try
            {
                toSelect = elem001;
                win.Tree.SelectItemFromHeader(toSelect);
                toSelect = elem11;
                win.Tree.SelectItemFromHeader(toSelect);
            }
            finally
            {
                win.Tree.SelectedItemChanged -= SelectedItemChanged;
            }
            Console.WriteLine("Finished tree accept test.");
        }

        void SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Console.WriteLine("Tree selection changed.");
            var header = e.NewValue as TreeElement;
            Assert.That(header, Is.Not.EqualTo(null));
            Assert.That(header.Name, Is.EqualTo(toSelect.Name));
            Assert.True(header == toSelect);
        }

        [Test]
        public void ShouldCollectAllHeaders()
        {
            var collector = new Collector(new List<TreeElement>());
            
            win.Tree.Accept(collector);

            Assert.That(collector.Context, Has.Count.EqualTo(linearize.Count));

            for (var i = 0; i < linearize.Count; i++)
            {
                Assert.That(collector.Context[i], Is.EqualTo(linearize[i]));
            }
        }

        class Collector : TreeViewItemVisitor<IList<TreeElement>>
        {
            public Collector(IList<TreeElement> list) : base(list) { }
            public override void Visit(TreeViewItem item)
            {
                this.Context.Add(item.Header as TreeElement);
            }
        }
    }

    public class TreeElement
    {
        public string Name { get; set; }
        public ElementType ElementType { get; set; }
        public ObservableCollection<TreeElement> Children { get; set; }

        public TreeElement()
        {
            Children = new ObservableCollection<TreeElement>();
        }
        public TreeElement(string n, ElementType t)
            : this()
        {
            this.Name = n;
            this.ElementType = t;
        }
    }

    public enum ElementType
    {
        Node,
        Leaf
    }
}
