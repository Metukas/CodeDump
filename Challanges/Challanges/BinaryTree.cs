using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenges
{
    class BinaryTree<T>
    {
        public BinaryTreeNode<T> Root;
        public BinaryTree(T rootItem)
        {
            Root = BinaryTreeNode<T>.MakeNewRootNode(rootItem);
        }

        //public BinaryTreeIterator GetIterator()
        //{
        //    return new BinaryTreeIterator(this);
        //}
        public void Add(T item)
        {
        }

        public void Iterate(Action<BinaryTreeNode<T>> action)
        {
            Iterate(Root, action);
        }

        private void Iterate(BinaryTreeNode<T> node, Action<BinaryTreeNode<T>> action)
        {
            if (node == null)
                return;

            action(node);
            Iterate(node.Right, action);
            Iterate(node.Left, action);
        }

        //public class BinaryTreeIterator
        //{
        //    public BinaryTreeNode<T> Current { get; private set; }
        //    BinaryTreeNode<T> root;
        //
        //    public BinaryTreeIterator(BinaryTree<T> tree)
        //    {
        //        Current = tree.Root;
        //        root = tree.Root;
        //    }
        //
        //    public bool MoveNext()
        //    {
        //        if (Current.NextRight != null)
        //        {
        //            Current = Current.NextRight;
        //            return true;
        //        }
        //        if(Current.NextLeft != null)
        //        {
        //            Current = Current.NextLeft;
        //            return true;
        //        }
        //        while(Current.Parent.NextLeft == null)
        //        {
        //            if(Current.Parent == root)
        //            {
        //                return false;
        //            }
        //            Current = Current.Parent;
        //        }
        //        return false;
        //    }
        //
        //}

    }

    

    class BinaryTreeNode<T>
    {
        bool isRoot;
        public T item;
        public BinaryTreeNode<T> Parent;
        public BinaryTreeNode<T> Right; // X
        public BinaryTreeNode<T> Left;  // Y

        public BinaryTreeNode(T item)
        {
            this.item = item;
        }

        private BinaryTreeNode(BinaryTreeNode<T> parent, T item)
        {
            this.item = item;
            this.Parent = parent;
        }

        public static BinaryTreeNode<T> AddNewNode(BinaryTreeNode<T> parent, T item)
        {
            return new BinaryTreeNode<T>(parent, item);
        }

        public static BinaryTreeNode<T> MakeNewRootNode(T item)
        {
            var node =  new BinaryTreeNode<T>(item);
            node.isRoot = true;
            return node;
        }

        public void AddToRight(T item)
        {
            Right = new BinaryTreeNode<T>(item);
        }
        public void AddToLeft(T item)
        {
            Left = new BinaryTreeNode<T>(item);
        }


    }

    class PointBinaryTree
    {
        BinaryTreeNode<Point> Root;

        public PointBinaryTree(Point item)
        {
            this.Root = BinaryTreeNode<Point>.MakeNewRootNode(item);
        }

        public PointBinaryTree() { }

        public void GetPathTree(int maxX, int maxY)
        {
            void AddToTree(ref BinaryTreeNode<Point> next, int nextX, int nextY)
            {
                if (nextX >= maxX || nextY >= maxY)
                    return;

                next = new BinaryTreeNode<Point>(new Point(nextX, nextY));
                AddToTree(ref next.Left, nextX + 1, nextY);
                AddToTree(ref next.Right, nextX, nextY + 1);
            }

            AddToTree(ref Root, 0, 0);
        }

        public BinaryTreeNode<Point> GetNextLeftInLevel(int level)
        {
            BinaryTreeNode<Point> nodeToReturn = this.Root;
            if (level == 0)
                return Root;
            for (int i = 0; i < level; i++)
            {
                nodeToReturn = nodeToReturn.Left;
            }
            return nodeToReturn;
        }

        public void Add(Point item)
        {
            if (Root == null)
            {
                Root = new BinaryTreeNode<Point>(item);
                return;
            }
            var currentNode = Root;
            bool added = false;
            do
            {
                if (item.X > currentNode.item.X && item.Y <= currentNode.item.Y)
                {
                    if (currentNode.Left == null)
                    {
                        var newNode = new BinaryTreeNode<Point>(item);
                        currentNode.Left = newNode;
                        added = true;
                    }
                    else
                    {
                        currentNode = currentNode.Left;
                    }
                }
                else//(item.Y > currentNode.item.Y && item.X <= currentNode.item.X)
                {
                    if (currentNode.Right == null)
                    {
                        var newNode = new BinaryTreeNode<Point>(item);
                        currentNode.Right = newNode;
                        added = true;
                    }
                    else
                    {
                        currentNode = currentNode.Right;
                    }
                }
            } while (!added);
        }


        public void Iterate(Action<BinaryTreeNode<Point>> action)
        {
            Iterate(Root, action);
        }

        private void Iterate(BinaryTreeNode<Point> node, Action<BinaryTreeNode<Point>> action)
        {
            if (node == null)
                return;

            action(node);
            Iterate(node.Left, action);
            Iterate(node.Right, action);
        }

        enum TreeNodePosition
        {
            Root,
            Left,
            Right
        }

        public void PrintTree()
        {
            //Console.Clear();
            var initialPosition = new Point(Console.CursorLeft, Console.CursorTop);
            void Helper(BinaryTreeNode<Point> node, TreeNodePosition pos, int level)
            {
                if (node == null)
                    return;

                switch (pos)
                {
                    case TreeNodePosition.Root:
                        Console.CursorLeft = Console.WindowWidth / 2;
                        Console.WriteLine($"--{node.item}-- [{level}]");
                        break;
                    case TreeNodePosition.Left:
                        Console.WriteLine($"/{node.item} [{level}]");
                        break;
                    case TreeNodePosition.Right:
                        Console.WriteLine($"\\{node.item} [{level}]");
                        break;
                }
                Helper(node.Left, TreeNodePosition.Left, level + 1);
                Helper(node.Right, TreeNodePosition.Right, level + 1);
            }
            Helper(Root, TreeNodePosition.Root, 0);
        }
        static void SetConsoleCursor(Point position)
        {

        }
    }


}
