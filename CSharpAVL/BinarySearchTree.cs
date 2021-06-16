using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSharpAVL
{
    public class BinarySearchTree<T> : ICollection<T> where T : IComparable
    {
        private Node<T>? _root;
        public int Count { get; private set; }

        #region IEnumerable implementanion

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            return new BinarySearchTreeEnum(this);
        }
 
        IEnumerator IEnumerable.GetEnumerator() {
            return new BinarySearchTreeEnum(this);
        }

        private class BinarySearchTreeEnum : IEnumerator<T>
        {
            private readonly BinarySearchTree<T> _collection;
            private int _position = -1;
            private Node<T>? _currentNode;
            private List<Node<T>> _path = new();
            private readonly List<Node<T>> _visited = new();
            private T? _current;

            object IEnumerator.Current => Current;

            public BinarySearchTreeEnum(BinarySearchTree<T> collection)
            {
                _collection = collection;
            }
            
            public T Current
            {
                get
                {
                    if (_current != null)
                    {
                        return _current;
                    }
                    throw new InvalidOperationException();
                }
                private set => _current = value;
            }
            

            public bool MoveNext()
            {
                _position++;

                if (_position >= _collection.Count)
                    return false;

                if (_position == 0)
                {
                    _currentNode = MinValueNode(_collection._root, out _path);
                }
                else
                {
                    if (_currentNode.Right != null)
                    {
                        _currentNode = MinValueNode(_currentNode.Right, out var relativePath);
                        _path.AddRange(relativePath);
                    }
                    else
                    {
                        while (_visited.Contains(_currentNode))
                        {
                            _currentNode = _path.Last();
                            _path.Remove(_currentNode);
                        }
                    }
                }
                Current = _currentNode.Data;
                _visited.Add(_currentNode);
                return true;
            }

            public void Reset()
            {
                _position = -1;
                _currentNode = _collection._root;
                _path.Clear();
                _visited.Clear();
            }

            public void Dispose()
            {
            }
        }
        
        
        #endregion

        #region ICollection<T> methods

        bool ICollection<T>.IsReadOnly => false;

        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(array, arrayIndex, Count);
        }

        public void CopyTo(T[] array)
        {
            CopyTo(array, 0, Count);
        }

        private static void CopyTo(T[] array, int arrayIndex, int count)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            
            if (arrayIndex > array.Length || count > array.Length - arrayIndex)
            {
                throw new ArgumentException("Not enough place in array to copy collection there");
            }
        }

        public bool Remove(T data)
        {
            _root = DeleteNode(_root, data, out var result);
            return result;
        }

        public bool Contains(T item)
        {
            FindValue(_root, item, out var result);
            return result;
        }

        public void Clear()
        {
            _root = null;
            Count = 0;
        }

        #endregion

        #region Traverse methods

        public enum TraverseOrder
        {
            Infix,
            Prefix,
            Postfix
        }

        public void TraverseTree(Action<T> action, TraverseOrder order = TraverseOrder.Prefix)
        {
            switch (order)
            {
                case TraverseOrder.Infix:
                    InfixTraverse(_root, action);
                    break;
                case TraverseOrder.Prefix:
                    PrefixTraverse(_root, action);
                    break;
                case TraverseOrder.Postfix:
                    PostfixTraverse(_root, action);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(order), order, null);
            }
        }

        private static void InfixTraverse(Node<T>? node, Action<T> action)
        {
            if (node == null) return;
            if (node.Left != null) InfixTraverse(node.Left, action);
            action(node.Data);
            if (node.Right != null) InfixTraverse(node.Right, action);
        }

        private static void PrefixTraverse(Node<T>? node, Action<T> action)
        {
            if (node == null) return;
            action(node.Data);
            if (node.Left != null) PrefixTraverse(node.Left, action);
            if (node.Right != null) PrefixTraverse(node.Right, action);
        }

        private static void PostfixTraverse(Node<T>? node, Action<T> action)
        {
            if (node == null) return;
            if (node.Left != null) PostfixTraverse(node.Left, action);
            if (node.Right != null) PostfixTraverse(node.Right, action);
            action(node.Data);
        }

        #endregion

        private static int Height(Node<T>? node)
        {
            return node?.Height ?? 0;
        }

        private static Node<T> RightRotate(Node<T> y)
        {
            var x = y.Left;
            var t2 = x.Right;

            x.Right = y;
            y.Left = t2;

            y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;
            x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;

            return x;
        }

        private static Node<T> LeftRotate(Node<T> x)
        {
            var y = x.Right;
            var t2 = y.Left;

            y.Left = x;
            x.Right = t2;

            x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;
            y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;

            return y;
        }

        private static int GetBalance(Node<T>? node)
        {
            return node != null ? Height(node.Left) - Height(node.Right) : 0;
        }
        
        public void Add(T item)
        {
            _root = AddIfNotPresent(_root, item, out _);
        }


        private Node<T> AddIfNotPresent(Node<T>? node, T data, out bool added)
        {
            added = false;

            if (node == null)
            {
                added = true;
                Count++;
                return new Node<T>(data);
            }

            switch (data.CompareTo(node.Data))
            {
                case < 0:
                    node.Left = AddIfNotPresent(node.Left, data, out added);
                    break;
                case > 0:
                    node.Right = AddIfNotPresent(node.Right, data, out added);
                    break;
                default:
                    return node;
            }

            node.Height = 1 + Math.Max(Height(node.Left),
                Height(node.Right));

            var balance = GetBalance(node);
            
            switch (balance)
            {
                case > 1 when data.CompareTo(node.Left.Data) < 0:
                    return RightRotate(node);
                case < -1 when data.CompareTo(node.Right.Data) > 0:
                    return LeftRotate(node);
                case > 1 when data.CompareTo(node.Left.Data) > 0:
                    node.Left = LeftRotate(node.Left);
                    return RightRotate(node);
                case < -1 when data.CompareTo(node.Right.Data) < 0:
                    node.Right = RightRotate(node.Right);
                    return LeftRotate(node);
                default:
                    return node;
            }
        }

        private static Node<T>? MinValueNode(Node<T>? node, out List<Node<T>> path)
        {
            path = new List<Node<T>>();
            if (node == null) return null;
            
            var current = node;
            while (current.Left != null)
            {
                path.Add(current);
                current = current.Left;
            }
            return current;
        }

        private T? FindValue(Node<T>? node, object value, out bool found)
        {
            found = false;
            if (node == null)
            {
                return default;
            }

            switch (node.Data.CompareTo(value))
            {
                case < 0: return FindValue(node.Right, value, out found);
                case > 0: return FindValue(node.Left, value, out found);
                default:
                    found = true;
                    return node.Data;
            }
        }

        private Node<T>? DeleteNode(Node<T>? root, T data, out bool deleted)
        {
            deleted = false;
            
            if (root == null)
                return null;

            switch (data.CompareTo(root.Data))
            {
                case < 0:
                    root.Left = DeleteNode(root.Left, data, out deleted);
                    break;
                case > 0:
                    root.Right = DeleteNode(root.Right, data, out deleted);
                    break;
                default:
                {
                    if (root.Left == null || root.Right == null)
                    {
                        var temp = root.Left ?? root.Right;
                        root = temp;
                        deleted = true;
                        Count--;
                    }
                    else
                    {
                        var temp = MinValueNode(root.Right, out _);
                        root.Data = temp.Data;
                        root.Right = DeleteNode(root.Right, temp.Data, out deleted);
                    }

                    break;
                }
            }

            if (root == null)
                return null;

            root.Height = Math.Max(Height(root.Left),
                Height(root.Right)) + 1;

            var balance = GetBalance(root);

            switch (balance)
            {
                case > 1 when GetBalance(root.Left) >= 0:
                    return RightRotate(root);
                case > 1 when GetBalance(root.Left) < 0:
                    root.Left = LeftRotate(root.Left);
                    return RightRotate(root);
                case < -1 when GetBalance(root.Right) <= 0:
                    return LeftRotate(root);
                case < -1 when GetBalance(root.Right) > 0:
                    root.Right = RightRotate(root.Right);
                    return LeftRotate(root);
                default:
                    return root;
            }
        }


        public void PrintTree()
        {
            TreeDrawer<T>.PrintNode(_root, "");
        }
    }
}