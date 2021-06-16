using System;

namespace CSharpAVL.App
{
    class Program
    {
        static void Main(string[] args)
        {
            BinarySearchTree<int> binarySearchTree = new();
            binarySearchTree.Add(10);
            binarySearchTree.Add(10);
            binarySearchTree.Add(10);
            binarySearchTree.Add(10);
            binarySearchTree.Add(10);
            binarySearchTree.Add(5);
            binarySearchTree.Add(5);
            binarySearchTree.Add(5);
            binarySearchTree.Add(0);
            binarySearchTree.Add(0);
            binarySearchTree.Add(0);
            binarySearchTree.Add(0);
            binarySearchTree.Add(6);
            binarySearchTree.Add(18);
            binarySearchTree.Add(18);
            binarySearchTree.Add(18);
            binarySearchTree.Add(18);
            binarySearchTree.Add(18);
            binarySearchTree.Add(3);
            binarySearchTree.Add(4);
            binarySearchTree.Add(9);
            binarySearchTree.Add(11);
            binarySearchTree.Add(11);
            binarySearchTree.Add(11);
            binarySearchTree.Add(11);
            binarySearchTree.Add(11);
            binarySearchTree.Add(-1);
            binarySearchTree.Add(1);
            binarySearchTree.Add(1);
            binarySearchTree.Add(1);
            binarySearchTree.Add(1);
            binarySearchTree.Add(2);
            binarySearchTree.Add(2);
            binarySearchTree.Add(2);
            binarySearchTree.Add(2);

            binarySearchTree.PrintTree();

            Console.WriteLine("Infix traverse order:");
            binarySearchTree.TraverseTree(x => Console.Write($"{x} "), BinarySearchTree<int>.TraverseOrder.Infix);
            Console.WriteLine("\nPostfix traverse order:");
            binarySearchTree.TraverseTree(x => Console.Write($"{x} "), BinarySearchTree<int>.TraverseOrder.Postfix);
            Console.WriteLine("\nPrefix traverse order:");
            binarySearchTree.TraverseTree(x => Console.Write($"{x} "));
            Console.WriteLine("\n Enumerator iteration:");
            foreach (var i in binarySearchTree) Console.WriteLine($"{i} ");
            Console.WriteLine("\nRemoving elements (true/false = found value in list");
            Console.WriteLine($"Removing {5} {binarySearchTree.Remove(5)}");
            Console.WriteLine($"Removing {14} {binarySearchTree.Remove(14)}");
            Console.WriteLine($"Removing {1} {binarySearchTree.Remove(1)}");
            Console.WriteLine($"Removing {2} {binarySearchTree.Remove(2)}");
            Console.WriteLine($"Removing {0} {binarySearchTree.Remove(0)}");
            Console.WriteLine($"Removing {-1} {binarySearchTree.Remove(-1)}");
            Console.WriteLine($"Removing {-10} {binarySearchTree.Remove(-10)}");
            Console.WriteLine($"Removing {9} {binarySearchTree.Remove(9)}");
            Console.WriteLine("After removing");
            binarySearchTree.PrintTree();
            Console.WriteLine("\n Enumerator iteration:");
            foreach (var i in binarySearchTree) Console.WriteLine($"{i} ");
            Console.WriteLine("\n Trying to iterate over empty BinarySearchTree:");
            foreach (var i in new BinarySearchTree<string>()) Console.WriteLine($"{i} ");


            
        }
    }
}
