using System;

namespace CSharpAVL
{
    public static class TreeDrawer<T> where T : IComparable
    {
        private const string Cross = " ├─";
        private const string Corner = " └─";
        private const string Vertical = " │ ";
        private const string Space = "   ";

        public static void PrintNode(Node<T> node, string indent)
        {
            if (node == null)
            {
                return;
            }
            Console.WriteLine(node.Data);
            if (node.Left != null) PrintChildNode(node.Left, indent, node.Right == null);
            if (node.Right != null) PrintChildNode(node.Right, indent, true);

            static void PrintChildNode(Node<T> node, string indent, bool isLast)
            {
                Console.Write(indent);

                if (isLast)
                {
                    Console.Write(Corner);
                    indent += Space;
                }
                else
                {
                    Console.Write(Cross);
                    indent += Vertical;
                }

                PrintNode(node, indent);
            }
        }
    }
}
