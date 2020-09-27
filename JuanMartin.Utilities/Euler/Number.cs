using System;
using System.Collections.Generic;
 
namespace JuanMartin.Utilities.Euler
{
    public class Number : IComparable<Number>
    {
        public int Value { get; set; }
        public Dictionary<Tuple<int, int>, int> Neighbors { get; private set; }
        public bool Visited { get; set; }
        public Number(int n)
        {
            Value = n;
            Neighbors = new Dictionary<Tuple<int, int>, int>();
            Visited = false;
        }
        public override string ToString()
        {
            return Value.ToString();
        }

        public int CompareTo(Number obj)
        {
            return Value.CompareTo(obj.Value);
        }
    }
}
