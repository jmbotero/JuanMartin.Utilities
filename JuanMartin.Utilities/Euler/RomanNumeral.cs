using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuanMartin.Utilities.Euler
{
    public class RomanNumeral
    {
        private string Value;

        private readonly Dictionary<char, int> Denominations = new Dictionary<char, int>() { { 'I', 1 }, { 'V', 2 }, { 'X', 10 }, { 'L', 50 }, { 'C', 100 }, { 'D', 500 }, { 'M', 1000 } };

        public RomanNumeral(string value)
        {
            Value = value;  
        }

        public bool IsValid()
        {
            var valid_charachters = Value.All(c => Denominations.ContainsKey(c));
            // var descending_numerals

            return valid_charachters; 
        }

        public int ToArabic()
        {
            throw new NotImplementedException();
        }
    }
}
