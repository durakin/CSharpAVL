using System;

namespace CSharpAVL
{
    public class Node <T> where T : IComparable
    {
        public int Height { get; set; } 
        public T Data { get; set; }
        public Node <T> Left { get; set; } 
        public Node <T> Right { get; set; } 
  
        public Node(T data) 
        {
            Data = data;
            Height = 1; 
        } 
    }
}
