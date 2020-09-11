using JuanMartin.Kernel.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuanMartin.Utilities.Euler
{
    public class Number : IComparable<Number>
    {
        public int Value { get; set; }
        public string FrequencyMap { get; private set; }
        public List<string> Patterns { get; private set; }
        public List<int> Family { get; private set; }

        public Number(int n)
        {
            Value = n;
            FrequencyMap = UtilityString.GetCharacterFrequencyMap(n.ToString());
            Patterns = UtilityMath.GetNumericPatterns(n);
        }

        public override string ToString()
        {
            return FrequencyMap;
        }

        public int CompareTo(Number obj)
        {
            return Value.CompareTo(obj.Value);
        }
    }
}
