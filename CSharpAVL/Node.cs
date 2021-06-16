using System;
using System.Collections;
using System.Collections.Generic;

namespace CSharpAVL
{
    public class Node<T> : IEnumerable<T>
        where T : IComparable
    {
        public int Height { get; set; }

        public T Data { get; set; }

        public Node<T>? Left { get; set; }

        public Node<T>? Right { get; set; }

        public Node(T data)
        {
            Data = data;
            Height = 1;
        }

        public IEnumerable<T> ToIEnumerable()
        {
            if (Left is null && Right is null)
            {
                yield return Data;
            }
            else if (Left is not null && Right is null)
            {
                foreach (var i in Left.ToIEnumerable())
                {
                    yield return i;
                }
                yield return Data;
            }
            else if (Left is null && Right is not null)
            {
                yield return Data;
                foreach (var i in Right.ToIEnumerable())
                {
                    yield return i;
                }
            }
            else if (Left is not null && Right is not null)
            {
                foreach (var i in Left.ToIEnumerable())
                {
                    yield return i;
                }
                yield return Data;
                foreach (var i in Right.ToIEnumerable())
                {
                    yield return i;
                }
            }
        }

        public IEnumerator<T> GetEnumerator() => ToIEnumerable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ToIEnumerable().GetEnumerator();
    }
}
